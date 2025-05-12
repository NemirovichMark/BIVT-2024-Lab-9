using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Lab_7;
using static Lab_7.Purple_2;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension { get { return "xml"; } }


        public abstract class NamedDTO
        {
            public string Name { get; set; }
        }

        public abstract class SurnamedDTO : NamedDTO
        {
            public string Surname { get; set; }
        }
        public class Purple_1_ParticipantDTO : SurnamedDTO
        {
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
            public double TotalScore { get; set; }
            public Purple_1_ParticipantDTO() { }

            public Purple_1_ParticipantDTO(string name, string surname, double[] coefs, int[][] marks)
            {
                Name = name;
                Surname = surname;
                Coefs = coefs;
                Marks = marks;
            }
        }

        public class Purple_1_JudgeDTO : NamedDTO
        {
            public int[] Marks { get; set; }
            public Purple_1_JudgeDTO() { }
            public Purple_1_JudgeDTO(string name, int[] marks)
            {
                Name = name;
                Marks = marks;
            }
        }

        public class Purple_1_CompetitionDTO
        {
            public Purple_1_JudgeDTO[] Judges { get; set; }
            public Purple_1_ParticipantDTO[] Participants { get; set; }

            public Purple_1_CompetitionDTO() { }
            public Purple_1_CompetitionDTO(Purple_1_JudgeDTO[] judges, Purple_1_ParticipantDTO[] participants)
            {
                Judges = judges;
                Participants = participants;
            }

        }

        public class Purple_2_ParticipantDTO : SurnamedDTO
        {
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public int Result { get; set; }
            public Purple_2_ParticipantDTO() { }
            public Purple_2_ParticipantDTO(string name, string surname, int distance, int[] marks, int result)
            {
                Name = name;
                Surname = surname;
                Distance = distance;
                Marks = marks;
                Result = result;
            }
        }

        public class Purple_2_SkiJumpingDTO : NamedDTO
        {
            public string Type { get; set; }
            public int Standard { get; set; }
            public Purple_2_ParticipantDTO[] Participants { get; set; }

            public Purple_2_SkiJumpingDTO() { }
            public Purple_2_SkiJumpingDTO(string type, string name, int standard, Purple_2_ParticipantDTO[] participants)
            {
                Type = type;
                Standard = standard;
                Participants = participants;
                Standard = standard;
            }
        }
        public class Purple_3_ParticipantDTO : SurnamedDTO
        {
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
            public int Score { get; set; }
            public Purple_3_ParticipantDTO() { }
            public Purple_3_ParticipantDTO(string name, string surname, double[] marks, int[] places, int score)
            {
                Name = name;
                Surname = surname;
                Places = places;
                Score = score;
                Marks = marks;
            }

        }

        public class Purple_3_SkatingDTO
        {
            public string Type { get; set; }
            public double[] Moods { get; set; }
            public Purple_3_ParticipantDTO[] Participants { get; set; }

            public Purple_3_SkatingDTO() { }
            public Purple_3_SkatingDTO(string type, double[] moods, Purple_3_ParticipantDTO[] participants)
            {
                Type = type;
                Moods = moods;
                Participants = participants;
            }
        }

        public class Purple_4_SportsmanDTO : SurnamedDTO
        {
            public string Type { get; set; }
            public double Time { get; set; }
            public Purple_4_SportsmanDTO() { }
            public Purple_4_SportsmanDTO(string name, string surname, string type, double time) {
                Name = name;
                Surname = surname;
                Type = type;
                Time = time;
            }

        }

        public class Purple_4_GroupDTO : NamedDTO
        {
            public Purple_4_SportsmanDTO[] Sportsmen {  get; set; }
            public Purple_4_GroupDTO() { }
            public Purple_4_GroupDTO(string name, Purple_4_SportsmanDTO[] sportsmen) {
                Name = name;
                Sportsmen = sportsmen;
            }
        }

        public class Purple_5_ResponseDTO
        {
            public string Animal {  get; set; }
            public string CharacterTrait {  get; set; }
            public string Concept {  get; set; }
            public Purple_5_ResponseDTO() { }
            public Purple_5_ResponseDTO(string animal, string characterTrait, string concept)
            {
                Animal = animal;
                CharacterTrait = characterTrait;
                Concept = concept;
            }
        }

        public class Purple_5_ResearchDTO : NamedDTO
        {
            public Purple_5_ResponseDTO[] Responses { get; set; }
            public Purple_5_ResearchDTO() { }
            public Purple_5_ResearchDTO(string name, Purple_5_ResponseDTO[] responses) {
                Name = name;
                Responses = responses;
            }

        }

        public class Purple_5_ReportDTO
        {
            public Purple_5_ResearchDTO[] Researches { get; set; }
            public Purple_5_ReportDTO() { }
            public Purple_5_ReportDTO(Purple_5_ResearchDTO[] researches) {
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

        private void SerializeDTO<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, obj);
            writer.Close();
        }

        private T DeserializeDTO<T>()
        {
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(T));
            var dto = (T)serializer.Deserialize(reader);
            reader.Close();
            return dto;
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var p = obj as Purple_1.Participant;
                var participantDTO = new Purple_1_ParticipantDTO(p.Name, p.Surname, p.Coefs, ToJagged(p.Marks));
                SerializeDTO(participantDTO);
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var j = obj as Purple_1.Judge;
                var judgeDTO = new Purple_1_JudgeDTO(j.Name, j.Marks);
                SerializeDTO(judgeDTO);
            }
            else //Competition
            {
                var c = obj as Purple_1.Competition;
                if (c == null) return; //idk frfr

                //Nested judges
                var judgesDTO = new List<Purple_1_JudgeDTO>();
                foreach (var j in c.Judges)
                    judgesDTO.Add(new Purple_1_JudgeDTO(j.Name, j.Marks));

                //Nested participants
                var participantsDTO = new List<Purple_1_ParticipantDTO>();
                foreach (var p in c.Participants)
                    participantsDTO.Add(new Purple_1_ParticipantDTO(p.Name, p.Surname, p.Coefs, ToJagged(p.Marks)));


                var competitionDTO = new Purple_1_CompetitionDTO(judgesDTO.ToArray(), participantsDTO.ToArray());
                SerializeDTO(competitionDTO);
            }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var s = jumping as Purple_2.SkiJumping;
            if (s == null) return;

            //Nested participants
            var participantsDTO = new List<Purple_2_ParticipantDTO>();
            foreach (var p in s.Participants)
                participantsDTO.Add(new Purple_2_ParticipantDTO(p.Name, p.Surname, p.Distance, p.Marks, p.Result));
            string type = (jumping is Purple_2.ProSkiJumping ? "Pro" : "Junior") + "SkiJumping";
            var skiJumpingDTO = new Purple_2_SkiJumpingDTO(type, s.Name, s.Standard, participantsDTO.ToArray());
            SerializeDTO(skiJumpingDTO);
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            var s = skating as Purple_3.Skating;
            if (s == null) return;

            //Nested participants
            var participantsDTO = new List<Purple_3_ParticipantDTO>();
            foreach (var p in s.Participants)
                participantsDTO.Add(new Purple_3_ParticipantDTO(p.Name, p.Surname, p.Marks, p.Places, p.Score));
            string type = (skating is Purple_3.FigureSkating ? "Figure" : "Ice") + "Skating";

            var skatingDTO = new Purple_3_SkatingDTO(type, s.Moods, participantsDTO.ToArray());
            SerializeDTO(skatingDTO);
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile (fileName);

            var g = participant as Purple_4.Group;
            if (g == null) return;


            //Nested participants
            var sportsmenDTO = new List<Purple_4_SportsmanDTO>();
            foreach(var s in g.Sportsmen)
            {
                string type = "Ski" + (s is Purple_4.SkiMan ? "Man" : "Woman");
                sportsmenDTO.Add(new Purple_4_SportsmanDTO(s.Name, s.Surname, type, s.Time));
            }

            var groupDTO = new Purple_4_GroupDTO(g.Name, sportsmenDTO.ToArray());
            SerializeDTO(groupDTO);
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);

            var r = group as Purple_5.Report;
            if (r == null) return;

            //Nested researches
            var researchesDTO = new List<Purple_5_ResearchDTO>();
            foreach(var rs in r.Researches)
            {
                //Nested responses
                var responsesDTO = new List<Purple_5_ResponseDTO>();
                foreach (var rp in rs.Responses)
                    responsesDTO.Add(new Purple_5_ResponseDTO(rp.Animal, rp.CharacterTrait, rp.Concept));
                
                researchesDTO.Add(new Purple_5_ResearchDTO(rs.Name, responsesDTO.ToArray()));
            }

            var reportDTO = new Purple_5_ReportDTO(researchesDTO.ToArray());
            SerializeDTO(reportDTO);
        }




        public override T DeserializePurple1<T>(string fileName) where T : class
        {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var pDTO = DeserializeDTO<Purple_1_ParticipantDTO>();
                var participant = new Purple_1.Participant(pDTO.Name, pDTO.Surname);
                participant.SetCriterias(pDTO.Coefs);
                foreach (var row in pDTO.Marks) participant.Jump(row);

                return participant as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var jDTO = DeserializeDTO<Purple_1_JudgeDTO>();
                var judge = new Purple_1.Judge(jDTO.Name, jDTO.Marks);

                return judge as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var cDTO = DeserializeDTO<Purple_1_CompetitionDTO>();

                //Nested judges
                var judges = new List<Purple_1.Judge>();
                foreach (var jDTO in cDTO.Judges) judges.Add(new Purple_1.Judge(jDTO.Name, jDTO.Marks));

                //Nested participants
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

            var sDTO = DeserializeDTO<Purple_2_SkiJumpingDTO>();

            //Nested participants
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

            var sDTO = DeserializeDTO<Purple_3_SkatingDTO>();

            //Nested participants
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

            var gDTO = DeserializeDTO<Purple_4_GroupDTO>();

            var group = new Purple_4.Group(gDTO.Name);

            //Nested sportsmen
            foreach(var sDTO in gDTO.Sportsmen) {
                Purple_4.Sportsman sportsman;
                //if (sDTO.Type.Equals(nameof(Purple_4.SkiMan)))
                //{
                //    sportsman = new Purple_4.SkiMan(sDTO.Name, sDTO.Surname, sDTO.Time);
                //}
                //else if (sDTO.Type.Equals(nameof(Purple_4.SkiWoman)))
                //{
                //    sportsman = new Purple_4.SkiWoman(sDTO.Name, sDTO.Surname, sDTO.Time);
                //}
                //else //just Sportsman
                //{
                sportsman = new Purple_4.Sportsman(sDTO.Name, sDTO.Surname);
                sportsman.Run(sDTO.Time);
                //}
                group.Add(sportsman);
            }


            return group;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            var rDTO = DeserializeDTO<Purple_5_ReportDTO>();

            var report = new Purple_5.Report();
            //Nested researches
            foreach(var rs in rDTO.Researches)
            {
                //Nested responses
                var research = new Purple_5.Research(rs.Name);
                if (rs.Responses == null) continue;
                foreach(var rp in rs.Responses)
                {
                    research.Add(new string[] { rp.Animal, rp.CharacterTrait, rp.Concept });
                }
                report.AddResearch(research);
            }

            return report;
        }
    }
}
