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

        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            var part = new
            {
                participant.Surname,
                participant.Group,
                participant.Trainer,
                participant.Result,
                Discipline = participant is Green_1.Participant100M ? "100M" : "500M"
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(part, Formatting.Indented));
        }
        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {
            var json = File.ReadAllText(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var part = JsonConvert.DeserializeObject<dynamic>(json);

            var surname = (string)part.Surname;
            var group = (string)part.Group;
            var trainer = (string)part.Trainer;
            var result = (double)part.Result;

            Green_1.Participant participant = part.Discipline == "100M"
                ? new Green_1.Participant100M(surname, group, trainer)
                : new Green_1.Participant500M(surname, group, trainer);

            participant.Run(result);

            return participant;
        }

        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            string filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");

            var data = human is Green_2.Student student
                ? (object)new { student.Name, student.Surname, student.Marks }
                : new { human.Name, human.Surname };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }
        public override Green_2.Human DeserializeGreen2Human(string fileName)
        {
            string json = File.ReadAllText(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var data = JObject.Parse(json);

            string name = (string)data["Name"];
            string surname = (string)data["Surname"];

            if (data["Marks"] is JArray marksArray)
            {
                var student = new Green_2.Student(name, surname);
                foreach (int mark in marksArray)
                {
                    student.Exam(mark);
                }
                return student;
            }

            return new Green_2.Human(name, surname);
        }

        public override void SerializeGreen3Student(Green_3.Student student, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            string filePath = $"{FolderPath}\\{fileName}.{Extension}";
            var studentData = new
            {
                student.Name,
                student.Surname,
                student.ID,
                student.Marks
            };
            File.WriteAllText(filePath, JsonConvert.SerializeObject(studentData, Formatting.Indented));
        }
        public override Green_3.Student DeserializeGreen3Student(string fileName)
        {
            string jsonText = File.ReadAllText($"{FolderPath}/{fileName}.{Extension}");
            var jsonData = JObject.Parse(jsonText);
            var student = new Green_3.Student(
                (string)jsonData["Name"],
                (string)jsonData["Surname"],
                (int)jsonData["ID"]);
            foreach (var mark in jsonData["Marks"])
            {
                student.Exam((int)mark);
            }

            return student;
        }

        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            var result = new
            {
                Type = discipline is LongJump ? "LongJump" : "HighJump",
                discipline.Name,
                Participants = discipline.Participants.Select(p => new { p.Name, p.Surname, p.Jumps })
            };
            File.WriteAllText($"{FolderPath}/{fileName}.{Extension}", JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            string json = File.ReadAllText(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var data = JObject.Parse(json);
            Green_4.Discipline discipline;
            switch (data["Type"]!.ToString())
            {
                case "LongJump":
                    discipline = new Green_4.LongJump();
                    break;
                case "HighJump":
                    discipline = new Green_4.HighJump();
                    break;
                default:
                    return null;
            }
            foreach (var participantData in data["Participants"]!)
            {
                var participant = new Green_4.Participant(
                    participantData["Name"]!.ToString(),
                    participantData["Surname"]!.ToString()
                );
                foreach (var jump in participantData["Jumps"]!.Values<double>())
                {
                    participant.Jump(jump);
                }
                discipline.Add(participant);
            }

            return discipline;
        }

        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            Directory.CreateDirectory(FolderPath);
            string fullPath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
            string groupType = group is Green_5.EliteGroup ? "EliteGroup" :
                              group is Green_5.SpecialGroup ? "SpecialGroup" : "Group";
            var outputData = new
            {
                Type = groupType,
                group.Name,
                Students = group.Students.Select(s => new
                {
                    s.Name,
                    s.Surname,
                    s.Marks
                })
            };
            string json = JsonConvert.SerializeObject(outputData, Formatting.Indented);
            File.WriteAllText(fullPath, json);
        }

        public override T DeserializeGreen5Group<T>(string fileName)
        {
            string json = File.ReadAllText(Path.Combine(FolderPath, $"{fileName}.{Extension}"));
            var jsonData = JObject.Parse(json);

            Green_5.Group group = jsonData["Type"].ToString() switch
            {
                "EliteGroup" => new Green_5.EliteGroup(jsonData["Name"].ToString()),
                "SpecialGroup" => new Green_5.SpecialGroup(jsonData["Name"].ToString()),
                _ => new Green_5.Group(jsonData["Name"].ToString())
            };
            foreach (var studentData in jsonData["Students"])
            {
                var student = new Green_5.Student(
                    studentData["Name"].ToString(),
                    studentData["Surname"].ToString());

                foreach (var mark in studentData["Marks"])
                {
                    student.Exam((int)mark);
                }

                group.Add(student);
            }

            return (T)(object)group;
        }
    }
}