using Lab_7;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        private void WriteData(Dictionary<string, string> data)
        {
            using (var sw = new StreamWriter(FilePath))
            {
                foreach (var row in data)
                {
                    sw.WriteLine($"{row.Key}:{row.Value}");
                }
            }
        }

        private Dictionary<string, string> GetDataFromFile()
        {
            var data = new Dictionary<string, string>();
            using (var sr = new StreamReader(FilePath))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    var row = line.Split(':');
                    data[row[0]] = row[1];
                    i++;
                }
            }
            return data;
        }

        private void AddPurple1Participant(Dictionary<string, string> data, Purple_1.Participant participant, int count = -1)
        {
            string prefix = "";
            if (count != -1) prefix = $"participant_{count}_";

            data[$"{prefix}type"] = "participant";
            data[$"{prefix}name"] = participant.Name;
            data[$"{prefix}surname"] = participant.Surname;
            data[$"{prefix}coefs"] = String.Join(';', participant.Coefs);
            data[$"{prefix}marks"] = String.Join(';', participant.Marks.Cast<int>());
        }

        private void AddPurple1Judge(Dictionary<string, string> data, Purple_1.Judge judge, int count = -1)
        {
            string prefix = "";
            if (count != -1) prefix = $"judge_{count}_";

            data[$"{prefix}type"] = "judge";
            data[$"{prefix}name"] = judge.Name;
            data[$"{prefix}marks"] = String.Join(";", judge.Marks);
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            var data = new Dictionary<string, string>();

            switch (obj)
            {
                case Purple_1.Participant participant:
                    AddPurple1Participant(data, participant);
                    break;
                case Purple_1.Judge judge:
                    AddPurple1Judge(data, judge);
                    break;
                case Purple_1.Competition competition:
                    var judges = competition.Judges;
                    var participants = competition.Participants;

                    data["type"] = "competition";
                    data["judges_count"] = judges.Length.ToString();
                    for (int i = 0; i < judges.Length; i++)
                    {
                        AddPurple1Judge(data, judges[i], i);
                    }

                    data["participants_count"] = participants.Length.ToString();
                    for (int i = 0; i < participants.Length; i++)
                    {
                        AddPurple1Participant(data, participants[i], i);
                    }

                    break;
            }

            WriteData(data);
        }
        private Purple_1.Participant DeserializePurple1Participant(Dictionary<string, string> data, int count = -1)
        {
            string prefix = "";
            if (count != -1) prefix = $"participant_{count}_";

            string name = data[$"{prefix}name"];
            string surname = data[$"{prefix}surname"];
            double[] coefs = data[$"{prefix}coefs"].Split(';').Select(double.Parse).ToArray();
            int[] marks = data[$"{prefix}marks"].Split(';').Select(int.Parse).ToArray();

            var participant = new Purple_1.Participant(name, surname);
            participant.SetCriterias(coefs);
            for (int i = 1; i <= 4; i++)
            {
                participant.Jump(marks[(7 * (i - 1))..(7 * i)]);
            }

            return participant;
        }
        private Purple_1.Judge DeserializePurple1Judge(Dictionary<string, string> data, int count = -1)
        {
            string prefix = "";
            if (count != -1) prefix = $"judge_{count}_";

            string name = data[$"{prefix}name"];
            int[] marks = data[$"{prefix}marks"].Split(';').Select(int.Parse).ToArray();

            var judge = new Purple_1.Judge(name, marks);

            return judge;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var data = GetDataFromFile();

            switch (data["type"])
            {
                case "participant":
                    return DeserializePurple1Participant(data) as T;
                case "judge":
                    return DeserializePurple1Judge(data) as T;
                case "competition":
                    int judges_count = int.Parse(data["judges_count"]);
                    var judges = new Purple_1.Judge[judges_count];
                    for (int i = 0; i < judges_count; i++)
                    {
                        judges[i] = DeserializePurple1Judge(data, i);
                    }

                    int participants_count = int.Parse(data["participants_count"]);
                    var participants = new Purple_1.Participant[participants_count];
                    for (int i = 0; i < participants_count; i++)
                    {
                        participants[i] = DeserializePurple1Participant(data, i);
                    }

                    var competition = new Purple_1.Competition(judges);
                    competition.Add(participants);

                    return competition as T;
            }

            return default(T);
        }
        private void AddPurple2Participant(Dictionary<string, string> data, Purple_2.Participant participant, int count)
        {
            string prefix = $"participant_{count}_";
            data[$"{prefix}name"] = participant.Name;
            data[$"{prefix}surname"] = participant.Surname;
            data[$"{prefix}distance"] = participant.Distance.ToString();
            data[$"{prefix}marks"] = String.Join(';', participant.Marks);
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var data = new Dictionary<string, string>();

            if (jumping is Purple_2.JuniorSkiJumping) data["type"] = "JuniorSkiJumping";
            else data["type"] = "ProSkiJumping";
            data["standard"] = jumping.Standard.ToString();
            data["participants_count"] = jumping.Participants.Length.ToString();
            for (int i = 0; i < jumping.Participants.Length; i++)
            {
                AddPurple2Participant(data, jumping.Participants[i], i);
            }

            WriteData(data);
        }
        private Purple_2.Participant DeserializePurple2Participant(Dictionary<string, string> data, int count, int standard)
        {
            string prefix = $"participant_{count}_";
            string name = data[$"{prefix}name"];
            string surname = data[$"{prefix}surname"];
            int distance = int.Parse(data[$"{prefix}distance"]);
            int[] marks = data[$"{prefix}marks"].Split(';').Select(int.Parse).ToArray();

            var participant = new Purple_2.Participant(name, surname);
            participant.Jump(distance, marks, standard);

            return participant;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var data = GetDataFromFile();

            string type = data["type"];
            int standard = int.Parse(data["standard"]);
            int participants_count = int.Parse(data["participants_count"]);
            var participants = new Purple_2.Participant[participants_count];
            for (int i = 0; i < participants_count; i++)
            {
                participants[i] = DeserializePurple2Participant(data, i, standard);
            }

            Purple_2.SkiJumping jumping;
            if (type == "JuniorSkiJumping") jumping = new Purple_2.JuniorSkiJumping();
            else jumping = new Purple_2.ProSkiJumping();
            jumping.Add(participants);

            return jumping as T;
        }
        private void AddPurple3Participant(Dictionary<string, string> data, Purple_3.Participant participant, int count)
        {
            string prefix = $"participant_{count}_";
            data[$"{prefix}name"] = participant.Name;
            data[$"{prefix}surname"] = participant.Surname;
            data[$"{prefix}marks"] = String.Join(';', participant.Marks);
            data[$"{prefix}places"] = String.Join(';', participant.Places);
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var data = new Dictionary<string, string>();

            if (skating is Purple_3.FigureSkating) data["type"] = "FigureSkating";
            else data["type"] = "IceSkating";

            data["participants_count"] = skating.Participants.Length.ToString();
            for (int i = 0; i < skating.Participants.Length; i++)
            {
                AddPurple3Participant(data, skating.Participants[i], i);
            }

            data["moods"] = String.Join(';', skating.Moods);

            WriteData(data);
        }
        private Purple_3.Participant DeserializePurple3Participant(Dictionary<string, string> data, int count)
        {
            string prefix = $"participant_{count}_";
            string name = data[$"{prefix}name"];
            string surname = data[$"{prefix}surname"];
            double[] marks = data[$"{prefix}marks"].Split(';').Select(double.Parse).ToArray();

            var participant = new Purple_3.Participant(name, surname);
            for (int i = 0; i < marks.Length; i++) participant.Evaluate(marks[i]);

            return participant;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var data = GetDataFromFile();

            string type = data["type"];
            int participants_count = int.Parse(data["participants_count"]);
            double[] moods = data["moods"].Split(';').Select(double.Parse).ToArray();

            Purple_3.Skating skating;
            if (type == "FigureSkating") skating = new Purple_3.FigureSkating(moods, false);
            else skating = new Purple_3.IceSkating(moods, false);

            var participants = new Purple_3.Participant[participants_count];
            for (int i = 0; i < participants_count; i++)
            {
                participants[i] = DeserializePurple3Participant(data, i);
            }
            Purple_3.Participant.SetPlaces(participants);
            skating.Add(participants);

            return skating as T;
        }
        private void AddPurple4Sportsman(Dictionary<string, string> data, Purple_4.Sportsman sportsman, int count)
        {
            string prefix = $"sportsman_{count}_";
            data[$"{prefix}name"] = sportsman.Name;
            data[$"{prefix}surname"] = sportsman.Surname;
            data[$"{prefix}time"] = sportsman.Time.ToString();
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            var data = new Dictionary<string, string>();

            data["name"] = participant.Name;
            data["sportsmen_count"] = participant.Sportsmen.Length.ToString();
            for (int i = 0; i < participant.Sportsmen.Length; i++)
            {
                AddPurple4Sportsman(data, participant.Sportsmen[i], i);
            }

            WriteData(data);
        }
        private Purple_4.Sportsman DeserializePurple4Sportsman(Dictionary<string, string> data, int count)
        {
            string prefix = $"sportsman_{count}_";
            string name = data[$"{prefix}name"];
            string surname = data[$"{prefix}surname"];
            double time = double.Parse(data[$"{prefix}time"]);

            var sportsman = new Purple_4.Sportsman(name, surname);
            sportsman.Run(time);

            return sportsman;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var data = GetDataFromFile();

            string name = data["name"];
            int sportsmen_count = int.Parse(data["sportsmen_count"]);
            var group = new Purple_4.Group(name);
            for (int i = 0; i < sportsmen_count; i++)
            {
                group.Add(DeserializePurple4Sportsman(data, i));
            }

            return group;
        }
        private void AddPurple5Response(Dictionary<string, string> data, Purple_5.Response response, int research_count, int response_count)
        {
            string prefix = $"response_{research_count}_{response_count}_";
            data[$"{prefix}animal"] = response.Animal;
            data[$"{prefix}character_trait"] = response.CharacterTrait;
            data[$"{prefix}concept"] = response.Concept;
        }
        private void AddPurple5Research(Dictionary<string, string> data, Purple_5.Research research, int count)
        {
            string prefix = $"research_{count}_";
            data[$"{prefix}name"] = research.Name;
            data[$"{prefix}responses_length"] = research.Responses.Length.ToString();
            for (int i = 0; i < research.Responses.Length; i++)
            {
                AddPurple5Response(data, research.Responses[i], count, i);
            }
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);
            var data = new Dictionary<string, string>();

            data["researches_count"] = group.Researches.Length.ToString();
            for (int i = 0; i < group.Researches.Length; i++)
            {
                AddPurple5Research(data, group.Researches[i], i);
            }

            WriteData(data);
        }
        private string[] DeserializePurple5Response(Dictionary<string, string> data, int research_count, int response_count)
        {
            string prefix = $"response_{research_count}_{response_count}_";
            string animal = data[$"{prefix}animal"] == "" ? null : data[$"{prefix}animal"];
            string character_trait = data[$"{prefix}character_trait"] == "" ? null : data[$"{prefix}character_trait"];
            string concept = data[$"{prefix}concept"] == "" ? null : data[$"{prefix}concept"];
            string[] response = new string[3] { animal, character_trait, concept };
            return response;
        }
        private Purple_5.Research DeserializePurple5Research(Dictionary<string, string> data, int count)
        {
            string prefix = $"research_{count}_";
            string name = data[$"{prefix}name"];
            int responses_length = int.Parse(data[$"{prefix}responses_length"]);
            var research = new Purple_5.Research(name);
            for (int i = 0; i < responses_length; i++)
            {
                research.Add(DeserializePurple5Response(data, count, i));
            }
            return research;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var data = GetDataFromFile();
            int researches_count = int.Parse(data["researches_count"]);
            var report = new Purple_5.Report();
            for (int i = 0; i < researches_count; i++)
            {
                report.AddResearch(DeserializePurple5Research(data, i));
            }
            return report;
        }
    }
}
