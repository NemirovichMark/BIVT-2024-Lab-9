using Lab_7;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static Lab_7.Purple_1;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";
        private T deserializer<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            T deserializeObject;
            using (var reader = File.OpenRead(FilePath)) 
                deserializeObject = (T)serializer.Deserialize(reader);
   
            return deserializeObject;
        }
        #region Purple_1

        public class Participant_Purple1_DTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[] Marks { get; set; }
        }

        public class Judge_Purple1_DTO
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
        }

        public class Competition_Purple1_DTO
        {
            public Judge_Purple1_DTO[] Judges { get; set; }
            public Participant_Purple1_DTO[] Participants { get; set; }
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            if (obj is Purple_1.Participant participant)
            {
                var d = new Participant_Purple1_DTO()
                {
                    Name = participant.Name,
                    Surname = participant.Surname,
                    Coefs = participant.Coefs,
                    Marks = participant.Marks.Cast<int>().ToArray(),
                };
                var serializer = new XmlSerializer(typeof(Participant_Purple1_DTO));
                using (var writer = XmlWriter.Create(FilePath))
                {
                    serializer.Serialize(writer, d);
                }
            }
            else if (obj is Purple_1.Judge judge)
            {
                var d = new Judge_Purple1_DTO()
                {
                    Name = judge.Name,
                    Marks = judge.Marks,
                };
                var serializer = new XmlSerializer(typeof(Judge_Purple1_DTO));
                using (var writer = XmlWriter.Create(FilePath)) { serializer.Serialize(writer, d); };
            }
            else if (obj is Purple_1.Competition competition) 
            {
                var d = new Competition_Purple1_DTO()
                {
                    Judges = competition.Judges.Select(r =>
                    new Judge_Purple1_DTO
                    {
                        Name = r.Name,
                        Marks = r.Marks,
                    }
                    ).ToArray(),
                    Participants = competition.Participants.Select(x =>
                    new Participant_Purple1_DTO
                    {
                        Name= x.Name,
                        Surname = x.Surname,
                        Coefs = x.Coefs,
                        Marks= x.Marks.Cast<int>().ToArray(),
                    }
                    ).ToArray(),
                };
                var serializer = new XmlSerializer(typeof(Competition_Purple1_DTO));
                using (var writer = XmlWriter.Create(FilePath)) { serializer.Serialize(writer, d); };
            }
        }
        
        private Purple_1.Participant deserializeToParticipant(Participant_Purple1_DTO r)
        {
            var participant = new Purple_1.Participant(r.Name, r.Surname);
            double[] coefs = r.Coefs;
            int[] marksArray = r.Marks;
            int[,] Marks = new int[4, 7];
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Marks[i, j] = marksArray[index++];
                }
            }
            participant.SetCriterias(coefs);
            for (int i = 0;i < 4; i++)
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
        private Purple_1.Judge deserializeToJudge(Judge_Purple1_DTO r)
        {
            return new Purple_1.Judge(r.Name,r.Marks);
        }
        private Purple_1.Competition deserializeToCompetiton(Competition_Purple1_DTO r)
        {
            var j = r.Judges;
            var Judges = new Purple_1.Judge[j.Length];
            int index = 0;
            foreach(var judge in j)
            {
                Judges[index++] = deserializeToJudge(judge);
            }
            var p = r.Participants;
            var Participants = new Purple_1.Participant[p.Length];
            index = 0;
            foreach (var participant in p)
            {
                Participants[index++] = deserializeToParticipant(participant);
            }
            var comp = new Purple_1.Competition(Judges);
            comp.Add(Participants);
            return comp;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Purple_1.Competition))
            {
                var r = deserializer<Competition_Purple1_DTO>();
                return deserializeToCompetiton(r) as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var r = deserializer<Judge_Purple1_DTO>();
                return deserializeToJudge(r) as T;
            }
            else if (typeof(T) == typeof(Purple_1.Participant))
            {
                var r = deserializer<Participant_Purple1_DTO>();
                return deserializeToParticipant(r) as T;
            }
            else return null;
        }


        #endregion
        #region Purple_2

        public class skiJumping_Purple2_DTO
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public Participant_Purple2_DTO[] Participants { get; set; }
        }
        public class Participant_Purple2_DTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            if (jumping is Purple_2.SkiJumping obj)
            {
                var d = new skiJumping_Purple2_DTO()
                {
                    Name = obj.Name,
                    Standard = obj.Standard,
                    Participants = obj.Participants.Select(x =>
                    new Participant_Purple2_DTO
                    {
                        Name = x.Name,
                        Surname = x.Surname,
                        Distance = x.Distance,
                        Marks = x.Marks
                    }).ToArray()
                };
                var serializer = new XmlSerializer(typeof(skiJumping_Purple2_DTO));
                using (var writer = XmlWriter.Create(FilePath))
                {
                    serializer.Serialize(writer, d);
                }
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var r = deserializer<skiJumping_Purple2_DTO>();
            return deserializeToskiJumping(r) as T;
        }
        private Purple_2.Participant deserializeToParticipant2(Participant_Purple2_DTO r, int target)
        {
            string Name = r.Name;
            string Surname = r.Surname;
            int Distance = r.Distance;
            int[] Marks = r.Marks;
            var participant = new Purple_2.Participant(Name, Surname);
            participant.Jump(Distance, Marks, target);
            return participant;
        }
        private Purple_2.SkiJumping deserializeToskiJumping(skiJumping_Purple2_DTO r)
        {
            string Name = r.Name;
            int Standerd = r.Standard;
            var p = r.Participants;
            Purple_2.Participant[] Participants = new Purple_2.Participant[p.Length];
            int index = 0;
            foreach (var participant in p)
            {
                Participants[index++] = deserializeToParticipant2(participant, Standerd);
            }
            Purple_2.SkiJumping jumping;
            if (Standerd == 100)
            {
                jumping = new Purple_2.JuniorSkiJumping();
            }
            else if (Standerd == 150)
                jumping = new Purple_2.ProSkiJumping();
            else
            {
                return null;
            }
            jumping.Add(Participants);
            return jumping;
        }

        #endregion
        #region Purple_3
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            if (skating is Purple_3.Skating obj)
            {
                var d = new Skating_Purple3_DTO()
                {
                    Moods = obj.Moods,
                    Type = obj.GetType().Name,
                    Participants = obj.Participants.Select(x =>
                    new Participant_Purple3_DTO
                    {
                        Name = x.Name,
                        Surname = x.Surname,
                        Marks = x.Marks,
                        Places = x.Places,
                    }
                    ).ToArray()
                };
                var serializer = new XmlSerializer(typeof(Skating_Purple3_DTO));
                using (var writer = XmlWriter.Create(FilePath))
                {
                    serializer.Serialize(writer, d);
                }

            }


        }

        public class Participant_Purple3_DTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
        }
        public class Skating_Purple3_DTO
        {
            public double[] Moods { get; set; }
            public Participant_Purple3_DTO[] Participants { get; set; }
            public string Type { get; set; }
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var r = deserializer<Skating_Purple3_DTO>();
            double[] Moods = r.Moods;
            string Type = r.Type;
            var p = r.Participants;
            var participants = new Purple_3.Participant[p.Length];
            for (int i = 0;i< p.Length;i++)
            {
                participants[i] = deserializeToParticipant3(p[i]);
            }
            Purple_3.Skating skating;
            if (Type.Contains("FigureSkating"))
            {
                skating = new Purple_3.FigureSkating(Moods, false);
            }
            else
            {
                skating = new Purple_3.IceSkating(Moods, false);
            }
            skating.Add(participants);
            return skating as T;

        }
        private Purple_3.Participant deserializeToParticipant3(Participant_Purple3_DTO r)
        {
            string Name = r.Name;
            string Surname = r.Surname;
            double[] Marks = r.Marks;
            var participant = new Purple_3.Participant(Name, Surname);
            for (int i = 0; i < Marks.Length; i++)
            {
                var mark = Marks[i];
                participant.Evaluate(mark);
            }
            return participant;

        }
        #endregion

        #region Purple_4



        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            if (participant is Purple_4.Group G)
            {
                var d = new Group_Purple4_DTO()
                {
                    Name = G.Name,
                    Sportsmen = G.Sportsmen.Select(s => 
                    new Sportsman_Purple4_DTO()
                    {
                        Name = s.Name,
                        Surname = s.Surname,
                        Time = s.Time
                    }).ToArray(),
                };
                var serializer = new XmlSerializer(typeof(Group_Purple4_DTO));
                using (var writer = XmlWriter.Create(FilePath))
                {
                    serializer.Serialize(writer, d);
                }
            }
        }

        public class Sportsman_Purple4_DTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
        }
        public class Group_Purple4_DTO
        {
            public string Name { get; set; }
            public Sportsman_Purple4_DTO[] Sportsmen {  get; set; }
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var r = deserializer<Group_Purple4_DTO>();
            var Name = r.Name;
            var S = r.Sportsmen;
            var sportsmen = new Purple_4.Sportsman[S.Length];
            for (int i = 0; i < sportsmen.Length; i++)
            {
                sportsmen[i] = deserealizeToParticipant4(S[i]);
            }
            Purple_4.Group G = new Purple_4.Group(Name);
            G.Add(sportsmen);
            return G;
        }
        private Purple_4.Sportsman deserealizeToParticipant4(Sportsman_Purple4_DTO r)
        {
            string Name = r.Name;
            string Surname = r.Surname;
            double Time = r.Time;
            var sportsman = new Purple_4.Sportsman(Name, Surname);
            sportsman.Run(Time);
            return sportsman;
        }
        #endregion

        #region Purple_5
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);
            var Researches = new Research_Purple5_DTO[group.Researches.Length];

            int index = 0;
            foreach(var r in group.Researches)
            {
                var response = new Response_Purple5_DTO[r.Responses.Length];
                for(int i = 0;i< r.Responses.Length;i++)
                {
                    var responsee = r.Responses[i];
                    Response_Purple5_DTO obj = new Response_Purple5_DTO()
                    {
                        Animal = responsee.Animal,
                        CharacterTrait = responsee.CharacterTrait,
                        Concept = responsee.Concept,
                    };
                    response[i] = obj;
                }

                var obj2 = new Research_Purple5_DTO()
                {
                    Name = r.Name,
                    Responses = response
                };
                Researches[index++] = obj2;


            }
            var report = new Report_Purple5_DTO()
            {
                Researches = Researches
            };
            var serializer = new XmlSerializer(typeof(Report_Purple5_DTO));
            using (var writer = XmlWriter.Create(FilePath))
            {
                serializer.Serialize(writer, report);
            }

        }
        public class Response_Purple5_DTO
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
        }
        public class Research_Purple5_DTO
        {
            public string Name { get; set; }
            public Response_Purple5_DTO[] Responses { get; set; }
        }
        public class Report_Purple5_DTO
        {
            public Research_Purple5_DTO[] Researches { get; set; }
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var Report = new Purple_5.Report();
            var r = deserializer<Report_Purple5_DTO>();
            var researches = r.Researches;

            foreach (var research in researches)
            {
                var res = new Purple_5.Research(research.Name);
                foreach(var response in research.Responses)
                {
                    string Animal = response.Animal == "" ? null: response.Animal;
                    string CharacterTrait = response.CharacterTrait == "" ? null : response.CharacterTrait;
                    string Concept = response.Concept == "" ? null : response.Concept;
                    res.Add(new string[3] { Animal, CharacterTrait, Concept });
                }
                Report.AddResearch(res);
            }
            return Report;

        }
        #endregion
    }
}
