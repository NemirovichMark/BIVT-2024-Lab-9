using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Lab_9.PurpleXMLSerializer;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {

        public override string Extension => "xml";

        private void ToXML<T>(T dto)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
            writer.Close();
        }

        private T FromXML<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StreamReader(FilePath);
            var dto = (T)serializer.Deserialize(reader);
            reader.Close();
            return dto;
        }

        public abstract class NameDTO
        {
            public string Name { get; set; }
        }

        public abstract class SurnameDTO : NameDTO
        {
            public string Surname { get; set; }
        }

        public class P1_ParticipantDTO : SurnameDTO
        {
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
            public double TotalScore { get; set; }
            public P1_ParticipantDTO() { }

            public P1_ParticipantDTO(string name, string surname, double[] coefs, int[][] marks)
            {
                Name = name;
                Surname = surname;
                Coefs = coefs;
                Marks = marks;
            }
        }

        public class P1_JudgeDTO : NameDTO
        {
            public int[] Marks { get; set; }
            public P1_JudgeDTO() { }
            public P1_JudgeDTO(string name, int[] marks)
            {
                Name = name;
                Marks = marks;
            }
        }

        public class P1_CompetitionDTO
        {
            public P1_JudgeDTO[] Judges { get; set; }
            public P1_ParticipantDTO[] Participants { get; set; }

            public P1_CompetitionDTO() { }
            public P1_CompetitionDTO(P1_JudgeDTO[] judges, P1_ParticipantDTO[] participants)
            {
                Judges = judges;
                Participants = participants;
            }

        }

        public class P2_ParticipantDTO : SurnameDTO
        {
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public int Result { get; set; }
            public P2_ParticipantDTO() { }
            public P2_ParticipantDTO(string name, string surname, int distance, int[] marks, int result)
            {
                Name = name;
                Surname = surname;
                Distance = distance;
                Marks = marks;
                Result = result;
            }
        }

        public class P2_SkiJumpingDTO : NameDTO
        {
            public string Type { get; set; }
            public int Standard { get; set; }
            public P2_ParticipantDTO[] Participants { get; set; }

            public P2_SkiJumpingDTO() { }
            public P2_SkiJumpingDTO(string type, string name, int standard, P2_ParticipantDTO[] participants)
            {
                Type = type;
                Standard = standard;
                Participants = participants;
                Standard = standard;
            }
        }
        public class P3_ParticipantDTO : SurnameDTO
        {
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
            public int Score { get; set; }
            public P3_ParticipantDTO() { }
            public P3_ParticipantDTO(string name, string surname, double[] marks, int[] places, int score)
            {
                Name = name;
                Surname = surname;
                Places = places;
                Score = score;
                Marks = marks;
            }

        }

        public class P3_SkatingDTO
        {
            public string Type { get; set; }
            public double[] Moods { get; set; }
            public P3_ParticipantDTO[] Participants { get; set; }

            public P3_SkatingDTO() { }
            public P3_SkatingDTO(string type, double[] moods, P3_ParticipantDTO[] participants)
            {
                Type = type;
                Moods = moods;
                Participants = participants;
            }
        }

        public class P4_SportsmanDTO : SurnameDTO
        {
            public string Type { get; set; }
            public double Time { get; set; }
            public P4_SportsmanDTO() { }
            public P4_SportsmanDTO(string name, string surname, string type, double time)
            {
                Name = name;
                Surname = surname;
                Type = type;
                Time = time;
            }

        }

        public class P4_GroupDTO : NameDTO
        {
            public P4_SportsmanDTO[] Sportsmen { get; set; }
            public P4_GroupDTO() { }
            public P4_GroupDTO(string name, P4_SportsmanDTO[] sportsmen)
            {
                Name = name;
                Sportsmen = sportsmen;
            }
        }

        public class P5_ResponseDTO
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
            public P5_ResponseDTO() { }
            public P5_ResponseDTO(string animal, string trait, string concept)
            {
                Animal = animal;
                CharacterTrait = trait;
                Concept = concept;
            }
        }

        public class P5_ResearchDTO : NameDTO
        {
            public P5_ResponseDTO[] Responses { get; set; }
            public P5_ResearchDTO() { }
            public P5_ResearchDTO(string name, P5_ResponseDTO[] responses)
            {
                Name = name;
                Responses = responses;
            }

        }

        public class P5_ReportDTO
        {
            public P5_ResearchDTO[] Researches { get; set; }
            public P5_ReportDTO() { }
            public P5_ReportDTO(P5_ResearchDTO[] researches)
            {
                Researches = researches;
            }

        }

        private int[][] ToJagged(int[,] matrix)
        {
            int[][] jagged = new int[matrix.GetLength(0)][];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                jagged[i] = new int[matrix.GetLength(1)];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    jagged[i][j] = matrix[i, j];
                }
            }

            return jagged;
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var p = obj as Purple_1.Participant;
                var participantDTO = new P1_ParticipantDTO(p.Name, p.Surname, p.Coefs, ToJagged(p.Marks));
                ToXML(participantDTO);
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var j = obj as Purple_1.Judge;
                var judgeDTO = new P1_JudgeDTO(j.Name, j.Marks);
                ToXML(judgeDTO);
            }
            else 
            {
                var c = obj as Purple_1.Competition;
                if (c == null) return; 

                var judgesDTO = new List<P1_JudgeDTO>();
                foreach (var j in c.Judges)
                    judgesDTO.Add(new P1_JudgeDTO(j.Name, j.Marks));

                var participantsDTO = new List<P1_ParticipantDTO>();
                foreach (var p in c.Participants)
                    participantsDTO.Add(new P1_ParticipantDTO(p.Name, p.Surname, p.Coefs, ToJagged(p.Marks)));


                var competitionDTO = new P1_CompetitionDTO(judgesDTO.ToArray(), participantsDTO.ToArray());
                ToXML(competitionDTO);
            }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var s = jumping as Purple_2.SkiJumping;
            if (s == null) return;

            var participantsDTO = new List<P2_ParticipantDTO>();
            foreach (var p in s.Participants)
            {
                participantsDTO.Add(new P2_ParticipantDTO(p.Name, p.Surname, p.Distance, p.Marks, p.Result));
            }
            string type = "";
            switch (jumping){
                case Purple_2.ProSkiJumping:
                    type = "ProSkiJumping";
                    break;
                case Purple_2.JuniorSkiJumping:
                    type = "JuniorSkiJumping";
                    break;
                default:
                    break;
            }
            var skiJumpingDTO = new P2_SkiJumpingDTO(type, s.Name, s.Standard, participantsDTO.ToArray());
            ToXML(skiJumpingDTO);
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            var s = skating as Purple_3.Skating;
            if (s == null) return;

            var participantsDTO = new List<P3_ParticipantDTO>();
            foreach (var p in s.Participants)
                participantsDTO.Add(new P3_ParticipantDTO(p.Name, p.Surname, p.Marks, p.Places, p.Score));
            string type = "";
            if(skating is Purple_3.FigureSkating)
            {
                type = "FigureSkating";
            }
            else if( skating is Purple_3.IceSkating)
            {
                type = "IceSkating";
            }

            var skatingDTO = new P3_SkatingDTO(type, s.Moods, participantsDTO.ToArray());
            ToXML(skatingDTO);
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);

            var g = participant as Purple_4.Group;
            if (g == null) return;


            var sportsmanDTO = new List<P4_SportsmanDTO>();
            foreach (var s in g.Sportsmen)
            {
                string type = "";

                if(s is Purple_4.SkiMan)
                {
                    type = "SkiMan";
                } else
                {
                    type = "SkiWoman";
                }
                sportsmanDTO.Add(new P4_SportsmanDTO(s.Name, s.Surname, type, s.Time));
            }

            var groupDTO = new P4_GroupDTO(g.Name, sportsmanDTO.ToArray());
            ToXML(groupDTO);
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);

            var r = group as Purple_5.Report;
            if (r == null) return;

            var researchDTO = new List<P5_ResearchDTO>();
            foreach (var rs in r.Researches)
            {
                var respDTO = new List<P5_ResponseDTO>();
                foreach (var rp in rs.Responses)
                    respDTO.Add(new P5_ResponseDTO(rp.Animal, rp.CharacterTrait, rp.Concept));

                researchDTO.Add(new P5_ResearchDTO(rs.Name, respDTO.ToArray()));
            }

            var reportDTO = new P5_ReportDTO(researchDTO.ToArray());
            ToXML(reportDTO);
        }




        public override T DeserializePurple1<T>(string fileName) where T : class
        {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var pDTO = FromXML<P1_ParticipantDTO>();
                var participant = new Purple_1.Participant(pDTO.Name, pDTO.Surname);
                participant.SetCriterias(pDTO.Coefs);
                foreach (var row in pDTO.Marks) participant.Jump(row);

                return participant as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var jDTO = FromXML<P1_JudgeDTO>();
                var judge = new Purple_1.Judge(jDTO.Name, jDTO.Marks);

                return judge as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var cDTO = FromXML<P1_CompetitionDTO>();

                
                var judges = new List<Purple_1.Judge>();
                foreach (var jDTO in cDTO.Judges) judges.Add(new Purple_1.Judge(jDTO.Name, jDTO.Marks));

               
                var participants = new List<Purple_1.Participant>();
                foreach (var pDTO in cDTO.Participants)
                {
                    var participant = new Purple_1.Participant(pDTO.Name, pDTO.Surname);
                    participant.SetCriterias(pDTO.Coefs);
                    foreach (var row in pDTO.Marks) participant.Jump(row);

                    participants.Add(participant);
                }

                var competition = new Purple_1.Competition(judges.ToArray());
                competition.Add(participants.ToArray());

                return competition as T;
            }
            return default(T);
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            var sDTO = FromXML<P2_SkiJumpingDTO>();

            
            var participants = new List<Purple_2.Participant>();
            foreach (var pDTO in sDTO.Participants)
            {
                var participant = new Purple_2.Participant(pDTO.Name, pDTO.Surname);
                var SumWithoutMinMax = pDTO.Marks.Sum() - pDTO.Marks.Min() - pDTO.Marks.Max();
                var target = Math.Ceiling((pDTO.Result == 0) ? (pDTO.Distance + (SumWithoutMinMax + 60) / 2.0)
                                           : (pDTO.Distance - (pDTO.Result - SumWithoutMinMax - 60) / 2.0));
                participant.Jump(pDTO.Distance, pDTO.Marks, (int)target);
                participants.Add(participant);
            }

            string type = sDTO.Type;

            Purple_2.SkiJumping skiJumping;

            if (type.Equals(nameof(Purple_2.ProSkiJumping)))
            {
                skiJumping = new Purple_2.ProSkiJumping();
            }
            else
            {
                skiJumping = new Purple_2.JuniorSkiJumping();
            }

            skiJumping.Add(participants.ToArray());

            return skiJumping as T;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var sDTO = FromXML<P3_SkatingDTO>();

            var participants = new List<Purple_3.Participant>();
            foreach (var pDTO in sDTO.Participants)
            {
                var participant = new Purple_3.Participant(pDTO.Name, pDTO.Surname);
                foreach (var mark in pDTO.Marks) participant.Evaluate(mark);
                participants.Add(participant);
            }

            Purple_3.Participant.SetPlaces(participants.ToArray());

            Purple_3.Skating skating;

            if (sDTO.Type.Equals(nameof(Purple_3.IceSkating)))
            {
                skating = new Purple_3.IceSkating(sDTO.Moods, false);
            }
            else
            {
                skating = new Purple_3.FigureSkating(sDTO.Moods, false);
            }

            skating.Add(participants.ToArray());


            return skating as T;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var gDTO = FromXML<P4_GroupDTO>();

            var group = new Purple_4.Group(gDTO.Name);

            foreach (var sDTO in gDTO.Sportsmen)
            {
                Purple_4.Sportsman sportsman;
                sportsman = new Purple_4.Sportsman(sDTO.Name, sDTO.Surname);
                sportsman.Run(sDTO.Time);
                group.Add(sportsman);
            }


            return group;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            var rDTO = FromXML<P5_ReportDTO>();

            var report = new Purple_5.Report();
            foreach (var rs in rDTO.Researches)
            {
                var research = new Purple_5.Research(rs.Name);
                if (rs.Responses == null) continue;
                foreach (var rp in rs.Responses)
                {
                    research.Add(new string[] { rp.Animal, rp.CharacterTrait, rp.Concept });
                }
                report.AddResearch(research);
            }

            return report;
        }
    }
}
