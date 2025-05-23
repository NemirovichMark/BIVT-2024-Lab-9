namespace Lab_9;
using Lab_7;


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        GreenJSONSerializer gf = new GreenJSONSerializer();
        Green_2.Student g2 = new Green_2.Student("Vika", "Smirnova");
        gf.SerializeGreen2Human(g2, "serial2");
        Green_3.Student g3_s = new Green_3.Student("Vika", "Smirnova");
        gf.SerializeGreen3Student(g3_s, "serial3_s");
        Green_4.Discipline g4_l = new Green_4.LongJump();
        gf.SerializeGreen4Discipline(g4_l, "serial4_l");
        Green_4.Discipline g4_h = new Green_4.HighJump();
        gf.SerializeGreen4Discipline(g4_h, "serial4_h");
        // Green_5.Group g5 = new Green_5.EliteGroup("asdf");
        // gf.SerializeGreen5Group("serial5");
    }
}
