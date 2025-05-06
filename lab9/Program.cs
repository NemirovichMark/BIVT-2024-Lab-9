using System;
using Lab_7;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_9
{
class Program
{
    static void Main(string[] args)
    {   
        Purple_1.Judge participant = new Purple_1.Judge("Вадим", new int[]{1, 2, 3, 4, 5, 6, 7});
        System.Console.WriteLine(participant.GetType());
        //Test_Purple_5_2();

    }   
        static void Test_Purple_1_1()
        {
            string[,] namesTask_1 = new string[,]
            {
                { "Дарья", "Тихонова" },
                { "Александр", "Козлов" },
                { "Никита", "Павлов" },
                { "Юрий", "Луговой" },
                { "Юрий", "Степанов" },
                { "Мария", "Луговая" },
                { "Виктор", "Жарков" },
                { "Марина", "Иванова" },
                { "Марина", "Полевая" },
                { "Максим", "Тихонов" }
            };

            double[,] coefficientsTask_1 = new double[,]
            {
            { 2.58, 2.90, 3.04, 3.43 },
            { 2.95, 2.63, 3.16, 2.89 },
            { 2.56, 3.40, 2.91, 2.69 },
            { 2.86, 2.90, 3.19, 3.14 },
            { 2.81, 2.64, 2.76, 3.20 },
            { 2.74, 3.30, 2.94, 3.27 },
            { 2.57, 2.79, 2.71, 3.46 },
            { 3.09, 2.67, 2.90, 3.50 },
            { 2.65, 3.47, 3.11, 3.39 },
            { 3.14, 3.46, 2.96, 2.76 }
            };

            int[,,] scores = new int[10, 4, 7]
            {
            {
                { 3, 4, 1, 2, 1, 3, 1 },
                { 5, 3, 4, 3, 3, 3, 3 },
                { 2, 4, 1, 5, 6, 1, 2 },
                { 6, 4, 3, 2, 2, 1, 1 },
                 },
                {
                { 3, 5, 4, 4, 5, 1, 4 },
                { 1, 6, 5, 2, 1, 4, 1 },
                { 6, 2, 4, 1, 2, 6, 5 },
                { 6, 5, 2, 2, 4, 3, 4 },
                 },
                {
                { 1, 1, 3, 5, 5, 5, 2 },
                { 4, 1, 1, 2, 2, 2, 5 },
                { 5, 2, 3, 3, 2, 2, 3 },
                { 3, 1, 3, 4, 2, 4, 5 },
                 },
                {
                { 3, 3, 5, 2, 1, 2, 4 },
                { 5, 5, 4, 2, 3, 2, 2 },
                { 6, 3, 1, 2, 2, 6, 6 },
                { 5, 1, 6, 6, 3, 2, 5 },
                 },
                {
                { 4, 3, 5, 4, 5, 1, 1 },
                { 5, 3, 4, 2, 1, 1, 2 },
                { 2, 2, 4, 2, 6, 3, 4 },
                { 3, 2, 1, 3, 5, 1, 5 },
                 },
                {
                { 6, 5, 5, 4, 2, 6, 4 },
                { 5, 4, 3, 2, 4, 6, 1 },
                { 1, 1, 3, 4, 4, 1, 6 },
                { 3, 1, 5, 1, 4, 3, 1 },
                 },
                {
                { 4, 6, 1, 4, 5, 3, 4 },
                { 1, 2, 3, 1, 5, 4, 3 },
                { 3, 6, 2, 3, 1, 6, 3 },
                { 3, 3, 6, 6, 3, 6, 6 },
                 },
                {
                { 6, 5, 3, 2, 6, 5, 3 },
                { 5, 4, 4, 2, 1, 2, 4 },
                { 4, 2, 2, 5, 1, 3, 1 },
                { 6, 5, 6, 1, 6, 3, 3 },
                 },
                {
                    { 3, 6, 3, 5, 4, 2, 3 },
                    { 4, 6, 1, 4, 2, 1, 5 },
                    { 1, 1, 3, 1, 3, 2, 6 },
                    { 1, 4, 4, 6, 6, 2, 5 },
                 },
                {
                    { 3, 3, 1, 4, 5, 6, 2 },
                    { 6, 4, 5, 4, 2, 3, 1 },
                    { 3, 3, 4, 2, 2, 3, 6 },
                    { 5, 1, 5, 5, 1, 3, 4 },
                }
            };

            Purple_1.Participant[] participants = new Purple_1.Participant[namesTask_1.GetLength(0)];
            for (int i = 0; i < namesTask_1.GetLength(0); i++)
            {
                Purple_1.Participant part = new Purple_1.Participant(namesTask_1[i, 0], namesTask_1[i, 1]);
                double[] coefs = new double[4];
                for (int j = 0; j < 4; j++)
                {
                    coefs[j] = coefficientsTask_1[i, j];
                    int[] marks = new int[7];
                    for (int k = 0; k < 7; k++)
                    {
                        marks[k] = scores[i, j, k];
                    }
                    part.Jump(marks);
                }
                part.SetCriterias(coefs);
                participants[i] = part;

            }
            Purple_1.Participant.Sort(participants);
            for (int i = 0; i < participants.Length; i++)
            {
                participants[i].Print();
            }
        }

        static void Test_Purple_3_1()
        {
            string[,] namesTask_3 = new string[,]
            {
                { "Виктор", "Полевой" },
                { "Алиса", "Козлова" },
                { "Ярослав", "Зайцев" },
                { "Савелий", "Кристиан" },
                { "Алиса", "Козлова" },
                { "Алиса", "Луговая" },
                { "Александр", "Петров" },
                { "Мария", "Смирнова" },
                { "Полина", "Сидорова" },
                { "Татьяна", "Сидорова" },
            };
            double[,] marksTask_3 = new double[,]
            {
                { 5.93, 5.44, 1.2, 0.28, 1.57, 1.86, 5.89 },
                { 1.68, 3.79, 3.62, 2.76, 4.47, 4.26, 5.79 },
                { 2.93, 3.1, 5.46, 4.88, 3.99, 4.79, 5.56 },
                { 4.2, 4.69, 3.9, 1.67, 1.13, 5.66, 5.4 },
                { 3.27, 2.43, 0.9, 5.61, 3.12, 3.76, 3.73 },
                { 0.75, 1.13, 5.43, 2.07, 2.68, 0.83, 3.68 },
                { 3.78, 3.42, 3.84, 2.19, 1.2, 2.51, 3.51 },
                { 1.35, 3.4, 1.85, 2.02, 2.78, 3.23, 3.03 },
                { 0.55, 5.93, 0.75, 5.15, 4.35, 1.51, 2.77 },
                { 3.86, 0.19, 0.46, 5.14, 5.37, 0.94, 0.84 },
            };
            Purple_3.Participant[] participants = new Purple_3.Participant[10];

            for (int i = 0; i < participants.Length; i++)
            {
                Purple_3.Participant part = new Purple_3.Participant(namesTask_3[i, 0], namesTask_3[i, 1]);
                for (int j = 0; j < marksTask_3.GetLength(1); j++)
                {
                    part.Evaluate(marksTask_3[i, j]);
                }
                participants[i] = part;
            }
            Purple_3.Participant.SetPlaces(participants);
            Purple_3.Participant.Sort(participants);

            for (int i = 0; i < participants.Length; i++)
            {

                participants[i].Print();
            }
        }

        static void Test_Purple_3_2()
        {
            string[,] namesTask_3 = new string[,]
            {
                { "Виктор", "Полевой" },
                { "Алиса", "Козлова" },
                { "Ярослав", "Зайцев" },
                { "Савелий", "Кристиан" },
                { "Алиса", "Козлова" },
                { "Алиса", "Луговая" },
                { "Александр", "Петров" },
                { "Мария", "Смирнова" },
                { "Полина", "Сидорова" },
                { "Татьяна", "Сидорова" },
            };
            double[][] marksTask_3 = new double[][]
            {
                new double[] { 5.93, 5.44, 1.2, 0.28, 1.57, 1.86, 5.89 },
                new double[] { 1.68, 3.79, 3.62, 2.76, 4.47, 4.26, 5.79 },
                new double[]  { 2.93, 3.1, 5.46, 4.88, 3.99, 4.79, 5.56 },
                new double[] { 4.2, 4.69, 3.9, 1.67, 1.13, 5.66, 5.4 },
                new double[] { 3.27, 2.43, 0.9, 5.61, 3.12, 3.76, 3.73 },
                new double[] { 0.75, 1.13, 5.43, 2.07, 2.68, 0.83, 3.68 },
                new double[] { 3.78, 3.42, 3.84, 2.19, 1.2, 2.51, 3.51 },
                new double[] { 1.35, 3.4, 1.85, 2.02, 2.78, 3.23, 3.03 },
                new double[] { 0.55, 5.93, 0.75, 5.15, 4.35, 1.51, 2.77 },
                new double[]  { 3.86, 0.19, 0.46, 5.14, 5.37, 0.94, 0.84 },
            };

            double[] moods_3 = new double[] { 1, 2, 3, 4, 5, 6, 7 };

            Purple_3.FigureSkating figureSkating = new Purple_3.FigureSkating(moods_3);

            Purple_3.Participant[] participants = new Purple_3.Participant[namesTask_3.GetLength(0)];

            for (int i = 0; i < participants.Length; i++)
            {
                Purple_3.Participant part = new Purple_3.Participant(namesTask_3[i, 0], namesTask_3[i, 1]);
                figureSkating.Add(part);
                figureSkating.Evaluate(marksTask_3[i]);
            }
        }

        static void Test_Purple_3_3()
        {
            string[,] namesTask_3 = new string[,]
            {
                { "Виктор", "Полевой" },
                { "Алиса", "Козлова" },
                { "Ярослав", "Зайцев" },
                { "Савелий", "Кристиан" },
                { "Алиса", "Козлова" },
                { "Алиса", "Луговая" },
                { "Александр", "Петров" },
                { "Мария", "Смирнова" },
                { "Полина", "Сидорова" },
                { "Татьяна", "Сидорова" },
            };
            double[][] marksTask_3 = new double[][]
            {
                new double[] { 5.93, 5.44, 1.2, 0.28, 1.57, 1.86, 5.89 },
                new double[] { 1.68, 3.79, 3.62, 2.76, 4.47, 4.26, 5.79 },
                new double[]  { 2.93, 3.1, 5.46, 4.88, 3.99, 4.79, 5.56 },
                new double[] { 4.2, 4.69, 3.9, 1.67, 1.13, 5.66, 5.4 },
                new double[] { 3.27, 2.43, 0.9, 5.61, 3.12, 3.76, 3.73 },
                new double[] { 0.75, 1.13, 5.43, 2.07, 2.68, 0.83, 3.68 },
                new double[] { 3.78, 3.42, 3.84, 2.19, 1.2, 2.51, 3.51 },
                new double[] { 1.35, 3.4, 1.85, 2.02, 2.78, 3.23, 3.03 },
                new double[] { 0.55, 5.93, 0.75, 5.15, 4.35, 1.51, 2.77 },
                new double[]  { 3.86, 0.19, 0.46, 5.14, 5.37, 0.94, 0.84 },
            };

            double[] moods_3 = new double[] { 1, 2, 3, 4, 5, 6, 7 };

            Purple_3.IceSkating iceSkating = new Purple_3.IceSkating(moods_3);

            Purple_3.Participant[] participants = new Purple_3.Participant[namesTask_3.GetLength(0)];

            for (int i = 0; i < participants.Length; i++)
            {
                Purple_3.Participant part = new Purple_3.Participant(namesTask_3[i, 0], namesTask_3[i, 1]);
                iceSkating.Add(part);
                iceSkating.Add(null);
                iceSkating.Evaluate(marksTask_3[i]);
            }

            var parts = iceSkating.Participants;
            Purple_3.Participant.SetPlaces(parts);
            Purple_3.Participant.Sort(parts);

            foreach (var part in parts)
                part.Print();
        }

        static void Test_Purple_5_2()
        {
            string[,] responses = new string[,]
            {
                { "Макака", null,  "Манга" },
                { "Тануки", "Проницательность",  "Манга" },
                { "Тануки", "Скромность",  "Кимоно" },
                { "Кошка", "Внимательность",  "Суши" },
                { "Сима_энага", "Дружелюбность",  "Кимоно" },
                { "Макака", "Внимательность",  "Самурай" },
                { "Панда", "Проницательность",  "Манга" },
                { "Сима_энага", "Проницательность",  "Суши" },
                { "Серау", "Внимательность",  "Сакура" },
                { "Панда", null,  "Кимоно" },
                { "Сима_энага", "Дружелюбность",  "Сакура" },
                { "Кошка", "Внимательность",  "Кимоно" },
                { "Панда", null,  "Сакура" },
                { "Кошка", "Уважительность",  "Фудзияма" },
                { "Панда", "Целеустремленность",  "Аниме" },
                { "Серау", "Дружелюбность",  null },
                { "Панда", null,  "Манга" },
                { "Сима_энага", "Скромность",  "Фудзияма" },
                { "Панда", "Проницательность",  "Самурай" },
                { "Кошка", "Внимательность",  "Сакура" },
            };

            Purple_5.Report report = new Purple_5.Report();

            for (int i = 0; i < responses.GetLength(0); i++)
            {
                Purple_5.Research research = report.MakeResearch();

                string[] resp = new string[] { responses[i, 0], responses[i, 1], responses[i, 2] };
                Console.WriteLine(resp);
                research.Add(resp);
                Console.WriteLine(research.Responses);
            }

            (string, double)[] generalReport = report.GetGeneralReport(3);

            for (int i = 0; i < generalReport.Length; i++)
            {
                Console.WriteLine(generalReport[i]);
            }


        }

        static void Test_Purple_5_1()
        {
            string[,] responses = new string[,]
            {
                { "Макака", null,  "Манга" },
                { "Тануки", "Проницательность",  "Манга" },
                { "Тануки", "Скромность",  "Кимоно" },
                { "Кошка", "Внимательность",  "Суши" },
                { "Сима_энага", "Дружелюбность",  "Кимоно" },
                { "Макака", "Внимательность",  "Самурай" },
                { "Панда", "Проницательность",  "Манга" },
                { "Сима_энага", "Проницательность",  "Суши" },
                { "Серау", "Внимательность",  "Сакура" },
                { "Панда", null,  "Кимоно" },
                { "Сима_энага", "Дружелюбность",  "Сакура" },
                { "Кошка", "Внимательность",  "Кимоно" },
                { "Панда", null,  "Сакура" },
                { "Кошка", "Уважительность",  "Фудзияма" },
                { "Панда", "Целеустремленность",  "Аниме" },
                { "Серау", "Дружелюбность",  null },
                { "Панда", null,  "Манга" },
                { "Сима_энага", "Скромность",  "Фудзияма" },
                { "Панда", "Проницательность",  "Самурай" },
                { "Кошка", "Внимательность",  "Сакура" },
            };

            Purple_5.Research research = new Purple_5.Research("res");

            for (int i = 0; i < responses.GetLength(0); i++)
            {
                research.Add(new string[] { responses[i, 0], responses[i, 1], responses[i, 2] });
            }

            string[] res1 = research.GetTopResponses(1),
                res2 = research.GetTopResponses(2),
                res3 = research.GetTopResponses(3);

            for (int i = 0; i < res1.Length; i++)
            {
                Console.WriteLine(res1[i]);
            }
            Console.WriteLine();

            for (int i = 0; i < res2.Length; i++)
            {
                Console.WriteLine(res2[i]);
            }
            Console.WriteLine();

            for (int i = 0; i < res3.Length; i++)
            {
                Console.WriteLine(res3[i]);
            }
            Console.WriteLine();
        }

}
}