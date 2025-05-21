using Lab_7;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

        public class Blue_1_ResponseDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }

            public Blue_1_ResponseDTO() { }
            public Blue_1_ResponseDTO(Blue_1.Response response)
            {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;
                if (response is Blue_1.HumanResponse hr)
                {
                    Surname = hr.Surname;
                }
            }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_1_ResponseDTO(participant);
            var serial = new XmlSerializer(typeof(Blue_1_ResponseDTO));

            using var pis = new StreamWriter(FilePath);
            serial.Serialize(pis, dto);
        }

        private static int[][] ConvertToJaggedArray(int[,] array)
        {
            if (array == null || array.Length == 0) return null;
            int[][] res = new int[array.GetLength(0)][];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new int[array.GetLength(1)];

                for (int j = 0; j < res[i].Length; j++)
                {
                    res[i][j] = array[i, j];
                }
            }
            return res;
        }

        public class Blue_2_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }

            public Blue_2_ParticipantDTO() { }
            public Blue_2_ParticipantDTO(Blue_2.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Marks = ConvertToJaggedArray(p.Marks);
            }
        }

        public class Blue_2_WaterJumpDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2_ParticipantDTO[] Participants { get; set; }

            public Blue_2_WaterJumpDTO() { }
            public Blue_2_WaterJumpDTO(Blue_2.WaterJump wj)
            {
                Type = wj.GetType().Name;
                Name = wj.Name;
                Bank = wj.Bank;
                Participants = wj.Participants.Select(p => new Blue_2_ParticipantDTO(p)).ToArray();
            }
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_2_WaterJumpDTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public class Blue_3_ParticipantDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }

            public Blue_3_ParticipantDTO() { }
            public Blue_3_ParticipantDTO(Blue_3.Participant p)
            {
                Type = p.GetType().Name;
                Name = p.Name;
                Surname = p.Surname;
                Penalties = p.Penalties;
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_3_ParticipantDTO(student);
            var serializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public class Blue_4_TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }

            public Blue_4_TeamDTO() { }
            public Blue_4_TeamDTO(Blue_4.Team t)
            {
                Type = t.GetType().Name;
                Name = t.Name;
                Scores = t.Scores;
            }
        }

        public class Blue_4_GroupDTO
        {
            public string Name { get; set; }
            public Blue_4_TeamDTO[] ManTeams { get; set; }
            public Blue_4_TeamDTO[] WomanTeams { get; set; }

            public Blue_4_GroupDTO() { }
            public Blue_4_GroupDTO(Blue_4.Group g)
            {
                Name = g.Name;
                ManTeams = g.ManTeams.Select(t => t == null ? null : new Blue_4_TeamDTO(t)).ToArray();
                WomanTeams = g.WomanTeams.Select(t => t == null ? null : new Blue_4_TeamDTO(t)).ToArray();
            }
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_4_GroupDTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public class Blue_5_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }

            public Blue_5_ParticipantDTO() { }
            public Blue_5_ParticipantDTO(Blue_5.Sportsman s)
            {
                Name = s.Name;
                Surname = s.Surname;
                Place = s.Place;
            }
        }

        public class Blue_5_TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Blue_5_ParticipantDTO[] Sporsman { get; set; }

            public Blue_5_TeamDTO() { }
            public Blue_5_TeamDTO(Blue_5.Team t)
            {
                Type = t.GetType().Name;
                Name = t.Name;
                Sporsman = t.Sportsmen.Select(p => p == null ? null : new Blue_5_ParticipantDTO(p)).ToArray();
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_5_TeamDTO(group);
            var serializer = new XmlSerializer(typeof(Blue_5_TeamDTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));
            var dto = (Blue_1_ResponseDTO)serializer.Deserialize(reader);

            Blue_1.Response response;
            if (dto.Surname != null)
            {
                response = new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes);
            }
            else
            {
                response = new Blue_1.Response(dto.Name, dto.Votes);
            }
            return response;
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));
            var dto = (Blue_2_WaterJumpDTO)serializer.Deserialize(reader);

            Blue_2.WaterJump waterJump;

            if (dto.Type == "WaterJump3m")
            {
                waterJump = new Blue_2.WaterJump3m(dto.Name, dto.Bank);
            }
            else
            {
                waterJump = new Blue_2.WaterJump5m(dto.Name, dto.Bank);
            }
            foreach (var p in dto.Participants)
            {
                var participant = new Blue_2.Participant(p.Name, p.Surname);
                for (int j = 0; j < 2; j++)
                    if (p.Marks[j].Length == 5) participant.Jump(p.Marks[j]);
                waterJump.Add(participant);
            }
            return waterJump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));
            var dto = (Blue_3_ParticipantDTO)serializer.Deserialize(reader);

            Blue_3.Participant p = dto.Type switch
            {
                "HockeyPlayer" => new Blue_3.HockeyPlayer(dto.Name, dto.Surname),
                "BasketballPlayer" => new Blue_3.BasketballPlayer(dto.Name, dto.Surname),
                _ => new Blue_3.Participant(dto.Name, dto.Surname)
            };

            foreach (var time in dto.Penalties) p.PlayMatch(time);
            return (T)p;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));
            var dto = (Blue_4_GroupDTO)serializer.Deserialize(reader);

            var group = new Blue_4.Group(dto.Name);
            foreach (var t in dto.ManTeams)
            {
                if (t == null) continue;
                var team = new Blue_4.ManTeam(t.Name);
                foreach (var score in t.Scores) team.PlayMatch(score);
                group.Add(team);
            }

            foreach (var t in dto.WomanTeams)
            {
                if (t == null) continue;
                var team = new Blue_4.WomanTeam(t.Name);
                foreach (var score in t.Scores) team.PlayMatch(score);
                group.Add(team);
            }
            return group;
        }
         //foreach (var t in dto.WomanTeams)
         //   {
         //       if (t == null) continue;
         //       var team = new Blue_4.WomanTeam(t.Name);
         //       foreach (var score in t.Scores) team.PlayMatch(score);
         //       group.Add(team);
         //   }
         //   return group;
        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Blue_5_TeamDTO));
            var dto = (Blue_5_TeamDTO)serializer.Deserialize(reader);

            Blue_5.Team team;

            if (dto.Type == "ManTeam")
            {
                team = new Blue_5.ManTeam(dto.Name);
            }
            else
            {
                team = new Blue_5.WomanTeam(dto.Name);
            }
            foreach (var s in dto.Sporsman)
            {
                if (s == null) continue;
                var sportsman = new Blue_5.Sportsman(s.Name, s.Surname);
                sportsman.SetPlace(s.Place);
                team.Add(sportsman);
            }

            return (T)team;
        }
    }
     //public string Type { get; set; }
     //   public string Name { get; set; }
     //   public string Surname { get; set; }
     //   public int[] Penalties { get; set; }
    }