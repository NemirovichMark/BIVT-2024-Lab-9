using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Lab_7;
using static Lab_7.Green_4;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Reflection;

namespace Lab_9
{
    public class GreenJSONSerializer : GreenSerializer
    {
        public override string Extension => "json";

        //Green_1
        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            var data = new
            {
                participant.Surname,
                participant.Group,
                participant.Trainer,
                participant.Result,
                Discipline = participant is Green_1.Participant100M ? "100M" : "500M"
            };

            File.WriteAllText(fullPath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }
        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {
            var json = File.ReadAllText(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var data = JsonConvert.DeserializeObject<dynamic>(json);

            Green_1.Participant participant = data.Discipline == "100M"
                ? new Green_1.Participant100M((string)data.Surname, (string)data.Group, (string)data.Trainer)
                : new Green_1.Participant500M((string)data.Surname, (string)data.Group, (string)data.Trainer);
            participant.Run((double)data.Result);

            return participant;
        }

        //Green_2
        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            if (human is Green_2.Student student)
            {
                File.WriteAllText(fullPath, JsonConvert.SerializeObject(new
                {
                    student.Name,
                    student.Surname,
                    student.Marks
                }, Formatting.Indented));
            }
            else
            {
                File.WriteAllText(fullPath, JsonConvert.SerializeObject(new
                {
                    human.Name,
                    human.Surname
                }, Formatting.Indented));
            }
        }
        public override Green_2.Human DeserializeGreen2Human(string fileName) 
        {
            var json = File.ReadAllText(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var data = JObject.Parse(json);

            if (data["Marks"] != null)
            {
                var student = new Green_2.Student((string)data["Name"], (string)data["Surname"]);
                foreach (var mark in data["Marks"])
                {
                    student.Exam((int)mark);
                }
                return student;
            }
            return new Green_2.Human((string)data["Name"], (string)data["Surname"]);
        }

        //Green_3
        public override void SerializeGreen3Student(Green_3.Student student, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            File.WriteAllText(fullPath, JsonConvert.SerializeObject(new
            {
                student.Name,
                student.Surname,
                student.ID,
                student.Marks
            }, Formatting.Indented));
        }
        public override Green_3.Student DeserializeGreen3Student(string fileName)
        {
            var json = File.ReadAllText(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var data = JObject.Parse(json);

            var student = new Green_3.Student(
                (string)data["Name"],
                (string)data["Surname"],
                (int)data["ID"]);
            foreach (var mark in data["Marks"])
            {
                student.Exam((int)mark);
            }
            return student;
        }

        //Green_4
        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            var data = new
            {
                Type = discipline is LongJump ? "LongJump" : "HighJump",
                discipline.Name,
                Participants = discipline.Participants.Select(p => new
                {
                    p.Name,
                    p.Surname,
                    p.Jumps
                })
            };

            File.WriteAllText(fullPath, JsonConvert.SerializeObject(data, Formatting.Indented));
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

        //Green_5
        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            var data = new
            {
                Type = group switch
                {
                    Green_5.EliteGroup _ => "EliteGroup",
                    Green_5.SpecialGroup _ => "SpecialGroup",
                    _ => "Group"
                },
                group.Name,
                Students = group.Students.Select(s => new
                {
                    s.Name,
                    s.Surname,
                    s.Marks
                })
            };

            File.WriteAllText(fullPath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public override T DeserializeGreen5Group<T>(string fileName)
        {
            var json = File.ReadAllText(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var data = JObject.Parse(json);

            Green_5.Group group = data["Type"].ToString() switch
            {
                "EliteGroup" => new Green_5.EliteGroup((string)data["Name"]),
                "SpecialGroup" => new Green_5.SpecialGroup((string)data["Name"]),
                _ => new Green_5.Group((string)data["Name"])
            };
            foreach (var student in data["Students"])
            {
                var s = new Green_5.Student(
                    (string)student["Name"],
                    (string)student["Surname"]);

                foreach (var mark in student["Marks"])
                {
                    s.Exam((int)mark);
                }

                group.Add(s);
            }
            return (T)(object)group;
        }
    }
}