using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using Newtonsoft.Json;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Linq;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
                return;
            var obj = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname
            };
            string json = System.Text.Json.JsonSerializer.Serialize(obj);
            File.WriteAllText(fileName, json);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
                return;
            var participants = new object[participant.Participants.Length];

            for (int i = 0; i < participant.Participants.Length; i++)
            {
                var p = participant.Participants[i];
                participants[i] = new
                {
                    Type = "Participant",
                    p.Name,
                    p.Surname,
                    Marks = p.Marks 
                };
            }

            var obj = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                participant.Bank,
                Participants = participants
            };
           
            string text = JsonConvert.SerializeObject(obj);
            File.WriteAllText(fileName, text);
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : class
        {
            if (student == null || string.IsNullOrEmpty(fileName))
                return;

            var obj = new
            {
                Type = student.GetType().Name,
                student.Name,
                student.Surname,
                PenaltyTimes = student.Penalties 
            };

            string json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(fileName, json);

        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
                return;
            int manc = 0;
            if (participant.ManTeams != null)
            {
                for (int i = 0; i < participant.ManTeams.Length; i++)
                    if (participant.ManTeams[i] != null)
                        manc++;
            }

            
            int womanc = 0;
            if (participant.WomanTeams != null)
            {
                for (int i = 0; i < participant.WomanTeams.Length; i++)
                    if (participant.WomanTeams[i] != null)
                        womanc++;
            }
            object[] manTeamsArr = new object[manc];
            object[] womanTeamsArr = new object[womanc];

            int idx = 0;
            for (int i = 0; i < participant.ManTeams.Length; i++)
            {
                var team = participant.ManTeams[i];
                if (team != null)
                {
                    manTeamsArr[idx++] = new
                    {
                        Type = "ManTeam",
                        team.Name,
                        Scores = team.Scores
                    };
                }
            }
            idx = 0;
            for (int i = 0; i < participant.WomanTeams.Length; i++)
            {
                var team = participant.WomanTeams[i];
                if (team != null)
                {
                    womanTeamsArr[idx++] = new
                    {
                        Type = "WomanTeam",
                        team.Name,
                        Scores = team.Scores
                    };
                }
            }
            var obj = new
            {
                participant.Name,
                ManTeams = manTeamsArr,
                WomanTeams = womanTeamsArr
            };

            string json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(fileName, json);


        }

        public override void SerializeBlue5Team<T>(T group, string fileName) //where T : class
        {
            if (group == null || string.IsNullOrEmpty(fileName))
                return;

           
            int sportsmenc = 0;
            if (group.Sportsmen != null)
                for (int i = 0; i < group.Sportsmen.Length; i++)
                    if (group.Sportsmen[i] != null)
                        sportsmenc++;

            
            object[] sportsmen = new object[sportsmenc];
            int idx = 0;
            for (int i = 0; i < group.Sportsmen.Length; i++)
            {
                var s = group.Sportsmen[i];
                if (s != null)
                {
                    sportsmen[idx++] = new
                    {
                        s.Name,
                        s.Surname,
                        s.Place
                    };
                }
            }

            var obj = new
            {
                Type = group.GetType().Name,
                group.Name,
                Sportsmen = sportsmen
            };

            string json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(fileName, json);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;
            string json = File.ReadAllText(fileName);
            dynamic parsing = JsonConvert.DeserializeObject<dynamic>(json);
            string type = parsing.Type;
            if (type == "HumanResponse")
            {
                string name = parsing.Name;
                string surname = parsing.Surname;
                int votes = parsing.Votes;

                Blue_1.HumanResponse humanResponse = new Blue_1.HumanResponse(name, surname, votes);
                return humanResponse;
            }
            else
            {
                string name = parsing.Name;
                int votes = parsing.Votes;

                Blue_1.Response response = new Blue_1.Response(name, votes);
                return response;
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;
            string json = File.ReadAllText(fileName);
            dynamic parsing = JsonConvert.DeserializeObject<dynamic>(json);
            string type = parsing.Type;
            string name = parsing.Name;
            int bank = parsing.Bank;

            Blue_2.WaterJump jumper = null;

            if (type == "WaterJump3m")
                jumper = new Blue_2.WaterJump3m(name, bank);
            else if (type == "WaterJump5m")
                jumper = new Blue_2.WaterJump5m(name, bank);
            foreach (var p in parsing.Participants)
            {
                Blue_2.Participant participant = new Blue_2.Participant((string)p.Name, (string)p.Surname);


                int[][] jumpi = JsonConvert.DeserializeObject<int[][]>(p.Marks.ToString());

                for (int i = 0; i < jumpi.Length; i++)
                {
                    if (jumpi[i].Length == 5)
                        participant.Jump(jumpi[i]);
                }

                jumper.Add(participant);
            }

            return jumper;
        }


    

        public override T DeserializeBlue3Participant<T>(string fileName) //where T : class
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;

            string text = File.ReadAllText(fileName);
            dynamic parsing = JsonConvert.DeserializeObject<dynamic>(text);

            string type = parsing.Type;
            string name = parsing.Name;
            string surname = parsing.Surname;
            Blue_3.Participant participant = null;
            if(type == "BasketballPlayer")
            {
                participant = new Blue_3.BasketballPlayer(name, surname);
            }
            else if (type == "HockeyPlayer")
            {
                participant = new Blue_3.HockeyPlayer(name, surname);
            }
            else
            {
                participant = new Blue_3.Participant(name, surname);
            }
           
            if (parsing.PenaltyTimes != null)
            {
                int[] penaltyTimes = JsonConvert.DeserializeObject<int[]>(parsing.PenaltyTimes.ToString());

                foreach (int time in penaltyTimes)
                {
                    participant.PlayMatch(time);
                }
            }
            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;
            string json = File.ReadAllText(fileName);
            var parsing = JsonConvert.DeserializeObject<dynamic>(json);
            var group = new Blue_4.Group((string)parsing.Name);
            if (parsing.ManTeams != null)
            {
                foreach (var team in parsing.ManTeams)
                {
                    var manTeam = new Blue_4.ManTeam((string)team.Name);
                    if (team.Scores != null)
                    {
                        foreach (var score in team.Scores)
                        {
                            manTeam.PlayMatch((int)score);
                        }
                    }
                    group.Add(manTeam);
                }
            }
            if (parsing.WomanTeams != null)
            {
                foreach (var team in parsing.WomanTeams)
                {
                    var womanTeam = new Blue_4.WomanTeam((string)team.Name);
                    if (team.Scores != null)
                    {
                        foreach (var score in team.Scores)
                        {
                            womanTeam.PlayMatch((int)score);
                        }
                    }
                    group.Add(womanTeam);
                }
            }

            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName) //where T : class
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;
            string json = File.ReadAllText(fileName);
            var parsing = JsonConvert.DeserializeObject<dynamic>(json);
            Blue_5.Team team = null;

            if (parsing.Type == "ManTeam")
                team = new Blue_5.ManTeam((string)parsing.Name);
            else if (parsing.Type == "WomanTeam")
                team = new Blue_5.WomanTeam((string)parsing.Name);
            if (parsing.Sportsmen != null)
            {
                foreach (var s in parsing.Sportsmen)
                {
                    var sportsman = new Blue_5.Sportsman((string)s.Name, (string)s.Surname);
                    sportsman.SetPlace((int)s.Place);
                    team.Add(sportsman);
                }
            }
            return (T)team;
        }
    }
}
