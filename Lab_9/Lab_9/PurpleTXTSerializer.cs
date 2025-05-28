using Lab_7;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_1;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        private void SerializeParticipant(Purple_1.Participant p, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(p.Name);
                writer.WriteLine(p.Surname);

                var Coefs = string.Join(" ", p.Coefs.Select(x => x.ToString(CultureInfo.InvariantCulture)));
                var Marks = string.Join(" ", p.Marks.OfType<int>().ToList());
                writer.WriteLine(Coefs);
                writer.WriteLine(Marks);
            }
        }

        private void SerializeJudge(Purple_1.Judge j, string fileName)
        {
            SelectFile(fileName);
            var Marks = string.Join(" ", j.Marks);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(j.Name);
                writer.WriteLine(Marks);
            }
        }

        // Purple_1
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj is Purple_1.Participant participant)
            {
                SelectFile(fileName);

                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine(participant.Name);
                    writer.WriteLine(participant.Surname);

                    var Coefs = string.Join(" ", participant.Coefs.Select(x => x.ToString(CultureInfo.InvariantCulture)));
                    var Marks = string.Join(" ", participant.Marks.OfType<int>().ToList());
                    writer.WriteLine(Coefs);
                    writer.WriteLine(Marks);
                }
            }
            else if (obj is Purple_1.Judge judge)
            {
                SelectFile(fileName);
                var Marks = string.Join(" ", judge.Marks);

                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine(judge.Name);
                    writer.WriteLine(Marks);
                }

            }
            else if (obj is Purple_1.Competition competition)
            {
                SelectFile(fileName);

                var CompetitionDirectory = Path.Combine(FolderPath, $"CompetitionDirectory_{fileName}");

                if (!Directory.Exists(CompetitionDirectory))
                {
                    Directory.CreateDirectory(CompetitionDirectory);
                }

                var p1 = competition.Participants.Length; var j1 = competition.Judges.Length;

                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine("Participants");
                    writer.WriteLine(p1);
                    writer.WriteLine("Judges");
                    writer.WriteLine(j1);

                    for (int i = 0; i < p1; i++)
                    {
                        var path = Path.Combine(CompetitionDirectory, $"participant {i}");
                        SerializeParticipant(competition.Participants[i], path);
                    }

                    for (int i = 0; i < j1; i++)
                    {
                        var path = Path.Combine(CompetitionDirectory, $"judge {i}");
                        SerializeJudge(competition.Judges[i], path);
                    }

                }
            }
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var fileLines = File.ReadAllLines(FilePath);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var participant = new Purple_1.Participant(fileLines[0], fileLines[1]);

                var coefParts = fileLines[2].Split(' ');
                var coefs = new double[coefParts.Length];

                for (int i = 0; i < coefParts.Length; i++)
                {
                    coefs[i] = double.Parse(coefParts[i], CultureInfo.InvariantCulture);
                }
                participant.SetCriterias(coefs);

                var markParts = fileLines[3].Split(' ');
                var marks = new int[markParts.Length];

                for (int i = 0; i < markParts.Length; i++)
                {
                    marks[i] = int.Parse(markParts[i]);
                }

                for (int i = 0; i < 4; i++)
                {
                    int[] MARKS = new int[7];
                    var position_of_jump = i * 7;
                    Array.Copy(marks, position_of_jump, MARKS, 0, 7);
                    participant.Jump(MARKS);
                }

                return participant as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                string[] marks = fileLines[1].Split(' ');
                int[] marks_massive = new int[marks.Length];

                for (int i = 0; i < marks.Length; i++)
                {
                    marks_massive[i] = int.Parse(marks[i]);
                }

                return new Purple_1.Judge(fileLines[0], marks_massive) as T;
            }
            else
            {

                var folder = Path.Combine(FolderPath, $"CompetitionDirectory_{fileName}");

                var judges = new Purple_1.Judge[int.Parse(fileLines[3])];

                for (int i = 0; i < int.Parse(fileLines[3]); i++)
                {
                    string judge = Path.Combine(folder, $"judge {i}");
                    judges[i] = DeserializeJudge(judge);
                }

                var result = new Purple_1.Competition(judges);

                for (int i = 0; i < int.Parse(fileLines[1]); i++)
                {
                    string person = Path.Combine(folder, $"participant {i}");
                    result.Add(DeserializeParticipant(person));
                }

                return result as T;
            }
        }

        // Purple_2
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                var SkiJumpingDirectory = Path.Combine(FolderPath, $"SkiJumpingDirectory_{fileName}");

                if (!Directory.Exists(SkiJumpingDirectory))
                {
                    Directory.CreateDirectory(SkiJumpingDirectory);
                }

                writer.WriteLine(jumping.Name);
                writer.WriteLine(jumping.Standard);
                writer.WriteLine(jumping.Participants.Length);

                int p1 = jumping.Participants.Length;

                for (int i = 0; i < p1; i++)
                {
                    var signle_participant = Path.Combine(SkiJumpingDirectory, $"participant {i}.{Extension}");

                    using (StreamWriter writer1 = new StreamWriter(signle_participant))
                    {

                        var Marks = string.Join(" ", jumping.Participants[i].Marks);

                        writer1.WriteLine(jumping.Participants[i].Name);
                        writer1.WriteLine(jumping.Participants[i].Surname);
                        writer1.WriteLine(jumping.Participants[i].Distance);
                        writer1.WriteLine(jumping.Participants[i].Result);
                        writer1.WriteLine(Marks);
                    }
                }

            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            var fileLines = File.ReadAllLines(FilePath);

            Purple_2.SkiJumping jump;

            if (fileLines[0] == "100m")
            {
                jump = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                jump = new Purple_2.ProSkiJumping();
            }

            var SkiJumpingDirectory = Path.Combine(FolderPath, $"SkiJumpingDirectory_{fileName}");

            var Participants = new Purple_2.Participant[int.Parse(fileLines[2])];

            int stand_of_jump = int.Parse(fileLines[1]);

            for (int i = 0; i < int.Parse(fileLines[2]); i++)
            {
                var participants = File.ReadAllLines(Path.Combine(SkiJumpingDirectory, $"participant {i}.{Extension}"));
                var participant = new Purple_2.Participant(participants[0], participants[1]);

                int distance = int.Parse(participants[2]);
                int result = int.Parse(participants[3]);
                int[] marks = participants[4].Split(' ').Select(int.Parse).ToArray();
                participant.Jump(distance, marks, stand_of_jump);
                Participants[i] = participant;
            }

            jump.Add(Participants);

            return (T)(Object)jump;

        }

        // Purple_3
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(skating.GetType().Name);
                writer.WriteLine(string.Join(" ", skating.Moods.Select(m => m.ToString(CultureInfo.InvariantCulture))));
                writer.WriteLine(skating.Participants.Length);

                foreach (var participant in skating.Participants)
                {
                    writer.WriteLine(
                        $"{participant.Name} " +
                        $"{participant.Surname} " +
                        $"{string.Join(" ", participant.Marks.Select(m => m.ToString(CultureInfo.InvariantCulture)))}");
                }
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var fileLines = File.ReadAllLines(FilePath);

            double[] Moods = fileLines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();


            Purple_3.Skating skating;
            if (fileLines[0] == "FigureSkating")
            {
                skating = new Purple_3.FigureSkating(Moods, false);
            }
            else
            {
                skating = new Purple_3.IceSkating(Moods, false);
            }

            int participantsCount = int.Parse(fileLines[2]);

            for (int i = 0; i < participantsCount && i + 3 < fileLines.Length; i++)
            {
                var info = fileLines[i + 3].Split(' ');
                if (info.Length < 3) { continue; }
                var participant = new Purple_3.Participant(info[0], info[1]);

                for (int j = 2; j < info.Length; j++)
                {
                    if (double.TryParse(info[j], CultureInfo.InvariantCulture, out double mark))
                    {
                        participant.Evaluate(mark);
                    }
                }
                skating.Add(participant);
            }
            return (T)(object)skating;
        }

        // Purple_4
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(group.Name);
                writer.WriteLine(group.Sportsmen.Length);

                var grouplen = group.Sportsmen.Length;
                for (int i = 0; i < grouplen; i++)
                {
                    var sportsman = group.Sportsmen[i];
                    writer.WriteLine($"{sportsman.Name} " +
                        $"{sportsman.Surname} " +
                        $"{sportsman.Time}");
                }

            }
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var fileLines = File.ReadAllLines(FilePath);

            var group = new Purple_4.Group(fileLines[0]);

            for (int i = 0; i < int.Parse(fileLines[1]); i++)
            {
                var sportsman_line_info = fileLines[i + 2].Split(' ');
                var sportsman = new Purple_4.Sportsman(sportsman_line_info[0], sportsman_line_info[1]);
                sportsman.Run(double.Parse(sportsman_line_info[2]));

                group.Add(sportsman);
            }

            return group;
        }

        // Purple_5
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            StringBuilder text_for_txt = new StringBuilder();

            int amount_of_researches = report.Researches.Length;
            text_for_txt.AppendLine($"ResearchesCount:{amount_of_researches}");

            for (int i = 0; i < amount_of_researches; i++)
            {
                text_for_txt.AppendLine($"Research{i}:{report.Researches[i].Name}");
                text_for_txt.AppendLine($"Responses{i}:{report.Researches[i].Responses.Length}");

                for (int j = 0; j < report.Researches[i].Responses.Length; j++)
                {
                    text_for_txt.AppendLine($"Animal{i}{j}:{report.Researches[i].Responses[j].Animal}");
                    text_for_txt.AppendLine($"characterTrait{i}{j}:{report.Researches[i].Responses[j].CharacterTrait}");
                    text_for_txt.AppendLine($"Concept{i}{j}:{report.Researches[i].Responses[j].Concept}");
                }
            }

            File.WriteAllText(FilePath, text_for_txt.ToString());
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            var text = File.ReadAllLines(FilePath);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var l in text)
            {
                if (l.Contains(":"))
                {
                    var topic_ans = l.Split(':');
                    dictionary[topic_ans[0].Trim()] = topic_ans[1].Trim();
                }
            }

            int ResearchesCount = int.Parse(dictionary["ResearchesCount"]);
            Purple_5.Report report_answer = new Purple_5.Report();

            for (int i = 0; i < ResearchesCount; i++)
            {
                string Research = dictionary[$"Research{i}"];
                int amount_of_responses = int.Parse(dictionary[$"Responses{i}"]);

                Purple_5.Research research = new Purple_5.Research(Research);

                for (int j = 0; j < amount_of_responses; j++)
                {
                    string? animal = dictionary[$"Animal{i}{j}"] == "" ? null : dictionary[$"Animal{i}{j}"];
                    string? characterTrait = dictionary[$"characterTrait{i}{j}"] == "" ? null : dictionary[$"characterTrait{i}{j}"];
                    string? concept = dictionary[$"Concept{i}{j}"] == "" ? null : dictionary[$"Concept{i}{j}"];
                    research.Add(new string[3] { animal, characterTrait, concept });
                }

                report_answer.AddResearch(research);
            }

            return report_answer;
        }

        private Purple_1.Participant DeserializeParticipant(string fileName)
        {
            SelectFile(fileName);
            var fileLines = File.ReadAllLines(FilePath);
            var participant = new Purple_1.Participant(fileLines[0], fileLines[1]);

            var coefParts = fileLines[2].Split(' ');
            var coefs = new double[coefParts.Length];

            for (int i = 0; i < coefParts.Length; i++)
            {
                coefs[i] = double.Parse(coefParts[i], CultureInfo.InvariantCulture);
            }

            participant.SetCriterias(coefs);


            var markParts = fileLines[3].Split(' ');
            var marks = new int[markParts.Length];

            for (int i = 0; i < markParts.Length; i++)
            {
                marks[i] = int.Parse(markParts[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                int[] MARKS = new int[7];
                var position_of_jump = i * 7;
                Array.Copy(marks, position_of_jump, MARKS, 0, 7);
                participant.Jump(MARKS);
            }

            return participant;
        }

        private Purple_1.Judge DeserializeJudge(string fileName)
        {
            SelectFile(fileName);
            var fileLines = File.ReadAllLines(FilePath);

            string[] marks = fileLines[1].Split(' ');
            int[] marks_massive = new int[marks.Length];

            for (int i = 0; i < marks.Length; i++)
            {
                marks_massive[i] = int.Parse(marks[i]);
            }

            return new Purple_1.Judge(fileLines[0], marks_massive);
        }
    }
}
