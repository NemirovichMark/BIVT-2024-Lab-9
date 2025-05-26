using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Lab_7;



namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";
        public class Blue1ResponseDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }

            public Blue1ResponseDTO() { }
            public Blue1ResponseDTO(Blue_1.Response response)
            {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;
                if (response is Blue_1.HumanResponse human) Surname = human.Surname;

            }
        }
        private void CheckArguments(object obj, string fileName)
        {
            if (obj == null || string.IsNullOrWhiteSpace(fileName)) return;
        }
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            CheckArguments(participant, fileName);
            SelectFile(fileName);
            Blue1ResponseDTO blue = new Blue1ResponseDTO(participant);
            XmlSerializer xml = new XmlSerializer(typeof(Blue1ResponseDTO));
            using (FileStream file = new FileStream(fileName, FileMode.Create))
            {
                xml.Serialize(file, blue);
            }
        }
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            XmlSerializer xml = new XmlSerializer(typeof(Blue1ResponseDTO));
            Blue1ResponseDTO blue;
            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                blue = (Blue1ResponseDTO)xml.Deserialize(file);
            }

            if (blue == null) return null;
            if (blue.Type == nameof(Blue_1.HumanResponse)) return new Blue_1.HumanResponse(blue.Name, blue.Surname, blue.Votes);
            else return new Blue_1.Response(blue.Name, blue.Votes);
        }

        public class Blue2_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }
            public Blue2_ParticipantDTO() { }
            public Blue2_ParticipantDTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = ToJagged(participant.Marks);
            }
            private int[][] ToJagged(int[,] array)
            {
                if (array == null) return null;
                int[][] newArray = new int[array.GetLength(0)][];
                for (int i = 0; i < newArray.Length; i++)
                {
                    newArray[i] = new int[array.GetLength(1)];
                    for (int j = 0; j < newArray[i].Length; j++)
                    {
                        newArray[i][j] = array[i, j];
                    }
                }
                return newArray;
            }
        }

        public class Blue2_WaterJumpDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue2_ParticipantDTO[] Participants { get; set; }
            public Blue2_WaterJumpDTO() { }
            public Blue2_WaterJumpDTO(Blue_2.WaterJump waterJump)
            {
                Type = waterJump.GetType().Name;
                Name = waterJump.Name;
                Bank = waterJump.Bank;
                if (waterJump.Participants != null)
                {
                    Participants = new Blue2_ParticipantDTO[waterJump.Participants.Length];
                    for (int i = 0; i < waterJump.Participants.Length; i++)
                    {
                        Participants[i] = new Blue2_ParticipantDTO(waterJump.Participants[i]);
                    }
                }
            }
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            CheckArguments(participant, fileName);
            SelectFile(fileName);
            Blue2_WaterJumpDTO blue = new Blue2_WaterJumpDTO(participant);
            XmlSerializer xml = new XmlSerializer(typeof(Blue2_WaterJumpDTO));
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                xml.Serialize(sw, blue);
            }
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            XmlSerializer xml = new XmlSerializer(typeof(Blue2_WaterJumpDTO));
            Blue2_WaterJumpDTO blue;
            using (StreamReader sr = new StreamReader(fileName))
            {
                blue = (Blue2_WaterJumpDTO)xml.Deserialize(sr);
            }
            Blue_2.WaterJump blue2 = new Blue_2.WaterJump3m(blue.Name, blue.Bank);
            if (blue.Type == "WaterJump5m")
            {
                blue2 = new Blue_2.WaterJump5m(blue.Name, blue.Bank);
            }
            foreach (Blue2_ParticipantDTO i in blue.Participants)
            {
                Blue_2.Participant blue1 = new Blue_2.Participant(i.Name, i.Surname);
                foreach (var j in i.Marks)
                {
                    blue1.Jump(j);
                }
                blue2.Add(blue1);
            }
            return blue2;
        }

        public class Blue3_ParticipantDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
            public Blue3_ParticipantDTO() { }
            public Blue3_ParticipantDTO(Blue_3.Participant participant)
            {
                Type = participant.GetType().Name;
                Name = participant.Name;
                Surname = participant.Surname;
                Penalties = participant.Penalties;
            }
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : class
        {
            CheckArguments(student, fileName);
            SelectFile(fileName);
            Blue3_ParticipantDTO blue = new Blue3_ParticipantDTO(student);
            XmlSerializer xml = new XmlSerializer(typeof(Blue3_ParticipantDTO));
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                xml.Serialize(sw, blue);
            }
        }
        public override T DeserializeBlue3Participant<T>(string fileName) //where T : class
        {
            if (string.IsNullOrEmpty(fileName)) return default(T);
            XmlSerializer xml = new XmlSerializer(typeof(Blue3_ParticipantDTO));
            Blue3_ParticipantDTO blue;
            using (StreamReader sr = new StreamReader(fileName))
            {
                blue = (Blue3_ParticipantDTO)xml.Deserialize(sr);
            }
            Blue_3.Participant blue2 = new Blue_3.Participant(blue.Name, blue.Surname);
            if (blue.Type == "BasketballPlayer")
            {
                blue2 = new Blue_3.BasketballPlayer(blue.Name, blue.Surname);
            }
            else if (blue.Type == "HockeyPlayer")
            {
                blue2 = new Blue_3.HockeyPlayer(blue.Name, blue.Surname);
            }
            foreach (var i in blue.Penalties)
            {
                blue2.PlayMatch(i);
            }
            return (T)blue2;
        }
        public class Blue4_TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }
            public Blue4_TeamDTO() { }
            public Blue4_TeamDTO(Blue_4.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Scores = team.Scores;
            }
        }
        public class Blue4_GroupDTO
        {
            public string Name { get; set; }
            public Blue4_TeamDTO[] ManTeams { get; set; }
            public Blue4_TeamDTO[] WomanTeams { get; set; }
            public Blue4_GroupDTO() { }
            public Blue4_GroupDTO(Blue_4.Group group)
            {
                Name=group.Name;
                if (group.ManTeams != null)
                {
                    ManTeams=new Blue4_TeamDTO[group.ManTeams.Length];
                    for (int i = 0; i < group.ManTeams.Length; i++)
                    {
                        ManTeams[i] = group.ManTeams[i] == null ? null : new Blue4_TeamDTO(group.ManTeams[i]);
                    }
                }
                if (group.WomanTeams != null)
                {
                    WomanTeams = new Blue4_TeamDTO[group.WomanTeams.Length];
                    for (int i = 0; i < group.WomanTeams.Length; i++)
                    {
                        WomanTeams[i] = group.WomanTeams[i] == null ? null : new Blue4_TeamDTO(group.WomanTeams[i]);
                    }
                }
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            CheckArguments(participant, fileName);
            SelectFile(fileName);
            Blue4_GroupDTO blue=new Blue4_GroupDTO(participant);
            XmlSerializer xml=new XmlSerializer(typeof(Blue4_GroupDTO));
            using(StreamWriter sw=new StreamWriter(fileName))
            {
                xml.Serialize(sw, blue);
            }
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            XmlSerializer xml = new XmlSerializer(typeof(Blue4_GroupDTO));
            Blue4_GroupDTO blue;
            using(StreamReader sr=new StreamReader(fileName))
            {
                blue=(Blue4_GroupDTO)xml.Deserialize(sr);
            }
            Blue_4.Group ans = new Blue_4.Group(blue.Name);
            if (blue.ManTeams != null)
            {
                foreach (var blue1 in blue.ManTeams)
                {
                    if (blue1==null) continue;
                    Blue_4.Team team=new Blue_4.ManTeam(blue1.Name);
                    if (blue1.Scores != null)
                    {
                        foreach(var score in blue1.Scores)
                        {
                            team.PlayMatch(score);
                        }
                    }
                    ans.Add(team);
                }
            }
            if (blue.WomanTeams != null)
            {
                foreach (var blue1 in blue.WomanTeams)
                {
                    if (blue1 == null) continue;
                    Blue_4.Team team = new Blue_4.WomanTeam(blue1.Name);
                    if (blue1.Scores != null)
                    {
                        foreach (var score in blue1.Scores)
                        {
                            team.PlayMatch(score);
                        }
                    }
                    ans.Add(team);
                }
            }
            return ans;
        }
        public class Blue5_SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }
            public Blue5_SportsmanDTO() { }
            public Blue5_SportsmanDTO(Blue_5.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }
        }
        public class Blue5_TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Blue5_SportsmanDTO[] Sportsman { get; set; }
            public Blue5_TeamDTO() { }
            public Blue5_TeamDTO(Blue_5.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                if (team.Sportsmen != null)
                {
                    Sportsman = new Blue5_SportsmanDTO[team.Sportsmen.Length];
                    for (int i = 0; i < team.Sportsmen.Length; i++)
                    {
                        Sportsman[i] = team.Sportsmen[i] == null ? null : new Blue5_SportsmanDTO(team.Sportsmen[i]);
                    }
                }
            }
        }
        public override void SerializeBlue5Team<T>(T group, string fileName)// where T : class
        {
            CheckArguments(group,fileName);
            SelectFile(fileName);
            Blue5_TeamDTO blue=new Blue5_TeamDTO(group);
            XmlSerializer xml=new XmlSerializer(typeof(Blue5_TeamDTO));
            using(StreamWriter sw=new StreamWriter(fileName))
            {
                xml.Serialize(sw, blue);
            }
        }
        public override T DeserializeBlue5Team<T>(string fileName)// where T : class
        {
            if (string.IsNullOrEmpty(fileName)) return default(T);

            XmlSerializer xml = new XmlSerializer(typeof(Blue5_TeamDTO));
            Blue5_TeamDTO blue1;
            using (StreamReader sr = new StreamReader(fileName))
            {
                blue1 = (Blue5_TeamDTO)xml.Deserialize(sr);
            }

            Blue_5.Team ans = new Blue_5.WomanTeam(blue1.Name);
            if (blue1.Type == "ManTeam")
            {
                ans = new Blue_5.ManTeam(blue1.Name);
            }

            if (blue1.Sportsman != null) 
            {
                foreach (var blue2 in blue1.Sportsman)
                {
                    if (blue2 == null) continue;

                    Blue_5.Sportsman sportsman = new Blue_5.Sportsman(blue2.Name, blue2.Surname); 
                    sportsman.SetPlace(blue2.Place);
                    ans.Add(sportsman);
                }
            }

            return (T)ans;
        }
    }
    
}

