using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServerLibrary
{
    
    public class MyServerAPM : MyServer
    {

        public delegate void MultiClientDataTransmissionDelegate(NetworkStream stream);
        public MyServerAPM(IPAddress IP, int port) : base(IP, port)
        {

        }

        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                Stream = tcpClient.GetStream();
                MultiClientDataTransmissionDelegate transmissionDelegate = new MultiClientDataTransmissionDelegate(BeginDataTransmission);
                
                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);
            }
        }

        private void TransmissionCallback(IAsyncResult ar)
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            tcpClient.Close();
        }
        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] wiadomosc;
            byte[] buffer = new byte[1024];
            bool pomoc = false;
            while (true)
            {
                try
                {
                    //int message_size = Stream.Read(buffer, 0, Buffer_size);
                    //Stream.Write(buffer, 0, message_size
                    wiadomosc = new ASCIIEncoding().GetBytes("Podaj nazwe panstwa: ");
                    if (!pomoc) stream.Write(wiadomosc, 0, wiadomosc.Length);
                    Array.Clear(wiadomosc, 0, wiadomosc.Length);
                    //int read_message_size = 0;
                    //while (read_message_size == 0)
                    //{
                    //    read_message_size = Stream.Read(buffer, 0, 1024);
                    //}
                    //read_message_size = 0;
                    //if(Stream.Read(buffer, 0, 1024) == 0)continue;
                    stream.Read(buffer, 0, 1024);
                    string Panstwo = Encoding.ASCII.GetString(buffer);
                    var OnlyLetters = new String(Panstwo.Where(Char.IsLetter).ToArray());
                    if (OnlyLetters == "")
                    {
                        pomoc = true;
                        continue;
                    }
                    if (cities.ContainsKey(OnlyLetters))
                    {
                        wiadomosc = new ASCIIEncoding().GetBytes("\rMiasta z tego panstwa to: " + cities[OnlyLetters] + "\r\n");
                        stream.Write(wiadomosc, 0, wiadomosc.Length);
                        Array.Clear(wiadomosc, 0, wiadomosc.Length);
                        Array.Clear(buffer, 0, buffer.Length);
                        pomoc = false;
                        //Thread.Sleep(100); 
                    }
                    //else if(pomoc != true)
                    else
                    {
                        wiadomosc = new ASCIIEncoding().GetBytes("\rSerwer nie posiada informacji o podanym panstwie! \r\n");
                        stream.Write(wiadomosc, 0, wiadomosc.Length);
                        Array.Clear(wiadomosc, 0, wiadomosc.Length);
                        Array.Clear(buffer, 0, buffer.Length);
                        pomoc = false;
                        //Thread.Sleep(100);
                    }

                }
                catch (IOException e)
                {
                    break;
                }
            }
        }

        public override void Start()
        {
            running = true;
            StartListening();
            //transmission starts within the accept function
            AcceptClient();
        }
    }
}
