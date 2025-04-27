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
            writer.WriteLine($"Type:{nameof(Purple_1.Participant)}");//participant.GetType().Name
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
            writer.WriteLine($"Type:{nameof(Purple_1.Judge)}");
            writer.WriteLine($"Name:{judge.Name}");
            writer.WriteLine($"Marks:{String.Join(",", judge.Marks)}");
        }

        public override void SerializePurple1<T>(T obj, string fileName) where T : class{
            if (obj == null) return;
            // using (StreamWriter writer = File.AppendText(fileName)){
            using (StreamWriter writer = File.CreateText(fileName)){
                if (obj is Purple_1.Participant participant){
                    Purple1ParticipantWriter(writer, participant);
                }

                else if (obj is Purple_1.Judge judge){
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

                    writer.WriteLine($"MainType:{nameof(Purple_1.Competition)}");
                    writer.WriteLine($"numParticipants:{participants.Length}");
                    foreach (Purple_1.Participant part in participants){
                        Purple1ParticipantWriter(writer, part);
                    }
                    writer.WriteLine($"numJudges:{judges.Length}");
                    foreach (Purple_1.Judge judg in judges){
                        Purple1JudgeWriter(writer, judg);
                    }
                    
                }
            }
        }

        public override T DeserializePurple1<T>(string fileName) where T : class{
            Dictionary <string, string> parsedObj = new Dictionary<string, string>();
            //SelectFile(fileName); //FilePath doesnt workin' 
            var lines = File.ReadAllLines(fileName);
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

            // else{
            //     using (StreamReader reader = new StreamReader(FilePath)){
            //         string type = reader.ReadLine();
            //         int.TryParse(reader.ReadLine().Trim(), out int partCount);
            //         for (int i = 0; i < partCount; i++){
            //             Dictionary <string, string> parsedObj = new Dictionary<string, string>();
            //             //SelectFile(fileName); //FilePath doesnt workin' 
            //             for (int j = 0; j < 5; j++){
            //                 if (line.Contains(":")){
            //                     var parts = line.Split(':');
            //                     parsedObj[parts[0].Trim()] = parts[1].Trim();
            //                 }
            //             }
            //             double[] coefs = new double [4];
            //             string[] coef = parsedObj.ContainsKey("Coefs") ? parsedObj["Coefs"].Split("|") : null;
            //             if (coef == null){
            //                 return null;
            //             }
            //             for (int i = 0; i < coef.Length && i < coefs.Length; i++){
            //                 double.TryParse(coef[i].Trim(), out coefs[i]);
            //             }
            //             Purple_1.Participant participant = new Purple_1.Participant(parsedObj["Name"], parsedObj["Surname"]);
            //             participant.SetCriterias(coefs);

            //             //int[,] marks = new int[4, 7];
            //             // Marks:1,2,3,4;5,6,7,8
            //             string[] rows = parsedObj.ContainsKey("Marks") ? parsedObj["Marks"].Split(";") : null;
            //             if (rows == null){
            //                 return null;
            //             }
            //             for (int i = 0; i < rows.Length; i++){
            //                 string[] cols = rows[i].Split(",");
            //                 int[] marks = new int[7];
            //                 for (int j = 0; j < cols.Length; j++){
            //                     int.TryParse(cols[j], out marks[j]);
            //                 }
            //                 participant.Jump(marks);
            //             }      
            //         }
            //     }
            // }

                // int.TryParse(parsedObj["numParticipants"], out int numParticipants);
                // // int.TryParse(parsedObj["numJudges"], out int numJudges);
            //}
            
            Purple_1.Participant nullParticipant = new Purple_1.Participant("Вадим", "Вадим");
            T nullPart = nullParticipant as T;
            return nullPart;
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName){
            throw new NotImplementedException();
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName){
            throw new NotImplementedException();
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName){
            throw new NotImplementedException();
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName){
            throw new NotImplementedException();
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName){
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