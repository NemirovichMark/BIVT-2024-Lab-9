using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Lab_9;
using Lab_7;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer{
        public override string Extension => "json";

        private class tmp_Purple_1Participant{
            public string Name{ get; set; }
            public string Surname{ get; set; }
            public double[] Coefs{ get; set; }
            public int[,] Marks{get; set; }
        }

        private class tmp_Purple_1Competition{
            public Purple_1.Judge[] Judges{ get; set; }
            public tmp_Purple_1Participant[] Participants{get; set; }
        }

        private void Purple1ParticipantWriter(StreamWriter writer, Purple_1.Participant participant){
            tmp_Purple_1Participant part = new tmp_Purple_1Participant{
                Name = participant.Name,
                Surname = participant.Surname,
                Coefs = participant.Coefs,
                Marks = participant.Marks,
            };
            string json = JsonConvert.SerializeObject(part);
            writer.WriteLine();
            writer.Write(json);
        }

        public override void SerializePurple1<T>(T obj, string fileName){            
            using(StreamWriter writer = new StreamWriter(fileName)){
                if (obj is Purple_1.Participant participant){
                    Purple1ParticipantWriter(writer, participant);
                }
                else if (obj is Purple_1.Judge judge){
                    writer.WriteLine(JsonConvert.SerializeObject(judge));
                }
                else{
                    var competition = obj as Purple_1.Competition;
                    var tmp_partcipants = new tmp_Purple_1Participant[competition.Participants.Length];
                    for (int i = 0; i < competition.Participants.Length; i++){
                        tmp_partcipants[i] = new tmp_Purple_1Participant{
                            Name = competition.Participants[i].Name,
                            Surname = competition.Participants[i].Surname,
                            Coefs = competition.Participants[i].Coefs,
                            Marks = competition.Participants[i].Marks,

                        };
                    }
                    var compet = new tmp_Purple_1Competition{
                        Participants = tmp_partcipants,
                        Judges = competition.Judges,
                    };
                    writer.WriteLine(JsonConvert.SerializeObject(compet));
                }
            }
        }

        public override T DeserializePurple1<T>(string fileName){
            var content = File.ReadAllText(fileName);
            if (typeof(T) == typeof(Purple_1.Participant)){
                tmp_Purple_1Participant deserializedObj = JsonConvert.DeserializeObject<tmp_Purple_1Participant>(content);
                Purple_1.Participant participant = new Purple_1.Participant(deserializedObj.Name, deserializedObj.Surname);
                participant.SetCriterias(deserializedObj.Coefs);
                for (int i = 0; i < deserializedObj.Marks.GetLength(0); i++){
                    var marks = new int[7];
                    for(int j = 0; j < deserializedObj.Marks.GetLength(1); j++){
                        marks[j] = deserializedObj.Marks[i, j];
                    }
                    participant.Jump(marks);
                }
                T part = participant as T;
                return part;
            }
            else if (typeof(T) == typeof(Purple_1.Judge)){ // у судьи все поля которые у него есть передаются в конструкторе, поэтому можно просто делать в лоб
                var deserializedObj = JsonConvert.DeserializeObject<T>(content);
                return deserializedObj;
            }  
            else{
                var compet = JsonConvert.DeserializeObject<tmp_Purple_1Competition>(content);
                var competition = new Purple_1.Competition(compet.Judges);
                for (int i = 0; i < compet.Participants.Length; i++){
                    var participant = new Purple_1.Participant(compet.Participants[i].Name, compet.Participants[i].Surname);
                    participant.SetCriterias(compet.Participants[i].Coefs);
                    for (int k = 0; k < compet.Participants[i].Marks.GetLength(0); k++){
                        var marks = new int[7];
                        for(int j = 0; j < compet.Participants[i].Marks.GetLength(1); j++){
                            marks[j] = compet.Participants[i].Marks[k, j];
                        }
                        participant.Jump(marks);
                    }
                    competition.Add(participant);
                }
                T comp = competition as T;
                return comp;
            } 
        }

        private class tmpPurple_2Participant{
            public string Name { get; set; }
            public string Surname { get; set;}
            public int Distance {get; set; }
            public int[] Marks {get; set; }
            public int Result {get; set; }
        }

        private class tmpPurple_2Jumper{
            public string Type { get; set; }
            public string Name { get; set; }
            public int Standard{ get; set; }
            public tmpPurple_2Participant[] Participants { get; set; }
        }

        // public override void SerializePurple2SkiJumping<T>(T jumping, string fileName){
        //     throw new NotImplementedException();
        // } 

        // public override T DeserializePurple2SkiJumping<T>(string fileName){
        //     throw new NotImplementedException();
        // }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName){
            var Name = jumping.Name;
            var Standard = jumping.Standard;
            
            var participants = new tmpPurple_2Participant[jumping.Participants.Length];
            for (int i = 0; i < jumping.Participants.Length; i++){
                participants[i] = new tmpPurple_2Participant{
                    Name = jumping.Participants[i].Name,
                    Surname = jumping.Participants[i].Surname,
                    Distance = jumping.Participants[i].Distance,
                    Marks = jumping.Participants[i].Marks,
                    Result = jumping.Participants[i].Result,
                };
            }

            var jumper = new tmpPurple_2Jumper{
                Type = jumping.GetType().Name,
                Name = Name,
                Standard = Standard,
                Participants = participants,
            };

            string json = JsonConvert.SerializeObject(jumper);

            using (var stream = new StreamWriter(fileName)){
                stream.Write(json);
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName){
            var content = File.ReadAllText(fileName);
            tmpPurple_2Jumper jumper = JsonConvert.DeserializeObject<tmpPurple_2Jumper>(content);
            Purple_2.SkiJumping tmpJumper;
            if (jumper.Type == nameof(Purple_2.ProSkiJumping)){
                tmpJumper = new Purple_2.ProSkiJumping();
            }
            else{
                tmpJumper = new Purple_2.JuniorSkiJumping();
            }
            Purple_2.Participant[] participants = new Purple_2.Participant[jumper.Participants.Length];
            for (int i = 0; i < jumper.Participants.Length; i++){
                participants[i] = new Purple_2.Participant(jumper.Participants[i].Name, jumper.Participants[i].Surname);
                double target;
                var marks = jumper.Participants[i].Marks;
                var result = jumper.Participants[i].Result;
                var distance = jumper.Participants[i].Distance;
                var markMinMax = marks.Sum() - marks.Min() - marks.Max();
                if (result != 0){
                    target = distance - ((result - markMinMax - 60)/2.0);
                }
                else{
                    target = distance + (result + markMinMax + 60);
                }
                participants[i].Jump(jumper.Participants[i].Distance, jumper.Participants[i].Marks, (int)target);//tmpJumper.Standard
            }
            tmpJumper.Add(participants);
            T jump = tmpJumper as T;
            return jump;
        }
        
        private class tmp_Purple_3Participant{
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks {get; set; }
            public int[] Places{get; set; }
            public int Score {get; set; }

        }

        private class tmp_Purple_3Skating{
            public string Type { get; set; }
            public tmp_Purple_3Participant[] Participants {get; set; }
            public double[] Moods {get; set; }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName){
            string Type;
            if (skating is Purple_3.FigureSkating){
                Type = nameof(Purple_3.FigureSkating);
            }
            else{
                Type = nameof(Purple_3.IceSkating);
            }
            tmp_Purple_3Participant[] participants = new tmp_Purple_3Participant[skating.Participants.Length];
            for (int i = 0; i < skating.Participants.Length; i++){
                participants[i] = new tmp_Purple_3Participant{
                    Name = skating.Participants[i].Name,
                    Surname = skating.Participants[i].Surname,
                    Marks = skating.Participants[i].Marks,
                    Places = skating.Participants[i].Places,
                    Score = skating.Participants[i].Score};
            }

            var skater = new tmp_Purple_3Skating{
                Type = Type,
                Participants = participants,
                Moods = skating.Moods,
            };

            var json = JsonConvert.SerializeObject(skater);//, Formatting.Indented
            using (StreamWriter writer = new StreamWriter(fileName)){
                writer.WriteLine(json);
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName){
            var content = File.ReadAllText(fileName);
            tmp_Purple_3Skating tmpSkater = JsonConvert.DeserializeObject<tmp_Purple_3Skating>(content);
            Purple_3.Skating sakter;
            double[] moods = tmpSkater.Moods;
            if (tmpSkater.Type == nameof(Purple_3.IceSkating)){
                // for (int i = 0; i < moods.Length; i++){
                //     //moods[i] += moods[i]*((i+1)/100.0);
                //     //ans = moods[i](1 + ((i+1)/100.0))
                //     //moods[i] = ans/(1 + ((i+1)/100.0))
                //     moods[i] /= 1 + ((i+1)/100.0);
                // }
                sakter = new Purple_3.IceSkating(moods, needModificate: false);
            }
            else{
                // for (int i = 0; i < moods.Length; i++){
                //     //moods[i] += (i+1)/10.0;
                //     moods[i] -= (i+1)/10.0;
                // }
                sakter = new Purple_3.FigureSkating(moods, needModificate: false);
            }
            
            for (int i = 0; i < tmpSkater.Participants.Length; i++){
                var participant = new Purple_3.Participant(tmpSkater.Participants[i].Name, tmpSkater.Participants[i].Surname);
                foreach (var mark in tmpSkater.Participants[i].Marks){
                    participant.Evaluate(mark);
                }
                sakter.Add(participant);
                sakter.Evaluate(tmpSkater.Participants[i].Marks);
            }

            T skat = sakter as T;
            return skat;
        }

        private class tmp_Purple4Sportsman{
            public string Type {get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time {get; set; }
        }

        private class tmp_Purple4Group{
            public string Name {get; set; }
            public tmp_Purple4Sportsman[] Sportsmen {get; set; }
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string fileName){
            tmp_Purple4Sportsman[] sportsmens = new tmp_Purple4Sportsman[participant.Sportsmen.Length];
            for (int i = 0; i < sportsmens.Length; i++){
                string type;
                if (participant.Sportsmen[i] is Purple_4.SkiMan){
                    type = nameof(Purple_4.SkiMan);
                }
                else if (participant.Sportsmen[i] is Purple_4.SkiWoman){
                    type = nameof(Purple_4.SkiWoman);
                }
                else{
                    type = nameof(Purple_4.Sportsman);
                }
                sportsmens[i] = new tmp_Purple4Sportsman{
                    Type = type,
                    Name = participant.Sportsmen[i].Name,
                    Surname = participant.Sportsmen[i].Surname,
                    Time = participant.Sportsmen[i].Time,
                };
            }
            tmp_Purple4Group group = new tmp_Purple4Group{
                Name = participant.Name, 
                Sportsmen = sportsmens
            };
            string json = JsonConvert.SerializeObject(group);
            using (StreamWriter writer = new StreamWriter(fileName)){
                writer.WriteLine(json);
            }
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName){
            string content = File.ReadAllText(fileName);
            tmp_Purple4Group tmpGroup = JsonConvert.DeserializeObject<tmp_Purple4Group>(content);
            Purple_4.Sportsman[] sportsmen = new Purple_4.Sportsman[tmpGroup.Sportsmen.Length];
            for (int i = 0; i < sportsmen.Length; i++){
                if (tmpGroup.Sportsmen[i].Type == nameof(Purple_4.SkiMan)){
                    sportsmen[i] = new Purple_4.SkiMan(tmpGroup.Sportsmen[i].Name, tmpGroup.Sportsmen[i].Surname);
                }
                else if (tmpGroup.Sportsmen[i].Type == nameof(Purple_4.SkiWoman)){
                    sportsmen[i] = new Purple_4.SkiWoman(tmpGroup.Sportsmen[i].Name, tmpGroup.Sportsmen[i].Surname);
                }
                else{
                    sportsmen[i] = new Purple_4.Sportsman(tmpGroup.Sportsmen[i].Name, tmpGroup.Sportsmen[i].Surname);
                }
                sportsmen[i].Run(tmpGroup.Sportsmen[i].Time);
            }
            Purple_4.Group Group = new Purple_4.Group(tmpGroup.Name);
            Group.Add(sportsmen);
            return Group;
        }

        private class tmp_Purple5Response{
            public string Animal { get; set; }
            public string CharacterTrait {get; set; }
            public string Concept { get; set ; } 
        }

        private class tmp_Purple5Research{
            public string Name { get; set; } 
            public tmp_Purple5Response[] Responses {get; set; }
        }

        private class tmp_Purple5Report{
            public tmp_Purple5Research[] Researches {get; set; }
        }

        public override void SerializePurple5Report(Purple_5.Report group, string fileName){
            tmp_Purple5Research[] researches = new tmp_Purple5Research[group.Researches.Length];
            for(int i = 0; i < researches.Length; i++){
                var resName = group.Researches[i].Name;
                tmp_Purple5Response[] responses = new tmp_Purple5Response[group.Researches[i].Responses.Length];
                for (int j = 0; j < group.Researches[i].Responses.Length; j++){
                    responses[j] = new tmp_Purple5Response{
                        Animal = group.Researches[i].Responses[j].Animal,
                        CharacterTrait = group.Researches[i].Responses[j].Animal,
                        Concept = group.Researches[i].Responses[j].Animal,
                    };
                }
                researches[i] = new tmp_Purple5Research{
                    Name = resName,
                    Responses = responses,
                };
            }
            var report = new tmp_Purple5Report{
                Researches = researches,
            };
            string json = JsonConvert.SerializeObject(report);
            using (StreamWriter writer = new StreamWriter(fileName)){
                writer.WriteLine(json);
            }
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName){
            string content = File.ReadAllText(fileName);
            tmp_Purple5Report tmpReport = JsonConvert.DeserializeObject<tmp_Purple5Report>(content);
            Purple_5.Research[] researches = new Purple_5.Research[tmpReport.Researches.Length];
            for (int i = 0; i < researches.Length; i++){
                Purple_5.Response[] responses = new Purple_5.Response[tmpReport.Researches[i].Responses.Length];
                researches[i] = new Purple_5.Research(tmpReport.Researches[i].Name);
                string[] stringResponses = new string[3];
                for (int j = 0; j < responses.Length; j++){
                    //responses[j] = new Purple_5.Response(tmpReport.Researches[i].Responses[j].Animal, tmpReport.Researches[i].Responses[j].CharacterTrait, tmpReport.Researches[i].Responses[j].Concept);
                    stringResponses[0] = String.IsNullOrEmpty(tmpReport.Researches[i].Responses[j].Animal) ? null : tmpReport.Researches[i].Responses[j].Animal;
                    stringResponses[1] = String.IsNullOrEmpty(tmpReport.Researches[i].Responses[j].CharacterTrait) ? null: tmpReport.Researches[i].Responses[j].CharacterTrait;
                    stringResponses[2] = String.IsNullOrEmpty(tmpReport.Researches[i].Responses[j].Concept) ? null : tmpReport.Researches[i].Responses[j].Concept;
                    researches[i].Add(stringResponses);
                }
            }
            var report = new Purple_5.Report();
            report.AddResearch(researches);
            return report;
        }
    }
}