using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Lab_7.Blue_2;
using static Lab_9.BlueXMLSerializer;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

        //blue1
        public class ResponseData
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }
            
            public ResponseData(Blue_1.Response response) //сериализация, преобразование
            {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;
                Surname = (response as Blue_1.HumanResponse)?.Surname;
                //if (response is Blue_1.HumanResponse resp)
                //{
                //    Surname = resp.Surname;
                //}
            }
            public ResponseData() { } //десериализация, конструткор
        }

        //blue2
        public class WaterJumpData
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public ParticipantData[] Participants { get; set; }
            public WaterJumpData() { }

            public WaterJumpData(Blue_2.WaterJump w)
            {
                Type = w.GetType().Name;
                Name = w.Name;
                Bank = w.Bank;
                Participants = w.Participants.Select(p => new ParticipantData(p)).ToArray();
            }
        }
        //blue2
        public class ParticipantData
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }
            public ParticipantData() { }
            public ParticipantData(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = ConvMarks(participant.Marks);
            }
        }

        private static int[][] ConvMarks(int[,] marks)
        {
            if (marks == null)
            {
                return null;
            }
            int row = marks.GetLength(0);
            int col = marks.GetLength(1);
            int[][] New = new int[row][];
            for (int i = 0; i < row; i++)
            {
                New[i] = new int[col];
                for (int j = 0; j < col; j++)
                {
                    New[i][j] = marks[i, j];
                }
            }
            return New;
        }

        //blue3
        public class PenaltiesData
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
            public PenaltiesData(Blue_3.Participant part)
            {
                Type = part.GetType().Name;
                Name = part.Name;
                Surname = part.Surname;
                Penalties = part.Penalties;
            }
            public PenaltiesData() { }
        }
        //blue4
        public class GroupData
        {
            public string Name { get; set; }
            public TeamData[] ManTeams { get; set; }
            public TeamData[] WomanTeams { get; set; }
            public GroupData() { }
            public GroupData(Blue_4.Group group)
            {
                Name = group.Name;
                ManTeams = group.ManTeams
                    .Select( t => t == null ? null : new TeamData(t)).ToArray();
                WomanTeams = group.WomanTeams
                    .Select( t => t == null ? null : new TeamData(t)).ToArray();
            }
        }

        public class TeamData
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public int[] Scores { get; set; }
            public TeamData(Blue_4.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Scores = team.Scores;
            }
            public TeamData() { }
        }
        //blue5
        public class Part5Data
        {
            public string Name { get; set; }
            public int Place{ get; set; }
            public string Surname { get; set; }
            public Part5Data() { }
            public Part5Data(Blue_5.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Place = sportsman.Place;
                Surname = sportsman.Surname;
                
            }
        }

        public class Team5Data
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Part5Data[] Sporsman { get; set; }
            public Team5Data() { }
            public Team5Data(Blue_5.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Sporsman = team.Sportsmen.Select(p => p == null ? null : new Part5Data(p)).ToArray();
            }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) 
            { 
                return; 
            }
            SelectFile(fileName);
            var parti = new ResponseData(participant);
            var xml = new XmlSerializer(typeof(ResponseData));
            using var p = new StreamWriter(FilePath);
            xml.Serialize(p, parti);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) 
            { 
                return null; 
            }
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(ResponseData));
            using (var reader = new StreamReader(FilePath))
            {
                var data = (ResponseData)serializer.Deserialize(reader);
                if (data == null) 
                {
                    return null;
                }
                Blue_1.Response response;
                if (data.Surname != null)
                {
                    return new Blue_1.HumanResponse(data.Name, data.Surname, data.Votes);
                }
                else
                {
                    return new Blue_1.Response(data.Name, data.Votes);
                }
            }
        }
        
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName)) return;
            SelectFile(fileName);
            var data = new WaterJumpData(participant);
            var serializer = new XmlSerializer(typeof(WaterJumpData));

            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, data);
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) 
            { 
                return null; 
            }
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(WaterJumpData));
            Blue_2.WaterJump waterJ;
            using (var reader = new StreamReader(FilePath))
            {
                var data = (WaterJumpData)serializer.Deserialize(reader);

                if (data.Type == "WaterJump5m")
                {
                    waterJ = new Blue_2.WaterJump5m(data.Name, data.Bank);
                }
                else if (data.Type == "WaterJump3m")
                {
                    waterJ = new Blue_2.WaterJump3m(data.Name, data.Bank);
                }
                else 
                { 
                    return null; 
                }
                if (data.Participants != null)
                {
                    foreach (var part in data.Participants)
                    {
                        var participant = new Blue_2.Participant(part.Name, part.Surname);
                        foreach (var marks in part.Marks)
                        {
                            if (marks != null && marks.Length == 5)
                            {
                                participant.Jump(marks);
                            }
                        }
                        waterJ.Add(participant);
                    }
                }
                
            }
            return waterJ;
        }

        
        public override void SerializeBlue3Participant<T>(T student, string fileName) //T: Blue_3.Participant
        {
            if (student == null || string.IsNullOrEmpty(fileName)) 
            { 
                return; 
            }
            SelectFile(fileName);
            var data = new PenaltiesData
            {
                Name = student.Name,
                Surname = student.Surname,
                Penalties = student.Penalties
            };
            var serializer = new XmlSerializer(typeof(PenaltiesData));
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, data);
            }
        }
        
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            PenaltiesData data;
            if (string.IsNullOrEmpty(fileName)) 
            { 
                return null; 
            }
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(PenaltiesData));
            using (var reader = new StreamReader(fileName))
            {
                data = (PenaltiesData)serializer.Deserialize(reader);
            }
            Blue_3.Participant Ans=null;
            switch (data.Type)
            {
                case "Participant":
                    Ans= new Blue_3.Participant(data.Name, data.Surname);

                    break;

                case "HockeyPlayer":
                    Ans = new Blue_3.HockeyPlayer(data.Name, data.Surname);

                    break;

                case "BasketballPlayer":
                    Ans = new Blue_3.BasketballPlayer(data.Name, data.Surname);

                    break;
            }
            foreach (var p in data.Penalties)
            {
                Ans.PlayMatch(p);
            }
            return (T)Ans;
        }


        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) 
            { 
                return; 
            }
            SelectFile(fileName);
            GroupData data = new GroupData(participant);
            var serializer = new XmlSerializer(typeof(GroupData));
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, data);
            }
        }
        
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) 
            { 
                return null; 
            }
            SelectFile(fileName);
            GroupData groupData;
            Blue_4.Group ans;
            var xmlReader = new XmlSerializer(typeof(GroupData));
            using (var stream = new StreamReader(fileName))
            {
                groupData = (GroupData)xmlReader.Deserialize(stream);
            }
            ans = new Blue_4.Group(groupData.Name);
            foreach (var manTeamData in groupData.ManTeams)
            {
                if (manTeamData == null)
                {
                    ans.Add(default(Blue_4.ManTeam));
                    continue;
                }

                Blue_4.ManTeam team = null;
                if (manTeamData.Type == "ManTeam")
                {
                    team = new Blue_4.ManTeam(manTeamData.Name);
                    foreach (var score in manTeamData.Scores)
                    {
                        team.PlayMatch(score);
                    }
                }
                ans.Add(team);
            }
            foreach (var womanTeamDto in groupData.WomanTeams)
            {
                if (womanTeamDto == null)
                {
                    ans.Add(default(Blue_4.WomanTeam));
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

                ans.Add(team);
            }

            return ans;
        }


        public override void SerializeBlue5Team<T>(T group, string fileName)//T: Blue_5.Team
        {
            if (group == null || string.IsNullOrEmpty(fileName)) 
            { 
                return; 
            }
            SelectFile(fileName);

            Team5Data data = new Team5Data(group);
            var serializer = new XmlSerializer(typeof(Team5Data));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, data);
            }
        }

        public override T DeserializeBlue5Team<T>(string fileName) //T: Blue_5.Team
        {
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Team5Data));

            Team5Data data;
            Blue_5.Team team;
            using (StreamReader reader = new StreamReader(FilePath))
            { 
                data = (Team5Data)serializer.Deserialize(reader);
            }
            if (data.Type == "ManTeam")
            {
                team = new Blue_5.ManTeam(data.Name);
            }
            else
            {
                team = new Blue_5.WomanTeam(data.Name);
            }
            for (int i = 0; i < data.Sporsman.Length; i++)
            {
                if (data.Sporsman[i] != null)
                {
                    Blue_5.Sportsman sportsman = new Blue_5.Sportsman(data.Sporsman[i].Name, data.Sporsman[i].Surname);
                    sportsman.SetPlace(data.Sporsman[i].Place);

                    team.Add(sportsman);
                }
            }
            return (T)team;
        }
    }
}
