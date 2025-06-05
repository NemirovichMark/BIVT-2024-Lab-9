using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Lab_7.Blue_1;
using static Lab_7.Blue_2;
using static Lab_7.Blue_5;
using static Lab_9.BlueXMLSerializer;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public class Blue1_Response_DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Votes { get; set; }
            public Blue1_Response_DTO() { } // обязателен для сериализации
            public Blue1_Response_DTO(Blue_1.Response smth) //
            {
                Type = smth.GetType().Name;
                Name = smth.Name;
                Votes = smth.Votes;
                if (smth is Blue_1.HumanResponse smone) Surname = smone.Surname;
            }
            
        }
        public class Blue2_WaterJump_DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue2_Participant_DTO[] Participants { get; set; }

            public Blue2_WaterJump_DTO() { } // обязателен для сериализации
            public Blue2_WaterJump_DTO(Blue_2.WaterJump smth) //
            {
                Type = smth.GetType().Name;
                Name = smth.Name;
                Bank = smth.Bank;
                if (smth.Participants != null)
                {
                    Participants = new Blue2_Participant_DTO[smth.Participants.Length];
                    for (int i = 0; i < smth.Participants.Length; i++)
                    {
                        Participants[i] = new Blue2_Participant_DTO(smth.Participants[i]);
                    }
                }
            }
        }
        
        public class Blue2_Participant_DTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }
            public Blue2_Participant_DTO() { } // обязателен для сериализации
            public Blue2_Participant_DTO(Blue_2.Participant rival)
            {
                Name = rival.Name;
                Surname = rival.Surname;
                Marks = Cr_marks(rival.Marks);
            }
            private int[][] Cr_marks(int[,] a)//рваный
            {
                if (a == null) return null;
                int[][] c = new int[a.GetLength(0)][];
                for (int i = 0; i < c.GetLength(0); i++)
                {
                    c[i] = new int[a.GetLength(1)];
                    for (int j = 0; j < c[i].Length; j++)
                    {
                        c[i][j] = a[i, j];
                    }
                }
                return c;
            }
        }
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            Blue1_Response_DTO ex_b= new Blue1_Response_DTO(participant);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue1_Response_DTO));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xmlSerializer.Serialize(fs, ex_b);
            }
           
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            Blue2_WaterJump_DTO ex_b = new Blue2_WaterJump_DTO(participant);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue2_WaterJump_DTO));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xmlSerializer.Serialize(fs, ex_b);
            }
            
        }
        public class Blue3_Participant_DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
            public Blue3_Participant_DTO() { }
            public Blue3_Participant_DTO(Blue_3.Participant smone)
            {
                Type = smone.GetType().Name;
                Name = smone.Name;
                Surname = smone.Surname;
                Penalties = smone.Penalties;
            }
        }


        public override void SerializeBlue3Participant<T>(T student, string fileName) where T : class
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            Blue3_Participant_DTO info_3 = new Blue3_Participant_DTO(student);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue3_Participant_DTO));
            using (StreamWriter fs = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(fs, info_3);
            }

        }
        public class Blue4_Team_DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }
            public Blue4_Team_DTO() { }
            public Blue4_Team_DTO(Blue_4.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Scores = team.Scores;
            }
        }

        public class Blue4_Group_DTO
        {
            public string Name { get; set; }
            public Blue4_Team_DTO[] ManTeams { get; set; }
            public Blue4_Team_DTO[] WomanTeams { get; set; }
            public Blue4_Group_DTO() { }
            public Blue4_Group_DTO(Blue_4.Group group)
            {
                Name = group.Name;

                if (group.ManTeams != null)
                {
                    var manTeamsList = new List<Blue4_Team_DTO>();
                    foreach (var team in group.ManTeams)
                    {
                        manTeamsList.Add(team != null ? new Blue4_Team_DTO(team):null);
                    }
                    ManTeams = manTeamsList.ToArray();
                }
                else ManTeams = null;

                if (group.WomanTeams != null)
                {
                    var womanTeamsList = new List<Blue4_Team_DTO>();
                    foreach (var team in group.WomanTeams)
                    {
                        womanTeamsList.Add(team != null ? new Blue4_Team_DTO(team):null);
                    }
                    WomanTeams = womanTeamsList.ToArray();
                }
                else WomanTeams = null;
                
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName)) return;
            SelectFile(fileName);
            Blue4_Group_DTO info = new Blue4_Group_DTO(participant);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue4_Group_DTO));
            using (StreamWriter fs = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(fs, info);
            }
        }

        public class Blue5_Sportsman_DTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }

            public Blue5_Sportsman_DTO() { }

            public Blue5_Sportsman_DTO(Blue_5.Sportsman sportsman)
            {
                if (sportsman == null) return;
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }
        }
        public class Blue5_Team_DTO
        {
            
            public string Type { get; set; }
            public string Name { get; set; }
            public Blue5_Sportsman_DTO[] Sportsmen { get; set; }

            public Blue5_Team_DTO() { }

            public Blue5_Team_DTO(Blue_5.Team team)
            {
                if (team == null) return;
                Type = team.GetType().Name;
                Name = team.Name;
                Sportsmen = team.Sportsmen?.Where(s => s != null).Select(s => new Blue5_Sportsman_DTO(s)).ToArray();
            }
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) where T : class
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            Blue5_Team_DTO inf = new Blue5_Team_DTO(group);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue5_Team_DTO));
            using (StreamWriter fs = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(fs, inf);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;
            Blue1_Response_DTO blue1_Response_DTO;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue1_Response_DTO));

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                blue1_Response_DTO = xmlSerializer.Deserialize(fs) as Blue1_Response_DTO;
                
            }
            if (blue1_Response_DTO==null) return null;
            if (blue1_Response_DTO.Type == nameof(Blue_1.HumanResponse)) return new Blue_1.HumanResponse(blue1_Response_DTO.Name, blue1_Response_DTO.Surname, blue1_Response_DTO.Votes);
            else return new Blue_1.Response(blue1_Response_DTO.Name, blue1_Response_DTO.Votes);
            
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            Blue2_WaterJump_DTO blue2_DTO;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue2_WaterJump_DTO));

            using (StreamReader fs = new StreamReader(fileName))
            {
                blue2_DTO = xmlSerializer.Deserialize(fs) as Blue2_WaterJump_DTO;

            }
            Blue_2.WaterJump camp_of_p;
            if (blue2_DTO.Type == "WaterJump5m") camp_of_p = new Blue_2.WaterJump5m(blue2_DTO.Name, blue2_DTO.Bank);
            else camp_of_p = new Blue_2.WaterJump3m(blue2_DTO.Name, blue2_DTO.Bank);

            foreach (Blue2_Participant_DTO rival in blue2_DTO.Participants)
            {
                Blue_2.Participant ad_p = new Blue_2.Participant(rival.Name, rival.Surname);
                foreach (var number in rival.Marks)
                {
                    ad_p.Jump(number);
                }
                camp_of_p.Add(ad_p);
            }
            return camp_of_p;

        }
        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue3_Participant_DTO));
            Blue3_Participant_DTO pchel;//just pchel
            using (StreamReader fs = new StreamReader(fileName))
            {
                pchel = xmlSerializer.Deserialize(fs) as Blue3_Participant_DTO;
            }

            Blue_3.Participant real_chel = new Blue_3.Participant(pchel.Name, pchel.Surname);//general then check whish player it is
            if (pchel.Type == "BasketballPlayer")
            {
                real_chel = new Blue_3.BasketballPlayer(pchel.Name, pchel.Surname);
            }
            else if (pchel.Type == "HockeyPlayer")
            {
                real_chel = new Blue_3.HockeyPlayer(pchel.Name, pchel.Surname);
            }

            foreach (var outmin in pchel.Penalties)
            {
                real_chel.PlayMatch(outmin);
            }
            return (T)real_chel;
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            Blue4_Group_DTO inf;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue4_Group_DTO));
            using (var ss = new StreamReader(fileName))
            {
                inf = xmlSerializer.Deserialize(ss) as Blue4_Group_DTO;
            }
            if (inf == null) return null;
            var group = new Blue_4.Group(inf.Name);
            void AddTeams(Blue4_Team_DTO[] teams, Func<string, Blue_4.Team> team_maker)
            {
                if (teams == null) return;

                foreach (var teamdto in teams)
                {
                    if (teamdto == null) continue;
                    var team = team_maker(teamdto.Name);
                    if (teamdto.Scores != null)
                    {
                        foreach (var point in teamdto.Scores)
                        {
                            team.PlayMatch(point);
                        }
                    }
                    group.Add(team);
                }
            }
            AddTeams(inf.ManTeams, name => new Blue_4.ManTeam(name));
            AddTeams(inf.WomanTeams, name => new Blue_4.WomanTeam(name));
            return group;
        }
        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            Blue5_Team_DTO team_dto;
            var xmlSerializer = new XmlSerializer(typeof(Blue5_Team_DTO));
            using (var fs = new StreamReader(fileName))
            {
                team_dto = xmlSerializer.Deserialize(fs) as Blue5_Team_DTO;
            }
            if (team_dto == null) return null;
            Blue_5.Team team = team_dto.Type == "ManTeam" ? new Blue_5.ManTeam(team_dto.Name) : new Blue_5.WomanTeam(team_dto.Name);
            if (team_dto.Sportsmen != null)
            {
                foreach (var sportsman_Dto in team_dto.Sportsmen.Where(dto => dto != null))
                {
                    var sportsman_r = new Blue_5.Sportsman(sportsman_Dto.Name, sportsman_Dto.Surname);
                    sportsman_r.SetPlace(sportsman_Dto.Place);
                    team.Add(sportsman_r);
                }
            }
            return (T)team;
        }
        public override string Extension => "xml";
    }
}
