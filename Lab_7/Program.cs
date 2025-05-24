using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_1;
using static Lab_7.Purple_2;
using static Lab_7.Purple_4;
using static Lab_7.Purple_5;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            //program.Task_1();
            //program.Task_2();
            //dprogram.Task_3();
            //program.Task_4();
            program.Task_5();
        }
        public void Task_1()
        {
            var program = new Program();

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
                Purple_1.Judge judge = new Purple_1.Judge(names[i], judgeFavoriteMarks[i]);

                judges[i] = judge;
            }

            Purple_1.Participant[] participants = new Purple_1.Participant[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                Purple_1.Participant p = new Purple_1.Participant(names[i], surnames[i]);

                p.SetCriterias(coefs[i]);
                participants[i] = p;
            }
            Purple_1.Competition competition = new Purple_1.Competition(judges);
            competition.Add(participants);
            competition.Sort();
            participants = competition.Participants;

            string[] items = new string[] { "Name", "Surname", "TotalScore" };
            PrintHead(items);
            for (int i = 0; i < 10; i++) participants[i].Print();
        }
        public void Task_2()
        {
            // 1. Массив с именами
            string[] names = new string[] { "Оксана", "Полина", "Дмитрий", "Евгения", "Савелий", "Евгения", "Егор", "Степан", "Анастасия", "Светлана" };

            // 2. Массив с фамилиями
            string[] surnames = new string[] { "Сидорова", "Полевая", "Полевой", "Распутина", "Луговой", "Павлова", "Свиридов", "Свиридов", "Козлова", "Свиридова" };

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
            Purple_2.Participant[] participants = new Purple_2.Participant[10];

            for (int i = 0; i < 10; i++)
            {
                Purple_2.Participant p = new Purple_2.Participant(names[i], surnames[i]);
                participants[i] = p;
            }
            Purple_2.JuniorSkiJumping juniorSkiJumping = new Purple_2.JuniorSkiJumping();
            juniorSkiJumping.Add(participants);

            for (int i = 0; i < 10; i++)
            {
                juniorSkiJumping.Jump(distances[i], marks[i]);
            }
            juniorSkiJumping.Print();

            participants = new Purple_2.Participant[10];

            for (int i = 0; i < 10; i++)
            {
                Purple_2.Participant p = new Purple_2.Participant(names[i], surnames[i]);
                participants[i] = p;
            }

            Purple_2.ProSkiJumping proSkiJumping = new Purple_2.ProSkiJumping();
            proSkiJumping.Add(participants);

            for (int i = 0; i < 10; i++)
            {
                proSkiJumping.Jump(distances[i], marks[i]);
            }
            proSkiJumping.Print();
        }
        public void Task_3()
        {
            // 1 массив с именами
            string[] names = {
            "Виктор", "Алиса", "Ярослав", "Савелий", "Алиса",
            "Алиса", "Александр", "Мария", "Полина", "Татьяна"
            };

            // 2 массив с фамилиями
            string[] surnames = {
            "Полевой", "Козлова", "Зайцев", "Кристиан", "Козлова",
            "Луговая", "Петров", "Смирнова", "Сидорова", "Сидорова"
            };

            // 3 массив оценок
            double[][] marks = {
            new double[] {5.93, 5.44, 1.20, 0.28, 1.57, 1.86, 5.89},
            new double[] {1.68, 3.79, 3.62, 2.76, 4.47, 4.26, 5.79},
            new double[] {2.93, 3.10, 5.46, 4.88, 3.99, 4.79, 5.56},
            new double[] {4.20, 4.69, 3.90, 1.67, 1.13, 5.66, 5.40},
            new double[] {3.27, 2.43, 0.90, 5.61, 3.12, 3.76, 3.73},
            new double[] {0.75, 1.13, 5.43, 2.07, 2.68, 0.83, 3.68},
            new double[] {3.78, 3.42, 3.84, 2.19, 1.20, 2.51, 3.51},
            new double[] {1.35, 3.40, 1.85, 2.02, 2.78, 3.23, 3.03},
            new double[] {0.55, 5.93, 0.75, 5.15, 4.35, 1.51, 2.77},
            new double[] {3.86, 0.19, 0.46, 5.14, 5.37, 0.94, 0.84}
            };

            Purple_3.Participant[] participants = new Purple_3.Participant[10];

            for (int i = 0; i < 10; i++)
                participants[i] = new Purple_3.Participant(names[i], surnames[i]);

            double[] moods = { 3.14, 0.57, 2.71, 1.62, 4.67, 0.89, 5.43, 3.33 };
            Purple_3.FigureSkating figureSkating = new Purple_3.FigureSkating(moods);
            figureSkating.Add(participants);
            for (int i = 0; i < 10; i++)
                figureSkating.Evaluate(marks[i]);
            Purple_3.Participant.SetPlaces(figureSkating.Participants);

            Console.WriteLine("Данные для первого сравнения");
            string[] items = new string[] { "Name", "Surname", "Score", "TopPlace", "TotalMark" };
            PrintHead(items);
            for (int i = 0; i < 10; i++) participants[i].Print();

            participants = new Purple_3.Participant[10];

            for (int i = 0; i < 10; i++)
                participants[i] = new Purple_3.Participant(names[i], surnames[i]);

            Purple_3.IceSkating iceSkating = new Purple_3.IceSkating(moods);
            iceSkating.Add(participants);
            for (int i = 0; i < 10; i++)
                iceSkating.Evaluate(marks[i]);
            Purple_3.Participant.SetPlaces(iceSkating.Participants);

            Console.WriteLine("Данные для второго сравнения");
            PrintHead(items);
            for (int i = 0; i < 10; i++) participants[i].Print();
        }
        public void Task_4()
        {
            // 1. Male first names
            string[] maleFirstNames = { "Савелий", "Дмитрий", "Дмитрий", "Савелий", "Степан" };

            // 2. Male last names
            string[] maleLastNames = { "Козлов", "Иванов", "Полевой", "Петров", "Павлов" };

            // 3. Female first names
            string[] femaleFirstNames = { "Полина", "Екатерина", "Евгения", "Екатерина", "Мария", "Ольга", "Ольга", "Дарья", "Дарья", "Евгения" };

            // 4. Female last names
            string[] femaleLastNames = { "Луговая", "Жаркова", "Распутина", "Луговая", "Иванова", "Павлова", "Полевая", "Павлова", "Свиридова", "Свиридова" };

            // 5. Male times
            double[] maleTimes = { 142.05, 294.32, 79.26, 230.63, 292.38 };

            // 6. Female times
            double[] femaleTimes = { 422.64, 185.23, 135.16, 376.12, 279.20, 467.60, 473.82, 290.14, 368.60, 212.67 };
            Purple_4.SkiMan[] skiMan = new Purple_4.SkiMan[maleFirstNames.Length];
            Purple_4.SkiWoman[] skiWoman = new Purple_4.SkiWoman[femaleFirstNames.Length];

            for (int i = 0; i < maleFirstNames.Length; i++)
            {
                Purple_4.SkiMan p = new Purple_4.SkiMan(maleFirstNames[i], maleLastNames[i]);
                p.Run(maleTimes[i]);
                skiMan[i] = p;
            }
            for (int i = 0; i < femaleFirstNames.Length; i++)
            {
                Purple_4.SkiWoman p = new Purple_4.SkiWoman(femaleFirstNames[i], femaleLastNames[i]);
                p.Run(femaleTimes[i]);
                skiWoman[i] = p;
            }
            Purple_4.Group group1 = new Purple_4.Group("Группа 1");
            group1.Add(skiMan);
            group1.Add(skiWoman);

            // 1. Male first names
            maleFirstNames = new string[] { "Александр", "Степан", "Игорь", "Лев", "Савелий", "Егор" };

            // 2. Male last names
            maleLastNames = new string[] { "Павлов", "Свиридов", "Сидоров", "Петров", "Козлов", "Свиридов" };

            // 3. Female first names
            femaleFirstNames = new string[] { "Анастасия", "Евгения", "Мария", "Оксана", "Светлана", "Полина", "Екатерина", "Юлия", "Евгения" };

            // 4. Female last names
            femaleLastNames = new string[] { "Жаркова", "Сидорова", "Сидорова", "Жаркова", "Петрова", "Петрова", "Павлова", "Полевая", "Павлова" };

            // 5. Male times
            maleTimes = new double[] { 472.11, 213.92, 102.13, 248.68, 325.28, 300.00 };

            // 6. Female times
            femaleTimes = new double[] { 112.49, 263.21, 350.75, 252.16, 402.20, 397.33, 384.94, 118.09, 480.52 };

            skiMan = new Purple_4.SkiMan[maleFirstNames.Length];
            skiWoman = new Purple_4.SkiWoman[femaleFirstNames.Length];

            for (int i = 0; i < maleFirstNames.Length; i++)
            {
                Purple_4.SkiMan p = new Purple_4.SkiMan(maleFirstNames[i], maleLastNames[i]);
                p.Run(maleTimes[i]);
                skiMan[i] = p;
            }
            for (int i = 0; i < femaleFirstNames.Length; i++)
            {
                Purple_4.SkiWoman p = new Purple_4.SkiWoman(femaleFirstNames[i], femaleLastNames[i]);
                p.Run(femaleTimes[i]);
                skiWoman[i] = p;
            }
            Purple_4.Group group2 = new Purple_4.Group("Группа 2");
            group2.Add(skiMan);
            group2.Add(skiWoman);

            Purple_4.Group result = Purple_4.Group.Merge(group1, group2);

            result.Shuffle();

            result.Print();
        }
        public void Task_5()
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
            (string, double)[] result = report.GetGeneralReport(2);
            PrintArray(result);
            //TestCountVotes(research.Responses, 0, 2);
            //research.Print();

        }
        public void TestCountVotes(Response[] responses, int responsesNumber, int questionNumber)
        {
            int result = responses[responsesNumber].CountVotes(responses, questionNumber);
            Console.WriteLine(result);
        }
        public void PrintHead(string[] items)
        {
            foreach (string item in items)
            {
                Console.Write(PrintItem(item));
            }
            Console.WriteLine();
        }
        public string PrintItem(string item)
        {
            return item + new string(' ', 15 - item.Length);
        }
        public void PrintArray<T>(T[] array)
        {
            Console.WriteLine("Элементы массива:");
            foreach (var item in array)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }
    }
}