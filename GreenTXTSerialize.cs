using Lab_7; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; 
using System.Text.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using static Lab_7.Green_1;
using static Lab_7.Green_4;
using static Lab_7.Green_3;
using static Lab_7.Green_2;
using static Lab_7.Green_5;

namespace Lab_9
{
    // Класс сериализации в TXT 
    public class GreenTXTSerializer : GreenSerializer
    {
        
        public override string Extension => "txt";

        // Получение полного пути к файлу по имени (с расширением)
        private string GetFilePath(string fileName) =>
            Path.Combine(FolderPath, $"{fileName}.{Extension}");
        private string GetJsonPath(string fileName) =>
            Path.Combine(FolderPath, $"{fileName}.{Extension}");

       
        public override void SerializeGreen1Participant(Green_1.Participant p, string fileName)
        {
            // Формируем поля бесячего объекта
            var dict = new Dictionary<string, object>
            {
                ["Surname"] = p.Surname,
                ["Group"] = p.Group,
                ["Trainer"] = p.Trainer,
                ["Result"] = p.Result,
                ["Discipline"] = p is Green_1.Participant100M ? "100M" : "500M"
            };
            // Это сериализуем в строку JSON
            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);
            // Сохраняем строку в файл
            File.WriteAllText(GetJsonPath(fileName), json);
        }

        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {
            // Читаем строку JSON из файла
            string json = File.ReadAllText(GetJsonPath(fileName));
            // Десериализуем в словарь
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            // Извлекаем значения полей
            string surname = dict["Surname"].ToString();
            string group = dict["Group"].ToString();
            string trainer = dict["Trainer"].ToString();
            double result = Convert.ToDouble(dict["Result"]);
            string discipline = dict["Discipline"].ToString();

            //По типу дисциплины создаём нужный объект
            Green_1.Participant p = discipline == "100M"
                ? new Green_1.Participant100M(surname, group, trainer)
                : (Green_1.Participant)new Green_1.Participant500M(surname, group, trainer);

            // Восстанавливаем результат
            p.Run(result);
            return p;
        }

        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(human.Name); // Имя
                writer.WriteLine(human.Surname); // Фамилия
                if (human is Green_2.Student st)
                {
                    writer.WriteLine(st.Marks.Length); // Количество оценок
                    foreach (var mark in st.Marks)
                        writer.WriteLine(mark); // Каждая оценка
                }
            }
        }

        public override Green_2.Human DeserializeGreen2Human(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string name = reader.ReadLine(); // Имя
                string surname = reader.ReadLine(); // Фамилия

                string marksCountLine = reader.ReadLine(); // Количество оценок (или null)

                if (marksCountLine == null)
                {
                    // Если нет оценок — это Human
                    return new Green_2.Human(name, surname);
                }
                else
                {
                    int marksCount = int.Parse(marksCountLine); // Количество оценок
                    int[] marks = new int[marksCount];
                    for (int i = 0; i < marksCount; i++)
                    {
                        marks[i] = int.Parse(reader.ReadLine()); // Читаем каждую оценку
                    }

                    var student = new Green_2.Student(name, surname);
                    foreach (var mark in marks)
                    {
                        student.Exam(mark); // Добавляем оценку студенту
                    }
                    return student;
                }
            }
        }

        public override void SerializeGreen3Student(Green_3.Student student, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(student.Name); // Имя
                writer.WriteLine(student.Surname); // Фамилия
                writer.WriteLine(student.ID); // ID
                writer.WriteLine(student.Marks.Length); // Количество оценок
                foreach (var mark in student.Marks)
                {
                    writer.WriteLine(mark); // Каждая оценка
                }
                writer.WriteLine(student.IsExpelled); // Статус отчисления
            }
        }

        public override Green_3.Student DeserializeGreen3Student(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string name = reader.ReadLine(); // Имя
                string surname = reader.ReadLine(); // Фамилия
                int id = int.Parse(reader.ReadLine()); // ID

                // Создаём объект студента с ID
                var student = new Green_3.Student(name, surname, id);

                // Восстанавливаем оценки
                int marksCount = int.Parse(reader.ReadLine());
                for (int i = 0; i < marksCount; i++)
                {
                    int mark = int.Parse(reader.ReadLine());
                    student.Exam(mark); // Добавляем оценку
                }

                bool isExpelled = bool.Parse(reader.ReadLine()); // Статус отчисления

                return student;
            }
        }

        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(GetFilePath(fileName)))
            {
                string type = discipline is Green_4.LongJump ? "LongJump" : "HighJump";
                writer.WriteLine(type); // Тип дисциплины
                writer.WriteLine(discipline.Name); // Имя дисциплины
                var participants = discipline.Participants;
                writer.WriteLine(participants.Length); // Количество участников
                foreach (var pa in participants)
                {
                    writer.WriteLine(pa.Name); // Имя участника
                    writer.WriteLine(pa.Surname); // Фамилия участника
                    writer.WriteLine(string.Join(" ", pa.Jumps)); // Прыжки через пробел
                }
            }
        }


        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            using (StreamReader reader = new StreamReader(GetFilePath(fileName)))
            {
                string type = reader.ReadLine(); // Тип дисциплины
                string name = reader.ReadLine(); // Имя дисциплины
                int count = int.Parse(reader.ReadLine()); // Количество участников
                Green_4.Discipline disc = type == "LongJump" ? new Green_4.LongJump() : new Green_4.HighJump();

                for (int i = 0; i < count; i++)
                {
                    string pname = reader.ReadLine(); // Имя участника
                    string psurname = reader.ReadLine(); // Фамилия участника
                    var jumps = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray(); // Прыжки
                    var p = new Green_4.Participant(pname, psurname);
                    foreach (var j in jumps)
                        p.Jump(j); // Восстанавливаем все прыжки
                    disc.Add(p); // Добавляем участника в дисциплину
                }
                return disc;
            }
        }

        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(GetFilePath(fileName)))
            {
                writer.WriteLine(group.GetType().Name); // Тип группы
                writer.WriteLine(group.Name); // Имя группы
                var students = group.Students ?? new Green_5.Student[0];
                writer.WriteLine(students.Length); // Количество студентов
                foreach (var s in students)
                {
                    writer.WriteLine(s.Name); // Имя студента
                    writer.WriteLine(s.Surname); // Фамилия студента
                    var marks = s.Marks ?? Array.Empty<int>();
                    writer.WriteLine(marks.Length); // Количество оценок
                    foreach (var mark in marks)
                        writer.WriteLine(mark); // Каждая оценка
                }
            }
        }


        public override T DeserializeGreen5Group<T>(string fileName)
        {
            using (StreamReader reader = new StreamReader(GetFilePath(fileName)))
            {
                string type = reader.ReadLine(); // Тип группы
                string name = reader.ReadLine(); // Имя группы
                int studentsCount = int.Parse(reader.ReadLine()); // Количество студентов

                var groupType = Type.GetType($"Lab_7.Green_5+{type}, Lab_7");
                if (groupType == null)
                    throw new InvalidOperationException($"Тип 'Lab_7.Green_5+{type}' не найден.");
                var group = (Green_5.Group)Activator.CreateInstance(groupType, name);

                for (int i = 0; i < studentsCount; i++)
                {
                    string sname = reader.ReadLine(); // Имя студента
                    string ssurname = reader.ReadLine(); // Фамилия студента
                    int marksCount = int.Parse(reader.ReadLine()); // Количество оценок
                    int[] marks = new int[marksCount];
                    for (int j = 0; j < marksCount; j++)
                    {
                        string markStr = reader.ReadLine();
                        marks[j] = int.Parse(markStr); // Оценка
                    }
                    var student = new Green_5.Student(sname, ssurname);
                    foreach (var mark in marks)
                        student.Exam(mark); // Добавляем оценку
                    group.Add(student); // Добавляем студента в группу
                }
                return (T)group;
            }
        }
    }
}
