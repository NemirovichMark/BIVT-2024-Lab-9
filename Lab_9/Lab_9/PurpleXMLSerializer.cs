using Lab_7;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Lab_7.Purple_4;
using static Lab_7.Purple_5;
using static Lab_9.PurpleXMLSerializer;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";
        public class ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
            public double TotalScore { get; set; }
            public ParticipantDTO() { }
            public ParticipantDTO(Purple_1.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Coefs = participant.Coefs;
                var marks = new int[participant.Marks.GetLength(0)][];
                for (int i = 0; i < participant.Marks.GetLength(0); i++)
                {
                    marks[i] = new int[participant.Marks.GetLength(1)];
                    for (int j = 0; j < participant.Marks.GetLength(1); j++)
                    {
                        marks[i][j] = participant.Marks[i, j];
                    }
                }
                Marks = marks;
            }
            public Purple_1.Participant ForPart()
            {
                var p = new Purple_1.Participant(Name, Surname);
                p.SetCriterias(Coefs);
                foreach(var x in Marks)  p.Jump(x);
                return p;
            }
        }
            public class JudgeDTO
            {
                public string Name { get; set; }
                public int[] Marks { get; set; }
                public JudgeDTO() { }
                public JudgeDTO(Purple_1.Judge j)
                {
                    Name = j.Name;
                    Marks = j.Marks;
                }
                public Purple_1.Judge ForJudge()
                {
                    var j = new Purple_1.Judge(Name, Marks);
                    return j;
                }
            }
            public class CompetitionDTO
            {
                public JudgeDTO[] Judge { get; set; }
                public ParticipantDTO[] participant { get; set; }
                public CompetitionDTO() { }
                public CompetitionDTO(Purple_1.Competition c)
                {
                    Judge = new JudgeDTO[c.Judges.Length];
                    for (int i = 0; i < c.Judges.Length; i++)
                    {
                        Judge[i] = new JudgeDTO(c.Judges[i]);
                    }
                    participant = new ParticipantDTO[c.Participants.Length];
                    for (int i = 0; i < c.Participants.Length; i++)
                    {
                        participant[i] = new ParticipantDTO(c.Participants[i]);
                    }
                }
                public Purple_1.Competition ForComp()
                {
                    var c = new Purple_1.Competition(Judge.Select(x => x.ForJudge()).ToArray());
                    c.Add(participant.Select(x => x.ForPart()).ToArray()); return c;
                }
            }

        private void XMLS<T>(T part) where T : class
        {

            
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, part);
            }
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            if (obj is Purple_1.Participant p)
            {
                var pdto = new ParticipantDTO(p);
                XMLS(pdto);
            }
            else if (obj is Purple_1.Judge j)
            {
                var pdto = new JudgeDTO(j);
                XMLS(pdto);
            }
            else if (obj is Purple_1.Competition c)
            {
                var pdto = new CompetitionDTO(c);
                XMLS(pdto);
            }
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            using (var reader = new StreamReader(FilePath))
            {
                if (typeof(T) == typeof(Purple_1.Participant))
                {
                    var ser = new XmlSerializer(typeof(ParticipantDTO));
                    var dto = (ParticipantDTO)ser.Deserialize(reader);

                    return (T)(Object)dto.ForPart();
                }
                else if (typeof(T) == typeof(Purple_1.Judge))
                {
                    var ser = new XmlSerializer(typeof(JudgeDTO));
                    var dto = (JudgeDTO)ser.Deserialize(reader);
                    return (T)(Object)dto.ForJudge();
                }
                else if (typeof(T) == typeof(Purple_1.Competition))
                {
                    var ser = new XmlSerializer(typeof(CompetitionDTO));
                    var dto = (CompetitionDTO)ser.Deserialize(reader);
                    return (T)(Object)dto.ForComp();
                }
                else return null;
            }
        }
        public class ParticipantDTO2
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public ParticipantDTO2() { }
            public ParticipantDTO2(Purple_2.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Distance = p.Distance;
                Marks = p.Marks;
            }
            public Purple_2.Participant NeedPart(int t)
            {
                var part = new Purple_2.Participant(Name, Surname);
                part.Jump(Distance, Marks,t);
                return part;
            }
        }
        public class SkiJumpingDTO
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public string Type { get; set; }
            public ParticipantDTO2[] part { get; set; }
            public SkiJumpingDTO() { }
            public SkiJumpingDTO(Purple_2.SkiJumping s)
            {
                Name = s.Name;
                Standard = s.Standard;
                Type=s.GetType().Name;
                part = new ParticipantDTO2[s.Participants.Length];
                for (int i = 0; i < s.Participants.Length; i++)
                {
                    part[i] = new ParticipantDTO2(s.Participants[i]);
                }
            }
           
            public Purple_2.SkiJumping JuniorOrPro()
            {
                if (Standard == 100)
                {
                    var s = new Purple_2.JuniorSkiJumping();
                    var party = new Purple_2.Participant[part.Length];
                    for (int i = 0; i < part.Length; i++)
                    {
                        party[i] = part[i].NeedPart(100);
                    }
                    s.Add(party);
                    return s;
                }
                else if (Standard == 150)
                {
                    var s = new Purple_2.ProSkiJumping();
                    var party = new Purple_2.Participant[part.Length];
                    for (int i = 0; i < part.Length; i++)
                    {
                        party[i] = part[i].NeedPart(150);
                    }
                    s.Add(party);
                    return s;
                }
                else return null;
            }

        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            if (jumping is Purple_2.JuniorSkiJumping j)
            {
                var res = new SkiJumpingDTO(j);
                XMLS(res);
            }
            else if (jumping is Purple_2.ProSkiJumping p)
            {
                var res = new SkiJumpingDTO(p);
                XMLS(res);
            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            using (var reader = new StreamReader(FilePath))
            {
                var ser = new XmlSerializer(typeof(SkiJumpingDTO));
                var dto = (SkiJumpingDTO)ser.Deserialize(reader);
                return (T)dto.JuniorOrPro();
            }
        }
        public class ParticipantDTO3
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Places { get; set; }
            public double[] Marks { get; set; }
            public ParticipantDTO3() { }
            public ParticipantDTO3(Purple_3.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Places = p.Places;
                Marks = p.Marks;
            }
            public Purple_3.Participant ForPart3()
            {
               var part = new Purple_3.Participant(Name,Surname);
                foreach (var x in Marks) part.Evaluate(x);
                return part;
            }
        }
        public class SkatingDTO
        {
            public double[] Moods { get; set; }
            public string Type { get; set; }
            public ParticipantDTO3[] part { get; set; }
            public SkatingDTO() { }
            public SkatingDTO(Purple_3.Skating sk)
            {
                Moods=sk.Moods;
                Type= sk.GetType().Name;
                part = new ParticipantDTO3[sk.Participants.Length];
                for (int i = 0; i < part.Length; i++)
                {
                    part[i] = new ParticipantDTO3(sk.Participants[i]);
                }
            }
            public Purple_3.Skating ForSkat()
            {
                if (Type == "FigureSkating")
                {
                    var skating = new Purple_3.FigureSkating(Moods, false);
                    skating.Add(part.Select(x => x.ForPart3()).ToArray());
                    Purple_3.Participant.SetPlaces(skating.Participants);
                    return skating;
                }

                else if (Type == "IceSkating")
                {
                    var skating = new Purple_3.IceSkating(Moods, false);
                    skating.Add(part.Select(x => x.ForPart3()).ToArray());
                    Purple_3.Participant.SetPlaces(skating.Participants);
                    return skating;
                }
                else return null;
            }
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var res = new SkatingDTO(skating);
            XMLS(res);
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            using (var reader = new StreamReader(FilePath))
            {
                var ser = new XmlSerializer(typeof(SkatingDTO));
                var dto = (SkatingDTO)ser.Deserialize(reader);
                return (T)dto.ForSkat();
            }
        }
        public class SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
            public SportsmanDTO() { }
            public SportsmanDTO(Purple_4.Sportsman sk)
            {
                Name = sk.Name;
                Surname = sk.Surname;
                Time = sk.Time;
            }
            public Purple_4.Sportsman ForSport()
            {
                var man =new Purple_4.Sportsman(Name, Surname);
                man.Run(Time);
                return man;
            }
        }
        public class GroupDTO
        {
            public string Name { get; set; }
            public SportsmanDTO[] Sportsmen {  get; set; }
            public GroupDTO() { }
            public GroupDTO(Purple_4.Group sk)
            {
                Name = sk.Name;
                Sportsmen = new SportsmanDTO[sk.Sportsmen.Length];
                for (int i = 0; i < Sportsmen.Length; i++)
                {
                    Sportsmen[i] = new SportsmanDTO(sk.Sportsmen[i]);
                }
            }
            public Purple_4.Group ForGroup()
            {
                var g = new Purple_4.Group(Name);
                g.Add(Sportsmen.Select(x=>x.ForSport()).ToArray());
                return g;
            }
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            using var writer = new StreamWriter(FilePath);
            var ser = new XmlSerializer(typeof(GroupDTO));
            var g = new GroupDTO(group);
            ser.Serialize(writer, g);
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            using (var reader = new StreamReader(FilePath))
            {
                var ser = new XmlSerializer(typeof(GroupDTO));
                var dto = (GroupDTO)ser.Deserialize(reader);
                return dto.ForGroup();
            }
        }
        public class ResponsesDTO
        {
            public string Animal {  get; set; }
            public string CharacterTrait { get; set; }
            public string Concept {  get; set; }
            public ResponsesDTO() { }
            public ResponsesDTO(Purple_5.Response res)
            {
                Animal = res.Animal;
                CharacterTrait = res.CharacterTrait;
                Concept = res.Concept;
            }
            public Purple_5.Response ForRes()
            {
                var resp = new Purple_5.Response(Animal, CharacterTrait, Concept);
                return resp;
            }
        }
        public class ResearchDTO
        {
            public string Name { get; set; }
            public ResponsesDTO[] Responses { get; set; }
            public ResearchDTO() { }
            public ResearchDTO(Purple_5.Research research)
            {
                Name = research.Name;
                Responses = new ResponsesDTO[research.Responses.Length];
                for (int i = 0; i < Responses.Length; i++)
                {
                    Responses[i] = new ResponsesDTO(research.Responses[i]);
                }
            }
            public Purple_5.Research ForRes()
            {
                var res = new Purple_5.Research(Name);
                foreach(var item in Responses)
                {
                    var i = new string[] { item.Animal, item.CharacterTrait, item.Concept };
                    res.Add(i);
                }
                return res;
            }
        }
        public class ReportDTO
        {
            public ResearchDTO[] Research { get; set; }
            public ReportDTO() { }
            public ReportDTO (Purple_5.Report research)
            {
                Research = new ResearchDTO[research.Researches.Length];
                for (int i = 0; i < Research.Length; i++)
                {
                    Research[i] = new ResearchDTO(research.Researches[i]);
                }
            }
            public Purple_5.Report ForRep()
            {
                var rep = new Purple_5.Report();
                rep.AddResearch(Research.Select(x=>x.ForRes()).ToArray());
                return rep;
            }
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            using var writer = new StreamWriter(FilePath);
            var ser = new XmlSerializer(typeof(ReportDTO));
            var g = new ReportDTO(report);
            ser.Serialize(writer, g);
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            using (var reader = new StreamReader(FilePath))
            {
                var ser = new XmlSerializer(typeof(ReportDTO));
                var dto = (ReportDTO)ser.Deserialize(reader);
                return dto.ForRep();
            }
        }
    }
}
