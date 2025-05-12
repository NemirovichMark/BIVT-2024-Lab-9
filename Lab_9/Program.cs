using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using static Lab_7.Blue_2;

namespace Lab_9
{
    public class Program
    {
        static void Main(string[] args)
        {
            string fileName = "C:\\Users\\syrcd\\Desktop\\testiki\\filetest.txt";

            //Blue_1

            //BlueTXTSerializer program = new BlueTXTSerializer();
            //Blue_1.Response vova1 = new Blue_1.Response("Vova", 3);
            //string fileName = "C:\\Users\\syrcd\\Desktop\\testiki\\filetest.txt";
            //program.SerializeBlue1Response(vova1, fileName);
            //var dez = program.DeserializeBlue1Response(fileName);
            //Console.WriteLine($"Проверка Response: Name={dez.Name}, Votes={dez.Votes}");


            //// Тестирование HumanResponse(с фамилией)
            //var humanResponse = new Blue_1.HumanResponse("Петр", "Петров", 200);
            //program.SerializeBlue1Response(humanResponse, fileName);
            //var deserializedHuman = program.DeserializeBlue1Response(fileName);
            //if (deserializedHuman is Blue_1.HumanResponse human)
            //{
            //    Console.WriteLine($"Проверка HumanResponse: Name={human.Name}, Surname={human.Surname}, Votes={human.Votes}");
            //}

            //BlueJSONSerializer program = new BlueJSONSerializer();
            //Blue_1.Response vova1 = new Blue_1.Response("Vova", 3);
            //string fileName = "C:\\Users\\syrcd\\Desktop\\testiki\\filetest.txt";
            //program.SerializeBlue1Response(vova1, fileName);
            //var dez = program.DeserializeBlue1Response(fileName);
            //Console.WriteLine($"Проверка Response: Name={dez.Name}, Votes={dez.Votes}");


            //Тестирование HumanResponse(с фамилией)
            //var humanResponse = new Blue_1.HumanResponse("Петр", "Петров", 200);
            //program.SerializeBlue1Response(humanResponse, fileName);
            //var deserializedHuman = program.DeserializeBlue1Response(fileName);
            //if (deserializedHuman is Blue_1.HumanResponse human)
            //{
            //    Console.WriteLine($"Проверка HumanResponse: Name={human.Name}, Surname={human.Surname}, Votes={human.Votes}");
            //}

            //Blue_2

            //BlueTXTSerializer program = new BlueTXTSerializer();

            ////3 m
            //var waterJump = new Blue_2.WaterJump3m("Чемпионат мира", 10000);
            ////participant
            //var participant1 = new Blue_2.Participant("Иван", "Иванов");
            //participant1.Jump(new[] { 5, 6, 7, 8, 9 });
            //participant1.Jump(new[] { 6, 7, 8, 9, 10 });
            //waterJump.Add(participant1);

            //var participant2 = new Blue_2.Participant("Петр", "Петров");
            //participant2.Jump(new[] { 7, 8, 9, 10, 10 });
            //participant2.Jump(new[] { 8, 9, 10, 10, 10 });
            //waterJump.Add(participant2);

            //program.SerializeBlue2WaterJump(waterJump, fileName);
            //var deserializedWaterJump = program.DeserializeBlue2WaterJump(fileName);
            //Console.WriteLine("\nСравнение исходного и десериализованного объектов:");
            //Console.WriteLine($"Название: оригинал - {waterJump.Name}, десериализованный - {deserializedWaterJump.Name}");
            //Console.WriteLine($"Банк: оригинал - {waterJump.Bank}, десериализованный - {deserializedWaterJump.Bank}");
            //Console.WriteLine($"Тип: оригинал - {waterJump.GetType().Name}, десериализованный - {deserializedWaterJump.GetType().Name}");





            //Console.WriteLine("\nУчастники:");
            //for (int i = 0; i < 2; i++)
            //{
            //    var original = waterJump.Participants[i];
            //    var deserialized = deserializedWaterJump.Participants[i];

            //    Console.WriteLine($"\nУчастник {i + 1}:");
            //    Console.WriteLine($"Имя: оригинал - {original.Name}, десериализованный - {deserialized.Name}");
            //    Console.WriteLine($"Фамилия: оригинал - {original.Surname}, десериализованный - {deserialized.Surname}");

            //    Console.WriteLine("Оценки за первый прыжок:");
            //    Console.WriteLine($"Оригинал: {string.Join(", ", original.Marks.Cast<int>().Take(5))}");
            //    Console.WriteLine($"Десериализованный: {string.Join(", ", deserialized.Marks.Cast<int>().Take(5))}");

            //    Console.WriteLine("Оценки за второй прыжок:");
            //    Console.WriteLine($"Оригинал: {string.Join(", ", original.Marks.Cast<int>().Skip(5).Take(5))}");
            //    Console.WriteLine($"Десериализованный: {string.Join(", ", deserialized.Marks.Cast<int>().Skip(5).Take(5))}");
            //}

            //// Дополнительная проверка для 5-метрового трамплина
            //Console.WriteLine("\nТестирование для 5-метрового трамплина:");
            //var waterJump5m = new Blue_2.WaterJump5m("Кубок Европы", 15000);
            //waterJump5m.Add(new Blue_2.Participant("Алексей", "Сидоров"));
            //fileName = "waterjump5m_test.txt";

            //program.SerializeBlue2WaterJump(waterJump5m, fileName);
            //var deserialized5m = program.DeserializeBlue2WaterJump(fileName);

            //Console.WriteLine($"Тип оригинального: {waterJump5m.GetType().Name}");
            //Console.WriteLine($"Тип десериализованного: {deserialized5m.GetType().Name}");



            ////json Blue_2
            //// Создаем тестовый объект WaterJump
            //var waterJump = new WaterJump3m("Olympic Pool", 3);

            //// Добавляем участников
            //var participant1 = new Participant("John", "Doe");
            //participant1.Jump(new int[] { 8, 9, 7, 8, 9 });
            //participant1.Jump(new int[] { 9, 9, 8, 9, 9 });

            //var participant2 = new Participant("Jane", "Smith");
            //participant2.Jump(new int[] { 7, 8, 7, 8, 7 });


            //waterJump.Add(participant1);
            //waterJump.Add(participant2);

            //// Создаем сериализатор
            //var serializer = new BlueJSONSerializer();
            //serializer.SerializeBlue2WaterJump(waterJump, fileName);
            //var deserialized = serializer.DeserializeBlue2WaterJump(fileName);

            //// Проверяем результат десериализации
            //Console.WriteLine("\nDeserialized object:");
            //Console.WriteLine($"Type: {deserialized.GetType().Name}");
            //Console.WriteLine($"Name: {deserialized.Name}");
            //Console.WriteLine($"Bank: {deserialized.Bank}");
            //Console.WriteLine($"Participants count: {2}");

            //foreach (var p in deserialized.Participants)
            //{
            //    Console.WriteLine($"\nParticipant: {p.Name} {p.Surname}");
            //    Console.WriteLine("Marks:");
            //    for (int i = 0; i < p.Marks.GetLength(0); i++)
            //    {
            //        Console.Write($"Jump {i + 1}: ");
            //        for (int j = 0; j < p.Marks.GetLength(1); j++)
            //        {
            //            Console.Write($"{p.Marks[i, j]} ");
            //        }
            //        Console.WriteLine();
            //    }
            //    Console.WriteLine($"Total Score: {p.TotalScore}");
            //}

            //Console.WriteLine("\nTest completed successfully!");



            //XMLLLLLLLLLLLLLLLLLLLLLLLLLLLl
            BlueXMLSerializer testik = new BlueXMLSerializer();

            // 1.Инициализация сериализатора
            var serializer = new BlueXMLSerializer();
            string testFile = "test_response.xml";

            // 2. Тест базового класса Response
            Console.WriteLine("Тестирование Response:");
            var response = new Blue_1.Response("Опрос 1", 15);
            TestSerialization(serializer, response, testFile);

            // 3. Тест класса HumanResponse
            Console.WriteLine("\nТестирование HumanResponse:");
            var humanResponse = new Blue_1.HumanResponse("Иван", "Иванов", 10);
            TestSerialization(serializer, humanResponse, testFile);

            Console.WriteLine("\nТестирование завершено!");



        }
        static void TestSerialization(BlueXMLSerializer serializer,
                                Blue_1.Response response,
                                string fileName)
        {
            try
            {
                // A. Сериализация
                Console.WriteLine($"Сериализация: {response.Name}");
                serializer.SerializeBlue1Response(response, fileName);

                // Проверка что файл создан
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("Ошибка: Файл не создан!");
                    return;
                }

                // Проверка что файл не пустой
                if (new FileInfo(fileName).Length == 0)
                {
                    Console.WriteLine("Ошибка: Файл пустой!");
                    return;
                }


                Console.WriteLine("Файл успешно создан:");
                Console.WriteLine(File.ReadAllText(fileName));

                // B. Десериализация
                Console.WriteLine("\nДесериализация:");
                var deserialized = serializer.DeserializeBlue1Response(fileName);

                if (deserialized == null)
                {
                    Console.WriteLine("Ошибка: Десериализация вернула null");
                    return;
                }

                // C. Проверка данных
                Console.WriteLine($"Имя: {deserialized.Name}");
                Console.WriteLine($"Голосов: {deserialized.Votes}");

                if (deserialized is Blue_1.HumanResponse human)
                {
                    Console.WriteLine($"Фамилия: {human.Surname}");
                }

                // D. Сравнение объектов
                Console.WriteLine("\nСравнение объектов:");
                Console.WriteLine($"Тип совпадает: {response.GetType() == deserialized.GetType()}");
                Console.WriteLine($"Имя совпадает: {response.Name == deserialized.Name}");
                Console.WriteLine($"Голоса совпадают: {response.Votes == deserialized.Votes}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                // Очистка
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    Console.WriteLine("\nТестовый файл удален");
                }
            }
        }
    }
}
