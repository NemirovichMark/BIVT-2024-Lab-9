using Lab_7;
using static Lab_7.Green_4;
using System.Linq;
using System.Globalization;
using static Lab_7.Green_1;

namespace Lab_9
{
    public class GreenTXTSerializer : GreenSerializer
    {
        public override string Extension => "txt";

        //Green_1
        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
        {
            string filePath = Path.Combine(FolderPath, fileName + "." + Extension);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Surname: {participant.Surname}");
                writer.WriteLine($"Group: {participant.Group}");
                writer.WriteLine($"Trainer: {participant.Trainer}");
                writer.WriteLine($"Result: {participant.Result}");

                if (participant is Green_1.Participant100M)
                {
                    writer.WriteLine("Discipline: 100M");
                }

                if (participant is Green_1.Participant500M)
                {
                    writer.WriteLine("Discipline: 500M");
                }
            }
        }
        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            string[] lines = File.ReadAllLines(fullPath);
            
            string[] data = new string[5];
            for (int i = 0; i < 5; i++)
            {
                string[] parts = lines[i].Split(':');
                data[i] = parts[1].Trim();
            }
            Green_1.Participant participant = data[4] == "100M"
                ? new Green_1.Participant100M(data[0], data[1], data[2])
                : new Green_1.Participant500M(data[0], data[1], data[2]);

            string result_str = data[3]
                .Replace(',', '.')
                .Replace(" ", "");

            if (!double.TryParse(result_str, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return null;
            }

            participant.Run(result);
            return participant;
        }

        //Green_2
        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine($"Name:{human.Name}");
                writer.WriteLine($"Surname:{human.Surname}");
                if (human is Green_2.Student student)
                {
                    writer.WriteLine($"Marks:{string.Join(",", student.Marks)}");
                }
            }
        }
        public override Green_2.Human DeserializeGreen2Human(string fileName)
        {
            var lines = File.ReadAllLines(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var name = lines[0].Split(':')[1].Trim();
            var surname = lines[1].Split(':')[1].Trim();

            if (lines.Length <= 2)
            {
                return new Green_2.Human(name, surname);
            }
            var student = new Green_2.Student(name, surname);
            var marks = lines[2].Split(':')[1].Trim().Split(',').Select(int.Parse);

            foreach (var mark in marks)
            {
                student.Exam(mark);
            }
            return student;
        }

        //Green_3
        public override void SerializeGreen3Student(Green_3.Student student, string fileName)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine($"Name:{student.Name}");
                writer.WriteLine($"Surname:{student.Surname}");
                writer.WriteLine($"ID:{student.ID}");
                writer.WriteLine($"Marks:{string.Join(",", student.Marks)}");
            }
        }
        public override Green_3.Student DeserializeGreen3Student(string fileName)
        {
            var lines = File.ReadAllLines(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var student = new Green_3.Student(
                lines[0].Split(':')[1].Trim(),
                lines[1].Split(':')[1].Trim(),
                int.Parse(lines[2].Split(':')[1].Trim()));
            var marks = lines[3].Split(':')[1].Trim().Split(',').Select(int.Parse);

            foreach (var mark in marks)
            {
                student.Exam(mark);
            }
            return student;
        }

        //Green_4
        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            string filePath = Path.Combine(FolderPath, fileName + "." + Extension);
            using var writer = new StreamWriter(filePath);

            writer.WriteLine($"Name: {discipline.Name}");
            writer.WriteLine(
                $"Discipline: {(discipline is Green_4.LongJump ? "LongJump" : "HighJump")}"
            );

            var participants = discipline.Participants;
            writer.WriteLine($"Count: {participants.Length}");
            writer.WriteLine("Participants:");

            for (int i = 0; i < participants.Length; i++)
            {
                var participant = participants[i];
                var jumps = new string[participant.Jumps.Length];

                for (int j = 0; j < participant.Jumps.Length; j++)
                {
                    jumps[j] = participant.Jumps[j].ToString(CultureInfo.InvariantCulture);
                }

                writer.WriteLine($"{participant.Name}|{participant.Surname}|{string.Join(",", jumps)}");
            }
        }
        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using var str_reader = new StreamReader(fullPath);

            var nameLine = str_reader.ReadLine()!;
            var typeLine = str_reader.ReadLine()!;

            string name_line = nameLine.Split(':', 2)[1].Trim();
            string type_line = typeLine.Split(':', 2)[1].Trim();

            Green_4.Discipline discipline = type_line switch
            {
                "LongJump" => new Green_4.LongJump(),
                "HighJump" => new Green_4.HighJump(),
                _ => throw new InvalidOperationException()
            };

            string _line = str_reader.ReadLine()!;
            string reader = str_reader.ReadLine()!;

            if (reader != "Participants:")
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < int.Parse(_line.Split(':', 2)[1].Trim()); i++)
            {
                string line = str_reader.ReadLine()!;
                string[] parts = line.Split('|');

                string _name = parts[0];
                string _surname = parts[1];

                double[] jumps = parts[2]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => double.Parse(x, CultureInfo.InvariantCulture))
                    .ToArray();

                var participant = new Green_4.Participant(_name, _surname);
                foreach (double t in jumps)
                {
                    participant.Jump(t);
                }

                discipline.Add(participant);
            }

            return discipline;
        }

        //Green_5
        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine($"Type:{group.GetType().Name}");
                writer.WriteLine($"Name:{group.Name}");
                writer.WriteLine($"StudentCount:{group.Students.Length}");

                foreach (var student in group.Students)
                {
                    writer.WriteLine($"{student.Name}|{student.Surname}|{string.Join(",", student.Marks)}");
                }
            }
        }
        public override T DeserializeGreen5Group<T>(string fileName)
        {
            var lines = File.ReadAllLines(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var type = lines[0].Split(':')[1].Trim();
            var name = lines[1].Split(':')[1].Trim();
            int count = int.Parse(lines[2].Split(':')[1].Trim());

            Green_5.Group group = type switch
            {
                "EliteGroup" => new Green_5.EliteGroup(name),
                "SpecialGroup" => new Green_5.SpecialGroup(name),
                _ => new Green_5.Group(name)
            };
            for (int i = 3; i < 3 + count; i++)
            {
                var parts = lines[i].Split('|');
                var student = new Green_5.Student(parts[0], parts[1]);

                foreach (var mark in parts[2].Split(',').Select(int.Parse))
                {
                    student.Exam(mark);
                }

                group.Add(student);
            }
            return (T)(object)group;
        }
    }
}