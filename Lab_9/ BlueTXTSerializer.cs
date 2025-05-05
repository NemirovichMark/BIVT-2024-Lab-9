using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lab_7;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        private void SerializeData<T>(T data, string fileName) {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath) || data == null) {
                return;
            }

            string serializedData = JsonConvert.SerializeObject(data);
            var jObj = JObject.Parse(serializedData);
            jObj["$type"] = data.GetType().AssemblyQualifiedName;
            serializedData = jObj.ToString();

            using (var writer = new StreamWriter(FilePath)) {
                writer.Write(serializedData);
            }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName) {
            SerializeData(participant, fileName);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName) {
            SerializeData(participant, fileName);
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName) {
            SerializeData(student, fileName);
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName) {
            SerializeData(participant, fileName);
        }

        public override void SerializeBlue5Team<T>(T group, string fileName) {
            SerializeData(group, fileName);
        }

        // -------------
        private Dictionary<string, JToken>? GetFileData(string fileName) {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath)) {
                return null;
            }

            if (!File.Exists(FilePath)) return null;

            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, JToken>>(json);
        }

        public override Blue_1.Response? DeserializeBlue1Response(string fileName) {
            var data = GetFileData(fileName);
            if (data == null) return null; 

            var typeFullName = data["$type"]?.ToObject<string>();
            if (typeFullName is null || Type.GetType(typeFullName) is not Type objType) return null;

            var name = data["Name"]?.ToObject<string>() ?? null;
            int votes = data["Votes"]?.ToObject<int>() ?? 0;

            if (objType == typeof(Blue_1.HumanResponse)) {
                var surname = data["Surname"]?.ToObject<string>() ?? null;
                return new Blue_1.HumanResponse(name!, surname!, votes); 
            }

            return new Blue_1.Response(name!, votes);
        }

        public override Blue_2.WaterJump? DeserializeBlue2WaterJump(string fileName)
        {
            var data = GetFileData(fileName);
            if (data == null) return null;

            var typeFullName = data["$type"]?.ToObject<string>();
            if (typeFullName is null || Type.GetType(typeFullName) is not Type objType) return null;

            var name = data["Name"]?.ToObject<string>();
            int bank = data["Bank"]?.ToObject<int>() ?? 0;

            Blue_2.WaterJump? waterJump = objType switch
            {
                _ when objType == typeof(Blue_2.WaterJump3m) => new Blue_2.WaterJump3m(name!, bank),
                _ when objType == typeof(Blue_2.WaterJump5m) => new Blue_2.WaterJump5m(name!, bank),
                _ => null
            };

            if (waterJump == null) return null;

            if (data["Participants"] is JArray participantsData) {
                foreach (var participantData in participantsData)
                {
                    var pName = participantData["Name"]?.ToObject<string>();
                    var pSurname = participantData["Surname"]?.ToObject<string>();
                    var participant = new Blue_2.Participant(pName!, pSurname!);

                    if (participantData["Marks"] is JArray marksData) {
                        foreach (var jumpData in marksData) {
                            var marks = jumpData?.ToObject<int[]>();
                            if (marks != null && marks.Length == 5) participant.Jump(marks);
                        }
                    }

                    waterJump.Add(participant);
                }
            }

            return waterJump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName) 
        {
            var data = GetFileData(fileName);
            if (data == null) return null;

            var typeFullName = data["$type"]?.ToObject<string>();
            if (typeFullName is null) return null;

            var objType = Type.GetType(typeFullName);
            if (objType == null) return null;

            var name = data["Name"]?.ToObject<string>();
            var surname = data["Surname"]?.ToObject<string>();
            
            Blue_3.Participant participant;
            try
            {
                participant = (Blue_3.Participant)Activator.CreateInstance(objType, name, surname);
            }
            catch
            {
                return null;
            }

            if (data["Penalties"] is JArray penaltiesData) {
                foreach (var penalty in penaltiesData) {
                    int time = penalty?.ToObject<int>() ?? 0;
                    participant.PlayMatch(time);
                }
            }
            
            return (T)participant;
        }
        public override Blue_4.Group? DeserializeBlue4Group(string fileName) 
        {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
                return null;

            string json = File.ReadAllText(FilePath);
            var jObj = JObject.Parse(json);
            
            var typeFullName = jObj["$type"]?.ToString();
            if (typeFullName == null || Type.GetType(typeFullName) != typeof(Blue_4.Group))
                return null;

            var name = jObj["Name"]?.ToString();
            if (name == null) return null;

            var group = new Blue_4.Group(name);

            if (jObj["ManTeams"] is JArray manTeams) {
                foreach (var teamObj in manTeams.OfType<JObject>())
                {
                    var teamName = teamObj["Name"]?.ToString();

                    var manTeam = new Blue_4.ManTeam(teamName);
                    
                    if (teamObj["Scores"] is JArray scores)
                    {
                        foreach (var score in scores)
                        {
                            manTeam.PlayMatch(score.Value<int>());
                        }
                    }
                    
                    group.Add(manTeam);
                }
            }
            
            if (jObj["WomanTeams"] is JArray womanTeams) {
                foreach (var teamObj in womanTeams.OfType<JObject>()) {
                    var teamName = teamObj["Name"]?.ToString();

                    var womanTeam = new Blue_4.WomanTeam(teamName);
                    
                    if (teamObj["Scores"] is JArray scores)
                    {
                        foreach (var score in scores)
                        {
                            womanTeam.PlayMatch(score.Value<int>());
                        }
                    }
                    
                    group.Add(womanTeam);
                }
            }
            
            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName) 
        {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
                return null;

            string json = File.ReadAllText(FilePath);
            var jObj = JObject.Parse(json);
            
            var typeFullName = jObj["$type"]?.ToString();
            if (typeFullName == null)
                return null;

            var objType = Type.GetType(typeFullName);
            if (objType == null)
                return null;

            var teamName = jObj["Name"]?.ToString();
            if (teamName == null)
                return null;

            T team;
            if (objType == typeof(Blue_5.ManTeam)) {
                team = (T)(object)new Blue_5.ManTeam(teamName);
            }
            else if (objType == typeof(Blue_5.WomanTeam)) {
                team = (T)(object)new Blue_5.WomanTeam(teamName);
            }
            else return null;

            if (jObj["Sportsmen"] is JArray sportsmen) {
                foreach (var sportsmanObj in sportsmen.OfType<JObject>()) {
                    var name = sportsmanObj["Name"]?.ToString();
                    var surname = sportsmanObj["Surname"]?.ToString();
                    var place = sportsmanObj["Place"]?.Value<int>() ?? 0;
                    var sportsman = new Blue_5.Sportsman(name, surname);
                    sportsman.SetPlace(place);
                    
                    if (team is Blue_5.ManTeam manTeam)
                        manTeam.Add(sportsman);
                    else if (team is Blue_5.WomanTeam womanTeam)
                        womanTeam.Add(sportsman);
                }
            }

            return team;
        }
        
                    
    }
}