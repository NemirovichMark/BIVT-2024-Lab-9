using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Lab_7.Purple_1;
using static Lab_7.Purple_2;
using static Lab_7.Purple_5;

namespace Lab_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            var JSONSerialize = new JSONSerialize();
            JSONSerialize.serialize2();
            JSONSerialize.Deserialize2();

            var XMLSerialize = new XMLSerialize();
            //XMLSerialize.serialize1();
            //XMLSerialize.Deserialize1();
            //XMLSerialize.serialize2();
            //XMLSerialize.Deserialize2();
            //JSONSerialize.serialize5();
            //JSONSerialize.Deserialize5();
        }
    }
    public class JSONSerialize
    {
        private static PurpleJSONSerializer _json = new PurpleJSONSerializer();
        public JSONSerialize()
        {
            _json.SelectFolder(System.Reflection.Assembly.GetExecutingAssembly().Location.Split("\\bin")[0]);
        }
        public void serialize1()
        {
            var competition = CreateObj.CreatePurple1Competition();

            _json.SerializePurple1(competition, "purple1");
        }
        public void serialize2()
        {
            var skiJumping = CreateObj.CreatePurple2ProAndJuniorSkiJumping();
            var proSkiJumping = skiJumping.Item1;
            var juniorSkiJumping = skiJumping.Item2;

            _json.SerializePurple2SkiJumping(juniorSkiJumping, "juniorSkiJumping");
            _json.SerializePurple2SkiJumping(proSkiJumping, "proSkiJumping");
        }
        public void serialize5()
        {
            var report = CreateObj.CreatePurple5Report();

            _json.SerializePurple5Report(report, "report");

        }

        public void Deserialize1()
        {
            var data = _json.DeserializePurple1<Competition>("purple1");
        }
        public void Deserialize2()
        {
            var data = _json.DeserializePurple2SkiJumping<ProSkiJumping>("proSkiJumping");
        }
        public void Deserialize5()
        {
            var data = _json.DeserializePurple5Report("report");

            _json.SerializePurple5Report(data, "report1");
        }
    }
    public class XMLSerialize
    {
        private static PurpleXMLSerializer _xml = new PurpleXMLSerializer();
        public XMLSerialize()
        {
            _xml.SelectFolder(System.Reflection.Assembly.GetExecutingAssembly().Location.Split("\\bin")[0]);
        }
        public void serialize1()
        {
            var competition = CreateObj.CreatePurple1Competition();
            var participant = CreateObj.CreatePurple1Participants()[1];
            var judge = CreateObj.CreatePurple1Judges()[0];
            _xml.SerializePurple1(competition, "purple1");
        }
        public void serialize2()
        {
            var skiJumping = CreateObj.CreatePurple2ProAndJuniorSkiJumping();
            var proSkiJumping = skiJumping.Item1;
            var juniorSkiJumping = skiJumping.Item2;

            _xml.SerializePurple2SkiJumping(juniorSkiJumping, "juniorSkiJumping");
            _xml.SerializePurple2SkiJumping(proSkiJumping, "proSkiJumping");
        }
        public void serialize5()
        {
            var report = CreateObj.CreatePurple5Report();

            _xml.SerializePurple5Report(report, "report");

        }

        public void Deserialize1()
        {
            var data = _xml.DeserializePurple1<Purple_1.Competition>("purple1");
            _xml.SerializePurple1(data, "purple1Test");
        }
        public void Deserialize2()
        {
            var proSkiJumping = _xml.DeserializePurple2SkiJumping<SkiJumping>("proSkiJumping");
            var juniorSkiJumping = _xml.DeserializePurple2SkiJumping<SkiJumping>("juniorSkiJumping");
            _xml.SerializePurple2SkiJumping(juniorSkiJumping, "juniorSkiJumpingTest");
            _xml.SerializePurple2SkiJumping(proSkiJumping, "proSkiJumpingTest");
        }
        public void Deserialize5()
        {
            var data = _xml.DeserializePurple5Report("report");

            _xml.SerializePurple5Report(data, "report1");
        }
    }
    public class CreateObj
    {
        public static Purple_1.Participant[] CreatePurple1Participants()
        {
            // Массив имен
            string[] names = new string[]
            {
            "Дарья", "Александр", "Никита", "Юрий", "Юрий", "Мария", "Виктор", "Марина", "Марина", "Максим"
            };

            // Массив фамилий
            string[] surnames = new string[]
            {
            "Тихонова", "Козлов", "Павлов", "Луговой", "Степанов", "Луговая", "Жарков", "Иванова", "Полевая", "Тихонов"
            };

            // Массив коэффициентов
            double[][] coefs = new double[][]
            {
            new double[] {2.58, 2.90, 3.04, 3.43},
            new double[] {2.95, 2.63, 3.16, 2.89},
            new double[] {2.56, 3.40, 2.91, 2.69},
            new double[] {2.86, 2.90, 3.19, 3.14},
            new double[] {2.81, 2.64, 2.76, 3.20},
            new double[] {2.74, 3.30, 2.94, 3.27},
            new double[] {2.57, 2.79, 2.71, 3.46},
            new double[] {3.09, 2.67, 2.90, 3.50},
            new double[] {2.65, 3.47, 3.11, 3.39},
            new double[] {3.14, 3.46, 2.96, 2.76}
            };

            Purple_1.Participant[] participants = new Purple_1.Participant[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                Purple_1.Participant p = new Purple_1.Participant(names[i], surnames[i]);

                p.SetCriterias(coefs[i]);
                participants[i] = p;
            }
            return participants;
        }
        public static Purple_1.Judge[] CreatePurple1Judges()
        {
            string[] judgeNames = { "Алексей", "Мария", "Иван", "Ольга", "Дмитрий", "Екатерина", "Сергей", "Анна" };

            int[][] judgeFavoriteMarks =
            {
                new int[] { 3, 5, 2 },
                new int[] { 4, 6 },
                new int[] { 2, 1, 3, 5 },
                new int[] { 6, 4, 3 },
                new int[] { 1, 2, 4 },
                new int[] { 5, 3, 6, 2 },
                new int[] { 4, 2 },
                new int[] { 3, 5, 1 }
            };

            Purple_1.Judge[] judges = new Purple_1.Judge[judgeNames.Length];
            for (int i = 0; i < judgeNames.Length; i++)
            {
                Purple_1.Judge judge = new Purple_1.Judge(judgeNames[i], judgeFavoriteMarks[i]);

                judges[i] = judge;
            }
            return judges;
        }
        public static Purple_1.Competition CreatePurple1Competition()
        {
            Purple_1.Competition competition = new Purple_1.Competition(CreateObj.CreatePurple1Judges());
            competition.Add(CreateObj.CreatePurple1Participants());
            competition.Sort();

            return competition;
        }

        public static Purple_2.Participant[] CreatePurple2Participants()
        {
            // 1. Массив с именами
            string[] names = new string[] { "Оксана", "Полина", "Дмитрий", "Евгения", "Савелий", "Евгения", "Егор", "Степан", "Анастасия", "Светлана" };

            // 2. Массив с фамилиями
            string[] surnames = new string[] { "Сидорова", "Полевая", "Полевой", "Распутина", "Луговой", "Павлова", "Свиридов", "Свиридов", "Козлова", "Свиридова" };

            Purple_2.Participant[] participants = new Purple_2.Participant[10];

            for (int i = 0; i < 10; i++)
            {
                Purple_2.Participant p = new Purple_2.Participant(names[i], surnames[i]);
                participants[i] = p;
            }
            return participants;
        }
        public static (Purple_2.ProSkiJumping, Purple_2.JuniorSkiJumping) CreatePurple2ProAndJuniorSkiJumping()
        {
            var participants = CreatePurple2Participants();
            // 3. Массив с дистанциями
            int[] distances = new int[] { 135, 191, 147, 115, 112, 151, 186, 166, 112, 197 };

            // 4. Зубчатый двумерный массив (10x5)
            int[][] marks = new int[][]
            {
            new int[] { 15, 1, 3, 9, 15 },
            new int[] { 19, 14, 9, 11, 4 },
            new int[] { 20, 9, 1, 13, 6 },
            new int[] { 5, 20, 17, 9, 16 },
            new int[] { 19, 8, 1, 6, 17 },
            new int[] { 16, 12, 5, 20, 4 },
            new int[] { 5, 20, 3, 19, 18 },
            new int[] { 16, 12, 5, 4, 15 },
            new int[] { 7, 4, 19, 11, 12 },
            new int[] { 14, 3, 6, 17, 1 }
            };
            Purple_2.JuniorSkiJumping juniorSkiJumping = new Purple_2.JuniorSkiJumping();
            juniorSkiJumping.Add(participants);

            for (int i = 0; i < 10; i++)
            {
                juniorSkiJumping.Jump(distances[i], marks[i]);
            }
            Tools.Print(participants[0].Marks);
            Purple_2.ProSkiJumping proSkiJumping = new Purple_2.ProSkiJumping();
            proSkiJumping.Add(participants);

            for (int i = 0; i < 10; i++)
            {
                proSkiJumping.Jump(distances[i], marks[i]);
            }

            return (proSkiJumping, juniorSkiJumping);
        }
        public static Purple_5.Report CreatePurple5Report()
        {
            string[] animals = { "Макака", "Тануки", "Тануки", "Кошка", "Сима_энага", "Макака", "Панда", "Сима_энага", "Серау", "Панда", "Сима_энага", "Кошка", "Панда", "Кошка", "Панда", "Серау", "Панда", "Сима_энага", "Панда", "Кошка" };
            string[] qualities = { "", "Проницательность", "Скромность", "Внимательность", "Дружелюбность", "Внимательность", "Проницательность", "Проницательность", "Внимательность", "", "Дружелюбность", "Внимательность", "", "Уважительность", "Целеустремленность", "Дружелюбность", "", "Скромность", "Проницательность", "Внимательность" };
            string[] concept = { "Манга", "Манга", "Кимоно", "Суши", "Кимоно", "Самурай", "Манга", "Суши", "Сакура", "Кимоно", "Сакура", "Кимоно", "Сакура", "Фудзияма", "Аниме", "", "Манга", "Фудзияма", "Самурай", "Сакура" };

            Purple_5.Report report = new Purple_5.Report();
            Purple_5.Research research = report.MakeResearch();
            for (int i = 0; i < 20; i++)
            {
                string[] answers = new string[] { animals[i], qualities[i], concept[i] };

                research.Add(answers);
            }

            research = report.MakeResearch();
            for (int i = 0; i < 20; i++)
            {
                string[] answers = new string[] { animals[i], qualities[i], concept[i] };

                research.Add(answers);
            }

            return report;
        }
        
    }
    public class Tools
    {
        public static void Print<T>(T[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static void Print<T>(T[] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                Console.Write(array[i]);
            }
            Console.WriteLine();
        }
    }
}