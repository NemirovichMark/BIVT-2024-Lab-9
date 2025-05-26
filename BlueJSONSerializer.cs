using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var json = new JObject
            {
                ["Type"] = participant.GetType().Name,
                ["Name"] = participant.Name,
                ["Votes"] = participant.Votes
            };
            if (participant is Blue_1.HumanResponse human)
            {
                json["Surname"] = human.Surname;
            }
            File.WriteAllText(FilePath, json.ToString());
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }
            var deserialized = JObject.Parse(content);
            string name = deserialized["Name"].ToString();
            int votes = deserialized["Votes"].ToObject<int>();
            if (deserialized["Surname"] != null)
            {
                return new Blue_1.HumanResponse(name, deserialized["Surname"].ToString(), votes);
            }
            else
            {
                return new Blue_1.Response(name, votes);
            }

        }
       
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var json = new JObject
            {
                ["Type"] = participant.GetType().Name,
                ["Name"] = participant.Name,
                ["Bank"] = participant.Bank,
                ["Participants"] = new JArray()
            };
            foreach (var p in participant.Participants)
            {
                var pObj = new JObject
                {
                    ["Name"] = p.Name,
                    ["Surname"] = p.Surname,
                    ["Marks"] = new JArray()
                };
                for (int jump = 0; jump < 2; jump++)
                {
                    var marks = new JArray();
                    for (int i = 0; i < 5; i++)
                    {
                        marks.Add(p.Marks[jump, i]);
                    }
                    ((JArray)pObj["Marks"]).Add(marks);
                }
                ((JArray)json["Participants"]).Add(pObj);
            }
            File.WriteAllText(FilePath, json.ToString());
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            var deserialized = JObject.Parse(content);
            string type = deserialized["Type"].ToString();
            string name = deserialized["Name"].ToString();
            int bank = deserialized["Bank"].ToObject<int>();
            Blue_2.WaterJump waterjump;
            if (type == "WaterJump3m") waterjump = new Blue_2.WaterJump3m(name, bank);
            else if (type == "WaterJump5m") waterjump = new Blue_2.WaterJump5m(name, bank);
            else return null;

            foreach(var p in deserialized["Participants"])
            {
                string pName = p["Name"].ToString();
                string pSurname = p["Surname"].ToString();
                var participant = new Blue_2.Participant(pName, pSurname);
                foreach (var marks in p["Marks"])
                {
                    int[] participantMarks = marks.ToObject<int[]>();
                    participant.Jump(participantMarks);
                }
                waterjump.Add(participant);
            }
            return waterjump;
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName) where T : class
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var json = new JObject
            {
                ["Type"] = student.GetType().Name,
                ["Name"] = student.Name,
                ["Surname"] = student.Surname
            };
            if (student is Blue_3.Participant p && p.Penalties != null && p.Penalties.Length > 0)
            {
                json["Penalties"] = new JArray(p.Penalties);
            }
            File.WriteAllText(FilePath, json.ToString());
        }

        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            var deserialized = JObject.Parse(content);
            string type = deserialized["Type"].ToString();
            string name = deserialized["Name"].ToString();
            string surname = deserialized["Surname"].ToString();
            Blue_3.Participant participant = null;
            if (type == "BasketballPlayer")
            {
               participant = new Blue_3.BasketballPlayer(name, surname);
            }
            else if (type == "HockeyPlayer")
            {
                participant = new Blue_3.HockeyPlayer(name, surname);
            }
            else if (type == "Participant")
            {
                participant = new Blue_3.Participant(name, surname);
            }
            foreach (var penalty in deserialized["Penalties"])
            {
                participant.PlayMatch((int)penalty);
            }
            return participant as T;
        }


        private void AddTeam(Blue_4.Team team, JToken teamsArray, string teamType)
        {
            var teamObj = new JObject
            {
                ["Name"] = team.Name,
                ["Type"] = teamType,
                ["Scores"] = new JArray()
            };
            if (team.Scores != null)
            {
                foreach (int score in team.Scores)
                {
                    ((JArray)teamObj["Scores"]).Add(score);
                }
            }
            ((JArray)teamsArray).Add(teamObj);
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var json = new JObject
            {
                ["Name"] = participant.Name,
                ["ManTeams"] = new JArray(),
                ["WomanTeams"] = new JArray()
            };
            foreach (var team in participant.ManTeams)
            {
                if (team != null)
                {
                    AddTeam(team, json["ManTeams"], "man");
                }
            }
            foreach (var team in participant.WomanTeams)
            {
                if (team != null)
                {
                    AddTeam(team, json["WomanTeams"], "woman");
                }
            }
            File.WriteAllText(FilePath, json.ToString());
        }
        private void DeserializeTeams(JToken teamsJson, string type, Blue_4.Group group)
        {
            if (teamsJson == null) return;

            foreach (var teamJson in teamsJson)
            {
                string name = teamJson["Name"].ToString();
                Blue_4.Team team = null;
                if (type == "man")
                {
                    team = new Blue_4.ManTeam(name);
                }
                else if (type == "woman")
                {
                    team = new Blue_4.WomanTeam(name);
                }
                if (team != null && teamJson["Scores"] != null)
                {
                    foreach (var score in teamJson["Scores"])
                    {
                        team.PlayMatch((int)score);
                    }
                    group.Add(team);
                }
            }
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            var deserialized = JObject.Parse(content);
            string groupName = deserialized["Name"].ToString();
            var group = new Blue_4.Group(groupName);
            DeserializeTeams(deserialized["ManTeams"], "man", group);
            DeserializeTeams(deserialized["WomanTeams"], "woman", group);
            return group;
        }


        public override void SerializeBlue5Team<T>(T group, string fileName) where T : class
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var team = group as Blue_5.Team;
            if (team == null) return;
            var json = new JObject
            {
                ["Type"] = team.GetType().Name,
                ["Name"] = team.Name,
                ["Sportsmen"] = new JArray()
            };
            foreach (var sportsman in team.Sportsmen)
            {
                if (sportsman == null) continue;
                var sportsmanObj = new JObject
                {
                    ["Name"] = sportsman.Name,
                    ["Surname"] = sportsman.Surname,
                    ["Place"] = sportsman.Place
                };

                ((JArray)json["Sportsmen"]).Add(sportsmanObj);
            }

            File.WriteAllText(FilePath, json.ToString());
        }

        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            SelectFile(fileName);
            string content = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(content)) return null;
            var deserialized = JObject.Parse(content);

            string type = deserialized["Type"].ToString();
            string name = deserialized["Name"].ToString();
            Blue_5.Team team = null;
            if (type == "ManTeam")
            {
                team = new Blue_5.ManTeam(name);
            }
            else if (type == "WomanTeam") 
            { 
                 team = new Blue_5.WomanTeam(name);
            }
            if (team == null) return null;

            var sportsmenArray = (JArray)deserialized["Sportsmen"];
            if (sportsmenArray != null)
            {
                foreach (var s in sportsmenArray)
                {
                    string sName = s["Name"].ToString();
                    string sSurname = s["Surname"].ToString();
                    int place = s["Place"].ToObject<int>();
                    var sportsman = new Blue_5.Sportsman(sName, sSurname);
                    sportsman.SetPlace(place);
                    team.Add(sportsman);
                }
            }
            return team as T;
        }
    }
}