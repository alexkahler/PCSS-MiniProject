using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Multi-Threaded TCP Server started...");
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
            new TCPServer(ip, port);
            //TestBST();
        }

        static void TestBST()
        {
            BinaryTree<int, string> bt = new BinaryTree<int, string>();
            Random rand = new Random();
            int min, max, r;
            min = max = r = rand.Next(6);
            for (int i = 0; i < 10; i++)
            {
                r = rand.Next(5);
                min = (min < r) ? min : r;
                max = (max > r) ? max : r;
                Console.WriteLine("Adding: " + r + ". Min: " + min + ". Max: " + max);
                bt.Add(r, rand.Next(10) + "");
            }
            Console.WriteLine("Printing tree: " + bt.PrintTree());
            Console.WriteLine("Is BST?: " + bt.isBST(min, max));
            Console.ReadLine();
        }
    }
}
