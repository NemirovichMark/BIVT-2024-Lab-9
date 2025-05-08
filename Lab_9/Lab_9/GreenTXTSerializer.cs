using Lab_7;
using static Lab_7.Green_4;
using System.Linq;
using System.Globalization;

namespace Lab_9
{
    public class GreenTXTSerializer : GreenSerializer
    {
        public override string Extension => "txt";

        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
        {
            string filePath =  Path.Combine(FolderPath, fileName + "." + Extension);
            
            using (StreamWriter writer = new StreamWriter(filePath)) 
            {   writer.WriteLine($"Surname: {participant.Surname}");
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
            string filePath =  Path.Combine(FolderPath, fileName + "." + Extension);
            using (StreamReader reader = new StreamReader(filePath))
            {
                string surname = reader.ReadLine().Split(':')[1].Trim();
                string group = reader.ReadLine().Split(':')[1].Trim();
                string trainer = reader.ReadLine().Split(':')[1].Trim();
                double result = double.Parse(reader.ReadLine().Split(':')[1].Trim());

                string discipline = reader.ReadLine().Split(':')[1].Trim();

                if (discipline == "100M")
                {
                    var part = new Green_1.Participant100M(surname, group, trainer);
                    part.Run(result);
                    return part;
                }
                else if (discipline == "500M")
                {
                    var part = new Green_1.Participant500M(surname, group, trainer);
                    part.Run(result);
                    return part;
                }
                else
                {
                    throw new InvalidOperationException("Unknown discipline");
                }
            }
        }

        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            string filePath =  Path.Combine(FolderPath, fileName + "." + Extension);
            //if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Type: {human.GetType().Name}");
                writer.WriteLine($"Name: {human.Name}");
                writer.WriteLine($"Surname: {human.Surname}");

                if (human is Green_2.Student st)
                {
                    writer.WriteLine($"Marks: {string.Join(",", st.Marks)}");
                }
            }
        }

        public override Green_2.Human DeserializeGreen2Human(string fileName)
        {
            string filePath =  Path.Combine(FolderPath, fileName + "." + Extension);

            using (StreamReader reader = new StreamReader(filePath))
            { 
                string type = reader.ReadLine().Split(':')[1].Trim();
                string name = reader.ReadLine().Split(':')[1].Trim();
                string surname = reader.ReadLine().Split(':')[1].Trim();

                if (type == nameof(Green_2.Student))
                {
                    var st = new Green_2.Student(name,surname);
                    var marks = reader.ReadLine()?.Split(':')[1].Trim();

                    if (!string.IsNullOrEmpty(marks))
                    {
                        foreach (var m in marks.Split(',').Select(int.Parse))
                        {
                            if (m != 0) st.Exam(m);
                        }
                    }
                    return st;
                }
                else
                {
                    return new Green_2.Human(name, surname);
                } 
            }
        }

        public override void SerializeGreen3Student(Green_3.Student student, string fileName)
        {
            string filePath =  Path.Combine(FolderPath, fileName + "." + Extension);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Name:{student.Name}");
                writer.WriteLine($"Surname:{student.Surname}");
                writer.WriteLine($"ID:{student.ID}");
                writer.WriteLine($"Marks: {string.Join(",", student.Marks)}");
            }
        }

        public override Green_3.Student DeserializeGreen3Student(string fileName)
        {
            string filePath =  Path.Combine(FolderPath, fileName + "." + Extension);

            using (StreamReader reader = new StreamReader(filePath))
            {
                string name = reader.ReadLine().Split(':')[1].Trim();
                string surname = reader.ReadLine().Split(':')[1].Trim();
                int id = int.Parse(reader.ReadLine().Split(':')[1].Trim());
                var marksLine = reader.ReadLine().Split(':')[1].Trim();
                var marks = marksLine
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();
                var restored = new Green_3.Student(name, surname, id);

                foreach (var m in marks)
                {
                    restored.Exam(m);
                }

                return restored;
            }
        }

        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            string filePath = Path.Combine(FolderPath, fileName + "." + Extension);
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            using var writer = new StreamWriter(filePath);
            writer.WriteLine($"Name: {discipline.Name}");
            writer.WriteLine(
                $"Discipline: {(discipline is Green_4.LongJump ? "LongJump" : "HighJump")}"
            );

            var participants = discipline.Participants;
            writer.WriteLine($"Count: {participants.Length}");
            writer.WriteLine("Participants:");

            foreach (var p in participants)
            {
                var jumps = p.Jumps
                             .Select(j => j.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine($"{p.Name}|{p.Surname}|{string.Join(",", jumps)}");
            }
        }

        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            string filePath = Path.Combine(FolderPath, fileName + "." + Extension);
            using var reader = new StreamReader(filePath);

            var nameLine = reader.ReadLine()!;
            var typeLine = reader.ReadLine()!;
            string name = nameLine.Split(':', 2)[1].Trim();
            string type = typeLine.Split(':', 2)[1].Trim();

            Green_4.Discipline disc = type switch
            {
                "LongJump" => new Green_4.LongJump(),
                "HighJump" => new Green_4.HighJump(),
                _ => throw new InvalidOperationException($"Unknown type '{type}'")
            };

            var countLine = reader.ReadLine()!;
            int count = int.Parse(countLine.Split(':', 2)[1].Trim());
            var header = reader.ReadLine()!;
            if (header != "Participants:")
                throw new InvalidOperationException("Expected Participants:");

            for (int i = 0; i < count; i++)
            {
                var line = reader.ReadLine()!;
                var parts = line.Split('|');
                string nm = parts[0];
                string sr = parts[1];

                var jumps = parts[2]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => double.Parse(x, CultureInfo.InvariantCulture))
                    .ToArray();

                var p = new Green_4.Participant(nm, sr);
                foreach (var j in jumps)
                    p.Jump(j);

                disc.Add(p);
            }

            return disc;
        }

        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            string path = Path.Combine(FolderPath, fileName + "." + Extension);

            var real = (group.Students ?? Array.Empty<Green_5.Student>())
                       .Where(s => s.Marks != null)
                       .ToArray();

            double avg = real.Length > 0 ? real.Average(s => s.AvgMark) : 0;

            using var w = new StreamWriter(path);

            w.WriteLine($"Type: {group.GetType().Name}");
            w.WriteLine($"Name: {group.Name}");
            w.WriteLine($"AverageMark: {avg}");
            w.WriteLine($"Count: {real.Length}");
            w.WriteLine("Students:");
            foreach (var s in real)
                w.WriteLine($"{s.Name}|{s.Surname}|{string.Join(",", s.Marks)}");
        }

        public override T DeserializeGreen5Group<T>(string fileName)
        {
            string path = Path.Combine(FolderPath, fileName + "." + Extension);
            using var r = new StreamReader(path);


            string typeLine = r.ReadLine()!;
            string nameLine = r.ReadLine()!;
            r.ReadLine();                   

            string typeName = typeLine.Split(':', 2)[1].Trim();
            string groupName = nameLine.Split(':', 2)[1].Trim();

            int count = int.Parse(r.ReadLine()!.Split(':', 2)[1].Trim());
            r.ReadLine();

            Type t = typeName switch
            {
                nameof(Green_5.EliteGroup) => typeof(Green_5.EliteGroup),
                nameof(Green_5.SpecialGroup) => typeof(Green_5.SpecialGroup),
                _ => typeof(Green_5.Group)
            };

            var group = (T)Activator.CreateInstance(t, groupName)!;

            for (int i = 0; i < count; i++)
            {
                var parts = r.ReadLine()!.Split('|');
                var stud = new Green_5.Student(parts[0], parts[1]);
                foreach (var m in parts[2]
                              .Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(int.Parse))
                {
                    stud.Exam(m);
                }

                ((Green_5.Group)(object)group).Add(stud);
            }

            return group;
        }
    }
}
