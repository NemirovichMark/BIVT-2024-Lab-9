using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using System.IO;
using Newtonsoft.Json;


namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                if (participant == null || string.IsNullOrEmpty(fileName)) return;

                string type = "Response";
                if (participant is Blue_1.HumanResponse)
                    type = "HumanResponse";

                object objToSer;
                if (type == "HumanResponse")
                {
                    var human = participant as Blue_1.HumanResponse;
                    objToSer = new
                    {
                        Type = type,
                        Name = human.Name,
                        Surname = human.Surname,
                        Votes = human.Votes
                    };
                }
                else
                {
                    objToSer = new
                    {
                        Type = type,
                        Name = participant.Name,
                        Votes = participant.Votes
                    };
                }
                SelectFile(fileName);
                var json = JsonConvert.SerializeObject(objToSer);
                File.WriteAllText(FilePath, json);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(fileName))
            {
                string json = reader.ReadToEnd();
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
                string type = obj.Type;
                string name = obj.Name;
                int votes = obj.Votes;

                if (type == "HumanResponse")
                {
                    string surname = obj.Surname;
                    return new Blue_1.HumanResponse(name, surname, votes);
                }
                else
                {
                    return new Blue_1.Response(name, votes);
                }
            }
        }
        private object[] SerializeParticipants(Blue_2.Participant[] parts)
        {
            if (parts == null) return new object[0];

            var list = new object[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                var m = parts[i].Marks;
                int[][] jagged = new int[m.GetLength(0)][];
                for (int j = 0; j < m.GetLength(0); j++)
                {
                    jagged[j] = new int[m.GetLength(1)];
                    for (int k = 0; k < m.GetLength(1); k++)
                        jagged[j][k] = m[j, k];
                }

                list[i] = new
                {
                    Name = parts[i].Name,
                    Surname = parts[i].Surname,
                    Marks = jagged
                };
            }

            return list;
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            var type = participant is Blue_2.WaterJump3m ? "WaterJump3m" : "WaterJump5m";

            var objectToSer = new
            {
                Type = type,
                Name = participant.Name,
                Bank = participant.Bank,
                Participants = SerializeParticipants(participant.Participants)
            };

            SelectFile(fileName);
            var json = JsonConvert.SerializeObject(objectToSer);
            File.WriteAllText(FilePath, json);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath)) return null;
            var json = File.ReadAllText(FilePath);
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);

            string type = obj.Type;
            string name = obj.Name;
            int bank = obj.Bank;

            Blue_2.WaterJump jump;
            if (type != "WaterJump3m")
            {
                jump = new Blue_2.WaterJump5m(name, bank);
            }
            else
            {
                jump = new Blue_2.WaterJump3m(name, bank);
            }

            foreach (var p in obj.Participants)
            {
                string pname = p.Name;
                string psurname = p.Surname;

                int[][] marksT = JsonConvert.DeserializeObject<int[][]>(p.Marks.ToString());
                int[,] marks = new int[marksT.Length, 5];
                for (int i = 0; i < marksT.Length; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        marks[i, j] = marksT[i][j];
                    }
                }

                var part = new Blue_2.Participant(pname, psurname);

                for (int i = 0; i < marks.GetLength(0); i++)
                {
                    int[] jumpR = new int[5];
                    for (int j = 0; j < 5; j++)
                    {
                        jumpR[j] = marks[i, j];
                    }
                    part.Jump(jumpR);
                }
                jump.Add(part);
            }

            return jump;
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            string type = student switch
            {
                Blue_3.BasketballPlayer => "BasketballPlayer",
                Blue_3.HockeyPlayer => "HockeyPlayer",
                Blue_3.Participant => "Participant"
            };

            var objectToS = new
            {
                Type = type,
                Name = student.Name,
                Surname = student.Surname,
                Penalties = student.Penalties
            };

            SelectFile(fileName);
            var json = JsonConvert.SerializeObject(objectToS);
            File.WriteAllText(FilePath, json);
        }
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath)) return null;
            
            string json = File.ReadAllText(FilePath);
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);

            string type = obj.Type;
            string name = obj.Name;
            string surname = obj.Surname;

            Blue_3.Participant participant;

            if (type == "BasketballPlayer")
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
                
            if (obj.Penalties != null)
                {
                    int[] penalties = JsonConvert.DeserializeObject<int[]>(obj.Penalties.ToString());

                    foreach (int t in penalties)
                    {
                        participant.PlayMatch(t);
                    }
                }
            return (T)participant;
        }
        public override void SerializeBlue4Group(Blue_4.Group group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            var manteams = group.ManTeams;
            var womanteams = group.WomanTeams;
            int mank = 0;
            int womank = 0;

            foreach (var t in manteams)
            {
                if (t != null) mank++;
            }

            foreach (var t in womanteams)
            {
                if (t != null) womank++;
            }

            var mans = new object[mank];
            var womans = new object[womank];

            int i = 0;
            int j = 0;
            foreach (var t in manteams)
            {
                if (t == null) continue;
                mans[i++] = new
                {
                    Type = "ManTeam",
                    Name = t.Name,
                    Scores = t.Scores
                };
            }

            foreach (var t in womanteams)
            {
                if (t == null) continue;
                womans[j++] = new
                {
                    Type = "WomanTeam",
                    Name = t.Name,
                    Scores = t.Scores
                };
            }

            var data = new
            {
                Name = group.Name,
                ManTeams = mans,
                WomanTeams = womans
            };

            SelectFile(fileName);
            var json = JsonConvert.SerializeObject(data);
            File.WriteAllText(FilePath, json);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath)) return null;
            string json = File.ReadAllText(FilePath);
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            var group = new Blue_4.Group((string)obj.Name);
            if (obj.ManTeams != null)
            {
                foreach (var t in obj.ManTeams)
                {
                    var manTeam = new Blue_4.ManTeam((string)t.Name);
                    if (t.Scores != null)
                    {
                        foreach (var score in t.Scores)
                        {
                            manTeam.PlayMatch((int)score);
                        }
                    }
                    group.Add(manTeam);
                }
            }
            if (obj.WomanTeams != null)
            {
                foreach (var t in obj.WomanTeams)
                {
                    var womanTeam = new Blue_4.WomanTeam((string)t.Name);
                    if (t.Scores != null)
                    {
                        foreach (var s in t.Scores)
                        {
                            womanTeam.PlayMatch((int)s);
                        }
                    }
                    group.Add(womanTeam);
                }
            }

            return group;
        }
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            int k = 0;
            for (int i = 0; i < group.Sportsmen.Length; i++)
            {
                if (group.Sportsmen[i] != null) k++;
            }
            string[] names = new string[k];
            string[] surnames = new string[k];
            int[] places = new int[k];

            int ind = 0;
            for (int i = 0; i < group.Sportsmen.Length; i++)
            {
                if (group.Sportsmen[i] == null) continue;
                names[ind] = group.Sportsmen[i].Name;
                surnames[ind] = group.Sportsmen[i].Surname;
                places[ind] = group.Sportsmen[i].Place;
                ind++;
            }

            string type = group is Blue_5.ManTeam ? "ManTeam" : "WomanTeam";

            var data = new
            {
                Type = type,
                Name = group.Name,
                Names = names,
                Surnames = surnames,
                Places = places
            };

            SelectFile(fileName);
            var json = JsonConvert.SerializeObject(data);
            File.WriteAllText(FilePath, json);
        }
        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath)) return null;
            string json = File.ReadAllText(FilePath);
            dynamic obj = JsonConvert.DeserializeObject(json);
            Blue_5.Team team;
            string type = obj.Type;
            string name = obj.Name;
            string[] names = ((string[])JsonConvert.DeserializeObject<string[]>(obj.Names.ToString()));
            string[] surnames = ((string[])JsonConvert.DeserializeObject<string[]>(obj.Surnames.ToString()));
            int[] places = ((int[])JsonConvert.DeserializeObject<int[]>(obj.Places.ToString()));

            if (type == "ManTeam")
                team = new Blue_5.ManTeam(name);
            else
                team = new Blue_5.WomanTeam(name);

            for (int i = 0; i < names.Length; i++)
            {
                var sman = new Blue_5.Sportsman(names[i], surnames[i]);
                sman.SetPlace(places[i]);
                team.Add(sman);
            }

            return (T)team;
        }

    }
}