using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName))
                return;

            SelectFile(fileName);

            JObject responseData = JObject.FromObject(participant);
            string participantType = participant.GetType().ToString();
            responseData["Type"] = participantType;

            string jsonString = responseData.ToString();
            File.WriteAllText(FilePath, jsonString);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName))
                return;

            SelectFile(fileName);

            JObject jsonObj = JObject.FromObject(participant);
            jsonObj["Type"] = participant.GetType().ToString();

            File.WriteAllText(FilePath, jsonObj.ToString());
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrWhiteSpace(fileName))
                return;

            SelectFile(fileName);

            JObject jsonRepresentation = JObject.FromObject(student);
            string typeName = student.GetType().ToString();
            jsonRepresentation["Type"] = typeName;

            string jsonText = jsonRepresentation.ToString();
            File.WriteAllText(FilePath, jsonText);
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName))
                return;

            SelectFile(fileName);

            JObject groupJson = JObject.FromObject(participant);
            string outputJson = groupJson.ToString();

            File.WriteAllText(FilePath, outputJson);
        }

        public override void SerializeBlue5Team<T>(T group, string fileName) where T : class
        {
            if (group == null || string.IsNullOrEmpty(fileName))
                return;

            SelectFile(fileName);

            if (!(group is Blue_5.Team team))
                return;

            var sportsmenArray = new JArray();

            if (team.Sportsmen != null)
            {
                foreach (var member in team.Sportsmen)
                {
                    if (member == null)
                        continue;

                    var memberJson = new JObject();
                    memberJson["Name"] = member.Name;
                    memberJson["Surname"] = member.Surname;
                    memberJson["Place"] = member.Place;

                    sportsmenArray.Add(memberJson);
                }
            }

            var teamJson = new JObject
            {
                ["Type"] = team.GetType().Name,
                ["Name"] = team.Name,
                ["Sportsmen"] = sportsmenArray
            };

            File.WriteAllText(FilePath, teamJson.ToString());
        }


        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            SelectFile(fileName);

            string fileContent = File.ReadAllText(FilePath);
            JObject parsedJson = JObject.Parse(fileContent);

            Blue_1.Response result;

            string typeInfo = parsedJson["Type"].ToString();
            string name = parsedJson["Name"].ToString();
            int votes = parsedJson["Votes"].ToObject<int>();

            if (typeInfo == "Lab_7.Blue_1+Response")
            {
                result = new Blue_1.Response(name, votes);
            }
            else
            {
                string surname = parsedJson["Surname"].ToString();
                result = new Blue_1.HumanResponse(name, surname, votes);
            }

            return result;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            SelectFile(fileName);
            string fileContent = File.ReadAllText(FilePath);

            if (string.IsNullOrWhiteSpace(fileContent))
                return null;

            JObject root;
            try
            {
                root = JObject.Parse(fileContent);
            }
            catch
            {
                return null;
            }

            string typeString = root["Type"]?.ToString();
            string eventName = root["Name"]?.ToString();
            int prize = root["Bank"]?.ToObject<int>() ?? 0;

            Blue_2.WaterJump jumpEvent = null;

            if (typeString == "Lab_7.Blue_2+WaterJump3m")
                jumpEvent = new Blue_2.WaterJump3m(eventName, prize);
            else if (typeString == "Lab_7.Blue_2+WaterJump5m")
                jumpEvent = new Blue_2.WaterJump5m(eventName, prize);
            else
                return null;

            JToken participantsToken = root["Participants"];
            if (participantsToken == null || !participantsToken.HasValues)
                return jumpEvent;

            foreach (var pToken in participantsToken)
            {
                string firstName = pToken["Name"]?.ToString();
                string lastName = pToken["Surname"]?.ToString();

                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                    continue;

                var participant = new Blue_2.Participant(firstName, lastName);

                JToken marksToken = pToken["Marks"];
                if (marksToken is JArray marksArray && marksArray.Count >= 2)
                {
                    for (int attemptIndex = 0; attemptIndex < 2; attemptIndex++)
                    {
                        if (marksArray[attemptIndex] is JArray scoresArray)
                        {
                            int[] scores = new int[5];
                            for (int i = 0; i < scores.Length && i < scoresArray.Count; i++)
                            {
                                scores[i] = scoresArray[i].ToObject<int>();
                            }
                            participant.Jump(scores);
                        }
                    }
                }

                jumpEvent.Add(participant);
            }

            return jumpEvent;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return default;

            SelectFile(fileName);
            string content = File.ReadAllText(FilePath);
            JObject data = JObject.Parse(content);

            string typeName = data["Type"].ToString();
            string name = data["Name"].ToString();
            string surname = data["Surname"].ToString();

            Blue_3.Participant player;

            switch (typeName)
            {
                case "Lab_7.Blue_3+BasketballPlayer":
                    player = new Blue_3.BasketballPlayer(name, surname);
                    break;
                case "Lab_7.Blue_3+HockeyPlayer":
                    player = new Blue_3.HockeyPlayer(name, surname);
                    break;
                default:
                    player = new Blue_3.Participant(name, surname);
                    break;
            }

            int[] matchPenalties = data["Penalties"].ToObject<int[]>();
            foreach (int penalty in matchPenalties)
            {
                player.PlayMatch(penalty);
            }

            return (T)(object)player;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            SelectFile(fileName);
            string jsonContent = File.ReadAllText(FilePath);
            JObject data = JObject.Parse(jsonContent);

            var groupName = data["Name"]?.ToString();
            var group = new Blue_4.Group(groupName);

            var manTeams = data["ManTeams"]?.ToObject<Blue_4.ManTeam[]>();
            if (manTeams != null)
            {
                foreach (var team in manTeams)
                {
                    if (team == null) break;
                    var scores = data["ManTeams"]?[Array.IndexOf(manTeams, team)]?["Scores"]?.ToObject<int[]>();
                    if (scores != null)
                    {
                        foreach (var score in scores)
                            team.PlayMatch(score);
                    }
                }
                group.Add(manTeams);
            }

            var womanTeams = data["WomanTeams"]?.ToObject<Blue_4.WomanTeam[]>();
            if (womanTeams != null)
            {
                foreach (var team in womanTeams)
                {
                    if (team == null) break;
                    var scores = data["WomanTeams"]?[Array.IndexOf(womanTeams, team)]?["Scores"]?.ToObject<int[]>();
                    if (scores != null)
                    {
                        foreach (var score in scores)
                            team.PlayMatch(score);
                    }
                }
                group.Add(womanTeams);
            }

            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            SelectFile(fileName);

            string content = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(content))
                return null;

            JObject jsonObj = JObject.Parse(content);

            var teamType = jsonObj["Type"]?.ToString();
            var teamName = jsonObj["Name"]?.ToString();

            Blue_5.Team teamInstance = null;

            switch (teamType)
            {
                case "ManTeam":
                    teamInstance = new Blue_5.ManTeam(teamName);
                    break;
                case "WomanTeam":
                    teamInstance = new Blue_5.WomanTeam(teamName);
                    break;
                default:
                    return null;
            }

            var sportsmenToken = jsonObj["Sportsmen"] as JArray;
            if (sportsmenToken != null)
            {
                foreach (var sportsmanToken in sportsmenToken)
                {
                    var name = sportsmanToken["Name"]?.ToString();
                    var surname = sportsmanToken["Surname"]?.ToString();
                    int place = sportsmanToken["Place"]?.ToObject<int>() ?? 0;

                    if (name != null && surname != null)
                    {
                        var sportsman = new Blue_5.Sportsman(name, surname);
                        sportsman.SetPlace(place);
                        teamInstance.Add(sportsman);
                    }
                }
            }

            return teamInstance as T;
        }

    }
}
