using Lab_7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Lab_9
{
    public class BlueTXTSerializer: BlueSerializer
    {
        public override string Extension => "txt";
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            var content = new StringBuilder()
                .AppendLine($"Type: {participant.GetType().Name}")
                .AppendLine($"Name: {participant.Name}")
                .AppendLine($"Votes: {participant.Votes}");

            if (participant is Blue_1.HumanResponse human)
            {
                content.AppendLine($"Surname: {human.Surname}");
            }

            File.WriteAllText(FilePath, content.ToString());
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName))
                return;

            var lines = new List<string>();

            string firstLine = $"{participant.Name} {participant.Bank}";

            if (participant is Blue_2.WaterJump3m)
                firstLine += " 3";
            else
                firstLine += " 5";

            lines.Add(firstLine);

            foreach (var p in participant.Participants)
            {
                var line = new StringBuilder();
                line.Append($"{p.Name} {p.Surname}");

                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        line.Append($" {p.Marks[j, k]}");
                    }
                }
                lines.Add(line.ToString());
            }

            SelectFile(fileName);
            using (var writer = new StreamWriter(FilePath))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        public override void SerializeBlue3Participant<T>(T data, string fileName) where T : class
        {
            if (data == null || string.IsNullOrWhiteSpace(fileName))
                return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);

            using (StreamWriter writer = new StreamWriter(FilePath, append: true))
            {
                writer.WriteLine($"Type: {data.GetType().Name}");

                var type = data.GetType();
                var nameProp = type.GetProperty("Name");
                var surnameProp = type.GetProperty("Surname");
                var penaltiesProp = type.GetProperty("Penalties");

                if (nameProp != null && surnameProp != null && penaltiesProp != null)
                {
                    writer.WriteLine($"Name: {nameProp.GetValue(data)}");
                    writer.WriteLine($"Surname: {surnameProp.GetValue(data)}");

                    var penalties = penaltiesProp.GetValue(data) as IEnumerable<int>;
                    if (penalties != null)
                    {
                        writer.WriteLine("Penalties: [" + string.Join(", ", penalties) + "]");
                    }
                }

                if (data is Blue_4.Group group)
                {
                    writer.WriteLine($"Group: {group.Name}");

                    string FormatTeam(Blue_4.Team team)
                    {
                        string scores = team?.Scores != null && team.Scores.Length > 0
                            ? string.Join(", ", team.Scores)
                            : "no scores";
                        return $"{{Name: {team?.Name}, Scores: [{scores}]}}";
                    }

                    writer.Write("ManTeams: <");
                    if (group.ManTeams != null)
                    {
                        var manTeamsFormatted = group.ManTeams
                            .Where(t => t != null)
                            .Select(FormatTeam);
                        writer.Write(string.Join("; ", manTeamsFormatted));
                    }
                    writer.WriteLine(">");

                    writer.Write("WomanTeams: <");
                    if (group.WomanTeams != null)
                    {
                        var womanTeamsFormatted = group.WomanTeams
                            .Where(t => t != null)
                            .Select(FormatTeam);
                        writer.Write(string.Join("; ", womanTeamsFormatted));
                    }
                    writer.WriteLine(">");
                }
            }
        }


        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName))
                return;

            SelectFile(fileName);
            var filePath = FilePath;
            File.WriteAllText(filePath, string.Empty);

            using (var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine($"Name:: {participant.Name}");

                foreach (var teams_gender in new[] { "ManTeams", "WomanTeams" })
                {
                    writer.Write($"{teams_gender}:: ");

                    StringBuilder teamsBuilder = new StringBuilder("<");
                    bool hasTeams = false;

                    var teams = teams_gender == "ManTeams" ? participant.ManTeams : participant.WomanTeams;

                    foreach (var team in teams)
                    {
                        if (team != null)
                        {
                            teamsBuilder.Append("{");
                            teamsBuilder.Append($"Name:{team.Name} ");
                            teamsBuilder.Append("Scores:[");

                            if (team.Scores != null && team.Scores.Length > 0)
                            {
                                foreach (int score in team.Scores)
                                {
                                    teamsBuilder.Append($"{score},");
                                }
                                teamsBuilder.Length--; // удалить последнюю запятую
                            }

                            teamsBuilder.Append("]");
                            teamsBuilder.Append("};");
                            hasTeams = true;
                        }
                    }

                    if (hasTeams)
                    {
                        teamsBuilder.Length--; // удалить последнюю точку с запятой
                    }

                    teamsBuilder.Append(">");
                    writer.WriteLine(teamsBuilder.ToString());
                }
            }
        }


        public override void SerializeBlue5Team<T>(T team, string fileName)
        {
            if (team == null || string.IsNullOrWhiteSpace(fileName))
                return;

            string teamTypeLine;

            var teamType = team.GetType();
            if (teamType == typeof(Blue_5.ManTeam))
                teamTypeLine = "man";
            else if (teamType == typeof(Blue_5.WomanTeam))
                teamTypeLine = "woman";
            else
                return;

            var builder = new System.Text.StringBuilder();
            builder.AppendLine(teamTypeLine);
            builder.AppendLine(team.Name);

            var sportsmen = team.Sportsmen;
            if (sportsmen != null)
            {
                foreach (var athlete in sportsmen)
                {
                    if (athlete == null)
                        continue;

                    builder.Append(athlete.Name)
                           .Append(' ')
                           .Append(athlete.Surname)
                           .Append(' ')
                           .AppendLine(athlete.Place.ToString());
                }
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, builder.ToString());
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);

            var lines = File.ReadAllLines(FilePath);
            var data = new Dictionary<string, string>();

            foreach (string line in lines)
            {
                var separatorIndex = line.IndexOf(':');
                if (separatorIndex > -1)
                {
                    string key = line.Substring(0, separatorIndex).Trim();
                    string value = line.Substring(separatorIndex + 1).Trim();
                    data[key] = value;
                }
            }

            if (!data.ContainsKey("Type") || !data.ContainsKey("Name") || !data.ContainsKey("Votes"))
                return null;

            string type = data["Type"];
            string name = data["Name"];
            int votes = int.TryParse(data["Votes"], out var v) ? v : 0;

            if (type == "Response")
            {
                return new Blue_1.Response(name, votes);
            }

            string surname = data.ContainsKey("Surname") ? data["Surname"] : string.Empty;
            return new Blue_1.HumanResponse(name, surname, votes);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            SelectFile(fileName);
            using (var reader = new StreamReader(FilePath))
            {
                string firstLine = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(firstLine))
                    return null;

                string[] parts = firstLine.Split(' ');
                if (parts.Length < 3)
                    return null;

                Blue_2.WaterJump waterJump;
                if (parts[2] == "3")
                    waterJump = new Blue_2.WaterJump3m(parts[0], int.Parse(parts[1]));
                else
                    waterJump = new Blue_2.WaterJump5m(parts[0], int.Parse(parts[1]));

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var participantData = line.Split(' ');
                    if (participantData.Length < 12)
                        continue;  // или throw исключение, если считаете, что формат обязательно должен быть корректным

                    var participant = new Blue_2.Participant(participantData[0], participantData[1]);

                    int[] jumps = new int[5];

                    // Первая серия прыжков
                    for (int i = 2; i < 7; i++)
                        jumps[i - 2] = int.Parse(participantData[i]);
                    participant.Jump(jumps);

                    // Вторая серия прыжков
                    for (int i = 7; i < 12; i++)
                        jumps[i - 7] = int.Parse(participantData[i]);
                    participant.Jump(jumps);

                    waterJump.Add(participant);
                }

                return waterJump;
            }
        }


        private Blue_4.ManTeam[] DeserializeManTeams(string[] teams)
        {
            var result = new List<Blue_4.ManTeam>();

            foreach (var team in teams)
            {
                if (string.IsNullOrWhiteSpace(team) || team.Length < 2)
                    continue;

                var trimmed = team.Substring(1, team.Length - 2); // убираем фигурные скобки
                var keyValuePairs = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var fields = new Dictionary<string, string>();

                foreach (var pair in keyValuePairs)
                {
                    var parts = pair.Split(':');
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        fields[key] = value;
                    }
                }

                if (!fields.ContainsKey("Name") || !fields.ContainsKey("Scores"))
                    continue;

                var teamName = fields["Name"];
                var scoreRaw = fields["Scores"].Trim('[', ']');
                var scoreValues = scoreRaw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(int.Parse);

                var manTeam = new Blue_4.ManTeam(teamName);
                foreach (var score in scoreValues)
                {
                    manTeam.PlayMatch(score);
                }

                result.Add(manTeam);
            }

            return result.ToArray();
        }


        private Blue_4.WomanTeam[] DeserializeWomanTeams(string[] teams)
        {
            var result = new List<Blue_4.WomanTeam>();

            foreach (var team in teams)
            {
                if (string.IsNullOrWhiteSpace(team) || team.Length < 2)
                    continue;

                var content = team.Substring(1, team.Length - 2); // убираем фигурные скобки
                var keyValuePairs = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var fields = new Dictionary<string, string>();

                foreach (var pair in keyValuePairs)
                {
                    var parts = pair.Split(':');
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        fields[key] = value;
                    }
                }

                if (!fields.ContainsKey("Name") || !fields.ContainsKey("Scores"))
                    continue;

                var name = fields["Name"];
                var scoresRaw = fields["Scores"].Trim('[', ']');
                var scores = scoresRaw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(int.Parse);

                var womanTeam = new Blue_4.WomanTeam(name);
                foreach (var score in scores)
                {
                    womanTeam.PlayMatch(score);
                }

                result.Add(womanTeam);
            }

            return result.ToArray();
        }

        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            SelectFile(fileName);

            if (!File.Exists(FilePath))
                return null;

            string[] lines = File.ReadLines(FilePath).Take(4).ToArray();
            var dataMap = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                var parts = line.Split(':', 2);
                if (parts.Length == 2)
                    dataMap[parts[0].Trim()] = parts[1].Trim();
            }

            if (!dataMap.TryGetValue("Penalties", out string rawPenalties))
                return null;

            rawPenalties = rawPenalties.Trim('[', ']');
            var penaltyValues = rawPenalties
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => int.TryParse(p.Trim(), out var val) ? val : 0)
                .ToArray();

            Blue_3.Participant instance = dataMap["Type"] switch
            {
                "BasketballPlayer" => new Blue_3.BasketballPlayer(dataMap["Name"], dataMap["Surname"]),
                "HockeyPlayer" => new Blue_3.HockeyPlayer(dataMap["Name"], dataMap["Surname"]),
                _ => new Blue_3.Participant(dataMap["Name"], dataMap["Surname"])
            };

            foreach (var penalty in penaltyValues)
                instance.PlayMatch(penalty);

            return instance as T;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            var dict = new Dictionary<string, string>();

            using var reader = new StreamReader(File.OpenRead(FilePath));
            for (int i = 0; i < 3; i++)
            {
                var line = reader.ReadLine();
                if (line?.Contains("::") == true)
                {
                    var parts = line.Split(new[] { "::" }, StringSplitOptions.None);
                    if (parts.Length == 2)
                        dict[parts[0].Trim()] = parts[1].Trim();
                }
            }

            var group = new Blue_4.Group(dict["Name"]);

            string[] manTeamsRaw = dict["ManTeams"][1..^1].Split(';');
            string[] womanTeamsRaw = dict["WomanTeams"][1..^1].Split(';');

            var manTeams = manTeamsRaw.Length > 0 ? DeserializeManTeams(manTeamsRaw) : Array.Empty<Blue_4.ManTeam>();
            var womanTeams = womanTeamsRaw.Length > 0 ? DeserializeWomanTeams(womanTeamsRaw) : Array.Empty<Blue_4.WomanTeam>();

            if (manTeams.Length > 0) group.Add(manTeams);
            if (womanTeams.Length > 0) group.Add(womanTeams);

            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return default;

            SelectFile(fileName);
            if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
                return default;

            string content = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(content))
                return default;

            var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
                return default;

            Blue_5.Team team;
            switch (lines[0].Trim().ToLower())
            {
                case "man":
                    team = new Blue_5.ManTeam(lines[1]);
                    break;
                case "woman":
                    team = new Blue_5.WomanTeam(lines[1]);
                    break;
                default:
                    return default;
            }

            for (int i = 2; i < lines.Length; i++)
            {
                var parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                    continue;

                var sportsman = new Blue_5.Sportsman(parts[0], parts[1]);

                for (int j = 2; j < parts.Length; j++)
                {
                    if (int.TryParse(parts[j], out int place))
                        sportsman.SetPlace(place);
                }

                team.Add(sportsman);
            }

            if (team is T result)
                return result;

            return default;
        }

    }
}
