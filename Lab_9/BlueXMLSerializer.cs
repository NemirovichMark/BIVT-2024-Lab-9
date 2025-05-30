using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Lab_7;
using static Lab_7.Blue_1;
using static Lab_7.Blue_4;
using static Lab_7.Blue_5;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";
        //Вспомогательные классы и методы для решения
        public class ResponseDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }
            public ResponseDTO() { }
            public ResponseDTO(Blue_1.Response response)
            {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;
                if (response is Blue_1.HumanResponse r) Surname = r.Surname;
            }
        }

        public class ParticipantBlue_2DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }
            public ParticipantBlue_2DTO() { }
            public ParticipantBlue_2DTO(Blue_2.Participant participant)
            {
                Type = participant.GetType().Name;
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = MToZ(participant.Marks);
            }
        }
        private static int[][] MToZ(int[,] m)
        {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);
            int[][] a = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                a[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    a[i][j] = m[i, j];
                }
            }
            return a;
        }
        private static int[,] ZToM(int[][] a)
        {
            int rows = a.Length;
            int cols = a[0].Length;
            int[,] m = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    m[i, j] = a[i][j];
                }
            }
            return m;
        }
        public class WaterJumpDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public ParticipantBlue_2DTO[] Participants { get; set; }
            public WaterJumpDTO() { }
            public WaterJumpDTO(Blue_2.WaterJump jump)
            {
                Type = jump.GetType().Name;
                Name = jump.Name;
                Bank = jump.Bank;
                if (jump.Participants.Length >= 1)
                {
                    Participants = new ParticipantBlue_2DTO[jump.Participants.Length];
                    for (int i = 0; i < jump.Participants.Length; i++)
                    {
                        Participants[i] = new ParticipantBlue_2DTO(jump.Participants[i]);
                    }
                }
            }
        }
        public class ParticipantBlue_3DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
            public ParticipantBlue_3DTO() { }
            public ParticipantBlue_3DTO(Blue_3.Participant participant)
            {
                Type = participant.GetType().Name;
                Name = participant.Name;
                Surname = participant.Surname;
                if (participant.Penalties.Length > 0)
                {
                    Penalties = new int[participant.Penalties.Length];
                    for (int i = 0; i < participant.Penalties.Length; i++)
                    {
                        Penalties[i] = participant.Penalties[i];
                    }
                }
            }
        }
        public class TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }
            public TeamDTO() { }
            public TeamDTO(Blue_4.Team team)
            {
                if (team == null) return;
                Type = team.GetType().Name;
                Name = team.Name;
                Scores = new int[team.Scores.Length];
                if (team.Scores != null)
                {
                    for (int i = 0; i < team.Scores.Length; i++)
                    {
                        Scores[i] = team.Scores[i];
                    }
                }
            }
        }
        public class GroupBlue_4DTO
        {
            public string Name { get; set; }
            public TeamDTO[] ManTeams { get; set; }
            public TeamDTO[] WomanTeams { get; set; }
            public GroupBlue_4DTO() { }

            public GroupBlue_4DTO(Blue_4.Group group)
            {
                if (group == null) return;
                Name = group.Name;
                if (group.ManTeams != null && group.ManTeams.Length > 0)
                {
                    ManTeams = new TeamDTO[group.ManTeams.Length];
                    for (int i = 0; i < group.ManTeams.Length; i++)
                    {
                        ManTeams[i] = new TeamDTO(group.ManTeams[i]);
                    }
                }
                if (group.WomanTeams != null && group.WomanTeams.Length > 0)
                {
                    WomanTeams = new TeamDTO[group.WomanTeams.Length];
                    for (int i = 0; i < group.WomanTeams.Length; i++)
                    {
                        WomanTeams[i] = new TeamDTO(group.WomanTeams[i]);
                    }
                }
            }
        }
        public class SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }
            public SportsmanDTO() { }
            public SportsmanDTO(Blue_5.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }
        }
        public class TeamBlue_5DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public SportsmanDTO[] Sportsmen { get; set; }
            public TeamBlue_5DTO() { }
            public TeamBlue_5DTO(Blue_5.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                if (team.Sportsmen != null)
                {
                    Sportsmen = new SportsmanDTO[team.Sportsmen.Length];
                    for (int i = 0; i < team.Sportsmen.Length; i++)
                    {
                        if (team.Sportsmen[i] != null)
                        {
                            Sportsmen[i] = new SportsmanDTO(team.Sportsmen[i]);
                        }
                        else
                        {
                            Sportsmen[i] = default(SportsmanDTO);
                        }
                    }
                }
            }
        }
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var rdto = new ResponseDTO(participant);
            var Xml = new XmlSerializer(typeof(ResponseDTO));
            using (var writer = new StreamWriter(FilePath))
            {
                Xml.Serialize(writer, rdto);
            }
        }
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            ResponseDTO rdto;
            var Xml = new XmlSerializer(typeof(ResponseDTO));
            Blue_1.Response res;
            using (var reader = new StreamReader(FilePath))
            {
                rdto = (ResponseDTO)Xml.Deserialize(reader);
            }
            if (rdto.Surname != null)
            {
                res = new Blue_1.HumanResponse(rdto.Name, rdto.Surname, rdto.Votes);
            }
            else
            {
                res = new Blue_1.Response(rdto.Name, rdto.Votes);
            }
            return res;
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) { return; }
            SelectFile(fileName);
            var wjdto = new WaterJumpDTO(participant);
            var Xml = new XmlSerializer(typeof(WaterJumpDTO));
            using (var writer = new StreamWriter(FilePath))
            {
                Xml.Serialize(writer, wjdto);
            }
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            WaterJumpDTO wjdto;
            var Xml = new XmlSerializer(typeof(WaterJumpDTO));
            Blue_2.WaterJump res;
            using (var reader = new StreamReader(FilePath))
            {
                wjdto = (WaterJumpDTO)Xml.Deserialize(reader);
            }
            if (wjdto.Type != "WaterJump3m")
            {
                res = new Blue_2.WaterJump5m(wjdto.Name, wjdto.Bank);
            }
            else if (wjdto.Type == "WaterJump3m")
            {
                res = new Blue_2.WaterJump3m(wjdto.Name, wjdto.Bank);
            }
            else return null;
            if (wjdto.Participants != null)
            {
                for (int i = 0; i < wjdto.Participants.Length; i++)
                {
                    if (wjdto.Participants[i] == null) continue;
                    var participant = new Blue_2.Participant(wjdto.Participants[i].Name, wjdto.Participants[i].Surname);
                    int[,] marks = new int[2, 5];
                    if (wjdto.Participants[i].Marks == null) continue;
                    for (int j = 0; j < 2; j++)
                    {
                        if (j >= wjdto.Participants[i].Marks.Length || wjdto.Participants[i].Marks[j] == null)
                            continue;
                        for (int l = 0; l < 5; l++)
                        {
                            if (l >= wjdto.Participants[i].Marks[j].Length) break;
                            marks[j, l] = wjdto.Participants[i].Marks[j][l];
                        }
                    }
                    for (int j = 0; j < 2; j++)
                    {
                        int[] jump = new int[5];
                        for (int h = 0; h < 5; h++)
                        {
                            jump[h] = marks[j, h];
                        }
                        participant.Jump(jump);
                    }
                    res.Add(participant);
                }
            }
            return res;
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : Blue_3.Participant;
        {
            if (student == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var pb3dto = new ParticipantBlue_3DTO(student);
            var Xml = new XmlSerializer(typeof(ParticipantBlue_3DTO));
            using (var writer = new StreamWriter(FilePath))
            {
                Xml.Serialize(writer, pb3dto);
            }
        }
        public override T DeserializeBlue3Participant<T>(string fileName) //where T : Blue_3.Participant
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            ParticipantBlue_3DTO pb3dto;
            var Xml = new XmlSerializer(typeof(ParticipantBlue_3DTO));
            Blue_3.Participant res;
            using (var reader = new StreamReader(FilePath))
            {
                pb3dto = (ParticipantBlue_3DTO)Xml.Deserialize(reader);
            }
            if (pb3dto.Type == "Participant")
            {
                res = new Blue_3.Participant(pb3dto.Name, pb3dto.Surname);
            }
            else if (pb3dto.Type == "HockeyPlayer")
            {
                res = new Blue_3.HockeyPlayer(pb3dto.Name, pb3dto.Surname);
            }
            else if (pb3dto.Type == "BasketballPlayer")
            {
                res = new Blue_3.BasketballPlayer(pb3dto.Name, pb3dto.Surname);
            }
            else return null;
            for (int i = 0; i < pb3dto.Penalties.Length; i++)
            {
                res.PlayMatch(pb3dto.Penalties[i]);
            }
            return (T)res;
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var gb4dto = new GroupBlue_4DTO(participant);
            var Xml = new XmlSerializer(typeof(GroupBlue_4DTO));
            using (var writer = new StreamWriter(FilePath))
            {
                Xml.Serialize(writer, gb4dto);
            }
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            GroupBlue_4DTO gb4dto;
            var Xml = new XmlSerializer(typeof(GroupBlue_4DTO));
            using (var reader = new StreamReader(FilePath))
            {
                gb4dto = (GroupBlue_4DTO)Xml.Deserialize(reader);
            }
            Blue_4.Group res = new Blue_4.Group(gb4dto.Name);
            if (gb4dto.ManTeams != null)
            {
                for (int i = 0; i < gb4dto.ManTeams.Length; i++)
                {
                    if (gb4dto.ManTeams[i] == null)
                    {
                        res.Add(default(Blue_4.ManTeam));
                        continue;
                    }
                    else if (gb4dto.ManTeams[i].Type == "ManTeam")
                    {
                        Blue_4.ManTeam manT = new Blue_4.ManTeam(gb4dto.ManTeams[i].Name);
                        if (gb4dto.ManTeams[i].Scores != null)
                        {
                            for (int j = 0; j < gb4dto.ManTeams[i].Scores.Length; j++)
                            {
                                manT.PlayMatch(gb4dto.ManTeams[i].Scores[j]);
                            }
                        }
                        res.Add(manT);
                    }
                    else
                    {
                        res.Add(default(Blue_4.ManTeam));
                    }
                }
            }
            if (gb4dto.WomanTeams != null)
            {
                for (int i = 0; i < gb4dto.WomanTeams.Length; i++)
                {
                    if (gb4dto.WomanTeams[i] == null)
                    {
                        res.Add(default(Blue_4.WomanTeam));
                        continue;
                    }
                    else if (gb4dto.WomanTeams[i].Type == "WomanTeam")
                    {
                        Blue_4.WomanTeam womanT = new Blue_4.WomanTeam(gb4dto.WomanTeams[i].Name);
                        if (gb4dto.WomanTeams[i].Scores != null)
                        {
                            for (int j = 0; j < gb4dto.WomanTeams[i].Scores.Length; j++)
                            {
                                womanT.PlayMatch(gb4dto.WomanTeams[i].Scores[j]);
                            }
                        }
                        res.Add(womanT);
                    }
                    else res.Add(default(Blue_4.WomanTeam));
                }
            }
            return res;
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) //where T : Blue_5.Team;
        {
            if (group == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var tb5dto = new TeamBlue_5DTO(group);
            var Xml = new XmlSerializer(typeof(TeamBlue_5DTO));
            using (var writer = new StreamWriter(FilePath))
            {
                Xml.Serialize(writer, tb5dto);
            }
        }
        public override T DeserializeBlue5Team<T>(string fileName) //where T : Blue_5.Team;
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            TeamBlue_5DTO tb5dto;
            var Xml = new XmlSerializer(typeof(TeamBlue_5DTO));
            using (var reader = new StreamReader(FilePath))
            {
                tb5dto = (TeamBlue_5DTO)Xml.Deserialize(reader);
            }
            Blue_5.Team h;
            T res;
            if (tb5dto.Type == "ManTeam")
            {
                h = new Blue_5.ManTeam(tb5dto.Name);
            }
            else if (tb5dto.Type == "WomanTeam")
            {
                h = new Blue_5.WomanTeam(tb5dto.Name);
            }
            else return null;
            res = h as T;
            if (tb5dto.Sportsmen == null) return res;
            else
            {
                for (int i = 0; i < tb5dto.Sportsmen.Length; i++)
                {
                    if (tb5dto.Sportsmen[i] == null) continue;
                    var tS = new Blue_5.Sportsman(tb5dto.Sportsmen[i].Name, tb5dto.Sportsmen[i].Surname);
                    tS.SetPlace(tb5dto.Sportsmen[i].Place);
                    h.Add(tS);
                }
            }
            res = h as T;
            return res;
        }
    }
    
}