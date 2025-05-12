using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Lab_7;
using static Lab_7.Green_4;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Lab_9
{
    public class GreenJSONSerializer : GreenSerializer
    {
        public override string Extension => "json";

        public override void SerializeGreen1Participant(Green_1.Participant p, string fileName)
        {
            Directory.CreateDirectory(FolderPath);

            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            var dto = new
            {
                p.Surname,
                p.Group,
                p.Trainer,
                p.Result,
                Discipline = p is Green_1.Participant100M ? "100M" : "500M"
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(dto, Formatting.Indented));
        }

        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            string json = File.ReadAllText(filePath);

            var j = JsonConvert.DeserializeObject<dynamic>(json)
                    ?? throw new InvalidOperationException("JSON is null");

            string surname = j.Surname;
            string group = j.Group;
            string trainer = j.Trainer;
            double result = j.Result;
            string disc = j.Discipline;

            Green_1.Participant p = disc == "100M"
                ? new Green_1.Participant100M(surname, group, trainer)
                : (Green_1.Participant)new Green_1.Participant500M(surname, group, trainer);

            p.Run(result);
            return p;
        }

        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            Directory.CreateDirectory(FolderPath);

            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            object dto = human switch
            {
                Green_2.Student st => new
                {
                    st.Name,
                    st.Surname,
                    Marks = st.Marks
                },
                _ => new
                {
                    human.Name,
                    human.Surname
                }
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(dto, Formatting.Indented));
        }

        public override Green_2.Human DeserializeGreen2Human(string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            string json = File.ReadAllText(filePath);

            var jobj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json)
                      ?? throw new InvalidOperationException("Bad JSON");

            string name = jobj["Name"]?.ToString() ?? "";
            string surname = jobj["Surname"]?.ToString() ?? "";

            if (jobj.TryGetValue("Marks", out var marksToken))
            {
                var student = new Green_2.Student(name, surname);

                foreach (var m in marksToken.Values<int>())
                {
                    student.Exam(m);
                }
                return student;
            }
            else
            {
                return new Green_2.Human(name, surname);
            }
        }

        public override void SerializeGreen3Student(Green_3.Student student, string fileName)
        {
            Directory.CreateDirectory(FolderPath);

            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            var dto = new
            {
                student.Name,
                student.Surname,
                student.ID,
                Marks = student.Marks
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(dto, Formatting.Indented));
        }

        public override Green_3.Student DeserializeGreen3Student(string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            string json = File.ReadAllText(filePath);

            var jobj = JsonConvert
                .DeserializeObject<Newtonsoft.Json.Linq.JObject>(json)
                ?? throw new InvalidOperationException("Bad JSON for Student");

            string name = jobj["Name"]?.ToString() ?? "";
            string surname = jobj["Surname"]?.ToString() ?? "";
            int id = jobj["ID"]?.ToObject<int>() ?? 0;
            var marks = jobj["Marks"]?.ToObject<int[]>() ?? Array.Empty<int>();

            var student = new Green_3.Student(name, surname, id);

            foreach (int m in marks)
                student.Exam(m);

            return student;
        }

        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            Directory.CreateDirectory(FolderPath);

            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            var dto = new
            {
                Type = discipline.GetType().Name,
                Participants = discipline
                    .Participants
                    .Select(p => new {
                        p.Name,
                        p.Surname,
                        Jumps = p.Jumps
                    })
                    .ToArray()
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(dto, Formatting.Indented));
        }

        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            var json = File.ReadAllText(filePath);
            var jobj = JObject.Parse(json);

            string type = jobj["Type"]!.Value<string>();
            Green_4.Discipline d = type switch
            {
                nameof(Green_4.LongJump) => new Green_4.LongJump(),
                nameof(Green_4.HighJump) => new Green_4.HighJump(),
                _ => throw new InvalidOperationException($"Unknown type {type}")
            };

            foreach (var pj in jobj["Participants"]!.Children<JObject>())
            {
                string nm = pj["Name"]!.Value<string>();
                string sr = pj["Surname"]!.Value<string>();
                var part = new Green_4.Participant(nm, sr);

                foreach (var jump in pj["Jumps"]!.Values<double>())
                {
                    part.Jump(jump);
                }

                d.Add(part);
            }

            return d;
        }

        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            var dto = new
            {
                Type = group.GetType().Name,
                Name = group.Name,
                Students = group.Students.Select(s => new {s.Name, s.Surname, Marks = s.Marks}).ToArray()
            };

            File.WriteAllText( filePath, JsonConvert.SerializeObject(dto, Formatting.Indented));
        }

        public override T DeserializeGreen5Group<T>(string fileName)
        {
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            var j = JObject.Parse(File.ReadAllText(filePath));

            string typeName = j["Type"]!.Value<string>();
            Type grpType = typeName switch
            {
                nameof(Green_5.EliteGroup) => typeof(Green_5.EliteGroup),
                nameof(Green_5.SpecialGroup) => typeof(Green_5.SpecialGroup),
                _ => typeof(Green_5.Group)
            };

            var group = (T)Activator.CreateInstance(grpType, j["Name"]!.Value<string>())!;

            // восстановление студентов
            foreach (var sj in j["Students"]!.Children<JObject>())
            {
                string nm = sj["Name"]!.Value<string>();
                string sr = sj["Surname"]!.Value<string>();
                var student = new Green_5.Student(nm, sr);

                // восстановление оценок
                foreach (int m in sj["Marks"]!.Values<int>())
                    student.Exam(m);

                group.Add(student);
            }

            return group;
        }
    }
}
