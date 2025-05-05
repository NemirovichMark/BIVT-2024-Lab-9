using System.Xml.Serialization;
using System.IO;
using System.Linq;
using Lab_7;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

        public class Blue_1_ResponseDTO
        {
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }
        }

        public class Blue_2_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }
        }

        public class Blue_2_WaterJumpDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2_ParticipantDTO[] Participants { get; set; }
        }

        public class Blue_3_ParticipantDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
        }


        public class Blue_4_TeamDTO
        {
            public string Name { get; set; }
            public int[] Scores { get; set; }
        }

        public class Blue_4_GroupDTO
        {
            public string Name { get; set; }
            public Blue_4_TeamDTO[] ManTeams { get; set; }
            public Blue_4_TeamDTO[] WomanTeams { get; set; }
        }

        public class Blue_5_SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }
        }

        public class Blue_5_TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Blue_5_SportsmanDTO[] Sportsmen { get; set; }
        }

        private void SerializeObject<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath) || obj == null)
                return;

            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, obj);
            }
        }

        private T DeserializeObject<T>(string fileName) where T : class
        {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
                return null;

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(FilePath))
            {
                return serializer.Deserialize(reader) as T;
            }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName) {
            if (participant == null) return;
            
            var dto = new Blue_1_ResponseDTO {
                Name = participant.Name,
                Votes = participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname
            };

            SerializeObject(dto, fileName);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            var dto = DeserializeObject<Blue_1_ResponseDTO>(fileName);
            if (dto == null) return null;

            return dto.Surname != null 
                ? new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes)
                : new Blue_1.Response(dto.Name, dto.Votes);
        }
        
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            var dto = new Blue_2_WaterJumpDTO
            {
                Type = participant.GetType().AssemblyQualifiedName,
                Name = participant.Name,
                Bank = participant.Bank,
                Participants = participant.Participants.Select(p => 
                {
                    var marks = new int[p.Marks.GetLength(0)][];
                    for (int i = 0; i < p.Marks.GetLength(0); i++)
                    {
                        marks[i] = new int[p.Marks.GetLength(1)];
                        for (int j = 0; j < p.Marks.GetLength(1); j++)
                        {
                            marks[i][j] = p.Marks[i, j];
                        }
                    }
                    
                    return new Blue_2_ParticipantDTO
                    {
                        Name = p.Name,
                        Surname = p.Surname,
                        Marks = marks
                    };
                }).ToArray()
            };

            SerializeObject(dto, fileName);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            var dto = DeserializeObject<Blue_2_WaterJumpDTO>(fileName);
            if (dto == null) return null;

            Blue_2.WaterJump waterJump = null;
            if (Type.GetType(dto.Type) == typeof(Blue_2.WaterJump3m))
            {
                waterJump = new Blue_2.WaterJump3m(dto.Name, dto.Bank);
            }
            else if (Type.GetType(dto.Type) == typeof(Blue_2.WaterJump5m))
            {
                waterJump = new Blue_2.WaterJump5m(dto.Name, dto.Bank);
            }
            else
            {
                return null;
            }

            foreach (var pDto in dto.Participants)
            {
                var participant = new Blue_2.Participant(pDto.Name, pDto.Surname);
                foreach (var marks in pDto.Marks)
                {
                    participant.Jump(marks);
                }
                waterJump.Add(participant);
            }

            return waterJump;
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student is Blue_3.Participant participant)
            {
                var dto = new Blue_3_ParticipantDTO
                {
                    Type = participant.GetType().AssemblyQualifiedName,
                    Name = participant.Name,
                    Surname = participant.Surname,
                    Penalties = participant.Penalties.ToArray()
                };

                SerializeObject(dto, fileName);
            }
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            var dto = DeserializeObject<Blue_3_ParticipantDTO>(fileName);
            if (dto == null) return null;

            var type = Type.GetType(dto.Type);
            if (type == null) return null;

            var participant = (Blue_3.Participant)Activator.CreateInstance(type, dto.Name, dto.Surname);

            foreach (var penalty in dto.Penalties)
            {
                participant.PlayMatch(penalty);
            }

            return (T)participant;
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null) return;

            var dto = new Blue_4_GroupDTO
            {
                Name = participant.Name,
                ManTeams = participant.ManTeams == null ? Array.Empty<Blue_4_TeamDTO>() : 
                    participant.ManTeams.Where(t => t != null).Select(t => new Blue_4_TeamDTO
                    {
                        Name = t.Name,
                        Scores = t.Scores?.ToArray() ?? Array.Empty<int>()
                    }).ToArray(),
                WomanTeams = participant.WomanTeams == null ? Array.Empty<Blue_4_TeamDTO>() : 
                    participant.WomanTeams.Where(t => t != null).Select(t => new Blue_4_TeamDTO
                    {
                        Name = t.Name,
                        Scores = t.Scores?.ToArray() ?? Array.Empty<int>()
                    }).ToArray()
            };

            SerializeObject(dto, fileName);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            var dto = DeserializeObject<Blue_4_GroupDTO>(fileName);
            if (dto == null) return null;

            var group = new Blue_4.Group(dto.Name);

            if (dto.ManTeams != null)
            {
                foreach (var teamDto in dto.ManTeams)
                {
                    if (teamDto == null) continue;
                    
                    var team = new Blue_4.ManTeam(teamDto.Name);
                    if (teamDto.Scores != null)
                    {
                        foreach (var score in teamDto.Scores)
                        {
                            team.PlayMatch(score);
                        }
                    }
                    group.Add(team);
                }
            }

            if (dto.WomanTeams != null)
            {
                foreach (var teamDto in dto.WomanTeams)
                {
                    if (teamDto == null) continue;
                    
                    var team = new Blue_4.WomanTeam(teamDto.Name);
                    if (teamDto.Scores != null)
                    {
                        foreach (var score in teamDto.Scores)
                        {
                            team.PlayMatch(score);
                        }
                    }
                    group.Add(team);
                }
            }

            return group;
        }

        public override void SerializeBlue5Team<T>(T team, string fileName)
        {
            if (team is not Blue_5.Team t) return;

            var dto = new Blue_5_TeamDTO
            {
                Type = team.GetType().AssemblyQualifiedName,
                Name = t.Name,
                Sportsmen = t.Sportsmen?.Where(s => s != null).Select(s => new Blue_5_SportsmanDTO
                {
                    Name = s.Name,
                    Surname = s.Surname,
                    Place = s.Place
                }).ToArray() ?? Array.Empty<Blue_5_SportsmanDTO>()
            };

            SerializeObject(dto, fileName);
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            var dto = DeserializeObject<Blue_5_TeamDTO>(fileName);
            if (dto == null) return null;

            var type = Type.GetType(dto.Type);
            if (type == null) return null;

            Blue_5.Team team = type switch
            {
                _ when type == typeof(Blue_5.ManTeam) => new Blue_5.ManTeam(dto.Name),
                _ when type == typeof(Blue_5.WomanTeam) => new Blue_5.WomanTeam(dto.Name),
                _ => null
            };

            if (team == null) return null;

            if (dto.Sportsmen != null)
            {
                foreach (var sDto in dto.Sportsmen)
                {
                    if (sDto == null) continue;
                    
                    var sportsman = new Blue_5.Sportsman(sDto.Name, sDto.Surname);
                    sportsman.SetPlace(sDto.Place);
                    team.Add(sportsman);
                }
            }

            return (T)(object)team;
        }
    }
}