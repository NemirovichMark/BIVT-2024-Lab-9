using Lab_7;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        private void DataSerializer<T> (T data, string fileName)
        {
            SelectFile(fileName);

            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath) || data == null) return;

            string serData = JsonConvert.SerializeObject(data);

            var obj = JObject.Parse(serData);
            obj["$type"] = data.GetType().AssemblyQualifiedName;

            serData = obj.ToString();

            using (var file = new StreamWriter(FilePath)) // поток для записи
            {
                file.Write(serData);
            }
        }


        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            DataSerializer(participant, fileName);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            DataSerializer(participant, fileName);
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;

            DataSerializer(student, fileName);
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            DataSerializer(participant, fileName);
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;

            DataSerializer(group, fileName);
        }




        private Dictionary<string, JToken>? GetDataFromFile(string fileName)
        {
            SelectFile(fileName);

            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath)) return null;
            if (!File.Exists(FilePath)) return null;

            string jsonData = File.ReadAllText(FilePath);

            return JsonConvert.DeserializeObject<Dictionary<string, JToken>>(jsonData);
        }


        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var data = GetDataFromFile(fileName);
            if (data == null) return null;

            var typeFullName = data["$type"]?.ToObject<string>();
            if (typeFullName is null) return null;

            var objType = Type.GetType(typeFullName);
            if (objType == null) return null;



            var name = data["Name"]?.ToObject<string>() ?? null;    // поля родителя
            int votes = data["Votes"]?.ToObject<int>() ?? 0;

            if (objType == typeof(Blue_1.HumanResponse))            // если наследник + поле
            {
                var surname = data["Surname"]?.ToObject<string>() ?? null;
                return new Blue_1.HumanResponse(name, surname, votes);
            }

            return new Blue_1.Response(name, votes); // родитель
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var data = GetDataFromFile(fileName);
            if (data == null) return null;

            var typeFullName = data["$type"]?.ToObject<string>();
            if (typeFullName is null) return null;

            var objType = Type.GetType(typeFullName);
            if (objType == null) return null;



            var name = data["Name"]?.ToObject<string>() ?? null;  // parent's
            var bank = data["Bank"]?.ToObject<int>() ?? 0;

            Blue_2.WaterJump? waterJump = null;

            if (objType == typeof(Blue_2.WaterJump3m))            // наследники
                waterJump = new Blue_2.WaterJump3m(name, bank);

            else if (objType == typeof(Blue_2.WaterJump5m))
                waterJump = new Blue_2.WaterJump5m(name, bank);

            else return null; // родитель абстрактный

            if (data["Participants"] is JArray participants)
            {
                foreach (var DataParticipant in participants)
                {
                    var pName = DataParticipant["Name"]?.ToObject<string>();
                    var pSurname = DataParticipant["Surname"]?.ToObject<string>();

                    var participant = new Blue_2.Participant(pName, pSurname);

                    if (DataParticipant["Marks"] is JArray DataMarks)
                    {
                        foreach (var DataJump in DataMarks)
                        {
                            var marks = DataJump?.ToObject<int[]>();

                            if (marks != null && marks.Length == 5)
                                participant.Jump(marks);
                        }
                    }
                    waterJump.Add(participant);
                }
            }

            return waterJump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var data = GetDataFromFile(fileName);
            if (data == null) return null;

            var typeFullName = data["$type"]?.ToObject<string>();
            if (typeFullName is null) return null;

            var objType = Type.GetType(typeFullName);
            if (objType == null) return null;



            var name = data["Name"]?.ToObject<string>();
            var surname = data["Surname"]?.ToObject<string>();

            Blue_3.Participant? participant = null;

            if (objType == typeof(Blue_3.HockeyPlayer))                // наследники
                participant = new Blue_3.HockeyPlayer(name, surname);

            else if (objType == typeof(Blue_3.BasketballPlayer))
                participant = new Blue_3.BasketballPlayer(name, surname);

            else participant = new Blue_3.Participant(name, surname);   // или родитель

            if (data["Penalties"] is JArray penalties)
            {
                foreach (var penalty in penalties)
                {
                    int time = penalty?.ToObject<int>() ?? 0;
                    participant.PlayMatch(time);
                }
            }

            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var data = GetDataFromFile(fileName);
            if (data == null) return null;

            var typeFullName = data["$type"]?.ToObject<string>();
            if (typeFullName is null) return null;

            var objType = Type.GetType(typeFullName);
            if (objType == null) return null;



            var name = data["Name"]?.ToString();
            if (name == null) return null;

            var group = new Blue_4.Group(name);

            // man teams
            if (data["ManTeams"] is JArray manTeams)
            {
                foreach (var team in manTeams.OfType<JObject>())
                {
                    var teamName = team["Name"]?.ToString();
                    var manTeam = new Blue_4.ManTeam(teamName);

                    if (team["Scores"] is JArray scores)
                        foreach (var score in scores)
                        {
                            manTeam.PlayMatch(score.Value<int>());
                        }

                    group.Add(manTeam);
                }
            }

            // woman teams
            if (data["WomanTeams"] is JArray womanTeams)
            {
                foreach (var team in womanTeams.OfType<JObject>())
                {
                    var teamName = team["Name"]?.ToString();
                    var womanTeam = new Blue_4.WomanTeam(teamName);

                    if (team["Scores"] is JArray scores)
                        foreach (var score in scores)
                        {
                            womanTeam.PlayMatch(score.Value<int>());
                        }

                    group.Add(womanTeam);
                }
            }

            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var data = GetDataFromFile(fileName);
            if (data == null) return null;

            var typeFullName = data["$type"]?.ToObject<string>();
            if (typeFullName is null) return null;

            var objType = Type.GetType(typeFullName);
            if (objType == null) return null;



            var teamName = data["Name"]?.ToObject<string>();
            if (teamName == null) return null;

            T team;
            if (objType == typeof(Blue_5.ManTeam))
                team = (T)(object)new Blue_5.ManTeam(teamName);

            else if (objType == typeof(Blue_5.WomanTeam))
                team = (T)(object)new Blue_5.WomanTeam(teamName);

            else return null; // родитель абстрактный


            if (data["Sportsmen"] is JArray sportsmen)
            {
                foreach (var sportsman in sportsmen.OfType<JObject>())
                {
                    var name = sportsman["Name"]?.ToString();
                    var surname = sportsman["Surname"]?.ToString();
                    var place = sportsman["Place"]?.Value<int>() ?? 0;

                    var Sportsman = new Blue_5.Sportsman(name, surname);

                    Sportsman.SetPlace(place);

                    if (team is Blue_5.ManTeam manTeam)
                        manTeam.Add(Sportsman);

                    else if (team is Blue_5.WomanTeam womanTeam)
                        womanTeam.Add(Sportsman);
                }
            }

            return team;
        }
    }
}
