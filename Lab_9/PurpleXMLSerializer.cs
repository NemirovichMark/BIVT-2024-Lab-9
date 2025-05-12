using Lab_7;
using Lab_9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        public class Purple_1_XML() {
            [Serializable]
            [XmlRoot("Competition")]
            public class Competition
            {
                [XmlArray("Judges")]
                [XmlArrayItem("Judge")]
                public Judge[] Judges { get; set; }

                [XmlArray("Participants")]
                [XmlArrayItem("Participant")]
                public Participant[] Participants { get; set; }
                public Competition() {}
                public Competition(Purple_1.Competition comp){
                    this.Participants = new Participant[comp.Participants.Length];
                    for(int i = 0; i <comp.Participants.Length; i++){
                        this.Participants[i] = new Participant(comp.Participants[i]);
                    }
                    this.Judges = new Judge[comp.Judges.Length];
                    for(int i = 0; i <comp.Judges.Length; i++){
                        this.Judges[i] = new Judge(comp.Judges[i]);
                    }
                }
            }

            [Serializable]
            public class Participant
            {
                [XmlElement("Name")]
                public string Name { get; set; }
                [XmlElement("Surname")]
                public string Surname { get; set; }
                [XmlArray("Coefs")]
                [XmlArrayItem("Coef")]
                public double[] Coefs { get; set; }
                
                [XmlElement("LenN")]
                public int n { get; set; }
                
                [XmlElement("LenM")]
                public int m { get; set; }
                
                [XmlArray("Marks")]
                [XmlArrayItem("Mark")]
                public int[] Marks { get; set; }

                [XmlElement("TotalScore")]
                public double TotalScore { get; set; }
                public Participant() {}
                public Participant(Purple_1.Participant obj) {
                    this.Name = obj.Name;
                    this.Surname = obj.Surname;
                    this.Coefs = obj.Coefs;
                    this.TotalScore = obj.TotalScore;

                    n = obj.Marks.GetLength(0);
                    m = obj.Marks.GetLength(1);

                    Marks = new int[n * m];

                    for (int i = 0; i<n; i++){
                        for (int j = 0; j<m; j++){
                            Marks[i * m + j] = obj.Marks[i,j];
                        }
                    }
                }
            }

            [Serializable]
            public class Judge
            {
                [XmlElement("Name")]
                public string Name { get; set; }

                [XmlArray("Marks")]
                [XmlArrayItem("Mark")]
                public int[] Marks { get; set; }
                public Judge() {}
                public Judge(Purple_1.Judge obj) {
                    this.Name = obj.Name; this.Marks = obj.Marks;
                }
            }
        }

        public class Purple_2XML {
            public class Participant{
                [XmlElement("Name")]
                public string Name {get; set; }

                [XmlElement("Surname")]
                public string Surname {get; set; }

                [XmlElement("Distance")]
                public int Distance {get; set; }
                
                [XmlArray("Marks")]
                [XmlArrayItem("Mark")]
                public int[] Marks {get; set; }
                
                [XmlElement("Result")]
                public int Result { get; set; }   
                public Participant() {}
                public Participant(Purple_2.Participant obj) {
                    this.Name = obj.Name; this.Surname = obj.Surname;
                    this.Distance = obj.Distance;
                    this.Marks = obj.Marks;
                    this.Result = obj.Result;
                }
            }
            
            [Serializable]
            [XmlRoot("SkiJumping")]
            public class SkiJumping {
                [XmlElement("Name")]
                 public string Name { get; set; }
                [XmlElement("Standard")]
                public int Standard {get; set; }

                [XmlElement("Type")]
                public string Type {get; set; }

                [XmlArray("Participants")]
                [XmlArrayItem("Participant")]
                public Purple_2XML.Participant[] Participants { get; set; }
                public SkiJumping() {}
                public SkiJumping(Purple_2.SkiJumping obj) {
                    this.Name=obj.Name; this.Standard = obj.Standard;
                    var prts = new Purple_2XML.Participant[obj.Participants.Length];
                    for (int i = 0; i<obj.Participants.Length; i++){
                        prts[i] = new Purple_2XML.Participant(obj.Participants[i]);
                    }
                    this.Participants = prts;
                    if (obj is Purple_2.JuniorSkiJumping) {
                        this.Type = obj.GetType().AssemblyQualifiedName;
                    }
                }

            }
        }

        public class Purple_3XML {
            public class Participant{
                [XmlElement("Name")]
                public string Name {get; set; }

                [XmlElement("Surname")]
                public string Surname {get; set; }   

                [XmlArray("Marks")]
                [XmlArrayItem("Mark")]
                public double[] Marks {get; set; }

                [XmlArray("Places")]
                [XmlArrayItem("Place")]
                public int[] Places {get; set; }
                
                [XmlElement("Score")]
                public int Score { get; set; }   
                public Participant() {}
                public Participant(Purple_3.Participant obj) {
                    this.Name = obj.Name; this.Surname = obj.Surname;
                    this.Marks = obj.Marks;
                    this.Places = obj.Places;
                    this.Score = obj.Score;
                }
            }
            
            [Serializable]
            [XmlRoot("Skating")]
            public class Skating {
                [XmlElement("Type")]
                public string Type {get; set; }

                [XmlArray("Participants")]
                [XmlArrayItem("Participant")]
                public Purple_3XML.Participant[] Participants { get; set; }

                [XmlArray("Moods")]
                [XmlArrayItem("Mood")]
                public double[] Moods {get; set; }
                public Skating() {}
                public Skating(Purple_3.Skating obj) {
                    this.Moods = obj.Moods;
                    
                    var prts = new Purple_3XML.Participant[obj.Participants.Length];
                    for (int i = 0; i<obj.Participants.Length; i++){
                        prts[i] = new Purple_3XML.Participant(obj.Participants[i]);
                    }
                    this.Participants = prts;
                    if (obj is Purple_3.FigureSkating) {
                        this.Type = obj.GetType().AssemblyQualifiedName;
                    }
                }

            }
        }

        public class Purple_4XML {
            public class Sportsman {

                [XmlElement("Name")]
                public string Name {get; set; }
                
                [XmlElement("Surname")]
                public string Surname {get; set; }
                
                [XmlElement("Time")]
                public double Time {get; set; }
                public Sportsman() {}
                public Sportsman(Purple_4.Sportsman obj) {
                    this.Surname = obj.Surname; this.Time = obj.Time;
                    this.Name = obj.Name;
                }
            }
            
            [Serializable]
            [XmlRoot("Group")]
            public class Group {                
                [XmlElement("Name")]
                public string Name { get; set; }
                [XmlArray("Sportsmen")]
                [XmlArrayItem("Sportsman")]
                public Sportsman[] Sportsmen { get; set; }

                public Group() {}
                public Group(Purple_4.Group obj) {
                    this.Name = obj.Name;

                    var sprtsm = new Sportsman[obj.Sportsmen.Length];
                    for (int i = 0; i < obj.Sportsmen.Length; i++){
                        sprtsm[i] = new Sportsman(obj.Sportsmen[i]);
                        sprtsm[i].Time = (obj.Sportsmen[i].Time);
                    }

                    this.Sportsmen = sprtsm;
                }
            }
        }
       
       public class Purple_5XML {
            public class Response {
                [XmlElement("Animal")]
                public string Animal {get; set; }
                [XmlElement("CharacterTrait")]
                public string CharacterTrait {get; set; }
                [XmlElement("Concept")]
                public string Concept {get; set; }
                public Response() {}
                public Response(Purple_5.Response obj) {
                    this.Animal = obj.Animal;
                    this.CharacterTrait = obj.CharacterTrait;
                    this.Concept = obj.Concept;
                }
            }
            public class Research {
                [XmlElement("Name")]
                public string Name {get; set; }
                [XmlArray("Responses")]
                [XmlArrayItem("Response")]
                public Response[] Responses {get; set; }
                public Research() {}
                public Research(Purple_5.Research obj) {
                    Name = obj.Name;
                    Responses = new Response[obj.Responses.Length];
                    for (int i = 0; i < obj.Responses.Length; i++)
                        Responses[i] = new Response(obj.Responses[i]);
                }
            }
            public class Report {
                [XmlArray("Researches")]
                [XmlArrayItem("Research")]
                public Research[] Researches {get; set; }
                public Report() {}
                public Report(Purple_5.Report obj) {
                    Researches = new Research[obj.Researches.Length];
                    for (int i = 0; i < obj.Researches.Length; i++)
                        Researches[i] = new Research(obj.Researches[i]);
                }

            }
       }

        private void Serializer<T>(T obj)
        {
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath)) { return; }
            string path = Path.Combine(FolderPath, FilePath);

            var xmlFile = new XmlSerializer(typeof(T));

            using (var writer = new StreamWriter(path))
                xmlFile.Serialize(writer, obj);
        }
        public override void SerializePurple1<T>(T obj, string fileName) where T : class
        {
            SelectFile(fileName);
            if (obj is Purple_1.Participant xmlob) {
                var prt = new Purple_1_XML.Participant(xmlob);
                Serializer(prt);
            } else if (obj is Purple_1.Judge xmlobj){
                var prt = new Purple_1_XML.Judge(xmlobj);
                Serializer(prt);
                
            } else if (obj is Purple_1.Competition xml) {
                var prt = new Purple_1_XML.Competition(xml);
                Serializer(prt);
            }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var jmp = new Purple_2XML.SkiJumping(jumping);
            Serializer(jmp);
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var p = new Purple_3XML.Skating(skating);
            Serializer(p);
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            var gr = new Purple_4XML.Group(group);
            Serializer(gr);
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            var rep = new Purple_5XML.Report(report);
            Serializer(rep);
        }
        private T DeSerializer<T>(){
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath)) return default;
            string path = FilePath;
            if (!File.Exists(path)) return default;
            var xmlS = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(path)) {
                return (T)xmlS.Deserialize(reader);
            }
        }
        private int[,] ToMatrix(int[] arr, int n, int m) {
            int[,] matrix = new int[n, m];
            for (int i = 0; i<n; i++){
                for (int j = 0; j<m;j++){
                    matrix[i,j] = arr[i * m + j];
                }
            }
            return matrix;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Purple_1.Participant)) {
                var obj = DeSerializer<Purple_1_XML.Participant>();
                var prt = new Purple_1.Participant(obj.Name, obj.Surname);
                prt.SetCriterias(obj.Coefs);
                var marks = ToMatrix(obj.Marks, obj.n, obj.m);
                for (int i = 0; i<obj.n; i++){
                    int[] mrks = new int[obj.m];
                    for (int j = 0; j < obj.m; j++){
                        mrks[j] = marks[i, j];
                    }
                    prt.Jump(mrks);
                }
                return prt as T;
            } else if (typeof(T) == typeof(Purple_1.Judge)){
                var obj = DeSerializer<Purple_1_XML.Judge>();
                var jdg = new Purple_1.Judge(obj.Name, obj.Marks);
                return jdg as T;
            } else {
                var obj = DeSerializer<Purple_1_XML.Competition>();
                
                //Console.WriteLine("1");

                var jdges = new Purple_1.Judge[obj.Judges.Length];
                for (int i = 0; i<obj.Judges.Length; i++){
                    jdges[i] = new Purple_1.Judge(obj.Judges[i].Name, obj.Judges[i].Marks);
                }
                var prts = new Purple_1.Participant[obj.Participants.Length];
                
                //Console.WriteLine("2");

                for (int k = 0; k<obj.Participants.Length; k++){
                    var prt = new Purple_1.Participant(obj.Participants[k].Name, obj.Participants[k].Surname);
                    prt.SetCriterias(obj.Participants[k].Coefs);

                    var marks = ToMatrix(obj.Participants[k].Marks, obj.Participants[k].n, obj.Participants[k].m);
                    
                    //Console.WriteLine("2.1");

                    for (int i = 0; i < obj.Participants[k].n; i++){
                        int[] mrks = new int[obj.Participants[k].m];
                        for (int j = 0; j < obj.Participants[k].m; j++){
                            mrks[j] = marks[i, j];
                        }
                        prt.Jump(mrks);
                    }
                    prts[k] = prt;
                }
                
                //Console.WriteLine("3");

                Purple_1.Competition cmp = new Purple_1.Competition(jdges);
                cmp.Add(prts);
                return cmp as T;
            }
            return null;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var obj = DeSerializer<Purple_2XML.SkiJumping>();
            if (obj.Type == typeof(Purple_2.JuniorSkiJumping).AssemblyQualifiedName){
                var jsj = new Purple_2.JuniorSkiJumping();

                var prts = new Purple_2.Participant[obj.Participants.Length];
                for (int i = 0; i < obj.Participants.Length; i++){
                    var prt = new Purple_2.Participant(obj.Participants[i].Name,
                        obj.Participants[i].Surname);
                    prt.Jump(obj.Participants[i].Distance, obj.Participants[i].Marks,
                        obj.Standard);
                    prts[i] = prt;
                }

                jsj.Add(prts);

                return jsj as T;
            } else {
                var jsj = new Purple_2.ProSkiJumping();

                var prts = new Purple_2.Participant[obj.Participants.Length];
                for (int i = 0; i < obj.Participants.Length; i++){
                    var prt = new Purple_2.Participant(obj.Participants[i].Name,
                        obj.Participants[i].Surname);
                    prt.Jump(obj.Participants[i].Distance, obj.Participants[i].Marks,
                        obj.Standard);
                    prts[i] = prt;
                }

                jsj.Add(prts);

                return jsj as T;
            }
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var obj = DeSerializer<Purple_3XML.Skating>();
            if (obj.Type == typeof(Purple_3.FigureSkating).AssemblyQualifiedName){
                var jsj = new Purple_3.FigureSkating(obj.Moods, false);

                var prts = new Purple_3.Participant[obj.Participants.Length];
                for (int i = 0; i < obj.Participants.Length; i++){
                    var prt = new Purple_3.Participant(obj.Participants[i].Name,
                        obj.Participants[i].Surname);
                    for (int j = 0; j<obj.Participants[i].Marks.Length; j++){
                        prt.Evaluate(obj.Participants[i].Marks[j]);
                    }
                    prts[i] = prt;
                }

                Purple_3.Participant.SetPlaces(prts);

                jsj.Add(prts);

                return jsj as T;
            } else { 
                var jsj = new Purple_3.IceSkating(obj.Moods, false);

                var prts = new Purple_3.Participant[obj.Participants.Length];
                for (int i = 0; i < obj.Participants.Length; i++){
                    var prt = new Purple_3.Participant(obj.Participants[i].Name,
                        obj.Participants[i].Surname);
                    for (int j = 0; j<obj.Participants[i].Marks.Length; j++){
                        prt.Evaluate(obj.Participants[i].Marks[j]);
                    }
                    prts[i] = prt;
                }

                Purple_3.Participant.SetPlaces(prts);

                jsj.Add(prts);

                return jsj as T;
            }
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var obj = DeSerializer<Purple_4XML.Group>();
            var sprtsm = new Purple_4.Sportsman[obj.Sportsmen.Length];
            for (int i = 0; i < obj.Sportsmen.Length; i++){
                sprtsm[i] = new Purple_4.Sportsman(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname);
                sprtsm[i].Run(obj.Sportsmen[i].Time);
            }
            var res = new Purple_4.Group(obj.Name);
            res.Add(sprtsm);
            return res;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);            
            var obj = DeSerializer<Purple_5XML.Report>();
            var res = new Purple_5.Report();
            var rsrcs = new Purple_5.Research[obj.Researches.Length];
            for (int i = 0; i<obj.Researches.Length; i++){
                var research = new Purple_5.Research(obj.Researches[i].Name); 
                for (int j = 0; j < obj.Researches[i].Responses.Length; j++){
                    var resp = new string[] {obj.Researches[i].Responses[j].Animal,
                    obj.Researches[i].Responses[j].CharacterTrait, obj.Researches[i].Responses[j].Concept };
                
                    research.Add(resp);
                }
                rsrcs[i] = research;
            }
            
            res.AddResearch(rsrcs);  
            return res;
        }
    }
}
