using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using static Lab_7.Purple_1;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";
        #region Purple_1
        public override void SerializePurple1<T>(T obj, string fileName)
        {

            StringBuilder sb = new StringBuilder();
            if (obj is Participant participant)
            {
                int i = 0;
                sb.AppendLine($"NameP{i}:{participant.Name}");
                sb.AppendLine($"SurnameP{i}:{participant.Surname}");
                sb.AppendLine($"CoefsP{i}:{String.Join(" ", participant.Coefs)}");
                sb.AppendLine($"MarksP{i}:{String.Join(" ", participant.Marks.Cast<int>())}");
                sb.AppendLine($"TotalScoreP{i}:{participant.TotalScore}");
            }
            else if (obj is Judge judge)
            {
                int i = 0;
                sb.AppendLine($"NameJ{i}:{judge.Name}");
                sb.AppendLine($"MarksJ{i}:{String.Join(" ", judge.Marks)}");
            }
            else if (obj is Competition competition)
            {
                
                sb.AppendLine("JudgeStart");
                sb.AppendLine($"CountJudges:{competition.Judges.Length}");
                for (int i = 0; i < competition.Judges.Length; i++)
                {
                    var judgei = competition.Judges[i];
                    sb.AppendLine($"NameJ{i}:{judgei.Name}");
                    sb.AppendLine($"MarksJ{i}:{String.Join(" ", judgei.Marks)}");
                    sb.AppendLine("___");

                }
                sb.AppendLine("JudgeEnd");
                
                sb.AppendLine("participantStart");
                sb.AppendLine($"CountParticipants:{competition.Participants.Length}");
                for (int i = 0; i < competition.Participants.Length; i++)
                {
                    var participanti = competition.Participants[i];
                    sb.AppendLine($"NameP{i}:{participanti.Name}");
                    sb.AppendLine($"SurnameP{i}:{participanti.Surname}");
                    sb.AppendLine($"CoefsP{i}:{String.Join(" ", participanti.Coefs)}");
                    sb.AppendLine($"MarksP{i}:{String.Join(" ", participanti.Marks.Cast<int>())}");
                    sb.AppendLine($"TotalScoreP{i}:{participanti.TotalScore}");
                    sb.AppendLine("___");
                }
                sb.AppendLine("participantEnd");
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, sb.ToString());
        }
        private Participant DeserializePurple1_participant(int ii = 0)
        {

            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var pair = line.Split(':');
                    Dict[pair[0].Trim()] = pair[1];
                }
            }
            string Name = Dict[$"NameP{ii}"];
            string Surname = Dict[$"SurnameP{ii}"];
            double[] Coefs = Dict[$"CoefsP{ii}"].Split(' ')
                .Select(r => double.Parse(r))
                .ToArray();
            int[] marksArray = Dict[$"MarksP{ii}"].Split(' ')
                .Select(r => int.Parse(r))
                .ToArray();
            int[,] Marks = new int[4, 7];
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Marks[i, j] = marksArray[index++];
                }
            }
            Participant participant = new Participant(Name, Surname);
            participant.SetCriterias(Coefs);
            for (int i = 0; i < 4; i++)
            {
                int[] marks = new int[7];
                for (int j = 0; j < 7; j++)
                {
                    marks[j] = Marks[i, j];
                }
                participant.Jump(marks);
            }
            return participant;
        }
        private Judge DeserializePurple1_Judge(int ii=0)
        {
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var pair = line.Split(':');
                    Dict[pair[0].Trim()] = pair[1];
                }
            }
            string Name = Dict[$"NameJ{ii}"];
            int[] Marks = Dict[$"MarksJ{ii}"].Split(' ')
                .Select(r => int.Parse(r))
                .ToArray();
            var Judge = new Judge(Name, Marks);
            return Judge;

        }
        private Competition DeserializePurple1_Competition()
        {
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            string[] array = new string[lines.Length];
            int index = 0;
            Participant[] participants = null;
            Judge[] judges = null;
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var pair = line.Split(':');
                    Dict[pair[0].Trim()] = pair[1];
                }

            }
            int cntP = int.Parse(Dict["CountParticipants"]);
            int cntJ = int.Parse(Dict["CountJudges"]);
            participants = new Participant[cntP];
            judges = new Judge[cntJ];
            for (int i = 0; i < cntP; i++)
            {
                participants[i] = DeserializePurple1_participant(i);
            }
            for (int i = 0;i < cntJ; i++)
            {
                judges[i] = DeserializePurple1_Judge(i);
            }
            Competition competition = new Competition(judges);
            competition.Add(participants);
            return competition;

        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Participant))
            {
                return DeserializePurple1_participant() as T;
            }
            else if (typeof(T) == typeof(Judge))
            {
                return DeserializePurple1_Judge() as T;

            }
            else if (typeof(T) == typeof(Competition))
            {
                return DeserializePurple1_Competition() as T;
            }
            return null;
        }
        #endregion
        #region Purple_2
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Type:{jumping.GetType().Name}");
            sb.AppendLine($"nameComp:{jumping.Name}");
            sb.AppendLine($"Standard:{jumping.Standard}");
            sb.AppendLine($"Count:{jumping.Participants.Length}");
            sb.AppendLine("participantStart");
            Purple_2.Participant[] participants = jumping.Participants;
            int index = 0;
            foreach (var participant in participants)
            {
                sb.AppendLine($"Name{index}:{participant.Name}");
                sb.AppendLine($"Surname{index}:{participant.Surname}");
                sb.AppendLine($"Distance{index}:{participant.Distance}");
                sb.AppendLine($"Marks{index}:{String.Join(' ',participant.Marks)}");
                sb.AppendLine($"Result{index++}:{participant.Result}");


            }
            sb.AppendLine("participantEnd");
            File.WriteAllText(FilePath, sb.ToString());
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var pair = line.Split(':');
                    Dict[pair[0].Trim()] = pair[1];
                }
            }
            string Type = Dict["Type"];
            string nameComp = Dict["nameComp"];
            int Count = int.Parse(Dict["Count"]);
            int standard = int.Parse(Dict["Standard"]);
            Purple_2.SkiJumping p;
            if (Type.Contains("JuniorSkiJumping"))
                p = new Purple_2.JuniorSkiJumping();
            else
                p = new Purple_2.ProSkiJumping();
            var participants = new Purple_2.Participant[Count];
            for(int i = 0; i < Count;i++)
            {
                participants[i] = deserealizeParticipants_2(Dict, standard, i);
            }
            p.Add(participants);
            return p as T;


            
        }
        private Purple_2.Participant deserealizeParticipants_2(Dictionary<string, string> Dict, int target, int index=0)
        {
            string Name = Dict[$"Name{index}"];
            string Surname = Dict[$"Surname{index}"];
            int Distance = int.Parse(Dict[$"Distance{index}"]);
            int[] Marks = Dict[$"Marks{index}"].Split(' ').Select(x => int.Parse(x)).ToArray();
            int Result = int.Parse(Dict[$"Result{index}"]);
            var participant = new Purple_2.Participant(Name, Surname);
            participant.Jump(Distance, Marks, target);
            return participant;

        }
        #endregion
        #region Purple_3
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            StringBuilder sb = new StringBuilder();
            if (skating is Purple_3.FigureSkating p) sb.AppendLine("Type:FigureSkating");
            else sb.AppendLine("Type:IceSkating");
            sb.AppendLine($"Moods:{String.Join(' ', skating.Moods)}");
            sb.AppendLine($"Count:{skating.Participants.Length}");
            int count = skating.Participants.Length;
            for (int i = 0; i < count; i++)
            {
                addParticipants_3(sb, skating.Participants[i], i);
            }
            File.WriteAllText(FilePath, sb.ToString());
        }

        private void addParticipants_3(StringBuilder sb, Purple_3.Participant p, int index = 0)
        {
            sb.AppendLine($"Name{index}:{p.Name}");
            sb.AppendLine($"Surname{index}:{p.Surname}");
            sb.AppendLine($"Marks{index}:{String.Join(' ', p.Marks)}");
            sb.AppendLine($"Places{index}:{String.Join(' ', p.Places)}");
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string,string> Dict = new Dictionary<string,string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var pairs = line.Split(':');
                    Dict[pairs[0].Trim()] = pairs[1].Trim();
                }
            }
            string Type = Dict["Type"];
            int count = int.Parse(Dict["Count"]);
            double[] Moods = Dict["Moods"].Split(' ').Select(r => double.Parse(r)).ToArray();
            Purple_3.Skating skating;
            if (Type.Contains("FigureSkating"))
            {
                skating = new Purple_3.FigureSkating(Moods, false);
            }
            else
            {
                skating = new Purple_3.IceSkating(Moods, false);
            }

            var p = new Purple_3.Participant[count];

            for(int i = 0; i < count; i++)
            {
                p[i] = deserealizeParticipants_3(Dict, i);
            }
            skating.Add(p);


            Purple_3.Participant.SetPlaces(p);
            
            return skating as T;

        }
        private Purple_3.Participant deserealizeParticipants_3(Dictionary<string,string> Dict, int index = 0)
        {
            string Name = Dict[$"Name{index}"];
            string Surname = Dict[$"Surname{index}"];
            int[] Places = Dict[$"Places{index}"].Split(' ').Select(r => int.Parse(r)).ToArray();
            double[] Marks = Dict[$"Marks{index}"].Split(' ').Select(x => double.Parse(x)).ToArray();
            var participant = new Purple_3.Participant(Name, Surname);
            for (int i = 0; i < Marks.Length; i++)
            {
                participant.Evaluate(Marks[i]);
            }
            
            return participant;
        }
        #endregion
        #region Purple_4
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"nameGroup:{participant.Name}");
            int count = participant.Sportsmen.Length;
            sb.AppendLine($"Count:{count}");
            for (int i = 0;i<count;i++)
            {
                sb.AppendLine($"Name{i}:{participant.Sportsmen[i].Name}");
                sb.AppendLine($"Surname{i}:{participant.Sportsmen[i].Surname}");
                sb.AppendLine($"Time{i}:{participant.Sportsmen[i].Time}");
            }
            File.WriteAllText(FilePath, sb.ToString());
        }


        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string,string> dict = new Dictionary<string,string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var pair = line.Split(":");
                    dict[pair[0].Trim()] = pair[1].Trim();
                }
            }
            string nameGroup = dict["nameGroup"];
            int count = int.Parse(dict["Count"]);
            Purple_4.Sportsman[] s = new Purple_4.Sportsman[count];
            for (int i = 0; i < count; i++)
            {
                s[i] = deserealizeSportsmen(dict, i);
            }
            Purple_4.Group group = new Purple_4.Group(nameGroup);
            group.Add(s);
            return group;


        }
        private Purple_4.Sportsman deserealizeSportsmen(Dictionary<string,string> Dict, int index)
        {
            string Name = Dict[$"Name{index}"];
            string Surname = Dict[$"Surname{index}"];
            double Time = double.Parse(Dict[$"Time{index}"]);
            Purple_4.Sportsman s = new Purple_4.Sportsman(Name, Surname);
            s.Run(Time);
            return s;
        }
        #endregion
        #region Purple_5

        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);
            StringBuilder sb = new StringBuilder();
            int count = group.Researches.Length;
            sb.AppendLine($"researchesCount:{count}");
            for (int i = 0;i < count;i++)
            {
                addResearchh(sb, i, group.Researches[i]);
            }
            File.WriteAllText(FilePath, sb.ToString());

            
            
        }
        private void addResponses(StringBuilder sb, int index1, int index2, Purple_5.Response response)
        {
            sb.AppendLine($"Animal_{index1}_{index2}:{response.Animal}");
            sb.AppendLine($"characterTrait_{index1}_{index2}:{response.CharacterTrait}");
            sb.AppendLine($"Concept_{index1}_{index2}:{response.Concept}");
        }
        private void addResearchh(StringBuilder sb, int index1, Purple_5.Research research)
        {
            sb.AppendLine($"nameResearch{index1}:{research.Name}");
            sb.AppendLine($"responsesCount{index1}:{research.Responses.Length}");
            for (int i = 0; i < research.Responses.Length; i++)
            {
                addResponses(sb, index1, i, research.Responses[i]);
            }
        }

        private string[] deserializeResponses(Dictionary<string, string> dict, int index1, int index2)
        {
            string animal = dict[$"Animal_{index1}_{index2}"] == "" ? null : dict[$"Animal_{index1}_{index2}"];
            string characterTrait = dict[$"characterTrait_{index1}_{index2}"] == "" ? null : dict[$"characterTrait_{index1}_{index2}"];
            string concept = dict[$"Concept_{index1}_{index2}"] == "" ? null : dict[$"Concept_{index1}_{index2}"];
            string[] r = new string[3] {animal, characterTrait, concept};
            return r;
        }
        private Purple_5.Research deserializeResearches(Dictionary<string, string> dict, int index1)
        {
            string nameResearch = dict[$"nameResearch{index1}"];
            int count = int.Parse(dict[$"responsesCount{index1}"]);
            var research = new Purple_5.Research(nameResearch);

            for (int i = 0;i < count; i++)
            {
                research.Add(deserializeResponses(dict, index1, i));
            }
            return research;

        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            Dictionary<string,string> dict = new Dictionary<string,string>();
            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var pair = line.Split(':');
                    dict[pair[0].Trim()] = pair[1].Trim();
                }

            }
            int countResearch = int.Parse(dict["researchesCount"]);
            var r = new Purple_5.Report();
            for (int i = 0; i < countResearch; i++)
            {
                r.AddResearch(deserializeResearches(dict, i));
            }
            return r;
        }


        #endregion
    }
}
