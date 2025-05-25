using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";

        private void JsonSer<T>(T obj, string filename) where T : class
        {
            SelectFile(filename);
            var json = JObject.FromObject(obj);
            json.Add("Type", obj.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            JsonSer(obj, fileName);
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            JsonSer(jumping, fileName);
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            JsonSer(skating, fileName);
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            JsonSer(group, fileName);
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            JsonSer(report, fileName);
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var json = JObject.Parse(content);
            var r = json["Type"].ToString();
            if (json["Type"].ToString() == "Lab_7.Purple_1+Participant")
            {
                var result = part(json);
                return result as T;
            }
            else if (json["Type"].ToString() == "Lab_7.Purple_1+Judge")
            {
                var result = JsonConvert.DeserializeObject<Purple_1.Judge>(content);
                return result as T;
            }
            else if (json["Type"].ToString() == "Lab_7.Purple_1+Competition")
            {
                var result = JsonConvert.DeserializeObject<Purple_1.Competition>(content);
                result.Add(json["Participants"].ToObject<JObject[]>().Select(x=>part(x)).ToArray());
                return result as T;
            }
            else return null;
        }
        private Purple_1.Participant part (JObject json)
        {
            var result = new Purple_1.Participant(json["Name"].ToString(), json["Surname"].ToString());
            result.SetCriterias(json["Coefs"].ToObject<double[]>());
            var mark = json["Marks"].ToObject<int[][]>();
            foreach (var x in mark) result.Jump(x);
            return result;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var json = JObject.Parse(content);
            Purple_2.SkiJumping result;
            int stand;
            if (json["Type"].ToString() == "Lab_7.Purple_2+ProSkiJumping")
            {
                stand = 150;
                result = JsonConvert.DeserializeObject<Purple_2.ProSkiJumping>(content);
            }
            else if (json["Type"].ToString() == "Lab_7.Purple_2+JuniorSkiJumping")
            {
                stand = 100;
                result = JsonConvert.DeserializeObject<Purple_2.JuniorSkiJumping>(content);
            }
            else return null;
            result.Add(json["Participants"].ToObject<JObject[]>().Select(x => party(x, stand)).ToArray());
            return result as T;
        }
        private Purple_2.Participant party(JObject json, int stand)
        {
           var r = new Purple_2.Participant(json["Name"].ToString(), json["Surname"].ToString());
            r.Jump(json["Distance"].ToObject<int>(), json["Marks"].ToObject<int[]>(), stand);
            return r;
         }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var json = JObject.Parse(content);
            Purple_3.Skating result;
            if (json["Type"].ToString() == "Lab_7.Purple_3+FigureSkating")
            {
                result = JsonConvert.DeserializeObject<Purple_3.FigureSkating>(content);
            }
            else if (json["Type"].ToString() == "Lab_7.Purple_3+IceSkating")
            {
                result = JsonConvert.DeserializeObject<Purple_3.IceSkating>(content);
            }
            else return null;
            Purple_3.Participant.SetPlaces(result.Participants);
            result.Add(json["Participants"].ToObject<JObject[]>().Select(x => part3(x)).ToArray()) ;
            return result as T;
        }
        private Purple_3.Participant part3 (JObject json)
        {
            var part = new Purple_3.Participant(json["Name"].ToString(), json["Surname"].ToString());
            var mark = json["Marks"].ToObject<double[]>();
            foreach (var m in mark) part.Evaluate(m);
            return part;

        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var json = JObject.Parse(content);
            var result = JsonConvert.DeserializeObject<Purple_4.Group>(content);
            result.Add(json["Sportsmen"].ToObject<JObject[]>().Select(x=>sport(x)).ToArray());
            return result;
        }
        private Purple_4.Sportsman sport(JObject json)
        {
            var sport = new Purple_4.Sportsman(json["Name"].ToString(), json["Surname"].ToString());
            sport.Run(json["Time"].ToObject<double>());
            return sport;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var json = JObject.Parse(content);
            var result = JsonConvert.DeserializeObject<Purple_5.Report>(content);
            result.AddResearch(json["Researches"].ToObject<JObject[]>().Select((x)=>reser(x)).ToArray());
            return result;
            
        }
        private Purple_5.Research reser(JObject json)
        {
            var res = new Purple_5.Research(json["Name"].ToString());
            var respons = json["Responses"].ToObject<JObject[]>();
            foreach (var x in respons)
            {
                res.Add(response(x));
            }
            return res;
        }
        private string[] response(JObject json)
        {
            var resp = new Purple_5.Response(json["Animal"].ToObject<String>(), json["CharacterTrait"].ToObject<String>(), json["Concept"].ToObject<String>());
            string[] ans = new string[] { json["Animal"].ToObject<String>(), json["CharacterTrait"].ToObject<String>(), json["Concept"].ToObject<String>()};
            return ans;
        }
    }
}
