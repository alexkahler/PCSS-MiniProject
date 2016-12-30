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
            TCPServer server = new TCPServer(58008);
            //TestBST();
        }

        static void TestBST()
        {
            BinaryTree<int> bt = new BinaryTree<int>();
            Random rand = new Random();
            for (int i = 0; i < 20; i++)
            {
                int r = rand.Next(5);
                Console.WriteLine("Adding: " + r);
                bt.Add(r);
            }
            Console.WriteLine("Printing tree: " + bt.PrintTree());
            Console.ReadLine();
        }
    }
}
