using Lab_7;
using Lab_9;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";

        private void Serializer<T>(T obj)
        {
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath)) { return; }
            string path = Path.Combine(FolderPath, FilePath);

            string jsonString = JsonConvert.SerializeObject(obj);

            var Jobj = JObject.Parse(jsonString);
            Jobj["$type"] = obj.GetType().AssemblyQualifiedName;

            jsonString = Jobj.ToString();

            using (var writer = new StreamWriter(path))
                writer.Write(jsonString);
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            Serializer<T>(obj);
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            Serializer<T>(jumping);
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            Serializer<T>(skating);
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            Serializer<Purple_4.Group>(group);
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            Serializer<Purple_5.Report>(report);
        }
        private Dictionary<string, JToken> DeSerializer(){
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath)) return null;
            string path = FilePath;
            if (!File.Exists(path)) return null;
            var data = File.ReadAllText(path);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(data);
            return dict;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var dict = DeSerializer();
            if (typeof(T) == typeof(Purple_1.Participant)) {
                string name = dict.GetValueOrDefault("Name", null)?.ToObject<string>();
                string surname = dict.GetValueOrDefault("Surname", null)?.ToObject<string>();
                Purple_1.Participant res = new Purple_1.Participant(name, surname);
                var coefs = dict.GetValueOrDefault("Coefs", null)?.ToObject<double[]>();
                var _marks = dict.GetValueOrDefault("Marks", null)?.ToObject<int[,]>();
                res.SetCriterias(coefs);
                for (int i = 0; i < 4; i++) {
                    int[] marks = new int[7];
                    for (int j = 0; j < 7; j++){
                        marks[j] = _marks[i, j];
                    }
                    res.Jump(marks);
                }
                return res as T;
            } else if (typeof(T) == typeof(Purple_1.Judge)) {
                string name = dict.GetValueOrDefault("Name", null)?.ToObject<string>();
                var marks = dict.GetValueOrDefault("Marks", null)?.ToObject<int[]>();
                Purple_1.Judge res = new Purple_1.Judge(name, marks);

                return res as T;
            } else if (typeof(T) == typeof(Purple_1.Competition)){
                var judges = dict.GetValueOrDefault("Judges", null)?.ToObject<Purple_1.Judge[]>();
                Purple_1.Competition res = new Purple_1.Competition(judges);
                var prts = dict.GetValueOrDefault("Participants", null);
                foreach (var p in prts) {
                    var coefs = p["Coefs"]?.ToObject<double[]>();
                    var _marks = p["Marks"]?.ToObject<int[,]>();
                    
                    var prt = p?.ToObject<Purple_1.Participant>();
                    prt.SetCriterias(coefs);
                    for (int i = 0; i < 4; i++) {
                        int[] marks = new int[7];
                        for (int j = 0; j < 7; j++){
                            marks[j] = _marks[i, j];
                        }
                        prt.Jump(marks);
                    }
                    res.Add(prt);
                }

                return res as T;
            }
            return default(T);
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var dict = DeSerializer();
            var prts = dict.GetValueOrDefault("Participants", null);
            string name = dict.GetValueOrDefault("Name", null)?.ToObject<string>();
            int standard = dict.GetValueOrDefault("Standard", 0).ToObject<int>();
            var type = dict.GetValueOrDefault("$type", null)?.ToObject<string>();
           if (type == typeof(Purple_2.JuniorSkiJumping).AssemblyQualifiedName) {
                var res = new Purple_2.JuniorSkiJumping();
                
                foreach (var p in prts) {
                    string p_name = p["Name"]?.ToObject<string>();
                    string p_surname = p["Surname"]?.ToObject<string>();
                    int dist = p["Distance"].ToObject<int>();
                    int[] marks = p["Marks"].ToObject<int[]>();
                    var prt = new Purple_2.Participant(p_name, p_surname);
                    prt.Jump(dist, marks, standard);
                    //prt.Print();
                    res.Add(prt);
                }

                return (T)(object)res;
            } else {
                var res = new Purple_2.ProSkiJumping();
                
                foreach (var p in prts) {
                    string p_name = p["Name"]?.ToObject<string>();
                    string p_surname = p["Surname"]?.ToObject<string>();
                    int dist = p["Distance"].ToObject<int>();
                    int[] marks = p["Marks"].ToObject<int[]>();
                    var prt = new Purple_2.Participant(p_name, p_surname);
                    prt.Jump(dist, marks, standard);
                    res.Add(prt);
                }

                return (T)(object)res;
            }

            return default(T);
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var dict = DeSerializer();
            var type = dict.GetValueOrDefault("$type", null)?.ToObject<string>();
            var prts = dict.GetValueOrDefault("Participants", null);
            var moods = dict.GetValueOrDefault("Moods", null)?.ToObject<double[]>();
            if (type == typeof(Purple_3.FigureSkating).AssemblyQualifiedName){
                var res = new Purple_3.FigureSkating(moods, false);
                var arr = new Purple_3.Participant[0];
                foreach (var p in prts) {
                    var marks = p["Marks"].ToObject<double[]>();

                    var prt = p.ToObject<Purple_3.Participant>();
                    foreach (var i in marks) prt.Evaluate(i);
                    Array.Resize(ref arr, arr.Length + 1);
                    arr[arr.Length - 1] = prt;
                }
                Purple_3.Participant.SetPlaces(arr);
                res.Add(arr);
                return (T)(object)res;
            } else {
                var res = new Purple_3.IceSkating(moods, false);
                var arr = new Purple_3.Participant[0];
                foreach (var p in prts) {
                    var marks = p["Marks"].ToObject<double[]>();

                    var prt = p.ToObject<Purple_3.Participant>();
                    foreach (var i in marks) prt.Evaluate(i);
                    Array.Resize(ref arr, arr.Length + 1);
                    arr[arr.Length - 1] = prt;
                }
                Purple_3.Participant.SetPlaces(arr);
                res.Add(arr);

                return (T)(object)res;
            }
            return default(T);
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var dict = DeSerializer();
            var name = dict.GetValueOrDefault("Name")?.ToObject<string>();
            var sprts = dict.GetValueOrDefault("Sportsmen");
            var arr = new Purple_4.Sportsman[0];
            foreach (var s in sprts) {
                var marks = s["Time"].ToObject<double>();

                var prt = s.ToObject<Purple_4.Sportsman>();
                prt.Run(marks);
                Array.Resize(ref arr, arr.Length + 1);
                arr[arr.Length - 1] = prt;
            }
            var res = new Purple_4.Group(name);
            res.Add(arr);
            //res.Print();
            return (Purple_4.Group)(object)res;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var dict = DeSerializer();

            var res = new Purple_5.Report();
            var rs = dict.GetValueOrDefault("Researches", null);
            var resultResearches = new List<Purple_5.Research>();

            foreach (var r in rs) {
                var rsrc = r.ToObject<Purple_5.Research>();
                var resp = r["Responses"].ToObject<Purple_5.Response[]>();

                foreach (var rsp in resp) 
                    rsrc.Add(new string[3] {rsp.Animal, rsp.CharacterTrait, rsp.Concept});

                resultResearches.Add(rsrc);
            }
            res.AddResearch(resultResearches.ToArray());  
            return (Purple_5.Report)(object)res;
        }
    }
}
