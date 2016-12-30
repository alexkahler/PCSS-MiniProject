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
            Console.WriteLine("Provide IP or 'Enter' to use default (localhost):");
            String ip = Console.ReadLine();

            Console.WriteLine("Provide Port or 'Enter' to use default (58008):");
            int port = Int32.Parse(Console.ReadLine() + 0);
            if (ip == "")
            {
                ip = "127.0.0.1";
            }
            if (port == 0)
            {
                port = 58008;
            }
            new Client(ip, port);
            Console.WriteLine("Press 'R' to reconnect...");
            while (Console.ReadLine().ToLower().Equals("r"))
            {
                new Client(ip, port);
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
            Console.WriteLine("Connected to server (" + ipAddress + ":" + portNum + ")...");
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
                        sData = "GET BT";
                        break;
                    case "bs":
                        sData = "GET BS";
                        break;
                    case "qs":
                        sData = "GET QS";
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
                if (command == "bt" || command == "bs" || command == "qs")
                {
                    Console.WriteLine("Sending data...");
                    _sWriter.WriteLine(sData);
                    _sWriter.Flush();

                    Console.WriteLine("Receiving data...");
                    Console.WriteLine(_sReader.ReadLine());
                }
            }
        }
    }
}