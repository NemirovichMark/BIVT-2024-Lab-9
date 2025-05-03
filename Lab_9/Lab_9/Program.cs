using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    class Program
    {
        static void Main(string[] args)
        {
            var purpleJSONSerializer = new PurpleJSONSerializer();
            Console.WriteLine(purpleJSONSerializer.Extension);
        }
    }
}
