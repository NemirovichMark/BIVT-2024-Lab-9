using System.Diagnostics;
using System.Runtime.InteropServices;
using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        private void Serialize<T>(T obj)
        {
            if (string.IsNullOrEmpty(FilePath) || string.IsNullOrEmpty(FolderPath)) return;

            string json = JsonConvert.SerializeObject(obj);

            var Jobj = JObject.Parse(json);
            Jobj["$type"] = obj.GetType().AssemblyQualifiedName;

            string res = Jobj.ToString();

            using (var writer = new StreamWriter(FilePath))
            {
                writer.Write(res);
            }
        }

        private Dictionary<string, JToken> Deserializer()
        {
            if (string.IsNullOrEmpty(FilePath) || string.IsNullOrEmpty(FolderPath)) return null;
            if (!File.Exists(FilePath)) return null;
            var data = File.ReadAllText(FilePath);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(data);

            return dict;
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            SelectFile(fileName);
            Serialize(participant);
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            SelectFile(fileName);
            Serialize(participant);

        }
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            SelectFile(fileName);
            Serialize(student);

        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            Serialize(participant);

        }
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            SelectFile(fileName);
            Serialize(group);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            var dict = Deserializer();
            var name = dict["Name"]?.ToObject<string>();
            int votes = dict["Votes"].ToObject<int>();
            var type = dict["$type"]?.ToObject<string>();
            if (type == typeof(Blue_1.Response).AssemblyQualifiedName)
            {
                var res = new Blue_1.Response(name, votes);
                return res;
            }
            else
            {
                var surname = dict["Surname"]?.ToObject<string>();
                var res = new Blue_1.HumanResponse(name, surname, votes);
                return res;             
            }
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            var dict = Deserializer();
            var type = dict["$type"]?.ToObject<string>();
            if (type == typeof(Blue_2.WaterJump3m).AssemblyQualifiedName)
            {
                var name = dict["Name"]?.ToObject<string>();
                var bank = dict["Bank"].ToObject<int>();
                var res = new Blue_2.WaterJump3m(name, bank);

                var prtsj = dict["Participants"];
                foreach (var p in prtsj)
                {
                    var prt_name = p["Name"]?.ToObject<string>();
                    var prt_surnname = p["Surname"]?.ToObject<string>();

                    var prt = new Blue_2.Participant(prt_name, prt_surnname);

                    var marks = p["Marks"]?.ToObject<int[,]>();

                    for (int i = 0; i < 2; i++)
                    {
                        int[] r = new int[5];
                        for (int j = 0; j < 5; j++)
                        {
                            r[j] = marks[i, j];
                        }
                        prt.Jump(r);
                    }

                    res.Add(prt);
                }
                return res;
            }
            else
            {
                var name = dict["Name"]?.ToObject<string>();
                var bank = dict["Bank"].ToObject<int>();
                var res = new Blue_2.WaterJump5m(name, bank);

                var prtsj = dict["Participants"];
                foreach (var p in prtsj)
                {
                    var prt_name = p["Name"]?.ToObject<string>();
                    var prt_surnname = p["Surname"]?.ToObject<string>();

                    var prt = new Blue_2.Participant(prt_name, prt_surnname);

                    var marks = p["Marks"]?.ToObject<int[,]>();

                    for (int i = 0; i < 2; i++)
                    {
                        int[] r = new int[5];
                        for (int j = 0; j < 5; j++)
                        {
                            r[j] = marks[i, j];
                        }
                        prt.Jump(r);
                    }

                    res.Add(prt);
                }
                return res;
            }
        }
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            var dict = Deserializer();
            var type = dict["$type"]?.ToObject<string>();
            var name = dict["Name"]?.ToObject<string>();
            var surname = dict["Surname"]?.ToObject<string>();
            Blue_3.Participant participant;
            if (type == typeof(Blue_3.BasketballPlayer).AssemblyQualifiedName)
            {
                participant = new Blue_3.BasketballPlayer(name, surname);
            }
            else if (type == typeof(Blue_3.HockeyPlayer).AssemblyQualifiedName)
            {
                participant = new Blue_3.HockeyPlayer(name, surname);
            }
            else
            {
                participant = new Blue_3.Participant(name, surname);
            }
            var penalties = dict["Penalties"]?.ToObject<int[]>();
            if (penalties != null)
            {
                foreach (var penalty in penalties)
                {
                    participant.PlayMatch(penalty);
                }
            }
            
            return (T)(object)participant;
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            var dict = Deserializer();
            
            var groupName = dict["Name"]?.ToObject<string>();
            var group = new Blue_4.Group(groupName);

            // Десериализация мужских команд
            if (dict["ManTeams"] is Newtonsoft.Json.Linq.JArray manTeamsArray)
            {
                foreach (var teamToken in manTeamsArray)
                {
                    if (teamToken is Newtonsoft.Json.Linq.JObject teamObj)
                    {
                        var teamName = teamObj["Name"]?.ToObject<string>();
                        var manTeam = new Blue_4.ManTeam(teamName);

                        if (teamObj["Scores"] is Newtonsoft.Json.Linq.JArray scoresArray)
                        {
                            foreach (var scoreToken in scoresArray)
                            {
                                if (int.TryParse(scoreToken.ToString(), out int score))
                                {
                                    manTeam.PlayMatch(score);
                                }
                            }
                        }

                        group.Add(manTeam);
                    }
                }
            }

            // Десериализация женских команд
            if (dict["WomanTeams"] is Newtonsoft.Json.Linq.JArray womanTeamsArray)
            {
                foreach (var teamToken in womanTeamsArray)
                {
                    if (teamToken is Newtonsoft.Json.Linq.JObject teamObj)
                    {
                        var teamName = teamObj["Name"]?.ToObject<string>();
                        var womanTeam = new Blue_4.WomanTeam(teamName);

                        if (teamObj["Scores"] is Newtonsoft.Json.Linq.JArray scoresArray)
                        {
                            foreach (var scoreToken in scoresArray)
                            {
                                if (int.TryParse(scoreToken.ToString(), out int score))
                                {
                                    womanTeam.PlayMatch(score);
                                }
                            }
                        }

                        group.Add(womanTeam);
                    }
                }
            }

            return group;
        }
        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            var dict = Deserializer();
            
            var type = dict["$type"]?.ToObject<string>();
            var teamName = dict["Name"]?.ToObject<string>();

            // Создаем команду нужного типа
            Blue_5.Team team;
            if (type == typeof(Blue_5.ManTeam).AssemblyQualifiedName)
            {
                team = new Blue_5.ManTeam(teamName);
            }
            else
            {
                team = new Blue_5.WomanTeam(teamName);
            }

            // Десериализация спортсменов
            if (dict["Sportsmen"] is Newtonsoft.Json.Linq.JArray sportsmenArray)
            {
                foreach (var sportsmanToken in sportsmenArray)
                {
                    if (sportsmanToken is Newtonsoft.Json.Linq.JObject sportsmanObj)
                    {
                        var name = sportsmanObj["Name"]?.ToObject<string>();
                        var surname = sportsmanObj["Surname"]?.ToObject<string>();
                        var place = sportsmanObj["Place"]?.ToObject<int>() ?? 0;

                        var sportsman = new Blue_5.Sportsman(name, surname);
                        if (place > 0)
                        {
                            sportsman.SetPlace(place);
                        }

                        team.Add(sportsman);
                    }
                }
            }

            return (T)(object)team;
        }
    }
}