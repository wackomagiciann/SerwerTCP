using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServerLibrary
{

    public abstract class  MyServer
    {
        protected static Dictionary<string, string> cities = new Dictionary<string, string>(){
        {"UK", "London, Manchester, Birmingham"},
        {"USA", "Chicago, New York, Washington"},
        {"India", "Mumbai, New Delhi, Pune"},
        {"Poland", "Warsaw, Cracow"}
        };

        #region PolaKlasyAbstrakcyjnej
        IPAddress iPAddress;
        int port;
        int buffer_size = 1024;
        protected bool running;
        TcpListener tcpListener;
        TcpClient tcpClient;
        NetworkStream stream;
        #endregion

        #region TresciWiadomosci
        protected static string StartingMessage = "Witaj na serwerze!\r\n" +
           "Na tym serwerze mozna dowiedzec sie jakie miasta znajduja sie w danym panstwie!\r\n";
        protected static string EndingMessage = "Czy chcesz zakonczyc dzialanie (napisz tak/nie)?\r\n";
        #endregion

        #region WlasciwosciSerwera
        /// <summary>
        /// This property gives access to the IP address of a server instance. Property can't be changed when the Server is running. 
        /// </summary>
        public IPAddress IPAddress { get => iPAddress; set { if (!running) iPAddress = value; else throw new Exception("nie można zmienić adresu IP kiedy serwer jest uruchomiony"); } }
        /// <summary>
        /// This property gives access to the port of a server instance. Property can't be changed when the Server is running. Setting invalid port numbers will cause an exception. 
        /// </summary>
        public int Port
        {
            get => port; set
            {
                int tmp = port;
                if (!running) port = value; else throw new Exception("nie można zmienić portu kiedy serwer jest uruchomiony");
                if (!checkPort())
                {
                    port = tmp;
                    throw new Exception("błędna wartość portu");
                }
            }
        }
        
        /// <summary>
        /// This property gives access to the buffer size of a server instance. Property can't be changed when the Server is running. Setting invalid size numbers will cause an exception. 
        /// </summary>
        public int Buffer_size
        {
            get => buffer_size;
            set
            {
                if (value < 0 || value > 1024 * 1024 * 64) throw new Exception("błędny rozmiar pakietu");
                if (!running) buffer_size = value; else throw new Exception("nie można zmienić rozmiaru pakietu kiedy serwer jest uruchomiony");
            }
        }
        protected TcpListener TcpListener { get => tcpListener; set => tcpListener = value; }
        protected TcpClient TcpClient { get => tcpClient; set => tcpClient = value; }
        protected NetworkStream Stream { get => stream; set => stream = value; }
        #endregion

        #region KonstruktorySerwera
        /// <summary>

        /// A default constructor. It doesn't start the server. Invalid port numbers will thrown an exception.

        /// </summary>

        /// <param name="IP">IP address of the server instance.</param>

        /// <param name="port">Port number of the server instance.</param>
        public MyServer(IPAddress IP, int port)
        {
            running = false;
            IPAddress = IP;
            Port = port;
            if (!checkPort())
            {
                Port = 7000;
                throw new Exception("bledna nazwa portu, wartosc portu ustawiona na 7000");
            }
        }
        #endregion

        #region FunkcjeSerwera

        /// <summary>
        /// This function will return false if Port is set to a value lower than 1024 or higher than 49151.
        /// </summary>
        /// <returns>An information wether the set Port value is valid.</returns>
        protected bool checkPort()
        {
            if (port < 1024 || port > 49151) return false;
            return true;
        }

        /// <summary>
        /// This function starts the listener.
        /// </summary>
        protected void StartListening()
        {
            TcpListener = new TcpListener(IPAddress, Port);
            TcpListener.Start();
        }

        /// <summary>
        /// This function waits for the Client connection.
        /// </summary>
        protected abstract void AcceptClient();

        /// <summary>
        /// This function implements Echo and transmits the data between server and client.
        /// </summary>
        protected abstract void BeginDataTransmission(NetworkStream stream);

        /// <summary>
        /// This function fires off the default server behaviour. It interrupts the program.
        /// </summary>
        public abstract void Start();

        #endregion

        //private static int port = 1024;
       
        /*
        public static void ServerUp()
        {
            int i = 1;
            TcpListener server = new TcpListener(IPAddress.Any, port);
            byte[] buffer = new byte[1024];
            byte[] buffer1 = new byte[1024];
            byte[] message = new ASCIIEncoding().GetBytes(StartingMessage);
            byte[] messageEnd = new ASCIIEncoding().GetBytes(EndingMessage);
            server.Start();
            TcpClient client = server.AcceptTcpClient();

            while (true)
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
                        byte[] message3 = new ASCIIEncoding().GetBytes("\r\n Miasta z tego panstwa to: "+cities[OnlyLetters] + "\r\n");
                        client.GetStream().Write(message3, 0, message3.Length);
                    }
                }
                client.Close();
                break;
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
 //           }
  //          server.Stop();
   //     }*/
    }
}
