using System.Net;

namespace NNanomsg
{
    public class NanomsgSocket : NanomsgSocketBase, IConnectSocket, IBindSocket, IReceiveSocket, ISendSocket
    {
        public NanomsgSocket(Domain domain, Protocol protocol) : base(domain, protocol)
        {
        }

        #region Connect

        public NanomsgEndpoint Connect(string address)
        {
            return ConnectImpl(address);
        }

        public NanomsgEndpoint Connect(IPAddress address, int port)
        {
            return ConnectImpl(address, port);
        }

        #endregion Connect

        #region Bind

        public NanomsgEndpoint Bind(string address)
        {
            return BindImpl(address);
        }

        #endregion Bind

        #region Send

        public void Send(byte[] buffer)
        {
            SendImpl(buffer);
        }

        public bool SendImmediate(byte[] buffer)
        {
            return SendImmediateImpl(buffer);
        }

        public NanomsgWriteStream CreateSendStream()
        {
            return CreateSendStreamImpl();
        }

        public void SendStream(NanomsgWriteStream stream)
        {
            SendStreamImpl(stream);
        }

        public bool SendStreamImmediate(NanomsgWriteStream stream)
        {
            return SendStreamImmediateImpl(stream);
        }

        #endregion Send

        #region Receive

        public byte[] Receive()
        {
            return ReceiveImpl();
        }

        public byte[] ReceiveImmediate()
        {
            return ReceiveImmediateImpl();
        }

        public NanomsgReadStream ReceiveStream()
        {
            return ReceiveStreamImpl();
        }

        public NanomsgReadStream ReceiveStreamImmediate()
        {
            return ReceiveStreamImmediateImpl();
        }

        #endregion Receive
    }

    public interface IConnectSocket
    {
        NanomsgEndpoint Connect(string address);

        NanomsgEndpoint Connect(IPAddress address, int port);
    }

    public interface IBindSocket
    {
        NanomsgEndpoint Bind(string address);
    }

    public interface IReceiveSocket
    {
        byte[] Receive();

        byte[] ReceiveImmediate();

        NanomsgReadStream ReceiveStream();

        NanomsgReadStream ReceiveStreamImmediate();
    }

    public interface ISendSocket
    {
        void Send(byte[] buffer);

        bool SendImmediate(byte[] buffer);

        NanomsgWriteStream CreateSendStream();

        void SendStream(NanomsgWriteStream stream);

        bool SendStreamImmediate(NanomsgWriteStream stream);
    }
}