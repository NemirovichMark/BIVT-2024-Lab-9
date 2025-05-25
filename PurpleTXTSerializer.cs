using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            switch (obj)
            {
                case Purple_1.Participant paticipant:
                    SerializeParticipant(paticipant, fileName);
                    break;
                case Purple_1.Judge judge:
                    SerializeJudge(judge, fileName);
                    break;
                case Purple_1.Competition competition:
                    SerializeCompetition(competition, fileName);
                    break;
            }
        }

        private void SerializeParticipant(Purple_1.Participant participant, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(participant.Name);
                writer.WriteLine(participant.Surname);

                var coefs = String.Join(" ", participant.Coefs);
                writer.WriteLine(coefs);

                var marks = "";
                for (int i = 0; i < participant.Marks.GetLength(0); i++)
                {
                    var line = "";
                    for (int j = 0; j < participant.Marks.GetLength(1); j++)
                    {
                        line += $"{participant.Marks[i, j]} ";
                    }
                    marks += line.TrimEnd() + ';';
                }
                writer.WriteLine(marks.TrimEnd(';'));
                writer.WriteLine("participant");
            }
        }

        private void SerializeJudge(Purple_1.Judge judge, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(judge.Name);

                var marks = String.Join(" ", judge.Marks);
                writer.WriteLine(marks);
                writer.WriteLine("judge");
            }
        }

        private void SerializeCompetition(Purple_1.Competition competition, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(competition.Judges.Length);
                foreach (var judge in competition.Judges)
                {
                    writer.Write(judge.Name);
                    writer.Write(';');
                    writer.Write(String.Join(" ", judge.Marks));
                    writer.WriteLine();
                }

                writer.WriteLine(competition.Participants.Length);
                foreach (var participant in competition.Participants)
                {
                    writer.Write(participant.Name);
                    writer.Write(";");
                    writer.Write(participant.Surname);
                    writer.Write(";");

                    var coefs = String.Join(" ", participant.Coefs);
                    writer.Write(coefs);
                    writer.Write(";");

                    var marks = "";
                    for (int i = 0; i < participant.Marks.GetLength(0); i++)
                    {
                        var line = "";
                        for (int j = 0; j < participant.Marks.GetLength(1); j++)
                        {
                            line += $"{participant.Marks[i, j]} ";
                        }
                        marks += line.TrimEnd() + '|';
                    }
                    writer.Write(marks.TrimEnd('|'));
                    writer.WriteLine();
                }
                writer.WriteLine("competition");
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(jumping.Name);
                writer.WriteLine(jumping.Standard);

                writer.WriteLine(jumping.Participants.Length);

                foreach (var participant in jumping.Participants)
                {
                    writer.Write(participant.Name);
                    writer.Write(";");
                    writer.Write(participant.Surname);
                    writer.Write(";");
                    writer.Write(participant.Distance);
                    writer.Write(";");
                    writer.Write(participant.Result);
                    writer.Write(";");

                    var marks = String.Join(" ", participant.Marks);
                    writer.Write(marks);
                    writer.WriteLine();
                }
                if (jumping is Purple_2.JuniorSkiJumping)
                    writer.WriteLine("junior");
                else
                    writer.WriteLine("pro");
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                if (skating is Purple_3.IceSkating)
                    writer.WriteLine("ice");
                else
                    writer.WriteLine("figure");

                var moods = String.Join(" ", skating.Moods);
                writer.WriteLine(moods);

                writer.WriteLine(skating.Participants.Length);
                foreach (var participant in skating.Participants)
                {
                    writer.Write(participant.Name);
                    writer.Write(";");
                    writer.Write(participant.Surname);
                    writer.Write(";");

                    var marks = String.Join(" ", participant.Marks);
                    writer.Write(marks);
                    writer.WriteLine();
                }
            }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(group.Name);
                writer.WriteLine(group.Sportsmen.Length);

                foreach (var sportsman in group.Sportsmen)
                {
                    writer.Write(sportsman.Name);
                    writer.Write(";");
                    writer.Write(sportsman.Surname);
                    writer.Write(";");
                    writer.Write(sportsman.Time);
                    writer.WriteLine();
                }
            }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(report.Researches.Length);
                foreach (var research in report.Researches)
                {
                    writer.Write(research.Name);
                    writer.Write("|");
                    for(int i = 0; i < research.Responses.Length; i++) 
                    {
                        var response = research.Responses[i];
                        writer.Write(response.Animal);
                        writer.Write(";");
                        writer.Write(response.CharacterTrait);
                        writer.Write(";");
                        writer.Write(response.Concept);
                        if (i != research.Responses.Length - 1)
                            writer.Write(':');
                    }
                    writer.WriteLine();
                }
            }
        }


        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            string type = File.ReadAllLines(FilePath).Last();

            switch (type)
            {
                case "participant":
                    return DeserializeParticipant(fileName) as T;
                case "judge":
                    return DeserializeJudge(fileName) as T;
                case "competition":
                    return DeserializeCompetition(fileName) as T;
                default:
                    return default(T);
            }
        }
        private Purple_1.Participant DeserializeParticipant(string fileName)
        {
            //name, surname, coefs, marks

            string[] lines = File.ReadAllLines(FilePath);

            string name = lines[0];
            string surname = lines[1];
            double[] coefs = lines[2].Split(' ').Select(x => double.Parse(x)).ToArray();

            Purple_1.Participant participant = new Purple_1.Participant(name, surname);
            participant.SetCriterias(coefs);

            var marks_lines = lines[3].Split(';');
            int[] marks_line = new int[7];

            for (int i = 0; i < 4; i++)
            {
                marks_line = marks_lines[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                participant.Jump(marks_line);
            }

            return participant;
        }
        private Purple_1.Judge DeserializeJudge(string fileName)
        {
            //name, marks
            string[] lines = File.ReadAllLines(FilePath);

            string name = lines[0];
            int[] marks = lines[1].Split(' ').Select(x => int.Parse(x)).ToArray();

            Purple_1.Judge judge = new Purple_1.Judge(name, marks);
            return judge;
        }
        private Purple_1.Competition DeserializeCompetition(string fileName)
        {
            //Judges.Length, Name;marks, Participants.Length, Name;Surname;coefs;marks
            string[] lines = File.ReadAllLines(FilePath);

            int j_count = int.Parse(lines[0]);
            var judges = new Purple_1.Judge[j_count];
            for (int i = 1; i< j_count + 1; i++)
            {
                string[] line = lines[i].Split(';');
                string name = line[0];
                int[] marks = line[1].Split(' ').Select(x => int.Parse(x)).ToArray();
                var judge = new Purple_1.Judge(name, marks);
                judges[i-1] = judge;
                
            }

            int p_count = int.Parse(lines[j_count + 1]);
            var participants = new Purple_1.Participant[p_count];
            for (int i = j_count + 2; i < lines.Length - 1; i++)
            {
                string[] line = lines[i].Split(';');
                string name = line[0];
                string surname = line[1];
                double[] coefs = line[2].Split(' ').Select(x => double.Parse(x)).ToArray();

                Purple_1.Participant participant = new Purple_1.Participant(name, surname);
                participant.SetCriterias(coefs);

                var marks_lines = line[3].Split('|');
                int[] marks_line = new int[7];
                for (int j = 0; j < 4; j++)
                {
                    marks_line = marks_lines[j].Split(' ').Select(x => int.Parse(x)).ToArray();
                    participant.Jump(marks_line);
                }
                participants[i - j_count - 2] = participant;
            }
            
            var competition = new Purple_1.Competition(judges);
            competition.Add(participants);
            return competition;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {

            SelectFile(fileName);

            string[] lines = File.ReadAllLines(FilePath);
            string type = lines.Last();

            Purple_2.SkiJumping jumping;

            if (type == "junior")
                jumping = new Purple_2.JuniorSkiJumping();
            else
                jumping = new Purple_2.ProSkiJumping();

            string name = lines[0];
            int standard = int.Parse(lines[1]);
            int p_count = int.Parse(lines[2]);
            Purple_2.Participant[] participants = new Purple_2.Participant[p_count];

            for (int i = 3; i < lines.Length - 1; i++)
            {
                string[] line = lines[i].Split(';');
                string Name = line[0];
                string Surname = line[1];
                int Distance = int.Parse(line[2]);
                int Result = int.Parse(line[3]);
                int[] Marks = line[4].Split(' ').Select(x => int.Parse(x)).ToArray();

                var participant = new Purple_2.Participant(Name, Surname);
                participant.Jump(Distance, Marks, standard);
                participants[i-3] = participant;
            }

            jumping.Add(participants);

            return jumping as T;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            string[] lines = File.ReadAllLines(FilePath);

            string type = lines[0];
            double[] moods = lines[1].Split(' ').Select(x=>double.Parse(x)).ToArray();
            int p_counter = int.Parse(lines[2]);

            Purple_3.Participant[] participants = new Purple_3.Participant[p_counter];

            for (int i = 3; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(';');

                string name = line[0];
                string surname = line[1];
                double[] marks = line[2].Split(' ').Select(x => double.Parse(x)).ToArray();

                var participant = new Purple_3.Participant(name, surname);
                foreach (double result in marks)
                    participant.Evaluate(result);

                participants[i - 3] = participant;
            }

            Purple_3.Skating skating;
            if (type == "ice")
                skating = new Purple_3.IceSkating(moods, false);
            else 
                skating = new Purple_3.FigureSkating(moods, false);

            skating.Add(participants);
            return skating as T;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            string[] lines = File.ReadAllLines(FilePath);

            string name = lines[0];
            int s_count = int.Parse(lines[1]);
            Purple_4.Sportsman[] sportsmen = new Purple_4.Sportsman[s_count];

            for (int i = 2; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(';');

                string Name = line[0];
                string Surname = line[1];
                double Time = double.Parse(line[2]);

                Purple_4.Sportsman sportsman = new Purple_4.Sportsman(Name, Surname);
                sportsman.Run(Time);
                sportsmen[i - 2] = sportsman;
            }

            Purple_4.Group group = new Purple_4.Group(name);
            group.Add(sportsmen);
            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            string[] lines = File.ReadAllLines(FilePath);
            int r_count = int.Parse(lines[0]);
            Purple_5.Research[] researches = new Purple_5.Research[r_count];

            for(int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split('|');
                string name = line[0];
                string[] responses = line[1].Split(':');
                Purple_5.Research research = new Purple_5.Research(name);
                foreach(string res in responses)
                {
                    string[] answers = res.Split(';');
                    if (answers != null)
                    {
                        for(int j = 0; j < answers.Length; j++)
                        {
                            if (string.IsNullOrEmpty(answers[j]))
                                answers[j] = null;
                        }
                    }
                    research.Add(answers);
                }
                researches[i - 1] = research;
            }

            Purple_5.Report report = new Purple_5.Report();
            report.AddResearch(researches);
            return report;
        }
    }
}
