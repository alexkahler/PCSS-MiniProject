﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace OutputClient
{
    class OutputClientProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Output Client:");
            //Console.WriteLine("Provide IP:");
            //String ip = Console.ReadLine();

            //Console.WriteLine("Provide Port:");
            //int port = Int32.Parse(Console.ReadLine());

            ClientDemo client = new ClientDemo("127.0.0.1", 58008);
        }
    }

    class ClientDemo
    {
        private TcpClient _client;

        private StreamReader _sReader;
        private StreamWriter _sWriter;

        private Boolean _isConnected;

        public ClientDemo(String ipAddress, int portNum)
        {
            _client = new TcpClient();
            _client.Connect(ipAddress, portNum);

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
                //Console.Write("&gt; ");
                sData = "GET BT";

                // write data and make sure to flush, or the buffer will continue to 
                // grow, and your data might not be sent when you want it, and will
                // only be sent once the buffer is filled.
                _sWriter.WriteLine(sData);
                Console.WriteLine(_sReader.ReadLine());
                _sWriter.Flush();
                _sWriter.Flush();

                // if you want to receive anything
                // String sDataIncomming = _sReader.ReadLine();
            }
        }
    }
}