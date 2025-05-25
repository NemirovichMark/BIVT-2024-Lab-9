using Lab_7;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_5;

namespace Lab_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            program.Test_Purple_4();
        }
        void Test_Purple_1()
        {
            Purple_1.Judge[] judges = new Purple_1.Judge[]
            {
                new Purple_1.Judge("01", new int[] {1, 2, 3, 4}),
                new Purple_1.Judge("02", new int[] {1, 1, 1, 1}),
                new Purple_1.Judge("03", new int[] {1, 2, 2, 4}),
                new Purple_1.Judge("04", new int[] {1, 3, 3, 4}),
            };

            Purple_1.Competition competition = new Purple_1.Competition(judges);

            Purple_1.Participant[] participants =
            {
                new Purple_1.Participant("M", "M"),
                new Purple_1.Participant("R", "R"),
                new Purple_1.Participant("F", "F"),
                new Purple_1.Participant("C", "C")
            };
            participants[0].Jump(new int[] { 1, 2, 3, 4, 5, 6, 6 });

            competition.Add(participants);

            PurpleJSONSerializer serializer = new PurpleJSONSerializer();
            serializer.SelectFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            serializer.SerializePurple1(competition, "test1");

            var d_competition = serializer.DeserializePurple1<Purple_1.Competition>("test1");


            /*
            PurpleTXTSerializer purpleTXTSerializer = new PurpleTXTSerializer();
            purpleTXTSerializer.SelectFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            purpleTXTSerializer.SerializePurple1(competition, "testing");

            purpleTXTSerializer.SerializePurple1(participants[0], "testing_participant");
            var d_participant = purpleTXTSerializer.DeserializePurple1<Purple_1.Participant>("testing_participant");
            d_participant.Print();

            
            Purple_1.Competition d_competition = purpleTXTSerializer.DeserializePurple1<Purple_1.Competition>("testing");

            */
            foreach (var judge in d_competition.Judges)
                judge.Print();
            foreach(var participant in d_competition.Participants)
                participant.Print();
            
        }

        void Test_Purple_2()
        {
            var competitions = new Purple_2.SkiJumping[10]
            {
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.ProSkiJumping(),
                new Purple_2.ProSkiJumping(),
                new Purple_2.ProSkiJumping(),
                new Purple_2.ProSkiJumping(),
                new Purple_2.ProSkiJumping()
            };
            for (int i = 0; i < competitions.Length; i++)
            {
                var participants = new Purple_2.Participant[10]
                {
                new Purple_2.Participant("Vasya", "Petrovich"),
                new Purple_2.Participant("Petya", "Nikolayevich"),
                new Purple_2.Participant("Kolya", "Vadimovich"),
                new Purple_2.Participant("Vadim", "Maratovich"),
                new Purple_2.Participant("Marat", "Danilovich"),
                new Purple_2.Participant("Danil", "Romanovich"),
                new Purple_2.Participant("Roma", "Egorovich"),
                new Purple_2.Participant("Egor", "Vasiliyevich"),
                new Purple_2.Participant("Masha", "Nikolayevna"),
                new Purple_2.Participant("Dasha", "Vadimovna")
                };
                var p = participants.Take(i).ToArray();
                int[] jumps = new int[5];
                foreach (var item in p)
                {
                    for (int j = 0; j < jumps.Length; j++)
                        jumps[j] = 11;
                    competitions[i].Add(item);
                    competitions[i].Jump(200, jumps);
                }
            }


            PurpleXMLSerializer serializer = new PurpleXMLSerializer();
            serializer.SelectFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            

            for(int i = 0;  i < competitions.Length; i++)
            {
                serializer.SerializePurple2SkiJumping(competitions[i], $"tes{i}");
                var d_item = serializer.DeserializePurple2SkiJumping<Purple_2.SkiJumping>($"tes{i}");
                Console.WriteLine($"{d_item.GetType()} ");
            }

            



            /*
            PurpleTXTSerializer serializer = new PurpleTXTSerializer();
            serializer.SelectFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            serializer.SerializePurple2SkiJumping(junior, "testing_ski");
            */


            
        }

        void Test_Purple_3()
        {

            Purple_3.Participant participant1 = new Purple_3.Participant("Виктор", "Полевой");
            participant1.Evaluate(5.93);
            participant1.Evaluate(5.44);
            participant1.Evaluate(1.20);
            participant1.Evaluate(0.28);
            participant1.Evaluate(1.57);
            participant1.Evaluate(1.86);
            participant1.Evaluate(5.89);

            Purple_3.Participant participant2 = new Purple_3.Participant("Татьяна", "Сидорова");
            participant2.Evaluate(3.86);
            participant2.Evaluate(0.19);
            participant2.Evaluate(0.46);
            participant2.Evaluate(5.14);
            participant2.Evaluate(5.37);
            participant2.Evaluate(0.94);
            participant2.Evaluate(0.84);

            Purple_3.Participant participant3 = new Purple_3.Participant("Ярослав", "Зайцев");
            participant3.Evaluate(2.93);
            participant3.Evaluate(3.10);
            participant3.Evaluate(5.46);
            participant3.Evaluate(4.88);
            participant3.Evaluate(3.99);
            participant3.Evaluate(4.79);
            participant3.Evaluate(5.56);

            Purple_3.Participant[] participants = new Purple_3.Participant[] { participant1, participant2, participant3 };

            Purple_3.IceSkating junior = new Purple_3.IceSkating(new double[] { 33, 22, 11, 44, 3, 4, 1 });

            junior.Add(participants);

            PurpleTXTSerializer serializer = new PurpleTXTSerializer();
            serializer.SelectFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            serializer.SerializePurple3Skating(junior, "testing_skates");
        }

        void Test_Purple_4()
        {
            var participants = new Purple_4.Sportsman[10]
            {
                new Purple_4.Sportsman("Vasya", "Petrovich"),
                new Purple_4.Sportsman("Petya", "Nikolayevich"),
                new Purple_4.Sportsman("Kolya", "Vadimovich"),
                new Purple_4.Sportsman("Vadim", "Maratovich"),
                new Purple_4.Sportsman("Roma", "Egorovich"),
                new Purple_4.Sportsman("Egor", "Vasiliyevich"),
                new Purple_4.Sportsman("Masha", "Nikolayevna"),
                new Purple_4.Sportsman("Marat", "Danilovich"),
                new Purple_4.Sportsman("Danil", "Romanovich"),
                new Purple_4.Sportsman("Dasha", "Vadimovna")
            };
            foreach (var item in participants)
            {
                for (global::System.Int32 i = 0; i < 10; i++)
                {
                    item.Run( 50 + 10);
                }
            }
            var groups = new Purple_4.Group[5]
            {
                new Purple_4.Group("West"),
                new Purple_4.Group("East"),
                new Purple_4.Group("Europe"),
                new Purple_4.Group("Asia"),
                new Purple_4.Group("Global")
            };
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i].Add(participants.Take(2 * i + 1).ToArray());
            }

            PurpleXMLSerializer serializer = new PurpleXMLSerializer();
            serializer.SelectFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            Console.WriteLine(groups[1].Sportsmen.Length);

            for (int i = 0; i < groups.Length; i++)
            {
                serializer.SerializePurple4Group(groups[i], $"t{i}");
                var d_group = serializer.DeserializePurple4Group($"t{i}");
                //Console.WriteLine($"{groups[i].Sportsmen}    {d_group.Sportsmen}");
            }
        }

        void Test_Purple_5()
        {
            PurpleTXTSerializer serializer = new PurpleTXTSerializer();
            serializer.SelectFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            Purple_5.Report report = new Purple_5.Report();

            report.MakeResearch();
            report.MakeResearch();

            var a1 = new string[] { "Макака", null, "Манга" };
            var a2 = new string[] { "Тануки", "Проницательность", "Манга" };
            var a3 = new string[] { "Тануки", "Скромность", "Кимоно" };
            var a4 = new string[] { null, "Внимательность", "Суши" };
            var a5 = new string[] { "Сима_энага", "Дружелюбность", "Кимоно" };

            report.Researches[0].Add(a1);
            report.Researches[0].Add(a2);
            report.Researches[0].Add(a3);
            report.Researches[0].Add(a4);
            report.Researches[0].Add(a5);

            var a6 = new string[] { "Макака", "Внимательность", "Самурай" };
            var a7 = new string[] { "Панда", "Проницательность", "Манга" };
            var a8 = new string[] { "Сима_энага", "Проницательность", "Суши" };
            var a9 = new string[] { "Серау", "Внимательность", "Сакура" };
            var a10 = new string[] { "Панда", null, "Кимоно" };

            report.Researches[1].Add(a6);
            report.Researches[1].Add(a7);
            report.Researches[1].Add(a8);
            report.Researches[1].Add(a9);
            report.Researches[1].Add(a10);

            foreach(var research in report.Researches)
            {
                research.Print();
            }

            Console.WriteLine();

            serializer.SerializePurple5Report(report, "testing_report");
            var d_report = serializer.DeserializePurple5Report("testing_report");

            foreach(var research in d_report.Researches)
            {
                research.Print();
            }
        }
    }
}
