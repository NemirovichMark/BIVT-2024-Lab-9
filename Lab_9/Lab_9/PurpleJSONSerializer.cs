using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";

        private void SerializeAny<T>(T obj)
        {
            if (FilePath == null || FolderPath == null) return;

            string filePath = Path.Combine(FolderPath, $"{FilePath}.{Extension}");

            string serializedData = JsonConvert.SerializeObject(obj);

            JObject jObject = JObject.Parse(serializedData);
            jObject["$type"] = obj.GetType().AssemblyQualifiedName;

            serializedData = jObject.ToString();

            File.WriteAllText(filePath, serializedData);
        }

        private Dictionary<string, JToken> DeserializeAny()
        {
            if (FilePath == null || FolderPath == null) return null;

            string filePath = Path.Combine(FolderPath, $"{FilePath}.{Extension}");
            if (!File.Exists(filePath))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Dictionary<string, JToken>>(File.ReadAllText(filePath));
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            SerializeAny(obj);
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            SerializeAny(jumping);
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            SerializeAny(skating);
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            SerializeAny(group);
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            SerializeAny(report);
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            var dict = DeserializeAny();
            if (dict == null) return null;

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                string? name = dict.GetValueOrDefault("Name", null)?.ToObject<string>();
                string? surname = dict.GetValueOrDefault("Surname", null)?.ToObject<string>();
                double[]? coefs = dict.GetValueOrDefault("Coefs", null)?.ToObject<double[]>();
                int[,]? marks = dict.GetValueOrDefault("Marks", null)?.ToObject<int[,]>();

                Purple_1.Participant participant = new Purple_1.Participant(name, surname);
                participant.SetCriterias(coefs);

                for (int i = 0; i < 4; i++)
                {
                    int[] jumpMarks = new int[7];

                    for (int j = 0; j < 7; j++) jumpMarks[j] = marks[i, j];
                    participant.Jump(jumpMarks);
                }

                return participant as T;
            } else if (typeof(T) == typeof(Purple_1.Judge))
            {
                string? name = dict.GetValueOrDefault("Name", null)?.ToObject<string>();
                int[]? marks = dict.GetValueOrDefault("Marks", null)?.ToObject<int[]>();

                return (new Purple_1.Judge(name, marks)) as T;
            } else // Purple_1.Competition
            {
                Purple_1.Judge[] judges = dict.GetValueOrDefault("Judges", null)?.ToObject<Purple_1.Judge[]>();
                Purple_1.Competition competition = new Purple_1.Competition(judges);

                JToken? participantsObject = dict.GetValueOrDefault("Participants", null);

                List<Purple_1.Participant> listOfParticipants = new List<Purple_1.Participant>();

                foreach (var participantObject in participantsObject)
                {
                    var coefs = participantObject["Coefs"]?.ToObject<double[]>();
                    var marks = participantObject["Marks"]?.ToObject<int[,]>();

                    var participant = participantObject?.ToObject<Purple_1.Participant>();
                    participant.SetCriterias(coefs);

                    for (int i = 0; i < 4; i++)
                    {
                        int[] jumpMarks = new int[7];

                        for (int j = 0; j < 7; j++) jumpMarks[j] = marks[i, j];
                        participant.Jump(jumpMarks);
                    }

                    listOfParticipants.Add(participant);
                }

                competition.Add(listOfParticipants.ToArray());

                return competition as T;
            }

        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            var dict = DeserializeAny();
            if (dict == null) return null;

            Type? skijumpingType = Type.GetType(dict["$type"]?.ToObject<string>());
            string typeName = skijumpingType.Name;

            Purple_2.SkiJumping jumping;

            if (typeName == "JuniorSkiJumping")
            {
                jumping = new Purple_2.JuniorSkiJumping();
            } else // ProSkiJumping
            {
                jumping = new Purple_2.ProSkiJumping();
            }

            int? standard = dict.GetValueOrDefault("Standard", null)?.ToObject<int>();

            var participantsObject = dict.GetValueOrDefault("Participants", null);
            List<Purple_2.Participant> listOfParticipants = new List<Purple_2.Participant>();

            foreach (var participantObject in participantsObject)
            {
                int? distance = participantObject["Distance"].ToObject<int>();
                int? result = participantObject["Result"].ToObject<int>();
                int[]? marks = participantObject["Marks"].ToObject<int[]>();

                var participant = participantObject.ToObject<Purple_2.Participant>();
                participant.Jump((int)distance, marks, (int)standard);

                listOfParticipants.Add(participant);
            }

            jumping.Add(listOfParticipants.ToArray());

            return (T)jumping;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var dict = DeserializeAny();
            if (dict == null) return null;

            double[]? moods = dict.GetValueOrDefault("Moods", null)?.ToObject<double[]>();

            Type? skatingType = Type.GetType(dict["$type"]?.ToObject<string>());
            string typeName = skatingType.Name;

            Purple_3.Skating skating;

            if (typeName == "FigureSkating")
            {
                skating = new Purple_3.FigureSkating(moods, false);
            } else // IceSkating
            {
                skating = new Purple_3.IceSkating(moods, false);
            }

            var participantsObject = dict.GetValueOrDefault("Participants", null);
            List<Purple_3.Participant> listOfParticipants = new List<Purple_3.Participant>();

            foreach (var participantObject in participantsObject)
            {
                double[]? marks = participantObject["Marks"].ToObject<double[]>();

                var participant = participantObject.ToObject<Purple_3.Participant>();
                foreach (var mark in marks)
                    participant.Evaluate(mark);

                listOfParticipants.Add(participant);
            }

            Purple_3.Participant.SetPlaces(listOfParticipants.ToArray());

            skating.Add(listOfParticipants.ToArray());

            return (T)skating;

        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var dict = DeserializeAny();
            if (dict == null) return null;

            string? name = dict.GetValueOrDefault("Name", null)?.ToObject<string>();

            Purple_4.Group group = new Purple_4.Group(name);

            var sportsmenObject = dict.GetValueOrDefault("Sportsmen", null);
            List<Purple_4.Sportsman> listOfSportsmen = new List<Purple_4.Sportsman>();

            foreach (var sportsmanObject in sportsmenObject)
            {
                double? time = sportsmanObject["Time"]?.ToObject<double>();

                var sportsman = sportsmanObject.ToObject<Purple_4.Sportsman>();
                sportsman.Run((double)time);

                listOfSportsmen.Add(sportsman);
            }

            group.Add(listOfSportsmen.ToArray());

            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            var dict = DeserializeAny();
            if (dict == null) return null;

            Purple_5.Report report = new Purple_5.Report();

            var researchesObject = dict.GetValueOrDefault("Researches", null);
            List<Purple_5.Research> listOfResearches = new List<Purple_5.Research>();

            foreach (var researchObject in researchesObject)
            {
                var research = researchObject.ToObject<Purple_5.Research>();
                var responses = researchObject["Responses"].ToObject<Purple_5.Response[]>();

                foreach (var resp in responses)
                {
                    research.Add(new string[3] { resp.Animal, resp.CharacterTrait, resp.Concept });
                }

                listOfResearches.Add(research);
            }

            report.AddResearch(listOfResearches.ToArray());

            return report;
        }
    }
}
