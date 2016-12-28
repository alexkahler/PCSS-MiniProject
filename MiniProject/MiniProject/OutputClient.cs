using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions; // to count words

namespace MiniProject
{
    class OutputClient
    {
        public static void WordCount()
        {

            var words = new string[] { "Lol", "Lol", "Lol", "Hej", "hej", "hej" };
            var groups = words.GroupBy(w => w);

            foreach (var group in groups)
                Console.WriteLine("{0} -> {1}", group.Key, group.Count());
                Console.ReadKey();

        }
    }
}

