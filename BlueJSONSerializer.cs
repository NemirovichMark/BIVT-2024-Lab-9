using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Blue_2;
using static Lab_7.Blue_3;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            object objToSerialize;
            switch (participant)
            {
                case Blue_1.HumanResponse man:
                    objToSerialize = new
                    {
                        Type = participant.GetType().Name,
                        participant.Name,
                        participant.Votes,
                        man.Surname
                    };
                    break;

                default://just response
                    objToSerialize = new
                    {
                        Type = participant.GetType().Name,
                        participant.Name,
                        participant.Votes
                    };
                    break;
            }
            string json = JsonConvert.SerializeObject(objToSerialize, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            var participantsData = new List<object>();
            if (participant.Participants != null)
            {
                foreach (var part in participant.Participants)
                {
                    
                        participantsData.Add(new
                        {
                            part.Name,
                            part.Surname,
                            Marks = ConvertMarks(part.Marks),
                            part.TotalScore
                        });
                    
                }
            }
            var serializedData = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                participant.Bank,
                Participants = participantsData.ToArray(),
                Prize = participant.Prize
            };
            string json = JsonConvert.SerializeObject(serializedData, Formatting.Indented);
            File.WriteAllText(fileName, json);
            
        }
        private static int[][] ConvertMarks(int[,] marks)
        {
            if (marks == null || marks.GetLength(0) == 0 || marks.GetLength(1) == 0) return new int[0][];
            int rows = marks.GetLength(0);
            int cols = marks.GetLength(1);
            int[][] array = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    array[i][j] = marks[i, j];
                }
            }
            return array;
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) where T : class
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            
            var objToSerialize = new
            {
                Type = student.GetType().Name,
                student.Name,
                student.Surname,
                Penalties = student.Penalties,
                student.Total,
                IsExpelled = student.IsExpelled
            };
            string json = JsonConvert.SerializeObject(objToSerialize, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }
        public override void SerializeBlue4Group(Blue_4.Group group, string fileName)
        {
            if (group == null || string.IsNullOrWhiteSpace(fileName)) return;
            SelectFile(fileName);
            var teamsData = new List<object>();
            if (group.ManTeams != null)
            {
                teamsData.AddRange(
                    group.ManTeams.Where(team => team != null)
                        .Select(team => new
                        {
                            TeamType = "ManTeam",
                            TeamName = team.Name,
                            Scores = team.Scores ?? Array.Empty<int>()
                        })
                );
            }
            if (group.WomanTeams != null)
            {
                teamsData.AddRange(
                    group.WomanTeams.Where(team => team != null)
                        .Select(team => new
                        {
                            TeamType = "WomanTeam",
                            TeamName = team.Name,
                            Scores = team.Scores ?? Array.Empty<int>()
                        })
                );
            }
            var objToSerialize = new
            {
                GroupName = group.Name,
                Teams = teamsData.ToArray()
            };
            string json = JsonConvert.SerializeObject(objToSerialize, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) where T : class
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var inf = new
            {
                TeamType = group.GetType().Name,
                TeamName = group.Name,
                Sportsmen = group.Sportsmen?.Where(x => x != null).Select(x => new {x.Name,x.Surname,x.Place}).ToArray(),
            };
            string json = JsonConvert.SerializeObject(inf, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;
            string json = File.ReadAllText(fileName);
            dynamic data = JsonConvert.DeserializeObject(json);
            if (data.Type == "HumanResponse")
            {
                return new Blue_1.HumanResponse((string)data.Name,(string)data.Surname,(int)data.Votes);
            }
            else
            {
                return new Blue_1.Response((string)data.Name,(int)data.Votes);
            }
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;
            string json = File.ReadAllText(fileName);
            JObject obj = JObject.Parse(json);
            string type = obj["Type"]?.ToString();
            string name = obj["Name"]?.ToString();
            int bank = obj["Bank"] != null ? (int)obj["Bank"] : 0;
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(name)) return null;
            Blue_2.WaterJump jump = type switch
            {
                "WaterJump3m" => new Blue_2.WaterJump3m(name, bank),
                _ => new Blue_2.WaterJump5m(name, bank)
            };

            JArray participantsArray = obj["Participants"] as JArray;
            if (participantsArray == null) return jump;

            foreach (var participantToken in participantsArray)
            {
                string participantName = (string)participantToken["Name"];
                string participantSurname = (string)participantToken["Surname"];

                if (!string.IsNullOrEmpty(participantName) && !string.IsNullOrEmpty(participantSurname))
                {
                    var participant = new Blue_2.Participant(participantName, participantSurname);

                    JArray marksArray = participantToken["Marks"] as JArray;
                    if (marksArray != null && marksArray.Count <= 2)
                    {
                        for (int i = 0; i < marksArray.Count; i++)
                        {
                            JArray mark = marksArray[i] as JArray;
                            if (mark != null && mark.Count == 5)
                            {
                                int[] marks = mark.Select(m => m.ToObject<int>()).ToArray();
                                participant.Jump(marks);
                            }
                        }
                    }

                    jump.Add(participant);
                }
            }
            return jump;
        }
        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return default;
            string json = File.ReadAllText(fileName);
            
            dynamic data = JsonConvert.DeserializeObject(json);

            Blue_3.Participant participant;
            switch ((string)data.Type)
            {
                case "BasketballPlayer":
                    participant = new Blue_3.BasketballPlayer((string)data.Name,(string)data.Surname);
                    break;

                case "HockeyPlayer":
                    participant = new Blue_3.HockeyPlayer((string)data.Name,(string)data.Surname);
                    break;

                case "Participant":
                    participant = new Blue_3.Participant((string)data.Name,(string)data.Surname);
                    break;

                default:
                    return null;
            }

            if (data.Penalties != null)
            {
                foreach (int outmin in data.Penalties)
                {
                    participant.PlayMatch(outmin);
                }
            }

            return participant as T;
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;

            string json = File.ReadAllText(fileName);
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            if (obj == null) return null;
            var group = new Blue_4.Group(obj.GroupName?.ToString());

            if (obj.Teams != null)
            {
                foreach (var teaminfo in obj.Teams)
                {
                    string teamType = teaminfo.TeamType?.ToString();
                    string teamName = teaminfo.TeamName?.ToString();
                    JArray scores_array = teaminfo.Scores;
                    if (string.IsNullOrEmpty(teamName)) continue;
                    Blue_4.Team team = null;
                    switch (teamType)
                    {
                        case "ManTeam":
                            team = new Blue_4.ManTeam(teamName);
                            break;
                        case "WomanTeam":
                            team = new Blue_4.WomanTeam(teamName);
                            break;
                    }

                    if (team != null)
                    {
                        if (scores_array != null)
                        {
                            foreach (var score in scores_array)
                            {
                                team.PlayMatch((int)score);
                            }
                        }
                        group.Add(team);
                    }
                }
            }
            return group;
        }
        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;
            string json = File.ReadAllText(fileName);
            JObject inf = JObject.Parse(json);
            string teamType = inf["TeamType"]?.ToString();
            string teamName = inf["TeamName"]?.ToString();
            if (string.IsNullOrEmpty(teamType) || string.IsNullOrEmpty(teamName)) return null;
            
            Blue_5.Team team = CreateTeam(teamType, teamName);
            if (team == null) return null;
            
            Loadinf(team, inf["Sportsmen"] as JArray);
            
            return (T)team;

        }
        private void Loadinf(Blue_5.Team team, JArray sportsmen_inf)
        {
            if (sportsmen_inf == null || team == null) return;
            foreach (JToken sportsmantoken in sportsmen_inf)
            {
                string name = sportsmantoken["Name"]?.ToString() ?? string.Empty;
                string surname = sportsmantoken["Surname"]?.ToString() ?? string.Empty;
                int place = sportsmantoken["Place"]?.ToObject<int>() ?? 0;
                var sportsman = new Blue_5.Sportsman(name, surname);
                sportsman.SetPlace(place);
                team.Add(sportsman);
            }
        }
        private Blue_5.Team CreateTeam(string teamtype, string teamname)
        {
            switch (teamtype)
            {
                case "ManTeam":
                    return new Blue_5.ManTeam(teamname);
                case "WomanTeam":
                    return new Blue_5.WomanTeam(teamname);
                default:
                    return null;
            }
        }
        public override string Extension => "json";
    }
}
