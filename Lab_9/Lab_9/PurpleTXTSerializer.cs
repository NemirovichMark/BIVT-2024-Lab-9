using Lab_7;
using Lab_9;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";
        
        private string StringFromMatrix<T>(T[,] matrix)
        {
            var m = new StringBuilder();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    string r;
                    if (j != matrix.GetLength(1) - 1)
                    {
                        r = matrix[i, j].ToString() + ";";
                    }
                    else
                    {
                        r = matrix[i, j].ToString();
                    }
                        m.Append(r);
                }
                if (i != matrix.GetLength(0) - 1)
                {
                    m.Append("/");
                }
            }
            return m.ToString();

        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            var sText = new Dictionary<string, string>();
            using (var writer = new StreamWriter(FilePath))
            {
                if (obj is Purple_1.Participant s)
                {

                    sText["Type"] = "Participant";
                    sText["Name"] = s.Name;
                    sText["Surname"] = s.Surname;
                    sText["Coefs"] = String.Join(";", s.Coefs);
                    var matr = new StringBuilder();
                    for (int i = 0; i < s.Marks.GetLength(0); i++)
                    {
                        for (int j=0; j < s.Marks.GetLength(1); j++)
                        {
                            if (j != s.Marks.GetLength(1) - 1)
                            {
                                matr.Append(s.Marks[i, j].ToString() + ";");
                            }
                            else { 
                                matr.Append(s.Marks[i, j].ToString());
                                }
                        }
                        if (i != s.Marks.GetLength(0) - 1)
                        {
                            matr.Append('/');
                        }
                    }
                    sText["Marks"]=matr.ToString();
                    sText["TotalScore"]=s.TotalScore.ToString();
                }
                else if (obj is Purple_1.Judge p)
                {
                    sText["Type"] = "Judge";
                    sText["Name"] = p.Name;
                    sText["Marks"]=String.Join(";",p.Marks);

                }
                else if (obj is Purple_1.Competition comp)
                {
                    sText["Type"] = "Competition";
                    sText["JudgeCount"] = comp.Judges.Length.ToString();
                    for (int i = 0; i < comp.Judges.Length;i++)
                    {
                        sText[$"JudgeName-{i}"] = comp.Judges[i].Name;
                        sText[$"JudgeMarks-{i}"] = String.Join(";", comp.Judges[i].Marks);
                    }
                    sText["ParticipantCount"] = comp.Participants.Length.ToString();
                    for (int i = 0; i < comp.Participants.Length; i++)
                    {
                        sText[$"ParticipantName-{i}"] = comp.Participants[i].Name;
                        sText[$"ParticipantSurname-{i}"] = comp.Participants[i].Surname;
                        sText[$"ParticipantCoefs-{i}"] = String.Join(";",comp.Participants[i].Coefs);
                        sText[$"ParticipantMarks-{i}"] = StringFromMatrix(comp.Participants[i].Marks);
                    }
                }
                foreach (var t in sText) writer.WriteLine($"{t.Key}={t.Value}");
            }
        }
        private int[,] MatrixFromString(string s)
        {
            var Lines = s.Split('/');
            int count_row = Lines[0].Split(';').Length;
            var marks = new int[Lines.Length,count_row];
            for (int i = 0; i < Lines.Length; i++)
            {
                var line = Lines[i].Split(';');
                var row = new int[Lines[0].Length];
                for (int j = 0; j < line.Length; j++)
                {
                    marks[i,j]=int.Parse(line[j]);
                }

            }
            return marks;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var file = File.ReadAllLines(FilePath);
            Dictionary<string, string> dText = new Dictionary<string, string>();
            foreach (var line in file)
            {
                if (line.Contains("="))
                {
                    var parts = line.Split('=');
                    dText[parts[0].Trim()] = parts[1].Trim();
                }
            }
            if (dText["Type"] == "Participant")
            {
                var name = dText["Name"];
                var surname = dText["Surname"];
                double[] coefs = dText["Coefs"].Split(';').Select(x => double.Parse(x)).ToArray();
                var part = new Purple_1.Participant(name, surname);
                part.SetCriterias(coefs);
                var marks = MatrixFromString(dText["Marks"]);
                for (int i = 0; i < marks.GetLength(0); i++)
                {
                    var row = new int[marks.GetLength(1)];
                    for (int j = 0; j < marks.GetLength(1); j++)
                    {
                        row[j] = marks[i, j];
                    }
                    part.Jump(row);

                }
                return part as T;
            }
            else if (dText["Type"] == "Judge")
            {
                var name = dText["Name"];
                var marks = dText["Marks"].Split(';').Select(x => int.Parse(x)).ToArray();
                var j = new Purple_1.Judge(name, marks);
                return j as T;
            }
            else
            {
                int judgecount = int.Parse(dText["JudgeCount"]);
                var j = new Purple_1.Judge[judgecount];
                for (int i = 0; i < judgecount; i++)
                {
                    var judgeName = dText[$"JudgeName-{i}"];
                    var judgeMarks = dText[$"JudgeMarks-{i}"].Split(';').Select(x => int.Parse(x)).ToArray();
                    j[i] = new Purple_1.Judge(judgeName, judgeMarks);
                }
                var comp = new Purple_1.Competition(j);
                int partcount = int.Parse(dText["ParticipantCount"]);
                var part = new Purple_1.Participant[partcount];
                for (int i = 0; i < partcount; i++)
                {
                    var partName = dText[$"ParticipantName-{i}"];
                    var partsur = dText[$"ParticipantSurname-{i}"];
                    var partcoefs = dText[$"ParticipantCoefs-{i}"].Split(';').Select(x => double.Parse(x)).ToArray();
                    var p = new Purple_1.Participant(partName, partsur);
                    p.SetCriterias(partcoefs);
                    var partMarks = MatrixFromString(dText[$"ParticipantMarks-{i}"]);
                    for (int k = 0; k < partMarks.GetLength(0); k++)
                    {
                        var m = new int[partMarks.GetLength(1)];
                        for (int l = 0; l < partMarks.GetLength(1); l++)
                        {
                            m[l] = partMarks[k, l];
                        }
                        p.Jump(m);
                    }
                    comp.Add(p);
                }
                return comp as T;
            }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var sText = new Dictionary<string, string>();
            using (var writer = new StreamWriter(FilePath))
            {
                sText["Type"] = jumping is Purple_2.JuniorSkiJumping ? "juniorSkiJumping" : "ProSkiJumping";
                sText["Name"] = jumping.Name;
                sText["Standard"] = jumping.Standard.ToString();
                var partCount = jumping.Participants;
                sText["partCount"] = partCount.Length.ToString();
                for (int i = 0; i < partCount.Length; i++)
                {
                    sText[$"partName-{i}"] = partCount[i].Name;
                    sText[$"partSurname-{i}"] = partCount[i].Surname;
                    sText[$"partDist-{i}"] = partCount[i].Distance.ToString();
                    sText[$"partResult-{i}"] = partCount[i].Result.ToString();
                    sText[$"partMarks-{i}"] = String.Join(";", partCount[i].Marks);
                }
                foreach (var t in sText) writer.WriteLine($"{t.Key}={t.Value}");
            }

        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            Dictionary<string, string> dText = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                if (line.Contains("="))
                {
                    var parts = line.Split('=');
                    dText[parts[0].Trim()] = parts[1].Trim();
                }
            }
            Purple_2.SkiJumping s;
            int standart = int.Parse(dText["Standard"]);
            if (standart == 100)
            {
                s = new Purple_2.JuniorSkiJumping();
            }
            else s = new Purple_2.ProSkiJumping();
            string type = dText["Type"];
            string names = dText["Name"];

            int partcount = int.Parse(dText["partCount"]);
            for (int i = 0; i < partcount; i++)
            {
                string name = dText[$"partName-{i}"];
                string surname = dText[$"partSurname-{i}"];
                int distance = int.Parse(dText[$"partDist-{i}"]);
                string result = dText[$"partResult-{i}"];
                int[] marks = dText[$"partMarks-{i}"].Split(';').Select(x => int.Parse(x)).ToArray();
                Purple_2.Participant part = new Purple_2.Participant(name, surname);
                part.Jump(distance, marks, standart);
                s.Add(part);
            }
            return s as T;
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var sText = new Dictionary<string, string>();
            using (var writer = new StreamWriter(FilePath))
            {
                if (skating is Purple_3.IceSkating)
                {
                    sText["Type"] = "IceSkating";
                }
                else sText["Type"] = "FigureSkating";
                sText["Moods"] = String.Join(";", skating.Moods);
                var part = skating.Participants;
                sText["partcount"] = part.Length.ToString();
                for (int i = 0; i < part.Length; i++)
                {
                    sText[$"partName-{i}"] = part[i].Name;
                    sText[$"partSurname-{i}"] = part[i].Surname;
                    sText[$"partMarks-{i}"] = String.Join(";", part[i].Marks);
                    sText[$"places-{i}"] = String.Join(";", part[i].Places);
                }
                foreach (var t in sText) writer.WriteLine($"{t.Key}={t.Value}");
            }
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            Dictionary<string, string> dText = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                if (line.Contains("="))
                {
                    var parts = line.Split('=');
                    dText[parts[0].Trim()] = parts[1].Trim();
                }
            }
            var moods = dText["Moods"].Split(';').Select(x => double.Parse(x)).ToArray();
            Purple_3.Skating s;
            if (dText["Type"] == "IceSkating")
            {
                s = new Purple_3.IceSkating(moods, false);
            }
            else if (dText["Type"] == "FigureSkating")
            {
                s = new Purple_3.FigureSkating(moods, false);
            }
            else return null;
            Purple_3.Participant part = new Purple_3.Participant();
            int partcount = int.Parse(dText["partcount"]);
            for (int i = 0; i < partcount; i++)
            {
                var name = dText[$"partName-{i}"];
                var surname = dText[$"partSurname-{i}"];
                var marks = dText[$"partMarks-{i}"].Trim().Split(';').Select(x => double.Parse(x)).ToArray();
                part = new Purple_3.Participant(name, surname);
                
                foreach (var n in marks) { part.Evaluate(n);}
                s.Add(part);
            }
            Purple_3.Participant.SetPlaces(s.Participants);
            return s as T;
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            var sText = new Dictionary<string, string>();
            using (var writer = new StreamWriter(FilePath))
            {
                sText["Type"] = "Group";
                sText["Name"] = group.Name;
                var sportsms = group.Sportsmen;
                sText["count_sportsm"] = sportsms.Length.ToString();
                for (int i = 0; i < sportsms.Length; i++)
                {
                    sText[$"Name_s-{i}"] = sportsms[i].Name;
                    sText[$"Surname_s-{i}"] = sportsms[i].Surname;
                    sText[$"Time_s-{i}"] = sportsms[i].Time.ToString();
                }

                foreach (var t in sText) writer.WriteLine($"{t.Key}={t.Value}");
            }
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            Dictionary<string, string> dText = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                if (line.Contains("="))
                {
                    var parts = line.Split('=');
                    dText[parts[0].Trim()] = parts[1].Trim();
                }
            }
            var type = dText["Type"];
            var name_g = new Purple_4.Group(dText["Name"]);
            var count_sportsm = int.Parse(dText["count_sportsm"]);
            for (int i = 0; i < count_sportsm; i++)
            {
                var name = dText[$"Name_s-{i}"];
                var surname = dText[$"Surname_s-{i}"];
                var time = double.Parse(dText[$"Time_s-{i}"]);
                var part = new Purple_4.Sportsman(name, surname);
                part.Run(time);
                name_g.Add(part);
            }
            return name_g;
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            var sText = new Dictionary<string, string>();
            using (var writer = new StreamWriter(FilePath)) 
            { 
            
                sText["r_count"] = report.Researches.Length.ToString();
                for (int j=0;j<report.Researches.Length;j++)
                {
                    var researches=report.Researches[j];
                    sText[$"name{j}"] = researches.Name;
                    sText[$"count{j}"] = researches.Responses.Length.ToString();
                    for(int i = 0; i < researches.Responses.Length; i++)
                    {
                        var r = researches.Responses[i];
                        sText[$"Animal{i}_{j}"]=r.Animal;
                        sText[$"CharacterTrait{i}_{j}"]=r.CharacterTrait;
                        sText[$"Concept{i}_{j}"] = r.Concept;
                    }
                foreach (var t in sText) writer.WriteLine($"{t.Key}={t.Value}");
                }

            }
        }

       
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            Dictionary<string, string> dText = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                if (line.Contains("="))
                {
                    var parts = line.Split('=');
                    dText[parts[0].Trim()] = parts[1].Trim();
                }
            }
            var rep = new Purple_5.Report();
            var r_count = int.Parse(dText["r_count"]);
            for (int i = 0; i < r_count; i++)
            {
                var res = new Purple_5.Research(dText[$"name{i}"]);
                
                var count = int.Parse(dText[$"count{i}"]);
                for (int j = 0; j < count; j++)
                {
                    var a = dText[$"Animal{j}_{i}"] == "" ? null : dText[$"Animal{j}_{i}"];
                    var ct = dText[$"CharacterTrait{j}_{i}"] == "" ? null : dText[$"CharacterTrait{j}_{i}"];
                    var c = dText[$"Concept{j}_{i}"] == "" ? null : dText[$"Concept{j}_{i}"];
                    res.Add([a,ct,c]);
                }

              rep.AddResearch(res);
  

            }
            return rep;
        }


    }

}

