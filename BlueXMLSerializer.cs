using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab_9
{
    public class BlueXMLSerializer: BlueSerializer
    {
        public override string Extension
        {
            get
            {
                return "xml";
            }
        }

        public class Blue_1_DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Votes { get; set; }
            public string? Surname { get; set; }

            public Blue_1_DTO() { }

            public Blue_1_DTO(Blue_1.Response response)
            {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;
                if (response is Blue_1.HumanResponse r)
                    Surname = r.Surname;
            }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            Blue_1_DTO dto = new Blue_1_DTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_1_DTO));

            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, dto);
                File.WriteAllText(FilePath, stringWriter.ToString());
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            Blue_1_DTO dto;
            var serializer = new XmlSerializer(typeof(Blue_1_DTO));
            Blue_1.Response deserialized;

            string xmlContent = string.Join(Environment.NewLine, File.ReadAllLines(FilePath, Encoding.UTF8));

            using (var stringReader = new StringReader(xmlContent))
            {
                dto = (Blue_1_DTO)serializer.Deserialize(stringReader);
            }

            deserialized = dto.Surname != null
                ? new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes)
                : new Blue_1.Response(dto.Name, dto.Votes);

            return deserialized;
        }

        private static int[][] ConvertToJArray(int[,] m)
        {
            if (m == null || m.GetLength(0) == 0 || m.GetLength(1) == 0) return null;

            int[][] JArray = new int[m.GetLength(0)][];
            for (int i = 0; i < m.GetLength(0); i++)
            {
                JArray[i] = new int[m.GetLength(1)];
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    JArray[i][j] = m[i, j];
                }
            }
            return JArray;
        }

        private static int[,] ConvertToMatrix(int[][] arr)
        {
            int rows = arr.Length;
            int cols = arr[0].Length;
            int[,] m = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    m[i, j] = arr[i][j];
                }
            }

            return m;
        }


        public class WaterJump_Blue_2_DTO
        {

            public string Type { get; set; }

            public string Name { get; set; }

            public int Bank { get; set; }

            public Participant_BLue_2_DTO[] Participants { get; set; }

            public WaterJump_Blue_2_DTO() { }

            public WaterJump_Blue_2_DTO(Blue_2.WaterJump wj)
            {
                Type = wj.GetType().Name;
                Name = wj.Name;
                Bank = wj.Bank;
                Participants = wj.Participants.Select(p => new Participant_BLue_2_DTO(p)).ToArray();
            }
        }

        public class Participant_BLue_2_DTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }

            public int[][] Marks { get; set; }

            public Participant_BLue_2_DTO() { }

            public Participant_BLue_2_DTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = ConvertToJArray(participant.Marks);
            }

        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) { return; }
            SelectFile(fileName);
            WaterJump_Blue_2_DTO dto = new WaterJump_Blue_2_DTO(participant);
            var serializer = new XmlSerializer(typeof(WaterJump_Blue_2_DTO));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, dto);
                File.WriteAllText(FilePath, stringWriter.ToString());
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return null; }
            SelectFile(fileName);

            WaterJump_Blue_2_DTO obj;
            var serializer = new XmlSerializer(typeof(WaterJump_Blue_2_DTO));

            string xmlContent = File.ReadAllText(FilePath);

            Blue_2.WaterJump waterJump = null;
            using (var reader = new StreamReader(FilePath))
            {
                obj = (WaterJump_Blue_2_DTO)serializer.Deserialize(reader);
            }
            string type = obj.Type;

            if (type == nameof(Blue_2.WaterJump3m))
            {
                waterJump = new Blue_2.WaterJump3m((string)obj.Name, (int)obj.Bank);
            }
            else if (type == nameof(Blue_2.WaterJump5m))
            {
                waterJump = new Blue_2.WaterJump5m((string)obj.Name, (int)obj.Bank);
            }

            if (waterJump != null && obj.Participants != null)
            {
                foreach (var participantObj in obj.Participants)
                {
                    var part = new Blue_2.Participant((string)participantObj.Name, (string)participantObj.Surname);
                    int[][] marks = participantObj.Marks;
                    foreach (var m in marks)
                        part.Jump(m);
                    waterJump.Add(part);
                }

                return waterJump;
            }

            return waterJump;
        }

        public class Participant_Blue_3_DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }

            public Participant_Blue_3_DTO() { }

            public Participant_Blue_3_DTO(Blue_3.Participant p)
            {
                Type = p.GetType().Name;
                Name = p.Name;
                Surname = p.Surname;
                Penalties = p.Penalties;
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) { return; }
            SelectFile(fileName);
            Participant_Blue_3_DTO dto = new Participant_Blue_3_DTO(student);
            var serializer = new XmlSerializer(typeof(Participant_Blue_3_DTO));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, dto);
                File.WriteAllText(FilePath, stringWriter.ToString());
            }
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return null; }
            SelectFile(fileName);

            Participant_Blue_3_DTO obj;
            var serializer = new XmlSerializer(typeof(Participant_Blue_3_DTO));

            string xmlContent = File.ReadAllText(FilePath);

            Blue_3.Participant participant = null;
            using (var reader = new StreamReader(FilePath))
            {
                obj = (Participant_Blue_3_DTO)serializer.Deserialize(reader);
            }
            string type = obj.Type;
            if (type == "BasketballPlayer")
            {
                participant = new Blue_3.BasketballPlayer((string)obj.Name, (string)obj.Surname);
            }
            else if (type == "HockeyPlayer")
            {
                participant = new Blue_3.HockeyPlayer((string)obj.Name, (string)obj.Surname);
            }
            else
            {
                participant = new Blue_3.Participant((string)obj.Name, (string)obj.Surname);
            }

            foreach (var p in obj.Penalties)
            {
                participant.PlayMatch((int)p);
            }
            return (T)(object)participant;
        }


        public class Team_Blue_4_DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }
            public Team_Blue_4_DTO() { }
            public Team_Blue_4_DTO(Blue_4.Team t)
            {
                Type = t.GetType().Name;
                Name = t.Name;
                Scores = t.Scores;
            }
        }

        public class Group_Blue_4_DTO
        {
            public string Name { get; set; }
            public Team_Blue_4_DTO[] ManTeams { get; set; }
            public Team_Blue_4_DTO[] WomanTeams { get; set; }
            public Group_Blue_4_DTO() { }
            public Group_Blue_4_DTO(Blue_4.Group g)
            {
                Name = g.Name;
                ManTeams = g.ManTeams.Select(t => t == null ? null : new Team_Blue_4_DTO(t)).ToArray();
                WomanTeams = g.WomanTeams.Select(t => t == null ? null : new Team_Blue_4_DTO(t)).ToArray();
            }
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) { return; }
            SelectFile(fileName);
            Group_Blue_4_DTO dto = new Group_Blue_4_DTO(participant);
            var serializer = new XmlSerializer(typeof(Group_Blue_4_DTO));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, dto);
                File.WriteAllText(FilePath, stringWriter.ToString());
            }
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return null; }
            SelectFile(fileName);

            Group_Blue_4_DTO obj;
            var serializer = new XmlSerializer(typeof(Group_Blue_4_DTO));

            string xmlContent = File.ReadAllText(FilePath);
            using (var reader = new StreamReader(FilePath))
            {
                obj = (Group_Blue_4_DTO)serializer.Deserialize(reader);
            }

            Blue_4.Group group = new Blue_4.Group((string)obj.Name);

            if (obj.WomanTeams != null)
            {
                foreach (var w in obj.WomanTeams)
                {
                    if (w != null)
                    {
                        var Wteam = new Blue_4.WomanTeam((string)w.Name);
                        int[] scores = w.Scores;
                        if (scores != null)
                        {
                            foreach (var s in scores)
                            {
                                Wteam.PlayMatch(s);
                            }
                        }
                        group.Add(Wteam);
                    }
                }
            }

            if (obj.ManTeams != null)
            {
                foreach (var m in obj.ManTeams)
                {
                    if (m != null)
                    {
                        var Mteam = new Blue_4.ManTeam((string)m.Name);
                        int[] scores = m.Scores;
                        if (scores != null)
                        {
                            foreach (var s in scores)
                            {
                                Mteam.PlayMatch(s);
                            }
                        }
                        group.Add(Mteam);
                    }
                }
            }
            return group;
        }

        public class Sportsman_Blue_5_DTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }
            public Sportsman_Blue_5_DTO() { }
            public Sportsman_Blue_5_DTO(Blue_5.Sportsman s)
            {
                Name = s.Name;
                Surname = s.Surname;
                Place = s.Place;
            }
        }

        public class Team_Blue_5_DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Sportsman_Blue_5_DTO[] Sportsmen { get; set; }

            public Team_Blue_5_DTO() { }
            public Team_Blue_5_DTO(Blue_5.Team t)
            {
                Type = t.GetType().Name;
                Name = t.Name;
                Sportsmen = t.Sportsmen.Select(s => s != null ? new Sportsman_Blue_5_DTO(s) : null).ToArray();
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) { return; }
            SelectFile(fileName);
            Team_Blue_5_DTO dto = new Team_Blue_5_DTO(group);
            var serializer = new XmlSerializer(typeof(Team_Blue_5_DTO));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, dto);
                File.WriteAllText(FilePath, stringWriter.ToString());
            }
        
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return null; }
            SelectFile(fileName);

            Team_Blue_5_DTO obj;
            var serializer = new XmlSerializer(typeof(Team_Blue_5_DTO));

            string xmlContent = File.ReadAllText(FilePath);

            using (var reader = new StreamReader(FilePath))
            {
                obj = (Team_Blue_5_DTO)serializer.Deserialize(reader);
            }
            Blue_5.Team team;

            if ((string)obj.Type == "ManTeam")
            {
                team = new Blue_5.ManTeam((string)obj.Name);
            }
            else if ((string)obj.Type == "WomanTeam")
            {
                team = new Blue_5.WomanTeam((string)obj.Name);
            }
            else
            {
                return null;
            }

            if (obj.Sportsmen != null)
            {
                foreach (var s in obj.Sportsmen)
                {
                    if (s != null)
                    {
                        string name = (string)s.Name;
                        string surname = (string)s.Surname;
                        int place = (int)(s.Place);

                        var sportsman = new Blue_5.Sportsman(name, surname);
                        sportsman.SetPlace(place);
                        team.Add(sportsman);

                    }
                }
            }
            return (T)team;
        }
    }
}
