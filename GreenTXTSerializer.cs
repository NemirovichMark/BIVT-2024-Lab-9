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

        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(string.Join(Environment.NewLine,
    "Surname: " + participant.Surname,
    "Group: " + participant.Group,
    "Trainer: " + participant.Trainer,
    "Result: " + participant.Result) + Environment.NewLine);

                if (participant is Green_1.Participant100M)
                {
                    writer.Write("Discipline: " + "100M" + Environment.NewLine);
                }

                if (participant is Green_1.Participant500M)
                {
                    writer.Write("Discipline: " + "500M" + Environment.NewLine);
                }
            }
        }
        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            string[] st = File.ReadAllLines(filePath);
            string[] t = new string[5];

            for (int i = 0; i < 5; i++)
            {
                string[] chasti = st[i].Split(':');
                t[i] = chasti[1].Trim();
            }

            Green_1.Participant participant;
            if (t[4] == "100M")
            {
                participant = new Green_1.Participant100M(t[0], t[1], t[2]);
            }
            else
            {
                participant = new Green_1.Participant500M(t[0], t[1], t[2]);
            }

            string result = t[3]
                .Replace(',', '.')
                .Replace(" ", "");
            double res;
            if (!double.TryParse(result, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
            {
                return null;
            }
            participant.Run(res);
            return participant;
        }

        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Name: {human.Name}");
                writer.WriteLine($"Surname: {human.Surname}");
                if (human is Green_2.Student stud)
                {
                    writer.WriteLine($"Marks: {string.Join(",", stud.Marks)}");
                }
            }
        }
        public override Green_2.Human DeserializeGreen2Human(string fileName)
        {
            var file = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            var file_lines = File.ReadAllLines(file);
            var first = file_lines[0].Split(':')[1].Trim();
            var second = file_lines[1].Split(':')[1].Trim();

            if (file_lines.Length <= 2)
            {
                return new Green_2.Human(first, second);
            }
            var student = new Green_2.Student(first, second); //имя и фамилия
            var marks = file_lines[2].Split(':')[1].Trim().Split(',').Select(int.Parse);

            foreach (var mark in marks)
            {
                student.Exam(mark);
            }
            return student;
        }

        public override void SerializeGreen3Student(Green_3.Student student, string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write("Name:"); writer.WriteLine(student.Name);

                writer.Write("Surname:"); writer.WriteLine(student.Surname);

                writer.Write("ID:"); writer.WriteLine(student.ID);

                writer.Write("Marks:"); writer.WriteLine(string.Join(",", student.Marks));
            }
        }
        public override Green_3.Student DeserializeGreen3Student(string fileName)
        {
            var t = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            var st = File.ReadAllLines(t);

            var namePart = st[0].Split(':')[1].Trim();
            var surnamePart = st[1].Split(':')[1].Trim();
            var agePart = st[2].Split(':')[1].Trim();
            var age = int.Parse(agePart);

            var student = new Green_3.Student(namePart, surnamePart, age);

            var marks = st[3].Split(':')[1].Trim().Split(',').Select(int.Parse);

            foreach (var mark in marks)
            {
                student.Exam(mark);
            }
            return student;
        }

        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            Directory.CreateDirectory(FolderPath);

            using var writer = new StreamWriter(Path.Combine(FolderPath, $"{fileName}.{Extension}"));

            writer.WriteLine($"Name: {discipline.Name}");
            writer.WriteLine($"Discipline: {(discipline is Green_4.LongJump ? "LongJump" : "HighJump")}");
            writer.WriteLine($"Count: {discipline.Participants.Length}");
            writer.WriteLine("Participants:");

            foreach (var p in discipline.Participants)
            {
                string jumps = string.Join(",", p.Jumps.Select(j => j.ToString(CultureInfo.InvariantCulture)));
                writer.WriteLine($"{p.Name}|{p.Surname}|{jumps}");
            }
        }
        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            using var reader = new StreamReader(Path.Combine(FolderPath, $"{fileName}.{Extension}"));

            var name = reader.ReadLine()!.Split(':')[1].Trim();
            var type = reader.ReadLine()!.Split(':')[1].Trim();
            Green_4.Discipline disc = type == "LongJump"
    ? new Green_4.LongJump()
    : new Green_4.HighJump();

            reader.ReadLine(); // Count
            reader.ReadLine(); // Participants:

            while (!reader.EndOfStream)
            {
                var parts = reader.ReadLine()!.Split('|');
                var p = new Green_4.Participant(parts[0], parts[1]);

                foreach (var j in parts[2].Split(','))
                    p.Jump(double.Parse(j, CultureInfo.InvariantCulture));

                disc.Add(p);
            }

            return disc;
        }

        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Type:" + group.GetType().Name);
                writer.WriteLine("Name:" + group.Name);
                writer.WriteLine("StudentCount:" + group.Students.Length);

                for (int i = 0; i < group.Students.Length; i++)
                {
                    var student = group.Students[i];
                    writer.WriteLine(student.Name + "|" + student.Surname + "|" + string.Join(",", student.Marks));
                }
            }
        }
        public override T DeserializeGreen5Group<T>(string fileName)
        {
            var st = File.ReadAllLines(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var type = st[0].Split(':')[1].Trim();
            var name = st[1].Split(':')[1].Trim();
            int k = int.Parse(st[2].Split(':')[1].Trim());

            Green_5.Group group;
            switch (type)
            {
                case "EliteGroup":
                    group = new Green_5.EliteGroup(name);
                    break;
                case "SpecialGroup":
                    group = new Green_5.SpecialGroup(name);
                    break;
                default:
                    group = new Green_5.Group(name);
                    break;
            }
            for (int i = 3; i < 3 + k; i++)
            {
                var parts = st[i].Split('|');
                var student = new Green_5.Student(parts[0], parts[1]);

                var marks = parts[2].Split(',');
                for (int j = 0; j < marks.Length; j++)
                {
                    student.Exam(int.Parse(marks[j]));
                }

                group.Add(student);
            }
            return (T)(object)group;
        }
    }
}