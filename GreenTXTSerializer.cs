using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using Lab_9;
using System.Xml.Serialization;
using System.Reflection.Metadata;
using System.Formats.Asn1;
using System.Globalization;

namespace Lab_9 {
    public class GreenTXTSerializer : GreenSerializer {
        public override string Extension =>"txt";
        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) { return; }

            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using (StreamWriter writer = File.AppendText(FilePath)) 
            {
                writer.WriteLine($"Type: {participant.GetType().Name}");
                writer.WriteLine($"Surname: {participant.Surname}");
                writer.WriteLine($"Group: {participant.Group}");
                writer.WriteLine($"Trainer: {participant.Trainer}");
                writer.WriteLine($"Result: {participant.Result}");
            }  
        }
        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {   
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            Dictionary<string, string> participantData = new Dictionary<string, string>();
            foreach (var row in text.Split(Environment.NewLine)) {
                if (row.Contains(':')) {
                    var field = row.Split(':');
                    participantData[field[0].Trim()] = field[1].Trim();
                }
            }

            Green_1.Participant deserialized = default(Green_1.Participant);
            string surname = participantData["Surname"];
            string group = participantData["Group"];
            string trainer = participantData["Trainer"];
            double result = double.Parse(participantData["Result"]);

            switch (participantData["Type"])
            {
                case "Participant":
                    deserialized = new Green_1.Participant(surname, group, trainer);
                    break;
                case "Participant100M":
                    deserialized = new Green_1.Participant100M(surname, group, trainer);
                    break;
                case "Participant500M":
                    deserialized = new Green_1.Participant500M(surname, group, trainer);
                    break;
                default:
                    deserialized = null;
                    break;
            }

            if (deserialized != null && result > 0)
            {
                deserialized.Run(result);
            }

            return deserialized;
        }
        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            if (human == null || String.IsNullOrEmpty(fileName)) {return;}

            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Type: {human.GetType().Name}");
                writer.WriteLine($"Name: {human.Name}");
                writer.WriteLine($"Surname: {human.Surname}");
                
                if (human is Green_2.Student student)
                {
                    writer.WriteLine($"Marks: {string.Join(",", student.Marks ?? Array.Empty<int>())}");
                }
            }
        }
        public override Green_2.Human DeserializeGreen2Human(string fileName)
        {
            SelectFile(fileName);

            string[] lines = File.ReadAllLines(FilePath);
            var humanData = new Dictionary<string, string>();

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                    humanData[parts[0].Trim()] = parts[1].Trim();
            }

            string type = humanData["Type"];
            string name = humanData["Name"];
            string surname = humanData["Surname"];

            Green_2.Human deserialized = default(Green_2.Human);

            if (type == "Student")
            {
                deserialized = new Green_2.Student(name, surname);
                
                int[] marks = humanData["Marks"].Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();

                foreach (var m in marks)
                    ((Green_2.Student)deserialized).Exam(m);
            }
            else
            {
                deserialized = new Green_2.Human(name, surname);
            }

            return deserialized;
        }
        public override void SerializeGreen3Student(Green_3.Student student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName))
            return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Name: {student.Name}");
                writer.WriteLine($"Surname: {student.Surname}");
                writer.WriteLine($"ID: {student.ID}");
                writer.WriteLine($"Marks: {string.Join(",", student.Marks ?? Array.Empty<int>())}");
            }
        }
        public override Green_3.Student DeserializeGreen3Student(string fileName)
        {
            SelectFile(fileName);

            string[] lines = File.ReadAllLines(FilePath);
            var studentData = new Dictionary<string, string>();

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                    studentData[parts[0].Trim()] = parts[1].Trim();
            }

            string name = studentData["Name"];
            string surname = studentData["Surname"];
            int id = int.Parse(studentData["ID"]);

            var student = new Green_3.Student(name, surname, id);

            int[] marks = studentData["Marks"].Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
            foreach(var m in marks)
            {
                student.Exam(m);
            }

            return student;
        }
        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            if (discipline == null || string.IsNullOrEmpty(fileName))
                return;

            SelectFile(fileName);
            
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine($"Discipline: {(discipline is Green_4.LongJump ? "LongJump" : "HighJump")}");
                writer.WriteLine($"Name: {discipline.Name}");

                var participants = discipline.Participants;
                writer.WriteLine($"Count: {participants.Length}");
                writer.WriteLine("Participants:");
                
                foreach (var p in participants)
                {
                    string jumpsStr = p.Jumps != null ? string.Join(",", p.Jumps.Select(j => j.ToString(CultureInfo.InvariantCulture))): "";
                    
                    writer.WriteLine($"{p.Name}|{p.Surname}|{jumpsStr}");
                }
            }
        }

        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            SelectFile(fileName);

            var lines = File.ReadAllLines(FilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            var type = GetValueFromLine(lines[0], "Discipline");
            var name = GetValueFromLine(lines[1], "Name");
            var count = int.Parse(GetValueFromLine(lines[2], "Count"));

            Green_4.Discipline discipline = default(Green_4.Discipline);
            switch (type)
            {
                case "LongJump":
                    discipline = new Green_4.LongJump();
                    break;
                case "HighJump":
                    discipline = new Green_4.HighJump();
                    break;
            }

            for (int i = 4; i < 4 + count && i < lines.Length; i++)
            {
                var parts = lines[i].Split('|');

                var participant = new Green_4.Participant(parts[0], parts[1]);

                var jumps = parts[2].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray();

                foreach (var j in jumps)
                    participant.Jump(j);

                discipline.Add(participant);
            }

            return discipline;
        }

        private string GetValueFromLine(string line, string key)
        {
            var parts = line.Split(':', 2);
            if (parts.Length != 2 || parts[0].Trim() != key)
                throw new InvalidDataException($"Invalid {key} line format");
            return parts[1].Trim();
        }

        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName))
                return;

            SelectFile(fileName);
            
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine($"GroupType: {group.GetType().Name}");
                writer.WriteLine($"Name: {group.Name}");
                writer.WriteLine($"StudentCount: {group.Students.Length}");
                writer.WriteLine("Students:");
                
                foreach (var student in group.Students)
                {
                    string marksStr = student.Marks != null ? string.Join(",", student.Marks.Select(m => m.ToString())): "";
                    writer.WriteLine($"{student.Name}|{student.Surname}|{marksStr}");
                }
            }
        }

        public override T DeserializeGreen5Group<T>(string fileName)
        {
            SelectFile(fileName);

            var lines = File.ReadAllLines(FilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            var type = GetValueFromLine(lines[0], "GroupType");
            var name = GetValueFromLine(lines[1], "Name");
            var count = int.Parse(GetValueFromLine(lines[2], "StudentCount"));

            Green_5.Group group = default(Green_5.Group);
            switch (type)
            {
                case "Group":
                    group = new Green_5.Group(name);
                    break;
                case "EliteGroup":
                    group = new Green_5.EliteGroup(name);
                    break;
                case "SpecialGroup":
                    group = new Green_5.SpecialGroup(name);
                    break;
            }

            for (int i = 4; i < 4 + count && i < lines.Length; i++)
            {
                var parts = lines[i].Split('|');
                if (parts.Length < 2)
                    continue;

                var student = new Green_5.Student(parts[0], parts[1]);

                var marks = parts[2].Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                foreach (var mark in marks)
                    student.Exam(mark);

                group.Add(student);
            }

            return (T)(object)group;
        }
    }
}