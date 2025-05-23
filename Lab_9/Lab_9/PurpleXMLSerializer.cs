using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_1;
using static Lab_7.Purple_2;
using static Lab_7.Purple_3;
using static Lab_7.Purple_4;
using static Lab_7.Purple_5;
using System.Xml.Serialization;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";
        public class Purple1ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double TotalScore { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }

            public Purple1ParticipantDTO() { }
            public Purple1ParticipantDTO(Purple_1.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                TotalScore = participant.TotalScore;
                Coefs = (double[])participant.Coefs.Clone();
                Marks = ConvertToJaggedArray(participant.Marks);
            }
        }
        public class Purple1JudgeDTO
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
            public Purple1JudgeDTO() { }
            public Purple1JudgeDTO(Purple_1.Judge judge)
            {
                Name = judge.Name;
                Marks = (int[])judge.Marks.Clone();
            }
        }
        public class Purple1CompetitionDTO
        {
            public Purple1JudgeDTO[] Judges { get; set; }
            public Purple1ParticipantDTO[] Participants { get; set; }

            public Purple1CompetitionDTO() { }
            public Purple1CompetitionDTO(Purple_1.Competition competition)
            {
                Purple_1.Judge[] judges = competition.Judges;
                Purple_1.Participant[] participants = competition.Participants;

                Judges = new Purple1JudgeDTO[judges.Length];
                Participants = new Purple1ParticipantDTO[participants.Length];

                for (int i = 0; i < judges.Length; i++)
                    Judges[i] = new Purple1JudgeDTO(judges[i]);

                for (int i = 0; i < participants.Length; i++)
                    Participants[i] = new Purple1ParticipantDTO(participants[i]);
            }
        }
        private void SerializeObject<T>(T obj, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StreamWriter writer = new StreamWriter($"{Path.Combine(FolderPath, fileName)}.{Extension}"))
            {
                serializer.Serialize(writer, obj);
            }
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj is Purple_1.Participant participant) SerializeObject(new Purple1ParticipantDTO(participant), fileName);
            else if (obj is Purple_1.Judge judge) SerializeObject(new Purple1JudgeDTO(judge), fileName);
            else if (obj is Purple_1.Competition competition) SerializeObject(new Purple1CompetitionDTO(competition), fileName);
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SerializeObject(new SkiJumpingDTO(jumping), fileName);
        }

        public struct Purple2ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }

            public Purple2ParticipantDTO() { }
            public Purple2ParticipantDTO(Purple_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Distance = participant.Distance;
                Marks = (int[])participant.Marks.Clone();
            }
        }
        public class SkiJumpingDTO
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public Purple2ParticipantDTO[] Participants { get; set; }
            public string Type { get; set; }

            public SkiJumpingDTO() { }
            public SkiJumpingDTO(Purple_2.SkiJumping skiJumping)
            {
                Name = skiJumping.Name;
                Standard = skiJumping.Standard;
                Participants = new Purple2ParticipantDTO[skiJumping.Participants.Length];
                Type = skiJumping.GetType().AssemblyQualifiedName;

                for (int i = 0; i < skiJumping.Participants.Length; i++)
                    Participants[i] = new Purple2ParticipantDTO(skiJumping.Participants[i]);
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SerializeObject(new SkatingDTO(skating), fileName);
        }
        public struct Purple3ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }

            public Purple3ParticipantDTO() { }
            public Purple3ParticipantDTO(Purple_3.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = (double[])participant.Marks.Clone();
                Places = (int[])participant.Places.Clone();
            }
        }
        public class SkatingDTO
        {
            public Purple3ParticipantDTO[] Participants { get; set; }
            public double[] Moods { get; set; }
            public string Type { get; set; }

            public SkatingDTO() { }
            public SkatingDTO(Skating skating)
            {
                Participants = new Purple3ParticipantDTO[skating.Participants.Length];
                Type = skating.GetType().AssemblyQualifiedName;
                Moods = (double[])skating.Moods.Clone();

                for (int i = 0; i < skating.Participants.Length; i++)
                    Participants[i] = new Purple3ParticipantDTO(skating.Participants[i]);
            }
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SerializeObject(new GroupDTO(group), fileName);
        }
        public class SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
            public string Type { get; set; }

            public SportsmanDTO() { }
            public SportsmanDTO(Purple_4.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Time = sportsman.Time;
                Type = sportsman.GetType().AssemblyQualifiedName;
            }
        }
        public class GroupDTO
        {
            public string Name { get; set; }
            public SportsmanDTO[] Sportsmen { get; set; }

            public GroupDTO() { }
            public GroupDTO(Group group)
            {
                Name = group.Name;
                Sportsman[] sportsmen = group.Sportsmen;
                Sportsmen = new SportsmanDTO[sportsmen.Length];
                for (int i = 0; i < sportsmen.Length; i++)
                    Sportsmen[i] = new SportsmanDTO(sportsmen[i]);
            }
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SerializeObject(new ReportDTO(report), fileName);
        }
        public struct ResponseDTO
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }

            public ResponseDTO() { }
            public ResponseDTO(Response response)
            {
                Animal = response.Animal;
                CharacterTrait = response.CharacterTrait;
                Concept = response.Concept;
            }
        }
        public struct ResearchDTO
        {
            public string Name { get; set; }
            public ResponseDTO[] Responses { get; set; }
            public ResearchDTO() { }
            public ResearchDTO(Research research)
            {
                Name = research.Name;
                var responses = research.Responses;
                Responses = new ResponseDTO[responses.Length];

                for (int i = 0; i < responses.Length; i++)
                    Responses[i] = new ResponseDTO(responses[i]);
            }
        }
        public class ReportDTO
        {
            public ResearchDTO[] Researches { get; set; }

            public ReportDTO() { }
            public ReportDTO(Report report)
            {
                var researches = report.Researches;
                Researches = new ResearchDTO[researches.Length];

                for (int i = 0; i < researches.Length; i++)
                    Researches[i] = new ResearchDTO(researches[i]);
            }
        }

        private T DeserializeObject<T>(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T deserializedPerson;

            using (StreamReader reader = new StreamReader($"{Path.Combine(FolderPath, fileName)}.{Extension}"))
            {
                deserializedPerson = (T)serializer.Deserialize(reader);
            }
            return deserializedPerson;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var objDTO = DeserializeObject<Purple1ParticipantDTO>(fileName);
                return DeserializePurple1Participant(objDTO) as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var objDTO = DeserializeObject<Purple1JudgeDTO>(fileName);
                return DeserializePurple1Judge(objDTO) as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var objDTO = DeserializeObject<Purple1CompetitionDTO>(fileName);
                return DeserializePurple1Competition(objDTO) as T;
            }
            return null;
        }
        private Purple_1.Participant DeserializePurple1Participant(Purple1ParticipantDTO participant)
        {
            if (participant == null) return null;
            string name = participant.Name;
            string surname = participant.Surname;
            double[] coefs = (double[])participant.Coefs.Clone();
            int[,] marks = ConvertToRectangularArray(participant.Marks);

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
        private Purple_1.Judge DeserializePurple1Judge(Purple1JudgeDTO judge)
        {
            if (judge == null) return null;
            string name = judge.Name;
            int[] marks = (int[])judge.Marks.Clone();

            var newJudge = new Purple_1.Judge(name, marks);
            return newJudge;
        }
        private Purple_1.Competition DeserializePurple1Competition(Purple1CompetitionDTO competition)
        {
            if (competition == null) return null;
            var participantsDTO = competition.Participants;
            var judgesDTO = competition.Judges;
            var judges = new Purple_1.Judge[judgesDTO.Length];

            for (int i = 0; i < judgesDTO.Length; i++)
                judges[i] = DeserializePurple1Judge(judgesDTO[i]);

            var newCompetition = new Competition(judges);

            foreach (var participantDTO in participantsDTO)
                newCompetition.Add(DeserializePurple1Participant(participantDTO));

            return newCompetition;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            var objDTO = DeserializeObject<SkiJumpingDTO>(fileName);
            dynamic resultObj = Activator.CreateInstance(Type.GetType(objDTO.Type));

            var targetDistance = objDTO.Standard;
            var participants = objDTO.Participants;

            foreach (var participant in participants)
            {
                var newParticipant = DeserializePurple2Participant(participant, targetDistance);
                resultObj.Add(newParticipant);
            }

            return resultObj as T;
        }
        private Purple_2.Participant DeserializePurple2Participant(Purple2ParticipantDTO participant, int target)
        {
            string name = participant.Name;
            string surname = participant.Surname;
            int distance = participant.Distance;
            int[] marks = (int[])participant.Marks.Clone();

            var newParticipant = new Purple_2.Participant(name, surname);
            newParticipant.Jump(distance, marks, target);

            return newParticipant;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            var skatingDTO = DeserializeObject<SkatingDTO>(fileName);
            var participants = skatingDTO.Participants;
            double[] moods = (double[])skatingDTO.Moods.Clone();

            var type = Type.GetType(skatingDTO.Type);

            object[] constructorArgs = new object[] { moods, false };
            dynamic resultObj = Activator.CreateInstance(type, constructorArgs);

            var newParticipants = new Purple_3.Participant[participants.Length];
            for (int i = 0; i < participants.Length; i++)
            {
                newParticipants[i] = DeserializePurple3Participant(participants[i]);
            }

            resultObj.Add(newParticipants);
            Purple_3.Participant.SetPlaces(resultObj.Participants);

            return resultObj as T;
        }
        private Purple_3.Participant DeserializePurple3Participant(Purple3ParticipantDTO participant)
        {
            string name = participant.Name;
            string surname = participant.Surname;
            double[] marks = (double[])participant.Marks.Clone();
            int[] places = (int[])participant.Places.Clone();

            var newParticipant = new Purple_3.Participant(name, surname);
            for (int i = 0; i < marks.Length; i++)
                newParticipant.Evaluate(marks[i]);

            return newParticipant;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            var groupDTO = DeserializeObject<GroupDTO>(fileName);
            return DeserializeGroup(groupDTO);
        }
        private dynamic DeserializeSportsman(SportsmanDTO sportsmanDTO)
        {
            string name = sportsmanDTO.Name;
            string surname = sportsmanDTO.Surname;
            double time = sportsmanDTO.Time;

            string[] arguments = { name, surname };
            dynamic sportsman = Activator.CreateInstance(Type.GetType(sportsmanDTO.Type), arguments);
            sportsman.Run(time);

            return sportsman;
        }
        private Group DeserializeGroup(GroupDTO groupDTO)
        {
            string name = groupDTO.Name;
            SportsmanDTO[] sportsmenDTO = groupDTO.Sportsmen;

            Group group = new Group(name);

            for (int i = 0; i < sportsmenDTO.Length; i++)
                group.Add(DeserializeSportsman(sportsmenDTO[i]));

            return group;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            var reportDTO = DeserializeObject<ReportDTO>(fileName);
            return DeserializeReport(reportDTO);
        }
        private string[] DeserializeResponse(ResponseDTO responseDTO)
        {
            return new string[] { responseDTO.Animal, responseDTO.Concept, responseDTO.CharacterTrait };
        }
        private Research DeserializeResearch(ResearchDTO researchDTO)
        {
            var research = new Research(researchDTO.Name);
            var responsesDTO = researchDTO.Responses;

            for (int i = 0; i < responsesDTO.Length;i++)
                research.Add(DeserializeResponse(responsesDTO[i]));

            return research;
        }
        private Report DeserializeReport(ReportDTO reportDTO)
        {
            var researchesDTO = reportDTO.Researches;
            var report = new Report();

            for (int i = 0; i < researchesDTO.Length;i++) 
                report.AddResearch(DeserializeResearch(researchesDTO[i]));

            return report;
        }
        private static int[][] ConvertToJaggedArray(int[,] rectangularArray)
        {
            if (rectangularArray == null) return null;

            int rows = rectangularArray.GetLength(0);
            int cols = rectangularArray.GetLength(1);
            int[][] jaggedArray = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = rectangularArray[i, j];
                }
            }

            return jaggedArray;
        }

        private static int[,] ConvertToRectangularArray(int[][] jaggedArray)
        {
            if (jaggedArray == null || jaggedArray.Length == 0)
                return new int[0, 0];

            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;
            int[,] rectangularArray = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rectangularArray[i, j] = jaggedArray[i][j];
                }
            }

            return rectangularArray;
        }
    }
}