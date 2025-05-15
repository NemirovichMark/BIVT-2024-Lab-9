using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static Lab_7.Purple_1;

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

                var marks = String.Join(" ", participant.Marks);
                writer.WriteLine(marks);
            }
        }

        private void SerializeJudge(Purple_1.Judge judge, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(judge.Name);

                var marks = String.Join(" ", judge.Marks);
                writer.WriteLine(marks);
            }
        }

        private void SerializeCompetition(Purple_1.Competition competition, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine("Judge");
                writer.WriteLine(competition.Judges.Length);
                foreach (var judge in competition.Judges)
                {
                    writer.WriteLine(judge.Name);
                    writer.Write(';');
                    writer.Write(String.Join(" ", judge.Marks));
                }

                writer.WriteLine("Participants");
                writer.WriteLine(competition.Participants.Length);
                foreach (var participant in competition.Participants)
                {
                    writer.WriteLine(participant.Name);
                    writer.Write(";");
                    writer.Write(participant.Surname);
                    writer.Write(";");

                    var coefs = String.Join(" ", participant.Coefs);
                    writer.Write(coefs);
                    writer.WriteLine(";");

                    var marks = String.Join(" ", participant.Marks);
                    writer.Write(marks);
                }
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);

            using(StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(jumping.Name);
                writer.WriteLine(jumping.Standard);

                writer.WriteLine(jumping.Participants.Length);

                foreach (var participant in jumping.Participants)
                {
                    writer.WriteLine(participant.Name);
                    writer.Write(";");
                    writer.Write(participant.Surname);
                    writer.Write(";");
                    writer.Write(participant.Distance);
                    writer.Write(";");
                    writer.Write(participant.Result);
                    writer.Write(";");

                    var marks = String.Join(" ", participant.Marks);
                    writer.Write(marks);
                }
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            using(StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(skating.GetType().Name);
                var moods = String.Join(" ", skating.Moods);
                writer.Write(moods);

                writer.WriteLine(skating.Participants.Length);
                foreach (var participant in skating.Participants)
                {
                    writer.WriteLine(participant.Name);
                    writer.Write(";");
                    writer.Write(participant.Surname);
                    writer.Write(";");

                    var marks = String.Join(" ", participant.Marks);
                    writer.Write(marks);
                }
            }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);

            using(StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(group.Name);
                writer.WriteLine(group.Sportsmen.Length);

                foreach(var sportsman in group.Sportsmen)
                {
                    writer.WriteLine(sportsman.Name);
                    writer.Write(";");
                    writer.Write(sportsman.Surname);
                    writer.Write(";");
                    writer.Write(sportsman.Time);
                }
            }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(report.Researches.Length);
                foreach(var research in report.Researches)
                {
                    writer.WriteLine(research.Name);
                    foreach (var response in research.Responses)
                    {
                        writer.WriteLine(response.Animal);
                        writer.Write(";");
                        writer.Write(response.CharacterTrait);
                        writer.Write(";");
                        writer.Write(response.Concept);
                    }
                }
            }
        }


        public override T DeserializePurple1<T>(string fileName)
        {
            throw new NotImplementedException();
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            throw new NotImplementedException();
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            throw new NotImplementedException();
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            throw new NotImplementedException();
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
