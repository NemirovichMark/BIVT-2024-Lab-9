using Lab_7;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Lab_9
{
    public class BlueXMLSerializer: BlueSerializer
    {
        public override string Extension => "xml";

        public class Blue_1_ResponseDTO
        {
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }

            public Blue_1_ResponseDTO() { }

            public Blue_1_ResponseDTO(Blue_1.Response response)
            {
                if (response == null) return;

                Name = response.Name;
                Votes = response.Votes;
                Surname = (response as Blue_1.HumanResponse)?.Surname;
            }
        }

        public class Blue_2_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }

            public Blue_2_ParticipantDTO() { }

            public Blue_2_ParticipantDTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;

                int[,] original = participant.Marks;
                if (original != null)
                {
                    int rows = original.GetLength(0);
                    int cols = original.GetLength(1);
                    Marks = new int[rows][];

                    for (int row = 0; row < rows; row++)
                    {
                        int[] currentRow = new int[cols];
                        for (int col = 0; col < cols; col++)
                        {
                            currentRow[col] = original[row, col];
                        }
                        Marks[row] = currentRow;
                    }
                }
            }
        }

        public class Blue_2_WaterJumpDTO
        {
            public int Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2_ParticipantDTO[] Participants { get; set; }

            public Blue_2_WaterJumpDTO() { }

            public Blue_2_WaterJumpDTO(Blue_2.WaterJump jump)
            {
                if (jump == null)
                    return;

                if (jump is Blue_2.WaterJump3m)
                    Type = 3;
                else if (jump is Blue_2.WaterJump5m)
                    Type = 5;

                Name = jump.Name;
                Bank = jump.Bank;

                int count = jump.Participants.Length;
                Participants = new Blue_2_ParticipantDTO[count];

                for (int index = 0; index < count; index++)
                {
                    Blue_2.Participant originalParticipant = jump.Participants[index];
                    Participants[index] = new Blue_2_ParticipantDTO(originalParticipant);
                }
            }
        }

        public class Blue_3_ParticipantDTO
        {
            public char Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }

            public Blue_3_ParticipantDTO() { }

            public Blue_3_ParticipantDTO(Blue_3.Participant participant)
            {
                if (participant == null)
                    return;

                char typeCode = 'P';

                if (participant is Blue_3.BasketballPlayer)
                    typeCode = 'B';
                else if (participant is Blue_3.HockeyPlayer)
                    typeCode = 'H';

                Type = typeCode;
                Name = participant.Name;
                Surname = participant.Surname;

                if (participant.Penalties != null)
                {
                    int length = participant.Penalties.Length;
                    Penalties = new int[length];
                    for (int i = 0; i < length; i++)
                    {
                        Penalties[i] = participant.Penalties[i];
                    }
                }
            }
        }

        public class Blue_4_TeamDTO
        {
            public string Name { get; set; }
            public int[] Scores { get; set; }

            public Blue_4_TeamDTO() { }

            public Blue_4_TeamDTO(Blue_4.Team team)
            {
                if (team == null)
                    return;

                Name = team.Name;

                if (team.Scores != null)
                {
                    int size = team.Scores.Length;
                    Scores = new int[size];
                    for (int i = 0; i < size; i++)
                    {
                        Scores[i] = team.Scores[i];
                    }
                }
            }
        }

        public class Blue_4_GroupDTO
        {
            public string Name { get; set; }
            public Blue_4_TeamDTO[] ManTeams { get; set; }
            public Blue_4_TeamDTO[] WomanTeams { get; set; }

            public Blue_4_GroupDTO() { }

            public Blue_4_GroupDTO(Blue_4.Group group)
            {
                if (group == null)
                    return;

                Name = group.Name;

                if (group.ManTeams != null)
                {
                    int countMan = group.ManTeams.Length;
                    ManTeams = new Blue_4_TeamDTO[countMan];
                    for (int index = 0; index < countMan; index++)
                    {
                        var current = group.ManTeams[index];
                        if (current != null)
                        {
                            ManTeams[index] = new Blue_4_TeamDTO(current);
                        }
                    }
                }

                if (group.WomanTeams != null)
                {
                    int countWoman = group.WomanTeams.Length;
                    WomanTeams = new Blue_4_TeamDTO[countWoman];
                    for (int j = 0; j < countWoman; j++)
                    {
                        var current = group.WomanTeams[j];
                        if (current != null)
                        {
                            WomanTeams[j] = new Blue_4_TeamDTO(current);
                        }
                    }
                }
            }
        }

        public class Blue_5_SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }

            public Blue_5_SportsmanDTO() { }

            public Blue_5_SportsmanDTO(Blue_5.Sportsman source)
            {
                if (source == null)
                    return;

                Name = source.Name;
                Surname = source.Surname;
                Place = source.Place;
            }
        }

        public class Blue_5_TeamDTO
        {
            public char Type { get; set; }
            public string Name { get; set; }
            public Blue_5_SportsmanDTO[] Sportsmen { get; set; }

            public Blue_5_TeamDTO() { }

            public Blue_5_TeamDTO(Blue_5.Team originalTeam)
            {
                if (originalTeam is Blue_5.ManTeam)
                    Type = 'M';
                else if (originalTeam is Blue_5.WomanTeam)
                    Type = 'W';

                Name = originalTeam.Name;

                int count = originalTeam.Sportsmen.Length;
                Sportsmen = new Blue_5_SportsmanDTO[count];
                int index = 0;

                while (index < count)
                {
                    var current = originalTeam.Sportsmen[index];
                    if (current == null)
                        break;

                    Sportsmen[index] = new Blue_5_SportsmanDTO(current);
                    index++;
                }
            }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName))
                return;

            var responseDTO = new Blue_1_ResponseDTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));

            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, responseDTO);
            }
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName?.Trim()))
                return;

            var dto = new Blue_2_WaterJumpDTO(participant);
            var xmlSerializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));

            using (var stream = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(stream, dto);
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName?.Trim()))
                return;

            var dto = new Blue_3_ParticipantDTO(student);
            var xmlSerializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));

            using (var fileStream = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(fileStream, dto);
            }
        }

        public override void SerializeBlue4Group(Blue_4.Group group, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;
            if (group == null) return;

            void SerializeToFile<T>(T dto, string path)
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using var fileWriter = new StreamWriter(path);
                xmlSerializer.Serialize(fileWriter, dto);
            }

            var dtoGroup = new Blue_4_GroupDTO(group);
            SerializeToFile(dtoGroup, fileName);
        }

        public override void SerializeBlue5Team<T>(T team, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;
            if (team == null) return;

            void SerializeToFile<TDto>(TDto dto, string path)
            {
                var serializer = new XmlSerializer(typeof(TDto));
                using var writer = new StreamWriter(path);
                serializer.Serialize(writer, dto);
            }

            var dtoTeam = new Blue_5_TeamDTO(team);
            SerializeToFile(dtoTeam, fileName);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            using var reader = new StreamReader(fileName);
            var serializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));
            var dto = (Blue_1_ResponseDTO)serializer.Deserialize(reader);

            Blue_1.Response response = dto.Surname == null
                ? new Blue_1.Response(dto.Name, dto.Votes)
                : new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes);

            return response;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            using var reader = new StreamReader(fileName);
            var serializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));
            var dto = (Blue_2_WaterJumpDTO)serializer.Deserialize(reader);

            Blue_2.WaterJump waterJump = dto.Type == 3
                ? new Blue_2.WaterJump3m(dto.Name, dto.Bank)
                : new Blue_2.WaterJump5m(dto.Name, dto.Bank);

            foreach (var participantDto in dto.Participants)
            {
                var participant = new Blue_2.Participant(participantDto.Name, participantDto.Surname);

                int[] firstJump = new int[5];
                int[] secondJump = new int[5];

                for (int k = 0; k < 5; k++)
                {
                    firstJump[k] = participantDto.Marks[0][k];
                    secondJump[k] = participantDto.Marks[1][k];
                }

                participant.Jump(firstJump);
                participant.Jump(secondJump);

                waterJump.Add(participant);
            }

            return waterJump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            using var reader = new StreamReader(fileName);
            var serializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));
            var dto = (Blue_3_ParticipantDTO)serializer.Deserialize(reader);

            Blue_3.Participant participant = dto.Type switch
            {
                'B' => new Blue_3.BasketballPlayer(dto.Name, dto.Surname),
                'H' => new Blue_3.HockeyPlayer(dto.Name, dto.Surname),
                _ => new Blue_3.Participant(dto.Name, dto.Surname)
            };

            foreach (var penalty in dto.Penalties)
            {
                participant.PlayMatch(penalty);
            }

            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            using var reader = new StreamReader(fileName);
            var serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));
            var dto = (Blue_4_GroupDTO)serializer.Deserialize(reader);

            var group = new Blue_4.Group(dto.Name);

            Action<Blue_4_TeamDTO[], Func<string, Blue_4.Team>> addTeams = (teamsDto, teamFactory) =>
            {
                foreach (var teamDto in teamsDto)
                {
                    if (teamDto == null) break;
                    var team = teamFactory(teamDto.Name);
                    if (teamDto.Scores != null)
                    {
                        foreach (var score in teamDto.Scores)
                        {
                            team.PlayMatch(score);
                        }
                    }
                    group.Add(team);
                }
            };

            addTeams(dto.ManTeams, name => new Blue_4.ManTeam(name));
            addTeams(dto.WomanTeams, name => new Blue_4.WomanTeam(name));

            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return default;

            using var reader = new StreamReader(fileName);
            var serializer = new XmlSerializer(typeof(Blue_5_TeamDTO));
            var dto = (Blue_5_TeamDTO)serializer.Deserialize(reader);

            Blue_5.Team team = dto.Type == 'M'
                ? new Blue_5.ManTeam(dto.Name)
                : new Blue_5.WomanTeam(dto.Name);

            if (dto.Sportsmen != null)
            {
                foreach (var sportsmanDto in dto.Sportsmen)
                {
                    if (sportsmanDto == null) break;
                    var sportsman = new Blue_5.Sportsman(sportsmanDto.Name, sportsmanDto.Surname);
                    sportsman.SetPlace(sportsmanDto.Place);
                    team.Add(sportsman);
                }
            }

            return (T)(object)team;
        }

    }
}
