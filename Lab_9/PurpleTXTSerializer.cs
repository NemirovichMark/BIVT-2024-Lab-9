using Lab_7;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Lab_7.Purple_2;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        private void AddParticipants(Lab_7.Purple_1.Participant p, StringBuilder sb, int i = 1)
        {
            sb.AppendLine($"NameParticipant{i}:{p.Name}");
            sb.AppendLine($"SurnameParticipant{i}:{p.Surname}");
            sb.AppendLine($"TotalScoreParticipant{i}:{p.TotalScore}");
            sb.AppendLine($"CoefsParticipant{i}:{string.Join(";", p.Coefs)}");
            sb.AppendLine($"MarksParticipant{i}:{string.Join(";", p.Marks.Cast<int>())}");
        }
        private void AddJudge(Lab_7.Purple_1.Judge j, StringBuilder sb, int i = 1)
        {
            sb.AppendLine($"NameJudge{i}:{j.Name}");
            sb.AppendLine($"MarksJudge{i}:{string.Join(";", j.Marks)}");
        }
        public override string Extension => "txt";
        public override void SerializePurple1<T>(T obj, string fileName) where T : class
        {
            SelectFile(fileName);
            var sb = new StringBuilder();
            if (obj is Lab_7.Purple_1.Participant p)
            {
                AddParticipants(p, sb);
            }
            else if (obj is Lab_7.Purple_1.Judge j)
            {
                AddJudge(j, sb);
            }
            else if (obj is Lab_7.Purple_1.Competition comp)
            {
                var jud = comp.Judges;
                var parti = comp.Participants;
                sb.AppendLine($"JudgesCount:{comp.Judges.Length}");
                for (int i = 1; i <= jud.Length; i++)
                {
                    AddJudge(jud[i - 1], sb, i);
                }
                sb.AppendLine($"ParticipantsCount:{comp.Participants.Length}");
                for (int i = 1; i <= parti.Length; i++)
                {
                    AddParticipants(parti[i - 1], sb, i);
                }
            }
            File.WriteAllText(FilePath, sb.ToString());
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            var sb = new StringBuilder();
            if (jumping is Purple_2.SkiJumping ski)
            {
                sb.AppendLine($"Name:{ski.Name}");
                sb.AppendLine($"Standard:{ski.Standard}");
                var paticipations = ski.Participants;
                sb.AppendLine($"ParticipantCount:{paticipations.Length}");
                for (int i = 1; i <= paticipations.Length; i++)
                {
                    sb.AppendLine($"ParticipantName{i}:{paticipations[i - 1].Name}");
                    sb.AppendLine($"ParticipantSurname{i}:{paticipations[i - 1].Surname}");
                    sb.AppendLine($"Distance{i}:{paticipations[i - 1].Distance}");
                    sb.AppendLine($"Marks{i}:{string.Join(";", paticipations[i - 1].Marks)}");
                    sb.AppendLine($"Result{i}:{paticipations[i - 1].Result}");
                }
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, sb.ToString());
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            var sb = new StringBuilder();
            if (skating is Purple_3.Skating ski)
            {
                sb.AppendLine($"Type:{ski.GetType().Name}");
                sb.AppendLine($"Moods:{string.Join(";", ski.Moods)}");
                sb.AppendLine($"ParticipantsCount:{ski.Participants.Length}");
                for (int i = 1; i <= ski.Participants.Length; i++)
                {
                    sb.AppendLine($"SkaterName{i}:{ski.Participants[i - 1].Name}");
                    sb.AppendLine($"SkaterSurname{i}:{ski.Participants[i - 1].Surname}");
                    sb.AppendLine($"SkaterMarks{i}:{string.Join(";", ski.Participants[i - 1].Marks)}");
                }
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, sb.ToString());
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name:{participant.Name}");
            int i = 1;
            sb.AppendLine($"SportsmanCount:{participant.Sportsmen.Length}");
            foreach (Purple_4.Sportsman p in participant.Sportsmen)
            {
                sb.AppendLine($"SportsmanName{i}:{p.Name}");
                sb.AppendLine($"SportsmanSurname{i}:{p.Surname}");
                sb.AppendLine($"SportsmanTime{i}:{p.Time}");
                i++;
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, sb.ToString());
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            var sb = new StringBuilder();
            int j = 1;
            sb.AppendLine($"ResearchesCount:{group.Researches.Length}");
            foreach (var r in group.Researches)
            {
                sb.AppendLine($"ResearchName{j}:{r.Name}");
                sb.AppendLine($"ResponsesCount{j}:{r.Responses.Length}");
                int i = 1;
                foreach (var resp in r.Responses)
                {
                    sb.AppendLine($"Animal_{j}_{i}:{resp.Animal}");
                    sb.AppendLine($"Trait_{j}_{i}:{resp.CharacterTrait}");
                    sb.AppendLine($"Concept_{j}_{i}:{resp.Concept}");
                    i++;
                }
                j++;
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, sb.ToString());
        }
        private Lab_7.Purple_1.Participant DeserializeParticipant(int indexi = 1, bool flag = true)
        {
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> paticipantParsing = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var parts = line.Split(":");
                    paticipantParsing[parts[0].Trim()] = parts[1].Trim();
                }
            }
            string nameParticipant = paticipantParsing[$"NameParticipant{indexi}"];
            string surnameParticipant = paticipantParsing[$"SurnameParticipant{indexi}"];
            double[] coefsParticipant = paticipantParsing[$"CoefsParticipant{indexi}"].Split(";").Select(i => double.Parse(i)).ToArray();
            int[] MarksParticipant = paticipantParsing[$"MarksParticipant{indexi}"].Split(";").Select(i => int.Parse(i)).ToArray();
            int[,] marksInMatrix = new int[4, 7];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 7; j++)
                    marksInMatrix[i, j] = MarksParticipant[i * 7 + j];
            Purple_1.Participant p = new Purple_1.Participant(nameParticipant, surnameParticipant);
            p.SetCriterias(coefsParticipant);
            if (flag)
            {
                for (int i = 0; i < 4; i++)
                {
                    int[] marks = new int[7];
                    for (int j = 0; j < 7; j++)
                    {
                        marks[j] = marksInMatrix[i, j];
                    }
                    p.Jump(marks);
                }
            }
            return p;
        }
        private Lab_7.Purple_1.Judge DeserializeJudge(int indexi = 1)
        {
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> JudgeParsing = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var parts = line.Split(":");
                    JudgeParsing[parts[0].Trim()] = parts[1].Trim();
                }
            }
            string name_of_Judge = JudgeParsing[$"NameJudge{indexi}"];
            int[] marks_of_Judge = JudgeParsing[$"MarksJudge{indexi}"].Split(";").Select(i => int.Parse(i)).ToArray();
            Purple_1.Judge j = new Purple_1.Judge(name_of_Judge, marks_of_Judge);
            return j;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Purple_1.Participant))
            {
                return DeserializeParticipant() as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                return DeserializeJudge() as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var lines = File.ReadAllLines(FilePath);
                Dictionary<string, string> CompetitionParsing = new Dictionary<string, string>();
                foreach (var line in lines)
                {
                    if (line.Contains(":"))
                    {
                        var parts = line.Split(":");
                        CompetitionParsing[parts[0].Trim()] = parts[1].Trim();
                    }
                }
                int countJudges = int.Parse(CompetitionParsing["JudgesCount"]);
                Purple_1.Judge[] judges = new Purple_1.Judge[countJudges];
                int countPaticipants = int.Parse(CompetitionParsing["ParticipantsCount"]);
                Purple_1.Participant[] participants = new Purple_1.Participant[countPaticipants];
                for (int i = 1; i <= countJudges; i++)
                {
                    judges[i - 1] = DeserializeJudge(i);
                }
                for (int i = 1; i <= countPaticipants; i++)
                {
                    participants[i - 1] = DeserializeParticipant(i);
                }
                Purple_1.Competition competition = new Purple_1.Competition(judges);
                competition.Add(participants);
                return competition as T;
            }
            else
            {
                return null;
            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> skiJumpingDeserial = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var parts = line.Split(":");
                    skiJumpingDeserial[parts[0].Trim()] = parts[1].Trim();
                }
            }
            Purple_2.SkiJumping ski;
            int standart = int.Parse(skiJumpingDeserial["Standard"]);
            if (standart == 100)
                ski = new JuniorSkiJumping();
            else if (standart == 150)
                ski = new ProSkiJumping();
            else
            {
                return null;
            }
            string name = skiJumpingDeserial["Name"];
            int count_of_participants = int.Parse(skiJumpingDeserial["ParticipantCount"]);
            for (int i = 1; i <= count_of_participants; i++)
            {
                string pName = skiJumpingDeserial[$"ParticipantName{i}"];
                string pSurname = skiJumpingDeserial[$"ParticipantSurname{i}"];
                int pDistance = int.Parse(skiJumpingDeserial[$"Distance{i}"]);
                int[] pMarks = skiJumpingDeserial[$"Marks{i}"].Split(";").Select(i => int.Parse(i)).ToArray();
                Purple_2.Participant p = new Purple_2.Participant(pName, pSurname);
                p.Jump(pDistance, pMarks, standart);
                ski.Add(p);
            }
            return ski as T;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> skatings = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var parts = line.Split(":");
                    skatings[parts[0].Trim()] = parts[1].Trim();
                }
            }
            var moods = skatings["Moods"].Split(";").Select(i => double.Parse(i)).ToArray();
            Purple_3.Skating skating;
            string type = skatings["Type"];
            switch (type)
            {
                case nameof(Purple_3.IceSkating):
                    skating = new Purple_3.IceSkating(moods, false);
                    break;
                case nameof(Purple_3.FigureSkating):
                    skating = new Purple_3.FigureSkating(moods, false);
                    break;
                default:
                    return null;
            }
            int participantsCount = int.Parse(skatings["ParticipantsCount"]);
            for (int i = 1; i <= participantsCount; i++)
            {
                var pName = skatings[$"SkaterName{i}"];
                var pSurname = skatings[$"SkaterSurname{i}"];
                var p = new Purple_3.Participant(pName, pSurname);
                var marks = skatings[$"SkaterMarks{i}"].Split(";").Select(s => double.Parse(s)).ToArray();
                foreach (double m in marks)
                {
                    p.Evaluate(m);
                }
                skating.Add(p);
                
            }
            return skating as T;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> groupTxt = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var parts = line.Split(":");
                    groupTxt[parts[0].Trim()] = parts[1].Trim();
                }
            }
            string name = groupTxt["Name"];
            var group = new Purple_4.Group(name);
            int countSport = int.Parse(groupTxt["SportsmanCount"]);
            for (int i = 1; i <= countSport; i++)
            {
                string sName = groupTxt[$"SportsmanName{i}"];
                string sSurname = groupTxt[$"SportsmanSurname{i}"];
                double stime = double.Parse(groupTxt[$"SportsmanTime{i}"]);
                Purple_4.Sportsman sportsman = new Purple_4.Sportsman(sName, sSurname);
                sportsman.Run(stime);
                group.Add(sportsman);
            }
            return group;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> reportTxt = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var parts = line.Split(":");
                    reportTxt[parts[0].Trim()] = parts[1].Trim();
                }
            }
            int countResearches = int.Parse(reportTxt["ResearchesCount"]);
            var report = new Purple_5.Report();
            for (int i = 1; i <= countResearches; i++)
            {
                string rName = reportTxt[$"ResearchName{i}"];
                var research = new Purple_5.Research(rName);
                int countResp = int.Parse(reportTxt[$"ResponsesCount{i}"]);
                for (int j = 1; j <= countResp; j++)
                {
                    string rAnimal = reportTxt[$"Animal_{i}_{j}"] == "" ? null : reportTxt[$"Animal_{i}_{j}"];
                    string rTrait = reportTxt[$"Trait_{i}_{j}"] == "" ? null : reportTxt[$"Trait_{i}_{j}"];
                    string rConcept = reportTxt[$"Concept_{i}_{j}"] == "" ? null : reportTxt[$"Concept_{i}_{j}"];
                    research.Add(new[] { rAnimal, rTrait, rConcept });
                }
                report.AddResearch(research);
            }
            return report;
        }
    }
}
