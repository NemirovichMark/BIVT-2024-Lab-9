using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            Blue_1.Response desiral = default(Blue_1.Response);
            Console.WriteLine(FilePath);
            string json = File.ReadAllText(FilePath);
            var ser = JsonConvert.DeserializeObject<partSeril>(json);
            if(ser.Type == "Response")
            {
                desiral = new Blue_1.Response(ser.Name, ser.Votes);
            }
            else
            {
                desiral = new Blue_1.HumanResponse(ser.Name,ser.Surname, ser.Votes);
            }
            return desiral;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            Blue_2.WaterJump wj = default(Blue_2.WaterJump);
            string json = File.ReadAllText(FilePath);
            var ser = JsonConvert.DeserializeObject<WaterJumpSeril>(json);
            if(ser.Type == "WaterJump5m"){
                wj = new Blue_2.WaterJump5m(ser.Name, ser.Bank);
            }
            else{
                wj = new Blue_2.WaterJump3m(ser.Name, ser.Bank);
            }

            for(int i = 0; i < ser.Parts.Length; i++){
                Blue_2.Participant p = new Blue_2.Participant(ser.Parts[i].Name, ser.Parts[i].Surname);
                
                System.Console.WriteLine(ser.Parts[i].Marks);

                 int[,] marks = new int[ser.Parts[i].Marks.Length, ser.Parts[i].Marks[0].Length];
                    for(int j = 0; j < ser.Parts[i].Marks.Length;j++){
                        for(int q = 0; q < ser.Parts[i].Marks[0].Length;q++){
                            marks[j,q] = ser.Parts[i].Marks[j][q];
                        }
                    }
                  


                for(int j = 0; j < 2;j++){
                    int[] res = new int[5];
                    for(int q = 0; q < 5; q++){
                        res[q] = marks[j, q];
                    }
                    p.Jump(res);
                }
                wj.Add(p);

            }
            return wj;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            Blue_3.Participant part = default(Blue_3.Participant);
            string json = File.ReadAllText(FilePath);
            var desir = JsonConvert.DeserializeObject<part_b3>(json);
            if(desir.Type == "Participant"){
                part = new Blue_3.Participant(desir.Name, desir.Surname);
            }else if(desir.Type == "HockeyPlayer"){
                part = new Blue_3.HockeyPlayer(desir.Name, desir.Surname);
            }else{
                part = new Blue_3.BasketballPlayer(desir.Name, desir.Surname);
            }
            for(int i = 0 ; i < desir.Penalties.Length; i++){
                part.PlayMatch(desir.Penalties[i]);
            }
            return (T)part;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            
            string json = File.ReadAllText(FilePath);
            var dis  = JsonConvert.DeserializeObject<sirGroup>(json);
            var res = new Blue_4.Group(dis.Name);
            for(int i = 0; i < dis.ManTeams.Length; i++){
                var mt = default(Blue_4.ManTeam);
                if(dis.ManTeams[i] == null){
                    res.Add(mt);
                    continue;
                }
                else if(dis.ManTeams[i].Type == "ManTeam"){
                    mt = new Blue_4.ManTeam(dis.ManTeams[i].Name);
                    for(int j = 0; j < dis.ManTeams[i].Scores.Length; j++){
                        mt.PlayMatch(dis.ManTeams[i].Scores[j]);
                    }
                }
                res.Add(mt);


            }

            for(int i = 0; i < dis.WomanTeams.Length; i++){
                var mt = default(Blue_4.WomanTeam);
                if(dis.WomanTeams[i] == null){
                    res.Add(mt);
                    continue;
                }
                else if(dis.WomanTeams[i].Type == "WomanTeam"){
                    mt = new Blue_4.WomanTeam(dis.WomanTeams[i].Name);
                    for(int j = 0; j < dis.WomanTeams[i].Scores.Length; j++){
                        mt.PlayMatch(dis.WomanTeams[i].Scores[j]);
                    }
                }
                res.Add(mt);


            }
            return res;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            if(typeof(T).Name == "Sportsman"){
                sirSportsman sir = JsonConvert.DeserializeObject<sirSportsman>(json);
                Blue_5.Sportsman res = new Blue_5.Sportsman(sir.Name, sir.Surname);
                res.SetPlace(sir.Place);
                return sir as T;
            }else{
                sirTeam5 desir = JsonConvert.DeserializeObject<sirTeam5>(json);
                if(desir.Type == "WomanTeam"){
                    Blue_5.WomanTeam res = new Blue_5.WomanTeam(desir.Name);
                    for(int i = 0; i < desir.Sportsmen.Length;i++){
                        if(desir.Sportsmen[i] == null){
                            continue;
                        }
                        var sm = new Blue_5.Sportsman(desir.Sportsmen[i].Name, desir.Sportsmen[i].Surname);
                        sm.SetPlace(desir.Sportsmen[i].Place);
                        res.Add(sm);
                    }
                    return res as T;
                }else{
                    Blue_5.ManTeam res = new Blue_5.ManTeam(desir.Name);
                    for(int i = 0; i < desir.Sportsmen.Length;i++){
                        if(desir.Sportsmen[i] == null){
                            continue;
                        }
                        var sm = new Blue_5.Sportsman(desir.Sportsmen[i].Name, desir.Sportsmen[i].Surname);
                        sm.SetPlace(desir.Sportsmen[i].Place);
                        res.Add(sm);
                    }
                    return res as T;
                }
            }
        }

        private class partSeril
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }

            public partSeril() { }
            public partSeril(Blue_1.Response resp) {
                Type = resp.GetType().Name;
                Name = resp.Name;
                Votes = resp.Votes;
                if(resp is Blue_1.HumanResponse hr)
                {
                    Surname = hr.Surname;
                }
            }

        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            Console.WriteLine("SerializeBlue1Response");
           if(participant == null || String.IsNullOrEmpty(fileName))
            {
                return;
            }
            Console.WriteLine("fileName");

            SelectFile(fileName);
            Console.WriteLine(FilePath);
            var partSeril = new partSeril(participant);

            var json = JsonConvert.SerializeObject(partSeril, Formatting.Indented);
            Console.WriteLine(json);
            File.WriteAllText(FilePath, json);
        }


        private class part_bl2{
            public string Name{get;set;}
            public string Surname{get;set;}
            //public string Type {get; set;}
            public int[][] Marks {get;set;}
            public part_bl2(){}
            public part_bl2(Blue_2.Participant pr){
                Name = pr.Name;
                Surname = pr.Surname;
                //Type = pr.GetType().Name;
                int[][] res = new int[pr.Marks.GetLength(0)][];
                for(int i = 0; i < pr.Marks.GetLength(0);i++){
                    res[i] = new int[pr.Marks.GetLength(1)];
                    for(int j = 0; j < pr.Marks.GetLength(1);j++){
                        res[i][j] = pr.Marks[i,j];
                    }
                }
            
                Marks = res;
            }
        }
        private class WaterJumpSeril{
            public string Type {get;set;}
            public string Name {get;set;}

            public int Bank{get;set;}
            public part_bl2[] Parts {get;set;}
            public WaterJumpSeril(){}
            public WaterJumpSeril(Blue_2.WaterJump wj){
                Type = wj.GetType().Name;
                Name = wj.Name;
                Bank = wj.Bank;
                Parts = new part_bl2[wj.Participants.Length];
                for(int i = 0; i < Parts.Length; i++){
                    Parts[i] = new part_bl2(wj.Participants[i]);
                }

            }
        }

        
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if(participant == null || string.IsNullOrEmpty(fileName)){
                return;
            } 
            SelectFile(fileName);
            var wjSer = new WaterJumpSeril(participant);
            var json = JsonConvert.SerializeObject(wjSer);
            File.WriteAllText(FilePath, json);
           // SerializeObject(participant, fileName);
        }
        private class part_b3{
            public string Type {get;set;}
            public string Name {get;set;}
            public string Surname{get;set;}
            public int[] Penalties{get;set;}
            public part_b3(){}
            public part_b3(Blue_3.Participant p){
                Type = p.GetType().Name;
                Name = p.Name ;
                Surname = p.Surname;
                Penalties = p.Penalties;
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
           if(student == null || string.IsNullOrEmpty(fileName)){
                return;
           }
           SelectFile(fileName);
           var seril = new part_b3(student);
           var json = JsonConvert.SerializeObject(seril, Formatting.Indented);
           File.WriteAllText(FilePath, json);
        }

        private class sirTeam{
            public string Type {get; set;}
            public string Name {get; set;}
            public int[] Scores {get; set;}
            public sirTeam(){}
            public sirTeam(Blue_4.Team t){
                Type = t.GetType().Name;
                Name = t.Name ;
                Scores = t.Scores;
            }

        }

        private class sirGroup{
            public string Name{get;set;}
            public sirTeam[] ManTeams {get;set;}
            public sirTeam[] WomanTeams {get;set;}

            public sirGroup(){}
            public sirGroup(Blue_4.Group g){
                Name = g.Name;

                ManTeams = new sirTeam[g.ManTeams.Length];
                for(int i = 0; i < g.ManTeams.Length; i++){
                    if(g.ManTeams[i] == null){
                        continue;
                    }
                    ManTeams[i] = new sirTeam(g.ManTeams[i]);
                }

                WomanTeams = new sirTeam[g.WomanTeams.Length];


                for(int i = 0; i < g.WomanTeams.Length; i++){
                    if(g.WomanTeams[i] == null){
                        continue;
                    }
                    WomanTeams[i] = new sirTeam(g.WomanTeams[i]);
                }
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {

            if(participant == null || string.IsNullOrEmpty(fileName)){
                return;
            }
            SelectFile(fileName);
            var ser = new sirGroup(participant);
            var json = JsonConvert.SerializeObject(ser);
            
            File.WriteAllText(FilePath, json);
        }

        private class sirSportsman{
            public string Name{get;set;}
            public string Surname{get;set;}
            public int Place {get; set;}

            public sirSportsman(){}
            public sirSportsman(Blue_5.Sportsman sm){
                Name = sm.Name;
                Surname = sm.Surname;
                Place = sm.Place;
            }
        }

        private class sirTeam5{
            public string Type{get;set;}
            public string Name{get;set;}
            public sirSportsman[] Sportsmen {get;set;}
            public sirTeam5(){}
            public sirTeam5(Blue_5.Team t){
                Type = t.GetType().Name;
                Name = t.Name;
                Sportsmen = new sirSportsman[t.Sportsmen.Length];
                for(int i = 0; i < t.Sportsmen.Length; i++){
                    if(t.Sportsmen[i] != null){
                        Sportsmen[i] = new sirSportsman(t.Sportsmen[i]);
                    }
                }
            }
        }
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
           if(group == null || string.IsNullOrEmpty(fileName)){return;}

           SelectFile(fileName);

           object sir = null;
           if(group.GetType().Name == "ManTeam" || group.GetType().Name == "WomanTeam"){
            sir = new sirTeam5(group);
           }else if(group is Blue_5.Sportsman s){
            sir = new sirSportsman(s);
           }
           string json = JsonConvert.SerializeObject(sir);
           File.WriteAllText(FilePath, json);
        }
       
   
    }
}