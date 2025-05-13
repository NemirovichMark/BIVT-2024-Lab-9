using Lab_7;
using System.Text.Json.Serialization;
using static Lab_7.Blue_1;

namespace Lab_9
{
    public class SerializeObject
    {
        [Serializable]
        public class ResponseSerialize
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Votes { get; set; }
           

            public ResponseSerialize() { }
            public ResponseSerialize(Blue_1.Response respons)
            {
                Name = respons.Name;
                var sportsman = respons as Blue_1.HumanResponse;
                if (sportsman == null) Surname = null;
                else Surname = sportsman.Surname;
                Votes = respons.Votes;
                
            }
            [JsonConstructor]
            public ResponseSerialize(string name, string surname, int votes)
            {
                Name = name;
                
                Surname = surname;
                Votes = votes;
            }
        }

        [Serializable]
        public class ParticipantSeerialize
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public int[]? FirstJump { get; set; }
            public int[]? SecondJump { get; set; }

            public ParticipantSeerialize() { }

            [JsonConstructor]
            public ParticipantSeerialize(string name, string surname, int[]? firstJump, int[]? secondJump)
            {
                Name = name ?? string.Empty;
                Surname = surname ?? string.Empty;
                FirstJump = firstJump?.Length == 5 ? firstJump.ToArray() : new int[5];
                SecondJump = secondJump?.Length == 5 ? secondJump.ToArray() : new int[5];
            }

            public ParticipantSeerialize(Blue_2.Participant participant)
            {
                Name = participant.Name ?? string.Empty;
                Surname = participant.Surname ?? string.Empty;

                if (participant.Marks != null)
                {
                    FirstJump = new int[5];
                    SecondJump = new int[5];
                    for (int i = 0; i < 5; i++)
                    {
                        FirstJump[i] = participant.Marks[0, i];
                        SecondJump[i] = participant.Marks[1, i];
                    }
                }
            }
        }

        [Serializable]
        public class SerializeWaterJump
        {
            public int Len { get; set; }
            public string? Name { get; set; }
            public int Bank { get; set; }
            public ParticipantSeerialize[]? Participants { get; set; }

            public SerializeWaterJump() { }

            [JsonConstructor]
            public SerializeWaterJump(int len, string name, int bank, ParticipantSeerialize[] participants)
            {
                Len = len;
                Name = name;
                Bank = bank;
                Participants = new ParticipantSeerialize[participants.Length];
                for (int i = 0; i < participants.Length; i++)
                {
                    Participants[i] = participants[i];
                }
            }
            public SerializeWaterJump(Blue_2.WaterJump waterJump)
            {
                if (waterJump is Blue_2.WaterJump3m) Len = 3;
                else Len = 5;
                Name = waterJump.Name;
                Bank = waterJump.Bank;
                Participants = new ParticipantSeerialize[waterJump.Participants.Length];
                for (int i = 0; i < waterJump.Participants.Length; i++)
                {
                    Participants[i] = new ParticipantSeerialize(waterJump.Participants[i]);
                }
            }

        }
       


        public class OtherPart
        {
            public int Sport { get; set; }
            public string? Name { get; set; }
            public string? Surname { get; set; }
            public int[]? Penalties { get; set; }

            public OtherPart() { }
            public OtherPart(Blue_3.Participant participant)
            {
                if (participant is Blue_3.BasketballPlayer) Sport = 1;
                else if (participant is Blue_3.HockeyPlayer) Sport = 2;
                

                Name = participant.Name;
                Surname = participant.Surname;
                Penalties = new int[participant.Penalties.Length];

               
                for (int i = 0; i < participant.Penalties.Length; i++)
                {
                    Penalties[i] = participant.Penalties[i];
                }
            }
            [JsonConstructor]
            public OtherPart(int sport, string name, string surname, int[] penalties)
            {
                Sport = sport;
                Name = name;
                Surname = surname;
                Penalties = penalties;
            }
        }

      


        public class SerializeTeam
        {
            public bool ManTeamOrWoman { get; set; }
            public string? Name { get; set; }
            public int[]? Scores { get; set; }

            public SerializeTeam() { }
            public SerializeTeam(Blue_4.Team team)
            {
                if (team == null) return;

                ManTeamOrWoman = team is Blue_4.ManTeam;
                Name = team.Name;
                Scores = team.Scores;
            }

            [JsonConstructor]
            public SerializeTeam(bool manTeamOrWoman, string name, int[] scores)
            {
                ManTeamOrWoman = manTeamOrWoman;
                Name = name;
                Scores = scores;
            }
        }
        
        public class SerializeGroup
        {
            public string? Name { get; set; }
            public SerializeTeam[]? Teams { get; set; }

            public SerializeGroup() { }

            public SerializeGroup(Blue_4.Group group)
            {
                Name = group.Name;
                Teams = new SerializeTeam[group.ManTeams.Length + group.WomanTeams.Length];
                int cnt = 0;
                foreach (var team in group.ManTeams)
                {
                    Teams[cnt] = new SerializeTeam(team);
                    cnt++;
                }
                foreach (var team in group.WomanTeams)
                {
                    Teams[cnt] = new SerializeTeam(team);
                    cnt++;
                }
            }

            [JsonConstructor]
            public SerializeGroup(string name, SerializeTeam[] teams)
            {
                Name = name;
                Teams = teams;
            }
        }

        public class SerializeSportsman
        {
            public string? Name { get; set; }
            public string? Surname { get; set; }
            public int Place { get; set; }

            public SerializeSportsman() { }
            public SerializeSportsman(Blue_5.Sportsman sportsman)
            {
                if (sportsman == null) return;
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }
            [JsonConstructor]
            public SerializeSportsman(string name, string surname, int place)
            {
                Name = name;
                Surname = surname;
                Place = place;
            }

        }
        public class SerializeOtherTeam
        {
            public bool ManTeamOrWoman { get; set; }
            public string? Name { get; set; }
            public SerializeSportsman[]? Sportsman { get; set; }

            public SerializeOtherTeam() { }
            public SerializeOtherTeam(Blue_5.Team team)
            {
                if (team == null) return;

                ManTeamOrWoman = team is Blue_5.ManTeam;
                Name = team.Name;
                Sportsman = new SerializeSportsman[team.Sportsmen.Length];
                for (int i = 0; i < team.Sportsmen.Length; i++)
                {
                    Sportsman[i] = new SerializeSportsman(team.Sportsmen[i]);
                }
            }
            [JsonConstructor]
            public SerializeOtherTeam(bool manTeamOrWoman, string name, SerializeSportsman[] sportsman)
            {
                ManTeamOrWoman = manTeamOrWoman;
                Name = name;
                Sportsman = sportsman;
            }

        }
       

    }
}
