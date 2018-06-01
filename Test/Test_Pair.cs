using NNanomsg.Protocols;
using System;
using System.Diagnostics;
using System.Threading;

namespace Test
{
    internal class Test_Pair
    {
        private static byte[] _clientData, _serverData;
        private const string InprocAddress = "inproc://pair_test";
        private const int DataSize = TestConstants.DataSize, BufferSize = 1024 * 4, Iter = TestConstants.Iterations;

        public static void Execute()
        {
            Console.WriteLine("Executing Pair test");

            _clientData = new byte[DataSize];
            _serverData = new byte[DataSize];
            var r = new Random();
            r.NextBytes(_clientData);
            r.NextBytes(_serverData);

            var clientThread = new Thread(
                () =>
                {
                    var req = new PairSocket();
                    req.Connect(InprocAddress);

                    byte[] streamOutput = new byte[BufferSize];
                    while (true)
                    {
                        var sw = Stopwatch.StartNew();
                        for (int i = 0; i < Iter; i++)
                        {
                            var result = req.SendImmediate(_clientData);
                            Trace.Assert(result);
                            int read = 0;
                            using (var stream = req.ReceiveStream())
                                while (stream.Length != stream.Position)
                                    read += stream.Read(streamOutput, 0, streamOutput.Length);
                            Trace.Assert(read == _serverData.Length);
                        }
                        sw.Stop();
                        var secondsPerSend = sw.Elapsed.TotalSeconds / (double)Iter;
                        Console.WriteLine("Pair Time {0} us, {1} per second, {2} mb/s ",
                            (int)(secondsPerSend * 1000d * 1000d),
                            (int)(1d / secondsPerSend),
                            (int)(DataSize * 2d / (1024d * 1024d * secondsPerSend)));
                    }
                });
            clientThread.Start();

            {
                var rep = new PairSocket();
                rep.Bind(InprocAddress);

                byte[] streamOutput = new byte[BufferSize];

                var sw = Stopwatch.StartNew();
                while (sw.Elapsed.TotalSeconds < 10)
                {
                    int read = 0;
                    using (var stream = rep.ReceiveStream())
                        while (stream.Length != stream.Position)
                            read += stream.Read(streamOutput, 0, streamOutput.Length);
                    rep.SendImmediate(_serverData);
                }

                clientThread.Abort();
            }
        }
    }
}