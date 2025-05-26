using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Lab_7.Purple_3;
using System.Data.SqlTypes;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer   
    {
        public override string Extension => "txt";   
        public override void SerializePurple1<T>( T obj, string fileName) //where T : class
        {
            string res = "";
            if (obj is Purple_1.Participant obj1)   
            {
                /*
                res.Append($"{obj1.GetType().Name}{Environment.NewLine}" +
                    $"{obj1.Name} {obj1.Surname}{Environment.NewLine}");
                res.AppendLine(String.Join(" ", obj1.Coefs));
                for(int i = 0; i < obj1.Marks.GetLength(0); i++)
                {
                    for (int j = 0; j < obj1.Marks.GetLength(1); j++) res.Append(obj1.Marks[i, j].ToString()+" ");
                    res.AppendLine();
                }
                res.AppendLine(obj1.TotalScore.ToString());*/
                res = SerPurple1Part(obj1);

            }
            else if (obj is Purple_1.Judge  obj2)
            {
                res = SerPurple1Judge(obj2);


            }
            else if ( obj is Purple_1.Competition obj3)
            {
                res=SerPurple1Comp(obj3);
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, res);
        }
        private string SerPurple1Part(Purple_1.Participant obj1)
        {
            StringBuilder res = new StringBuilder();
            res.Append($"{obj1.GetType().Name}{Environment.NewLine}" +
                    $"{obj1.Name} {obj1.Surname}{Environment.NewLine}");
            res.AppendLine(String.Join(" ", obj1.Coefs));   
            for (int i = 0; i <obj1.Marks.GetLength(0); i++)
            {
                for (int j = 0; j < obj1.Marks.GetLength(1); j++) res.Append(obj1.Marks[i, j].ToString() + " ");
                res.AppendLine();
            }
            res.AppendLine(obj1.TotalScore.ToString() );
            return res.ToString();   
        }
        private string SerPurple1Judge(Purple_1.Judge obj2) 
        {
            return  $"{obj2.GetType().Name}{Environment.NewLine}" +
                    $"{obj2.Name}{Environment.NewLine}{String.Join(" ",  obj2.Marks)}{Environment.NewLine}";   
        }
        private string SerPurple1Comp(Purple_1.Competition  obj3)
        {
            StringBuilder res =new StringBuilder();
            res.AppendLine( obj3.GetType().Name );
            res.AppendLine(obj3.Judges.Length.ToString());
            foreach (var j in obj3.Judges) res.Append(SerPurple1Judge(j));

            res.AppendLine(obj3.Participants.Length.ToString());   
            foreach (var p in obj3.Participants) res.Append(SerPurple1Part(p));    

            return  res.ToString();
        }

        //Purple_2
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName) //where T : Purple_2.SkiJumping
        {
            SelectFile(fileName);   
            StringBuilder res = new StringBuilder();
            //if (jumping is Purple_2.JuniorSkiJumping)    res.Append( "JuniorSkiJumping" );
            res.AppendLine(jumping.GetType().Name);  
            res.AppendLine($"Name:{jumping.Name}{Environment.NewLine}Standard:{jumping.Standard}");
            res.AppendLine($"Count:{jumping.Participants.Length}");
            foreach(var part in jumping.Participants)    
            {
                res.AppendLine($"name:{part.Name}{Environment.NewLine}surname:{part.Surname}{Environment.NewLine}"+
                    $"distance:{part.Distance}{Environment.NewLine}marks:{ String.Join(" ",  part.Marks)}");
            }
            File.WriteAllText(FilePath,  res.ToString());    
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)   //where T : Purple_3.Skating   
        {
            SelectFile(fileName);
            StringBuilder s = new StringBuilder();
            s.AppendLine(skating.GetType().Name);
            s.AppendLine($"moods:{ArrayToString(skating.Moods)}");
            s.AppendLine(skating.Participants.Length.ToString());

            foreach( var part in skating.Participants) 
            {
                s.AppendLine($"name:{part.Name}{Environment.NewLine}surname:{part.Surname}");
                s.AppendLine("marks:"+ArrayToString(part.Marks));
                s.AppendLine("places:"+ArrayToString(part.Places));
                s.AppendLine("score:" + part.Score.ToString());   
                
            }
            File.WriteAllText(FilePath, s.ToString());  


        }
        private string ArrayToString(double[] arr)
        {
            return string.Join(" ", arr).Trim();

        }
        private string ArrayToString(int[] arr)
        {
            return string.Join(" ", arr).Trim();

        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(group.Name);   
            sb.AppendLine(group.Sportsmen.Length.ToString());   

            for(int i = 0; i<group.Sportsmen.Length; i++)   
            {
                sb.AppendLine(group.Sportsmen[i].GetType().Name);   
                sb.AppendLine($"{group.Sportsmen[i].Name} {group.Sportsmen[i].Surname} {group.Sportsmen[i].Time}");
            }

            File.WriteAllText(FilePath, sb.ToString() );   
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            StringBuilder sb= new StringBuilder();  
            sb.AppendLine(report.Researches.Length.ToString());  

            for(int i = 0;i <report.Researches.Length;i++)  
            {
                sb.AppendLine(report.Researches[i].Name);
                sb.AppendLine(report.Researches[i].Responses.Length.ToString());

                for(int j = 0;j< report.Researches[i].Responses.Length; j++)   
                {
                    sb.AppendLine($"{report.Researches[i].Responses[j].Animal},{report.Researches[i].Responses[j].CharacterTrait},{report.Researches[i].Responses[j].Concept}".Trim());
                }
            }

            File.WriteAllText (FilePath, sb.ToString());  


        }


        //Deserialize

        public override T DeserializePurple1<T>(string fileName) //where T : class
        {
            //return (T)(Object)new Purple_1.Participant("a", "b");
            SelectFile(fileName);
            string[] strings =File.ReadAllText(FilePath).Split(Environment.NewLine);
            string type = strings[0].Trim();  
            
            if( type == nameof(Purple_1.Participant))   
            {
                var p = DeserPurple1Part(strings);  
                return (T) (Object) p ;   
            }
            else if(type== nameof(Purple_1.Judge))   
            {
                var j = DeserPurple1Judge(strings);
                return (T) (Object)j;
            }
            else if (type ==nameof(Purple_1.Competition))
            {
                var c = DeserPurple1Comp(strings);
                return (T)(Object) c;
            }
            return null;

        }
        private Purple_1.Participant DeserPurple1Part(string[] strings)
        {
            string name = strings[1].Split()[0];  
            string surn = strings[1].Split()[1];
            string[] c = strings[2].Trim().Split();
            double[] coefs = new double[c.Length];
            for (int i = 0; i< coefs.Length; i++) coefs[i]=double.Parse(c[i]);
            var part = new Purple_1.Participant(name, surn);
            part.SetCriterias(coefs);
            for(int i =0; i< 4; i++ )   
            {
                string[] str = strings[3 + i].Trim().Split();   
                int[] jum = str.Select(x =>int.Parse(x)).ToArray();  
                part.Jump(jum);   
            }
            return  part;
        }
        private Purple_1.Judge DeserPurple1Judge(string[] strings)
        {
            
            string name = strings[1];   
            string[] str = strings[2].Trim().Split();
            int[] m = str.Select(x=> int.Parse(x)).ToArray();   
            return new Purple_1.Judge(name, m);
        }
        private Purple_1.Competition DeserPurple1Comp(string[] strings)
        {
            
            //return new Purple_1.Competition(new Purple_1.Judge[0]);
            int countjs = int.Parse(strings[1]);
            Purple_1.Judge[] judgs = new Purple_1.Judge[countjs];  
            for(int i = 0; i< judgs.Length; i++)
            {
                string[] str = new string[3] { strings[2+i*3], strings[3+i*3], strings[4+i*3] };
                judgs[i] = DeserPurple1Judge(str);
            }
            var c = new Purple_1.Competition(judgs);
            for(int i = 0; i<int.Parse(strings[countjs*3+2]); i++)
            {
                string[] str = new string[8];
                for (int s=0; s <8; s++) str[s] = strings[1+ countjs * 3 +1 + s+1 + i * 8];
                
                c.Add(DeserPurple1Part(str)); 
            }
            return c;
        }
        //Purple_2

        public override T DeserializePurple2SkiJumping<T>(string fileName) //where T : Purple_2.SkiJumping
        {
            SelectFile(fileName);
            string[] str = File.ReadAllLines(FilePath);
            Purple_2.SkiJumping ski=null;  

            if (str[0] == nameof(Purple_2.JuniorSkiJumping)) ski = new Purple_2.JuniorSkiJumping();
            else if (str[0] == nameof(Purple_2.ProSkiJumping)) ski = new Purple_2.ProSkiJumping();
            else return null;  
            for(int i=0; i < int.Parse(str[3].Split(":")[1]); i++)  
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                for(int s =4+i*4; s<8+i*4; s++)
                {
                    dict[str[s].Split(":")[0]]= str[s].Split(":")[1];  
                }

                Purple_2.Participant part = new Purple_2.Participant(dict["name"], dict["surname"]);
                part.Jump(int.Parse(dict["distance"]), dict["marks"].Split().Select(x=>int.Parse(x)).ToArray(), ski.Standard);
                ski.Add(part);
            }

            return (T)  (Object)ski;  
            //return (T)(Object)new Purple_2.ProSkiJumping();
        }
        public override T DeserializePurple3Skating<T>(string fileName)// where T : Purple_3.Skating
        {
            SelectFile(fileName);
            string[] str = File.ReadAllLines(FilePath);   

            Purple_3.Skating sk=null;
            if (str[0] == "FigureSkating") sk = new Purple_3.FigureSkating(StringTodArr(str[1].Split(":")[1]), false);
            else if (str[0] == "IceSkating") sk = new Purple_3.IceSkating(StringTodArr(str[1].Split(":")[1]), false);  
            else return (T) (Object)sk;  
            int count = int.Parse(str[2]);
            for(int c = 0; c<count; c++)
            {
                Dictionary<string, string> d = new Dictionary<string, string>();  
                for(int i = 3+5*c; i <3+5*(c+1); i++)
                {
                    d[str[i].Split(":")[0]] = str[i].Split(":")[1]; 
                }
                var part = new Purple_3.Participant(d["name"],  d["surname"]);   
                double[] mar = StringTodArr(d["marks"]);
                foreach (double m in mar)   part.Evaluate(m);   
                sk.Add(part);  
            }
            Purple_3.Participant.SetPlaces(sk.Participants);
            return (T) (Object)sk;
           
            //return (T)(Object)new Purple_3.FigureSkating(new double[] { 1 });
        }
        private double[] StringTodArr(string s)
        {
            return s.Split().Select(x => double.Parse(x)).ToArray();
        }
        private int[] StringToiArr(string s)
        {
            return s.Split().Select(x => int.Parse(x)).ToArray();
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string[] res = File.ReadAllLines(FilePath);
            Purple_4.Group obj = new Purple_4.Group(res[0]);   

            for(int i = 0; i< int.Parse(res[1]); i++)
            {
                Purple_4.Sportsman sp = null;  
                string[] data = res[3+ i * 2].Split();
                string t = res[2 + i*2];
                if (t == nameof(Purple_4.SkiWoman))  sp = new Purple_4.SkiWoman(data[0], data[1]);  
                else if (t == nameof(Purple_4.SkiMan))  sp = new Purple_4.SkiMan(data[0], data[1]);
                else if (t == nameof(Purple_4.Sportsman))  sp = new Purple_4.Sportsman(data[0],  data[1]);  
                sp.Run(double.Parse(data[2]));
                obj.Add(sp);  

            }
            return obj;

            //return new Purple_4.Group("w");
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string[] strings = File.ReadAllLines(FilePath);   
            Purple_5.Report rep = new Purple_5.Report();
            int ind = 1;
            for(int i = 0; i < int.Parse(strings[0]); i++)
            {
                string name = strings[ind++];
                Purple_5.Research res = new Purple_5.Research(name);
                int count = int.Parse(strings[ind++]);
                for (int j=0; j< count; j++)   
                {
                    string[] answers = strings[ind++].Split(',');
                    for(int k = 0; k< answers.Length; k++)  
                    {
                        if (answers[k] =="")   answers[k] = null;   
                    }
                    res.Add(answers);
                }

                rep.AddResearch(res);
            }

            return rep;
            
        }
    }
}
