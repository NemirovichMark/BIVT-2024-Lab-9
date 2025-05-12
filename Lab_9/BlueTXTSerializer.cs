using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lab_7;


namespace Lab_9
{

    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
           if(participant == null || string.IsNullOrEmpty(fileName)){
                return;
           }
           SelectFile(fileName);

           File.WriteAllText(FilePath, string.Empty);

           using(StreamWriter writer = File.AppendText(FilePath)){
                var s = participant.Name + " " + participant.Votes;
                if(participant is Blue_1.HumanResponse hr){
                    s = s + " " + hr.Surname;
                }
                writer.WriteLine(s);
           }
        }

  

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump waterJump, string fileName)
        {
            if (waterJump == null || string.IsNullOrEmpty(fileName)) {
                return;
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using (StreamWriter writer = File.AppendText(FilePath)) 
            {
                var s = waterJump.GetType().Name + " " + waterJump.Name + " " + waterJump.Bank;
                writer.WriteLine(s);
                for (int i = 0; i < waterJump.Participants.Length; i ++) {
                    var r = waterJump.Participants[i].Name + " " + waterJump.Participants[i].Surname + " |";
                    var marks = "";
                     for (int j = 0; j < 2; j++) {
                        for (int q = 0; q < 5; q++) {
                            marks += waterJump.Participants[i].Marks[j, q] + " ";
                        }

                    }
                     writer.Write(r);
                     writer.WriteLine(marks);
            }
            
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) {
                return;
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using (StreamWriter writer = File.AppendText(FilePath)) 
            {
                var s = student.GetType().Name + " " + student.Name + " " + student.Surname;
                writer.WriteLine(s);
                string r = "";
                for (int i = 0; i < student.Penalties.Length; i ++) {
                     r += student.Penalties[i].ToString() + " ";
                }
                r = r.Remove(r.Length - 1);
                writer.Write(r);

            }
        }
        public string teams_txt(Blue_4.Team team){
            string res = "";
            res += team.Name + " ";
            for(int i = 0; i < team.Scores.Length; i++){
                res += team.Scores[i] + " ";
            }
            res = res.Remove(res.Length - 1);
            return res;
        }
        public void sirTeam(Blue_4.Group gr, StreamWriter writer , string gend){
            writer.WriteLine(gend + " ");
            if(gend == "Men"){
                for(int i = 0; i < gr.ManTeams.Length;i++){
                    if(gr.ManTeams[i] != null){
                        writer.WriteLine(teams_txt(gr.ManTeams[i]));
                    }
                }
            }else{
                System.Console.WriteLine("0000");
                System.Console.WriteLine(gr.WomanTeams.Length);
                for(int i = 0; i < gr.WomanTeams.Length;i++){
                    if(gr.WomanTeams[i] != null){
                        System.Console.Write("?---------- ");
                        System.Console.WriteLine(gr.WomanTeams[i]);
                        writer.WriteLine(teams_txt(gr.WomanTeams[i]));
                    }
                }
                System.Console.WriteLine("---6---");
            }

        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            System.Console.WriteLine("SerilStart");
            if (participant == null ||string.IsNullOrEmpty(fileName)) {
                return;
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using(StreamWriter writer = File.AppendText(FilePath))
            {
                
                System.Console.WriteLine(participant.Name);
                writer.WriteLine(participant.Name);
                
                sirTeam(participant, writer, "Men");
                sirTeam(participant, writer, "Womenn");
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            File.WriteAllText(FilePath, string.Empty);
            using(StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine(group.GetType().Name + " " + group.Name);

                for(int i = 0; i < group.Sportsmen.Length; i++)
                {   
                    if (group.Sportsmen[i] != null) 
                    {
                        var s = group.Sportsmen[i].Name + " " + group.Sportsmen[i].Surname + " " + group.Sportsmen[i].Place;
                        writer.WriteLine(s);

                        
                    }
                }
            }
        }
        
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var res = default(Blue_1.Response);;
            foreach(var row in text.Split(Environment.NewLine)){
                if(row.Contains(' ')){
                    var res_s = row.Split(' ');
                    if(res_s.Length == 3){
                        res = new Blue_1.HumanResponse(res_s[0].Trim(), res_s[2].Trim(), Int32.Parse(res_s[1].Trim()));
                    }else{
                        //System.Console.WriteLine(res_s[1].Trim());
                        res = new Blue_1.Response(res_s[0].Trim(), Int32.Parse(res_s[1].Trim()));
                    }
                }
            }
            return res;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var res = default(Blue_2.WaterJump);
            int k = 0;
            
            foreach(var row in text.Split(Environment.NewLine)){
                if(k == 0){
                    if(row.Contains(' ')){
                        var res_s = row.Split(' ');
                        if(res_s[0] == "WaterJump3m"){
                            res = new Blue_2.WaterJump3m(res_s[1].Trim(), Int32.Parse(res_s[2].Trim()));
                        }else{
                            //System.Console.WriteLine(res_s[1].Trim());
                            res = new Blue_2.WaterJump5m(res_s[1].Trim(), Int32.Parse(res_s[2].Trim()));
                        }
                        k = 1;
                        System.Console.WriteLine("JUMP ");
                        System.Console.WriteLine(res.Name);
                    }
                }
                else if(row.Contains('|')){
                    var res_s = row.Split('|');
                    var res_p = res_s[0].Split(' ');
                    var part = new Blue_2.Participant(res_p[0], res_p[1]);
                    System.Console.WriteLine("Part ");
                    System.Console.WriteLine(part.Name);
                    if(res_s[1].Contains(' ')){
                        
                        var res_m = res_s[1].Split(' ');
                        var marks = new int[res_m.Length/2];

                        System.Console.WriteLine("Marks ");
                        System.Console.WriteLine(res_m.Length);

                        for(int i = 0; i < 5;i++){
                            System.Console.WriteLine(res_m[i]);
                            marks[i] = Int32.Parse(res_m[i]);
                        }
                        part.Jump(marks);
                        for(int i = 5; i < 10;i++){
                            marks[i - 5] = Int32.Parse(res_m[i]);
                        }
                        part.Jump(marks);
                    }
                    res.Add(part);

                }
            }
            return res;
            
        }

        public override T DeserializeBlue3Participant<T>(string fileName) where T: class
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var res = default(Blue_3.Participant);
            int k = 0;
            System.Console.WriteLine("00000");
            foreach(var row in text.Split(Environment.NewLine)){
                if(k == 0 && row.Contains(' ')){
                    var res_s = row.Split(' ');
                        //System.Console.WriteLine(res_s[1].Trim());
                    if(res_s[0] == "BasketballPlayer"){
                        res = new Blue_3.BasketballPlayer(res_s[1].Trim(), res_s[2].Trim());
                    }else if(res_s[0] == "HockeyPlayer"){
                        res = new Blue_3.HockeyPlayer(res_s[1].Trim(), res_s[2].Trim());
                    }else{
                        res = new Blue_3.Participant(res_s[1].Trim(), res_s[2].Trim());
                    }
                    k += 1;
                }else if(row.Contains(' ')){

                    System.Console.WriteLine("Desir ");
                    System.Console.WriteLine(row);
                    var res_s = row.Split(' ');
                    System.Console.WriteLine(row);
                    for(int i = 0; i < res_s.Length;i++){
                        res.PlayMatch( Int32.Parse(res_s[i]));
                    }

                }
            }
            
            return res as T;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var res = default(Blue_4.Group);
            int k = 0;
            System.Console.WriteLine("11111");
            foreach(var row in text.Split(Environment.NewLine)){
                System.Console.Write("Check: ");
                System.Console.WriteLine(row);
                if(string.IsNullOrEmpty(row)){
                    break;
                }
                if(k == 0){
                    System.Console.WriteLine("step 1");

                    var res_s = row;
                    System.Console.WriteLine(res_s);
                    res = new Blue_4.Group(res_s);
                    k = 1;
                }else if(k == 1){
                    System.Console.WriteLine("step 2");
                    var res_s = row.Split(' ');
                    System.Console.WriteLine(res_s[0]);
                    System.Console.WriteLine(res_s[0].Length);
                    if(res_s[0] == "Womenn"){
                        //System.Console.WriteLine("44");
                        k = 2;
                    }

                    else if(res_s[0] != "Men"){
                        var mt = new Blue_4.ManTeam(res_s[0]);
                        System.Console.Write("Add: ");
                        System.Console.WriteLine(row);
                        if(res_s.Length > 1){
                            for(int i = 1; i < res_s.Length; i++){
                                mt.PlayMatch(Int32.Parse(res_s[i]));
                            }
                        
                        }
                        res.Add(mt);
                        
                    }

                }else{
                    var res_s = row.Split(' ');
                    System.Console.WriteLine("step 3");
                    var mt = new Blue_4.WomanTeam(res_s[0]);
                    if(res_s[0] != "Men"){
                        
                        for(int i = 1; i < res_s.Length; i++){
                            mt.PlayMatch(Int32.Parse(res_s[i]));
                        }
                        
                    }
                    res.Add(mt);
                }
            }
            
            return res;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var res = default(Blue_5.Team);
            int k = 0;
            System.Console.WriteLine("11111");
            foreach(var row in text.Split(Environment.NewLine)){
                System.Console.Write("Check: ");
                System.Console.WriteLine(row);
                if(string.IsNullOrEmpty(row)){
                    break;
                }
                if(k == 0){
                    System.Console.WriteLine("step 1");

                    var res_s = row.Split(' ');
                    System.Console.WriteLine(res_s);
                    if(res_s[0] == "ManTeam"){
                        res = new Blue_5.ManTeam(res_s[1]);
                    }else{
                        res = new Blue_5.WomanTeam(res_s[1]);
                    }
                    
                    k = 1;
                }else{
                    var res_s = row.Split(' ');
                    var sm = new Blue_5.Sportsman(res_s[0], res_s[1]);
                    //System.Console.WriteLine(Int32.Parse(res_s[1]));
                    sm.SetPlace(Int32.Parse(res_s[2]));
                    res.Add(sm);
                }
            }
            return res as T;
        }
       
    }
}
