using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MiniProject
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
            LoopClients();
        }

        public void LoopClients()
        {
            while (IsRunning)
            {
                // wait for client connection
                TcpClient newClient = server.AcceptTcpClient();

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

            // sets two streams
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested

            Boolean isConnected = true;
            String sData = null;

            while (isConnected)
            {
                // reads from stream
                sData = sReader.ReadLine();
                if (sData.StartsWith("POST"))
                {
                    ReceivedData.Add(sData.TrimStart(new char[] { 'P', 'O', 'S', 'T' }));
                }
                else if (sData.StartsWith("GET"))
                {
                    if (ReceivedData.Count == 0)
                    {
                        sWriter.WriteLine("No data left");
                    }
                    else
                    {
                        string[] splitString = ReceivedData[ReceivedData.Count - 1].Split(new char[] { ' ' });

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
                                        s = group.Key + " " + group.Count() + ", ";
                                    }
                                    sWriter.WriteLine(s);
                                    break;
                                }
                            case "BT":
                                {
                                    BinaryTree<String> bt = new BinaryTree<string>();
                                    foreach (string s in splitString)
                                    {
                                        bt.Add(s);
                                    }
                                    sWriter.WriteLine(bt.PrintTree());
                                    break;
                                }
                            case "QS":
                                {
                                    QuickSort qs = new QuickSort();
                                    List<WordCount> wordCountList = new List<WordCount>();

                                    for (int i = 0; i < splitString.Length; i++)
                                    {
                                        foreach(WordCount wc in wordCountList)
                                        {
                                            if(wc.Word.Equals(splitString[i]))
                                            {
                                                wc.Count++;
                                            }
                                            else
                                            {
                                                wordCountList.Add(new WordCount(splitString[i], 1));
                                            }
                                        }
                                    }
                                    wordCountList = qs.Sort(wordCountList.ToArray()).ToList();
                                    string s = "";
                                    for (int i = 0; i < wordCountList.Count; i++)
                                    {
                                        s = s + wordCountList[i].Word + " " + wordCountList[i].Count;
                                        if (i < wordCountList.Count-1)
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
                sWriter.Close();
            }
        }

    }
}
