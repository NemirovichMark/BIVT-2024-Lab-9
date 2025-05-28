using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Lab_9.PurpleXMLSerializer;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        public abstract class NamedDTO
        {
            public string Name { get; set; }
        }

        public abstract class SurnamedDTO : NamedDTO
        {
            public string Surname { get; set; }
        }

        //DTO Purple_1
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
            public Purple_1_ParticipantDTO(Purple_1.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Coefs = participant.Coefs;
                TotalScore = participant.TotalScore;

                Marks = new int[participant.Marks.GetLength(0)][];

                for (int i = 0; i < participant.Marks.GetLength(0); i++)
                {
                    Marks[i] = new int[participant.Marks.GetLength(1)];

                    for (int j = 0; j < participant.Marks.GetLength(1); j++)
                        Marks[i][j] = participant.Marks[i, j];
                }
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
            public Purple_1_JudgeDTO(Purple_1.Judge judge)
            {
                Name = judge.Name;
                Marks = judge.Marks;
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
            public Purple_1_CompetitionDTO(Purple_1.Competition competition)
            {
                if (competition.Judges != null)
                {
                    Judges = new Purple_1_JudgeDTO[competition.Judges.Length];
                    for (int i = 0; i < competition.Judges.Length; i++)
                    {
                        Judges[i] = new Purple_1_JudgeDTO(competition.Judges[i]);
                    }
                }

                if (competition.Participants != null)
                {
                    Participants = new Purple_1_ParticipantDTO[competition.Participants.Length];
                    for (int i = 0; i < competition.Participants.Length; i++)
                    {
                        Participants[i] = new Purple_1_ParticipantDTO(competition.Participants[i]);
                    }
                }
            }
        }

        // DTO Purple_2
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

        //DTO Purple_3
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

        //DTO Purple_4
        public class Purple_4_SportsmanDTO : SurnamedDTO
        {
            public string Type { get; set; }
            public double Time { get; set; }
            public Purple_4_SportsmanDTO() { }
            public Purple_4_SportsmanDTO(string name, string surname, string type, double time)
            {
                Name = name;
                Surname = surname;
                Type = type;
                Time = time;
            }
        }

        public class Purple_4_GroupDTO : NamedDTO
        {
            public Purple_4_SportsmanDTO[] Sportsmen { get; set; }
            public Purple_4_GroupDTO() { }
            public Purple_4_GroupDTO(string name, Purple_4_SportsmanDTO[] sportsmen)
            {
                Name = name;
                Sportsmen = sportsmen;
            }
        }

        //DTO Purple_5
        public class Purple_5_ResponseDTO
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
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
            public Purple_5_ResearchDTO(string name, Purple_5_ResponseDTO[] responses)
            {
                Name = name;
                Responses = responses;
            }
        }

        public class Purple_5_ReportDTO
        {
            public Purple_5_ResearchDTO[] Researches { get; set; }
            public Purple_5_ReportDTO() { }
            public Purple_5_ReportDTO(Purple_5_ResearchDTO[] researches)
            {
                Researches = researches;
            }
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



        // Purple_1
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            switch (obj)
            {
                case Purple_1.Participant p:
                    var participantDTO = new Purple_1_ParticipantDTO(p);
                    SerializeDTO(participantDTO);
                    break;

                case Purple_1.Judge j:
                    var judgeDTO = new Purple_1_JudgeDTO(j);
                    SerializeDTO(judgeDTO);
                    break;

                case Purple_1.Competition c:
                    Purple_1_JudgeDTO[] judgesDTO = new Purple_1_JudgeDTO[c.Judges.Length];
                    
                    for (int i = 0; i < c.Judges.Length; i++)
                        judgesDTO[i] = new Purple_1_JudgeDTO(c.Judges[i]);

                    Purple_1_ParticipantDTO[] participantsDTO = new Purple_1_ParticipantDTO[c.Participants.Length];
                    
                    for (int i = 0; i < c.Participants.Length; i++)
                        participantsDTO[i] = new Purple_1_ParticipantDTO(c.Participants[i]);

                    var competitionDTO = new Purple_1_CompetitionDTO(judgesDTO, participantsDTO);
                    SerializeDTO(competitionDTO);
                    break;

                default:
                    return;
            }
        }

        public override T DeserializePurple1<T>(string fileName) where T : class
        {
            SelectFile(fileName);

            switch (typeof(T))
            {
                case Type t when t == typeof(Purple_1.Participant):
                    {
                        var dto = DeserializeDTO<Purple_1_ParticipantDTO>();
                        var participant = new Purple_1.Participant(dto.Name, dto.Surname);
                        participant.SetCriterias(dto.Coefs);

                        foreach (var row in dto.Marks)
                            participant.Jump(row);

                        return participant as T;
                    }

                case Type t when t == typeof(Purple_1.Judge):
                    {
                        var dto = DeserializeDTO<Purple_1_JudgeDTO>();
                        var judge = new Purple_1.Judge(dto.Name, dto.Marks);
                        return judge as T;
                    }

                case Type t when t == typeof(Purple_1.Competition):
                    {
                        var dto = DeserializeDTO<Purple_1_CompetitionDTO>();

                        var judges = new Purple_1.Judge[dto.Judges.Length];
                        for (int i = 0; i < dto.Judges.Length; i++)
                        {
                            var j = dto.Judges[i];
                            judges[i] = new Purple_1.Judge(j.Name, j.Marks);
                        }

                        var participants = new Purple_1.Participant[dto.Participants.Length];
                        for (int i = 0; i < dto.Participants.Length; i++)
                        {
                            var p = dto.Participants[i];
                            var participant = new Purple_1.Participant(p.Name, p.Surname);
                            participant.SetCriterias(p.Coefs);

                            foreach (var row in p.Marks)
                                participant.Jump(row);

                            participants[i] = participant;
                        }

                        var competition = new Purple_1.Competition(judges);
                        competition.Add(participants);

                        return competition as T;
                    }

                default:
                    return default;
            }
        }

        // Purple_2
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var s = jumping as Purple_2.SkiJumping;
            if (s == null) return;

            var participants = s.Participants;
            var participantsDTO = new Purple_2_ParticipantDTO[participants.Length];

            for (int i = 0; i < participants.Length; i++)
            {
                var p = participants[i];
                participantsDTO[i] = new Purple_2_ParticipantDTO(p.Name, p.Surname, p.Distance, p.Marks, p.Result);
            }

            string type = (jumping is Purple_2.ProSkiJumping ? "Pro" : "Junior") + "SkiJumping";
            var skiJumpingDTO = new Purple_2_SkiJumpingDTO(type, s.Name, s.Standard, participantsDTO);
            SerializeDTO(skiJumpingDTO);
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            var sDTO = DeserializeDTO<Purple_2_SkiJumpingDTO>();
            var dtoArray = sDTO.Participants;
            var participants = new Purple_2.Participant[dtoArray.Length];

            for (int i = 0; i < dtoArray.Length; i++)
            {
                var p = dtoArray[i];
                var participant = new Purple_2.Participant(p.Name, p.Surname);

                int sum = p.Marks.Sum() - p.Marks.Min() - p.Marks.Max();
                double baseScore = (p.Result == 0) ? p.Distance + (sum + 60) / 2.0 : p.Distance - (p.Result - sum - 60) / 2.0;
                int target = (int)Math.Ceiling(baseScore);

                participant.Jump(p.Distance, p.Marks, target);
                participants[i] = participant;
            }

            Purple_2.SkiJumping skiJumping;

            if (sDTO.Type == nameof(Purple_2.ProSkiJumping))
                skiJumping = new Purple_2.ProSkiJumping();
            else
                skiJumping = new Purple_2.JuniorSkiJumping();

            skiJumping.Add(participants);

            return skiJumping as T;
        }

        // Purple_3
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            if (skating is not Purple_3.Skating s) return;

            var source = s.Participants;
            var participantsDTO = new Purple_3_ParticipantDTO[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                var p = source[i];
                participantsDTO[i] = new Purple_3_ParticipantDTO(p.Name, p.Surname, p.Marks, p.Places, p.Score);
            }

            string type = (s is Purple_3.FigureSkating ? "Figure" : "Ice") + "Skating";
            var skatingDTO = new Purple_3_SkatingDTO(type, s.Moods, participantsDTO);

            SerializeDTO(skatingDTO);
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var sDTO = DeserializeDTO<Purple_3_SkatingDTO>();

            var source = sDTO.Participants;
            var participants = new Purple_3.Participant[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                var pDTO = source[i];
                var participant = new Purple_3.Participant(pDTO.Name, pDTO.Surname);

                foreach (var mark in pDTO.Marks)
                    participant.Evaluate(mark);

                participants[i] = participant;
            }

            Purple_3.Participant.SetPlaces(participants);
            Purple_3.Skating skating = sDTO.Type == nameof(Purple_3.IceSkating) ? new Purple_3.IceSkating(sDTO.Moods, false) : new Purple_3.FigureSkating(sDTO.Moods, false);

            skating.Add(participants);

            return skating as T;
        }

        // Purple_4
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            if (group == null) return;

            var sportsmen = group.Sportsmen;
            var sportsmenDTO = new Purple_4_SportsmanDTO[sportsmen.Length];

            for (int i = 0; i < sportsmen.Length; i++)
            {
                var s = sportsmen[i];
                string type;

                if (s is Purple_4.SkiMan) type = "SkiMan";
                else if (s is Purple_4.SkiWoman) type = "SkiWoman";
                else type = "Sportsman";

                sportsmenDTO[i] = new Purple_4_SportsmanDTO(s.Name, s.Surname, type, s.Time);
            }

            var groupDTO = new Purple_4_GroupDTO(group.Name, sportsmenDTO);
            SerializeDTO(groupDTO);
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var gDTO = DeserializeDTO<Purple_4_GroupDTO>();
            var group = new Purple_4.Group(gDTO.Name);

            foreach (var sDTO in gDTO.Sportsmen)
            {
                Purple_4.Sportsman sportsman;

                switch (sDTO.Type)
                {
                    case "SkiMan":
                        sportsman = new Purple_4.SkiMan(sDTO.Name, sDTO.Surname, sDTO.Time);
                        break;

                    case "SkiWoman":
                        sportsman = new Purple_4.SkiWoman(sDTO.Name, sDTO.Surname, sDTO.Time);
                        break;

                    default:
                        sportsman = new Purple_4.Sportsman(sDTO.Name, sDTO.Surname);
                        sportsman.Run(sDTO.Time);
                        break;
                }

                group.Add(sportsman);
            }

            return group;
        }

        // Purple_5
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            if (report == null) return;

            var researches = new Purple_5_ResearchDTO[report.Researches.Length];

            for (int i = 0; i < report.Researches.Length; i++)
            {
                var rs = report.Researches[i];
                var responses = new Purple_5_ResponseDTO[rs.Responses.Length];

                for (int j = 0; j < rs.Responses.Length; j++)
                {
                    var rp = rs.Responses[j];
                    responses[j] = new Purple_5_ResponseDTO(rp.Animal, rp.CharacterTrait, rp.Concept);
                }

                researches[i] = new Purple_5_ResearchDTO(rs.Name, responses);
            }

            var reportDTO = new Purple_5_ReportDTO(researches);
            SerializeDTO(reportDTO);
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            var rDTO = DeserializeDTO<Purple_5_ReportDTO>();
            var report = new Purple_5.Report();

            foreach (var rs in rDTO.Researches)
            {
                var research = new Purple_5.Research(rs.Name);

                if (rs.Responses != null)
                {
                    foreach (var rp in rs.Responses)
                    {
                        research.Add(new[] { rp.Animal, rp.CharacterTrait, rp.Concept });
                    }
                }

                report.AddResearch(research);
            }

            return report;
        }

    }
}
