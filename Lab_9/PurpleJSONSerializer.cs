using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Newtonsoft.Json;
using Lab_7;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using System.Collections.Immutable;
using System.Security;

namespace Lab_9 {
    public class PurpleJSONSerializer : PurpleSerializer {
        public override string Extension => "json";
        private void SerializeObject<T>(T obj) {
            if (FolderPath == null || FilePath  == null) return;

            string targetPath = $"{Path.Combine(FolderPath, FilePath)}.{Extension}";

            string serializedData = JsonConvert.SerializeObject(obj);
   
            var jObj = JObject.Parse(serializedData);
            jObj["$type"] = obj.GetType().AssemblyQualifiedName;
            serializedData = jObj.ToString();
        
            using (var writer = new StreamWriter(targetPath)) 
                writer.Write(serializedData);
        }
        private Dictionary<string, JToken> GetFileData() {
            if (FolderPath == null || FilePath == null) return null;

            string targetPath = $"{Path.Combine(FolderPath, FilePath)}.{Extension}";
            if (!File.Exists(targetPath)) return null;

            string objData = File.ReadAllText(targetPath);

            var objDict = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(objData);

            return objDict;
        }
        public override void SerializePurple1<T>(T obj, string fileName)  {
            SelectFile(fileName);
            SerializeObject(obj);
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName) {
            SelectFile(fileName);    
            SerializeObject(jumping);
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName) {
            SelectFile(fileName);
            SerializeObject(skating);
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string fileName) {
            SelectFile(fileName);
            SerializeObject(participant);
        }

        public override void SerializePurple5Report(Purple_5.Report group, string fileName) {
            SelectFile(fileName);
            SerializeObject(group);
        }

        public override T DeserializePurple1<T>(string fileName) {
            SelectFile(fileName);
            
            var objDict = GetFileData();
            if (objDict == null) return null;

            if (typeof(T) == typeof(Purple_1.Participant)) { 
                var Name = objDict.GetValueOrDefault("Name", null)?.ToObject<string>();
                var Surname = objDict.GetValueOrDefault("Surname", null)?.ToObject<string>();
                var Coefs = objDict.GetValueOrDefault("Coefs", null)?.ToObject<double[]>();
                var Marks = objDict.GetValueOrDefault("Marks", null)?.ToObject<int[,]>();

                var resultObj = new Purple_1.Participant(Name, Surname);
                resultObj.SetCriterias(Coefs);
                
                for (int i = 0; i < 4; i++) {
                    int[] curRow = new int[7];

                    for (int j = 0; j < 7; j++) 
                        curRow[j] = Marks[i, j];
                    
                    resultObj.Jump(curRow);
                }

                return resultObj as T;
            } 

            else if (typeof(T) == typeof(Purple_1.Judge)) { 
                var Name = objDict.GetValueOrDefault("Name", null)?.ToObject<string>();
                var Marks = objDict.GetValueOrDefault("Marks", null)?.ToObject<int[]>();

                var resultObj = new Purple_1.Judge(Name, Marks);

                return resultObj as T;
            }
            
            else { // Competition class
                var Judges = objDict.GetValueOrDefault("Judges", null)?.ToObject<Purple_1.Judge[]>();

                var participants = objDict.GetValueOrDefault("Participants", null);

                var resultObj = new Purple_1.Competition(Judges);

                var resultParticipants = new List<Purple_1.Participant>();

                foreach (var p in participants) {
                    var Coefs = p["Coefs"]?.ToObject<double[]>();
                    var Marks = p["Marks"]?.ToObject<int[,]>();

                    var curParticipant = p?.ToObject<Purple_1.Participant>();
                    curParticipant.SetCriterias(Coefs);

                    for (int i = 0; i < 4; i++) {
                        int[] curRow = new int[7];

                        for (int j = 0; j < 7; j++) 
                            curRow[j] = Marks[i, j];
                        
                        curParticipant.Jump(curRow);
                    }

                    resultParticipants.Add(curParticipant);
                }

                resultObj.Add(resultParticipants.ToArray());

                return resultObj as T;
            } 
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName) {
            SelectFile(fileName);

            var objDict = GetFileData();
            if (objDict == null) return null;

            var objTypeName = objDict["$type"].ToObject<string>();
            var objType = Type.GetType(objTypeName);
            dynamic resultObj = Activator.CreateInstance(objType);


            var Participants = objDict.GetValueOrDefault("Participants", null);
            var resultParticipants = new List<Purple_2.Participant>();

            foreach (var p in Participants) {
                var Distance = p["Distance"].ToObject<int>();
                var Marks = p["Marks"].ToObject<int[]>();
                var Result = p["Result"].ToObject<int>();   
                
                var SumWithoutMinMax = Marks.Sum() - Marks.Min() - Marks.Max();

                var target = Math.Ceiling((Result == 0) ? (Distance + (SumWithoutMinMax + 60) / 2.0) 
                                           : (Distance - (Result - SumWithoutMinMax - 60) / 2.0));

                var curParticipant = p.ToObject<Purple_2.Participant>();
                curParticipant.Jump(Distance, Marks, (int)target);

                resultParticipants.Add(curParticipant);
            }




            resultObj.Add(resultParticipants.ToArray());  

            return resultObj;
        }

        public override T DeserializePurple3Skating<T>(string fileName) {
            SelectFile(fileName);

            var objDict = GetFileData();
            if (objDict == null) return null;

            var Moods = objDict.GetValueOrDefault("Moods", null)?.ToObject<double[]>(); 

            var objTypeName = objDict["$type"].ToObject<string>();
            var objType = Type.GetType(objTypeName);

            dynamic resultObj = Activator.CreateInstance(objType, Moods, false);

            var Participants = objDict.GetValueOrDefault("Participants", null);
            var ResultParticipants = new List<Purple_3.Participant>();

            foreach (var p in Participants) {
                var Marks = p["Marks"].ToObject<double[]>();

                var curParticipant = p.ToObject<Purple_3.Participant>();
                foreach (var m in Marks)
                    curParticipant.Evaluate(m);

                ResultParticipants.Add(curParticipant);
            }

            var ResultParticipantsArray = ResultParticipants.ToArray();

            Purple_3.Participant.SetPlaces(ResultParticipantsArray);

            resultObj.Add(ResultParticipantsArray);

            return resultObj;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName) {
            SelectFile(fileName);

            var objDict = GetFileData();
            if (objDict == null) return null;

            var Name = objDict.GetValueOrDefault("Name", null)?.ToObject<string>();
    
            var resultObj = new Purple_4.Group(Name);
                
            var Sportsmen = objDict.GetValueOrDefault("Sportsmen", null);
            var resultSportsmen = new List<Purple_4.Sportsman>();

            foreach (var s in Sportsmen) {
                var Time = s["Time"].ToObject<double>();

                var curSportsman = s.ToObject<Purple_4.Sportsman>();
                curSportsman.Run(Time);

                resultSportsmen.Add(curSportsman);
            }

            resultObj.Add(resultSportsmen.ToArray()); 
            
            return resultObj;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName) {
            SelectFile(fileName);

            var objDict = GetFileData();
            if (objDict == null) return null;

            var resultObj = new Purple_5.Report();

            var Researches = objDict.GetValueOrDefault("Researches", null);
            var resultResearches = new List<Purple_5.Research>();

            foreach (var r in Researches) {
                var curResearch = r.ToObject<Purple_5.Research>();
                var curResponses = r["Responses"].ToObject<Purple_5.Response[]>();

                foreach (var rsp in curResponses) 
                    curResearch.Add(new string[3] {rsp.Animal, rsp.CharacterTrait, rsp.Concept});

                resultResearches.Add(curResearch);
            }

            resultObj.AddResearch(resultResearches.ToArray());  

            return resultObj;
        }
    }
}