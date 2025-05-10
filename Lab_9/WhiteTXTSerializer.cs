using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_9;
using Lab_7;

namespace Lab_9 {
    public class WhiteTXTSerializer : WhiteSerializer {
        public override string Extension => "txt";

        public override void SerializeWhite1Participant(White_1.Participant participant, string fileName) {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath)) {
                writer.WriteLine($"Surname: {participant.Surname}");
                writer.WriteLine($"Club: {participant.Club}");
                writer.WriteLine($"FirstJump: {participant.FirstJump}");
                writer.WriteLine($"SecondJump: {participant.SecondJump}");        
            }
        }
        public override void SerializeWhite2Participant(White_2.Participant participant, string fileName) {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath)) {
                writer.WriteLine($"Name: {participant.Name}");
                writer.WriteLine($"Surname: {participant.Surname}");
                writer.WriteLine($"FirstJump: {participant.FirstJump}");
                writer.WriteLine($"SecondJump: {participant.SecondJump}");             
            }
        }
        public override void SerializeWhite3Student(White_3.Student student, string fileName) {
            if (student == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath)) {
                writer.WriteLine($"Type: {student.GetType().Name}");
                writer.WriteLine($"Name: {student.Name}");
                writer.WriteLine($"Surname: {student.Surname}");
                int[] newArray = new int[student.Marks.Length];
                Array.Copy(student.Marks, newArray, student.Marks.Length);
                writer.WriteLine($"Marks: {String.Join(",", newArray)}");
                writer.WriteLine($"Skipped: {student.Skipped}");
            }
        }
        public override void SerializeWhite4Human(White_4.Human human, string fileName) {
            if (human == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath)) {
                writer.WriteLine($"Type: {human.GetType().Name}");
                writer.WriteLine($"Name: {human.Name}");
                writer.WriteLine($"Surname: {human.Surname}");
                if (human is White_4.Participant participant) {
                    double[] newArray = new double[participant.Scores.Length];
                    if (participant.Scores.Length == 0) {
                        writer.WriteLine($"Scores: {String.Join(",", newArray)}");
                    } else {
                        Array.Copy(participant.Scores, newArray, participant.Scores.Length);
                        writer.WriteLine($"Scores: {String.Join(",", newArray)}");
                    }
                }
            }
        }

        public override void SerializeWhite5Team(White_5.Team team, string fileName) {
            if (team == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath)) {
                writer.WriteLine($"Type: {team.GetType().Name}");
                writer.WriteLine($"Name: {team.Name}");
                for (int i = 0; i < team.Matches.Length; i++) {
                    writer.WriteLine($"Match{i + 1}: {team.Matches[i].Goals}-{team.Matches[i].Misses}");
                }
            }
        }

        public override White_1.Participant DeserializeWhite1Participant(string fileName) {
            SelectFile(fileName);
            Dictionary<string, string> reader = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines) {
                if (line.Contains(":")) {
                    var parts = line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    reader[parts[0]] = parts[1];
                }
            }

            White_1.Participant deserialized = new White_1.Participant(reader["Surname"], reader["Club"]);
            deserialized.Jump(Double.Parse(reader["FirstJump"]));
            deserialized.Jump(Double.Parse(reader["SecondJump"]));
            return deserialized;
        }
        public override White_2.Participant DeserializeWhite2Participant(string fileName) {
            SelectFile(fileName);
            Dictionary<string, string> reader = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines) {
                if (line.Contains(":")) {
                    var parts = line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    reader[parts[0]] = parts[1];
                }
            }

            White_2.Participant deserialized = new White_2.Participant(reader["Name"], reader["Surname"]);
            deserialized.Jump(Double.Parse(reader["FirstJump"]));
            deserialized.Jump(Double.Parse(reader["SecondJump"]));
            return deserialized;
        }
        public override White_3.Student DeserializeWhite3Student(string fileName) {
            SelectFile(fileName);
            Dictionary<string, string> reader = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines) {
                if (line.Contains(":")) {
                    var parts = line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    reader[parts[0]] = parts[1];
                }
            }

            White_3.Student deserialized;
            if (reader["Type"] == "Undergraduate") {
                deserialized = new White_3.Undergraduate(reader["Name"], reader["Surname"]);
            } else {
                deserialized = new White_3.Student(reader["Name"], reader["Surname"]);
            }

            int skipped = Int32.Parse(reader["Skipped"]);
            for (int i = 0; i < skipped; i++) {
                deserialized.Lesson(0);
            }

            string[] stringMarks = reader["Marks"].Split(',');
            foreach (var mark in stringMarks) {
                int intMark = Int32.Parse(mark);
                deserialized.Lesson(intMark);
            }
            return deserialized;
        }
        public override White_4.Human DeserializeWhite4Human(string fileName) {
            SelectFile(fileName);
            Dictionary<string, string> reader = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines) {
                if (line.Contains(":")) {
                    var parts = line.Split(':');
                    reader[parts[0].Trim()] = parts[1].Trim();
                }
            }

            White_4.Human deserialized;
            if (reader["Type"] == "Participant") {
                deserialized = new White_4.Participant(reader["Name"], reader["Surname"]);
            } else {
                deserialized = new White_4.Human(reader["Name"], reader["Surname"]);
            }

            if (reader.ContainsKey("Scores")) {
                foreach (string score in reader["Scores"].Split(',')) {
                    if (!String.IsNullOrEmpty(score)) {
                        ((White_4.Participant)deserialized).PlayMatch(Double.Parse(score));
                    }
                }
            }
            return deserialized;
        }
        
        public override White_5.Team DeserializeWhite5Team(string fileName) {
            SelectFile(fileName);
            Dictionary<string, string> reader = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines) {
                if (line.Contains(":")) {
                    var parts = line.Split(':');
                    reader[parts[0].Trim()] = parts[1].Trim();
                }
            }

            White_5.Team deserialized;
            if (reader["Type"] == "WomanTeam") {
                deserialized = new White_5.WomanTeam(reader["Name"]);
                foreach (var key in reader.Keys) {
                    if (key.Contains("Match")) {
                        int goals = Int32.Parse(reader[key].Split('-')[0]);
                        int misses = Int32.Parse(reader[key].Split('-')[1]);
                        ((White_5.WomanTeam)deserialized).PlayMatch(goals, misses);
                    }
                }
            } else {
                deserialized = new White_5.ManTeam(reader["Name"]);
                foreach (var key in reader.Keys) {
                    if (key.Contains("Match")) {
                        int goals = Int32.Parse(reader[key].Split('-')[0]);
                        int misses = Int32.Parse(reader[key].Split('-')[1]);
                        ((White_5.ManTeam)deserialized).PlayMatch(goals, misses);
                    }
                }
            }

            return deserialized;
        }
    }
}