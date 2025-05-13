using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Lab_7;
using static Lab_7.Blue_1;
using static Lab_9.BlueXMLSerializer;
using static Lab_7.Blue_4;
using static Lab_7.Blue_5;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";
        public class Blue_1ResponseDTO
        {
            public string type { get; set; }
            public string name { get; set; }
            public int votes { get; set; }
            public string Surname { get; set; }
            public Blue_1ResponseDTO() { }
            public Blue_1ResponseDTO(Blue_1.Response response)
            {
                type = response.GetType().Name;
                name = response.Name;
                votes = response.Votes;
                if (response is Blue_1.HumanResponse human)
                    Surname = human.Surname;
            }
        }
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
                return;
            SelectFile(fileName);
            Blue_1ResponseDTO dto = new Blue_1ResponseDTO(participant);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_1ResponseDTO));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fs, dto);
            }
        }
        public class Blue_2ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Marks1 { get; set; }
            public int[] Marks2 { get; set; }

            public Blue_2ParticipantDTO() { }

            public Blue_2ParticipantDTO(Blue_2.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                if (p.Marks != null && p.Marks.GetLength(0) == 2 && p.Marks.GetLength(1) == 5)
                {
                    Marks1 = new int[5];
                    Marks2 = new int[5];
                    for (int i = 0; i < 5; i++)
                    {
                        Marks1[i] = p.Marks[0, i];
                        Marks2[i] = p.Marks[1, i];
                    }
                }
            }
        }
        public class Blue_2WaterJumpDTO
        {
            public string Type { get; set; } 
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2ParticipantDTO[] Participants { get; set; }

            public Blue_2WaterJumpDTO() { }

            public Blue_2WaterJumpDTO(Blue_2.WaterJump jump)
            {
                Type = jump.GetType().Name;
                Name = jump.Name;
                Bank = jump.Bank;
                if (jump.Participants != null)
                {
                    Participants = new Blue_2ParticipantDTO[jump.Participants.Length];
                    for (int i = 0; i < jump.Participants.Length; i++)
                        Participants[i] = new Blue_2ParticipantDTO(jump.Participants[i]);
                }
            }
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
                return;
            Blue_2WaterJumpDTO dto = new Blue_2WaterJumpDTO(participant);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_2WaterJumpDTO));
            using (var fs = new FileStream(fileName, FileMode.Create))
                serializer.Serialize(fs, dto);
        }
        public class Blue_3ParticipantDTO
        {
            public string Type { get; set; }       
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }

            public Blue_3ParticipantDTO() { }

            public Blue_3ParticipantDTO(Blue_3.Participant p)
            {
                Type = p.GetType().Name;
                Name = p.Name;
                Surname = p.Surname;
                Penalties = p.Penalties;
            }
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : class
        {
            if (student == null || string.IsNullOrEmpty(fileName))
                return;
            Blue_3ParticipantDTO dto = new Blue_3ParticipantDTO(student);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_3ParticipantDTO));
            using (var fs = new FileStream(fileName, FileMode.Create))
                serializer.Serialize(fs, dto);
        }
        public class Blue_4TeamDTO
        {
            public string Type { get; set; }     
            public string Name { get; set; }
            public int[] Scores { get; set; }

            public Blue_4TeamDTO() { }

            public Blue_4TeamDTO(Lab_7.Blue_4.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Scores = team.Scores;
            }
        }
        public class Blue_4GroupDTO
        {
            public string Name { get; set; }
            public Blue_4TeamDTO[] ManTeams { get; set; }
            public Blue_4TeamDTO[] WomanTeams { get; set; }

            public Blue_4GroupDTO() { }

            public Blue_4GroupDTO(Blue_4.Group group)
            {
                Name = group.Name;

                
                int manCount = 0;
                for (int i = 0; i < group.ManTeams.Length && group.ManTeams[i] != null; i++)
                    manCount++;

                ManTeams = new Blue_4TeamDTO[manCount];
                for (int i = 0; i < manCount; i++)
                     ManTeams[i] = new Blue_4TeamDTO(group.ManTeams[i]);

                int womanCount = 0;
                for (int i = 0; i < group.WomanTeams.Length && group.WomanTeams[i] != null; i++)
                    womanCount++;

                 WomanTeams = new Blue_4TeamDTO[womanCount];
                for (int i = 0; i < womanCount; i++)
                    WomanTeams[i] = new Blue_4TeamDTO(group.WomanTeams[i]);
            }
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
                return;
            Blue_4GroupDTO dto = new Blue_4GroupDTO(participant);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_4GroupDTO));
            using (var fs = new FileStream(fileName, FileMode.Create))
                serializer.Serialize(fs, dto);
        }

        public class Blue_5SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }

            public Blue_5SportsmanDTO() { }

            public Blue_5SportsmanDTO(Blue_5.Sportsman s)
            {
                Name = s.Name;
                Surname = s.Surname;
                Place = s.Place;
            }
        }
        public class Blue_5TeamDTO
        {
            public string Type { get; set; } 
            public string Name { get; set; }
            public Blue_5SportsmanDTO[] Sportsmen { get; set; }

            public Blue_5TeamDTO() { }

            public Blue_5TeamDTO(Blue_5.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;

                int count = 0;
                var sportsmen = team.Sportsmen;
                for (int i = 0; i < sportsmen.Length; i++)
                    if (sportsmen[i] != null) count++;

                Sportsmen = new Blue_5SportsmanDTO[count];
                int k = 0;
                for (int i = 0; i < sportsmen.Length; i++)
                    if (sportsmen[i] != null)
                    {
                        Sportsmen[k] = new Blue_5SportsmanDTO(sportsmen[i]);
                        k++;
                    }
            }
        }
        public class Blue_5TeamsDTO
        {
            public Blue_5TeamDTO[] ManTeams { get; set; }
            public Blue_5TeamDTO[] WomanTeams { get; set; }

            public Blue_5TeamsDTO() { }

            public Blue_5TeamsDTO(Blue_5.Team[] manTeams, Blue_5.Team[] womanTeams)
            {
                int manCount = 0;
                for (int i = 0; i < manTeams.Length; i++)
                    if (manTeams[i] != null) manCount++;
                ManTeams = new Blue_5TeamDTO[manCount];
                int k = 0;
                for (int i = 0; i < manTeams.Length; i++)
                    if (manTeams[i] != null)
                    {
                        ManTeams[k] = new Blue_5TeamDTO(manTeams[i]);
                        k++;
                    }

                int womanCount = 0;
                for (int i = 0; i < womanTeams.Length; i++)
                    if (womanTeams[i] != null) womanCount++;
                WomanTeams = new Blue_5TeamDTO[womanCount];
                k = 0;
                for (int i = 0; i < womanTeams.Length; i++)
                    if (womanTeams[i] != null)
                    {
                        WomanTeams[k] = new Blue_5TeamDTO(womanTeams[i]);
                        k++;
                    }
            }
        }




        public override void SerializeBlue5Team<T>(T group, string fileName)// where T : class
        {
            var dto = new Blue_5TeamDTO(group);
            var serializer = new XmlSerializer(typeof(Blue_5TeamDTO));
            using (var fs = new FileStream(fileName, FileMode.Create))
                serializer.Serialize(fs, dto);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            XmlSerializer serializer = new XmlSerializer(typeof(Blue_1ResponseDTO));
            Blue_1ResponseDTO dto;

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                dto = (Blue_1ResponseDTO)serializer.Deserialize(fs);
            }

            if (dto == null)
                return null;

            
            if (dto.type == nameof(Blue_1.HumanResponse))
            {
                return new Blue_1.HumanResponse(dto.name, dto.Surname, dto.votes);
            }
            else
            {
                return new Blue_1.Response(dto.name, dto.votes);
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            var serializer = new XmlSerializer(typeof(Blue_2WaterJumpDTO));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var dto = (Blue_2WaterJumpDTO)serializer.Deserialize(fs);

                Blue_2.WaterJump jump;
                if (dto.Type == "WaterJump3m")
                    jump = new Blue_2.WaterJump3m(dto.Name, dto.Bank);
                else
                    jump = new Blue_2.WaterJump5m(dto.Name, dto.Bank);

                if (dto.Participants != null)
                {
                    for (int i = 0; i < dto.Participants.Length; i++)
                    {
                        var pDto = dto.Participants[i];
                        var p = new Blue_2.Participant(pDto.Name, pDto.Surname);
                        if (pDto.Marks1 != null)
                            p.Jump(pDto.Marks1);
                        if (pDto.Marks2 != null)
                            p.Jump(pDto.Marks2);
                        jump.Add(p);
                    }
                }
                return jump;
            }
        }

        public override T DeserializeBlue3Participant<T>(string fileName) //where T : class
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_3ParticipantDTO));
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                var dto = (Blue_3ParticipantDTO)serializer.Deserialize(fs);

                Blue_3.Participant participant;

                if (dto.Type == "BasketballPlayer")
                    participant = new Blue_3.BasketballPlayer(dto.Name, dto.Surname);
                else if (dto.Type == "HockeyPlayer")
                    participant = new Blue_3.HockeyPlayer(dto.Name, dto.Surname);
                else
                    participant = new Blue_3.Participant(dto.Name, dto.Surname);

                if (dto.Penalties != null)
                {
                    foreach (var time in dto.Penalties)
                        participant.PlayMatch(time);
                }

                return (T)participant;
            }
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            var serializer = new XmlSerializer(typeof(Blue_4GroupDTO));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var dto = (Blue_4GroupDTO)serializer.Deserialize(fs);
                var group = new Blue_4.Group(dto.Name);

                if (dto.ManTeams != null)
                {
                    for (int i = 0; i < dto.ManTeams.Length; i++)
                    {
                        var tDto = dto.ManTeams[i];
                        if (tDto == null) continue;
                        Blue_4.ManTeam team = new Blue_4.ManTeam(tDto.Name);
                        if (tDto.Scores != null)
                        {
                            for (int j = 0; j < tDto.Scores.Length; j++)
                                team.PlayMatch(tDto.Scores[j]);
                        }
                        group.Add(team);
                    }
                }

                if (dto.WomanTeams != null)
                {
                    for (int i = 0; i < dto.WomanTeams.Length; i++)
                    {
                        var tDto = dto.WomanTeams[i];
                        if (tDto == null) continue;
                        Blue_4.WomanTeam team = new Blue_4.WomanTeam(tDto.Name);
                        if (tDto.Scores != null)
                        {
                            for (int j = 0; j < tDto.Scores.Length; j++)
                                team.PlayMatch(tDto.Scores[j]);
                        }
                        group.Add(team);
                    }
                }

                return group;
            }
        }

        public override T DeserializeBlue5Team<T>(string fileName)// where T : class
        {
            if (string.IsNullOrEmpty(fileName))
                return default(T);

            var serializer = new XmlSerializer(typeof(Blue_5TeamDTO));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var dto = (Blue_5TeamDTO)serializer.Deserialize(fs);

                Blue_5.Team team = null;
              
                if (dto.Type == "ManTeam")
                    team = new Blue_5.ManTeam(dto.Name);
                else if (dto.Type == "WomanTeam")
                    team = new Blue_5.WomanTeam(dto.Name);

                if (team != null && dto.Sportsmen != null)
                {
                    for (int i = 0; i < dto.Sportsmen.Length; i++)
                    {
                        var sdto = dto.Sportsmen[i];
                        var sportsman = new Blue_5.Sportsman(sdto.Name, sdto.Surname);
                        sportsman.SetPlace(sdto.Place);
                        team.Add(sportsman);
                    }
                }

                return (T)team;
            }
        }
    }
}
