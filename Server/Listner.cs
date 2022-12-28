using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppServer
{
    public class Listner
    {
        Socket socket;
        public bool Listening
        {
            get;
            private set;
        }
        public int Port
        {
            get;
            private set;
        }

        public Listner(int nPort)
        {
            Port = nPort;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            if (Listening)
            {
                return;
            }
            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port));
            socket.Listen(0);
            socket.BeginAccept(CallBack, null);
            Listening = true;
        }

        public void Stop()
        {
            if (!Listening)
                return;
            socket.Close();
            socket.Dispose();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        void CallBack(IAsyncResult ar)
        {

            try
            {
                Socket s = socket.EndAccept(ar);
                if (SocketAccepted != null)
                {
                    SocketAccepted(s);
                }
                this.socket.BeginAccept(CallBack, null);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        public delegate void SocketAcceptedHandler(Socket e);
        public event SocketAcceptedHandler SocketAccepted;

    }
}
