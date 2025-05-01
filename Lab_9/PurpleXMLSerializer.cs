using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab_9
{
    public class PurpleXMLSerializer: PurpleSerializer
    {
        public override string Extension => "xml";

        public class Purple1ParticipantDTO {
            
            public Purple1ParticipantDTO() {}

            public Purple1ParticipantDTO(Purple_1.Participant participant) {
                Name = participant.Name;
                Surname = participant.Surname;
                Coefs = participant.Coefs;

                int rows = participant.Marks.GetLength(0), cols = participant.Marks.GetLength(1);

                Marks = new int[rows][];

                for (int i = 0; i < rows; i++) {
                    Marks[i] = new int[cols];
                    for (int j = 0; j < cols; j++) {
                        Marks[i][j] = participant.Marks[i, j];
                    }
                }
            }

            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
        }

        public class Purple1JudgeDTO {
            public Purple1JudgeDTO() {}

            public Purple1JudgeDTO(Purple_1.Judge judge) {
                Name = judge.Name;
                Marks = judge.Marks;
            }
            public string Name { get; set; }
            public int[] Marks { get; set; }
        }

        public class Purple1CompetitionDTO {

            public Purple1CompetitionDTO() {}

            public Purple1CompetitionDTO(Purple_1.Competition competition) {
                int judgeLen = competition.Judges.Length;
                int partLen = competition.Participants.Length;
                Judges = new Purple1JudgeDTO[judgeLen];
                Participants = new Purple1ParticipantDTO[partLen];

                for (int i = 0; i < judgeLen; i++)
                    Judges[i] = new Purple1JudgeDTO(competition.Judges[i]);
                
                for (int i = 0; i < partLen; i++)
                    Participants[i] = new Purple1ParticipantDTO(competition.Participants[i]);

            }

            public Purple1JudgeDTO[] Judges { get; set; }
            public Purple1ParticipantDTO[] Participants { get; set; }
        }

        private void SerializeObject<T>(StreamWriter writer, T obj) {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, obj);
        }

        private T ReadFromFile<T>(StreamReader reader) {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(reader);
        }
        public override void SerializePurple1<T>(T obj, string fileName) where T : class
        {
            if (obj == null)
                return;
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                if (typeof(T) == typeof(Purple_1.Participant))
                {
                    Purple1ParticipantDTO participant = new Purple1ParticipantDTO(obj as Purple_1.Participant);
                    SerializeObject<Purple1ParticipantDTO>(writer, participant);
                }
                else if (typeof(T) == typeof(Purple_1.Judge))
                {
                    Purple1JudgeDTO judge = new Purple1JudgeDTO(obj as Purple_1.Judge);
                    SerializeObject<Purple1JudgeDTO>(writer, judge);
                }
                else if (typeof(T) == typeof(Purple_1.Competition))
                {
                    Purple1CompetitionDTO competition = new Purple1CompetitionDTO(obj as Purple_1.Competition);
                    SerializeObject<Purple1CompetitionDTO>(writer, competition);
                }
            }
        }
        
        public class Purple2ParticipantDTO {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }

            public Purple2ParticipantDTO() {}

            public Purple2ParticipantDTO(Purple_2.Participant participant) {
                Name = participant.Name;
                Surname = participant.Surname;
                Distance = participant.Distance;
                Marks = participant.Marks;
            }
        }
        
        public class SkiJumpingDto {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Standard { get; set; }
            public Purple2ParticipantDTO[] Participants { get; set; }

            public SkiJumpingDto() {}

            public SkiJumpingDto(Purple_2.SkiJumping skiJumping) {
                if (skiJumping is Purple_2.JuniorSkiJumping) {
                    Type = typeof(Purple_2.JuniorSkiJumping).Name;
                } else {
                    Type = typeof(Purple_2.ProSkiJumping).Name;
                }

                Name = skiJumping.Name;

                Standard = skiJumping.Standard;

                int partLen = skiJumping.Participants.Length;

                Participants = new Purple2ParticipantDTO[partLen];

                for (int i = 0; i < partLen; i++) {
                    Participants[i] = new Purple2ParticipantDTO(skiJumping.Participants[i]);
                }
            }
        }
        
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (jumping == null) return;

            SelectFile(fileName);


            using (StreamWriter writer = File.AppendText(FilePath))
            {

                SkiJumpingDto skiJumpingDto;
                if (jumping is Purple_2.JuniorSkiJumping)
                {
                    skiJumpingDto = new SkiJumpingDto(jumping as Purple_2.JuniorSkiJumping);
                }
                else
                {
                    skiJumpingDto = new SkiJumpingDto(jumping as Purple_2.ProSkiJumping);
                }
                SerializeObject<SkiJumpingDto>(writer, skiJumpingDto);
            }
        }
        private void writePurple3Participant(StreamWriter writer, Purple_3.Participant participant)
        {
            writer.WriteLine($"Type: {nameof(Purple_3.Participant)}");
            writer.WriteLine($"Name: {participant.Name}");
            writer.WriteLine($"Surname: {participant.Surname}");
            writer.WriteLine($"Marks: {String.Join(" ", participant.Marks.Cast<int>().ToArray())}");
            writer.WriteLine($"Places: {String.Join(" ", participant.Places)}");
        }

        public class Purple3ParticipantDTO {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }

            public Purple3ParticipantDTO() {}

            public Purple3ParticipantDTO(Purple_3.Participant participant) {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = participant.Marks;
            }
        }

        public class SkatingDto {
            public string Type { get; set; }
            public double[] Moods { get; set; }
            public Purple3ParticipantDTO[] Participants { get; set; }

            public SkatingDto() {}

            public SkatingDto(Purple_3.Skating skating) {
                
                if (skating is Purple_3.IceSkating) {
                    Type = typeof(Purple_3.IceSkating).Name;
                } else {
                    Type = typeof(Purple_3.FigureSkating).Name;
                }
                Moods = skating.Moods;

                int partLen = skating.Participants.Length;

                Participants = new Purple3ParticipantDTO[partLen];

                for (int i = 0; i < partLen; i++) {
                    Participants[i] = new Purple3ParticipantDTO(skating.Participants[i]);
                }
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            if (skating == null) return;
            SelectFile(fileName);


            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                SkatingDto skateDto = new SkatingDto(skating);
                SerializeObject<SkatingDto>(writer, skateDto);
            }
        }
        public class SportsmanDTO {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }

            public SportsmanDTO() {}

            public SportsmanDTO(Purple_4.Sportsman sportsman) {
                Name = sportsman.Name;
                Surname = sportsman.Surname;

                Time = sportsman.Time;
            }
        }

        public class GroupDto {
            public string Name { get; set; }
            public SportsmanDTO[] Sportsmen { get; set; }

            public GroupDto() {}

            public GroupDto(Purple_4.Group group) {
                Name = group.Name;
                
                int sportLen = group.Sportsmen.Length;

                Sportsmen = new SportsmanDTO[sportLen];

                for (int i = 0; i < sportLen; i++) {
                    Sportsmen[i] = new SportsmanDTO(group.Sportsmen[i]);
                }
            }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            if (group == null) return;

            SelectFile(fileName);

            using (StreamWriter writer = File.AppendText(FilePath))
            {
                GroupDto groupDto = new GroupDto(group);

                SerializeObject<GroupDto>(writer, groupDto);
            }

        }

        public class ResponseDTO {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }

            public ResponseDTO() {}

            public ResponseDTO(Purple_5.Response response) {
                Animal = response.Animal;
                CharacterTrait = response.CharacterTrait;
                Concept = response.Concept;
            }
        }
        
        public class ResearchDTO {
            public string Name { get; set; }
            public ResponseDTO[] Responses { get; set; }

            public ResearchDTO() {}

            public ResearchDTO(Purple_5.Research research) {
                Name = research.Name;
                
                int rspLen = research.Responses.Length;

                Responses = new ResponseDTO[rspLen];

                for (int i = 0; i < rspLen; i++) {
                    Responses[i] = new ResponseDTO(research.Responses[i]);
                }
            }
        }

        public class ReportDTO {
            public ResearchDTO[] Researches { get; set; }

            public ReportDTO() {}

            public ReportDTO(Purple_5.Report report) {
                
                int rschLen = report.Researches.Length;

                Researches = new ResearchDTO[rschLen];

                for (int i = 0; i < rschLen; i++) {
                    Researches[i] = new ResearchDTO(report.Researches[i]);
                }
            }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            if (report == null) return;

            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                ReportDTO reportDTO = new ReportDTO(report);

                SerializeObject<ReportDTO>(writer, reportDTO);
            }
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            if (fileName == null) return null;
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                if (typeof(T) == typeof(Purple_1.Participant)) {
                    Purple1ParticipantDTO partDto = ReadFromFile<Purple1ParticipantDTO>(reader);

                    Purple_1.Participant participant = new Purple_1.Participant(partDto.Name, partDto.Surname);

                    participant.SetCriterias(partDto.Coefs);

                    foreach(int[] marks in partDto.Marks)
                        participant.Jump(marks);
                    
                    return participant as T;
                } else if (typeof(T) == typeof(Purple_1.Judge)) {
                    Purple1JudgeDTO judgeDto = ReadFromFile<Purple1JudgeDTO>(reader);

                    Purple_1.Judge judge = new Purple_1.Judge(judgeDto.Name, judgeDto.Marks);
                    
                    return judge as T;
                } else {
                    Purple1CompetitionDTO compDto = ReadFromFile<Purple1CompetitionDTO>(reader);
                    Purple_1.Judge[] judges = compDto.Judges.Select(j => new Purple_1.Judge(j.Name, j.Marks)).ToArray();
                    Purple_1.Competition competition = new Purple_1.Competition(judges);

                    foreach(Purple1ParticipantDTO partDto in compDto.Participants) {
                        Purple_1.Participant participant = new Purple_1.Participant(partDto.Name, partDto.Surname); 

                        participant.SetCriterias(partDto.Coefs);

                        foreach (int[] marks in partDto.Marks) {
                            participant.Jump(marks);
                        }
                        competition.Add(participant);
                    }
                    
                    return competition as T;
                }

            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            if (fileName == null) {
                return null;
            }

            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                SkiJumpingDto skiDto = ReadFromFile<SkiJumpingDto>(reader);
                Purple_2.Participant[] participants = new Purple_2.Participant[skiDto.Participants.Length];
                int iter = 0;
                foreach (Purple2ParticipantDTO partDto in skiDto.Participants) {
                    participants[iter] = new Purple_2.Participant(partDto.Name, partDto.Surname);
                    participants[iter].Jump(partDto.Distance, partDto.Marks, skiDto.Standard);
                    iter++;
                }
                if (skiDto.Type == nameof(Purple_2.ProSkiJumping)) {
                    Purple_2.ProSkiJumping proSkiJumping = new Purple_2.ProSkiJumping();
                    proSkiJumping.Add(participants);
                    return proSkiJumping as T;
                } else {
                    Purple_2.JuniorSkiJumping juniorSkiJumping = new Purple_2.JuniorSkiJumping();
                    juniorSkiJumping.Add(participants);
                    return juniorSkiJumping as T;
                }
            }
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            if (fileName == null) {
                return null;
            }

            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                SkatingDto skateDto = ReadFromFile<SkatingDto>(reader);
                Purple_3.Participant[] participants = new Purple_3.Participant[skateDto.Participants.Length];
                int iter = 0;
                foreach (Purple3ParticipantDTO partDto in skateDto.Participants) {
                    participants[iter] = new Purple_3.Participant(partDto.Name, partDto.Surname);
                    foreach (double mark in partDto.Marks)
                        participants[iter].Evaluate(mark);
                    iter++;
                }
                double[] moods = skateDto.Moods;
                if (skateDto.Type == nameof(Purple_3.IceSkating)) {
                    for (int i = 0; i < moods.Length; i++) {
                        moods[i] /= (1 + (i + 1) / 100.0);
                    }

                    Purple_3.IceSkating iceSkating = new Purple_3.IceSkating(moods);
                    iceSkating.Add(participants);
                    return iceSkating as T;
                } else {
                    for (int i = 0; i < moods.Length; i++) {
                        moods[i] -= ((i + 1) / 10.0);
                    }
                    
                    Purple_3.FigureSkating figureSkating = new Purple_3.FigureSkating(moods);
                    figureSkating.Add(participants);
                    return figureSkating as T;
                }
            }
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            if (fileName == null) {
                return null;
            }
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                GroupDto groupDto = ReadFromFile<GroupDto>(reader);
                if (groupDto == null)
                    return null;
                Purple_4.Sportsman[] sportsmen = new Purple_4.Sportsman[groupDto.Sportsmen.Length];
                int iter = 0;
                foreach (SportsmanDTO sportDto in groupDto.Sportsmen) {
                    sportsmen[iter] = new Purple_4.Sportsman(sportDto.Name, sportDto.Surname);
                    sportsmen[iter].Run(sportDto.Time);
                    iter++;
                }
                Purple_4.Group group = new Purple_4.Group(groupDto.Name);
                group.Add(sportsmen);
                return group;
            }
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            if (fileName == null) {
                return null;
            }
            SelectFile(fileName);

            using (StreamReader reader = new StreamReader(FilePath))
            {
                ReportDTO reportDto = ReadFromFile<ReportDTO>(reader);
                if (reportDto == null)
                    return null;
                Purple_5.Research[] researches = new Purple_5.Research[reportDto.Researches.Length];

                Purple_5.Report report = new Purple_5.Report();
                int iter = 0;
                foreach (ResearchDTO rschDto in reportDto.Researches) {
                    researches[iter] = new Purple_5.Research(rschDto.Name);
                    foreach(ResponseDTO rsp in rschDto.Responses) {
                        researches[iter].Add(new string[] {rsp.Animal, rsp.CharacterTrait, rsp.Concept});
                    }
                    iter++;
                }
                report.AddResearch(researches);
                return report;
            }
        }
    }
}
