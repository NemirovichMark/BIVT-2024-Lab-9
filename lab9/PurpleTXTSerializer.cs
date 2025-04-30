using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_9;
using Lab_7;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer{
        public override string Extension => "txt";
        private void Purple1ParticipantWriter (StreamWriter writer, Purple_1.Participant participant){
            // writer.WriteLine($"Type:{nameof(Purple_1.Participant)}");//participant.GetType().Name
            writer.WriteLine($"Name:{participant.Name}");
            writer.WriteLine($"Surname:{participant.Surname}");
            writer.WriteLine($"Coefs:{String.Join("|", participant.Coefs)}");
            writer.Write("Marks:");
            for (int i = 0; i < participant.Marks.GetLength(0); i++) {
                for (int j = 0; j < participant.Marks.GetLength(1); j++) {
                    writer.Write($"{participant.Marks[i, j]}"); // write a mark
                    if (j < participant.Marks.GetLength(1) - 1){ // if not last symb in row - ","
                        writer.Write(","); // , везде кроме ласт
                    } 
                }
                if (i < participant.Marks.GetLength(0) - 1){
                        writer.Write(";"); // ориентир строки - ; 
                }
            }
            writer.WriteLine();
            writer.WriteLine($"TotalScore:{participant.TotalScore}");
        }

        private void Purple1JudgeWriter(StreamWriter writer, Purple_1.Judge judge){
            //writer.WriteLine($"Type:{nameof(Purple_1.Judge)}");
            writer.WriteLine($"Name:{judge.Name}");
            writer.WriteLine($"Marks:{String.Join(",", judge.Marks)}");
        }

        public override void SerializePurple1<T>(T obj, string fileName) where T : class{
            if (obj == null) return;
            // using (StreamWriter writer = File.AppendText(fileName)){
            using (StreamWriter writer = File.CreateText(fileName)){
                if (obj is Purple_1.Participant participant){
                    writer.WriteLine($"Type:{nameof(Purple_1.Participant)}");
                    Purple1ParticipantWriter(writer, participant);
                }

                else if (obj is Purple_1.Judge judge){
                    writer.WriteLine($"Type:{nameof(Purple_1.Judge)}");
                    Purple1JudgeWriter(writer, judge);
                }

                else{
                    Purple_1.Competition competition = obj as Purple_1.Competition;
                    Purple_1.Participant[] participants = competition.Participants;
                    Purple_1.Judge[] judges = competition.Judges;

                    if (participants == null){
                        participants = new Purple_1.Participant[0];
                    }

                    if (judges == null){
                        judges = new Purple_1.Judge[0];
                    }

                    writer.WriteLine($"Type:{nameof(Purple_1.Competition)}");
                    writer.WriteLine($"ParticipantsCount:{participants.Length}");
                    foreach (Purple_1.Participant part in participants){
                        Purple1ParticipantWriter(writer, part);
                    }
                    writer.WriteLine($"JudgesCount:{judges.Length}");
                    foreach (Purple_1.Judge judg in judges){
                        Purple1JudgeWriter(writer, judg);
                    }
                    
                }
            }
        }

        public override T DeserializePurple1<T>(string fileName) where T : class{
            Dictionary <string, string> parsedObj = new Dictionary<string, string>();
            //SelectFile(fileName); //FilePath doesnt workin' 
            var lines = File.ReadAllLines(fileName); // list of lines
            foreach (var line in lines){
                if (line.Contains(":")){
                    var parts = line.Split(':');
                    parsedObj[parts[0].Trim()] = parts[1].Trim();
                }
            }
            if (parsedObj["Type"] == nameof(Purple_1.Participant)){
                double[] coefs = new double [4];
                string[] coef = parsedObj.ContainsKey("Coefs") ? parsedObj["Coefs"].Split("|") : null;
                if (coef == null){
                    return null;
                }
                for (int i = 0; i < coef.Length && i < coefs.Length; i++){
                    double.TryParse(coef[i].Trim(), out coefs[i]);
                }
                Purple_1.Participant participant = new Purple_1.Participant(parsedObj["Name"], parsedObj["Surname"]);
                participant.SetCriterias(coefs);

                //int[,] marks = new int[4, 7];
                // Marks:1,2,3,4;5,6,7,8
                string[] rows = parsedObj.ContainsKey("Marks") ? parsedObj["Marks"].Split(";") : null;
                if (rows == null){
                    return null;
                }
                for (int i = 0; i < rows.Length; i++){
                    string[] cols = rows[i].Split(",");
                    int[] marks = new int[7];
                    for (int j = 0; j < cols.Length; j++){
                        int.TryParse(cols[j], out marks[j]);
                    }
                    participant.Jump(marks);
                }
                
                T part = participant as T;
                return part;
            }
            
            else if (parsedObj["Type"] == nameof(Purple_1.Judge)){
                
                string[] stringMarks = parsedObj.ContainsKey("Marks") ? parsedObj["Marks"].Split(",") : null;
                
                if (stringMarks == null){
                    stringMarks = new string[0];
                }

                int[] marks = new int[stringMarks.Length];

                for (int i = 0; i < stringMarks.Length; i++){
                    int.TryParse(stringMarks[i], out marks[i]);
                }
                Purple_1.Judge judge = new Purple_1.Judge(parsedObj["Name"], marks);

                T judg = judge as T;
                return judg;
            }

            else{
                
                Purple_1.Participant[] participants = new Purple_1.Participant[0];
                
                int currLine = 1; //index of numParticipants 
                if (currLine < lines.Length && lines[currLine].StartsWith("ParticipantsCount")){
                    int.TryParse(lines[currLine].Split(":")[1].Trim(), out int ParticipantsCount);
                    currLine++;

                    // Purple_1.Participant[] participants = new Purple_1.Participant[ParticipantsCount];
                    Array.Resize(ref participants, ParticipantsCount);

                    for (int i = 0; i < ParticipantsCount && currLine < lines.Length; i++){
                        Dictionary<string, string> PartData = new Dictionary<string, string>();
                        for (int j = 0; j < 5 && j < lines.Length; j++){
                            var parsedLine = lines[currLine].Split(":");
                            PartData[parsedLine[0].Trim()] = parsedLine[1].Trim();
                            currLine++;
                        }
                        double[] coefs = new double [4];
                        string[] coef = PartData.ContainsKey("Coefs") ? PartData["Coefs"].Split("|") : null;
                        if (coef == null){
                            return null;
                        }
                        for (int k = 0; k < coef.Length && k < coefs.Length; k++){
                            double.TryParse(coef[k].Trim(), out coefs[k]);
                        }
                        participants[i] = new Purple_1.Participant(PartData["Name"], PartData["Surname"]);
                        participants[i].SetCriterias(coefs);

                        //int[,] marks = new int[4, 7];
                        // Marks:1,2,3,4;5,6,7,8
                        string[] rows = PartData.ContainsKey("Marks") ? PartData["Marks"].Split(";") : null; //parse all rows
                        if (rows == null){
                            return null;
                        }
                        for (int k = 0; k < rows.Length; k++){ //loop in rows
                            string[] cols = rows[k].Split(","); // parse rows elements
                            int[] marks = new int[7];
                            for (int j = 0; j < cols.Length; j++){
                                int.TryParse(cols[j], out marks[j]);
                            }
                            participants[i].Jump(marks);
                        }
                    } 
                
                
                }
                

                Purple_1.Judge[] judges = new Purple_1.Judge[0];
                if (currLine < lines.Length && lines[currLine].StartsWith("JudgesCount")){
                    int.TryParse(lines[currLine].Split(":")[1].Trim(), out int JudgesCount);
                    currLine++;

                    //Purple_1.Judge[] judges = new Purple_1.Judge[JudgesCount];
                    Array.Resize(ref judges, JudgesCount);

                    for (int i = 0; i < JudgesCount && i < lines.Length; i++){
                        Dictionary<string, string> judgeData = new Dictionary<string, string>();
                        for (int j = 0; j < 2 && j < lines.Length; j++){
                            var parsedLine = lines[currLine].Split(":");
                            judgeData[parsedLine[0].Trim()] = parsedLine[1].Trim();
                            currLine++;
                        }

                    string[] stringMarks = judgeData.ContainsKey("Marks") ? judgeData["Marks"].Split(",") : null;
                
                    if (stringMarks == null){
                        stringMarks = new string[0];
                    }

                    int[] marks = new int[stringMarks.Length];

                    for (int k = 0; k < stringMarks.Length; k++){
                        int.TryParse(stringMarks[k], out marks[k]);
                    }
                    judges[i] = new Purple_1.Judge(judgeData["Name"], marks);
                    }
                }
                
                // Purple_1.Judge judge = new Purple_1.Judge("Вадим", new int[]{1, 2, 3, 4, 5, 6, 7});
                // Purple_1.Judge[] test_judges = new Purple_1.Judge[1]{judge};
                Purple_1.Competition competition = new Purple_1.Competition(judges);
                competition.Add(participants);

                T comp = competition as T;
                return comp;
            }
            
            // Purple_1.Participant nullParticipant = new Purple_1.Participant("Вадим", "Вадим");
            // T nullPart = nullParticipant as T;
            // return nullPart;
        }


        private void Purple2ParticipantWriter(StreamWriter writer, Purple_2.Participant participant){
            writer.WriteLine($"ParticipantName:{participant.Name}");
            writer.WriteLine($"Surname:{participant.Surname}");
            writer.WriteLine($"Distance:{participant.Distance}");
            writer.WriteLine($"Result:{participant.Result}");
            writer.WriteLine($"Marks:{String.Join("|", participant.Marks)}");
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName){
            using (StreamWriter writer = File.AppendText(fileName)){
                if (jumping is Purple_2.JuniorSkiJumping){
                    writer.WriteLine($"Type:{nameof(Purple_2.JuniorSkiJumping)}");
                }
                else if (jumping is Purple_2.ProSkiJumping){
                    writer.WriteLine($"Type:{nameof(Purple_2.ProSkiJumping)}");
                }
                writer.WriteLine($"Name:{jumping.Name}"); 
                writer.WriteLine($"Standard:{jumping.Standard}");
                writer.WriteLine($"ParticipantsCount:{jumping.Participants.Length}");
                foreach (var participant in jumping.Participants){
                    Purple2ParticipantWriter(writer, participant);
                }
            }
                
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName){
            throw new NotImplementedException();
        }

        // public override T DeserializePurple2SkiJumping<T>(string fileName){
        //     Dictionary <string, string> parsedObj = new Dictionary<string, string>();

        //     var lines = File.ReadAllLines(fileName);
        //     foreach(var line in lines){
        //         if (line.Contains(":")){
        //             var parts = line.Split(":");
        //             parsedObj[parts[0].Trim()] = parts[1].Trim();
        //         }
        //     }
        //     string type = parsedObj["Type"];
            
        //     string name = parsedObj["Name"];
        //     int.TryParse(parsedObj["Standard"], out int standard);
        //     Purple_2.SkiJumping jumper;
        //     if (type == nameof(Purple_2.JuniorSkiJumping))
        //     {
        //         jumper = new Purple_2.JuniorSkiJumping();
        //     }
        //     else
        //     {
        //         jumper = new Purple_2.ProSkiJumping();
        //     }

        //     int.TryParse(parsedObj["ParticipantsCount"], out int participantsCount);
        //     int currLine = 4;
        //     Purple_2.Participant[] participants = new Purple_2.Participant[participantsCount];

        //     for (int i = 0; i < participantsCount; i++){
        //         Dictionary<string, string> parsedParticipant = new Dictionary<string, string>();
        //         for (int k = 0; k < 5; k++){
        //             if (lines[currLine].Contains(":")){
        //                 var parsedParts = lines[currLine+k].Split(":");
        //                 parsedParticipant[parsedParts[0].Trim()] = parsedParts[1].Trim();
        //             }
        //         }
        //         currLine+=5;
        //         string participantName = parsedParticipant["ParticipantName"];
        //         string surname = parsedParticipant["Surname"];
        //         var distance = int.Parse(parsedParticipant["Distance"]);
        //         var result = int.Parse(parsedParticipant["Result"]);

        //         int[] marks = new int[5];
        //         string[] mark = parsedParticipant.ContainsKey("Marks") ? parsedParticipant["Marks"].Split("|") : null;

        //         for (int j = 0; j < mark.Length && j < marks.Length; j++){
        //             int.TryParse(mark[j].Trim(), out marks[j]);
        //         }
        //         participants[i] = new Purple_2.Participant(participantName, surname);
        //         //int target = type == nameof(Purple_2.JuniorSkiJumping) ? 100 : 150;
        //         participants[i].Jump(distance, marks, jumper.Standard);//target
                
        //         //jumper.Jump(distance, marks);
        //     }
        //     jumper.Add(participants);
        //     // if (type == nameof(Purple_2.JuniorSkiJumping)){
        //     //     Purple_2.JuniorSkiJumping jumper = new Purple_2.JuniorSkiJumping();
        //     //     jumper.Add(participants);
        //     //     T jump = jumper as T;
        //     //     return jump;
        //     // }
        //     // else{
        //     //     Purple_2.ProSkiJumping jumper = new Purple_2.ProSkiJumping();
        //     //     jumper.Add(participants);
        //     //     T jump = jumper as T;
        //     //     return jump;
        //     // }
            
        //     // Purple_1.Participant nullParticipant = new Purple_1.Participant("Вадим", "Вадим");
        //     T jump = jumper as T;
        //     return jump;
            
        // }

        public override void SerializePurple3Skating<T>(T skating, string fileName){
            throw new NotImplementedException();
        }


        public override void SerializePurple4Group(Purple_4.Group participant, string fileName){
            throw new NotImplementedException();
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName){
            throw new NotImplementedException();
        }


        public override T DeserializePurple3Skating<T>(string fileName){
            throw new NotImplementedException();
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName){
            throw new NotImplementedException();
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName){
            throw new NotImplementedException();
        }
    }
}


// using (StreamReader reader = new StreamReader(fileName)){//FilePath
                //     string type = reader.ReadLine().Split(":")[1].Trim(); //type
                //     int.TryParse(reader.ReadLine().Split(":")[1].Trim(), out int partCount); //numParticipants
                //     for (int i = 0; i < partCount; i++){
                //         Dictionary <string, string> parsedParticipants = new Dictionary<string, string>();
                //         //SelectFile(fileName); //FilePath doesnt workin' 
                //         for (int j = 0; j < 5; j++){
                //             var name_string = reader.ReadLine().Trim().Split(":");
                //             //parsedParticipants[name_string[0].Trim()] = name_string[1].Trim();

                //             var surname_string = reader.ReadLine().Trim().Split(":");
                //             parsedParticipants[surname_string[0].Trim()] = surname_string[1].Trim();

                //             double[] coefs = new double [4];

                //             var coef = reader.ReadLine().Trim().Split(":")[1].Trim().Split("|");
                //             // string[] coef = parsedObj.ContainsKey("Coefs") ? parsedObj["Coefs"].Split("|") : null;
                //             if (coef == null){
                //                 return null;
                //             }
                //             for (int k = 0; k < coef.Length && k < coefs.Length; k++){
                //                 double.TryParse(coef[i].Trim(), out coefs[i]);
                //             }
                //             Purple_1.Participant participant = new Purple_1.Participant(parsedObj["Name"], parsedObj["Surname"]);
                //             participant.SetCriterias(coefs);


                            
                //         }
                //         // double[] coefs = new double [4];
                //         // string[] coef = parsedObj.ContainsKey("Coefs") ? parsedObj["Coefs"].Split("|") : null;
                //         // if (coef == null){
                //         //     return null;
                //         // }
                //         // for (int i = 0; i < coef.Length && i < coefs.Length; i++){
                //         //     double.TryParse(coef[i].Trim(), out coefs[i]);
                //         // }
                //         // Purple_1.Participant participant = new Purple_1.Participant(parsedObj["Name"], parsedObj["Surname"]);
                //         // participant.SetCriterias(coefs);

                //         // //int[,] marks = new int[4, 7];
                //         // // Marks:1,2,3,4;5,6,7,8
                //         // string[] rows = parsedObj.ContainsKey("Marks") ? parsedObj["Marks"].Split(";") : null;
                //         // if (rows == null){
                //         //     return null;
                //         // }
                //         // for (int i = 0; i < rows.Length; i++){
                //         //     string[] cols = rows[i].Split(",");
                //         //     int[] marks = new int[7];
                //         //     for (int j = 0; j < cols.Length; j++){
                //         //         int.TryParse(cols[j], out marks[j]);
                //         //     }
                //         //     participant.Jump(marks);
                //         // }      
                //     }
                // }