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
            using (StreamWriter writer = new StreamWriter(fileName)){
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
            Dictionary <string, string> parsedObj = new Dictionary<string, string>();

            var lines = File.ReadAllLines(fileName);
            foreach(var line in lines){
                if (line.Contains(":")){
                    var parts = line.Split(":");
                    parsedObj[parts[0].Trim()] = parts[1].Trim();
                }
            }
            string type = parsedObj["Type"];
            
            string name = parsedObj["Name"];
            int.TryParse(parsedObj["Standard"], out int standard);
            Purple_2.SkiJumping jumper;
            if (type == nameof(Purple_2.JuniorSkiJumping))
            {
                jumper = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                jumper = new Purple_2.ProSkiJumping();
            }

            int.TryParse(parsedObj["ParticipantsCount"], out int participantsCount);
            int currLine = 4;
            Purple_2.Participant[] participants = new Purple_2.Participant[participantsCount];

            for (int i = 0; i < participantsCount; i++){
                Dictionary<string, string> parsedParticipant = new Dictionary<string, string>();
                for (int k = 0; k < 5; k++){
                    if (lines[currLine].Contains(":")){
                        var parsedParts = lines[currLine+k].Split(":");
                        parsedParticipant[parsedParts[0].Trim()] = parsedParts[1].Trim();
                    }
                }
                currLine+=5;
                string participantName = parsedParticipant["ParticipantName"];
                string surname = parsedParticipant["Surname"];
                var distance = int.Parse(parsedParticipant["Distance"]);
                var result = int.Parse(parsedParticipant["Result"]);

                int[] marks = new int[5];
                string[] mark = parsedParticipant.ContainsKey("Marks") ? parsedParticipant["Marks"].Split("|") : null;

                for (int j = 0; j < mark.Length && j < marks.Length; j++){
                    int.TryParse(mark[j].Trim(), out marks[j]);
                }
                participants[i] = new Purple_2.Participant(participantName, surname);

                //_result += deafultPoints + (_distance - target) * extraPoints;
                //_result = markMinMax + 60 + (_distance - target) * extraPoints;
                //target = distance - ((_result - markMinMax - 60)/2.0)

                double target;
                var markMinMax = marks.Sum() - marks.Min() - marks.Max();
                if (result != 0){
                    target = distance - ((result - markMinMax - 60)/2.0);
                }
                else{
                    target = distance + (result + markMinMax + 60);
                }
                participants[i].Jump(distance, marks, (int)target);// | jumper.Standard
            }
            jumper.Add(participants);
            T jump = jumper as T;
            return jump;
            
        }

        private void Purple_3ParticipantWriter(StreamWriter writer, Purple_3.Participant[] participants){
            foreach (var participant in participants){
                writer.WriteLine($"Name:{participant.Name}");
                writer.WriteLine($"Surname:{participant.Surname}");
                writer.WriteLine($"Marks:{String.Join("|", participant.Marks)}");
                writer.WriteLine($"Places:{String.Join("|", participant.Places)}");
                writer.WriteLine($"Score:{participant.Score}");

            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName){
            using (StreamWriter writer = new StreamWriter(fileName)){
                if (skating is Purple_3.FigureSkating){
                    writer.WriteLine($"Type:{nameof(Purple_3.FigureSkating)}");
                }
                else{
                    writer.WriteLine($"Type:{nameof(Purple_3.IceSkating)}");
                }
                writer.WriteLine($"Moods:{String.Join("|", skating.Moods)}");
                writer.WriteLine($"ParticipantsCount:{skating.Participants.Length}");
                Purple_3ParticipantWriter(writer, skating.Participants);
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName){
            Dictionary<string, string> parsedObj = new Dictionary<string, string>();
            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines){
                if (line.Contains(":")){
                    var parts = line.Split(":");
                    parsedObj[parts[0].Trim()] = parts[1].Trim();
                }
            }
            var type = parsedObj["Type"];
            var moods = new double[7];
            var mood = parsedObj.ContainsKey("Moods") ? parsedObj["Moods"].Split("|") : null;
            for (int i = 0; i < Math.Min(7, moods.Length); i++){
                double.TryParse(mood[i], out moods[i]);
            }

            Purple_3.Skating skater;
            if (type == nameof(Purple_3.FigureSkating)){
                skater = new Purple_3.FigureSkating(moods, needModificate: false);
            }
            else{
                skater = new Purple_3.IceSkating(moods, needModificate: false);
            }
            
            int.TryParse(parsedObj["ParticipantsCount"], out int ParticipantsCount);
            Purple_3.Participant[] participants = new Purple_3.Participant[ParticipantsCount];
            int currLine = 3;
            for (int i = 0; i < ParticipantsCount; i++){
                Dictionary<string, string> parsedParticipant = new Dictionary<string, string>();
                for (int k = 0; k < 5; k++){
                    if (lines[currLine].Contains(":")){
                        var parsedParts = lines[currLine+k].Split(":");
                        parsedParticipant[parsedParts[0].Trim()] = parsedParts[1].Trim();
                    }
                }
                currLine+=5;
                string partName = parsedParticipant["Name"];
                string partSurname = parsedParticipant["Surname"];
                var marks = new double[7];
                var mark = parsedParticipant.ContainsKey("Marks") ? parsedParticipant["Marks"].Split("|") : null;
                for (int j = 0; j < mark.Length; j++){
                    double.TryParse(mark[j], out marks[j]);
                }
                var places = new double[7];
                var place = parsedParticipant.ContainsKey("Places") ? parsedParticipant["Places"].Split("|") : null;
                for (int j = 0; j < place.Length; j++){
                    double.TryParse(place[j], out places[j]);
                }
                int.TryParse(parsedParticipant["Score"], out int partScore);
                participants[i] = new Purple_3.Participant(partName, partSurname);
                foreach (var partMark in marks){
                    participants[i].Evaluate(partMark);
                }
                
                skater.Add(participants[i]);
                skater.Evaluate(marks);
            }
            //Purple_3.Participant.SetPlaces(participants);
            
            T skat = skater as T;
            return skat;
        }


        public override void SerializePurple4Group(Purple_4.Group participant, string fileName){
            using (StreamWriter writer = new StreamWriter(fileName)){
                writer.WriteLine($"MainType:{nameof(Purple_4.Group)}");
                writer.WriteLine($"Name:{participant.Name}");
                writer.WriteLine($"ParticipantsCount:{participant.Sportsmen.Length}");
                foreach (var sportsman in participant.Sportsmen){
                    if (sportsman is Purple_4.SkiMan){
                        writer.WriteLine($"Type:{nameof(Purple_4.SkiMan)}");
                    }
                    else if (sportsman is Purple_4.SkiWoman){
                        writer.WriteLine($"Type:{nameof(Purple_4.SkiWoman)}");
                    }
                    else{
                        writer.WriteLine($"Type:{nameof(Purple_4.Sportsman)}");
                    }
                    writer.WriteLine($"SportsmanName:{sportsman.Name}");
                    writer.WriteLine($"SportsmanSurname:{sportsman.Surname}");
                    writer.WriteLine($"Time:{sportsman.Time}");
                }
            }
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName){
            Dictionary<string, string> parsedObj = new Dictionary<string, string>();
            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines){
                if (line.Contains(":")){
                    var parts = line.Split(':');
                    parsedObj[parts[0].Trim()] = parts[1].Trim();
                }
            }
            var MainType = parsedObj["MainType"];
            var Name = parsedObj["Name"];
            int.TryParse(parsedObj["ParticipantsCount"], out int ParticipantsCount);
            Purple_4.Sportsman[] sportsmen = new Purple_4.Sportsman[ParticipantsCount];
            int currLine = 3;
            for(int i = 0; i < ParticipantsCount; i++){
                Dictionary<string, string> parsedParticipant = new Dictionary<string, string>();
                for(int k = 0; k < 4; k++){
                    if (lines[currLine+k].Contains(":")){
                        var parsedPart = lines[currLine+k].Split(":");
                        parsedParticipant[parsedPart[0].Trim()] = parsedPart[1].Trim();
                    }
                }
                currLine+=4;
                var PartType = parsedParticipant["Type"];
                var PartName = parsedParticipant["SportsmanName"];
                var partSurname = parsedParticipant["SportsmanSurname"];
                double.TryParse(parsedParticipant["Time"], out double partTime);
                Purple_4.Sportsman sportsman;
                if (PartType == nameof(Purple_4.SkiMan)){
                    sportsman = new Purple_4.SkiMan(PartName, partSurname);
                }
                else if (PartType == nameof(Purple_4.SkiWoman)){
                    sportsman = new Purple_4.SkiWoman(PartName, partSurname);
                }
                else{
                    sportsman = new Purple_4.Sportsman(PartName, partSurname);
                }
                sportsman.Run(partTime); 
                sportsmen[i] = sportsman;
            }
        Purple_4.Group group = new Purple_4.Group(Name);
        group.Add(sportsmen);

        return group;
        }
     

        public override void SerializePurple5Report(Purple_5.Report group, string fileName){
            using (StreamWriter writer = new StreamWriter(fileName)){
                writer.WriteLine($"MainType:{nameof(Purple_5.Report)}");
                writer.WriteLine($"ResearchesCount:{group.Researches.Length}");
                foreach (var research in group.Researches){
                    writer.WriteLine($"ResearchName:{research.Name}");
                    writer.WriteLine($"ResponseCount:{research.Responses.Length}");
                    foreach (var response in research.Responses){
                        writer.WriteLine($"Animal:{response.Animal}");
                        writer.WriteLine($"CharacterTrait:{response.CharacterTrait}");
                        writer.WriteLine($"Concept:{response.Concept}");
                    }
                }
            }
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName){
            Dictionary<string, string> parsedObj = new Dictionary<string, string>();
            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines){
                if (line.Contains(":")){
                    var parts = line.Split(':');
                    parsedObj[parts[0].Trim()] = parts[1].Trim();
                }
            }
            var type = parsedObj["MainType"];
            int.TryParse(parsedObj["ResearchesCount"], out int ResearchesCount);
            Purple_5.Research[] researches = new Purple_5.Research[ResearchesCount];
            int currLine = 2;
            for(int i = 0; i < ResearchesCount; i++){
                Dictionary<string, string> parsedResearch = new Dictionary<string, string>();
                for (int j = 0; j < 2; j++){
                    if (lines[currLine+j].Contains(":")){
                        var parsedParts = lines[currLine+j].Split(":");
                        parsedResearch[parsedParts[0].Trim()] = parsedParts[1].Trim();
                    }
                }
                currLine+=2;
                var ResearchName = parsedResearch["ResearchName"];
                researches[i] = new Purple_5.Research(ResearchName);
                int.TryParse(parsedResearch["ResponseCount"], out int ResponseCount);
                //string[,] allResponses = new string[ResponseCount, 3]; //строка - одни из резпонзов, в каждой колонке ответ на соотв вопрос
                for(int k = 0; k < ResponseCount; k++){
                    Dictionary<string, string> responses = new Dictionary<string, string>();
                    string[] stringResponses = new string[3];
                    for (int z = 0; z < 3; z++){
                        if (lines[currLine+z].Contains(":")){
                            var parts = lines[currLine+z].Split(":");
                            responses[parts[0].Trim()] = parts[1].Trim();
                        }
                    }
                    currLine+=3;
                    stringResponses[0] = responses["Animal"];
                    stringResponses[1] = responses["CharacterTrait"];
                    stringResponses[2] = responses["Concept"];
                    researches[i].Add(stringResponses);
                }
                // currLine+=ResearchesCount*3;
            }
            var report = new Purple_5.Report();
            report.AddResearch(researches);
            return report;
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