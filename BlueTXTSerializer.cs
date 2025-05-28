using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Lab_7;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";


        


        

        private void ReadPropertiesFromFile(object obj, string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split('=');
                    if (parts.Length != 2) continue;

                    var propName = parts[0];
                    var propValue = parts[1];

                    var property = obj.GetType().GetProperty(propName);
                    if (property != null && property.CanWrite)
                    {
                        try
                        {
                            var convertedValue = propValue == "null" ? null :
                                Convert.ChangeType(propValue,
                                    Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                            property.SetValue(obj, convertedValue);
                        }
                        catch
                        {

                            continue;
                        }
                    }
                }
            }
        }

        private void WriteValue(StreamWriter writer, string key, object? value, int indent = 0)
        {
            var indentation = new string(' ', indent * 2);
            if (value == null)
            {
                writer.WriteLine($"{indentation}{key}: null");
                return;
            }

            if (value is Array array)
            {
                writer.WriteLine($"{indentation}{key}:");
                foreach (var item in array)
                {
                    WriteValue(writer, "-", item, indent + 1);
                }
                return;
            }

            if (value is IEnumerable<object> enumerable && !(value is string))
            {
                writer.WriteLine($"{indentation}{key}:");
                foreach (var item in enumerable)
                {
                    WriteValue(writer, "-", item, indent + 1);
                }
                return;
            }

            writer.WriteLine($"{indentation}{key}: {value}");
        }

        public override void SerializeBlue1Response(Blue_1.Response? participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                WriteValue(writer, "Type", participant.GetType().Name);
                WriteValue(writer, "Name", participant.Name);
                WriteValue(writer, "Votes", participant.Votes);
                if (participant is Blue_1.HumanResponse humanResponse)
                {
                    WriteValue(writer, "Surname", humanResponse.Surname);
                }
            }
        }

        public override Blue_1.Response? DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);

            var lines = File.ReadAllLines(FilePath);
            string? type = null, name = null, surname = null;
            int votes = 0;

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ": " }, StringSplitOptions.None);
                if (parts.Length != 2) continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "Type":
                        type = value;
                        break;
                    case "Name":
                        name = value;
                        break;
                    case "Surname":
                        surname = value;
                        break;
                    case "Votes":
                        int.TryParse(value, out votes);
                        break;
                }
            }

            if (string.IsNullOrEmpty(name)) return null;

            Blue_1.Response response;
            if (type == "HumanResponse" && !string.IsNullOrEmpty(surname))
            {
                response = new Blue_1.HumanResponse(name, surname);
                // Восстанавливаем голоса через временный массив
                var tempResponses = Enumerable.Repeat(response, votes).ToArray();
                ((Blue_1.HumanResponse)response).CountVotes(tempResponses);
            }
            else
            {
                response = new Blue_1.Response(name);
                // Восстанавливаем голоса через временный массив
                var tempResponses = Enumerable.Repeat(response, votes).ToArray();
                response.CountVotes(tempResponses);
            }

            return response;
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump? wj, string fileName)
        {
            if (wj == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                WriteValue(writer, "Type", wj.GetType().Name);
                WriteValue(writer, "Name", wj.Name);
                WriteValue(writer, "Bank", wj.Bank);

                if (wj.Participants?.Any() == true)
                {
                    writer.Write("Participants::");
                    foreach (var p in wj.Participants)
                    {
                        SerializeBlue2Participant(p, writer);
                    }
                }
            }
        }

        public override Blue_2.WaterJump? DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);

            var lines = File.ReadAllLines(FilePath);
            string? type = null, name = null;
            int bank = 0;
            var participants = new List<Blue_2.Participant>();

            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("Participants::"))
                {
                    var participantsData = line.Substring(line.IndexOf("::") + 2);
                    DeserializeBlue2Participants(participants, participantsData);
                    continue;
                }

                var parts = line.Split(new[] { ": " }, StringSplitOptions.None);
                if (parts.Length != 2) continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "Type":
                        type = value;
                        break;
                    case "Name":
                        name = value;
                        break;
                    case "Bank":
                        if (int.TryParse(value, out int bankValue))
                        {
                            bank = bankValue;
                        }
                        break;
                }
            }

            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(name)) return null;

            var waterJump = type switch
            {
                "WaterJump3m" => new Blue_2.WaterJump3m(name, bank),
                "WaterJump5m" => new Blue_2.WaterJump5m(name, bank),
                _ => (Blue_2.WaterJump?)null
            };

            if (waterJump == null) return null;

            foreach (var participant in participants)
            {
                waterJump.Add(participant);
            }

            return waterJump;
        }

        private void DeserializeBlue2Participants(List<Blue_2.Participant> participants, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            var participantStrings = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pStr in participantStrings)
            {
                if (pStr.Length <= 2) continue;
                var participantData = pStr.Substring(1, pStr.Length - 2);
                var fields = participantData.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string? name = null, surname = null;
                var marks = new List<int[]>();

                foreach (var field in fields)
                {
                    var fieldParts = field.Split(':');
                    if (fieldParts.Length != 2) continue;

                    var fieldName = fieldParts[0];
                    var fieldValue = fieldParts[1];

                    switch (fieldName)
                    {
                        case "Name":
                            name = fieldValue;
                            break;
                        case "Surname":
                            surname = fieldValue;
                            break;
                        case "Marks":
                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                var markSets = fieldValue.Split("][");
                                foreach (var markSet in markSets)
                                {
                                    var cleanMarkSet = markSet.Trim('[', ']');
                                    if (!string.IsNullOrEmpty(cleanMarkSet))
                                    {
                                        var markValues = cleanMarkSet.Split(',')
                                            .Where(m => !string.IsNullOrWhiteSpace(m))
                                            .Select(m => int.Parse(m.Trim()))
                                            .ToArray();
                                        if (markValues.Length > 0)
                                        {
                                            marks.Add(markValues);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(surname))
                {
                    var participant = new Blue_2.Participant(name, surname);
                    foreach (var markSet in marks)
                    {
                        participant.Jump(markSet);
                    }
                    participants.Add(participant);
                }
            }
        }

        public override void SerializeBlue3Participant<T>(T? participant, string fileName) where T : class
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                if (participant is Blue_3.Participant baseParticipant)
                {
                    string typeName = participant switch
                    {
                        Blue_3.BasketballPlayer => "BasketballPlayer",
                        Blue_3.HockeyPlayer => "HockeyPlayer",
                        _ => "Participant"
                    };

                    WriteValue(writer, "Type", typeName);
                    WriteValue(writer, "Name", baseParticipant.Name);
                    WriteValue(writer, "Surname", baseParticipant.Surname);
                    WriteValue(writer, "Penalties", string.Join(",", baseParticipant.Penalties));
                }
            }
        }

        public override T? DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            if (string.IsNullOrEmpty(fileName)) return default;
            SelectFile(fileName);

            try
            {
                var lines = File.ReadAllLines(FilePath);
                string type = "", name = "", surname = "";
                int[] penalties = Array.Empty<int>();

                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { ": " }, StringSplitOptions.None);
                    if (parts.Length != 2) continue;

                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    switch (key)
                    {
                        case "Type":
                            type = value;
                            break;
                        case "Name":
                            name = value;
                            break;
                        case "Surname":
                            surname = value;
                            break;
                        case "Penalties":
                            penalties = value.Split(',')
                                           .Where(s => !string.IsNullOrEmpty(s))
                                           .Select(int.Parse)
                                           .ToArray();
                            break;
                    }
                }

                if (string.IsNullOrEmpty(name)) return default;

                Blue_3.Participant participant = type switch
                {
                    "BasketballPlayer" => new Blue_3.BasketballPlayer(name, surname),
                    "HockeyPlayer" => new Blue_3.HockeyPlayer(name, surname),
                    _ => new Blue_3.Participant(name, surname)
                };

                foreach (var penalty in penalties)
                {
                    participant.PlayMatch(penalty);
                }

                return (T)(object)participant;
            }
            catch
            {
                return default;
            }
        }



        public override void SerializeBlue4Group(Blue_4.Group? group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                WriteValue(writer, "Name", group.Name);
                SerializeManOrFemaleTeams(group, "ManTeams", writer);
                SerializeManOrFemaleTeams(group, "WomanTeams", writer);
            }
        }

        public override Blue_4.Group? DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);

            var lines = File.ReadAllLines(FilePath);
            string? groupName = null;
            var manTeams = new List<Blue_4.ManTeam>();
            var womanTeams = new List<Blue_4.WomanTeam>();

            string? currentTeamType = null;
            string? teamName = null;
            List<int>? teamScores = null;

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine == "ManTeams:")
                {
                    currentTeamType = "ManTeams";
                    teamName = null; teamScores = null;
                    continue;
                }
                else if (trimmedLine == "WomanTeams:")
                {
                    currentTeamType = "WomanTeams";
                    teamName = null; teamScores = null;
                    continue;
                }

                var parts = line.Split(new[] { ": " }, 2, StringSplitOptions.None);
                if (parts.Length != 2) continue;

                string originalKeyPart = parts[0];
                string key = originalKeyPart.Trim();
                string value = parts[1].Trim();

                if (key == "Name" && !originalKeyPart.StartsWith(" ") && !originalKeyPart.StartsWith("  -"))
                {
                    groupName = value;
                    continue;
                }

                if (originalKeyPart.StartsWith("    "))
                {
                    key = originalKeyPart.TrimStart();
                    switch (key)
                    {
                        case "Name":
                            teamName = value;
                            break;
                        case "Scores":
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                teamScores = value.Split(',')
                                    .Where(s => !string.IsNullOrWhiteSpace(s))
                                    .Select(s => int.Parse(s.Trim()))
                                    .ToList();
                            }
                            break;
                    }

                    if (teamName != null && teamScores != null && currentTeamType != null)
                    {
                        if (currentTeamType == "ManTeams")
                        {
                            var team = new Blue_4.ManTeam(teamName);
                            foreach (var score in teamScores) team.PlayMatch(score);
                            manTeams.Add(team);
                        }
                        else if (currentTeamType == "WomanTeams")
                        {
                            var team = new Blue_4.WomanTeam(teamName);
                            foreach (var score in teamScores) team.PlayMatch(score);
                            womanTeams.Add(team);
                        }
                        teamName = null; teamScores = null;
                    }
                }
                else if (originalKeyPart.Trim().StartsWith("- Type"))
                {
                    teamName = null;
                    teamScores = null;
                }
            }

            if (string.IsNullOrEmpty(groupName)) return null;

            var group = new Blue_4.Group(groupName);
            if (manTeams.Any()) group.Add(manTeams.ToArray());
            if (womanTeams.Any()) group.Add(womanTeams.ToArray());

            return group;
        }

        public override void SerializeBlue5Team<T>(T? team, string fileName) where T : class
        {
            if (team == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                WriteValue(writer, "Type", team.GetType().Name);
                WriteValue(writer, "Name", team.Name);
                if (team is Blue_5.Team blueTeam)
                {
                    SerializeBlue5Sportsmen(blueTeam, writer);
                }
            }
        }

        public override T? DeserializeBlue5Team<T>(string fileName) where T : class
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);

            var lines = File.ReadAllLines(FilePath);
            string? type = null, teamEntityName = null;
            var sportsmen = new List<Blue_5.Sportsman>();

            string? sportsmanName = null, sportsmanSurname = null;
            int? place = null;
            bool readingSportsmen = false;

            foreach (var line in lines)
            {
                if (line.Trim() == "Sportsmen:")
                {
                    readingSportsmen = true;
                    sportsmanName = null; sportsmanSurname = null; place = null;
                    continue;
                }

                var parts = line.Split(new[] { ": " }, StringSplitOptions.None);
                if (parts.Length != 2) continue;

                string originalKeyPart = parts[0];
                string key = originalKeyPart.Trim();
                string value = parts[1].Trim();

                if (!readingSportsmen)
                {
                    switch (key)
                    {
                        case "Type":
                            type = value;
                            break;
                        case "Name":
                            teamEntityName = value;
                            break;
                    }
                }
                else
                {
                    if (originalKeyPart.Trim().StartsWith("- Type"))
                    {
                        sportsmanName = null;
                        sportsmanSurname = null;
                        place = null;
                    }
                    else if (originalKeyPart.StartsWith("    "))
                    {
                        key = originalKeyPart.TrimStart();
                        switch (key)
                        {
                            case "Name":
                                sportsmanName = value;
                                break;
                            case "Surname":
                                sportsmanSurname = value;
                                break;
                            case "Place":
                                if (int.TryParse(value, out int placeValue))
                                {
                                    place = placeValue;
                                }
                                break;
                        }

                        if (sportsmanName != null && sportsmanSurname != null && place.HasValue)
                        {
                            var sportsman = new Blue_5.Sportsman(sportsmanName, sportsmanSurname);
                            sportsman.SetPlace(place.Value);
                            sportsmen.Add(sportsman);
                            sportsmanName = null; sportsmanSurname = null; place = null;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(teamEntityName)) return null;

            T? instance = null;
            if (type == "ManTeam")
            {
                instance = new Blue_5.ManTeam(teamEntityName) as T;
            }
            else if (type == "WomanTeam")
            {
                instance = new Blue_5.WomanTeam(teamEntityName) as T;
            }

            if (instance is Blue_5.Team teamInstance)
            {
                foreach (var sportsman in sportsmen)
                {
                    teamInstance.Add(sportsman);
                }
            }

            return instance;
        }

        private void SerializeBlue2Participant(Blue_2.Participant participant, StreamWriter writer)
        {
            writer.Write(";{");
            writer.Write($"Name:{participant.Name} ");
            writer.Write($"Surname:{participant.Surname} ");
            if (participant.Marks != null)
            {
                writer.Write("Marks:");
                for (int i = 0; i < participant.Marks.GetLength(0); i++)
                {
                    writer.Write("[");
                    for (int j = 0; j < participant.Marks.GetLength(1); j++)
                    {
                        if (j > 0) writer.Write(",");
                        writer.Write(participant.Marks[i, j]);
                    }
                    writer.Write("]");
                }
            }
            writer.Write("}");
        }

        private void SerializeManOrFemaleTeams(Blue_4.Group group, string teamType, StreamWriter writer)
        {
            if (group == null) return;

            Blue_4.Team[]? teams = teamType == "ManTeams" ? group.ManTeams : group.WomanTeams;
            if (teams != null && teams.Length > 0)
            {
                writer.WriteLine($"{teamType}:");
                foreach (var team in teams)
                {
                    if (team == null) continue;
                    writer.WriteLine($"  - Type: {team.GetType().Name}");
                    writer.WriteLine($"    Name: {team.Name}");
                    if (team.Scores != null && team.Scores.Length > 0)
                    {
                        writer.WriteLine($"    Scores: {string.Join(",", team.Scores)}");
                    }
                }
            }
        }

        private void SerializeBlue5Sportsmen(Blue_5.Team team, StreamWriter writer)
        {
            if (team.Sportsmen?.Any() != true) return;

            writer.WriteLine("Sportsmen:");
            foreach (var sportsman in team.Sportsmen)
            {
                if (sportsman == null) continue;
                writer.WriteLine($"  - Type: {sportsman.GetType().Name}");
                writer.WriteLine($"    Name: {sportsman.Name}");
                writer.WriteLine($"    Surname: {sportsman.Surname}");
                writer.WriteLine($"    Place: {sportsman.Place}");
            }
        }

        private Blue_2.Participant? CreateParticipant(string? name, string? surname)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname)) return null;
            return new Blue_2.Participant(name, surname);
        }
    }
}
