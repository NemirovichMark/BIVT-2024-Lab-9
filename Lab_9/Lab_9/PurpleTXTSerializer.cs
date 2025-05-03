using Lab_7;
using System.Globalization;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj is Purple_1.Participant participant)
            {
                SerializeParticipant(participant, fileName);
            } else if (obj is Purple_1.Judge judge)
            {
                SerializeJudge(judge, fileName);
            } else if (obj is Purple_1.Competition competition)
            {
                SerializeCompetition(competition, fileName);
            }
        }

        private void SerializeParticipant(Purple_1.Participant participant, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(participant.Name); // name
                writer.WriteLine(participant.Surname); // surname

                var coefs = string.Join(",",
                    participant.Coefs.Select(coef => coef.ToString(CultureInfo.InvariantCulture))
                    );
                writer.WriteLine(coefs); // coefs

                var marks = participant.Marks.Cast<int>().ToList();
                writer.WriteLine(string.Join(",", marks)); // marks
            }
        }

        private void SerializeJudge(Purple_1.Judge judge, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(judge.Name); // name
                writer.WriteLine(string.Join(",", judge.Marks)); // marks
            }
        }

        private void SerializeCompetition(Purple_1.Competition competition, string fileName)
        {
            SelectFile(fileName);

            var additionalData = Path.Combine(FolderPath, $"competitionData_${fileName}");

            if (!Directory.Exists(additionalData))
            {
                Directory.CreateDirectory(additionalData);
            }

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(competition.Judges.Length); // judges len
                for (int i = 0; i < competition.Judges.Length; i++)
                {
                    SerializeJudge(competition.Judges[i], Path.Combine(additionalData, $"judge{i}"));
                }
                writer.WriteLine(competition.Participants.Length); // participants len
                for (int i = 0; i < competition.Participants.Length; i++)
                {
                    SerializeParticipant(competition.Participants[i], Path.Combine(additionalData, $"participant{i}"));
                }
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

                var additionalData = Path.Combine(FolderPath, $"skijumpingParticipants_${fileName}");

                if (!Directory.Exists(additionalData))
                {
                    Directory.CreateDirectory(additionalData);
                }

                for (int i = 0; i < jumping.Participants.Length; i++)
                {
                    var fileParticipant = Path.Combine(additionalData, $"participant{i}.{Extension}");
                    using (StreamWriter participantWriter = new StreamWriter(fileParticipant))
                    {
                        participantWriter.WriteLine(jumping.Participants[i].Name); // name
                        participantWriter.WriteLine(jumping.Participants[i].Surname); // surname
                        participantWriter.WriteLine(jumping.Participants[i].Distance); // distance
                        participantWriter.WriteLine(jumping.Participants[i].Result); // result
                        participantWriter.WriteLine(string.Join(",", jumping.Participants[i].Marks)); // marks
                    }
                }

            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(skating.GetType().Name); // type
                writer.WriteLine(string.Join(";", skating.Moods)); // moods
                writer.WriteLine(skating.Participants.Length); // paritcipants length
                foreach (var participant in skating.Participants) // participants
                {
                    writer.WriteLine($"{participant.Name} {participant.Surname} {string.Join(";", participant.Marks)}");
                }
            }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(group.Name); // group name
                writer.WriteLine(group.Sportsmen.Length); // sportsmen len

                foreach (var sportsman in group.Sportsmen) // every sportsman
                {
                    writer.WriteLine($"{sportsman.Name} {sportsman.Surname} {sportsman.Time}");
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
                    if (research.Responses == null) continue;

                    writer.Write($" {research.Responses.Length}\n");
                    foreach (var response in research.Responses)
                    {
                        writer.WriteLine($"{response.Animal};{response.CharacterTrait};{response.Concept}");
                    }
                }
            }
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            if (typeof(T) == typeof(Purple_1.Participant))
            {
                return DeserializeParticipant(fileName) as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                return DeserializeJudge(fileName) as T;
            }
            else return DeserializeCompetition(fileName) as T;
        }

        private Purple_1.Participant DeserializeParticipant(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            Purple_1.Participant participant = new Purple_1.Participant(lines[0], lines[1]);
            
            var coefs = lines[2].Split(',').Select(coef => double.Parse(coef, CultureInfo.InvariantCulture)).ToArray();
            participant.SetCriterias(coefs);

            var marks = lines[3].Split(',').Select(int.Parse).ToArray();

            for (int i = 0; i < 4; i++)
            {
                int[] jumpMarks = new int[7];
                Array.Copy(marks, i * 7, jumpMarks, 0, 7);
                participant.Jump(jumpMarks);
            }

            return participant;
        }

        private Purple_1.Judge DeserializeJudge(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
           

            return new Purple_1.Judge(lines[0], lines[1].Split(',').Select(int.Parse).ToArray());
        }

        private Purple_1.Competition DeserializeCompetition(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            var folder = Path.Combine(FolderPath, $"competitionData_${fileName}");

            var judgesLen = int.Parse(lines[0]);
            var judges = new Purple_1.Judge[judgesLen];
            for (int i = 0; i < judgesLen; i++)
            {
                string judgeFile = Path.Combine(folder, $"judge{i}");
                judges[i] = DeserializeJudge(judgeFile);
            }

            Purple_1.Competition competition = new Purple_1.Competition(judges);

            var participantsLen = int.Parse(lines[1]);
            for (int i = 0; i< participantsLen; i++)
            {
                string participantFile = Path.Combine(folder, $"participant{i}");
                competition.Add(DeserializeParticipant(participantFile));
            }

            return competition;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);

            Purple_2.SkiJumping jumping;

            if (lines[0] == "100m") // lines[0] - name
            {
                jumping = new Purple_2.JuniorSkiJumping();
            } else
            {
                jumping = new Purple_2.ProSkiJumping();
            }

            var folder = Path.Combine(FolderPath, $"skijumpingParticipants_${fileName}");
            var participantsLen = int.Parse(lines[2]);
            Purple_2.Participant[] participants = new Purple_2.Participant[participantsLen];

            int standard = int.Parse(lines[1]);

            for (int i = 0; i < participantsLen; i++)
            {
                var participantLines = File.ReadAllLines(Path.Combine(folder, $"participant{i}.{Extension}"));
                var participant = new Purple_2.Participant(participantLines[0], participantLines[1]); // participantLines[0] - participant name, participantLines[1] - participant surname

                int distance = int.Parse(participantLines[2]);
                int result = int.Parse(participantLines[3]);
                int[] marks = participantLines[4].Split(',').Select(int.Parse).ToArray();

                participant.Jump(distance, marks, standard);
                participants[i] = participant;
            }

            jumping.Add(participants);

            return (T)jumping;

        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            var participantsLen = int.Parse(lines[2]);

            double[] moods = lines[1].Split(';').Select(double.Parse).ToArray();

            Purple_3.Skating skating;
            if (lines[0] == "FigureSkating")
            {
                skating = new Purple_3.FigureSkating(moods, needModificate: false);
            } else // IceSkating
            {
                skating = new Purple_3.IceSkating(moods, needModificate: false);
            }

            for (int i = 0; i < participantsLen; i++)
            {
                var participantInfo = lines[i + 3].Split(' '); // 0 - name 1 - surname 2 - marks
                var marks = participantInfo[2].Split(';').Select(double.Parse).ToArray();
                Purple_3.Participant participant = new Purple_3.Participant(participantInfo[0], participantInfo[1]);
                foreach (var mark in marks)
                {
                    participant.Evaluate(mark);
                }

                skating.Add(participant);
            }

            return (T)skating;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);

            Purple_4.Group group = new Purple_4.Group(lines[0]); // lines[0] - group name

            int sportsmenLen = int.Parse(lines[1]);

            for (int i = 0; i < sportsmenLen; i++)
            {
                var currLine = lines[i + 2].Split(' ');
                Purple_4.Sportsman sportsman = new Purple_4.Sportsman(currLine[0], currLine[1]); // currLine[0] - name, currLine[1] - surname
                sportsman.Run(double.Parse(currLine[2])); // currLine[2] - time

                group.Add(sportsman);
            }

            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            Purple_5.Report report = new Purple_5.Report();

            using (StreamReader reader = new StreamReader(FilePath))
            {
                int researchesLen = int.Parse(reader.ReadLine() ?? "0");
                for (int i = 0; i < researchesLen; i++)
                {
                    var info = reader.ReadLine().Split(' ');
                    var responsesLen = int.Parse(info[1] ?? "0");

                    Purple_5.Research research = new Purple_5.Research(info[0]); // info[0] - name, info[1] - responses count

                    for (int j = 0; j < responsesLen; j++)
                    {
                        var resp = reader.ReadLine().Split(';');

                        string? animal = string.IsNullOrEmpty(resp[0]) ? null : resp[0];
                        string? characterTrait = string.IsNullOrEmpty(resp[1]) ? null : resp[1];
                        string? concept = string.IsNullOrEmpty(resp[2]) ? null : resp[2];

                        research.Add(new string[] { animal, characterTrait, concept });
                    }

                    report.AddResearch(research);
                }
            }

            return report;
        }
    }
}
