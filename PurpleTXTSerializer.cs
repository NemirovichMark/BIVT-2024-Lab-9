using Lab_7;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Lab_7.Purple_2;
using static Lab_7.Purple_3;
using static Lab_7.Purple_5;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            StringBuilder S = new StringBuilder();
            if(obj is Purple_1.Participant participant)
            {
                SerializeParticipants(participant, S);
            }
            else if (obj is Purple_1.Judge judge)
            {
                SerializeJudge(judge, S);
            }
            else if (obj is Purple_1.Competition competition)
            {
                SerializeCompetition(competition, S);
            }
            File.WriteAllText(FilePath, S.ToString());
            Console.WriteLine(File.ReadAllText(FilePath));
        }
        
        private void SerializeParticipants(Purple_1.Participant participant, StringBuilder S, int index = 0)
        {
            S.AppendLine($"ParticipantName{index}:{participant.Name}");
            S.AppendLine($"ParticipantSurname{index}:{participant.Surname}");
            S.AppendLine($"ParticipantCoefs{index}:{string.Join("!", participant.Coefs)}");
            S.AppendLine($"ParticipantMarks{index}:{string.Join("!",participant.Marks.Cast<int>())}");
        }
        private void SerializeJudge(Purple_1.Judge judge, StringBuilder S, int index = 0)
        {
            S.AppendLine($"JudgeName{index}:{judge.Name}");
            S.AppendLine($"JudgeMarks{index}:{string.Join("!", judge.Marks.Cast<int>())}");
        }
        private void SerializeCompetition(Purple_1.Competition competition, StringBuilder S)
        {
            S.AppendLine($"ParticipantSum:{ competition.Participants.Length}");
            for (int i = 0; i < competition.Participants.Length; i++)
            {
                SerializeParticipants(competition.Participants[i], S, i);
            }
            S.AppendLine($"JudgeSum:{competition.Judges.Length}");
            for (int i = 0; i < competition.Judges.Length; i++)
            {
                SerializeJudge(competition.Judges[i], S, i);
            }      
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
                return DeserializeCompetition() as T;
            }
            else
            {
                return null;
            }
        }
        private Purple_1.Participant DeserializeParticipant(int index = 0)
        {
            string[] answer = File.ReadAllLines(FilePath);
            Dictionary<string, string> ParticipantWords = new Dictionary<string, string>();
            foreach(string word in answer)
            {
                if (word.Contains(":"))
                {
                    var words = word.Split(':');
                    ParticipantWords.Add(words[0].Trim(), words[1].Trim());
                }           
            }
            string ParticipantName = ParticipantWords[$"ParticipantName{index}"];
            string ParticipantSurname = ParticipantWords[$"ParticipantSurname{index}"];
            double[] ParticipantCoefs = ParticipantWords[$"ParticipantCoefs{index}"].Split("!").Select(double.Parse).ToArray();
            int[] ParticipantMarks = ParticipantWords[$"ParticipantMarks{index}"].Split("!").Select(int.Parse).ToArray();
            Purple_1.Participant participant = new Purple_1.Participant(ParticipantName, ParticipantSurname);
            participant.SetCriterias(ParticipantCoefs);
            int[,] marks = new int[4, 7];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 7; j++)
                    marks[i, j] = ParticipantMarks[i * 7 + j];
            for (int i = 0; i < 4; i++)
            {
                int[] marks1 = new int[7];
                for (int j = 0; j < 7; j++)
                {
                    marks1[j] = marks[i, j];
                }
                participant.Jump(marks1);
            }
            return participant;
        }
        private Purple_1.Judge DeserializeJudge(int index = 0)
        {
            string[] answer = File.ReadAllLines(FilePath);
            Dictionary<string, string> JudgeWords = new Dictionary<string, string>();
            foreach (string word in answer)
            {
                if (word.Contains(":"))
                {
                    var words = word.Split(':');
                    JudgeWords.Add(words[0].Trim(), words[1].Trim());
                }
            }
            string name = JudgeWords[$"JudgeName{index}"];
            int[] marks = JudgeWords[$"JudgeMarks{index}"].Split("!").Select(int.Parse).ToArray();
            Purple_1.Judge judge = new Purple_1.Judge(name, marks);
            return judge;
        }
        private Purple_1.Competition DeserializeCompetition()
        {
            string[] answer = File.ReadAllLines(FilePath);
            Dictionary<string, string> CompetitionWords = new Dictionary<string, string>();
            foreach (string word in answer)
            {
                if (word.Contains(":"))
                {
                    var words = word.Split(':');
                    CompetitionWords.Add(words[0].Trim(), words[1].Trim());
                }
            }
            int JudgeSum = int.Parse(CompetitionWords["JudgeSum"]);
            Purple_1.Judge[] judges = new Purple_1.Judge[JudgeSum];
            int PaticipantSum = int.Parse(CompetitionWords["ParticipantSum"]);
            Purple_1.Participant[] participants = new Purple_1.Participant[PaticipantSum];
            for (int i = 0; i < PaticipantSum; i++)
            {
                participants[i] = DeserializeParticipant(i);
            }
            for (int i = 0; i < JudgeSum; i++)
            {
                judges[i] = DeserializeJudge(i);
            }
            Purple_1.Competition competition = new Purple_1.Competition(judges);
            competition.Add(participants);
            return competition;
        }

       /// Purple_2

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName) 
        {
            SelectFile(fileName);
            StringBuilder S = new StringBuilder();
            if(jumping is Purple_2.SkiJumping skiJumping)
            {
                S.AppendLine($"SkiJumpingName:{skiJumping.Name}");
                S.AppendLine($"SkiJumpingStandard:{skiJumping.Standard}");
                S.AppendLine($"ParticipantSum:{skiJumping.Participants.Length}");
                for(int i=0;i< skiJumping.Participants.Length; i++)
                {
                    S.AppendLine($"ParticipantName{i}:{skiJumping.Participants[i].Name}");
                    S.AppendLine($"ParticipantSurname{i}:{skiJumping.Participants[i].Surname}");
                    S.AppendLine($"ParticipantDistance{i}:{skiJumping.Participants[i].Distance}");
                    S.AppendLine($"ParticipantMarks{i}:{string.Join("!", skiJumping.Participants[i].Marks)}");
                }
                File.WriteAllText(FilePath, S.ToString());
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            string[] answer = File.ReadAllLines(FilePath);
            Dictionary<string, string> SkiJumpingWords = new Dictionary<string, string>();
            foreach (string word in answer)
            {
                if (word.Contains(":"))
                {
                    var words = word.Split(':');
                    SkiJumpingWords.Add(words[0].Trim(), words[1].Trim());
                }
            }
            Purple_2.SkiJumping skiJumping;
            string name = SkiJumpingWords["SkiJumpingName"];
            int standart = int.Parse(SkiJumpingWords["SkiJumpingStandard"]);
            if (standart == 100)
                skiJumping = new JuniorSkiJumping();
            else if (standart == 150)
                skiJumping = new ProSkiJumping();
            else
            {
                return default(T);
            }

            int participantSum = int.Parse(SkiJumpingWords["ParticipantSum"]);
            for (int i = 0; i < participantSum; i++)
            {
                string Name = SkiJumpingWords[$"ParticipantName{i}"];
                string Surname = SkiJumpingWords[$"ParticipantSurname{i}"];
                int Distance = int.Parse(SkiJumpingWords[$"ParticipantDistance{i}"]);
                int[] Marks = SkiJumpingWords[$"ParticipantMarks{i}"].Split("!").Select(int.Parse).ToArray();
                Purple_2.Participant participant = new Purple_2.Participant(Name, Surname);
                participant.Jump(Distance, Marks, standart);
                skiJumping.Add(participant);
            }
            return skiJumping as T;
        }


        //Purple_3


        

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            StringBuilder S = new StringBuilder();
            if(skating is Purple_3.Skating skating1)
            {
                S.AppendLine($"Type:{skating1.GetType().Name}");
                S.AppendLine($"Moods:{string.Join("!", skating1.Moods)}");
                if (skating1.Participants != null)
                {
                    int a = skating1.Participants.Length;
                    
                    S.AppendLine($"ParticipantSum:{a}");
                    for (int i = 0; i < a; i++)
                    {
                        S.AppendLine($"ParticipantName{i}:{skating1.Participants[i].Name}");
                        S.AppendLine($"ParticipantSurname{i}:{skating1.Participants[i].Surname}");
                        S.AppendLine($"ParticipantMarks{i}:{string.Join("!", skating1.Participants[i].Marks)}");
                    }
                }
               
            }
            File.WriteAllText(FilePath, S.ToString());
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string[] answer = File.ReadAllLines(FilePath);
            Dictionary<string, string> SkatingWords = new Dictionary<string, string>();
            foreach (string word in answer)
            {
                if (word.Contains(":"))
                {
                    var words = word.Split(':');
                    SkatingWords.Add(words[0].Trim(), words[1].Trim());
                }
            }
            Purple_3.Skating skating;
            double[] moods = SkatingWords["Moods"].Split("!").Select(double.Parse).ToArray();
            string type = SkatingWords["Type"];
            if (type == "IceSkating")
            {
                skating = new Purple_3.IceSkating(moods, false);
            }
            else if (type == "FigureSkating")
            {
                skating = new Purple_3.FigureSkating(moods, false);
            }
            else
            {
                return default(T);
            }
            int participantSum = int.Parse(SkatingWords["ParticipantSum"]);
            for (int i = 0; i < participantSum; i++)
            {
                var Name = SkatingWords[$"ParticipantName{i}"];
                var Surname = SkatingWords[$"ParticipantSurname{i}"];
                var participant = new Purple_3.Participant(Name, Surname);
                var marks = SkatingWords[$"ParticipantMarks{i}"].Split("!").Select(double.Parse).ToArray();
                foreach (double mark in marks)
                {
                    participant.Evaluate(mark);
                }
                skating.Add(participant);

            }
            return skating as T;

        }

        //Purple_4

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            StringBuilder S = new StringBuilder();
            if(group is Purple_4.Group)
            {
                S.AppendLine($"GroupName:{group.Name}");
                S.AppendLine($"SportsmenSum:{group.Sportsmen.Length}");
                for(int i=0;i< group.Sportsmen.Length; i++)
                {
                    S.AppendLine($"SportsmenName{i}:{group.Sportsmen[i].Name}");
                    S.AppendLine($"SportsmenSurname{i}:{group.Sportsmen[i].Surname}");
                    S.AppendLine($"SportsmenTime{i}:{group.Sportsmen[i].Time}");
                }
            }
            File.WriteAllText(FilePath, S.ToString());
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string[] answer = File.ReadAllLines(FilePath);
            Dictionary<string, string> GroupWords = new Dictionary<string, string>();
            foreach (string word in answer)
            {
                if (word.Contains(":"))
                {
                    var words = word.Split(':');
                    GroupWords.Add(words[0].Trim(), words[1].Trim());
                }
            }
            string Name = GroupWords["GroupName"];
            Purple_4.Group group = new Purple_4.Group(Name);
            int SportsmenSum = int.Parse(GroupWords["SportsmenSum"]);
            for (int i = 0; i < SportsmenSum; i++)
            {
                string name = GroupWords[$"SportsmenName{i}"];
                string surname = GroupWords[$"SportsmenSurname{i}"];
                double time = double.Parse(GroupWords[$"SportsmenTime{i}"]);
                Purple_4.Sportsman sportsman = new Purple_4.Sportsman(name, surname);
                sportsman.Run(time);
                group.Add(sportsman);
            }
            return group;
        }


        // Purple_5

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            StringBuilder S = new StringBuilder();
            if(report is Purple_5.Report)
            {
                S.AppendLine($"ResearchSum:{report.Researches.Length}");
                for(int i=0;i< report.Researches.Length; i++)
                {
                    S.AppendLine($"ResearchName{i}:{report.Researches[i].Name}");
                    S.AppendLine($"ResponsSum{i}:{report.Researches[i].Responses.Length}");
                    for(int j=0;j< report.Researches[i].Responses.Length; j++)
                    {
                        S.AppendLine($"Animal{j}_{i}:{report.Researches[i].Responses[j].Animal}");
                        S.AppendLine($"CharacterTrait{j}_{i}:{report.Researches[i].Responses[j].CharacterTrait}");
                        S.AppendLine($"Concept{j}_{i}:{report.Researches[i].Responses[j].Concept}");
                    }
                }
                File.WriteAllText(FilePath, S.ToString());
            }
        }
         
        
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string[] answer = File.ReadAllLines(FilePath);
            Dictionary<string, string> ReportWords = new Dictionary<string, string>();
            foreach (string word in answer)
            {
                if (word.Contains(":"))
                {
                    var words = word.Split(':');
                    ReportWords.Add(words[0].Trim(), words[1].Trim());
                }
            }
            Purple_5.Report Report = new Purple_5.Report();
            int ResearchSum = int.Parse(ReportWords["ResearchSum"]);
            for(int i=0;i < ResearchSum; i++)
            {
                string Name = ReportWords[$"ResearchName{i}"];
                var research = new Purple_5.Research(Name);
                int ResponsesSum = int.Parse(ReportWords[$"ResponsSum{i}"]);
                for (int j=0;j<ResponsesSum;j++)
                {
                    string Animal = ReportWords[$"Animal{j}_{i}"] == "" ? null : ReportWords[$"Animal{j}_{i}"];
                    string Trait = ReportWords[$"CharacterTrait{j}_{i}"] == "" ? null : ReportWords[$"CharacterTrait{j}_{i}"];
                    string Concept = ReportWords[$"Concept{j}_{i}"] == "" ? null : ReportWords[$"Concept{j}_{i}"];
                    research.Add(new string[] { Animal, Trait, Concept });
                }
                Report.AddResearch(research);
            }
            return Report;
        }
    }
}
