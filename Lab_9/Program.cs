using Lab_7;
namespace Lab_9 {

    class Program
    {
        static void Main(string[] args)
        {
            Blue_1.Response r1 = new Blue_1.Response("Name");
            Blue_1.Response r2 = new Blue_1.Response("German", 567);
            Blue_1.Response r3 = new Blue_1.HumanResponse("German", "Pikel", 234);
            Console.WriteLine("Hello, World!!!");
            r2.Print();

            // BlueTXTSerializer ser = new BlueTXTSerializer(); // Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Blue_TXT")
            // ser.SerializeBlue1Response(r3, "Blue_task");
        }
    }
}