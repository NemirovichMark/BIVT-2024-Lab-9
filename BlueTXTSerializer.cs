using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Blue_1;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var lines = participant is Blue_1.HumanResponse humanResponse
            ? new[] { "HumanResponse", humanResponse.Name, humanResponse.Surname, humanResponse.Votes.ToString() }
            : new[] { "Response", participant.Name, participant.Votes.ToString() };
            File.WriteAllLines(fileName, lines);
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            var lines = new List<string>
            {participant.GetType().Name,participant.Name,participant.Bank.ToString()};

            if (participant.Participants != null)
            {
                foreach (var p in participant.Participants)
                {
                    lines.AddRange(new[]
                    {"Participant",p.Name,p.Surname,
                    string.Join(",", p.Marks)});
                }
            }
            File.WriteAllLines(fileName, lines);
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) where T : class
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var lines = new[]
            {
                student.GetType().Name,
                student.Name,
                student.Surname,
                string.Join(",", student.Penalties)
            };
            File.WriteAllLines(fileName, lines);
        }
        public override void SerializeBlue4Group(Blue_4.Group group, string fileName)
        {
            if (group == null || string.IsNullOrWhiteSpace(fileName)) return;
            SelectFile(fileName);
            var lines = new List<string> {"Group", group.Name };

            if (group.ManTeams != null)
            {
                foreach (var team in group.ManTeams.Where(t => t != null))
                {
                    lines.AddRange(new[]
                    {"ManTeam",team.Name,string.Join(",", team.Scores)});
                }
            }

            if (group.WomanTeams != null)
            {
                foreach (var team in group.WomanTeams.Where(t => t != null))
                {
                    lines.AddRange(new[]
                    {"WomanTeam",team.Name,string.Join(",", team.Scores)});
                }
            }
            File.WriteAllLines(fileName, lines);
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) where T : class
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var lines = new List<string> {group.GetType().Name,group.Name};

            if (group.Sportsmen != null)
            {
                foreach (var sportsman in group.Sportsmen.Where(s => s != null))
                {
                    lines.AddRange(new[]
                    {"Sportsman",sportsman.Name,sportsman.Surname,sportsman.Place.ToString()});
                }
            }
            File.WriteAllLines(fileName, lines);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || (!File.Exists(fileName))) return null;
            string[] lines = File.ReadAllLines(FilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            int index = 0;

            if (lines[index] == "HumanResponse")
            {
                return new Blue_1.HumanResponse(lines[++index],lines[++index],int.Parse(lines[++index]));
            }
            else 
            {
                return new Blue_1.Response(lines[++index],int.Parse(lines[++index]));
            }

        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;
            string[] lines = File.ReadAllLines(FilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            int index = 0;
            string type = lines[index++];
            string name = lines[index++];
            int bank = int.Parse(lines[index++]);

            Blue_2.WaterJump jump = type switch
            {
                "WaterJump3m" => new Blue_2.WaterJump3m(name, bank),
                "WaterJump5m" => new Blue_2.WaterJump5m(name, bank),
            };

            while (index < lines.Length && lines[index] == "Participant")
            {
                index++;
                string p_Name = lines[index++];
                string p_Surname = lines[index++];
                int[] marks = lines[index++].Split(',').Select(int.Parse).ToArray();
                var participant = new Blue_2.Participant(p_Name, p_Surname);
                participant.Jump(marks);
                jump.Add(participant);
            }
            return jump;

        }
        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return default;
            string[] lines = File.ReadAllLines(FilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            int index = 0;

            string type = lines[index++];
            string name = lines[index++];
            string surname = lines[index++];
            int[] penalties = lines[index++].Split(',').Select(int.Parse).ToArray();

            Blue_3.Participant participant = type switch
            {
                "BasketballPlayer" => new Blue_3.BasketballPlayer(name, surname),
                "HockeyPlayer" => new Blue_3.HockeyPlayer(name, surname),
                _ => new Blue_3.Participant(name, surname),
            };

            foreach (int outmin in penalties)
            {
                participant.PlayMatch(outmin);
            }

            return (T)participant;
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;
            string[] lines = File.ReadAllLines(FilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            int index = 0;
            if (lines[index++] != "Group") return null;
            string name = lines[index++];
            var group = new Blue_4.Group(name);

            while (index < lines.Length)
            {
                switch (lines[index++])
                {
                    case "ManTeam":
                        string manTeam_Name = lines[index++];
                        int[] manScores = lines[index++].Split(',').Select(int.Parse).ToArray();
                        var manTeam = new Blue_4.ManTeam(manTeam_Name);
                        foreach (int point in manScores)
                        {
                            manTeam.PlayMatch(point);
                        }
                        group.Add(manTeam);
                        break;

                    case "WomanTeam":
                        string womanTeam_Name = lines[index++];
                        int[] womanScores = lines[index++].Split(',').Select(int.Parse).ToArray();
                        var womanTeam = new Blue_4.WomanTeam(womanTeam_Name);
                        foreach (int point in womanScores)
                        {
                            womanTeam.PlayMatch(point);
                        }
                        group.Add(womanTeam);
                        break;
                }
            }
            return group;
        }
        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;
            string[] lines = File.ReadAllLines(FilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            int index = 0;
            string type = lines[index++];
            string name = lines[index++];

            Blue_5.Team team = type switch
            {
                "ManTeam" => new Blue_5.ManTeam(name),
                "WomanTeam" => new Blue_5.WomanTeam(name),
                
            };

            while (index < lines.Length && lines[index] == "Sportsman")
            {
                index++;
                string s_Name = lines[index++];
                string s_Surname = lines[index++];
                int place = int.Parse(lines[index++]);
                var sportsman = new Blue_5.Sportsman(s_Name, s_Surname);
                sportsman.SetPlace(place);
                team.Add(sportsman);
            }
            return (T)team;
        }
        public override string Extension => "txt";
    }
}
