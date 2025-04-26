using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class PurpleXMLSerializer: PurpleSerializer
    {
        public override string Extension => "xml";

        private void writePurple1Participant(StreamWriter writer, Purple_1.Participant participant)
        {
            writer.WriteLine($"Type: {nameof(Purple_1.Participant)}");
            writer.WriteLine($"Name: {participant.Name}");
            writer.WriteLine($"Surname: {participant.Surname}");
            writer.WriteLine($"Coefs: {String.Join(" ", participant.Coefs)}");
            writer.WriteLine($"Marks: {String.Join(" ", participant.Marks.Cast<int>().ToArray())}");
            writer.WriteLine($"TotalScore: {participant.TotalScore}");

        }
        private void writePurple1Judge(StreamWriter writer, Purple_1.Judge judge)
        {
            writer.WriteLine($"Type: {nameof(Purple_1.Judge)}");
            writer.WriteLine($"Name: {judge.Name}");
            writer.WriteLine($"Marks: {String.Join(" ", judge.Marks.Cast<int>().ToArray())}");
        }
        public override void SerializePurple1<T>(T obj, string fileName) where T : class
        {
            if (obj == null)
                return;
            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                if (obj.GetType().Name == nameof(Purple_1.Participant))
                {
                    Purple_1.Participant participant = obj as Purple_1.Participant;
                    writePurple1Participant(writer, participant);
                }
                else if (obj.GetType().Name == nameof(Purple_1.Judge))
                {
                    Purple_1.Judge judge = obj as Purple_1.Judge;
                    writePurple1Judge(writer, judge);
                }
                else if (obj.GetType().Name == nameof(Purple_1.Competition))
                {
                    Purple_1.Competition competition = obj as Purple_1.Competition;
                    Purple_1.Participant[] participants = competition.Participants;
                    Purple_1.Judge[] judges = competition.Judges;

                    if (participants == null)
                    {
                        participants = new Purple_1.Participant[0];
                    }
                    if (judges == null)
                    {
                        judges = new Purple_1.Judge[0];
                    }

                    writer.WriteLine($"Type: {nameof(Purple_1.Competition)}");
                    writer.WriteLine(participants.Length);
                    foreach (Purple_1.Participant participant in participants)
                    {
                        writePurple1Participant(writer, participant);
                    }

                    writer.WriteLine(judges.Length);
                    foreach (Purple_1.Judge judge in judges)
                    {
                        writePurple1Judge(writer, judge);
                    }
                }
            }
        }
        private void writePurple2Participant(StreamWriter writer, Purple_2.Participant participant)
        {
            writer.WriteLine($"Type: {nameof(Purple_2.Participant)}");
            writer.WriteLine($"Name: {participant.Name}");
            writer.WriteLine($"Surname: {participant.Surname}");
            writer.WriteLine($"Distance: {participant.Distance}");
            writer.WriteLine($"Marks: {String.Join(" ", participant.Marks.Cast<int>().ToArray())}");
            writer.WriteLine($"Result: {participant.Result}");
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (jumping == null) return;

            SelectFile(fileName);


            using (StreamWriter writer = File.AppendText(FilePath))
            {
                if (jumping is Purple_2.JuniorSkiJumping)
                {
                    writer.WriteLine($"Type: {nameof(Purple_2.JuniorSkiJumping)}");
                }
                else if (jumping is Purple_2.ProSkiJumping)
                {
                    writer.WriteLine($"Type: {nameof(Purple_2.ProSkiJumping)}");
                }
                else
                {
                    writer.WriteLine($"Type: NULL");
                }
                writer.WriteLine($"Name: {jumping.Name}");
                writer.WriteLine($"Standard: {jumping.Standard}");
                Purple_2.Participant[] participants = jumping.Participants;
                writer.WriteLine(participants.Length);
                foreach (Purple_2.Participant participant in participants)
                    writePurple2Participant(writer, participant);
            }
        }
        private void writePurple3Participant(StreamWriter writer, Purple_3.Participant participant)
        {
            writer.WriteLine($"Type: {nameof(Purple_3.Participant)}");
            writer.WriteLine($"Name: {participant.Name}");
            writer.WriteLine($"Surname: {participant.Surname}");
            writer.WriteLine($"Marks: {String.Join(" ", participant.Marks.Cast<int>().ToArray())}");
            writer.WriteLine($"Places: {String.Join(" ", participant.Places)}");
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            if (skating == null) return;
            SelectFile(fileName);


            using (StreamWriter writer = File.AppendText(FilePath))
            {

                if (skating is Purple_3.IceSkating)
                {
                    writer.WriteLine($"Type: {nameof(Purple_3.IceSkating)}");
                }
                else if (skating is Purple_3.FigureSkating)
                {
                    writer.WriteLine($"Type: {nameof(Purple_3.FigureSkating)}");
                }
                else
                {
                    writer.WriteLine($"Type: NULL");
                }
                Purple_3.Participant[] participants = skating.Participants;
                if (participants == null)
                    participants = new Purple_3.Participant[0];
                writer.WriteLine(participants.Length);
                foreach (Purple_3.Participant participant in participants)
                    writePurple3Participant(writer, participant);
                writer.WriteLine($"Moods: {String.Join(" ", skating.Moods)}");
            }
        }
        private void writePurple4Sportsman(StreamWriter writer, Purple_4.Sportsman sportsman)
        {
            if (sportsman == null)
            {
                writer.WriteLine($"Type: NULL");
                return;
            }
            else if (sportsman is Purple_4.SkiMan)
            {
                writer.WriteLine($"Type: {nameof(Purple_4.SkiMan)}");
            }
            else if (sportsman is Purple_4.SkiWoman)
            {
                writer.WriteLine($"Type: {nameof(Purple_4.SkiWoman)}");
            }

            writer.WriteLine($"Name: {sportsman.Name}");
            writer.WriteLine($"Surname: {sportsman.Surname}");
            writer.WriteLine($"Time: {sportsman.Time}");
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            if (group == null) return;

            SelectFile(fileName);

            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Type: {nameof(group)}");
                writer.WriteLine($"Name: {group.Name}");
                Purple_4.Sportsman[] sportsmen = group.Sportsmen;
                if (sportsmen == null)
                    sportsmen = new Purple_4.Sportsman[0];
                writer.WriteLine(sportsmen.Length);
                foreach (Purple_4.Sportsman sportsman in sportsmen)
                    writePurple4Sportsman(writer, sportsman);
            }

        }

        private void writePurple5Response(StreamWriter writer, Purple_5.Response response)
        {
            writer.WriteLine($"Type: {nameof(response)}");
            writer.WriteLine($"Animal: {response.Animal}");
            writer.WriteLine($"CharacterTrait: {response.CharacterTrait}");
            writer.WriteLine($"Concept: {response.Concept}");
        }

        private void writePurple5Research(StreamWriter writer, Purple_5.Research research)
        {
            writer.WriteLine($"Type: {nameof(research)}");
            writer.WriteLine($"Name: {research.Name}");

            Purple_5.Response[] responses = research.Responses;
            if (responses == null)
                responses = new Purple_5.Response[0];

            foreach (Purple_5.Response response in responses)
                writePurple5Response(writer, response);
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            if (report == null) return;

            SelectFile(fileName);

            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Type: {nameof(report)}");
                Purple_5.Research[] researches = report.Researches;
                if (researches == null)
                    researches = new Purple_5.Research[0];

                foreach (Purple_5.Research research in researches)
                    writePurple5Research(writer, research);
            }
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string type = reader.ReadLine();
                if (type == nameof(Purple_1.Participant))
                {
                    string name = reader.ReadLine().Split(':')[1].Trim();
                    string surname = reader.ReadLine().Split(':')[1].Trim();
                    string[] inputCoefs = reader.ReadLine().Split(':')[1].Trim().Split();
                    string[] inputMarks = reader.ReadLine().Split(':')[1].Trim().Split();

                    double[] coefs = new double[inputCoefs.Length];

                    for (int i = 0; i < inputCoefs.Length; i++)
                    {
                        double.TryParse(inputCoefs[i].Trim(), out coefs[i]);
                    }
                    Purple_1.Participant participant = new Purple_1.Participant(name, surname);
                    participant.SetCriterias(coefs);

                    int[] marks = new int[7];

                    for (int i = 0, k = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            Int32.TryParse(inputMarks[k++].Trim(), out marks[j]);
                        }
                        participant.Jump(marks);
                    }
                    T part = participant as T;
                    return part;

                }
                else if (type == nameof(Purple_1.Judge))
                {
                    string name = reader.ReadLine().Split(':')[1].Trim();
                    string[] inputMarks = reader.ReadLine().Split(':')[1].Trim().Split();

                    int[] marks = new int[inputMarks.Length];

                    for (int i = 0; i < inputMarks.Length; i++)
                    {
                        Int32.TryParse(inputMarks[i].Trim(), out marks[i]);
                    }

                    Purple_1.Judge judge = new Purple_1.Judge(name, marks);

                    T jdg = judge as T;
                    return jdg;
                }
                else
                {
                    int numParticipants;
                    Int32.TryParse(reader.ReadLine().Trim(), out numParticipants);

                    Purple_1.Participant[] participants = new Purple_1.Participant[numParticipants];

                    for (int i = 0; i < numParticipants; i++)
                    {
                        string tp = reader.ReadLine();
                        string name = reader.ReadLine().Split(':')[1].Trim();
                        string surname = reader.ReadLine().Split(':')[1].Trim();
                        string[] inputCoefs = reader.ReadLine().Split(':')[1].Trim().Split();
                        string[] inputMarks = reader.ReadLine().Split(':')[1].Trim().Split();

                        double[] coefs = new double[inputCoefs.Length];

                        for (int j = 0; j < inputCoefs.Length; j++)
                        {
                            double.TryParse(inputCoefs[j].Trim(), out coefs[j]);
                        }
                        participants[i] = new Purple_1.Participant(name, surname);
                        participants[i].SetCriterias(coefs);

                        int[] marks = new int[7];

                        for (int j = 0, k = 0; j < 4; j++)
                        {
                            for (int z = 0; z < 7; z++)
                            {
                                Int32.TryParse(inputMarks[k++].Trim(), out marks[z]);
                            }
                            participants[i].Jump(marks);
                        }
                    }

                    int numJudges;
                    Int32.TryParse(reader.ReadLine().Trim(), out numJudges);

                    Purple_1.Judge[] judges = new Purple_1.Judge[numJudges];

                    for (int i = 0; i < numJudges; i++)
                    {
                        string tp = reader.ReadLine();
                        string name = reader.ReadLine().Split(':')[1].Trim();
                        string[] inputMarks = reader.ReadLine().Split(':')[1].Trim().Split();

                        int[] marks = new int[inputMarks.Length];

                        for (int j = 0; j < inputMarks.Length; j++)
                        {
                            Int32.TryParse(inputMarks[j].Trim(), out marks[j]);
                        }

                        judges[i] = new Purple_1.Judge(name, marks);
                    }

                    Purple_1.Competition competition = new Purple_1.Competition(judges);
                    competition.Add(participants);

                    T comp = competition as T;
                    return comp;
                }

            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            return null;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            return null;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            return null;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            return null;
        }
    }
}
