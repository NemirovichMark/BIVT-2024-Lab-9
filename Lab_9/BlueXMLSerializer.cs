using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using System.Xml.Serialization;
using System.Reflection.Metadata;
using System.Formats.Asn1;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

    public class partSeril
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
            if(participant == null || string.IsNullOrEmpty(fileName)){
                return;
            }

            SelectFile(fileName);
            partSeril res = new partSeril(participant);
            System.Console.WriteLine("Step 1");
            var sir = new XmlSerializer(typeof(partSeril));
            System.Console.WriteLine("Step 2");
            using(var writer = new StreamWriter(FilePath)){
                sir.Serialize(writer, res);
            }
        }



        private static int[,] ArrToMat(int[][] matrix){
            int[,] res = new int[matrix.Length, matrix[0].Length];
            for(int i = 0; i < matrix.Length;i++){
                for(int j = 0; j < matrix[0].Length;j++){
                    res[i,j] = matrix[i][j];
                }
            }
            return res;
        }


        public class part_bl2{
            public string Name{get;set;}
            public string Surname{get;set;}
            //public string Type {get; set;}
            public int[][] Marks {get;set;}
            public part_bl2(){}
            public part_bl2(Blue_2.Participant pr){
                Name = pr.Name;
                Surname = pr.Surname;



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
        
        public class WaterJumpSeril{
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
            WaterJumpSeril res = new WaterJumpSeril(participant);
            var sir = new XmlSerializer(typeof(WaterJumpSeril));
            using (var writer = new StreamWriter(FilePath)){
                sir.Serialize(writer, res);
            }
        }


        public class part_b3{
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
                SelectFile(fileName);
            }
            part_b3 res = new part_b3(student);
            SelectFile(fileName);
            var sir = new XmlSerializer(typeof(part_b3));

            using(var writer = new StreamWriter(FilePath)){
                sir.Serialize(writer, res);
            }
        }

        public class sirTeam{
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

        public class sirGroup{
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
            sirGroup res = new sirGroup(participant);
            var sir = new XmlSerializer(typeof(sirGroup));
            using( var writer = new StreamWriter(FilePath)){
                sir.Serialize(writer, res);
            }

        }
         public class sirSportsman{
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

        public class sirTeam5{
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
            if(group == null || string.IsNullOrEmpty(fileName)){
                return;
            }
            SelectFile(fileName);
            object res = null;
            var sir = default(XmlSerializer);
            if(group.GetType().Name == "ManTeam" || group.GetType().Name == "WomanTeam"){
                res = new sirTeam5(group);
                sir = new XmlSerializer(typeof(sirTeam5));
            }else if(group is Blue_5.Sportsman sm){
                res = new sirSportsman(sm);
                sir = new XmlSerializer(typeof(sirSportsman));

            }

            using(var writer = new StreamWriter(FilePath)){
                sir.Serialize(writer, res);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            partSeril res;
            var sir = new XmlSerializer(typeof(partSeril));
            Blue_1.Response desir;
            using(var reader = new StreamReader(FilePath)){
                res = (partSeril)sir.Deserialize(reader);
            }
            if(res.Surname != null){
                desir = new Blue_1.HumanResponse(res.Name, res.Surname, res.Votes);
            }else{
                desir = new Blue_1.Response(res.Name, res.Votes);
            }
            return desir;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            WaterJumpSeril res;
            var sir = new XmlSerializer(typeof(WaterJumpSeril));
            Blue_2.WaterJump wj;
            using(var reader = new StreamReader(FilePath)){
                res = (WaterJumpSeril)sir.Deserialize(reader);
            }
            if(res.Type == "WaterJump5m"){
                wj = new Blue_2.WaterJump5m(res.Name, res.Bank);
            }else{
                wj = new Blue_2.WaterJump3m(res.Name, res.Bank);
            }
            System.Console.WriteLine("CHECKPOINT 1");
            for( int i = 0; i < res.Parts.Length; i++){
                Blue_2.Participant p = new Blue_2.Participant(res.Parts[i].Name, res.Parts[i].Surname);

                int[,] marks = new int[res.Parts[i].Marks.Length, res.Parts[i].Marks[0].Length];
                for(int j = 0; j < res.Parts[i].Marks.Length;j++){
                    for(int q = 0; q < res.Parts[i].Marks[0].Length;q++){
                        marks[j,q] = res.Parts[i].Marks[j][q];
                    }
                }


                System.Console.Write("CHECKPOINT 2");
                System.Console.WriteLine(i);
                for(int j = 0; j < 2; j++){
                    int[] toadd = new int[5];
                    for (int q = 0; q < 5; q++){
                        toadd[q] = marks[j, q];
                    }
                    p.Jump(toadd);
                }

                wj.Add(p);
            }
            return wj;

        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            Blue_3.Participant res = null;
            part_b3 desir;
            var sir = new XmlSerializer(typeof(part_b3));
            using( var reader = new StreamReader(FilePath)){
                desir = (part_b3)sir.Deserialize(reader);
            }
            if(desir.Type == "Participant"){
                res = new Blue_3.Participant(desir.Name, desir.Surname);
            }else if(desir.Type == "HockeyPlayer"){
                res = new Blue_3.HockeyPlayer(desir.Name, desir.Surname);
            }else{
                res = new Blue_3.BasketballPlayer(desir.Name, desir.Surname);
            }

            for(int i = 0; i < desir.Penalties.Length; i++){
                res.PlayMatch(desir.Penalties[i]);
            }
            return (T)res;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            Blue_4.Group res = default(Blue_4.Group);
            var sir = new XmlSerializer(typeof(sirGroup));
            sirGroup desir;;
            using(var reader = new StreamReader(FilePath)){
                desir = (sirGroup)sir.Deserialize(reader);
            }
            res = new Blue_4.Group(desir.Name);

            for(int i = 0; i < desir.ManTeams.Length; i++){
                var mt = default(Blue_4.ManTeam);
                if(desir.ManTeams[i] != null){
                    System.Console.Write("Pre check ");
                    System.Console.WriteLine(desir.ManTeams[i].Name);
                    if(desir.ManTeams[i].Type == "ManTeam"){

                        mt = new Blue_4.ManTeam(desir.ManTeams[i].Name);
                        System.Console.Write("222Pre check222 ");
                        System.Console.WriteLine(mt.Name);
                        for(int j = 0; j < desir.ManTeams[i].Scores.Length; j++){
                            mt.PlayMatch(desir.ManTeams[i].Scores[j]);
                        }
                    }
                }
                res.Add(mt);
            }
            

            for(int i = 0; i < desir.WomanTeams.Length; i++){
                var mt = default(Blue_4.WomanTeam);
                if(desir.WomanTeams[i] == null){
                    res.Add(mt);
                    continue;
                }else{
                    if(desir.WomanTeams[i].Type == "WomanTeam"){
                        mt = new Blue_4.WomanTeam(desir.WomanTeams[i].Name);
                        for(int j = 0; j < desir.WomanTeams[i].Scores.Length; j++){
                            mt.PlayMatch(desir.WomanTeams[i].Scores[j]);
                        }
                    }
                }
                res.Add(mt);
            }
            for(int i = 0; i < res.ManTeams.Length;i++){
                if(res.ManTeams[i] != null){
                    System.Console.WriteLine(res.ManTeams[i].Name);
                }
                
            }
            return res;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            using(var reader = new StreamReader(FilePath)){
                if(typeof(T).Name == "Sportsman"){
                    var sir = new XmlSerializer(typeof(sirSportsman));
                    sirSportsman res = (sirSportsman)sir.Deserialize(reader);
                    Blue_5.Sportsman desir = new Blue_5.Sportsman(res.Name, res.Surname);
                    desir.SetPlace(res.Place);
                    return desir as T;
                }
                
            
            else{
                var sir = new XmlSerializer(typeof(sirTeam5));
                sirTeam5 desir = (sirTeam5)sir.Deserialize(reader);
                if(desir.Type == "ManTeam"){
                    Blue_5.ManTeam res = new Blue_5.ManTeam(desir.Name);
                    for(int i = 0; i < desir.Sportsmen.Length;i++){
                        if(desir.Sportsmen[i] == null){
                            continue;
                        }
                        Blue_5.Sportsman sm = new Blue_5.Sportsman(desir.Sportsmen[i].Name, desir.Sportsmen[i].Surname);
                        sm.SetPlace(desir.Sportsmen[i].Place);
                        res.Add(sm);
                    }
                    return res as T;

                }else{
                    Blue_5.WomanTeam res = new Blue_5.WomanTeam(desir.Name);
                    for(int i = 0; i < desir.Sportsmen.Length;i++){
                        if(desir.Sportsmen[i] == null){
                            continue;
                        }
                         Blue_5.Sportsman sm = new Blue_5.Sportsman(desir.Sportsmen[i].Name, desir.Sportsmen[i].Surname);
                        sm.SetPlace(desir.Sportsmen[i].Place);

                        res.Add(sm);
                    }
                    return res as T;
                }
            }
            }
        }
        
    }
}
