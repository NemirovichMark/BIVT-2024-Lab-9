using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Lab_7.Purple_1;
using System.Xml.Serialization;
using System.IO;
using System.CodeDom;
using static Lab_7.Purple_3;
using System.Xml.Schema;
using System.Runtime.Remoting.Contexts;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        //DTO classes

        //Purple_1
        public class Purple_1ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname {  get; set; }
            public double[] Coefs { get; set; }
            public int[][]  Marks { get; set; }

            public Purple_1ParticipantDTO() { }
            public Purple_1ParticipantDTO(Purple_1.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Coefs = participant.Coefs;
                Marks = new int[participant.Marks.GetLength(0)][];

                for (int i = 0; i < participant.Marks.GetLength(0); i++)
                {
                    Marks[i] = new int[participant.Marks.GetLength(1)];
                    for (int j = 0; j < participant.Marks.GetLength(1); j++)
                        Marks[i][j] = participant.Marks[i,j];
                }
            }

            public Purple_1.Participant Unpack()
            {
                var participant = new Purple_1.Participant(Name, Surname);
                participant.SetCriterias(Coefs);

                for (int i = 0; i < Marks.GetLength(0); i++)
                {
                    int[] m = Marks[i];
                    participant.Jump(m);
                }
                return participant;
            }
        }
        public class Purple_1JudgeDTO
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }

            public Purple_1JudgeDTO() { }
            public Purple_1JudgeDTO(Purple_1.Judge judge)
            {
                Name = judge.Name;
                Marks = judge.Marks;
            }

            public Purple_1.Judge Unpack()
            {
                var judge = new Purple_1.Judge(Name, Marks);

                return judge;
            }
        }
        public class Purple_1CompetitionDTO
        {
            public Purple_1ParticipantDTO[] Participants { get; set; }
            public Purple_1JudgeDTO[] Judges { get; set; }

            public Purple_1CompetitionDTO() { }
            public Purple_1CompetitionDTO(Purple_1.Competition competition)
            {
                Participants = new Purple_1ParticipantDTO[competition.Participants.Length];
                Judges = new Purple_1JudgeDTO[competition.Judges.Length];

                for (int i = 0; i < competition.Participants.Length; i++)
                    Participants[i] = new Purple_1ParticipantDTO(competition.Participants[i]);

                for (int i = 0; i < competition.Judges.Length; i++)
                    Judges[i] = new Purple_1JudgeDTO(competition.Judges[i]);
            }

            public Purple_1.Competition Unpack()
            {
                var participants = new Purple_1.Participant[Participants.Length];
                var judges = new Purple_1.Judge[Judges.Length];

                for (int i = 0; i < participants.Length; i++)
                {
                    participants[i] = Participants[i].Unpack();
                }

                for (int i = 0; i < judges.Length; i++)
                    judges[i] = Judges[i].Unpack();

                var competition = new Purple_1.Competition(judges);
                competition.Add(participants);

                return competition;
            }
        }

        //Purple_2
        public class Purple_2ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int Result { get; set; }
            public int[] Marks { get; set; }

            public Purple_2ParticipantDTO() { }
            public Purple_2ParticipantDTO(Purple_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Distance = participant.Distance;
                Marks = participant.Marks;
                Result = participant.Result;
            }

            public Purple_2.Participant Unpack(int standard)
            {
                var participant = new Purple_2.Participant(Name, Surname);
                participant.Jump(Distance, Marks, standard);

                return participant;
            }
        }
        public class Purple_2SkiJumpingDTO
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public string Type { get; set; }
            public Purple_2ParticipantDTO[] Participants { get; set; }

            public Purple_2SkiJumpingDTO() { }
            public Purple_2SkiJumpingDTO(Purple_2.SkiJumping jumping)
            {
                Type = jumping.GetType().Name;
                Name = jumping.Name;
                Standard = jumping.Standard;
                Participants = new Purple_2ParticipantDTO[jumping.Participants.Length];

                for(int i = 0;i<jumping.Participants.Length;i++)
                    Participants[i] = new Purple_2ParticipantDTO(jumping.Participants[i]);
            }

            public Purple_2.SkiJumping Unpack()
            {
                Purple_2.SkiJumping jumping;

                if (Type == "JuniorSkiJumping")
                    jumping = new Purple_2.JuniorSkiJumping();
                else if (Type == "ProSkiJumping")
                    jumping = new Purple_2.ProSkiJumping();
                else
                {
                    return default(Purple_2.SkiJumping);
                }
                    
                for (int i = 0; i < Participants.Length; i++)
                    jumping.Add(Participants[i].Unpack(Standard));

                return jumping;
            }
        }

        //Purple_3
        public class Purple_3ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }

            public Purple_3ParticipantDTO() { }
            public Purple_3ParticipantDTO(Purple_3.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = participant.Marks;
                Places = participant.Places;
            }

            public Purple_3.Participant Unpack()
            {
                var participant = new Purple_3.Participant(Name, Surname);

                for (int i = 0; i < Marks.Length; i++)
                    participant.Evaluate(Marks[i]);

                return participant;
            }
        }

        public class Purple_3SkatingDTO
        {
            public Purple_3ParticipantDTO[] Participants { get; set; }
            public double[] Moods { get; set; }
            public string Type { get; set; }

            public Purple_3SkatingDTO() { }
            public Purple_3SkatingDTO(Purple_3.Skating skating)
            {
                Type = skating.GetType().Name;
                Moods = skating.Moods;
                Participants = new Purple_3ParticipantDTO[skating.Participants.Length];

                for(int i = 0;i < skating.Participants.Length;i++)
                    Participants[i] = new Purple_3ParticipantDTO(skating.Participants[i]);
            }

            public Purple_3.Skating Unpack()
            {
                Purple_3.Skating skating;

                if (Type == "FigureSkating")
                    skating = new Purple_3.FigureSkating(Moods, false);
                else
                    skating = new Purple_3.IceSkating(Moods, false);

                for (int i = 0; i < Participants.Length;i++)
                    skating.Add(Participants[i].Unpack());

                return skating;
            }
        }

        //Purple_4
        public class Purple_4SportsmanDTO
        {
            public string Name {  get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }

            public Purple_4SportsmanDTO() { }
            public Purple_4SportsmanDTO(Purple_4.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Time = sportsman.Time;
            }

            public Purple_4.Sportsman Unpack()
            {
                var sportsman = new Purple_4.Sportsman(Name, Surname);
                sportsman.Run(Time);
                return sportsman;
            }
        }

        public class Purple_4GroupDTO
        {
            public string Name { get; set; }
            public Purple_4SportsmanDTO[] Sportsmen { get; set; }

            public Purple_4GroupDTO() { }
            public Purple_4GroupDTO(Purple_4.Group group)
            {
                Name = group.Name;
                Sportsmen = new Purple_4SportsmanDTO[group.Sportsmen.Length];

                for (int i = 0; i < group.Sportsmen.Length; i++)
                    Sportsmen[i] = new Purple_4SportsmanDTO(group.Sportsmen[i]);
            }

            public Purple_4.Group Unpack()
            {
                var group = new Purple_4.Group(Name);
                for (int i = 0; i < Sportsmen.Length; i++)
                    group.Add(Sportsmen[i].Unpack());

                return group;
            }
        }

        //Purple_5
        public class Purple_5ResponseDTO
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept {  get; set; }

            public Purple_5ResponseDTO() { }
            public Purple_5ResponseDTO(Purple_5.Response response)
            {
                Animal = response.Animal;
                CharacterTrait = response.CharacterTrait;
                Concept = response.Concept;

                if (string.IsNullOrEmpty(Animal)) Animal = null;
                if (string.IsNullOrEmpty(CharacterTrait)) CharacterTrait = null;
                if (string.IsNullOrEmpty(Concept)) Concept = null;
            }

            public Purple_5.Response Unpack()
            {
                var response = new Purple_5.Response(Animal, CharacterTrait, Concept);
                return response;
            }
        }

        public class Purple_5ResearchDTO
        {
            public string Name { get; set; }
            public Purple_5ResponseDTO[] Responses { get; set; }

            public Purple_5ResearchDTO() { }
            public Purple_5ResearchDTO(Purple_5.Research research)
            {
                Name = research.Name;
                Responses = new Purple_5ResponseDTO[research.Responses.Length];
                for (int i = 0; i < research.Responses.Length; i++)
                    Responses[i] = new Purple_5ResponseDTO(research.Responses[i]);
            }

            public Purple_5.Research Unpack()
            {
                var research = new Purple_5.Research(Name);
                for (int i = 0; i < Responses.Length; i++)
                    research.Add(new string[] { Responses[i].Animal, Responses[i].CharacterTrait, Responses[i].Concept });
                return research;
            }
        }

        public class Purple_5ReportDTO
        {
            public Purple_5ResearchDTO[] Researches { get; set; }

            public Purple_5ReportDTO() { }
            public Purple_5ReportDTO(Purple_5.Report report)
            {
                Researches = new Purple_5ResearchDTO[report.Researches.Length];
                for (int i = 0;i < report.Researches.Length;i++)
                    Researches[i] = new Purple_5ResearchDTO(report.Researches[i]);
            }

            public Purple_5.Report Unpack()
            {
                var report = new Purple_5.Report();
                for (int i = 0; i < Researches.Length; i++)
                    report.AddResearch(Researches[i].Unpack());
                return report;
            }
        }

            //serialize

            public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            using(StreamWriter sw = new StreamWriter(FilePath))
            {
                if (typeof(T) == typeof(Purple_1.Participant))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Purple_1ParticipantDTO));
                    serializer.Serialize(sw, new Purple_1ParticipantDTO(obj as Purple_1.Participant));
                }    
                else if (typeof(T) == typeof(Purple_1.Judge))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Purple_1JudgeDTO));
                    serializer.Serialize(sw, new Purple_1JudgeDTO(obj as Purple_1.Judge));
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Purple_1CompetitionDTO));
                    serializer.Serialize(sw, new Purple_1CompetitionDTO(obj as Purple_1.Competition));
                }
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);

            using(StreamWriter sw = new StreamWriter(FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Purple_2SkiJumpingDTO));
                serializer.Serialize(sw, new Purple_2SkiJumpingDTO(jumping));
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Purple_3SkatingDTO));
                serializer.Serialize(sw, new Purple_3SkatingDTO(skating));
            }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Purple_4GroupDTO));
                serializer.Serialize(sw, new Purple_4GroupDTO(group));
            }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Purple_5ReportDTO));
                serializer.Serialize(sw, new Purple_5ReportDTO(report));
            }
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            using (StreamReader sr = new StreamReader(FilePath))
            {
                if (typeof(T) == typeof(Purple_1.Participant))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Purple_1ParticipantDTO));
                    var participantDTO = serializer.Deserialize(sr) as Purple_1ParticipantDTO;
                    var participant = participantDTO.Unpack();

                    return participant as T;
                }
                else if(typeof(T) == typeof(Purple_1.Judge))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Purple_1JudgeDTO));
                    var judgeDTO = serializer.Deserialize(sr) as Purple_1JudgeDTO;
                    var judge = judgeDTO.Unpack();

                    return judge as T;
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Purple_1CompetitionDTO));
                    var competitionDTO = serializer.Deserialize(sr) as Purple_1CompetitionDTO;
                    var competition = competitionDTO.Unpack();

                    return competition as T;
                }
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            using (StreamReader sr = new StreamReader(FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Purple_2SkiJumpingDTO));
                var jumpingDTO = serializer.Deserialize(sr) as Purple_2SkiJumpingDTO;
                var jumping = jumpingDTO.Unpack();

                return jumping as T;
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            using(StreamReader sr = new StreamReader(FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Purple_3SkatingDTO));
                var skatingDTO = serializer.Deserialize(sr) as Purple_3SkatingDTO;
                var skating = skatingDTO.Unpack();

                return skating as T;
            }
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            using (StreamReader sr = new StreamReader(FilePath))
            {
                XmlSerializer serializer = new XmlSerializer (typeof(Purple_4GroupDTO));
                var groupDTO = serializer.Deserialize(sr) as Purple_4GroupDTO;
                var group = groupDTO.Unpack();

                return group;
            }
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            using (StreamReader sr = new StreamReader(FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Purple_5ReportDTO));
                var reportDTO = serializer.Deserialize(sr) as Purple_5ReportDTO;
                var report = reportDTO.Unpack();

                return report;
            }
        }

        
    }
}
