using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int year = DateTime.Today.Month;
            Console.WriteLine($"{year:d6}");
            //Console.WriteLine(5+10);
        }
    }
}
