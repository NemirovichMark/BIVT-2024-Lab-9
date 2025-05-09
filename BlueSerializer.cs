using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


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



        // вспомогательные классы 
        // для оптимизации передчи информации между классами
        [Serializable]
        public class ResponseOutput
        {
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }

            public ResponseOutput() { }
            public ResponseOutput(Blue_1.Response response)
            {
                Name = response.Name;
                Votes = response.Votes;
                var sportsman = response as Blue_1.HumanResponse;
                if (sportsman == null) Surname = null;
                else Surname = sportsman.Surname;
            }
            [JsonConstructor]
            public ResponseOutput(string name, int votes, string surname)
            {
                Name = name;
                Votes = votes;
                Surname = surname;
            }
        }

        [Serializable]
        public class Blue_2_ParticipantOutput
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public int[]? FirstJump { get; set; }
            public int[]? SecondJump { get; set; }

            public Blue_2_ParticipantOutput() { }

            [JsonConstructor]
            public Blue_2_ParticipantOutput(string name, string surname, int[]? firstJump, int[]? secondJump)
            {
                Name = name ?? string.Empty;
                Surname = surname ?? string.Empty;
                FirstJump = firstJump?.Length == 5 ? firstJump.ToArray() : new int[5];
                SecondJump = secondJump?.Length == 5 ? secondJump.ToArray() : new int[5];
            }

            public Blue_2_ParticipantOutput(Blue_2.Participant participant)
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
        public class WaterJumpOutput
        {
            public int Height { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2_ParticipantOutput[] Participants { get; set; }

            public WaterJumpOutput() { }

            [JsonConstructor]
            public WaterJumpOutput(int height, string name, int bank, Blue_2_ParticipantOutput[] participants)
            {
                Height = height;
                Name = name;
                Bank = bank;
                Participants = new Blue_2_ParticipantOutput[participants.Length];
                for (int i = 0; i < participants.Length; i++)
                {
                    Participants[i] = participants[i];
                }
            }
            public WaterJumpOutput(Blue_2.WaterJump waterJump)
            {
                if (waterJump is Blue_2.WaterJump3m) Height = 3;
                else Height = 5;
                Name = waterJump.Name;
                Bank = waterJump.Bank;
                Participants = new Blue_2_ParticipantOutput[waterJump.Participants.Length];
                for (int i = 0; i < waterJump.Participants.Length; i++)
                {
                    Participants[i] = new Blue_2_ParticipantOutput(waterJump.Participants[i]);
                }
            }

        }
        protected Blue_2.WaterJump GetWaterJump(WaterJumpOutput waterjump)
        {
            if (waterjump.Height == 3) return new Blue_2.WaterJump3m(waterjump.Name, waterjump.Bank);
            return new Blue_2.WaterJump5m(waterjump.Name, waterjump.Bank);
        }


        public class Blue_3_ParticipantOutput
        {
            public int Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }

            public Blue_3_ParticipantOutput() { }
            public Blue_3_ParticipantOutput(Blue_3.Participant participant)
            {
                if (participant is Blue_3.BasketballPlayer) Type = 1;
                else if (participant is Blue_3.HockeyPlayer) Type = 2;
                else Type = 0;

                Name = participant.Name;
                Surname = participant.Surname;
                Penalties = new int[participant.Penalties.Length];

                for (int i = 0; i < participant.Penalties.Length; i++)
                {
                    Penalties[i] = participant.Penalties[i];
                }
            }
            [JsonConstructor]
            public Blue_3_ParticipantOutput(int type, string name, string surname, int[] penalties)
            {
                Type = type;
                Name = name;
                Surname = surname;
                Penalties = penalties;
            }
        }

        protected Blue_3.Participant GetPlayer(Blue_3_ParticipantOutput participant)
        {
            if (participant.Type == 1)
            {
                return new Blue_3.BasketballPlayer(participant.Name, participant.Surname);
            }
            if (participant.Type == 2)
            {
                return new Blue_3.HockeyPlayer(participant.Name, participant.Surname);
            }
            return new Blue_3.Participant(participant.Name, participant.Surname);
        }


        public class Blue_4_TeamOutput
        {
            public bool IsManTeam { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }

            public Blue_4_TeamOutput() { }
            public Blue_4_TeamOutput(Blue_4.Team team)
            {
                if (team == null) return;

                IsManTeam = team is Blue_4.ManTeam;
                Name = team.Name;
                Scores = team.Scores;
            }

            [JsonConstructor]
            public Blue_4_TeamOutput(bool isManTeam, string name, int[] scores)
            {
                IsManTeam = isManTeam;
                Name = name;
                Scores = scores;
            }
        }
        protected Blue_4.Team GetTeam(Blue_4_TeamOutput team)
        {
            if (team.Name == null) return null;
            if (team.IsManTeam)
            {
                return new Blue_4.ManTeam(team.Name);
            }
            return new Blue_4.WomanTeam(team.Name);
        }
        public class Blue_4_GroupOutput
        {
            public string Name { get; set; }
            public Blue_4_TeamOutput[] Teams { get; set; }

            public Blue_4_GroupOutput() { }

            public Blue_4_GroupOutput(Blue_4.Group group)
            {
                Name = group.Name;
                Teams = new Blue_4_TeamOutput[group.ManTeams.Length + group.WomanTeams.Length];
                int counter = 0;
                foreach (var team in group.ManTeams)
                {
                    Teams[counter] = new Blue_4_TeamOutput(team);
                    counter++;
                }
                foreach (var team in group.WomanTeams)
                {
                    Teams[counter] = new Blue_4_TeamOutput(team);
                    counter++;
                }
            }

            [JsonConstructor]
            public Blue_4_GroupOutput(string name, Blue_4_TeamOutput[] teams)
            {
                Name = name;
                Teams = teams;
            }
        }

        public class SportsmanOutput  // Blue_5
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }

            public SportsmanOutput() { }
            public SportsmanOutput(Blue_5.Sportsman sportsman)
            {
                if (sportsman == null) return;
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }
            [JsonConstructor]
            public SportsmanOutput(string name, string surname, int place)
            {
                Name = name;
                Surname = surname;
                Place = place;
            }

        }
        public class Blue_5_TeamOutput
        {
            public bool IsManTeam { get; set; }
            public string Name { get; set; }
            public SportsmanOutput[] Sportsman { get; set; }

            public Blue_5_TeamOutput() { }
            public Blue_5_TeamOutput(Blue_5.Team team)
            {
                if (team == null) return;

                IsManTeam = team is Blue_5.ManTeam;
                Name = team.Name;
                Sportsman = new SportsmanOutput[team.Sportsmen.Length];
                for (int i = 0; i < team.Sportsmen.Length; i++)
                {
                    Sportsman[i] = new SportsmanOutput(team.Sportsmen[i]);
                }
            }
            [JsonConstructor]
            public Blue_5_TeamOutput(bool isManTeam, string name, SportsmanOutput[] sportsman)
            {
                IsManTeam = isManTeam;
                Name = name;
                Sportsman = sportsman;
            }

        }
        protected Blue_5.Team GetTeam(Blue_5_TeamOutput team)
        {
            if (team.Name == null) return null;

            return team.IsManTeam ? new Blue_5.ManTeam(team.Name) : new Blue_5.WomanTeam(team.Name);
        }

    }
}
