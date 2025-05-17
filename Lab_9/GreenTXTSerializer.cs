using Lab_7;
using static Lab_7.Green_4;
using System.Linq;
using System.Globalization;

namespace Lab_9
{
    public class GreenTXTSerializer : GreenSerializer
    {
        public override string Extension => "txt";

        //Green_1
        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            string fullpath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (var writer = new Writer(fullPath))
            {
                writer.WriteLine($"Surname:{participant.Surname}");
                writer.WriteLine($"Group:{participant.Group}");
                writer.WriteLine($"Trainer:{participant.Trainer}");
                writer.WriteLine($"Result:{participant.Result}");
                writer.WriteLine($"Discipline:{(participant is Green_1.Participant100M ? "100M" : "500M")}");
            })
        }
        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {
            var lines = File.ReadAllLines(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var data = lines.Select(l => l.Split(':')[1].Trim()).ToArray();

            Green_1.Participant participant = data[4] == "100M"
                ? new Green_1.Participant100M(data[0], data[1], data[2])
                : new Green_1.Participant500M(data[0], data[1], data[2]);

            participant.Run(double.Parse(data[3], CultureInfo.InvariantCulture));
            return participant;
        }

        //Green_2
        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (var writer = new Writer(fullPath))
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
            using (var writer = new Writer(fullPath))
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
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (var writer = new Writer(fullPath))
            {
                writer.WriteLine($"Name:{discipline.Name}");
                writer.WriteLine($"Type:{(discipline is LongJump ? "LongJump" : "HighJump")}");
                writer.WriteLine($"ParticipantCount:{discipline.Participants.Length}");

                foreach (var participant in discipline.Participants)
                {
                    writer.WriteLine($"{participant.Name}|{participant.Surname}|{string.Join(",", participant.Jumps)}");
                }
            }
        }
        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            var lines = File.ReadAllLines(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var discipline = lines[1].EndsWith("LongJump")
                ? new LongJump()
                : new HighJump();

            discipline.Name = lines[0].Split(':')[1].Trim();
            int count = int.Parse(lines[2].Split(':')[1].Trim());

            for (int i = 3; i < 3 + count; i++)
            {
                var parts = lines[i].Split('|');
                var participant = new Participant(parts[0], parts[1]);

                foreach (var jump in parts[2].Split(',').Select(double.Parse))
                {
                    participant.Jump(jump);
                }

                discipline.Add(participant);
            }
            return discipline;
        }

        //Green_5
        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (var writer = new Writer(fullPath))
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