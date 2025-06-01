using Lab_7;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

        public class Blue_1XML {
            public class Response
            {
                public string Name { get; set; }
                public string Surname { get; set; }
                public int Votes { get; set; }
                public string Type {get; set; }

                //конструктор
                public Response() {}
                public Response(Blue_1.Response obj)
                {
                    this.Type = obj.GetType().AssemblyQualifiedName;
                    this.Name = obj.Name;
                    this.Votes = obj.Votes;
                    if (obj is Blue_1.HumanResponse hs)
                    {
                        this.Surname = hs.Surname;
                    }
                }
            }
        }

        public class Blue_2XML
        {
            public class Participant
            {
                public string Name { get; set; }
                public string Surname { get; set; }
                public int[] Marks { get; set; }
                public int TotalScore { get; set; }
                public int N { get; set; }
                public int M { get; set; }

                public Participant() { }
                public Participant(Blue_2.Participant obj)
                {
                    this.Name = obj.Name;
                    this.Surname = obj.Surname;
                    this.TotalScore = obj.TotalScore;
                    int n = obj.Marks.GetLength(0), m = obj.Marks.GetLength(1);
                    this.Marks = FromMatrix(obj.Marks, n, m);
                    this.N = n;
                    this.M = m;
                }
            }

            public class WaterJump
            {
                public string Name { get; set; }
                public int Bank { get; set; }
                public Participant[] Participants { get; set; }
                public int Count5m { get; set; }
                public string Type { get; set; }
                public double[] Prize { get; set; }

                public WaterJump() { }
                public WaterJump(Blue_2.WaterJump obj)
                {
                    this.Type = obj.GetType().AssemblyQualifiedName;
                    this.Name = obj.Name;
                    this.Bank = obj.Bank;
                    this.Prize = obj.Prize;
                    this.Count5m = obj.Count5m;

                    var prts = new Participant[obj.Participants.Length];
                    for (int i = 0; i < obj.Participants.Length; i++) prts[i] = new Participant(obj.Participants[i]);
                    this.Participants = prts;
                }
                
            }
        }
        public class Blue_3XML
        {
            public class Participant
            {
                public string Name { get; set; }
                public string Surname { get; set; }
                public int[] PenaltyTimes { get; set; }
                public int Total { get; set; }
                public bool IsExpelled { get; set; }
                public string Type { get; set; }

                public Participant() { }
                public Participant(Blue_3.Participant obj)
                {
                    this.Name = obj.Name;
                    this.Surname = obj.Surname;
                    this.Total = obj.Total;
                    this.IsExpelled = obj.IsExpelled;
                    this.Type = obj.GetType().AssemblyQualifiedName;
                    
                    if (obj.Penalties != null)
                    {
                        this.PenaltyTimes = new int[obj.Penalties.Length];
                        Array.Copy(obj.Penalties, this.PenaltyTimes, obj.Penalties.Length);
                    }
                }
            }
        }

        public class Blue_4XML
        {
            public class Team
            {
                public string Name { get; set; }
                public int[] Scores { get; set; }
                public string Type { get; set; }

                public Team() { }
                public Team(Blue_4.Team obj)
                {
                    this.Name = obj.Name;
                    this.Type = obj.GetType().AssemblyQualifiedName;
                    
                    if (obj.Scores != null)
                    {
                        this.Scores = new int[obj.Scores.Length];
                        Array.Copy(obj.Scores, this.Scores, obj.Scores.Length);
                    }
                }
            }

            public class Group
            {
                public string Name { get; set; }
                public Team[] ManTeams { get; set; }
                public Team[] WomanTeams { get; set; }

                public Group() { }
                public Group(Blue_4.Group obj)
                {
                    this.Name = obj.Name;
                    
                    if (obj.ManTeams != null)
                    {
                        this.ManTeams = new Team[obj.ManTeams.Length];
                        for (int i = 0; i < obj.ManTeams.Length; i++)
                            if (obj.ManTeams[i] != null)
                                this.ManTeams[i] = new Team(obj.ManTeams[i]);
                    }
                    
                    if (obj.WomanTeams != null)
                    {
                        this.WomanTeams = new Team[obj.WomanTeams.Length];
                        for (int i = 0; i < obj.WomanTeams.Length; i++)
                            if (obj.WomanTeams[i] != null)
                                this.WomanTeams[i] = new Team(obj.WomanTeams[i]);
                    }
                }
            }
        }

        public class Blue_5XML
        {
            public class Sportsman
            {
                public string Name { get; set; }
                public string Surname { get; set; }
                public int Place { get; set; }

                public Sportsman() { }
                public Sportsman(Blue_5.Sportsman obj)
                {
                    this.Name = obj.Name;
                    this.Surname = obj.Surname;
                    this.Place = obj.Place;
                }
            }

            public class Team
            {
                public string Name { get; set; }
                public Sportsman[] Sportsmen { get; set; }
                public string Type { get; set; }

                public Team() { }
                public Team(Blue_5.Team obj)
                {
                    this.Name = obj.Name;
                    this.Type = obj.GetType().AssemblyQualifiedName;
                    
                    if (obj.Sportsmen != null)
                    {
                        this.Sportsmen = new Sportsman[obj.Sportsmen.Length];
                        for (int i = 0; i < obj.Sportsmen.Length; i++)
                            if (obj.Sportsmen[i] != null)
                                this.Sportsmen[i] = new Sportsman(obj.Sportsmen[i]);
                    }
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

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            SelectFile(fileName);
            var prt = new Blue_1XML.Response(participant);
            Serializer(prt);
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            SelectFile(fileName);
            var prt = new Blue_2XML.WaterJump(participant);
            Serializer(prt);
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            SelectFile(fileName);
            var prt = new Blue_3XML.Participant(student as Blue_3.Participant);
            Serializer(prt);
        }
        public override void SerializeBlue4Group(Blue_4.Group group, string fileName)
        {
            SelectFile(fileName);
            var prt = new Blue_4XML.Group(group);
            Serializer(prt);
        }
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            SelectFile(fileName);
            var prt = new Blue_5XML.Team(group as Blue_5.Team);
            Serializer(prt);
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
        private static int[,] ToMatrix(int[] arr, int n, int m) {
            int[,] matrix = new int[n, m];
            for (int i = 0; i<n; i++){
                for (int j = 0; j<m;j++){
                    matrix[i,j] = arr[i * m + j];
                }
            }
            return matrix;
        }

        private static int[] FromMatrix(int[,] arr, int n, int m)
        {
            var res = new int[n * m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    res[i * m + j] = arr[i, j];
                }
            }
            return res;
        }
        
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            var prt = DeSerializer<Blue_1XML.Response>();
            System.Console.WriteLine(prt.Type);
            System.Console.WriteLine(typeof(Blue_1.Response).GetType().AssemblyQualifiedName);
            if (prt.Type == typeof(Blue_1.Response).AssemblyQualifiedName)
            {
                var res = new Blue_1.Response(prt.Name, prt.Votes);
                return res;
            }
            else
            {
                var res = new Blue_1.HumanResponse(prt.Name, prt.Surname, prt.Votes);
                return res;
            }
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            var prt = DeSerializer<Blue_2XML.WaterJump>();
            if (prt.Type == typeof(Blue_2.WaterJump3m).AssemblyQualifiedName)
            {
                var res = new Blue_2.WaterJump3m(prt.Name, prt.Bank);
                foreach (var item in prt.Participants)
                {
                    var p = new Blue_2.Participant(item.Name, item.Surname);
                    var marks = ToMatrix(item.Marks, item.N, item.M);

                    for (int i = 0; i < 2; i++)
                    {
                        int[] r = new int[5];
                        for (int j = 0; j < 5; j++)
                        {
                            r[j] = marks[i, j];
                        }
                        p.Jump(r);
                    }
                    res.Add(p);
                }
                return res;
            }
            else
            {
                var res = new Blue_2.WaterJump5m(prt.Name, prt.Bank);
                foreach (var item in prt.Participants)
                {
                    var p = new Blue_2.Participant(item.Name, item.Surname);
                    var marks = ToMatrix(item.Marks, item.N, item.M);

                    for (int i = 0; i < 2; i++) 
                    {
                        int[] r = new int[5];
                        for (int j = 0; j < 5; j++)
                        {
                            r[j] = marks[i, j];
                        }
                        p.Jump(r);
                    }

                    res.Add(p);
                }
                return res;
            }
        }
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            var prt = DeSerializer<Blue_3XML.Participant>();
            
            object participant;
            if (prt.Type == typeof(Blue_3.BasketballPlayer).AssemblyQualifiedName)
            {
                participant = new Blue_3.BasketballPlayer(prt.Name, prt.Surname);
            }
            else if (prt.Type == typeof(Blue_3.HockeyPlayer).AssemblyQualifiedName)
            {
                participant = new Blue_3.HockeyPlayer(prt.Name, prt.Surname);
            }
            else
            {
                // Если тип не определен, создаем базовый Participant
                participant = new Blue_3.Participant(prt.Name, prt.Surname);
            }

            if (prt.PenaltyTimes != null)
            {
                foreach (var penalty in prt.PenaltyTimes)
                {
                    ((Blue_3.Participant)participant).PlayMatch(penalty);
                }
            }

            return (T)participant;
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            var prt = DeSerializer<Blue_4XML.Group>();
            var group = new Blue_4.Group(prt.Name);

            if (prt.ManTeams != null)
            {
                foreach (var teamXml in prt.ManTeams)
                {
                    if (teamXml != null)
                    {
                        var team = new Blue_4.ManTeam(teamXml.Name);
                        if (teamXml.Scores != null)
                        {
                            foreach (var score in teamXml.Scores)
                            {
                                team.PlayMatch(score);
                            }
                        }
                        group.Add(team);
                    }
                }
            }

            if (prt.WomanTeams != null)
            {
                foreach (var teamXml in prt.WomanTeams)
                {
                    if (teamXml != null)
                    {
                        var team = new Blue_4.WomanTeam(teamXml.Name);
                        if (teamXml.Scores != null)
                        {
                            foreach (var score in teamXml.Scores)
                            {
                                team.PlayMatch(score);
                            }
                        }
                        group.Add(team);
                    }
                }
            }

            return group;
        }
        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            var prt = DeSerializer<Blue_5XML.Team>();
            
            Blue_5.Team team;
            if (prt.Type == typeof(Blue_5.ManTeam).AssemblyQualifiedName)
            {
                team = new Blue_5.ManTeam(prt.Name);
            }
            else
            {
                team = new Blue_5.WomanTeam(prt.Name);
            }

            if (prt.Sportsmen != null)
            {
                foreach (var sportsmanXml in prt.Sportsmen)
                {
                    if (sportsmanXml != null)
                    {
                        var sportsman = new Blue_5.Sportsman(sportsmanXml.Name, sportsmanXml.Surname);
                        if (sportsmanXml.Place > 0)
                        {
                            sportsman.SetPlace(sportsmanXml.Place);
                        }
                        team.Add(sportsman);
                    }
                }
            }

            return (T)(object)team;
        }
    }
}