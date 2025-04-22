using System;
using Lab_7;
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
        Test_Purple_1_1();
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
}
}