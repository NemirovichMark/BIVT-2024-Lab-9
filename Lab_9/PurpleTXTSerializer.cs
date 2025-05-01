using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_1;
using static Lab_7.Purple_2;
using static Lab_7.Purple_3;
using static Lab_7.Purple_5;

namespace Lab_9
{
    public class PurpleTXTSerializer: PurpleSerializer
    {
        public override string Extension => "txt";

        private void writePurple1Participant(StreamWriter writer, Purple_1.Participant participant)
        {
            writer.WriteLine($"Type: {nameof(Purple_1.Participant)}");
            writer.WriteLine($"Name: {participant.Name}");
            writer.WriteLine($"Surname: {participant.Surname}");
            writer.WriteLine($"Coefs: {String.Join(" ", participant.Coefs)}");

            int[,] marks = participant.Marks;
            int[] partMarks = new int[28];

            for (int i = 0, k = 0; i < 4; i++) {
                for (int j = 0; j < 7; j++) {
                    partMarks[k++] = marks[i, j];
                }
            }
            writer.WriteLine($"Marks: {String.Join(" ", partMarks)}");
            writer.WriteLine($"TotalScore: {participant.TotalScore}");

            // Console.WriteLine($"Name: {participant.Name}");
            // Console.WriteLine($"Surname: {participant.Surname}");
            // Console.WriteLine($"Coefs: {String.Join(" ", participant.Coefs)}");
            // Console.WriteLine($"Marks: {String.Join(" ", partMarks)}");
            
            // Console.WriteLine($"TotalScore: {participant.TotalScore}");

        }
        private void writePurple1Judge(StreamWriter writer, Purple_1.Judge judge)
        {
            writer.WriteLine($"Type: {nameof(Purple_1.Judge)}");
            writer.WriteLine($"Name: {judge.Name}");
            writer.WriteLine($"Marks: {String.Join(" ", judge.Marks)}");
            
            // Console.WriteLine($"Name: {judge.Name}");
            // Console.WriteLine($"Marks: {String.Join(" ", judge.Marks)}");
        }
        public override void SerializePurple1<T>(T obj, string fileName) where T : class
        {
            if (obj == null)
                return;
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                
                if (obj.GetType().Name == nameof(Purple_1.Participant))
                {
                    Purple_1.Participant participant = obj as Purple_1.Participant;
                    writePurple1Participant(writer, participant);
                } else if (obj.GetType().Name == nameof(Purple_1.Judge)) { 
                    Purple_1.Judge judge = obj as Purple_1.Judge;
                    writePurple1Judge(writer, judge);
                } else if (obj.GetType().Name == nameof(Purple_1.Competition)) {
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

                    // System.Console.WriteLine($"SerializePartsLen: {participants.Length}");
                    foreach(Purple_1.Participant participant in participants) {
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
            writer.WriteLine($"Marks: {String.Join(" ", participant.Marks)}");
            writer.WriteLine($"Result: {participant.Result}");

            Console.WriteLine($"Name: {participant.Name}");
            Console.WriteLine($"Surname: {participant.Surname}");
            Console.WriteLine($"Distance: {participant.Distance}");
            Console.WriteLine($"Marks: {String.Join(" ", participant.Marks)}");
            Console.WriteLine($"Result: {participant.Result}");
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (jumping == null) return;

            SelectFile(fileName);
           

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                if (jumping is Purple_2.JuniorSkiJumping)
                {
                    writer.WriteLine($"Type: {nameof(Purple_2.JuniorSkiJumping)}");
                }
                else if (jumping is Purple_2.ProSkiJumping)
                {
                    writer.WriteLine($"Type: {nameof(Purple_2.ProSkiJumping)}");
                } else
                {
                    writer.WriteLine($"Type: NULL");
                }
                writer.WriteLine($"Name: {jumping.Name}");
                writer.WriteLine($"Standard: {jumping.Standard}");
                Purple_2.Participant[] participants = jumping.Participants;
                writer.WriteLine(participants.Length);
                foreach(Purple_2.Participant participant in participants)
                    writePurple2Participant(writer, participant);
            }
        }
        private void writePurple3Participant(StreamWriter writer, Purple_3.Participant participant)
        {
            writer.WriteLine($"Type: {nameof(Purple_3.Participant)}");
            writer.WriteLine($"Name: {participant.Name}");
            writer.WriteLine($"Surname: {participant.Surname}");
            writer.WriteLine($"Marks: {String.Join(" ", participant.Marks)}");
            writer.WriteLine($"Places: {String.Join(" ", participant.Places)}");

            // Console.WriteLine($"Type: {nameof(Purple_3.Participant)}");
            // Console.WriteLine($"Name: {participant.Name}");
            // Console.WriteLine($"Surname: {participant.Surname}");
            // Console.WriteLine($"Marks: {String.Join(" ", participant.Marks)}");
            // Console.WriteLine($"Places: {String.Join(" ", participant.Places)}");
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            if (skating == null) return;
            SelectFile(fileName);


            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                double[] moods = skating.Moods;
                if (skating is Purple_3.IceSkating)
                {
                    writer.WriteLine($"Type: {nameof(Purple_3.IceSkating)}");
                    for (int i = 0; i < moods.Length; i++) 
                        moods[i] /= (1 + (i + 1) / 100.0);
                }
                else if (skating is Purple_3.FigureSkating)
                {
                    writer.WriteLine($"Type: {nameof(Purple_3.FigureSkating)}");
                    for (int i = 0; i < moods.Length; i++) 
                        moods[i] -= ((i + 1) / 10.0);
                } else
                {
                    writer.WriteLine($"Type: NULL");
                }
                Purple_3.Participant[] participants = skating.Participants;
                if (participants == null)
                    participants = new Purple_3.Participant[0];
                writer.WriteLine(participants.Length);
                foreach (Purple_3.Participant participant in participants)
                    writePurple3Participant(writer, participant);
                
                writer.WriteLine($"Moods: {String.Join(" ", moods)}");
                // Console.WriteLine($"Moods: {String.Join(" ", skating.Moods)}");
            }
        }
        private void writePurple4Sportsman(StreamWriter writer, Purple_4.Sportsman sportsman)
        {
            if (sportsman is null)
            {
                writer.WriteLine($"Type: NULL");
                return;
            } else if (sportsman is Purple_4.SkiMan)
            {
                writer.WriteLine($"Type: {nameof(Purple_4.SkiMan)}");
            } else if (sportsman is Purple_4.SkiWoman)
            {
                writer.WriteLine($"Type: {nameof(Purple_4.SkiWoman)}");
            } else {
                writer.WriteLine($"Type: {nameof(Purple_4.Sportsman)}");
            }
            
            
            writer.WriteLine($"Name: {sportsman.Name}");
            writer.WriteLine($"Surname: {sportsman.Surname}");
            writer.WriteLine($"Time: {sportsman.Time}");

            // Console.WriteLine($"Type: {sportsman.GetType().Name}");
            // Console.WriteLine($"Name: {sportsman.Name}");
            // Console.WriteLine($"Surname: {sportsman.Surname}");
            // Console.WriteLine($"Time: {sportsman.Time}");
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            if (group == null) return;

            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
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
            writer.WriteLine(responses.Length);
            foreach(Purple_5.Response response in responses)
                writePurple5Response(writer, response);
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            if (report == null) return;

            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine($"Type: {nameof(report)}");
                Purple_5.Research[] researches = report.Researches;
                if (researches == null)
                    researches = new Purple_5.Research[0];
                writer.WriteLine(researches.Length);
                foreach (Purple_5.Research research in researches)
                    writePurple5Research(writer, research);
            }
        }
        public override T DeserializePurple1<T>(string fileName) 
        {
            if (fileName == null) {
                System.Console.WriteLine("HERE");
                return null;
            }
                
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string type = reader.ReadLine().Split(':')[1].Trim();
                // System.Console.WriteLine($"TYPE {type} {nameof(Purple_1.Participant)} {type == nameof(Purple_1.Participant)}");
                if (type == nameof(Purple_1.Participant))
                {
                    string name = reader.ReadLine().Split(':')[1].Trim();
                    // System.Console.WriteLine($"Name: {name}");
                    string surname = reader.ReadLine().Split(':')[1].Trim();
                    string[] inputCoefs = reader.ReadLine().Split(':')[1].Trim().Split();
                    string[] inputMarks = reader.ReadLine().Split(':')[1].Trim().Split();

                    
                    // Console.WriteLine($"Surname: {surname}");
                    // Console.WriteLine($"Coefs: {String.Join(" ", inputCoefs)}");
                    
                    // Console.WriteLine($"TotalScore: {inputMarks}");

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

                } else if (type == nameof(Purple_1.Judge))
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
                } else
                {
                    int numParticipants;
                    Int32.TryParse(reader.ReadLine().Trim(), out numParticipants);

                    Purple_1.Participant[] participants = new Purple_1.Participant[numParticipants];

                    for (int i = 0; i < numParticipants; i++)
                    {
                        string tp = reader.ReadLine();
                        string name = reader.ReadLine().Split(':')[1].Trim();
                        string surname = reader.ReadLine().Split(':')[1].Trim();
                        string buf1 = reader.ReadLine();
                        string[] inputCoefs = buf1.Split(':')[1].Trim().Split();
                        string buf = reader.ReadLine();
                        string[] inputMarks = buf.Split(':')[1].Trim().Split();
                        double totalScore;
                        double.TryParse(reader.ReadLine().Trim(), out totalScore);

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
            if (fileName == null) {
                return null;
            }
            // System.Console.WriteLine("DESI*******************************");
                
            SelectFile(fileName);
            // System.Console.WriteLine($"FILENAME {fileName}");
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string type = reader.ReadLine().Split(':')[1].Trim();
                string jumpName = reader.ReadLine().Split(':')[1].Trim();
                int standard;
                Int32.TryParse(reader.ReadLine().Split(":")[1].Trim(), out standard);

                int numParticipants;
                Int32.TryParse(reader.ReadLine().Trim(), out numParticipants);

                Purple_2.Participant[] participants = new Purple_2.Participant[numParticipants];

                int[][] partMarks = new int[numParticipants][];
                int[] partDistances = new int[numParticipants];

                for (int i = 0; i < numParticipants; i++) {
                    string tp = reader.ReadLine().Split(':')[1].Trim();
                    string partName = reader.ReadLine().Split(':')[1].Trim();
                    string partSurname = reader.ReadLine().Split(':')[1].Trim();

                    Int32.TryParse(reader.ReadLine().Split(":")[1].Trim(), out partDistances[i]);
                    
                    // System.Console.WriteLine($"PartDist {i} {partDistances[i]}");

                    string[] marks = reader.ReadLine().Split(':')[1].Trim().Split();
                    partMarks[i] = new int[marks.Length];
                    for (int j = 0; j < marks.Length; j++)
                    {
                        // System.Console.WriteLine($"Mark {marks[j].Trim()}");
                        
                        Int32.TryParse(marks[j].Trim(), out partMarks[i][j]);
                    }
                    int partResult;
                    Int32.TryParse(reader.ReadLine().Split(":")[1].Trim(), out partResult);

                    participants[i] = new Purple_2.Participant(partName, partSurname);
                    participants[i].Jump(partDistances[i], partMarks[i], standard);
                    // Console.WriteLine($"Name: {participants[i].Name}");
                    // Console.WriteLine($"Surname: {participants[i].Surname}");
                    // Console.WriteLine($"Distance: {partDistances[i]}");
                    // Console.WriteLine($"Marks: {String.Join(" ", partMarks[i])}");
                    // Console.WriteLine($"Result: {partResult}");
                }
                
                

                // System.Console.WriteLine($"TYPE {type} {nameof(Purple_1.Participant)} {type == nameof(Purple_1.Participant)}");
                if (type == nameof(Purple_2.JuniorSkiJumping))
                {
                    Purple_2.JuniorSkiJumping juniorSkiJumping = new Purple_2.JuniorSkiJumping();
                    juniorSkiJumping.Add(participants);
                    // for (int i = 0; i < numParticipants; i++)
                    //     juniorSkiJumping.Jump(partDistances[i], partMarks[i]);
                    T res = juniorSkiJumping as T;
                    return res;
                } else if (type == nameof(Purple_2.ProSkiJumping))
                {
                    Purple_2.ProSkiJumping proSkiJumping = new Purple_2.ProSkiJumping();
                    proSkiJumping.Add(participants);
                    // for (int i = 0; i < numParticipants; i++)
                    //     proSkiJumping.Jump(partDistances[i], partMarks[i]);
                    T res = proSkiJumping as T;
                    return res;
                } else {
                    return null;
                }

            }
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            if (fileName == null) {
                return null;
            }
            // System.Console.WriteLine("DESI*******************************");
                
            SelectFile(fileName);
            // System.Console.WriteLine($"FILENAME {fileName}");
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string type = reader.ReadLine().Split(':')[1].Trim();
                int numParticipants;
                Int32.TryParse(reader.ReadLine().Trim(), out numParticipants);

                Purple_3.Participant[] participants = new Purple_3.Participant[numParticipants];


                for (int i = 0; i < numParticipants; i++) {
                    string tp = reader.ReadLine().Split(':')[1].Trim();
                    string partName = reader.ReadLine().Split(':')[1].Trim();
                    string partSurname = reader.ReadLine().Split(':')[1].Trim();

                    
                    // System.Console.WriteLine($"PartDist {i} {partDistances[i]}");

                    string[] inPutMarks = reader.ReadLine().Split(':')[1].Trim().Split();
                    double[] partMarks = new double[inPutMarks.Length];
                    for (int j = 0; j < inPutMarks.Length; j++)
                    {
                        // System.Console.WriteLine($"Mark {marks[j].Trim()}");
                        
                        Double.TryParse(inPutMarks[j].Trim(), out partMarks[j]);
                    }
                    string[] places = reader.ReadLine().Split(':')[1].Trim().Split();

                    participants[i] = new Purple_3.Participant(partName, partSurname);
                    
                    for (int j = 0; j < inPutMarks.Length; j++) {
                        participants[i].Evaluate(partMarks[j]);
                    }
                    Console.WriteLine($"Name: {participants[i].Name}");
                    Console.WriteLine($"Surname: {participants[i].Surname}");
                    Console.WriteLine($"Marks: {String.Join(" ", partMarks)}");
                }
                
                string[] inputMoods = reader.ReadLine().Split(':')[1].Trim().Split();
                double[] moods = new double[inputMoods.Length];

                for (int i = 0; i < inputMoods.Length; i++) {
                    double.TryParse(inputMoods[i].Trim(), out moods[i]);
                }
                

                // System.Console.WriteLine($"TYPE {type} {nameof(Purple_1.Participant)} {type == nameof(Purple_1.Participant)}");
                if (type == nameof(Purple_3.IceSkating))
                {
                    Purple_3.IceSkating iceSkating = new Purple_3.IceSkating(moods);
                    iceSkating.Add(participants);
                    T res = iceSkating as T;
                    return res;
                } else 
                {
                    Purple_3.FigureSkating figureSkating = new Purple_3.FigureSkating(moods);
                    figureSkating.Add(participants);
                    T res = figureSkating as T;
                    return res;
                }
            }
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {

            if (fileName == null) {
                return null;
            }
            // System.Console.WriteLine("DESI*******************************");
                
            SelectFile(fileName);
            // System.Console.WriteLine($"FILENAME {fileName}");
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string type = reader.ReadLine().Split(':')[1].Trim();
                string groupName = reader.ReadLine().Split(':')[1].Trim();
                int numSportsmen;
                Int32.TryParse(reader.ReadLine().Trim(), out numSportsmen);

                Purple_4.Sportsman[] sportsmen = new Purple_4.Sportsman[numSportsmen];


                for (int i = 0; i < numSportsmen; i++) {
                    string tp = reader.ReadLine().Split(':')[1].Trim();
                    System.Console.WriteLine(tp);
                    string partName = reader.ReadLine().Split(':')[1].Trim();
                    System.Console.WriteLine(partName);
                    string partSurname = reader.ReadLine().Split(':')[1].Trim();
                    double sportTime;
                    double.TryParse(reader.ReadLine().Split(':')[1].Trim(), out sportTime);

                    
                    // System.Console.WriteLine($"PartDist {i} {partDistances[i]}");

                    if (tp == nameof(Purple_4.SkiMan)) {
                        sportsmen[i] = new Purple_4.SkiMan(partName, partSurname);
                    } else if (tp == nameof(Purple_4.SkiMan)) {
                        sportsmen[i] = new Purple_4.SkiWoman(partName, partSurname);
                    } else {
                        sportsmen[i] = new Purple_4.Sportsman(partName, partSurname);
                    }
                    sportsmen[i].Run(sportTime);
                }
                
            
                Purple_4.Group group = new Purple_4.Group(groupName);
                group.Add(sportsmen);

                return group;
            }
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            if (fileName == null) {
                return null;
            }
            // System.Console.WriteLine("DESI*******************************");
                
            SelectFile(fileName);
            // System.Console.WriteLine($"FILENAME {fileName}");
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string type = reader.ReadLine().Split(':')[1].Trim();
            
                int numResearches;
                Int32.TryParse(reader.ReadLine().Trim(), out numResearches);


                Purple_5.Research[] researches = new Purple_5.Research[numResearches];


                for (int i = 0; i < numResearches; i++) {
                    string tpRsch = reader.ReadLine().Split(':')[1].Trim();
                    string rschName = reader.ReadLine().Split(':')[1].Trim();
                    int numResponses;
                    int.TryParse(reader.ReadLine().Trim(), out numResponses);
                    researches[i] = new Purple_5.Research(rschName);
                    
                    // System.Console.WriteLine($"PartDist {i} {partDistances[i]}");

                    for (int j = 0; j < numResponses; j++) {
                        string tp = reader.ReadLine().Split(':')[1].Trim();
                        string animal = reader.ReadLine().Split(':')[1].Trim();
                        string trait = reader.ReadLine().Split(':')[1].Trim();
                        string concept = reader.ReadLine().Split(':')[1].Trim();
                        if (string.IsNullOrEmpty(animal))
                            animal = null;
                        if (string.IsNullOrEmpty(trait))
                            trait = null;
                        if (string.IsNullOrEmpty(concept))
                            concept = null;
                        researches[i].Add(new string[] { animal, trait, concept });
                    }
                }
                
            
                Purple_5.Report report = new Purple_5.Report();
                report.AddResearch(researches);

                return report;
            }
        }
    }
}
