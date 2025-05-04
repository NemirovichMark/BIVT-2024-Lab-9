using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Lab_9
{
    public abstract class BlueSerializer : FileSerializer
    {
        public abstract void SerializeBlue1Response(Blue_1.Response participant, string fileName);
        public abstract void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName);
        public abstract void SerializeBlue3Participant<T>(T student, string fileName) where T : Blue_3.Participant;
        public abstract void SerializeBlue4Group(Blue_4.Group participant, string fileName);
        public abstract void SerializeBlue5Team<T>(T group, string fileName) where T : Blue_5.Team;

        public abstract Blue_1.Response DeserializeBlue1Response(string fileName);
        public abstract Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName);
        public abstract T DeserializeBlue3Participant<T>(string fileName) where T : Blue_3.Participant;
        public abstract Blue_4.Group DeserializeBlue4Group(string fileName);
        public abstract T DeserializeBlue5Team<T>(string fileName) where T : Blue_5.Team;

        // Blue_1
        [Serializable]
        public class ResponseDTO
        {
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }

            public ResponseDTO() { }
            public ResponseDTO(Blue_1.Response res)
            {
                Name = res.Name;
                Votes = res.Votes;
                var hum = res as Blue_1.HumanResponse;
                if (hum == null) Surname = null;
                else Surname = hum.Surname;
            }
            [JsonConstructor]
            public ResponseDTO(string name, int votes, string surname)
            {
                Name = name;
                Votes = votes;
                Surname = surname;
            }
        }


        // Blue_2
        [Serializable]
        public class Blue_2_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Jump_1 { get; set; }
            public int[] Jump_2 { get; set; }


            public Blue_2_ParticipantDTO() { }

            [JsonConstructor]
            public Blue_2_ParticipantDTO(string name, string surname, int[] jump_1, int[] jump_2)
            {
                Name = name;
                Surname = surname;
                Jump_1 = new int[5];
                for (int k = 0; k < 5; k++)
                    Jump_1[k] = jump_1[k];
                Jump_2 = new int[5];
                for (int k = 0; k < 5; k++)
                    Jump_2[k] = jump_2[k];
            }
            public Blue_2_ParticipantDTO(Blue_2.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                if (p.Marks == null)
                {
                    Jump_1 = null;
                    Jump_2 = null;
                }
                else
                {
                    Jump_1 = new int[5];
                    for (int k = 0; k < 5; k++)
                    {
                        Jump_1[k] = p.Marks[0, k];
                    }
                    Jump_2 = new int[5];
                    for (int k = 0; k < 5; k++)
                    {
                        Jump_2[k] = p.Marks[1, k];
                    }
                }
            }
        }

        [Serializable]
        public class WaterJumpDTO
        {
            public bool Is3m { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2_ParticipantDTO[] Participants { get; set; }

            public WaterJumpDTO() { }

            [JsonConstructor]
            public WaterJumpDTO(bool is3m, string name, int bank, Blue_2_ParticipantDTO[] participants)
            {
                Is3m = is3m;
                Name = name;
                Bank = bank;
                Participants = new Blue_2_ParticipantDTO[participants.Length];
                for (int k = 0; k < participants.Length; k++)
                {
                    Participants[k] = participants[k];
                }
            }
            public WaterJumpDTO(Blue_2.WaterJump waterJump)
            {
                if (waterJump is Blue_2.WaterJump3m)
                    Is3m = true;
                else Is3m = false;
                Name = waterJump.Name;
                Bank = waterJump.Bank;
                Participants = new Blue_2_ParticipantDTO[waterJump.Participants.Length];
                for (int k = 0; k < waterJump.Participants.Length; k++)
                {
                    Participants[k] = new Blue_2_ParticipantDTO(waterJump.Participants[k]);
                }
            }

        }

        protected Blue_2.WaterJump GetWaterJump(WaterJumpDTO wj)
        {
            if (wj.Is3m) return new Blue_2.WaterJump3m(wj.Name, wj.Bank);
            return new Blue_2.WaterJump5m(wj.Name, wj.Bank);
        }


        // Blue_3
        public class Blue_3_ParticipantDTO
        {
            public int Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }

            public Blue_3_ParticipantDTO() { }
            public Blue_3_ParticipantDTO(Blue_3.Participant other)
            {
                if (other is Blue_3.BasketballPlayer)
                {
                    Type = 1;
                }
                else if (other is Blue_3.HockeyPlayer)
                {
                    Type = 2;
                }
                else Type = 0;
                Name = other.Name;
                Surname = other.Surname;
                Penalties = new int[other.Penalties.Length];
                for (int k = 0; k < other.Penalties.Length; k++)
                {
                    Penalties[k] = other.Penalties[k];
                }
            }
            [JsonConstructor]
            public Blue_3_ParticipantDTO(int type, string name, string surname, int[] penalties)
            {
                Type = type;
                Name = name;
                Surname = surname;
                Penalties = penalties;
            }
        }

        protected Blue_3.Participant GetPlayer(Blue_3_ParticipantDTO other)
        {
            if (other.Type == 1)
            {
                return new Blue_3.BasketballPlayer(other.Name, other.Surname);
            }
            if (other.Type == 2)
            {
                return new Blue_3.HockeyPlayer(other.Name, other.Surname);
            }
            return new Blue_3.Participant(other.Name, other.Surname);
        }


        // Blue_4
        public class Blue_4_TeamDTO
        {
            public bool IsMan { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }

            public Blue_4_TeamDTO() { }
            public Blue_4_TeamDTO(Blue_4.Team other)
            {
                if (other == null)
                    return;

                if (other is Blue_4.ManTeam)
                    IsMan = true;
                else IsMan = false;
                Name = other.Name;
                Scores = other.Scores;
            }

            [JsonConstructor]
            public Blue_4_TeamDTO(bool isMan, string name, int[] scores)
            {
                IsMan = isMan;
                Name = name;
                Scores = scores;
            }
        }
        protected Blue_4.Team GetTeam(Blue_4_TeamDTO other)
        {
            if (other.Name == null)
                return null;
            if (other.IsMan)
            {
                return new Blue_4.ManTeam(other.Name);
            }
            return new Blue_4.WomanTeam(other.Name);
        }
        public class Blue_4_GroupDTO
        {
            public string Name { get; set; }
            public Blue_4_TeamDTO[] Teams { get; set; }

            public Blue_4_GroupDTO() { }

            public Blue_4_GroupDTO(Blue_4.Group other)
            {
                Name = other.Name;
                Teams = new Blue_4_TeamDTO[other.ManTeams.Length + other.WomanTeams.Length];
                int counter = 0;
                foreach (var team in other.ManTeams)
                {
                    Teams[counter] = new Blue_4_TeamDTO(team);
                    counter++;
                }
                foreach (var team in other.WomanTeams)
                {
                    Teams[counter] = new Blue_4_TeamDTO(team);
                    counter++;
                }
            }

            [JsonConstructor]
            public Blue_4_GroupDTO(string name, Blue_4_TeamDTO[] teams)
            {
                Name = name;
                Teams = teams;
            }
        }


        // Blue_5
        public class SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }

            public SportsmanDTO() { }
            public SportsmanDTO(Blue_5.Sportsman other)
            {
                if (other == null) return;
                Name = other.Name;
                Surname = other.Surname;
                Place = other.Place;
            }
            [JsonConstructor]
            public SportsmanDTO(string name, string surname, int place)
            {
                Name = name;
                Surname = surname;
                Place = place;
            }

        }
        public class Blue_5_TeamDTO
        {
            public bool IsMan { get; set; }
            public string Name { get; set; }
            public SportsmanDTO[] Sportsmen { get; set; }

            public Blue_5_TeamDTO() { }
            public Blue_5_TeamDTO(Blue_5.Team other)
            {
                if (other == null) return;
                if(other is Blue_5.ManTeam)
                    IsMan = true;
                else IsMan = false;
                Name = other.Name;
                Sportsmen = new SportsmanDTO[other.Sportsmen.Length];
                for(int k = 0; k < other.Sportsmen.Length; k++)
                {
                    Sportsmen[k] = new SportsmanDTO(other.Sportsmen[k]);
                }
            }
            [JsonConstructor]
            public Blue_5_TeamDTO(bool isMan, string name, SportsmanDTO[] sportsmen)
            {
                IsMan = isMan;
                Name = name;
                Sportsmen = sportsmen;
            }
            
        }
        protected Blue_5.Team GetTeam(Blue_5_TeamDTO other)
        {
            if (other.Name == null) return null;
            if(other.IsMan)
                return new Blue_5.ManTeam(other.Name);
            return new Blue_5.WomanTeam(other.Name);
        }
    }
}
