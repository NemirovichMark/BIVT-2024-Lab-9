using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using Lab_7;
using System.Reflection;
using System.Linq;
using System.Xml;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

        [XmlInclude(typeof(HumanResponseDTO))]
        public class ResponseDTO
        {
            public string Type { get; set; } = "Response";
            public string Name { get; set; } = string.Empty;
            public int Votes { get; set; }

            public ResponseDTO() { }

            public ResponseDTO(Blue_1.Response response)
            {
                Name = response.Name;
                Votes = response.Votes;
            }
        }

        public class HumanResponseDTO : ResponseDTO
        {
            public string Surname { get; set; } = string.Empty;

            public HumanResponseDTO()
            {
                Type = "HumanResponse";
            }

            public HumanResponseDTO(Blue_1.HumanResponse response) : base(response)
            {
                Type = "HumanResponse";
                Surname = response.Surname;
            }
        }

        public class ParticipantDTO
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public List<int[]> Marks { get; set; } = new List<int[]>();

            public ParticipantDTO() { }

            public ParticipantDTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                if (participant.Marks != null)
                {
                    for (int i = 0; i < participant.Marks.GetLength(0); i++)
                    {
                        int[] jumpMarks = new int[participant.Marks.GetLength(1)];
                        for (int j = 0; j < participant.Marks.GetLength(1); j++)
                        {
                            jumpMarks[j] = participant.Marks[i, j];
                        }
                        Marks.Add(jumpMarks);
                    }
                }
            }
        }

        public class WaterJumpDTO
        {
            public string Name { get; set; } = string.Empty;
            public int Bank { get; set; }
            public List<ParticipantDTO> Participants { get; set; } = new List<ParticipantDTO>();
            public string Type { get; set; } = "WaterJump";

            public WaterJumpDTO() { }

            public WaterJumpDTO(Blue_2.WaterJump waterJump)
            {
                Name = waterJump.Name;
                Bank = waterJump.Bank;
                if (waterJump.Participants != null)
                {
                    foreach (var participant in waterJump.Participants)
                    {
                        Participants.Add(new ParticipantDTO(participant));
                    }
                }
                Type = waterJump is Blue_2.WaterJump3m ? "WaterJump3m" :
                       waterJump is Blue_2.WaterJump5m ? "WaterJump5m" : "WaterJump";
            }
        }

        public class Participant3DTO
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public int[] Penalties { get; set; } = Array.Empty<int>();
            public string Type { get; set; } = "Participant";

            public Participant3DTO() { }

            public Participant3DTO(Blue_3.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Penalties = participant.Penalties?.ToArray() ?? Array.Empty<int>();
                Type = participant.GetType().Name;
            }
        }

        [XmlInclude(typeof(TeamDTO))]
        public class GroupDTO
        {
            public string Name { get; set; } = string.Empty;
            public List<TeamDTO> ManTeams { get; set; } = new List<TeamDTO>();
            public List<TeamDTO> WomanTeams { get; set; } = new List<TeamDTO>();

            public GroupDTO() { }

            public GroupDTO(Blue_4.Group group)
            {
                Name = group.Name;
                if (group.ManTeams != null) ManTeams = group.ManTeams.Where(t => t != null).Select(t => new TeamDTO(t)).ToList();
                if (group.WomanTeams != null) WomanTeams = group.WomanTeams.Where(t => t != null).Select(t => new TeamDTO(t)).ToList();
            }
        }

        public class TeamDTO
        {
            public string Name { get; set; } = string.Empty;
            public int[] Scores { get; set; } = Array.Empty<int>();
            public string Type { get; set; } = "Team";

            public TeamDTO() { }

            public TeamDTO(Blue_4.Team team)
            {
                Name = team.Name;
                Scores = team.Scores?.ToArray() ?? Array.Empty<int>();
                Type = team.GetType().Name;
            }
        }

        public class SportsmanDTO
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public int Place { get; set; }

            public SportsmanDTO() { }

            public SportsmanDTO(Blue_5.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }
        }

        [XmlInclude(typeof(SportsmanDTO))]
        public class Team5DTO
        {
            public string Name { get; set; } = string.Empty;
            public List<SportsmanDTO> Sportsmen { get; set; } = new List<SportsmanDTO>();
            public string Type { get; set; } = "Team";

            public Team5DTO() { }

            public Team5DTO(Blue_5.Team team)
            {
                Name = team.Name;
                Type = team.GetType().Name;
                if (team.Sportsmen != null) Sportsmen = team.Sportsmen.Where(s => s != null).Select(s => new SportsmanDTO(s)).ToList();
            }
        }


        private string GetXmlRootElementName(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = XmlReader.Create(fs))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            return reader.Name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML Helper Error: Could not get root element name from {filePath}. Error: {ex.Message}");
            }
            return null;
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            object dto;
            Type dtoType;
            if (participant is Blue_1.HumanResponse humanResponse)
            {
                dto = new HumanResponseDTO(humanResponse);
                dtoType = typeof(HumanResponseDTO);
            }
            else
            {
                dto = new ResponseDTO(participant);
                dtoType = typeof(ResponseDTO);
            }
            var serializer = new XmlSerializer(dtoType);
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName); 
            
            Blue_1.Response domainResponse = null;
            int votes = 0;

            try
            {
                string rootElementName = GetXmlRootElementName(this.FilePath);

                if (string.IsNullOrEmpty(rootElementName))
                {
                    Console.WriteLine($"XML Deserialization for Blue1: Could not determine root element name from file: {this.FilePath}");
                    return null;
                }

                object deserializedDto = null;

                
                using (var fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) 
                using (var streamReader = new StreamReader(fs)) 
                {
                    if (rootElementName == "HumanResponseDTO")
                    {
                        var serializer = new XmlSerializer(typeof(HumanResponseDTO));
                        deserializedDto = serializer.Deserialize(streamReader);
                    }
                    else if (rootElementName == "ResponseDTO")
                    {
                        var serializer = new XmlSerializer(typeof(ResponseDTO));
                        deserializedDto = serializer.Deserialize(streamReader);
                    }
                    else
                    {
                        Console.WriteLine($"XML Deserialization for Blue1: Unexpected root element '{rootElementName}' in file: {this.FilePath}");
                        return null;
                    }
                }

                if (deserializedDto == null)
                {
                    Console.WriteLine("XML Deserialization for Blue1: DTO is null after specific deserialization attempt based on root element.");
                    return null;
                }

                
                if (deserializedDto is HumanResponseDTO humanDto)
                {
                    domainResponse = new Blue_1.HumanResponse(humanDto.Name, humanDto.Surname);
                    votes = humanDto.Votes;
                }
                else if (deserializedDto is ResponseDTO responseDto) 
                {
                    domainResponse = new Blue_1.Response(responseDto.Name);
                    votes = responseDto.Votes;
                }
                else
                {
                     Console.WriteLine($"XML Deserialization for Blue1: DTO is of unexpected type '{deserializedDto.GetType().FullName}' after deserialization.");
                     return null;
                }
                
                
                if (domainResponse != null)
                {
                    var field = typeof(Blue_1.Response).GetField("_votes", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null) 
                    {
                        field.SetValue(domainResponse, votes);
                    }
                    else 
                    {
                         Console.WriteLine("XML Deserialization for Blue1: _votes field not found via reflection.");
                    }
                }
                else
                {
                     Console.WriteLine("XML Deserialization for Blue1: domainResponse object is null after DTO type check and instantiation.");
                }

                return domainResponse;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"XML Deserialization Error for Blue1: {ex.Message} StackTrace: {ex.StackTrace}");
                return null; 
            }
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(WaterJumpDTO));
            var dto = new WaterJumpDTO(participant);
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            try
            {
                var serializer = new XmlSerializer(typeof(WaterJumpDTO));
                using var reader = new StreamReader(FilePath);
                var dto = (WaterJumpDTO)serializer.Deserialize(reader);
                if (dto == null) return null;

                Blue_2.WaterJump waterJump = dto.Type switch
                {
                    "WaterJump3m" => new Blue_2.WaterJump3m(dto.Name, dto.Bank),
                    "WaterJump5m" => new Blue_2.WaterJump5m(dto.Name, dto.Bank),
                    _ => null
                };
                if (waterJump == null) return null;

                foreach (var pDto in dto.Participants)
                {
                    if (pDto == null) continue;
                    var newParticipant = new Blue_2.Participant(pDto.Name, pDto.Surname);
                    if (pDto.Marks != null)
                    {
                        foreach (var jumpMarksArray in pDto.Marks)
                        {
                            if (jumpMarksArray != null)
                                newParticipant.Jump(jumpMarksArray);
                        }
                    }
                    waterJump.Add(newParticipant);
                }
                return waterJump;
            }
            catch { return null; }
        }

        public override void SerializeBlue3Participant<T>(T participant, string fileName) where T : class
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Participant3DTO(participant as Blue_3.Participant);
            var serializer = new XmlSerializer(typeof(Participant3DTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            try
            {
                var serializer = new XmlSerializer(typeof(Participant3DTO));
                using var reader = new StreamReader(FilePath);
                var dto = (Participant3DTO)serializer.Deserialize(reader);
                if (dto == null) return null;

                Blue_3.Participant domainParticipant = dto.Type switch
                {
                    "BasketballPlayer" => new Blue_3.BasketballPlayer(dto.Name, dto.Surname),
                    "HockeyPlayer" => new Blue_3.HockeyPlayer(dto.Name, dto.Surname),
                    _ => new Blue_3.Participant(dto.Name, dto.Surname)
                };
                if (dto.Penalties != null)
                {
                    foreach(var penalty in dto.Penalties) domainParticipant.PlayMatch(penalty);
                }
                return domainParticipant as T;
            }
            catch { return null; }
        }

        public override void SerializeBlue4Group(Blue_4.Group group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new GroupDTO(group);
            var serializer = new XmlSerializer(typeof(GroupDTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
             if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            try
            {
                var serializer = new XmlSerializer(typeof(GroupDTO));
                using var reader = new StreamReader(FilePath);
                var dto = (GroupDTO)serializer.Deserialize(reader);
                if (dto == null || string.IsNullOrEmpty(dto.Name)) return null;

                var group = new Blue_4.Group(dto.Name);
                if (dto.ManTeams != null)
                {
                    foreach(var teamDto in dto.ManTeams)
                    {
                        if(teamDto == null) continue;
                        var manTeam = new Blue_4.ManTeam(teamDto.Name);
                        if(teamDto.Scores != null) foreach(var score in teamDto.Scores) manTeam.PlayMatch(score);
                        group.Add(manTeam);
                    }
                }
                if (dto.WomanTeams != null)
                {
                    foreach(var teamDto in dto.WomanTeams)
                    {
                        if(teamDto == null) continue;
                        var womanTeam = new Blue_4.WomanTeam(teamDto.Name);
                        if(teamDto.Scores != null) foreach(var score in teamDto.Scores) womanTeam.PlayMatch(score);
                        group.Add(womanTeam);
                    }
                }
                return group;
            }
            catch { return null; }
        }

        public override void SerializeBlue5Team<T>(T team, string fileName) where T : class
        {
            if (team == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Team5DTO(team as Blue_5.Team);
            var serializer = new XmlSerializer(typeof(Team5DTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            try
            {
                var serializer = new XmlSerializer(typeof(Team5DTO));
                using var reader = new StreamReader(FilePath);
                var dto = (Team5DTO)serializer.Deserialize(reader);
                if (dto == null || string.IsNullOrEmpty(dto.Name)) return null;

                Blue_5.Team domainTeam = dto.Type switch 
                {
                    "ManTeam" => new Blue_5.ManTeam(dto.Name),
                    "WomanTeam" => new Blue_5.WomanTeam(dto.Name),
                    _ => null
                };
                if(domainTeam == null) return null;

                if (dto.Sportsmen != null)
                {
                    foreach(var spDto in dto.Sportsmen)
                    {
                        if(spDto == null) continue;
                        var sportsman = new Blue_5.Sportsman(spDto.Name, spDto.Surname);
                        sportsman.SetPlace(spDto.Place);
                        domainTeam.Add(sportsman);
                    }
                }
                return domainTeam as T;
            }
            catch { return null; }
        }
    }
}