using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using System.Xml.Serialization;
using System.Reflection.Metadata;
using System.Formats.Asn1;
using System.IO;

namespace Lab_9{
    public class BlueXMLSerializer : BlueSerializer{
        public override string Extension{
            get{
                return "xml";
            }
        }
        public class ResponseDTO
        {
            public string Type { get; set; } 
            public string Name { get; set; }
            public int Votes { get; set; }
            public string? Surname { get; set; } 

            public ResponseDTO() { }

            public ResponseDTO(Blue_1.Response response) {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;
                if (response is Blue_1.HumanResponse r)
                    Surname = r.Surname;
            }
        }
        private static int[][] ChangeMatrix (int[,] matrix) { // Преобразует обычную матрицу (int[,]) в "рваный" массив (int[][]), где каждая строка — это отдельный массив.
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[][] changed = new int[rows][];
            for (int i = 0; i < rows; i++) {
                changed[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    changed[i][j] = matrix[i, j];
            }
            return changed;
        }

        private static int[,] MatrixToOriginal (int[][] arr) {
            int rows = arr.Length;
            int cols = arr[0].Length;
            int[,] original = new int[rows, cols];
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < cols; j++){
                    original[i, j] = arr[i][j];
                }
            }

            return original;
        }
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            ResponseDTO obj = new ResponseDTO(participant);
            var serializer = new XmlSerializer(typeof(ResponseDTO));
            string path = Path.Combine(FolderPath, fileName);
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj);
            }
        }
        public class WaterJumpDTO {

            public string Type {get; set;}

            public string Name {get; set;}

            public int Bank {get; set;}

            public ParticipantB2DTO[] Participants {get; set;}

            public WaterJumpDTO() {}

            public WaterJumpDTO(Blue_2.WaterJump wj) {
                Type = wj.GetType().Name;
                Name = wj.Name;
                Bank = wj.Bank;
                Participants = wj.Participants.Select(p => new ParticipantB2DTO(p)).ToArray();
            }
        }

        public class ParticipantB2DTO {
            public string Name {get; set;}
            public string Surname {get; set;}

            public int[][] Marks {get; set;}

            public ParticipantB2DTO() {}

            public ParticipantB2DTO(Blue_2.Participant participant) {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = ChangeMatrix(participant.Marks);
            }

        }

        
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            WaterJumpDTO obj = new WaterJumpDTO(participant);
            var serializer = new XmlSerializer(typeof(WaterJumpDTO));
            string path = Path.Combine(FolderPath, fileName);
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj);
            }
        }
        public class ParticipantB3DTO {
            public string Type {get; set;}
            public string Name {get; set;}
            public string Surname {get; set;}
            public int[] Penalties {get; set;}

            public ParticipantB3DTO() {}

            public ParticipantB3DTO(Blue_3.Participant p) {
                Type = p.GetType().Name;
                Name = p.Name;
                Surname = p.Surname;
                Penalties = p.Penalties;
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName) where T: class
        {
            if (student == null || String.IsNullOrEmpty(fileName)) return;
            ParticipantB3DTO obj = new ParticipantB3DTO(student);
            var serializer = new XmlSerializer(typeof(ParticipantB3DTO));
            string path = Path.Combine(FolderPath, fileName);
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj);
            }
        }
        public class TeamDTO {
            public string Type {get; set;}
            public string Name {get; set;}
            public int[] Scores {get; set;}
            public TeamDTO() {}
            public TeamDTO(Blue_4.Team t) {
                Type = t.GetType().Name;
                Name = t.Name;
                Scores = t.Scores;
            } 
        }
        public class GroupDTO {
            public string Name {get; set;}
            public TeamDTO[] ManTeams {get; set;}
            public TeamDTO[] WomanTeams {get; set;}
            public GroupDTO() {}
            public GroupDTO(Blue_4.Group g) {
                Name = g.Name;
                ManTeams = g.ManTeams.Select(t => t == null ? null : new TeamDTO(t)).ToArray();
                WomanTeams = g.WomanTeams.Select(t => t == null ? null : new TeamDTO(t)).ToArray();
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            GroupDTO obj = new GroupDTO(participant);
            var serializer = new XmlSerializer(typeof(GroupDTO));
            string path = Path.Combine(FolderPath, fileName);
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj);
            }
        }
        public class SportsmanDTO {
            public string Name {get; set;}
            public string Surname {get; set;}
            public int Place {get; set;}
            public SportsmanDTO() {}
            public SportsmanDTO(Blue_5.Sportsman s) {
                Name = s.Name;
                Surname = s.Surname;
                Place = s.Place;
            }
        }

        public class TeamB5DTO {
            public string Type {get; set;}
            public string Name {get; set;}
            public SportsmanDTO[] Sportsmen {get; set;}

            public TeamB5DTO() {}
            public TeamB5DTO(Blue_5.Team t) {
                Type = t.GetType().Name;
                Name = t.Name;
                Sportsmen = t.Sportsmen.Select(s => s != null ? new SportsmanDTO(s) : null).ToArray();
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName) where T: class
        {
            if (group == null || String.IsNullOrEmpty(fileName)) return;
            object obj = null;
            var serializer = default(XmlSerializer);
            if (group.GetType().Name == nameof(Blue_5.ManTeam) || group.GetType().Name == nameof(Blue_5.WomanTeam))
            {
                obj = new TeamB5DTO(group);
                serializer = new XmlSerializer(typeof(TeamB5DTO));
            }
            else if (group is Blue_5.Sportsman sportsman){
                obj = new SportsmanDTO(sportsman);
                serializer = new XmlSerializer(typeof(SportsmanDTO));
            }
            string path = Path.Combine(FolderPath, fileName);
            using(var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {

            Blue_1.Response result;
            ResponseDTO responseData;
            string path = Path.Combine(FolderPath, fileName);
            var xmlFormatter = new XmlSerializer(typeof(ResponseDTO));
            using (var stream = new StreamReader(path))
            {
                responseData = (ResponseDTO)xmlFormatter.Deserialize(stream);
            }

            if (!string.IsNullOrEmpty(responseData.Surname))
            {
                result = new Blue_1.HumanResponse(responseData.Name, responseData.Surname, responseData.Votes);
            }
            else
            {
                result = new Blue_1.Response(responseData.Name, responseData.Votes);
            }

            return result;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            
            WaterJumpDTO waterJumpData;
            Blue_2.WaterJump waterJump;
            string path = Path.Combine(FolderPath, fileName);
            var xmlDeserializer = new XmlSerializer(typeof(WaterJumpDTO));
            using (var fileReader = new StreamReader(path))
            {
                waterJumpData = (WaterJumpDTO)xmlDeserializer.Deserialize(fileReader);
            }

            waterJump = waterJumpData.Type == "WaterJump3m"
                ? new Blue_2.WaterJump3m(waterJumpData.Name, waterJumpData.Bank)
                : new Blue_2.WaterJump5m(waterJumpData.Name, waterJumpData.Bank);

            foreach (var participantDto in waterJumpData.Participants)
            {
                var participant = new Blue_2.Participant(participantDto.Name, participantDto.Surname);
                var scoresMatrix = MatrixToOriginal(participantDto.Marks);

                for (int attempt = 0; attempt < 2; attempt++)
                {
                    var attemptMarks = new int[5];
                    for (int judge = 0; judge < 5; judge++)
                    {
                        attemptMarks[judge] = scoresMatrix[attempt, judge];
                    }
                    participant.Jump(attemptMarks);
                }

                waterJump.Add(participant);
            }

            return waterJump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            
            ParticipantB3DTO data;
            Blue_3.Participant participantInstance = null;

            string path = Path.Combine(FolderPath, fileName);
            var xmlReader = new XmlSerializer(typeof(ParticipantB3DTO));
            using (var stream = new StreamReader(path))
            {
                data = (ParticipantB3DTO)xmlReader.Deserialize(stream);
            }

            switch (data.Type)
            {
                case "Participant":
                    participantInstance = new Blue_3.Participant(data.Name, data.Surname);
                    break;
                case "HockeyPlayer":
                    participantInstance = new Blue_3.HockeyPlayer(data.Name, data.Surname);
                    break;
                case "BasketballPlayer":
                    participantInstance = new Blue_3.BasketballPlayer(data.Name, data.Surname);
                    break;
            }

            foreach (var penalty in data.Penalties)
            {
                participantInstance.PlayMatch(penalty);
            }

            return participantInstance as T;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            string path = Path.Combine(FolderPath, fileName);
            GroupDTO groupData;
            Blue_4.Group groupInstance;

            var xmlReader = new XmlSerializer(typeof(GroupDTO));
            using (var stream = new StreamReader(path))
            {
                groupData = (GroupDTO)xmlReader.Deserialize(stream);
            }

            groupInstance = new Blue_4.Group(groupData.Name);

            foreach (var manTeamDto in groupData.ManTeams)
            {
                if (manTeamDto == null)
                {
                    groupInstance.Add(default(Blue_4.ManTeam));
                    continue;
                }

                Blue_4.ManTeam team = null;
                if (manTeamDto.Type == "ManTeam")
                {
                    team = new Blue_4.ManTeam(manTeamDto.Name);
                    foreach (var score in manTeamDto.Scores)
                    {
                        team.PlayMatch(score);
                    }
                }

                groupInstance.Add(team);
            }

            foreach (var womanTeamDto in groupData.WomanTeams)
            {
                if (womanTeamDto == null)
                {
                    groupInstance.Add(default(Blue_4.WomanTeam));
                    continue;
                }

                Blue_4.WomanTeam team = null;
                if (womanTeamDto.Type == "WomanTeam")
                {
                    team = new Blue_4.WomanTeam(womanTeamDto.Name);
                    foreach (var score in womanTeamDto.Scores)
                    {
                        team.PlayMatch(score);
                    }
                }

                groupInstance.Add(team);
            }

            return groupInstance;
        }

        private Blue_5.Sportsman ConvertDtoToSportsman(SportsmanDTO dto)
        {
            var sportsman = new Blue_5.Sportsman(dto.Name, dto.Surname);
            sportsman.SetPlace(dto.Place);
            return sportsman;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            
            string path = Path.Combine(FolderPath, fileName);
            using (var fileStream = new StreamReader(path))
            {
                if (typeof(T) == typeof(Blue_5.Sportsman))
                {
                    var sportsmanReader = new XmlSerializer(typeof(SportsmanDTO));
                    var sportsmanData = (SportsmanDTO)sportsmanReader.Deserialize(fileStream);

                    var individual = new Blue_5.Sportsman(sportsmanData.Name, sportsmanData.Surname);
                    individual.SetPlace(sportsmanData.Place);
                    return individual as T;
                }
                else
                {
                    var teamReader = new XmlSerializer(typeof(TeamB5DTO));
                    var teamData = (TeamB5DTO)teamReader.Deserialize(fileStream);

                    if (teamData.Type == "WomanTeam")
                    {
                        var femaleTeam = new Blue_5.WomanTeam(teamData.Name);
                        foreach (var memberDto in teamData.Sportsmen)
                        {
                            if (memberDto == null) continue;
                            femaleTeam.Add(ConvertDtoToSportsman(memberDto));
                        }
                        return femaleTeam as T;
                    }
                    else
                    {
                        var maleTeam = new Blue_5.ManTeam(teamData.Name);
                        foreach (var memberDto in teamData.Sportsmen)
                        {
                            if (memberDto == null) continue;
                            maleTeam.Add(ConvertDtoToSportsman(memberDto));
                        }
                        return maleTeam as T;
                    }
                }
            }
        }

    }
}