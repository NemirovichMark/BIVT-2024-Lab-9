using System;
using static Lab_7.Green_1;
using static Lab_7.Green_2;
using static Lab_7.Green_4;
using static Lab_7.Green_5;

namespace GreenSerializerJsonTests
{
    class Program
    {
        static void Main()
        {
            var serializer = new Lab_9.GreenJSONSerializer();
            serializer.SelectFolder("TestJsonOutput");

            // 1. Participant100M
            var p100 = new Participant100M("Ivanov", "G1", "Coach");
            p100.Run(11.5);
            serializer.SerializeGreen1Participant(p100, "participant100m");
            var dp100 = serializer.DeserializeGreen1Participant("participant100m");
            Assert(dp100 is Participant100M, "Participant100M type");
            Assert(dp100.Surname == p100.Surname, "Participant100M Surname");
            Assert(Math.Abs(dp100.Result - p100.Result) < 1e-6, "Participant100M Result");

            // 2. Participant500M
            var p500 = new Participant500M("Petrov", "G2", "Coach2");
            p500.Run(85.0);
            serializer.SerializeGreen1Participant(p500, "participant500m");
            var dp500 = serializer.DeserializeGreen1Participant("participant500m");
            Assert(dp500 is Participant500M, "Participant500M type");
            Assert(dp500.Surname == p500.Surname, "Participant500M Surname");
            Assert(Math.Abs(dp500.Result - p500.Result) < 1e-6, "Participant500M Result");

            // 3. Human
            var human = new Human("Petr", "Petrov");
            serializer.SerializeGreen2Human(human, "human");
            var dh = serializer.DeserializeGreen2Human("human");
            Assert(dh.Name == human.Name, "Human Name");
            Assert(dh.Surname == human.Surname, "Human Surname");

            // 4. Green_3.Student
            var st3 = new Lab_7.Green_3.Student("Oleg", "Olegov");
            st3.Exam(4);
            st3.Exam(2);
            serializer.SerializeGreen3Student(st3, "student3");
            var dst3 = serializer.DeserializeGreen3Student("student3");
            Assert(dst3.ID == st3.ID, "Student3 ID");
            Assert(dst3.Name == st3.Name, "Student3 Name");
            Assert(dst3.Surname == st3.Surname, "Student3 Surname");
            for (int i = 0; i < dst3.Marks.Length; i++)
                Assert(dst3.Marks[i] == st3.Marks[i], $"Student3 Mark[{i}]");

            // 5. Discipline without participants
            var lj = new LongJump();
            serializer.SerializeGreen4Discipline(lj, "longjump");
            var dlj = serializer.DeserializeGreen4Discipline("longjump");
            Assert(dlj is LongJump, "LongJump type");
            Assert(dlj.Participants.Length == 0, "LongJump no participants");

            var hj = new HighJump();
            serializer.SerializeGreen4Discipline(hj, "highjump");
            var dhj = serializer.DeserializeGreen4Discipline("highjump");
            Assert(dhj is HighJump, "HighJump type");
            Assert(dhj.Participants.Length == 0, "HighJump no participants");

            // 6. Group types without students
            TestGroup(new Lab_7.Green_5.Group("G1"), "group");
            TestGroup(new EliteGroup("Elite1"), "elitegroup");
            TestGroup(new SpecialGroup("Special1"), "specialgroup");

            Console.WriteLine("All GreenJSONSerializer tests passed.");
        }

        static void TestGroup<T>(T group, string fileName) where T : Lab_7.Green_5.Group
        {
            var serializer = new Lab_9.GreenJSONSerializer();
            serializer.SelectFolder("TestJsonOutput");
            serializer.SerializeGreen5Group(group, fileName);
            var dgroup = serializer.DeserializeGreen5Group<T>(fileName);
            Assert(dgroup.Name == group.Name, $"Group {typeof(T).Name} Name");
        }

        static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FAIL] {message}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[OK] {message}");
                Console.ResetColor();
            }
        }
    }
}