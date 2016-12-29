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
            Console.WriteLine("Input Client:");
            //Console.WriteLine("Provide IP:");
            //String ip = Console.ReadLine();

            //Console.WriteLine("Provide Port:");
            //int port = Int32.Parse(Console.ReadLine());

            Client client = new Client("127.0.0.1", 58008);
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
                //Console.Write("&gt; ");

                //input = Console.ReadLine();
                input = "POST Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin tempus lacus vitae odio ullamcorper tempor. " +
                    "Nulla suscipit dolor id libero lacinia, et vulputate arcu varius. Donec varius eleifend ante vel facilisis. " +
                    "Vestibulum ac mauris felis. Nunc libero purus, efficitur et ex a, volutpat scelerisque lectus. Praesent at erat massa. " +
                    "Pellentesque mauris neque, bibendum sit amet hendrerit vitae, lacinia non lectus. Nam lacus turpis, dictum nec diam vel, " +
                    "scelerisque efficitur metus. Mauris volutpat lorem nec felis ornare, sit amet auctor enim congue. Etiam vel ullamcorper odio, " +
                    "sit amet tristique lacus. Aliquam vel lectus mollis, ultricies urna in, congue mi. Donec eget arcu blandit, sollicitudin erat vitae, " +
                    "imperdiet ligula. Duis finibus nunc vitae est sodales convallis. Nam vitae libero et sem venenatis tristique non nec erat. " +
                    "Proin eget risus congue, gravida purus non, posuere elit. " +
                    "Cras ornare arcu et eros maximus viverra.Proin purus nisl, iaculis id arcu ut, eleifend convallis mauris. " +
                    "Vestibulum vel odio ultrices, lobortis dolor nec, bibendum enim.Sed fermentum aliquam nulla. In dapibus ex libero.";
                    
                input = Regex.Replace(input, "[^0-9a-zA-Z\x20]+", "");
                input = input.ToLower();

                // write data and make sure to flush, or the buffer will continue to 
                // grow, and your data might not be sent when you want it, and will
                // only be sent once the buffer is filled.
                _sWriter.WriteLine(input);
                _sWriter.Flush();

                // if you want to receive anything
                // String sDataIncomming = _sReader.ReadLine();
            }
        }
    }
}