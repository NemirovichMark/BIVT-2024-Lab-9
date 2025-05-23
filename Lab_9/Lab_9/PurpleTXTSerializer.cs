using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Lab_7.Purple_1;
using static Lab_7.Purple_2;
using static Lab_7.Purple_3;
using static Lab_7.Purple_4;
using static Lab_7.Purple_5;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        private void SerializeObject<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            if (obj == null || string.IsNullOrEmpty(FilePath)) return;
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };

            string dataJson = JsonConvert.SerializeObject(obj, settings);

            using (var writer = new StreamWriter(FilePath)) writer.Write(dataJson);
        }
        private JObject DeserializeData(string fileName)
        {
            SelectFile(fileName);
            if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath)) return null;
            string dataJson = string.Empty;
            using (var reader = new StreamReader(FilePath)) dataJson = reader.ReadToEnd();

            var data = JsonConvert.DeserializeObject<JObject>(dataJson);
            return data;
        }

        public override void SerializePurple1<T>(T obj, string fileName) { SerializeObject(obj, fileName); }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName) { SerializeObject(jumping, fileName); }
        public override void SerializePurple3Skating<T>(T skating, string fileName) { SerializeObject(skating, fileName); }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName) { SerializeObject(group, fileName); }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName) { SerializeObject(report, fileName); }

        public override T DeserializePurple1<T>(string fileName)
        {
            var data = DeserializeData(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
                return DeserializePurple1Participant(data) as T;
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                string name = data["Name"].ToObject<string>();
                int[] marks = data["Marks"].ToObject<int[]>();

                var judge = new Purple_1.Judge(name, marks);

                return judge as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var participants = data["Participants"]["$values"];
                var judges = data["Judges"]["$values"].ToObject<Judge[]>();
                var newCompetition = new Competition(judges);

                foreach (var participant in participants)
                    newCompetition.Add(DeserializePurple1Participant(participant));

                return newCompetition as T;
            }
            return null;
        }
        private Purple_1.Participant DeserializePurple1Participant(JToken participant)
        {
            string name = participant["Name"].ToObject<string>();
            string surname = participant["Surname"].ToObject<string>();
            double[] coefs = participant["Coefs"].ToObject<double[]>();
            int[,] marks = participant["Marks"].ToObject<int[,]>();

            var newParticipant = new Purple_1.Participant(name, surname);
            newParticipant.SetCriterias(coefs);

            for (int i = 0; i < marks.GetLength(0); i++)
            {
                int[] curJumpMarks = new int[marks.GetLength(1)];
                for (int j = 0; j < marks.GetLength(1); j++)
                    curJumpMarks[j] = marks[i, j];

                newParticipant.Jump(curJumpMarks);
            }
            return newParticipant;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            var data = DeserializeData(fileName);
            var participants = data["Participants"]["$values"];
            string typeName = data["$type"].ToObject<string>();
            var type = Type.GetType(typeName);
            int targetDistance = data["Standard"].ToObject<int>();
            dynamic resultObj = Activator.CreateInstance(type);
            foreach (var participant in participants)
            {
                var newParticipant = DeserializePurple2Participant(participant, targetDistance);
                resultObj.Add(newParticipant);
            }
            return resultObj as T;
        }
        private Purple_2.Participant DeserializePurple2Participant(JToken participant, int target)
        {
            string name = participant["Name"].ToObject<string>();
            string surname = participant["Surname"].ToObject<string>();
            int distance = participant["Distance"].ToObject<int>();
            int[] marks = participant["Marks"].ToObject<int[]>();

            var newParticipant = new Purple_2.Participant(name, surname);
            newParticipant.Jump(distance, marks, target);

            return newParticipant;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            var data = DeserializeData(fileName);
            var participants = data["Participants"]["$values"];
            double[] moods = data["Moods"].ToObject<double[]>();

            string typeName = data["$type"].ToObject<string>();
            var type = Type.GetType(typeName);
            object[] constructorArgs = new object[] { moods, false };
            dynamic resultObj = Activator.CreateInstance(type, constructorArgs);

            var newParticipants = new Purple_3.Participant[participants.ToObject<Purple_3.Participant[]>().Length];
            int k = 0;
            foreach (var participant in participants)
            {
                var newParticipant = DeserializePurple3Participant(participant);
                newParticipants[k] = newParticipant;
                k++;
            }

            resultObj.Add(newParticipants);
            Purple_3.Participant.SetPlaces(resultObj.Participants);

            return resultObj as T;
        }
        private Purple_3.Participant DeserializePurple3Participant(JToken participant)
        {
            string name = participant["Name"].ToObject<string>();
            string surname = participant["Surname"].ToObject<string>();
            double[] marks = participant["Marks"].ToObject<double[]>();
            int[] places = participant["Places"].ToObject<int[]>();

            var newParticipant = new Purple_3.Participant(name, surname);
            for (int i = 0; i < marks.Length; i++)
                newParticipant.Evaluate(marks[i]);

            return newParticipant;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            var data = DeserializeData(fileName);
            var sportsmen = data["Sportsmen"]["$values"];
            string name = data["Name"].ToObject<string>();

            var newGroup = new Purple_4.Group(name);
            foreach (var sportsman in sportsmen)
            {
                var newSportsman = DeserializePurple4Sportsman(sportsman);
                newGroup.Add(newSportsman);
            }
            return newGroup;
        }
        private dynamic DeserializePurple4Sportsman(JToken sportsman)
        {
            string name = sportsman["Name"].ToObject<string>();
            string surname = sportsman["Surname"].ToObject<string>();
            double time = sportsman["Time"].ToObject<double>();

            string typeName = sportsman["$type"].ToObject<string>();
            var type = Type.GetType(typeName);
            object[] constructorArgs = new object[] { name, surname };
            dynamic resultObj = Activator.CreateInstance(type, constructorArgs);
            resultObj.Run(time);

            return resultObj;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            var researches = DeserializeData(fileName)["Researches"]["$values"];
            var newReport = new Purple_5.Report();
            foreach (var research in researches)
            {
                newReport.AddResearch(DeserializePurple5Reserch(research));
            }
            return newReport;
        }
        private Purple_5.Research DeserializePurple5Reserch(JToken research)
        {
            string name = research["Name"].ToObject<string>();
            var responses = research["Responses"]["$values"];

            var newResearch = new Purple_5.Research(name);
            foreach (JObject response in responses)
            {
                string[] responseValues = response.Properties().Where(y => y.Name != "$type").Select(value => value.Value.ToString()).ToArray();
                newResearch.Add(responseValues);
            }
            return newResearch;
        }

        private void Print<T>(T[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        private void Print<T>(T[] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                Console.Write(array[i]);
            }
            Console.WriteLine();
        }
    }
}