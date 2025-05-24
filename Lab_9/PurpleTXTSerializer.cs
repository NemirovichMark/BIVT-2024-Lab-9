using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Lab_7.Purple_1;
using static Lab_7.Purple_2;
using static Lab_7.Purple_3;
using static Lab_7.Purple_4;
using static Lab_7.Purple_5;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            switch(obj)
            {
                case Purple_1.Participant participant:
                    FileWriter(SerializePurple1Participant(participant), fileName);
                    break;
                case Purple_1.Judge judge:
                    FileWriter(SerializeJudge(judge), fileName);
                    break;
                case Purple_1.Competition competition:
                    FileWriter(SerializeCompetition(competition), fileName);
                    break;
            }
        }
        private void FileWriter(string text, string fileName)
        {
            using (var writer = new StreamWriter($"{Path.Combine(FolderPath, fileName)}.{Extension}"))
            {
                writer.Write(text.Trim());
            }
        }
        private string SerializePurple1Participant(Purple_1.Participant participant)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Participant}");
            sb.AppendLine(participant.Name);
            sb.AppendLine(participant.Surname);
            sb.AppendLine(string.Join(" ", participant.Coefs));
            int[,] marks = participant.Marks;
            for (int i = 0; i < marks.GetLength(0); i++)
            {
                int[] lineMarks = new int[marks.GetLength(1)];
                for (int j = 0; j < marks.GetLength(1); j++)
                    lineMarks[j] = marks[i, j];
                sb.AppendLine(string.Join(" ", lineMarks));
            }
            return sb.ToString().Trim();
        }
        private string SerializeJudge(Judge judge)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Judge}");
            sb.AppendLine(judge.Name);
            sb.AppendLine(string.Join(" ", judge.Marks));

            return sb.ToString().Trim();
        }
        private string SerializeCompetition(Competition competition)
        {
            var judges = competition.Judges;
            var participants = competition.Participants;
            var sb = new StringBuilder();

            sb.AppendLine("{Judges}");
            foreach (var judge in judges)
                sb.AppendLine(SerializeJudge(judge));
            sb.AppendLine("{Participants}");
            foreach (var participant in participants)
                sb.AppendLine(SerializePurple1Participant(participant));

            return sb.ToString();
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName) { FileWriter(SerializeSkiJumping(jumping), fileName); }
        private string SerializePurple2Participant(Purple_2.Participant participant)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Participant}");
            sb.AppendLine(participant.Name);
            sb.AppendLine(participant.Surname);
            sb.AppendLine(participant.Distance.ToString());
            sb.AppendLine(string.Join(" ", participant.Marks));

            return sb.ToString().Trim();
        }
        private string SerializeSkiJumping(Purple_2.SkiJumping skiJumping)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{SkiJumping}");
            sb.AppendLine(skiJumping.GetType().AssemblyQualifiedName);
            sb.AppendLine(skiJumping.Name);
            sb.AppendLine(skiJumping.Standard.ToString());
            var participants = skiJumping.Participants;

            sb.AppendLine("{Participants}");
            foreach (var participant in participants)
                sb.AppendLine(SerializePurple2Participant(participant));

            return sb.ToString().Trim();
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName) { FileWriter(SerializeSkating(skating), fileName); }
        private string SerializePurple3Participant(Purple_3.Participant participant)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Participant}");
            sb.AppendLine(participant.Name);
            sb.AppendLine(participant.Surname);
            sb.AppendLine(string.Join(" ", participant.Marks));
            sb.AppendLine(string.Join(" ", participant.Places));
            return sb.ToString().Trim();
        }
        private string SerializeSkating(Purple_3.Skating skating)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Skating}");
            sb.AppendLine(skating.GetType().AssemblyQualifiedName);
            sb.AppendLine(string.Join(" ", skating.Moods));
            var participants = skating.Participants;

            sb.AppendLine("{Participants}");
            foreach (var participant in participants)
                sb.AppendLine(SerializePurple3Participant(participant));

            return sb.ToString().Trim();
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName) { FileWriter(SerializeGroup(group), fileName); }
        private string SerializeSportsman(dynamic sportsman)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Sportsman}");
            sb.AppendLine(sportsman.GetType().AssemblyQualifiedName);
            sb.AppendLine(sportsman.Name);
            sb.AppendLine(sportsman.Surname);
            sb.AppendLine(sportsman.Time.ToString());

            return sb.ToString().Trim();
        }
        private string SerializeGroup(Purple_4.Group group)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Group}");
            sb.AppendLine(group.Name);
            var sportsmen = group.Sportsmen;

            sb.AppendLine("{Sportsmen}");
            foreach (var sportsman in sportsmen)
                sb.AppendLine(SerializeSportsman(sportsman));

            return sb.ToString().Trim();
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName) { FileWriter(SerializeReport(report), fileName); }
        private string SerializeResponse(Purple_5.Response response)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Response}");
            sb.AppendLine(string.IsNullOrEmpty(response.Animal) ? "null" : response.Animal);
            sb.AppendLine(string.IsNullOrEmpty(response.CharacterTrait) ? "null" : response.CharacterTrait);
            sb.AppendLine(string.IsNullOrEmpty(response.Concept) ? "null" : response.Concept);

            return sb.ToString().Trim();
        }
        private string SerializeResearch (Purple_5.Research research)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Research}");
            sb.AppendLine(research.Name);
            var responses = research.Responses;
            sb.AppendLine("{Responses}");
            foreach (var response in responses)
                sb.AppendLine(SerializeResponse(response));

            return sb.ToString().Trim();
        }
        private string SerializeReport (Purple_5.Report report)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{Report}");
            var researches = report.Researches;

            sb.AppendLine("{Researches}");
            foreach (var research in researches)
                sb.AppendLine(SerializeResearch(research));

            return sb.ToString().Trim();
        }

        private string FileReader(string fileName)
        {
            string text = "";
            using (var reader = new StreamReader($"{Path.Combine(FolderPath, fileName)}.{Extension}"))
            {
                text = reader.ReadToEnd();
            }
            return text.Trim();
        }
        private Purple_1.Participant DeserializePurple1Participant(string text)
        {
            string[] data = text.Trim()
                .Split("{Participant}", StringSplitOptions.RemoveEmptyEntries)[0].Trim()
                .Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim())
                .ToArray();

            double[] coefs = data[2].Split(' ')
                        .Select(s => double.Parse(s))
                        .ToArray();
            var participant = new Purple_1.Participant(data[0], data[1]);
            participant.SetCriterias(coefs);

            for (int i = 3; i < data.Length; i++)
            {
                int[] curMarks = data[i].Split(" ").Select(s => int.Parse(s)).ToArray();
                participant.Jump(curMarks);
            }

            return participant;
        }
        private Purple_1.Judge DeserializeJudge(string text)
        {
            string[] data = text
                .Split("{Judge}", StringSplitOptions.RemoveEmptyEntries)[0].Trim()
                .Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim())
                .ToArray();

            return new Purple_1.Judge(data[0], data[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.ToString())).ToArray());
        }
        private Purple_1.Competition DeserializeCompetition(string text)
        {
            string[][] data = text.Split(new string[] { "{Participants}", "{Judges}"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim().Split(new string[] { "{Participant}", "{Judge}" }, StringSplitOptions.RemoveEmptyEntries).ToArray())
                .ToArray();

            Judge[] judges = new Judge[data[0].Length];
            for (int i = 0; i < judges.Length; i++)
                judges[i] = DeserializeJudge(data[0][i]);

            var competition = new Competition(judges);

            if (data.Length == 1) return competition;
            for (int i = 0; i < data[1].Length; i++) 
                competition.Add(DeserializePurple1Participant(data[1][i]));

            return competition;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            string text = FileReader(fileName);
            if (typeof(T) == typeof(Purple_1.Participant))
                return DeserializePurple1Participant(text) as T;
            else if (typeof(T) == typeof(Purple_1.Judge))
                return DeserializeJudge(text) as T;
            else if (typeof(T) == typeof(Purple_1.Competition))
                return DeserializeCompetition(text) as T;
            return null;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName) { return DeserializeSkiJumping(FileReader(fileName)) as T; }
        private Purple_2.Participant DeserializePurple2Participant(string text, int target)
        {
            string[] data = text.Trim()
                .Split("{Participant}", StringSplitOptions.RemoveEmptyEntries)[0].Trim()
                .Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim())
                .ToArray();

            int[] marks = data[3].Split(' ')
                        .Select(s => int.Parse(s))
                        .ToArray();
            var participant = new Purple_2.Participant(data[0], data[1]);

            participant.Jump(int.Parse(data[2]), marks, target);

            return participant;
        }
        private dynamic DeserializeSkiJumping(string text)
        {
            string[] data = text.Split("{Participants}", StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();
            string[] skiJumpingInfo = data[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).ToArray();

            var type = Type.GetType(skiJumpingInfo[1]);
            dynamic resultObj = Activator.CreateInstance(type);

            if (data.Length < 2) return resultObj;

            string[] participants = data[1].Split("{Participant}", StringSplitOptions.RemoveEmptyEntries).ToArray();

            foreach (var participant in participants)
            {
                var newParticipant = DeserializePurple2Participant(participant, int.Parse(skiJumpingInfo[3]));
                resultObj.Add(newParticipant);
            }

            return resultObj;
        }
        public override T DeserializePurple3Skating<T>(string fileName) { return DeserializeSkating(FileReader(fileName)); }
        private Purple_3.Participant DeserializePurple3Participant(string text)
        {
            string[] data = text.Trim()
                .Split("{Participant}", StringSplitOptions.RemoveEmptyEntries)[0].Trim()
                .Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim())
                .ToArray();

            double[] marks = data[2].Split(' ')
                        .Select(s => double.Parse(s))
                        .ToArray();

            int[] places = data[3].Split(' ')
                        .Select(s => int.Parse(s))
                        .ToArray();

            var participant = new Purple_3.Participant(data[0], data[1]);

            for (int i = 0; i < marks.Length; i++)
                participant.Evaluate(marks[i]);

            return participant;
        }
        private dynamic DeserializeSkating(string text)
        {
            string[] data = text.Split("{Participants}", StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();
            string[] skatingInfo = data[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).ToArray();

            var type = Type.GetType(skatingInfo[1]);
            object[] arguments = new object[] { skatingInfo[2].Split(" ").Select(e => double.Parse(e)).ToArray(), false };
            dynamic resultObj = Activator.CreateInstance(type, arguments);

            if (data.Length < 2) return resultObj;

            string[] participants = data[1].Split("{Participant}", StringSplitOptions.RemoveEmptyEntries).ToArray();

            foreach (var participant in participants)
            {
                var newParticipant = DeserializePurple3Participant(participant);
                resultObj.Add(newParticipant);
            }

            return resultObj;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName) { return DeserializeGroup(FileReader(fileName)); }
        private dynamic DeserializeSportsman(string text)
        {
            string[] data = text.Trim()
                .Split("{Sportsman}", StringSplitOptions.RemoveEmptyEntries)[0].Trim()
                .Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim())
                .ToArray();

            object[] arguments = new object[] { data[1], data[2] };
            dynamic sportsman = Activator.CreateInstance(Type.GetType(data[0]), arguments);
            sportsman.Run(double.Parse(data[3]));

            return sportsman;
        }
        private Purple_4.Group DeserializeGroup(string text)
        {
            string[] data = text.Split("{Sportsmen}", StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();
            string[] groupInfo = data[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).ToArray();

            var group = new Purple_4.Group(groupInfo[1]);
            if (data.Length < 2) return group;

            string[] sportsmen = data[1].Split("{Sportsman}", StringSplitOptions.RemoveEmptyEntries).ToArray();

            foreach (var sportsman in sportsmen)
            {
                var newSportsman = DeserializeSportsman(sportsman);
                group.Add(newSportsman);
            }

            return group;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName) { return DeserializeReport(FileReader(fileName)); }
        private string[] DeserializeResponse(string text)
        {
            string[] data = text.Trim()
                .Split("{Response}", StringSplitOptions.RemoveEmptyEntries)[0].Trim()
                .Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim())
                .ToArray();

            return data;
        }
        private Purple_5.Research DeserializeResearch(string text)
        {
            string[] data = text.Split("{Responses}", StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();
            string[] groupInfo = data[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).ToArray();

            var research = new Purple_5.Research(groupInfo[0]);
            if (data.Length < 2) return research;

            string[] responses = data[1].Split("{Response}", StringSplitOptions.RemoveEmptyEntries).ToArray();

            foreach (var response in responses)
            {
                var newResponse = DeserializeResponse(response).Select(e => e == "null" ? null : e).ToArray();

                research.Add(newResponse);
            }

            return research;
        }
        private Purple_5.Report DeserializeReport(string text)
        {
            string[] data = text.Split("{Researches}", StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();
            string[] groupInfo = data[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).ToArray();

            var report = new Purple_5.Report();
            if (data.Length < 2) return report;

            string[] researches = data[1].Split("{Research}", StringSplitOptions.RemoveEmptyEntries).ToArray();

            foreach (var research in researches)
            {
                var newResponse = DeserializeResearch(research);
                report.AddResearch(newResponse);
            }

            return report;
        }

        private void Print<T>(T[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        private void Print<T>(T[] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                Console.Write(array[i]);
            }
            Console.WriteLine();
        }
    }
}