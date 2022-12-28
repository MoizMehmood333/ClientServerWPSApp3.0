using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace WpfAppServer
{

    public class Client
    {
        Socket sck;
        public string ID {
            get;
            private set;

        }

        public IPEndPoint EndPoint {
            get;
             private set;

        }
        public DateTime TimeConnected
        {
            get;
             set;
        }
        public string Message
        {
            get;
             set;
        }

        public Client(Socket accepted) {
            sck = accepted;
            ID = Guid.NewGuid().ToString();
            EndPoint = (IPEndPoint)sck.RemoteEndPoint;
            sck.BeginReceive(new byte[] { 0},0,0,0,CallBack, null);
        }

        void CallBack(IAsyncResult ar) {
            try
            {
                sck.EndReceive(ar);
                byte[] buf = new byte[8192];
                int rec = sck.Receive(buf, buf.Length, 0);
                if (rec < buf.Length) {
                    Array.Resize<byte>(ref buf, rec);
                }

                if (rec!=null)
                {
                    Received(this, buf);
                }

                //loop to read again
                sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, CallBack, null);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Close();

                if (Disconnected != null)
                {
                    Disconnected(this);
                }
            }
        }

        public void Close() {
            sck.Close();
            sck.Dispose();
        }

        public delegate void ClientRecievedHandler(Client sender, byte[] data);
        public delegate void ClientDisconnectedHandler(Client sender);
        public event ClientRecievedHandler Received;
        public event ClientDisconnectedHandler Disconnected;
    }
}
