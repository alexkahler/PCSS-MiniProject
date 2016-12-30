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
            TestBST();
            return;
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
            
        }

        static void TestBST()
        {
            BinaryTree<int, int> bt = new BinaryTree<int, int>();
            Random rand = new Random();
            int min, max, k, v;
            min = max = k = rand.Next(6);
            for (int i = 0; i < 20; i++)
            {
                k = rand.Next(6);
                v = rand.Next(6);
                min = (min < k) ? min : k;
                max = (max > k) ? max : k;
                Console.WriteLine("Adding: " + k + "  " + v + ". Min: " + min + ". Max: " + max);
                bt.Add(k, v);
            }
            Console.WriteLine("Printing tree: " + bt.PrintTree());
            k = rand.Next(6);
            v = rand.Next(6);
            Console.WriteLine("Removing: " + k + " " + v + ". Result: " + bt.Remove(k, v));
            k = rand.Next(6);
            v = rand.Next(6);
            Console.WriteLine("Removing: " + k + " " + v + ". Result: " + bt.Remove(k, v));
            Console.WriteLine("Printing tree: " + bt.PrintTree());
            Console.WriteLine("Is BST?: " + bt.isBST(min, max));
            Console.ReadLine();
        }
    }
}
