using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using static Lab_7.Purple_1;
using static Lab_7.Purple_2;
using static Lab_7.Purple_3;
using static Lab_7.Purple_5;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        
        //остались свойства

        //переопределили свойтсво 
        public override string Extension => "txt";

        //реализовать абстрактные методы (сериализация/десериализация) родителя

        //сериализация 
        public override void SerializePurple1<T>(T obj, string fileName) 
        {
            //проверить что folderPath точно существует (вызывался selectFolder)
            if (_folderPath == null || obj==null || fileName==null) return;

            //не выпендриваемся (не используем selectFile, так как кто сказал что мы можем менять filePath)
            string fullpath = Path.Combine(_folderPath, fileName + "." + Extension);
            using (FileStream fs = File.Create(fullpath))
            {
            };
            using (StreamWriter writer = File.AppendText(fullpath)) //если файла нет, будет создан автоматически
            {
                if (obj is Purple_1.Participant)
                {
                    writer.WriteLine("class: Participant");
                    Purple_1.Participant newObj = obj as Purple_1.Participant;
                    writer.WriteLine($"Name: {newObj.Name}");
                    writer.WriteLine($"Surname: {newObj.Surname}");
                    writer.WriteLine($"Coefs: {string.Join(" ", newObj.Coefs)}");
                    
                    int[] marks = new int[newObj.Marks.GetLength(0)*newObj.Marks.GetLength(1)];
                    int count = 0;
                    for (int i = 0; i < newObj.Marks.GetLength(0); i++)
                    {
                        for (int j = 0; j < newObj.Marks.GetLength(1); j++)
                        {
                            marks[count]= newObj.Marks[i,j];
                            count++;
                        }
                        
                    }
                    writer.WriteLine($"Marks: {string.Join(" ",marks)}");
                    writer.WriteLine($"TotalScore: {newObj.TotalScore}");

                }

                if (obj is Purple_1.Judge)
                {
                    writer.WriteLine("class: Judge");
                    Purple_1.Judge newObj = obj as Purple_1.Judge;
                    writer.WriteLine($"Name: {newObj.Name}");
                    writer.WriteLine($"Marks: {string.Join(" ",newObj.Marks)}");
                    
                }

                if (obj is Purple_1.Competition)
                {
                    writer.WriteLine("class: Competition");
                    Purple_1.Competition newObj = obj as Purple_1.Competition;
                    writer.WriteLine($"Participants:");
                    foreach (Purple_1.Participant participant in newObj.Participants)
                    {
                        writer.WriteLine($"Name: {participant.Name}");
                        writer.WriteLine($"Surname: {participant.Surname}");
                        writer.WriteLine($"Coefs: {string.Join(" ", participant.Coefs)}");
                        int[] marks = new int[participant.Marks.GetLength(0) * participant.Marks.GetLength(1)];
                        int count = 0;
                        for (int i = 0; i < participant.Marks.GetLength(0); i++)
                        {
                            for (int j = 0; j < participant.Marks.GetLength(1); j++)
                            {
                                //Console.WriteLine(participant.Marks[i, j]);
                                marks[count++] = participant.Marks[i, j];
                            }

                        }
                        foreach (var objjj in participant.Marks)
                        {
                            //Console.WriteLine(objjj);
                        }
                        writer.WriteLine($"Marks: {string.Join(" ", marks)}");
                        writer.WriteLine($"TotalScore: {participant.TotalScore}");
                        writer.WriteLine();
                    }
                    writer.WriteLine();
                    writer.WriteLine("Judges:");
                    foreach (Purple_1.Judge judge in newObj.Judges)
                    {
                        writer.WriteLine($"Name: {judge.Name}");
                        writer.WriteLine($"Marks: {string.Join(" ", judge.Marks)}");
                        writer.WriteLine();
                    }

                }
                writer.WriteLine();

            }

        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (jumping == null || fileName == null || _folderPath==null) return;

            string fullpath = Path.Combine(_folderPath, fileName + "." + Extension);
            using (FileStream fs = File.Create(fullpath)) { };
            using (StreamWriter writer = File.AppendText(fullpath))
            {
                if (jumping is JuniorSkiJumping)
                {
                    writer.WriteLine($"Type: Junior");
                }
                else
                { 
                    writer.WriteLine($"Type: Pro");
                }
                writer.WriteLine($"Name: {jumping.Name}");
                writer.WriteLine($"Standard: {jumping.Standard}");
                writer.WriteLine();
                writer.WriteLine("Participants:" );
                foreach (Purple_2.Participant participant in jumping.Participants)
                {
                    writer.WriteLine($"Name: {participant.Name}");
                    writer.WriteLine($"Surname: {participant.Surname}");
                    writer.WriteLine($"Distanse: {participant.Distance}");
                    writer.WriteLine($"Marks: {string.Join(" ", participant.Marks)}");
                    writer.WriteLine($"Result: {string.Join(" ", participant.Result)}");
                    writer.WriteLine();
                }
                writer.WriteLine();
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            if (skating == null || fileName == null || _folderPath == null) return;

            string fullPath = Path.Combine(_folderPath,fileName + "." + Extension);

            using (FileStream fs = File.Create(fullPath)) { };
            using (StreamWriter writer = File.AppendText(fullPath))
            {
                if (skating is Purple_3.FigureSkating)
                {
                    writer.WriteLine($"Type: FigureSkating");
                }
                else
                { 
                    writer.WriteLine($"Type: IceSkating");
                
                }
                writer.WriteLine($"Moods: {string.Join(" ", skating.Moods)}");
                writer.WriteLine("Participants: ");
                foreach (Purple_3.Participant participant in skating.Participants)
                {
                    writer.WriteLine($"Name: {participant.Name}");
                    writer.WriteLine($"Surname: {participant.Surname}");
                    writer.WriteLine($"Marks: {string.Join(" ", participant.Marks)}");
                    writer.WriteLine($"Places: {string.Join(" ", participant.Places)}");
                    writer.WriteLine();
                }
                writer.WriteLine();
            }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            if (group == null || fileName == null || _folderPath == null) return;

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            using (FileStream fs = File.Create(fullPath)) { };

            using (StreamWriter writer = File.AppendText(fullPath))
            {
                writer.WriteLine($"GroupName: {group.Name}");
                writer.WriteLine(" ");
                writer.WriteLine("Sportsmen: ");
                foreach (Purple_4.Sportsman sportsman in group.Sportsmen)
                {
                    writer.WriteLine($"Name: {sportsman.Name}");
                    writer.WriteLine($"Surname: {sportsman.Surname}");
                    writer.WriteLine($"Time: {sportsman.Time}");
                    writer.WriteLine(" ");
                    
                }
                writer.WriteLine();
            }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            if (report == null || fileName == null || _folderPath == null) return;

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);

            using (FileStream fs = File.Create(fullPath)) { };

            using (StreamWriter writer = File.AppendText(fullPath))
            {

                foreach (Purple_5.Research research in report.Researches)
                {
                    writer.WriteLine($"Name: {research.Name}");
                    writer.WriteLine("Responses:");

                    foreach (Purple_5.Response response in research.Responses)
                    {

                        writer.WriteLine($"Animal: {response.Animal}");
                        writer.WriteLine($"CharacterTrait: {response.CharacterTrait}");
                        writer.WriteLine($"Concept: {response.Concept}");
                        writer.WriteLine();
                    }
                    writer.WriteLine();

                }
                writer.WriteLine();
            }
        }

        //десериализация
        public override T DeserializePurple1<T>(string fileName)
        {
            if (fileName == null || _folderPath == null) return null;
            

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath)) return default (T);
            T answer;
            
            //создать словарь 
            Dictionary<string, string> fileParsing = new Dictionary<string, string>();

            //построчное чтение файла с помощью потока

            string firstline;
            using (StreamReader reader = new StreamReader(fullPath))
            {
                firstline = reader.ReadLine().Trim();
            }
            if (firstline == "class: Participant" || firstline == "class: Judge")
            {
                
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(":"))
                        {
                            string[] parts = line.Split(':');
                            //добавить ключ+значение в словарь 
                            fileParsing[parts[0].Trim()] = parts[1].Trim();
                        }
                    }

                }
                //foreach (var pair in fileParsing)
                //{
                //    Console.WriteLine($"Key: {pair.Key}, Value: {pair.Value}");
                //}
                
                if (fileParsing["class"] == "Participant")
                {
                    Purple_1.Participant participant = new Purple_1.Participant(fileParsing["Name"], fileParsing["Surname"]);
                    participant.SetCriterias(Array.ConvertAll(fileParsing["Coefs"].Split(' '), double.Parse));


                    int[] marks = Array.ConvertAll(fileParsing["Marks"].Split(' '), int.Parse);

                    for (int i = 0; i < 4; i++)
                    {

                        int[] marksJump = new int[7];
                        for (int j = 0; j < 7; j++)
                        {
                            marksJump[j] = marks[7 * i + j];
                        }
                        participant.Jump(marksJump);
                    }

                    answer = participant as T;
                    return answer;
                }
                if (fileParsing["class"] == "Judge")
                {
                    Purple_1.Judge judge = new Purple_1.Judge(fileParsing["Name"], Array.ConvertAll(fileParsing["Marks"].Split(' '), int.Parse));

                    answer = judge as T;
                    return answer;
                }
            }
            if (firstline == "class: Competition")
            {
                //снова построчное чтение 
                //вычленить участников 
                Purple_1.Participant[] participants = new Purple_1.Participant[0] ;
                Purple_1.Judge[] judges = new Purple_1.Judge[0]; 
                
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    string line;
                    bool flagPart=false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(":"))
                        {
                            string[] pairs = line.Split(':');
                            //начались участники
                            if (pairs[0].Trim() == "Participants")
                            {
                                flagPart = true;
                                continue;
                            }
                            fileParsing[pairs[0].Trim()]=pairs[1].Trim();
                            if (flagPart == true && pairs[0].Trim() == "Marks") //всё ещё учатсники и пересобрали фулл информацию об одном из них
                            {

                                Purple_1.Participant participant = new Purple_1.Participant(fileParsing["Name"], fileParsing["Surname"]);
                                participant.SetCriterias(Array.ConvertAll(fileParsing["Coefs"].Split(' '), double.Parse));


                                int[] marks = Array.ConvertAll(fileParsing["Marks"].Split(' '), int.Parse);

                                for (int i = 0; i < 4; i++)
                                {

                                    int[] marksJump = new int[7];
                                    for (int j = 0; j < 7; j++)
                                    {
                                        marksJump[j] = marks[7 * i + j];
                                    }
                                    participant.Jump(marksJump);

                                }
                                Array.Resize(ref participants, participants.Length + 1);
                                participants[participants.Length-1]=participant;//добавили участника
        
                            }
                            //начались судьи
                            if (pairs[0].Trim() == "Judges")
                            {
                                flagPart = false;
                                continue;
                            }
                            if (flagPart == false && pairs[0].Trim()=="Marks")
                            {
                                Purple_1.Judge judge= new Purple_1.Judge(fileParsing["Name"], Array.ConvertAll(fileParsing["Marks"].Split(' '), int.Parse));
                                
                                Array.Resize(ref judges, judges.Length + 1);
                                judges[judges.Length - 1] = judge;//добавили 

                            }

                        }
                    }
                }

                //у нас есть партисипантс и джаджес 
                Purple_1.Competition competition = new Purple_1.Competition(judges);
                competition.Add(participants);
                return competition as T;

            }
            return default(T);
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            if (fileName == null || _folderPath == null) return null;
            

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath)) return default(T);

            //создать словарь 
            Dictionary<string, string> fileParsing = new Dictionary<string, string>();

            Purple_2.Participant[] participants = new Purple_2.Participant[0];

            using (StreamReader reader = new StreamReader(fullPath))
            {
                string line;
                bool flagPart = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(":"))
                    {
                        string[] pairs = line.Split(':');
                        fileParsing[pairs[0].Trim()] = pairs[1].Trim();
                        //начались участники
                        if (pairs[0].Trim() == "Participants")
                        {
                            flagPart = true;
                            continue;

                        }
                        
                        if (flagPart == true && pairs[0].Trim() == "Marks") //начались участники и собрали всё для одного 
                        {
                            Purple_2.Participant participant = new Purple_2.Participant(fileParsing["Name"], fileParsing["Surname"]);
                            if (fileParsing["Standard"] =="100")
                            {

                                int.TryParse(fileParsing["Distanse"], out int distanse);
                                participant.Jump(distanse, Array.ConvertAll(fileParsing["Marks"].Split(' '), int.Parse), 100);
                            }
                            if (fileParsing["Standard"] == "150")
                            {

                                int.TryParse(fileParsing["Distanse"], out int distanse);
                                participant.Jump(distanse, Array.ConvertAll(fileParsing["Marks"].Split(' '), int.Parse), 150);
                            }
                            Array.Resize(ref participants, participants.Length+1);
                            participants[participants.Length-1] = participant;
                        }

                    }
                }
            }

            if (fileParsing["Standard"] == "150")
            {
                Purple_2.ProSkiJumping ski = new ProSkiJumping();
                ski.Add(participants);
                return ski as T;
            }
            else
            {
                Purple_2.JuniorSkiJumping ski = new Purple_2.JuniorSkiJumping();
                ski.Add(participants);
                return ski as T;
            }
            
            return default(T);
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            if (fileName == null || _folderPath == null) return null;


            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath)) return default(T);

            //создать словарь 
            Dictionary<string, string> fileParsing = new Dictionary<string, string>();

            Purple_3.Participant[] participants = new Purple_3.Participant[0];

            using (StreamReader reader = new StreamReader(fullPath))
            {
                string line;
                bool flagPart = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(":"))
                    {
                        string[] pairs = line.Split(':');
                        fileParsing[pairs[0].Trim()] = pairs[1].Trim();
                        //начались участники
                        if (pairs[0].Trim() == "Participants")
                        {
                            flagPart = true;
                            continue;

                        }

                        if (flagPart == true && pairs[0].Trim() == "Places") //начались участники и собрали всё для одного 
                        {
                            Purple_3.Participant participant = new Purple_3.Participant(fileParsing["Name"], fileParsing["Surname"]);
                            double[] marks = Array.ConvertAll(fileParsing["Marks"].Split(' '), double.Parse);
                            foreach (var mark in marks)
                                participant.Evaluate(mark);
                            Array.Resize(ref participants, participants.Length + 1);
                            participants[participants.Length - 1] = participant;
                        }

                    }
                }
            }
            if (fileParsing["Type"] == "FigureSkating")
            {
                Purple_3.FigureSkating skating = new Purple_3.FigureSkating(Array.ConvertAll(fileParsing["Moods"].Split(' '), double.Parse), false);
                Purple_3.Participant.SetPlaces(participants);

                skating.Add(participants);
                return skating as T;
            }
            else
            { 
                Purple_3.IceSkating skating = new Purple_3.IceSkating(Array.ConvertAll(fileParsing["Moods"].Split(' '), double.Parse), false);
                Purple_3.Participant.SetPlaces(participants);
                
                skating.Add(participants);
                return skating as T;
            }
            return default(T);
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {

            if (fileName == null || _folderPath == null) return null;


            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath)) return default(Purple_4.Group);

            //создать словарь 
            Dictionary<string, string> fileParsing = new Dictionary<string, string>();

            Purple_4.Sportsman[] sportsmen = new Purple_4.Sportsman[0];

            using (StreamReader reader = new StreamReader(fullPath))
            {
                string line;
                bool flagPart = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(":"))
                    {
                        string[] pairs = line.Split(':');
                        fileParsing[pairs[0].Trim()] = pairs[1].Trim();
                        //начались участники
                        if (pairs[0].Trim() == "Sportsmen")
                        {
                            flagPart = true;
                            continue;

                        }

                        if (flagPart == true && pairs[0].Trim() == "Time") //начались участники и собрали всё для одного 
                        {
                            Purple_4.Sportsman sportsman = new Purple_4.Sportsman(fileParsing["Name"], fileParsing["Surname"]);
                            double time;
                            double.TryParse(fileParsing["Time"], out time);
                            sportsman.Run(time);
                            Array.Resize(ref sportsmen, sportsmen.Length + 1);
                            sportsmen[sportsmen.Length - 1] = sportsman;
                        }

                    }
                }
            }
            Purple_4.Group group = new Purple_4.Group(fileParsing["GroupName"]);
           
            group.Add(sportsmen);
          
            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            //return default(Purple_5.Report);
            if (fileName == null || _folderPath == null || fileName=="") return null;
            
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath)) return default(Purple_5.Report);

            //создать словарь 
            Dictionary<string, string> fileParsing = new Dictionary<string, string>();

            Purple_5.Report report = new Purple_5.Report();
            Purple_5.Research research = default (Purple_5.Research);

            using (StreamReader reader = new StreamReader(fullPath))
            {
                string line;
                bool flagFirstName = false;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(":"))
                    {
                        string[] pairs = line.Split(':');
                        fileParsing[pairs[0].Trim()] = pairs[1].Trim();

                        //первый ресерч 
                        if (pairs[0].Trim() == "Name" && flagFirstName == false)
                        {
                            flagFirstName = true;
                            research = new Purple_5.Research(fileParsing["Name"]);
                            continue;

                        }
                        //закинуть ответы в исследование 
                        if (pairs[0].Trim() == "Concept")
                        {
                            //Console.WriteLine(fileParsing["Concept"]);
                            research.Add(new string[] { fileParsing["Animal"], fileParsing["CharacterTrait"], fileParsing["Concept"] });
                            //Console.WriteLine(research.Responses[research.Responses.Length - 1].Animal);
                        }

                        //если новое началось, старое закинуть в репорт
                        if (flagFirstName == true && pairs[0].Trim() == "Name") //
                        {
                            report.AddResearch(research);
                            research = new Purple_5.Research(fileParsing["Name"]);
                        }

                    }
                }
            }
            //закинуть последний ресерч 
            report.AddResearch(research);
            // foreach (var research in report.Researches)
            // {
            //     Console.WriteLine(research.Name);
            //     foreach (var responce in research.Responses)
            //     {
            //         Console.WriteLine($"{responce.Animal} {responce.CharacterTrait} {responce.Concept}");
            //     }
            //     Console.WriteLine();
            // }
            return report;
        }

    }
}
