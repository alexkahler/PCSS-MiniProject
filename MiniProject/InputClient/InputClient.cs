using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace InputClient
{
    class InputClientProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input Client started...");
            Console.WriteLine("Provide IP or blank to use default (localhost):");
            String ip = Console.ReadLine();

            Console.WriteLine("Provide Port or blank to use default (58008):");
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
            String input = null;
            while (_isConnected)
            {
                //Console.WriteLine("Enter a sentence...");
                //input = Console.ReadLine();
                input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                    "Aliquam facilisis, tellus eget laoreet tincidunt, erat justo fermentum augue, sed euismod est dolor sed felis. " +
                    "Sed blandit venenatis est. In ante leo, volutpat eget dignissim nec, scelerisque quis justo. " + 
                    "Mauris congue elit ac sapien semper luctus at sed ante. Proin vitae justo at metus efficitur volutpat a ac dolor. " +
                    "Aliquam luctus eleifend elit in blandit. Fusce massa turpis, finibus vitae quam vel, iaculis vestibulum nulla. " + 
                    "Nam turpis neque, commodo a ante ut, volutpat vehicula nulla. Sed posuere elit eget nibh lacinia molestie. " + 
                    "Mauris imperdiet feugiat orci, venenatis bibendum quam varius et. In tincidunt purus ut pulvinar hendrerit. " +
                    "Etiam ultricies maximus ultrices. Etiam at diam quis libero cursus iaculis. Sed lacinia iaculis nunc, ut interdum lorem. " +
                    "Aliquam quis ante nec lectus ultricies sollicitudin. Donec finibus fermentum interdum. " +
                    "In est augue, feugiat convallis tortor egestas, placerat pharetra lorem. " +
                    "In vulputate ipsum ac lectus tincidunt scelerisque. Sed ultricies ex nec sodales ultricies. " + 
                    "Nam ac sodales tellus, vel finibus lacus. Nunc consequat quis turpis sed congue. " +
                    "Sed blandit fermentum magna quis dapibus. Nunc tempor ut urna rhoncus lacinia. " +
                    "Integer elementum id enim sit amet varius. " + 
                    "Praesent sit amet lorem a odio iaculis accumsan sed vel nunc. " +
                    "In quam dui, vestibulum at elit ut, posuere porttitor odio. " +
                    "Sed fringilla ut lorem id imperdiet. Vestibulum vehicula turpis nulla, et pellentesque purus interdum et. " +
                    "Etiam dolor libero, dictum eget iaculis vitae, vehicula sed leo. Sed vestibulum id lorem id pulvinar. " + 
                    "Curabitur maximus dolor eu sem hendrerit, iaculis porta enim sollicitudin. " + 
                    "Etiam ligula elit, semper quis felis porttitor, viverra eleifend nulla. " +
                    "Phasellus sed tortor feugiat, sollicitudin ante eu, semper quam. Duis turpis turpis, rutrum in ex at, euismod finibus turpis. " + 
                    "Mauris facilisis lobortis turpis, id auctor leo auctor in. Sed quis metus ac tortor mattis tristique. " +
                    "Quisque maximus sit amet lorem sit amet luctus. Integer venenatis odio urna, non vestibulum mauris bibendum at. " + 
                    "Curabitur interdum vulputate libero. Phasellus fermentum quam mi, eget rutrum nisi molestie quis. " + 
                    "Suspendisse potenti. Vivamus in leo vel justo egestas ultricies ac et nisi. In hac habitasse platea dictumst.";

                input = Regex.Replace(input, "[^0-9a-zA-Z\x20]+", "");
                input = input.ToLower();
                input = "POST " + input;

                // write data and make sure to flush, or the buffer will continue to 
                // grow, and your data might not be sent when you want it, and will
                // only be sent once the buffer is filled.
                Console.WriteLine("Sending data...");
                _sWriter.WriteLine(input);
                _sWriter.Flush();
                Console.WriteLine("Data sent.");
                Console.WriteLine("Press 'R' to resend...");
                if (!Console.ReadLine().ToLower().Equals("r"))
                {
                    _isConnected = false;
                }
            }
            _sReader.Close();
            _sWriter.Close();
            Console.WriteLine("Connection closed.");
        }
    }
}