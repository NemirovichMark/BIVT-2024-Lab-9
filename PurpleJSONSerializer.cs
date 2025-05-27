using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj == null || fileName == null || fileName == "" || _folderPath == null) return;

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            string json = JsonConvert.SerializeObject(obj);
            
            //добавить еще тип объекта 

            //парсинг в объект JObject
            JObject jsonObject = JObject.Parse(json);

            //добавление нового поля
            if (obj is Purple_1.Participant) jsonObject["Type"] = "Participant";
            else
            {
                if (obj is Purple_1.Judge) jsonObject["Type"] = "Judge";
                else jsonObject["Type"] = "Competition";

            }
            //запись изменений в файл 
            string updatedJson = jsonObject.ToString(Formatting.Indented);
            File.WriteAllText(fullPath, updatedJson);
        }

        public override T DeserializePurple1<T>(string fileName)
        {

            if (fileName == null || fileName == "" || _folderPath == null) return default(T);

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            string json = "";
            using (StreamReader reader = new StreamReader(fullPath))
            {
                json = reader.ReadToEnd();
            }
            if (json == null || json == "") return default(T);

            
            JObject jsonObject = JObject.Parse(json);
            string type = jsonObject.Value<string>("Type");
            jsonObject.Remove("Type");
            if (type == "Participant")
            {
                Purple_1.Participant participant = jsonObject.ToObject<Purple_1.Participant>();
                
                //coefs
                JArray coefsJ = jsonObject["Coefs"] as JArray;
                double[] coefs = coefsJ.ToObject<double[]>();
                participant.SetCriterias(coefs);

                //marks
                JArray marksJ = jsonObject["Marks"] as JArray;
                int[,] marks = marksJ.ToObject<int[,]>();
                for (int i = 0; i < 4; i++)
                    {

                        int[] marksJump = new int[7];
                        for (int j = 0; j < 7; j++)
                        {
                            marksJump[j] = marks[i, j];
                        }
                        participant.Jump(marksJump);
                    }

                return participant as T;
            }
            else
            {
                if (type == "Judge")
                {
                    Purple_1.Judge judge = jsonObject.ToObject<Purple_1.Judge>();
                    return judge as T;
                }
                else
                { 
                    Purple_1.Competition competition = jsonObject.ToObject<Purple_1.Competition>();
                    JArray participantsJ = jsonObject["Participants"] as JArray;
                    Purple_1.Participant[] participants = new Purple_1.Participant[0];
                    
                    foreach (JObject participantJ in participantsJ)
                    {
                        Purple_1.Participant participant = participantJ.ToObject<Purple_1.Participant>();

                        //coefs
                        JArray coefsJ = participantJ["Coefs"] as JArray;
                        double[] coefs = coefsJ.ToObject<double[]>();
                        participant.SetCriterias(coefs);

                        //marks
                        JArray marksJ = participantJ["Marks"] as JArray;
                        int[,] marks = marksJ.ToObject<int[,]>();
                        for (int i = 0; i < 4; i++)
                        {

                            int[] marksJump = new int[7];
                            for (int j = 0; j < 7; j++)
                            {
                                marksJump[j] = marks[i, j];
                            }
                            participant.Jump(marksJump);
                        }
                        Array.Resize(ref participants, participants.Length + 1);
                        participants[participants.Length - 1] = participant;
                    }
                    competition.Add(participants);

                    return competition as T;
                }


            }
            return default(T);

        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (jumping == null || fileName == null || fileName == "" || _folderPath == null) return;

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);

            string json = JsonConvert.SerializeObject(jumping);
            JObject jsonObject = JObject.Parse(json);

            if (jumping is Purple_2.JuniorSkiJumping) jsonObject["Type"] = "JuniorSkiJumping";
            else jsonObject["Type"] = "ProSkiJumping";

            string updatedJson = jsonObject.ToString(Formatting.Indented);
            File.WriteAllText(fullPath, updatedJson);

        }


        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            if (fileName == null || fileName == "" || _folderPath == null) return default(T);

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            string json = "";
            using (StreamReader reader = new StreamReader(fullPath))
            {
                json = reader.ReadToEnd();
            }
        
            
            if (string.IsNullOrEmpty(json)) return default(T);

            JObject jObject = JObject.Parse(json);

            //вычленить тайп
            string type = jObject.Value<string>("Type");
            jObject.Remove("Type");
            //в этом же блоке создаем будущий десериализованный объект в зависимости от типа
            Purple_2.SkiJumping skiJumping;
            if (type == "JuniorSkiJumping")
            {
                skiJumping = new Purple_2.JuniorSkiJumping();
            }
            else skiJumping = new Purple_2.ProSkiJumping();


            //создаем массив из json- объектов 
            JArray participantsJ = jObject["Participants"] as JArray;

            Purple_2.Participant[] participants = new Purple_2.Participant[0];
            //итерируемся по ним
            foreach (JObject participantJ in participantsJ)
            {
                //в конструкторе заполняются нейм и сернейм
                Purple_2.Participant participant = participantJ.ToObject<Purple_2.Participant>();

                JArray marksJ = participantJ["Marks"] as JArray;
                int[] marks = marksJ.ToObject<int[]>();

                string distanceString = participantJ.Value<string>("Distance");
                int distanceInt;
                int.TryParse(distanceString, out distanceInt);

                skiJumping.Add(participant);
                skiJumping.Jump(distanceInt, marks);


            }
            return skiJumping as T;

        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            if (skating == null || fileName == null || fileName == "" || _folderPath == null) return;

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);

            string json = JsonConvert.SerializeObject(skating);
            JObject jObject = JObject.Parse(json);

            if (skating is Purple_3.FigureSkating) jObject["Type"] = "FigureSkating";
            else jObject["Type"] = "IceSkating";

            string updatedJson = jObject.ToString(Formatting.Indented);

            File.WriteAllText(fullPath, updatedJson);


        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            if (fileName == null || fileName == "" || _folderPath == null) return default(T);

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);

            string json = "";
            using (StreamReader reader = new StreamReader(fullPath))
            {
                json = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(json)) return default(T);

            JObject jsonObject = JObject.Parse(json);
            string type = jsonObject.Value<string>("Type");
            jsonObject.Remove("Type");

            //moods
            JArray moodsJ = jsonObject["Moods"] as JArray;
            double[] moods = moodsJ.ToObject<double[]>();

            Purple_3.Skating skating;
            if (type == "FigureSkating") skating = new Purple_3.FigureSkating(moods, false);
            else skating = new Purple_3.IceSkating(moods, false);

            //теперь партисипантс 
            JArray participantsJ = jsonObject["Participants"] as JArray;
            foreach (JObject participantJ in participantsJ)
            {
                Purple_3.Participant participant = participantJ.ToObject<Purple_3.Participant>();
                

                JArray marksJ = participantJ["Marks"] as JArray;
                double[] marks = marksJ.ToObject<double[]>();
                for (int i = 0; i < marks.Length; i++)
                {
                    participant.Evaluate(marks[i]);
                } 
                skating.Add(participant);
                //skating.Evaluate(marks);
            }
            Purple_3.Participant.SetPlaces(skating.Participants);

            return skating as T;

        }

        
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            if (group == null || fileName == null || fileName == "" || _folderPath == null) return;

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);

            string json = JsonConvert.SerializeObject(group, Formatting.Indented);
            
            File.WriteAllText(fullPath, json);

        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            if (fileName == null || fileName == "" || _folderPath == null) return default(Purple_4.Group);

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);

            string json = "";
            using (StreamReader reader = new StreamReader(fullPath))
            {
                json = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(json)) return default(Purple_4.Group);

            JObject jsonObject = JObject.Parse(json);

            Purple_4.Group group = new Purple_4.Group(jsonObject["Name"].ToString());
           
            //теперь sportsmen
            JArray sportsmenJ = jsonObject["Sportsmen"] as JArray;
            foreach (JObject sportsmanJ in sportsmenJ)
            {
                Purple_4.Sportsman sportsman = sportsmanJ.ToObject<Purple_4.Sportsman>();

                double time = sportsmanJ.Value<double>("Time");
                // double time;
                // double.TryParse(timeS, out time);
                
                sportsman.Run(time);
                group.Add(sportsman);
                
            }
           
            return group;
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            if (report == null || fileName == null || fileName == "" || _folderPath == null) return;

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);

            string json = JsonConvert.SerializeObject(report, Formatting.Indented);
            
            File.WriteAllText(fullPath, json);

        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            
            if (fileName == null || _folderPath == null || fileName=="") return default(Purple_5.Report);
            //System.Console.WriteLine("if1");
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath))
            {
                //System.Console.WriteLine("ex");
                return default(Purple_5.Report);
            }
            
            //System.Console.WriteLine("if2");
            string json = "";
            using (StreamReader reader = new StreamReader(fullPath))
            {
                json = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(json)) return default(Purple_5.Report);
            //System.Console.WriteLine("if3");
            JObject jsonObject = JObject.Parse(json);

            Purple_5.Report report = new Purple_5.Report();
            //System.Console.WriteLine("report");
            //теперь researches
            JArray researchesJ = jsonObject["Researches"] as JArray;
            //System.Console.WriteLine("res");
            foreach (JObject researchJ in researchesJ)
            {
                Purple_5.Research research = researchJ.ToObject<Purple_5.Research>();
                JArray responsesJ = researchJ["Responses"] as JArray;

                foreach (JObject responseJ in responsesJ)
                {

                    string animal = responseJ["Animal"].ToString();
                    string characterTrait = responseJ["CharacterTrait"].ToString();
                    //System.Console.WriteLine(characterTrait=="");
                    string concept = responseJ["Concept"].ToString();

                    research.Add(new string[] { string.IsNullOrEmpty(animal) ? null : animal,
                            string.IsNullOrEmpty(characterTrait) ? null : characterTrait,
                            string.IsNullOrEmpty(concept) ? null : concept });
                    //System.Console.WriteLine(research.Responses[research.Responses.Length-1].CharacterTrait == "");

                }
                
                report.AddResearch(research);

            }
           
            return report;
        }

        


    }
}
