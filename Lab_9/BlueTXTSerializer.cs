using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lab_7;

namespace Lab_9 {
    public class BlueTXTSerializer : BlueSerializer {
        public override string Extension => "txt";
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) {return;}

            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using (StreamWriter writer = File.AppendText(FilePath)) 
            {
                writer.WriteLine($"Type: {participant.GetType().Name}");
                writer.WriteLine($"Name: {participant.Name}");
                writer.WriteLine($"Votes: {participant.Votes}");
                if (participant is Blue_1.HumanResponse p) {
                    writer.WriteLine($"Surname: {p.Surname}");
                }
            }  
        }

        private void SerializeBlue2Participant(Blue_2.Participant p, StreamWriter writer) {
            writer.Write("(");
            writer.Write($"Name:{p.Name} ");
            writer.Write($"Surname:{p.Surname} ");
            string marks = "{";
            for (int i = 0; i < 2; i++) {
                marks += "[";
                for (int j = 0; j < 5; j++) {
                    marks += $"{p.Marks[i, j]},";
                }
                marks = marks.Remove(marks.Length - 1);
                marks += "],";
            }
            marks = marks.Remove(marks.Length - 1);
            marks += "}";
            writer.Write("Marks:" + marks);
            writer.Write(");");
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump wj, string fileName)
        {
            if (wj == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using (StreamWriter writer = File.AppendText(FilePath)) 
            {
                writer.WriteLine($"Type:: {wj.GetType().Name}");
                writer.WriteLine($"Name:: {wj.Name}");
                writer.WriteLine($"Bank:: {wj.Bank}");
                writer.Write("Participants::");
                foreach (Blue_2.Participant p in wj.Participants) {
                    SerializeBlue2Participant(p, writer);
                }
            }
            
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName) where T : class
        {
            if (student == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using (StreamWriter writer = File.AppendText(FilePath)) 
            {
                writer.WriteLine($"Type: {student.GetType().Name}");
                writer.WriteLine($"Name: {student.Name}");
                writer.WriteLine($"Surname: {student.Surname}");
                writer.Write("Penalties: ");
                string penalties = "[";
                foreach (var penalty in student.Penalties) {
                    penalties += $"{penalty},";
                }
                penalties = penalties.Remove(penalties.Length - 1);
                penalties += "]";
                writer.Write(penalties);
            }
        }

        private void FillTeams(Blue_4.Team team, ref string teams) {
            teams += "{";
            teams += $"Name:{team.Name} ";
            teams += "Scores:[";
            if (team.Scores != null)
                foreach (int score in team.Scores) {
                    teams += $"{score},";
                }
            teams = teams.Remove(teams.Length - 1);
            teams += "]";
            teams += "};";
        }
        
        private void SerializeManOrFemaleTeams(Blue_4.Group group, string teams_gender, StreamWriter writer) {
            writer.Write(teams_gender + ":: ");
            string teams = "<";
            if (teams_gender == "ManTeams") {
                foreach (var team in group.ManTeams) {
                    if (team != null)
                        FillTeams(team, ref teams);
                }
            }
            else {
                foreach (var team in group.WomanTeams) {
                    if (team != null)
                        FillTeams(team, ref teams);
                }
            }
            teams = teams.Length > 1 ? teams.Remove(teams.Length - 1) : teams;
            teams += ">";
            writer.WriteLine(teams);
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Name:: {participant.Name}");
                SerializeManOrFemaleTeams(participant, "ManTeams", writer);
                SerializeManOrFemaleTeams(participant, "WomanTeams", writer);
            }
        }

        private void SerializeBlue5Sportsmen(Blue_5.Team group, StreamWriter writer)
        {
            writer.Write("Sportsmen:: ");
            string text = "<";
            foreach(var sportsman in group.Sportsmen)
            {   
                if (sportsman != null) 
                {
                    text += "{";
                    text += $"Name:{sportsman.Name} ";
                    text += $"Surname:{sportsman.Surname} ";
                    text += $"Place:{sportsman.Place}";
                    text += "};";
                }
            }
            text = text.Length > 2 ? text.Remove(text.Length - 1): text;
            text += ">";
            writer.WriteLine(text);
        }

        public override void SerializeBlue5Team<T>(T group, string fileName) where T: class
        {
            if (group == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using(StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Type:: {group.GetType().Name}");
                writer.WriteLine($"Name:: {group.Name}");
                SerializeBlue5Sportsmen(group, writer);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {   
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            Dictionary<string, string> response = new Dictionary<string, string>();
            foreach (var row in text.Split(Environment.NewLine)) {
                if (row.Contains(':')) {
                    var field = row.Split(':');
                    response[field[0].Trim()] = field[1].Trim();
                }
            }
            Blue_1.Response deserialized = default(Blue_1.Response);
            if (response["Type"] == "Response") {
                deserialized = new Blue_1.Response(response["Name"], Int32.Parse(response["Votes"]));
            } 
            else {
                deserialized = new Blue_1.HumanResponse(response["Name"], response["Surname"], Int32.Parse(response["Votes"]));
            }
            return deserialized;
        }

        private void DeserializeBlue2Participants(Blue_2.WaterJump wj, Dictionary<string, string> dict, TextReader reader) {
            string row = reader.ReadLine();
            var key_val = row.Split("::");
            int count = key_val[1].Split(';').Length - 1;
            Blue_2.Participant[] participants = new Blue_2.Participant[count];
            int added = 0;
            if (count != 0) 
            {
                foreach (string p in key_val[1].Split(';')) {
                    if (p.Length > 1) {
                        Dictionary<string, string> tmp = new Dictionary<string, string>();
                        string human = p.Substring(1, p.Length - 2);
                        var splitted_human = human.Split(' ');
                        foreach (string s in splitted_human) {
                            var splitted_field = s.Split(':');
                            tmp[splitted_field[0].Trim()] = splitted_field[1].Trim();
                            if (splitted_field[0].Trim() == "Marks") {
                                participants[added++] = new Blue_2.Participant(tmp["Name"], tmp["Surname"]);
                                int first_square_backet = tmp["Marks"].IndexOf('[') + 1;
                                int second_square_backet = tmp["Marks"].IndexOf(']');
                                int third_square_backet = tmp["Marks"].IndexOf('[', first_square_backet + 1) + 1;
                                int fourth_square_backet = tmp["Marks"].IndexOf(']', second_square_backet + 1);
                                int[] first_jump = Array.ConvertAll(tmp["Marks"][first_square_backet..second_square_backet].Split(','), Int32.Parse);
                                int[] second_jump = Array.ConvertAll(tmp["Marks"][third_square_backet..fourth_square_backet].Split(','), Int32.Parse);
                                participants[added - 1].Jump(first_jump);
                                participants[added - 1].Jump(second_jump);
                            }
                        }
                    } 
                }
                wj.Add(participants);
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Blue_2.WaterJump wj = default(Blue_2.WaterJump);

            using (FileStream fs = File.OpenRead(FilePath))
            using (TextReader reader = new StreamReader(fs)) 
            {
                string row = "";
                for (int i = 0; i < 3; i++) {
                    row = reader.ReadLine();
                    if (!String.IsNullOrEmpty(row) && row.Contains("::")) {
                        var field = row.Split("::");
                        dict[field[0].Trim()] = field[1].Trim();
                    }
                }
                if (dict["Type"] == "WaterJump3m") 
                    wj = new Blue_2.WaterJump3m(dict["Name"], Int32.Parse(dict["Bank"]));
                else
                    wj = new Blue_2.WaterJump5m(dict["Name"], Int32.Parse(dict["Bank"]));
                DeserializeBlue2Participants(wj, dict, reader);
            }
            
            return wj;
        }

        public override T DeserializeBlue3Participant<T>(string fileName) where T: class
        {
            SelectFile(fileName);
            Blue_3.Participant deserialized = default(Blue_3.Participant);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            using (FileStream fs = File.OpenRead(FilePath))
            using (TextReader reader = new StreamReader(fs))
            {
                string row = "";
                for (int i = 0; i < 4; i++) {
                    row = reader.ReadLine();
                    if (row.Contains(':')) {
                        var field = row.Split(':');
                        dict[field[0].Trim()] = field[1].Trim();
                    }
                }
            }
            dict["Penalties"] = dict["Penalties"].Substring(1, dict["Penalties"].Length - 2);
            int[] penalties = Array.ConvertAll(dict["Penalties"].Split(','), Int32.Parse);
            if (dict["Type"] == "Participant")
                deserialized = new Blue_3.Participant(dict["Name"], dict["Surname"]);
            else if (dict["Type"] == "BasketballPlayer")
                deserialized = new Blue_3.BasketballPlayer(dict["Name"], dict["Surname"]);
            else
                deserialized = new Blue_3.HockeyPlayer(dict["Name"], dict["Surname"]);
            foreach (int p in penalties)
                deserialized.PlayMatch(p);
            return (T)deserialized;
        }

        private Dictionary<string, string> ProcessTeam(string processing) {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            var str_fields = processing.Split(' ');
            foreach (string s_field in str_fields) {
                var key_val = s_field.Split(':');
                fields[key_val[0].Trim()] = key_val[1].Trim();
            }
            fields["Scores"] = fields["Scores"].Substring(1, fields["Scores"].Length - 2);
            return fields;
        }

        private Blue_4.ManTeam[] DeserializeManTeams(string[] teams) {
            Blue_4.ManTeam[] des = new Blue_4.ManTeam[0];
            foreach (string team in teams) {
                if (team.Length < 2) continue;
                string processing = team.Substring(1, team.Length - 2);
                var fields = ProcessTeam(processing);
                Blue_4.ManTeam mt = new Blue_4.ManTeam(fields["Name"]);
                int[] scores = Array.ConvertAll(fields["Scores"].Split(','), Int32.Parse);
                foreach (int score in scores) {
                    mt.PlayMatch(score);
                }
                Array.Resize(ref des, des.Length + 1);
                des[des.Length - 1] = mt;
            }
            return des;
        }

        private Blue_4.WomanTeam[] DeserializeWomanTeams(string[] teams) {
            Blue_4.WomanTeam[] des = new Blue_4.WomanTeam[0];
            foreach (string team in teams) {
                if (team.Length < 2) continue;
                string processing = team.Substring(1, team.Length - 2);
                var fields = ProcessTeam(processing);
                Blue_4.WomanTeam wt = new Blue_4.WomanTeam(fields["Name"]);
                int[] scores = Array.ConvertAll(fields["Scores"].Split(','), Int32.Parse);
                foreach (int score in scores) {
                    wt.PlayMatch(score);
                }
                Array.Resize(ref des, des.Length + 1);
                des[des.Length - 1] = wt;
            }
            return des;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            Blue_4.Group deserialized = default(Blue_4.Group);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            using(FileStream fs = File.OpenRead(FilePath))
            using(TextReader reader = new StreamReader(fs)) 
            {
                string row = "";
                for (int i = 0; i < 3; i++) {
                    row = reader.ReadLine();
                    if (row.Contains("::")) {
                        var field = row.Split("::");
                        dict[field[0].Trim()] = field[1].Trim();
                    }
                }
            }
            deserialized = new Blue_4.Group(dict["Name"]);
            string[] mt_to_process = dict["ManTeams"].Substring(1, dict["ManTeams"].Length - 2).Split(';');
            string[] wt_to_process = dict["WomanTeams"].Substring(1, dict["WomanTeams"].Length - 2).Split(';');
            Blue_4.ManTeam[] manTeams_to_add = mt_to_process.Length >= 1 ? DeserializeManTeams(mt_to_process) : new Blue_4.ManTeam[0];
            Blue_4.WomanTeam[] womanTeams_to_add = wt_to_process.Length >= 1 ? DeserializeWomanTeams(wt_to_process) : new Blue_4.WomanTeam[0];
            if (manTeams_to_add.Length != 0)
                deserialized.Add(manTeams_to_add);
            if (womanTeams_to_add.Length != 0)
                deserialized.Add(womanTeams_to_add);
            return deserialized;
        }

        private Blue_5.Sportsman[] DeserializeSportsmen(string text) {
            Blue_5.Sportsman[] deserialized = new Blue_5.Sportsman[0];
            text = text.Substring(1, text.Length - 2);
            string[] str_sportsmen = text.Split(';');
            foreach (string sp in str_sportsmen) {
                if (sp.Length < 2)
                    continue;
                string processing = sp.Substring(1, sp.Length - 2);
                string[] fields = processing.Split(' ');
                Dictionary<string, string> tmp = new Dictionary<string, string>();
                foreach (string field in fields) {
                    var key_val = field.Split(':');
                    tmp[key_val[0].Trim()] = key_val[1].Trim();
                }
                Blue_5.Sportsman sp_to_add = new Blue_5.Sportsman(tmp["Name"], tmp["Surname"]);
                sp_to_add.SetPlace(Int32.Parse(tmp["Place"]));
                Array.Resize(ref deserialized, deserialized.Length + 1);
                deserialized[deserialized.Length - 1] = sp_to_add;
            }
            return deserialized;
        }

        public override T DeserializeBlue5Team<T>(string fileName) where T: class
        {
            SelectFile(fileName);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Blue_5.Team deserialized = default(Blue_5.Team);
            string row = "";
            using(FileStream fs = File.OpenRead(FilePath))
            using(TextReader reader = new StreamReader(fs)) 
            {
                for (int i = 0; i < 3; i++) {
                    row = reader.ReadLine();
                    if (row != null && row.Contains("::")) {
                        var field = row.Split("::");
                        dict[field[0].Trim()] = field[1].Trim();
                    }

                }
            }
            if (dict.Count != 0) {
                if (dict["Type"] == "ManTeam")
                    deserialized = new Blue_5.ManTeam(dict["Name"]);
                else
                    deserialized = new Blue_5.WomanTeam(dict["Name"]);
                Blue_5.Sportsman[] sportsmen = DeserializeSportsmen(dict["Sportsmen"]);
                if (sportsmen.Length > 0)
                    deserialized.Add(sportsmen);
            }
            return (T)deserialized;
        }
    }
}