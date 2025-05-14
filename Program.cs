using System;
using Lab_7;
namespace Lab_9
{
    class Program
    {
        internal static void Main(string[] args)
        {
            BlueTXTSerializer blueTXTSerializer = new BlueTXTSerializer();
            Blue_1 blue_1 = new Blue_1();
            Blue_1.Response response = new Blue_1.Response("lwsea", 102);
            string pathOfFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string prostoPath = Path.Combine(pathOfFile, "ewd.txt");
            blueTXTSerializer.SerializeBlue1Response(response,"ewd.txt");
        }
    }
}