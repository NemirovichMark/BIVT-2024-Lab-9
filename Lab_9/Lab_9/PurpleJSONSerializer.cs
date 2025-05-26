using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_3;
using static Lab_7.Purple_4;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            JObject jsons = JObject.FromObject(obj);  
            jsons.Add("type", obj.GetType().Name);
            //string s = JsonConvert.SerializeObject(obj);
            File.WriteAllText(FilePath, jsons.ToString());  
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            JObject jsons=JObject.FromObject(jumping);   
            jsons.Add("type",  jumping.GetType().Name); 


            File.WriteAllText(FilePath, jsons.ToString());
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);   
            JObject jsons = JObject.FromObject(skating);
            jsons.Add("type", skating.GetType().Name);   

            File.WriteAllText(FilePath, jsons.ToString());
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);


            string s = JsonConvert.SerializeObject(group);
                 
            File.WriteAllText(FilePath,s);  
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);   
            string s = JsonConvert.SerializeObject(report);
            File.WriteAllText(FilePath, s);   
        }



        //Deserialize
        public override T DeserializePurple1<T>(string fileName)  
        { 
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            JObject j = JObject.Parse(json);

            if (j["type"].ToString()== nameof(Purple_1.Participant))   
            {
                Purple_1.Participant p = new Purple_1.Participant(j["Name"].ToString(), j["Surname"].ToString());
                p.SetCriterias(j["Coefs"].ToObject<double[]>());
                int[][] mar = j["Marks"].ToObject<int[][]>();
                for (int i = 0; i < mar.Length; i++) p.Jump(mar[i]);   
                return (T)(Object)p;
            }
             
            else if(j["type"].ToString() == nameof(Purple_1.Judge)){   
                return (T) (Object) new Purple_1.Judge(j["Name"].ToString(), j["Marks"].ToObject<int[]>());  
            }
               
            else if(j["type"].ToString() == nameof(Purple_1.Competition))   
            {
                var arr = j["Participants"].ToObject<JObject[]>();
                var arr1 = new Purple_1.Participant[arr.Length];
                for(int i = 0; i<arr1.Length; i++)   
                {
                    arr1[i]= new Purple_1.Participant(arr[i]["Name"].ToString(),  arr[i]["Surname"].ToString());  
                    arr1[i].SetCriterias( arr[i]["Coefs"].ToObject<double[]>());
                    int[][] mar = arr[i]["Marks"].ToObject<int[][]>();
                    for (int k = 0; k <mar.Length; k++) arr1[i].Jump(mar[k]); 
                }
                Purple_1.Judge[] arr2 = j["Judges"].ToObject<Purple_1.Judge[]>();
                var res = new Purple_1.Competition(arr2);
                res.Add(arr1);
                return (T) (Object)res;
            }

            return null;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            string s = File.ReadAllText(FilePath);
            var jobj = JObject.Parse(s);
            Purple_2.SkiJumping ski;

            if (jobj["type"].ToString() ==nameof(Purple_2.ProSkiJumping)) ski = new Purple_2.ProSkiJumping();
                
            else if (jobj["type"].ToString() ==nameof(Purple_2.JuniorSkiJumping))  ski  = new Purple_2.JuniorSkiJumping();  

            else return null;

            for(int i = 0; i< jobj["Participants"].ToObject<Purple_2.Participant[]>().Length; i++)   
            {
                var part = JsonConvert.DeserializeObject<Purple_2.Participant>(jobj["Participants"][i].ToString());
                part.Jump(int.Parse(jobj["Participants"][i]["Distance"].ToString()),  jobj["Participants"][i]["Marks"].ToObject<int[]>(), int.Parse(jobj["Standard"].ToString()));
                ski.Add(part);
            }
            return (T)(Object) ski;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string jsons = File.ReadAllText(FilePath);
            JObject jo = JObject.Parse(jsons);
            Purple_3.Skating sk;   
            if (jo["type"].ToString()  == nameof(Purple_3.FigureSkating)) sk = new Purple_3.FigureSkating(jo["Moods"].ToObject<double[]>(), false);
              
            else if (jo["type"].ToString()==nameof(Purple_3.IceSkating)) sk = new Purple_3.IceSkating(jo["Moods"].ToObject<double[]>(), false);

            else return null;
              
            JObject[] arr = jo["Participants"].ToObject<JObject[]>();
            for(int i = 0; i<arr.Length; i++)
            {  
                var p = JsonConvert.DeserializeObject<Purple_3.Participant>(arr[i].ToString());  
                double[] mar = arr[i]["Marks"].ToObject<double[]>();
                foreach (double m in mar)   p.Evaluate(m);  
                sk.Add(p);
            }
            return (T) (Object) sk;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);   
            string jsons = File.ReadAllText(FilePath);   
            JObject jo = JObject.Parse(jsons);
            var gr = new Purple_4.Group(jo["Name"].ToString());   
            var parts =jo["Sportsmen"].ToObject<JObject[]>();

            for(int i = 0; i< parts.Length;i++)
            {
                var part = JsonConvert.DeserializeObject<Purple_4.Sportsman>(parts[i].ToString());
                part.Run(double.Parse(parts[i]["Time"].ToString()));  
                gr.Add(part);
            }
            return gr;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName); 
            string s = File.ReadAllText(FilePath);
            var jo = JObject.Parse(s);  
            Purple_5.Report rep = new Purple_5.Report();  
            JObject[] jarr = jo["Researches"].ToObject<JObject[]>();  

            for (int i=0; i< jarr.Length; i++)  
            { 
                var research = new Purple_5.Research(jarr[i]["Name"].ToString());
                Purple_5.Response[] resps = jarr[i]["Responses"].ToObject<Purple_5.Response[]>();
                   
                for(int j = 0; j<resps.Length;j++)
                {
                    research.Add(new string[] { resps[j].Animal, resps[j].CharacterTrait, resps[j].Concept });   
                }
                rep.AddResearch(research);   
            }
            return rep;
        }
    }
}

