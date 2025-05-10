using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Lab_7.Purple_1;
using static Lab_7.Purple_3;
using static Lab_9.PurpleXMLSerializer;

namespace Lab_9
{
    public class PurpleXMLSerializer:PurpleSerializer
    {
        public override string Extension => "xml";
        public class Purple1ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
            public double TotalScore { get; set; }
            public Purple1ParticipantDTO() { }
            public Purple1ParticipantDTO(Purple_1.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Coefs = participant.Coefs;
                Marks = new int[participant.Marks.GetLength(0)][];
                for (int i =0; i< participant.Marks.GetLength(0); i++)
                {
                    Marks[i] = new int[participant.Marks.GetLength(1)];
                    for (int j = 0; j < participant.Marks.GetLength(1); j++)
                        Marks[i][j] = participant.Marks[i,j];
                }
            }
            public Purple_1.Participant ParticipantDtotoParticipant()
            {
                var participant = new Purple_1.Participant(Name, Surname);
                participant.SetCriterias(Coefs);
                for (int i = 0; i< Marks.Length; i++)
                {
                    int[] line = new int[Marks[0].Length];
                    for (int j = 0; j < Marks[0].Length; j++)
                        line[j] = Marks[i][ j];
                    participant.Jump(line);
                }
                return participant;
            }
        }
        public class Purple1JudgeDTO
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
            public Purple1JudgeDTO() { }
            public Purple1JudgeDTO(Purple_1.Judge judge)
            {
                Name = judge.Name;
                Marks = judge.Marks;
            }
            public Purple_1.Judge JudgeDTOtoJudge()
            {
                return new Purple_1.Judge(Name, Marks);
            }
        }
        public class Purple1CompetitionDTO
        {
            public Purple1JudgeDTO[] Judges { get; set; }
            public Purple1ParticipantDTO[] Participants { get; set; }
            public Purple1CompetitionDTO() { }
            public Purple1CompetitionDTO(Purple_1.Competition competition)
            {
                Judges = new Purple1JudgeDTO[competition.Judges.Length];
                for (int i = 0; i < competition.Judges.Length; i++)
                    Judges[i] = new Purple1JudgeDTO(competition.Judges[i]);
                Participants = new Purple1ParticipantDTO[competition.Participants.Length];
                for (int i = 0; i < competition.Participants.Length; i++)
                    Participants[i] = new Purple1ParticipantDTO(competition.Participants[i]);
            }
            public Purple_1.Competition CompetitionDTOtoCompetiton()
            {
                var Judgesout = new Purple_1.Judge[Judges.Length];
                for (int i = 0; i < Judges.Length; i++)
                    Judgesout[i] = Judges[i].JudgeDTOtoJudge();
                var Participantsout = new Purple_1.Participant[Participants.Length];
                for (int i = 0; i < Participants.Length; i++)
                    Participantsout[i] = Participants[i].ParticipantDtotoParticipant();
                var competiton = new Purple_1.Competition(Judgesout);
                competiton.Add(Participantsout);
                return competiton;
            }
        }
        public class Purple2ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public int Result { get; set; }
            public Purple2ParticipantDTO() { }
            public Purple2ParticipantDTO(Purple_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Distance = participant.Distance;
                Marks = participant.Marks;
                Result = participant.Result;
            }
            public Purple_2.Participant ParticipantDTOtoParticipant(int target)
            {
                var participant = new Purple_2.Participant(Name, Surname);
                participant.Jump(Distance, Marks, target);
                return participant;
            }
            
        }
        public class Purple2SkiJumpingDTO
        {
            public string Name { get; set; }
            public int Standart { get; set; }
            public Purple2ParticipantDTO[] Participants { get; set; }
            public Purple2SkiJumpingDTO() { }
            public Purple2SkiJumpingDTO(Purple_2.SkiJumping skijumping)
            {
                Name = skijumping.Name;
                Standart = skijumping.Standard;
                Participants = new Purple2ParticipantDTO[skijumping.Participants.Length];
                for (int i = 0; i < skijumping.Participants.Length; i++)
                {
                    Participants[i] = new Purple2ParticipantDTO(skijumping.Participants[i]);
                }
            }
            public Purple_2.SkiJumping SkiJumpingDTOtoSkiJumping()
            {
                if (Standart == 100)
                {
                    var ski = new Purple_2.JuniorSkiJumping();
                    var participants = new Purple_2.Participant[Participants.Length];
                    for (int i = 0; i < Participants.Length; i++)
                        participants[i] = Participants[i].ParticipantDTOtoParticipant(100);
                    ski.Add(participants);
                    return ski;
                }
                else if (Standart == 150)
                {
                    var ski = new Purple_2.ProSkiJumping();
                    var participants = new Purple_2.Participant[Participants.Length];
                    for (int i = 0; i < Participants.Length; i++)
                        participants[i] = Participants[i].ParticipantDTOtoParticipant(150);
                    ski.Add(participants);
                    return ski;
                }
                return null;
            }
        }
        public class Purple3ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Places { get; set; }
            public double[] Marks { get; set; }
            public int Score { get; set; }
            public Purple3ParticipantDTO() { }
            public Purple3ParticipantDTO(Purple_3.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Places = participant.Places;
                Marks = participant.Marks;
                Score = participant.Score;
            }
            public Purple_3.Participant ParticipantDTOtoParticipant()
            {
                var participant = new Purple_3.Participant(Name, Surname);
                for (int i = 0; i < Marks.Length; i++)
                    participant.Evaluate(Marks[i]);
                return participant;
            }

        }
        public class Purple3SkatingDTO
        {
            public double[] Moods { get; set; }
            public string Typeclass { get; set; }
            public Purple3ParticipantDTO[] Participants { get; set; }
            public Purple3SkatingDTO() { }
            public Purple3SkatingDTO(Purple_3.Skating skating)
            {
                Moods = skating.Moods;
                if (skating is Purple_3.FigureSkating)
                    Typeclass = typeof(Purple_3.FigureSkating).ToString();
                else //(skating is Purple_3.IceSkating)
                    Typeclass = typeof(Purple_3.IceSkating).ToString();
                Participants = new Purple3ParticipantDTO[skating.Participants.Length];
                for (int i = 0; i < skating.Participants.Length; i++)
                    Participants[i] = new Purple3ParticipantDTO(skating.Participants[i]);
            }
            public Purple_3.Skating SkatingDTOtoSkating()
            {
                Purple_3.Skating skating = null;
                if (Typeclass == typeof(Purple_3.FigureSkating).ToString())
                {
                    skating = new Purple_3.FigureSkating(Moods, false);
                    var participants = new Purple_3.Participant[Participants.Length];
                    for (int i = 0; i < participants.Length; i++)
                        participants[i] = Participants[i].ParticipantDTOtoParticipant();
                    Purple_3.Participant.SetPlaces(participants);
                    skating.Add(participants);
                    return skating;
                }
                else if (Typeclass == typeof(Purple_3.IceSkating).ToString())
                {
                    skating = new Purple_3.IceSkating(Moods, false);
                    var participants = new Purple_3.Participant[Participants.Length];
                    for (int i = 0; i < participants.Length; i++)
                        participants[i] = Participants[i].ParticipantDTOtoParticipant();
                    Purple_3.Participant.SetPlaces(participants);
                    skating.Add(participants);
                    return skating;
                }
                return null;
            }
        }
        public class Purple4SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time{ get; set; }
            public Purple4SportsmanDTO() { }
            public Purple4SportsmanDTO(Purple_4.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Time = sportsman.Time;
            }
            public Purple_4.Sportsman SportsmanDTOtoSportsman()
            {
                var sportsman = new Purple_4.Sportsman(Name, Surname);
                sportsman.Run(Time);
                return sportsman;
            }

        }
        public class Purple4GroupDTO
        {
            public string Name { get; set; }
            public Purple4SportsmanDTO[] Sportsmen { get; set; }
            public Purple4GroupDTO() { }
            public Purple4GroupDTO(Purple_4.Group group)
            {
                Name = group.Name;
                Sportsmen = new Purple4SportsmanDTO[group.Sportsmen.Length];
                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    Sportsmen[i] = new Purple4SportsmanDTO(group.Sportsmen[i]);
                }
            }
            public Purple_4.Group GroupDTOtoGroup()
            {
                var group = new Purple_4.Group(Name);
                var sportsmen = new Purple_4.Sportsman[Sportsmen.Length];
                for (int i = 0; i < Sportsmen.Length; i++)
                    sportsmen[i] = Sportsmen[i].SportsmanDTOtoSportsman();
                group.Add(sportsmen);
                return group;
            }
        }
        public class Purple5ResponseDTO
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
            public Purple5ResponseDTO() { }
            public Purple5ResponseDTO(Purple_5.Response response)
            {
                Animal = response.Animal;
                CharacterTrait = response.CharacterTrait;
                Concept = response.Concept;
            }
            public Purple_5.Response ResponseDTOtoResponse()
            {
                return new Purple_5.Response(Animal, CharacterTrait, Concept);
            }
        }
        public class Purple5Research
        {
            public string Name { get; set; }
            public Purple5ResponseDTO[] Responses { get; set; }
            public Purple5Research() { }
            public Purple5Research(Purple_5.Research research)
            {
                Name = research.Name;
                Responses = new Purple5ResponseDTO[research.Responses.Length];
                for (int i = 0; i < research.Responses.Length; i++)
                    Responses[i] = new Purple5ResponseDTO(research.Responses[i]);
            }
            public Purple_5.Research ResearchDTOtoResearch()
            {
                var research = new Purple_5.Research(Name);
                for (int i = 0; i < Responses.Length; i++)
                    research.Add(new string[] { Responses[i].Animal, Responses[i].CharacterTrait, Responses[i].Concept });
                return research;
            }
        }
        public class Purple5ReportDTO
        {
            public Purple5Research[] Researches { get; set; }
            public Purple5ReportDTO() { }
            public Purple5ReportDTO(Purple_5.Report report)
            {
                Researches = new Purple5Research[report.Researches.Length];
                for (int i = 0;i< Researches.Length; i++)
                    Researches[i] =new Purple5Research(report.Researches[i]);
            }
            public Purple_5.Report ReportDTOtoReport()
            {
                var report = new Purple_5.Report();
                var research = new Purple_5.Research[Researches.Length];
                for (int i = 0; i < Researches.Length; i++)
                    research[i] = Researches[i].ResearchDTOtoResearch();
                report.AddResearch(research);
                return report;
            }
        }

        private void Serialize_Xml<T>(T obj, string filename) where T:class
        {
            SelectFile(filename);
            if (FilePath == null) return;
            var serializer = new XmlSerializer(typeof(T));
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                serializer.Serialize(sw, obj);
            }
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj is Purple_1.Participant participant)
            {
                var dto = new Purple1ParticipantDTO(participant);
                Serialize_Xml(dto, fileName);
            }
            else if (obj is Purple_1.Judge judje)
            {
                var dto = new Purple1JudgeDTO(judje);
                Serialize_Xml(dto, fileName);
            }
            else if (obj is Purple_1.Competition competition)
            {
                var dto = new Purple1CompetitionDTO(competition);
                Serialize_Xml(dto, fileName);
            }
            else return;
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (jumping is Purple_2.JuniorSkiJumping junior)
            {
                var dto = new Purple2SkiJumpingDTO(junior);
                Serialize_Xml(dto, fileName);
            }
            else if (jumping is Purple_2.ProSkiJumping pro)
            {
                var dto = new Purple2SkiJumpingDTO(pro);
                Serialize_Xml(dto, fileName);
            }
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            var dto = new Purple3SkatingDTO(skating);
            Serialize_Xml(dto, fileName);
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            
            var dto = new Purple4GroupDTO(group);
            Serialize_Xml(dto, fileName);
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            var dto = new Purple5ReportDTO(report);
            Serialize_Xml(dto, fileName);
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            if (typeof(T) == typeof(Purple_1.Participant))
            {
                SelectFile(fileName);
                if (FilePath == null) return null;
                var serializer = new XmlSerializer(typeof(Purple1ParticipantDTO));
                
                using var reader = new StreamReader(FilePath);
                var dto = (Purple1ParticipantDTO)serializer.Deserialize(reader);
                reader.Close();
                var participant = dto.ParticipantDtotoParticipant();

                return participant as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {

                SelectFile(fileName);
                if (FilePath == null) return null;
                var serializer = new XmlSerializer(typeof(Purple1JudgeDTO));

                using var reader = new StreamReader(FilePath);
                var dto = (Purple1JudgeDTO)serializer.Deserialize(reader);
                reader.Close();
                var judje = dto.JudgeDTOtoJudge();

                return judje as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                SelectFile(fileName);
                if (FilePath == null) return null;
                var serializer = new XmlSerializer(typeof(Purple1CompetitionDTO));

                using var reader = new StreamReader(FilePath);
                var dto = (Purple1CompetitionDTO)serializer.Deserialize(reader);
                reader.Close();
                var competition = dto.CompetitionDTOtoCompetiton();

                return competition as T;
            }
            return null;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null) return null;
            var serializer = new XmlSerializer(typeof(Purple2SkiJumpingDTO));

            using var reader = new StreamReader(FilePath);
            var dto = (Purple2SkiJumpingDTO)serializer.Deserialize(reader);
            reader.Close();
            var competition = dto.SkiJumpingDTOtoSkiJumping();
            return competition as T;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null) return null;
            var serializer = new XmlSerializer(typeof(Purple3SkatingDTO));

            using var reader = new StreamReader(FilePath);
            var dto = (Purple3SkatingDTO)serializer.Deserialize(reader);
            reader.Close();
            Purple_3.Skating skating = null;
            skating = dto.SkatingDTOtoSkating();
            return skating as T;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null) return null;
            var serializer = new XmlSerializer(typeof(Purple4GroupDTO));

            using var reader = new StreamReader(FilePath);
            var dto = (Purple4GroupDTO)serializer.Deserialize(reader);
            reader.Close();
            
            var group = dto.GroupDTOtoGroup();
           
            return group;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null) return null;
            var serializer = new XmlSerializer(typeof(Purple5ReportDTO));

            using var reader = new StreamReader(FilePath);
            var dto = (Purple5ReportDTO)serializer.Deserialize(reader);
            reader.Close();

            var report = dto.ReportDTOtoReport();

            return report;
        }
    }
}
