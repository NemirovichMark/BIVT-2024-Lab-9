using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;

namespace Lab_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var p = new Purple_1.Participant("aaa", "bbb");
            p.Jump(new int[] { 1, 1, 1, 1, 1, 1, 1 });

            Console.WriteLine(desktopPath);
            var ser = new PurpleTXTSerializer();
            ser.SelectFolder(desktopPath);
            ser.SerializePurple1(p, "example");
            Console.WriteLine("Hello");
        }
    }
}
