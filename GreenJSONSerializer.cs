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
        private string GetFilePath(string fileName) =>
           Path.Combine(FolderPath, $"{fileName}.{Extension}");

        public override void SerializeGreen1Participant(Green_1.Participant p, string fileName)
        {
            SelectFile(fileName);

            var dict = new Dictionary<string, object>
            {
                ["Surname"] = p.Surname,
                ["Group"] = p.Group,
                ["Trainer"] = p.Trainer,
                ["Result"] = p.Result,
                ["Discipline"] = p is Green_1.Participant100M ? "100M" : "500M"
            };

            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {
            SelectFile(fileName);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(FilePath));

            string surname = dict["Surname"].ToString();
            string group = dict["Group"].ToString();
            string trainer = dict["Trainer"].ToString();
            double result = Convert.ToDouble(dict["Result"]);
            string discipline = dict["Discipline"].ToString();

            Green_1.Participant p = discipline == "100M"
                ? new Green_1.Participant100M(surname, group, trainer)
                : (Green_1.Participant)new Green_1.Participant500M(surname, group, trainer);

            p.Run(result);
            return p;
        }

        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            SelectFile(fileName);

            var dict = new Dictionary<string, object>
            {
                ["Type"] = human is Green_2.Student ? "Student" : "Human",
                ["Name"] = human.Name,
                ["Surname"] = human.Surname
            };

            if (human is Green_2.Student st)
                dict["Marks"] = st.Marks;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override Green_2.Human DeserializeGreen2Human(string fileName)
        {
            SelectFile(fileName);

            var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(FilePath));
            string type = dict["Type"].ToString();
            string name = dict["Name"].ToString();
            string surname = dict["Surname"].ToString();

            if (type == "Student")
            {
                var marksRaw = dict["Marks"] as Newtonsoft.Json.Linq.JArray;
                int[] marks = marksRaw.Select(m => (int)m).ToArray();
                var st = new Green_2.Student(name, surname);
                foreach (var m in marks)
                    st.Exam(m);
                return st;
            }
            else
            {
                return new Green_2.Human(name, surname);
            }
        }

        public override void SerializeGreen3Student(Green_3.Student student, string fileName)
        {
            SelectFile(fileName);

            var dict = new Dictionary<string, object>
            {
                ["Name"] = student.Name,
                ["Surname"] = student.Surname,
                ["ID"] = student.ID,
                ["Marks"] = student.Marks,
                ["IsExpelled"] = student.IsExpelled
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override Green_3.Student DeserializeGreen3Student(string fileName)
        {
            SelectFile(fileName);

            var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(FilePath));
            string name = dict["Name"].ToString();
            string surname = dict["Surname"].ToString();
            int id = Convert.ToInt32(dict["ID"]);
            var marksRaw = dict["Marks"] as Newtonsoft.Json.Linq.JArray;
            int[] marks = marksRaw.Select(m => (int)m).ToArray();

            var st = new Green_3.Student(name, surname, id);
            foreach (var m in marks)
                st.Exam(m);
            return st;
        }

        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            SelectFile(fileName);

            var dict = new Dictionary<string, object>
            {
                ["Type"] = discipline is Green_4.LongJump ? "LongJump" : "HighJump",
                ["Name"] = discipline.Name,
                ["Participants"] = discipline.Participants.Select(p => new Dictionary<string, object>
                {
                    ["Name"] = p.Name,
                    ["Surname"] = p.Surname,
                    ["Jumps"] = p.Jumps
                }).ToArray()
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            SelectFile(fileName);

            var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(FilePath));
            string type = dict["Type"].ToString();
            string name = dict["Name"].ToString();
            Green_4.Discipline disc = type == "LongJump" ? new Green_4.LongJump() : new Green_4.HighJump();

            var participantsRaw = dict["Participants"] as Newtonsoft.Json.Linq.JArray;
            foreach (var pRaw in participantsRaw)
            {
                var pDict = pRaw.ToObject<Dictionary<string, object>>();
                string pname = pDict["Name"].ToString();
                string psurname = pDict["Surname"].ToString();
                double[] jumps = ((Newtonsoft.Json.Linq.JArray)pDict["Jumps"]).Select(j => (double)j).ToArray();

                var p = new Green_4.Participant(pname, psurname);
                foreach (var jump in jumps)
                    p.Jump(jump);
                disc.Add(p);
            }
            return disc;
        }
        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            SelectFile(fileName);

            var dict = new Dictionary<string, object>
            {
                ["Type"] = group.GetType().Name,
                ["Name"] = group.Name,
                ["Students"] = group.Students.Select(s => new Dictionary<string, object>
                {
                    ["Name"] = s.Name,
                    ["Surname"] = s.Surname,
                    ["Marks"] = s.Marks
                }).ToArray()
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override T DeserializeGreen5Group<T>(string fileName)
        {
            SelectFile(fileName);

            var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(FilePath));
            string type = dict["Type"].ToString();
            string name = dict["Name"].ToString();
            var groupType = Type.GetType($"Lab_7.Green_5+{type}, Lab_7");
            if (groupType == null)
                throw new InvalidOperationException($"Тип 'Lab_7.Green_5+{type}' не найден.");

            var group = (Green_5.Group)Activator.CreateInstance(groupType, name);


            var studentsRaw = dict["Students"] as JArray;
            foreach (var sRaw in studentsRaw)
            {
                var sDict = sRaw.ToObject<Dictionary<string, object>>();
                string sname = sDict["Name"].ToString();
                string ssurname = sDict["Surname"].ToString();
                int[] marks = ((JArray)sDict["Marks"]).Select(m => (int)m).ToArray();
                var student = new Green_5.Student(sname, ssurname);
                foreach (var mark in marks)
                    student.Exam(mark);
                group.Add(student);
            }
            return (T)group;
        }
    }
}