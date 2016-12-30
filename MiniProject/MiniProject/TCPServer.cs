using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Server
{
    class TCPServer
    {
        private TcpListener server;
        private Boolean isRunning;
        private List<String> receivedData;

        private Boolean IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                isRunning = value;
            }
        }

        private List<String> ReceivedData
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return receivedData;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                receivedData = value;
            }
        }

        public TCPServer(int port)
        {
            ReceivedData = new List<String>();
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(ip, port);
            server.Start();

            IsRunning = true;
            Console.WriteLine("Waiting for connection...");
            LoopClients();
        }

        public void LoopClients()
        {
            while (IsRunning)
            {
                // wait for client connection

                TcpClient newClient = server.AcceptTcpClient();
                Console.WriteLine("Found client...");

                // client found.
                // create a thread to handle communication
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;

            // set two streams
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);

            Boolean isConnected = true;
            String sData;

            while (isConnected)
            {
                // reads from stream
                try
                {
                    sData = sReader.ReadLine();
                    if (sData == null)
                    {
                        break;
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
                Console.WriteLine("Got data from client: " + sData);
                if (sData.StartsWith("post"))
                {
                    ReceivedData.Add(sData.TrimStart(new char[] { 'p', 'o', 's', 't' }));
                }
                else if (sData.StartsWith("get"))
                {
                    if (ReceivedData.Count == 0)
                    {
                        sWriter.WriteLine("No words available.");
                    }
                    else
                    {
                        string[] splitString = ReceivedData[ReceivedData.Count - 1].Split(new char[] { ' ' });
                        ReceivedData.RemoveAt(ReceivedData.Count - 1);
                        //"GET QS / BT / BS lorem ipsum
                        string[] method = sData.Split(new char[] { ' ' });
                        switch (method[1])
                        {
                            case "BS":
                                {
                                    splitString = BubbleSort.Sort(splitString);
                                    IEnumerable<IGrouping<string, string>> groupCount = splitString.GroupBy(w => w);
                                    String s = "";
                                    foreach (IGrouping<string, string> group in groupCount)
                                    {
                                        s = s + group.Key + " " + group.Count() + ", ";
                                    }
                                    sWriter.WriteLine(s);
                                    break;
                                }
                            case "BT":
                                {
                                    IEnumerable<IGrouping<string, string>> groupCount = splitString.GroupBy(w => w);
                                    BinaryTree<string, int> bt = new BinaryTree<string, int>();
                                    foreach (IGrouping<string, string> group in groupCount)
                                    {
                                        bt.Add(group.Key, group.Count());
                                    }
                                    sWriter.WriteLine(bt.PrintTree(BinaryTree<string, int>.TraversalMethods.Inorder, BinaryTree<string, int>.TraversalDirection.Backwards));
                                    break;
                                }
                            case "QS":
                                {
                                    QuickSort qs = new QuickSort();
                                    Dictionary<string, WordCount> wordCountDict = new Dictionary<string, WordCount>();
                                    for (int i = 0; i < splitString.Length; i++)
                                    {
                                        string temp = splitString[i];
                                        if (wordCountDict.ContainsKey(temp))
                                        {
                                            wordCountDict[temp].Count++;
                                        }
                                        else
                                        {
                                            wordCountDict.Add(temp, new WordCount(temp, 1));
                                        }
                                    }
                                    WordCount[] wordCountList = new WordCount[wordCountDict.Count];

                                    wordCountList = qs.Sort(wordCountDict.Values.ToArray());
                                    string s = "";
                                    for (int i = 0; i < wordCountList.Length; i++)
                                    {
                                        s = s + wordCountList[i].Word + " " + wordCountList[i].Count;
                                        if (i < wordCountList.Length - 1)
                                        {
                                            s = s + ", ";
                                        }
                                    }
                                    sWriter.WriteLine(s);
                                    break;
                                }
                            default:
                                {
                                    sWriter.WriteLine("No sorting method chosen.");
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    sWriter.WriteLine("Wrong header");

                }
                sWriter.Flush();
            }
            Console.WriteLine("Connection closed to client.");

        }

    }
}
