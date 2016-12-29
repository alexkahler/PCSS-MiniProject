using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MiniProject
{
    class Server
    {
        public static string data = null;
        private static byte[] bytes = new Byte[1024];


        public static void StartListening()
        {
            

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 58008);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                Thread inputThread = new Thread(Input);

                while (true)
                {
                    

                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    Console.WriteLine("Text received: {0}", data);

                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress Enter to continue ...");
            Console.Read();
        }

        static void Main(string[] args)
        {
            StartListening();  
        }

        static byte[] Input()
        {
            while (true)
            {
                Console.WriteLine("Waiting for connection ...");
                Socket handler = listener.Accept();
                data = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }
                Console.WriteLine("Text received: {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes(data);
                return msg;
            }
        }

        static void Output(Socket handler, byte[] msg)
        {
            
            handler.Send(msg);
        }
    }
}