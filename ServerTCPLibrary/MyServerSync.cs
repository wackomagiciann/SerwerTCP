using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

namespace TCPServerLibrary
{
    public class MyServerSync : MyServer
    {

        byte[] wiadomosc;
        public MyServerSync(IPAddress IP, int port) : base(IP,port)
        {

        }
        protected override void AcceptClient()
        {
            TcpClient = TcpListener.AcceptTcpClient();
            byte[] buffer = new byte[Buffer_size];
            Stream = TcpClient.GetStream();
            wiadomosc = new ASCIIEncoding().GetBytes(StartingMessage);
            Stream.Write(wiadomosc, 0, wiadomosc.Length);
            Array.Clear(wiadomosc, 0, wiadomosc.Length);
            BeginDataTransmission(Stream);
        }

        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            bool pomoc = false;
            while (true)
            {
                try
                {
                    //int message_size = Stream.Read(buffer, 0, Buffer_size);
                    //Stream.Write(buffer, 0, message_size
                    wiadomosc = new ASCIIEncoding().GetBytes("Podaj nazwe panstwa: ");
                    if(!pomoc)stream.Write(wiadomosc, 0, wiadomosc.Length);
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

        
        public static void ServerUp()
        {
            int i = 1;
            TcpListener server = new TcpListener(IPAddress.Any, 3000);
            byte[] buffer = new byte[1024];
            byte[] buffer1 = new byte[1024];
            byte[] message = new ASCIIEncoding().GetBytes(StartingMessage);
            byte[] messageEnd = new ASCIIEncoding().GetBytes(EndingMessage);
            server.Start();
            TcpClient client = server.AcceptTcpClient();

            while (true)
            {
                try
                {
                    if (i == 1)
                    {
                        client.GetStream().Write(message, 0, message.Length);
                        i++;
                    }
                    if (i > 0)
                    {
                        byte[] message2 = new ASCIIEncoding().GetBytes("Podaj nazwe panstwa:\r\n ");
                        client.GetStream().Write(message2, 0, message2.Length);
                        client.GetStream().Read(buffer, 0, 1024);
                        string Panstwo = Encoding.ASCII.GetString(buffer);
                        var OnlyLetters = new String(Panstwo.Where(Char.IsLetter).ToArray());
                        if (cities.ContainsKey(OnlyLetters))
                        {
                            byte[] message3 = new ASCIIEncoding().GetBytes("\r\n Miasta z tego panstwa to: " + cities[OnlyLetters] + "\r\n");
                            client.GetStream().Write(message3, 0, message3.Length);
                        }
                    }
                    //client.Close();
                    //break;
                    /*
                    client.GetStream().Write(messageEnd, 0, messageEnd.Length);
                    client.GetStream().Read(buffer1, 0, 1024);
                    string Decyzja = Encoding.ASCII.GetString(buffer1);
                    var OnlyLetters1 = new String(Decyzja.Where(Char.IsLetter).ToArray());
                    if (OnlyLetters1 == "tak")
                    {
                        client.Close();
                        break;
                    }
                    */
                }
                catch (IOException e)
                {
                    break;
                }
            }
                
                  //server.Stop();
        }
        /// <summary>
        /// Overrided comment.
        /// </summary>
        public override void Start()
        {
            running = true;
            StartListening();
            //transmission starts within the accept function
            AcceptClient();
        }
    }
}
