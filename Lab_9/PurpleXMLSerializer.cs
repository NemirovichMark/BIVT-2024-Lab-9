using Lab_7;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static Lab_7.Purple_1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";
        private void SerializeToXml<T>(T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = XmlWriter.Create(FilePath))
            {
                serializer.Serialize(writer, data);
            }
        }
        private T DeserializeFromXml<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            T deserializeObject;
            using (var reader = File.OpenRead(FilePath))
            {
                deserializeObject = (T)serializer.Deserialize(reader);
            }
            return deserializeObject;
        }
        public class Purple_1_ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[] Marks { get; set; }
        }
        public class Purple_1_JudgeDto
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
        }
        public class Purple_1_CompetitionDto
        {
            public Purple_1_JudgeDto[] Judges { get; set; }
            public int countPart { get; set; }
            public int countJudge { get; set; }
            public Purple_1_ParticipantDto[] Participants { get; set; }
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            if (obj is Purple_1.Participant p)
            {
                var data = new Purple_1_ParticipantDto
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Coefs = p.Coefs,
                    Marks = p.Marks.Cast<int>().ToArray()
                };
                SerializeToXml(data);
            }
            else if (obj is Purple_1.Judge j)
            {
                var data = new Purple_1_JudgeDto
                {
                    Name = j.Name,
                    Marks = j.Marks
                };
                SerializeToXml(data);
            }
            else if (obj is Purple_1.Competition comp)
            {

                var data = new Purple_1_CompetitionDto
                {
                    countJudge = comp.Judges.Length,
                    countPart = comp.Participants.Length,
                    Judges = comp.Judges.Select(j =>
                    new Purple_1_JudgeDto
                    {
                        Name = j.Name,
                        Marks = j.Marks
                    }
                    ).ToArray(),
                    Participants = comp.Participants.Select(j =>
                    new Purple_1_ParticipantDto
                    {
                        Name = j.Name,
                        Surname = j.Surname,
                        Coefs = j.Coefs,
                        Marks = j.Marks.Cast<int>().ToArray()
                    }).ToArray()
                };
                SerializeToXml(data);
            }
        }
        public class Participant_Purple_2Dto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
        }
        public class Purple_2_SkiJumpingDto
        {
            public string Name { get; set; }
            public int Standart { get; set; }
            public int ParticipantsCount { get; set; }
            public Participant_Purple_2Dto[] Participants { get; set; }

        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            if (jumping is Purple_2.SkiJumping ski)
            {
                Purple_2_SkiJumpingDto data = new Purple_2_SkiJumpingDto
                {
                    Name = ski.Name,
                    Standart = ski.Standard,
                    Participants = ski.Participants.Select(p =>
                    new Participant_Purple_2Dto
                    {
                        Name = p.Name,
                        Surname = p.Surname,
                        Distance = p.Distance,
                        Marks = p.Marks,
                    }).ToArray()
                };
                SerializeToXml(data);
            }
        }
        public class Purple_3_ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
        }
        public class Purple_3_SkatingDto
        {
            public double[] Moods { get; set; }
            public string Type { get; set; }
            public Purple_3_ParticipantDto[] Participants { get; set; }
            public int ParticipantsCount { get; set; }

        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            if (skating is Purple_3.Skating ski)
            {
                Purple_3_SkatingDto data = new Purple_3_SkatingDto
                {
                    Moods = ski.Moods,
                    Participants = ski.Participants.Select(p =>
                    new Purple_3_ParticipantDto
                    {
                        Name = p.Name,
                        Surname = p.Surname,
                        Marks = p.Marks,
                        Places = p.Places,
                    }).ToArray(),
                    ParticipantsCount = ski.Participants.Length,
                    Type = ski.GetType().Name
                };
                SerializeToXml(data);
            }

        }
        public class Purple_4_Sportsman
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
        }
        public class Purple_4_Group
        {
            public string Name { get; set; }
            public Purple_4_Sportsman[] Sportsmen { get; set; }
            public int SportsmenCount;
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            if (participant is Purple_4.Group g)
            {
                var data = new Purple_4_Group
                {
                    Name = g.Name,
                    SportsmenCount = g.Sportsmen.Length,
                    Sportsmen = g.Sportsmen.Select(p =>
                    new Purple_4_Sportsman
                    {
                        Name = p.Name,
                        Surname = p.Surname,
                        Time = p.Time,
                    }).ToArray()
                };
                SerializeToXml(data);
            }

        }
        public class Purple_5_ResponseDto
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
        }
        public class Purple_5_ResearchDto
        {
            public string Name { get; set; }
            public Purple_5_ResponseDto[] Responses { get; set; }
        }
        public class Purple_5_ReportDto
        {
            public Purple_5_ResearchDto[] Researches { get; set; }
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);
            Purple_5_ResearchDto[] researches = new Purple_5_ResearchDto[group.Researches.Length];
            int j = 0;
            foreach (var research in group.Researches)
            {
                Purple_5_ResponseDto[] resp = new Purple_5_ResponseDto[research.Responses.Length];
                int i = 0;
                foreach (var response in research.Responses)
                {
                    Purple_5_ResponseDto r = new Purple_5_ResponseDto
                    {
                        Animal = response.Animal,
                        CharacterTrait = response.CharacterTrait,
                        Concept = response.Concept,
                    };
                    resp[i++] = r;
                }
                Purple_5_ResearchDto researchDto = new Purple_5_ResearchDto
                {
                    Name = research.Name,
                    Responses = resp
                };
                researches[j++] = researchDto;
            }
            Purple_5_ReportDto report = new Purple_5_ReportDto
            {
                Researches = researches,
            };
            SerializeToXml(report);
        }
        private Purple_1.Participant DeserializeParticipantPurple1(Purple_1_ParticipantDto result)
        {
            var p = new Purple_1.Participant(result.Name, result.Surname);
            int[] MarksParticipant = result.Marks;
            double[] coefs = result.Coefs;
            int[,] marksInMatrix = new int[4, 7];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 7; j++)
                    marksInMatrix[i, j] = MarksParticipant[i * 7 + j];
            p.SetCriterias(coefs);
            for (int i = 0; i < 4; i++)
            {
                int[] marks = new int[7];
                for (int j = 0; j < 7; j++)
                {
                    marks[j] = marksInMatrix[i, j];
                }
                p.Jump(marks);
            }
            return p;
        }
        private Purple_1.Judge DeserializeJudgePurple1(Purple_1_JudgeDto result)
        {
            var j = new Purple_1.Judge(result.Name, result.Marks);
            return j;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var result = DeserializeFromXml<Purple_1_ParticipantDto>();
                return DeserializeParticipantPurple1(result) as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var result = DeserializeFromXml<Purple_1_JudgeDto>();
                return DeserializeJudgePurple1(result) as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var result = DeserializeFromXml<Purple_1_CompetitionDto>();
                var judges = result.Judges;
                Purple_1.Judge[] judgesList = new Judge[result.countJudge];
                int i = 0;
                foreach (var j in judges)
                {
                    judgesList[i++] = DeserializeJudgePurple1(j);
                }
                Purple_1.Competition c = new Purple_1.Competition(judgesList);
                var participants = result.Participants;
                foreach (var p in participants)
                {
                    c.Add(DeserializeParticipantPurple1(p));
                }
                return c as T;
            }
            else
            {
                return null;
            }
        }
        private Purple_2.Participant DeserializeParticipantPurple2(Participant_Purple_2Dto result, int standart)
        {
            Purple_2.Participant participant = new Purple_2.Participant(result.Name, result.Surname);
            participant.Jump(result.Distance, result.Marks, standart);
            return participant;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var xml = DeserializeFromXml<Purple_2_SkiJumpingDto>();
            Purple_2.SkiJumping ski;
            if (xml.Standart == 100)
            {
                ski = new Purple_2.JuniorSkiJumping();
            }
            else if (xml.Standart == 150)
            {
                ski = new Purple_2.ProSkiJumping();
            }
            else
            {
                return null;
            }
            var participants = xml.Participants;
            foreach (var p in participants)
            {
                ski.Add(DeserializeParticipantPurple2(p, xml.Standart));
            }
            return ski as T;
        }
        private Purple_3.Participant DeserializeParticipantPurple3(Purple_3_ParticipantDto res)
        {
            Purple_3.Participant participant = new Purple_3.Participant(res.Name, res.Surname);
            var marks = res.Marks;
            foreach (var mark in marks)
            {
                participant.Evaluate(mark);
            }
            return participant;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var xml = DeserializeFromXml<Purple_3_SkatingDto>();
            Purple_3.Skating skate;
            if (xml.Type == nameof(Purple_3.IceSkating))
            {
                skate = new Purple_3.IceSkating(xml.Moods, false);
            }
            else if (xml.Type == nameof(Purple_3.FigureSkating))
            {
                skate = new Purple_3.FigureSkating(xml.Moods, false);
            }
            else
            {
                return default;
            }
            var participants = xml.Participants;
            foreach (var p in participants)
            {
                skate.Add(DeserializeParticipantPurple3(p));
            }
            return skate as T;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var xml = DeserializeFromXml<Purple_4_Group>();
            Purple_4.Group group = new Purple_4.Group(xml.Name);
            var sportsmen = xml.Sportsmen;
            foreach (var sp in sportsmen)
            {
                Purple_4.Sportsman sport = new Purple_4.Sportsman(sp.Name, sp.Surname);
                sport.Run(sp.Time);
                group.Add(sport);
            }
            return group;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var r = DeserializeFromXml<Purple_5_ReportDto>();
            Purple_5.Report report = new Purple_5.Report();
            foreach (var research in r.Researches)
            {
                Purple_5.Research res = new Purple_5.Research(research.Name);
                foreach (Purple_5_ResponseDto response in research.Responses)
                {
                    string rAnimal = response.Animal == "" ? null : response.Animal;
                    string rTrait = response.CharacterTrait == "" ? null : response.CharacterTrait;
                    string rConcept = response.Concept == "" ? null : response.Concept;
                    res.Add(new string[] { rAnimal, rTrait, rConcept });
                }
                report.AddResearch(res);
            }
            return report;
        }
    }
}
