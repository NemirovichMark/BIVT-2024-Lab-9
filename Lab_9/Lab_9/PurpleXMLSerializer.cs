using Lab_7;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Lab_7.Purple_1;
using static Lab_7.Purple_4;
using static Lab_7.Purple_5;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";
        public override void SerializePurple1<T>(T obj, string fileName)  
        {
            SelectFile(fileName);
            XmlSerializer serial;

            using (var wr = new StreamWriter(FilePath))   
            {
                if (obj.GetType().Name == nameof(Purple_1.Participant))
                {
                    var part = new P1ParticipantDTO(obj as Purple_1.Participant);  
                    serial = new XmlSerializer( typeof(P1ParticipantDTO));
                    serial.Serialize(wr, part);  
                } 

                else if(obj is Purple_1.Judge judge)
                {
                    var part = new P1JudgeDTO(judge.Name,  judge.Marks);
                    serial = new XmlSerializer( typeof(P1JudgeDTO));
                    serial.Serialize(wr, part);
                }

                else if(obj is Purple_1.Competition comp)  
                {
                    var part = new P1CompDTO(comp);
                    serial = new XmlSerializer(typeof(P1CompDTO));
                    serial.Serialize(wr,part);  
                }
            }
               
            
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);  
            XmlSerializer serial = new XmlSerializer(typeof(P2SkiJumpingDTO));
            P2SkiJumpingDTO xml = new P2SkiJumpingDTO(jumping);  

            using (var wr =new StreamWriter(FilePath))
            {
                serial.Serialize(wr, xml);  
            }
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            using var wr = new StreamWriter(FilePath);   
            var serial = new XmlSerializer(typeof(P3SkatingDTO));
            serial.Serialize(wr, new P3SkatingDTO(skating));  
            wr.Close(); 

        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);   
            using StreamWriter wr = new StreamWriter(FilePath);
            XmlSerializer ser = new XmlSerializer(typeof(P4GroupDTO));  
            ser.Serialize(wr,  new P4GroupDTO(group));   
            wr.Close();
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName); 
              
            using (var wr = new StreamWriter(FilePath))
            {
                var ser = new XmlSerializer(typeof(P5ReportDTO));
                ser.Serialize(wr,new P5ReportDTO(report));
            }

        }


        // Deserialize
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);  
            XmlSerializer serial;
            using (var read= new StreamReader(FilePath))  
            {
                if (typeof(T) == typeof(Purple_1.Participant))  
                {  
                    serial = new XmlSerializer(typeof(P1ParticipantDTO));
                    P1ParticipantDTO part=(P1ParticipantDTO) serial.Deserialize(read); 
                    Purple_1.Participant newpart = new Purple_1.Participant(part.Name, part.Surname);
                    newpart.SetCriterias(part.Coefs);
                    for (int i = 0; i < part.Marks.GetLength(0);i++) newpart.Jump(part.Marks[i]);  
                    return (T)(Object)newpart;
                } 

                else if (typeof(T) == typeof(Purple_1.Judge))
                {
                    serial = new XmlSerializer(typeof(P1JudgeDTO));
                    P1JudgeDTO j = (P1JudgeDTO) serial.Deserialize(read);
                    Purple_1.Judge ans = new Purple_1.Judge(j.Name, j.Marks);
                    return (T)(Object) ans;

                }

                else if (typeof(T) == typeof(Purple_1.Competition))
                {
                    serial = new XmlSerializer(typeof(P1CompDTO));
                    P1CompDTO c = (P1CompDTO)serial.Deserialize(read);   
                    Purple_1.Competition ans = new Purple_1.Competition( c.Judges.Select(x=>new Purple_1.Judge(x.Name,x.Marks)).ToArray());
                    for(int i =0; i <c.Participants.Length; i++)  
                    {
                        var p = new Purple_1.Participant(c.Participants[i].Name, c.Participants[i].Surname);   
                        p.SetCriterias(c.Participants[i].Coefs);
                        for (int j = 0; j<c.Participants[i].Marks.GetLength(0);j++)   p.Jump(c.Participants[i].Marks[j]);
                        ans.Add(p);
                    }
                    return (T) (Object)ans;
                }

                return null;
            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);  
            XmlSerializer serial = new XmlSerializer(typeof(P2SkiJumpingDTO));
            using var read = new StreamReader(FilePath);
            var xml = (P2SkiJumpingDTO)serial.Deserialize(read);  
            read.Close();  
            Purple_2.SkiJumping ans; 
            if (xml.type == nameof(Purple_2.ProSkiJumping))  ans = new Purple_2.ProSkiJumping(); 

            else if (xml.type == nameof(Purple_2.JuniorSkiJumping))  ans = new Purple_2.JuniorSkiJumping();
                
            else return null;

            foreach(P2ParticipantDTO p in  xml.Participants)
            {
                var part = new Purple_2.Participant(p.Name, p.Surname);
                part.Jump(p.Distance, p.Marks, xml.Standard);
                ans.Add(part);
            }

            return (T) (Object) ans; 
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var serial = new XmlSerializer(typeof(P3SkatingDTO)); 
            using var read = new StreamReader(FilePath);
            var xml = (P3SkatingDTO)serial.Deserialize(read);   
            read.Close();
            Purple_3.Skating ans;  

            if (xml.type == nameof(Purple_3.IceSkating)) ans = new Purple_3.IceSkating(xml.Moods, false); 
            else if (xml.type == nameof(Purple_3.FigureSkating)) ans = new Purple_3.FigureSkating(xml.Moods, false); 
            else return null;
            foreach(var p in xml.Participants)  
            {
                var part = new Purple_3.Participant(p.Name, p.Surname);
                foreach (var mark in p.Marks)   part.Evaluate(mark); 
                ans.Add(part); 
            }
            return (T) (Object)ans;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName); 
            using StreamReader reader = new StreamReader(FilePath);
            XmlSerializer ser = new XmlSerializer(typeof(P4GroupDTO));
            P4GroupDTO xml = (P4GroupDTO) ser.Deserialize(reader); 
            var ans = new Purple_4.Group(xml.Name);
            foreach(var sp in xml.Sportsmen)  
            {
                Purple_4.Sportsman sport;

                if (sp.type ==nameof(Purple_4.SkiMan))  sport = new Purple_4.SkiMan(sp.Name, sp.Surname); 
                else if (sp.type == nameof(Purple_4.SkiWoman))  sport = new Purple_4.SkiWoman(sp.Name, sp.Surname); 
                else if (sp.type ==nameof(Purple_4.Sportsman))  sport = new Purple_4.Sportsman(sp.Name, sp.Surname);
                else return null;
                sport.Run(sp.Time);
                ans.Add(sport);  
            }
            reader.Close();
            return ans;

        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);  
            using var reader = new StreamReader(FilePath);
            var ser = new XmlSerializer(typeof(P5ReportDTO));
            var xml = (P5ReportDTO)ser.Deserialize(reader);
            var ans = new Purple_5.Report();
            foreach(var res in xml.Researches)  
            {
                var research = new Purple_5.Research(res.Name);  
                foreach(var resp in res.Responses)
                {
                    research.Add(new string[] { resp.Animal, resp.CharacterTrait, resp.Concept }); 
                }
                  

                ans.AddResearch(research); 
            }
            return ans;
        }

        //Classes
        public class P1ParticipantDTO
        {
            
            public string Name { get; set; }
            public string Surname { get; set; }   
            public double[] Coefs { get; set; }
            

            public int[][] Marks { get; set; }
            public double TotalScore {  get; set; }
            public P1ParticipantDTO() { }

            public P1ParticipantDTO(Purple_1.Participant obj)   
            {
                Name = obj.Name;
                Surname = obj.Surname;
                Coefs = obj.Coefs; 
                int[][] marks = new int[obj.Marks.GetLength(0)][];
                TotalScore =obj.TotalScore;
                
                for (int i = 0; i<obj.Marks.GetLength(0); i++)
                {
                    marks[i] = new int[obj.Marks.GetLength(1)]; 
                    for (int j = 0; j <obj.Marks.GetLength(1);j++) 
                    {
                        marks[i][j] = obj.Marks[i,j];  
                    }
                }
                Marks = marks;
            }
        }
        public class P1JudgeDTO
        {
            public int[] Marks { get; set; } 
            public string Name { get; set; }
            public P1JudgeDTO() {}  
            public P1JudgeDTO(string name, int[] marks)
            {
                Name = name;
                if (marks != null)
                {
                    Marks = new int[marks.Length];
                    Array.Copy(marks, Marks, marks.Length); 
                }

                else Marks = null;
                

            }
        }
        public class P1CompDTO
        {
            public P1JudgeDTO[] Judges { get; set; }
            public P1ParticipantDTO[] Participants { get; set; }
            public P1CompDTO() {} 
            public P1CompDTO(Purple_1.Competition comp)
            {
                Judges = comp.Judges.Select(x=> new  P1JudgeDTO(x.Name,x.Marks)).ToArray();  
                Participants = comp.Participants.Select(x =>new P1ParticipantDTO(x)).ToArray();
            }  
        }


        public class P2ParticipantDTO
        {
            public string Name{ get; set; }
            public string Surname { get; set; } 
            
            public int Distance { get; set; }
            public int[] Marks{ get; set; }
            

            public int Result { get; set; }  
               
            public P2ParticipantDTO() { }
            public P2ParticipantDTO(Purple_2.Participant p) 
            {
                Name = p.Name;
                Surname = p.Surname; 
                Distance = p.Distance;
                Marks = p.Marks;
                Result = p.Result;  

            }
        }
        public class P2SkiJumpingDTO
        {
            public string Name { get; set; }
            public P2ParticipantDTO[] Participants { get; set; }  
            public int Standard { get; set; }
            public string type { get; set;}
            public P2SkiJumpingDTO() { }
            public P2SkiJumpingDTO(Purple_2.SkiJumping sk)  
            {
                Name = sk.Name;
                Standard = sk.Standard;
                Participants = sk.Participants.Select(x=>new P2ParticipantDTO(x)).ToArray();

                type = sk.GetType().Name;
            }
        }

        public class P3ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; } 
            public double[] Marks { get; set; }
            
            public int[] Places { get; set; }
            
              
            public int Score { get; set; }
             
            public P3ParticipantDTO() { } 
            public P3ParticipantDTO(Purple_3.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Marks = p.Marks; 
                Places= p.Places; 
                Score= p.Score;
            } 
        }
        public class P3SkatingDTO
        {
            public P3ParticipantDTO[] Participants { get; set; } 
            public double[] Moods { get; set; }
            public string type { get; set; }  

            public P3SkatingDTO()
            {

            }
            public P3SkatingDTO(Purple_3.Skating sk)
            {
                Moods = sk.Moods;  
                Participants = sk.Participants.Select(x=> new  P3ParticipantDTO(x)).ToArray();
                type = sk.GetType().Name;
                
            }
        }


        public class P4SportsmanDTO
        {
            public string Name {get;set; }
            public string Surname { get; set; }  
            public double Time { get; set; }
            public string type { get; set; }  

            public P4SportsmanDTO() {}
            public P4SportsmanDTO(Purple_4.Sportsman sp)
            {
                Name = sp.Name;
                Surname = sp.Surname;  
                Time = sp.Time;

                type = sp.GetType().Name;

            }
        }
        public class P4GroupDTO
        {
            public string Name { get; set; } 
            public P4SportsmanDTO[] Sportsmen { get; set; } 
              
            public P4GroupDTO() { }
            public P4GroupDTO(Purple_4.Group gr)
            { 
                Name=gr.Name;
                Sportsmen = gr.Sportsmen.Select(x =>new P4SportsmanDTO(x)).ToArray();  
              
            }
        }

        public class P5ResponseDTO
        {  
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }  

            public P5ResponseDTO() { } 
            public P5ResponseDTO(Purple_5.Response resp)  
            {
                Animal=resp.Animal;
                CharacterTrait=resp.CharacterTrait;
                Concept=resp.Concept;   
                
            }
        }

        public class P5ResearchDTO
        {
            public string Name { get;set; }
            public P5ResponseDTO[] Responses { get;set; }
             
            public P5ResearchDTO() { }
            public P5ResearchDTO(Purple_5.Research res) 
            {
                Name=res.Name;
                Responses=res.Responses.Select( x=>new P5ResponseDTO(x)).ToArray();  
                
            }
        }
        public class P5ReportDTO
        {
            public P5ResearchDTO[] Researches { get; set; }
            
            public P5ReportDTO(Purple_5.Report  rep)
            {
                Researches = rep.Researches.Select (x =>new P5ResearchDTO(x)).ToArray();
                
            }
            public P5ReportDTO() { }  
        }
    }
}

