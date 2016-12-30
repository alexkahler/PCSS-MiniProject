using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace OutputClient
{
    class OutputClientProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Output Client started...");
            //Console.WriteLine("Provide IP:");
            //String ip = Console.ReadLine();

            //Console.WriteLine("Provide Port:");
            //int port = Int32.Parse(Console.ReadLine());

            Client client = new Client("127.0.0.1", 58008);
            Console.WriteLine("Press 'R' to reconnect...");
            while (Console.ReadLine().ToLower().Equals("r"))
            {
                new Client("127.0.0.1", 58008);
                Console.WriteLine("Press 'R' to reconnect...");
            }
        }
    }

    class Client
    {
        private TcpClient _client;

        private StreamReader _sReader;
        private StreamWriter _sWriter;

        private Boolean _isConnected;

        public Client(String ipAddress, int portNum)
        {
            _client = new TcpClient();
            _client.Connect(ipAddress, portNum);
            Console.WriteLine("Connected to server...");
            HandleCommunication();
        }

        public void HandleCommunication()
        {
            _sReader = new StreamReader(_client.GetStream(), Encoding.ASCII);
            _sWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);

            _isConnected = true;
            String sData = null;
            while (_isConnected)
            {
                Console.WriteLine("Choose an option: 'BT' > BinaryTree, 'BS' > BubbleSort, 'QS' > QuickSort, 'E' > Exit");
                String command = Console.ReadLine().ToLower();
                switch (command)
                {
                    case "bt":
                        sData = "get BT";
                        break;
                    case "bs":
                        sData = "get BS";
                        break;
                    case "qs":
                        sData = "get QS";
                        break;
                    case "e":
                        _sReader.Close();
                        _sWriter.Close();
                        Console.WriteLine("Connection closed.");
                        return;
                    default:
                        Console.WriteLine("Invalid option...");
                        break;
                }
                if (sData == "get BT" || sData == "get BS" || sData == "get QS")
                {
                    Console.WriteLine("Sending data...");
                    // write data and make sure to flush, or the buffer will continue to 
                    // grow, and your data might not be sent when you want it, and will
                    // only be sent once the buffer is filled.
                    _sWriter.WriteLine(sData);
                    _sWriter.Flush();

                    Console.WriteLine("Receiving data...");
                    Console.WriteLine(_sReader.ReadLine());
                }
            }
        }
    }
}