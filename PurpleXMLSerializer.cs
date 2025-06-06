using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using static Lab_7.Purple_1;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        private T LoadXml<T>()
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
            using (FileStream fs = File.OpenRead(FilePath))
                return (T)xmlFormat.Deserialize(fs);
        }

        #region Purple_1
        [XmlRoot("ParticipantData")]
        public class Purple1ParticipantModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [XmlArrayItem("Value")]
            public double[] Coefficients { get; set; }
            [XmlArrayItem("Score")]
            public int[] Scores { get; set; }
        }

        [XmlRoot("JudgeData")]
        public class Purple1JudgeModel
        {
            public string FullName { get; set; }
            [XmlArrayItem("Mark")]
            public int[] Evaluations { get; set; }
        }

        [XmlRoot("CompetitionData")]
        public class Purple1CompetitionModel
        {
            [XmlArray("Judges")]
            [XmlArrayItem("Judge")]
            public Purple1JudgeModel[] Officials { get; set; }

            [XmlArray("Contestants")]
            [XmlArrayItem("Contestant")]
            public Purple1ParticipantModel[] Players { get; set; }
        }

        public override void SerializePurple1<T>(T data, string filePath)
        {
            SelectFile(filePath);
            if (data is Participant participantData)
            {
                var model = new Purple1ParticipantModel
                {
                    FirstName = participantData.Name,
                    LastName = participantData.Surname,
                    Coefficients = participantData.Coefs,
                    Scores = participantData.Marks.Cast<int>().ToArray()
                };
                SaveModel(model);
            }
            else if (data is Judge judgeData)
            {
                var model = new Purple1JudgeModel
                {
                    FullName = judgeData.Name,
                    Evaluations = judgeData.Marks
                };
                SaveModel(model);
            }
            else if (data is Competition competitionData)
            {
                var model = new Purple1CompetitionModel
                {
                    Officials = competitionData.Judges.Select(j => new Purple1JudgeModel
                    {
                        FullName = j.Name,
                        Evaluations = j.Marks
                    }).ToArray(),
                    Players = competitionData.Participants.Select(p => new Purple1ParticipantModel
                    {
                        FirstName = p.Name,
                        LastName = p.Surname,
                        Coefficients = p.Coefs,
                        Scores = p.Marks.Cast<int>().ToArray()
                    }).ToArray()
                };
                SaveModel(model);
            }
        }

        private void SaveModel<T>(T model)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
            using (XmlWriter writer = XmlWriter.Create(FilePath))
                xmlFormat.Serialize(writer, model);
        }

        private Participant ConvertParticipantModel(Purple1ParticipantModel model)
        {
            var athlete = new Participant(model.FirstName, model.LastName);
            double[] coefficients = model.Coefficients;
            int[] scores = model.Scores;
            int[,] marksGrid = new int[4, 7];

            int position = 0;
            for (int round = 0; round < 4; round++)
            {
                for (int judge = 0; judge < 7; judge++)
                {
                    marksGrid[round, judge] = scores[position++];
                }
            }

            athlete.SetCriterias(coefficients);
            for (int round = 0; round < 4; round++)
            {
                int[] roundScores = new int[7];
                for (int judge = 0; judge < 7; judge++)
                {
                    roundScores[judge] = marksGrid[round, judge];
                }
                athlete.Jump(roundScores);
            }
            return athlete;
        }

        private Judge ConvertJudgeModel(Purple1JudgeModel model)
        {
            return new Judge(model.FullName, model.Evaluations);
        }

        private Competition ConvertCompetitionModel(Purple1CompetitionModel model)
        {
            Judge[] officials = new Judge[model.Officials.Length];
            for (int i = 0; i < model.Officials.Length; i++)
            {
                officials[i] = ConvertJudgeModel(model.Officials[i]);
            }

            Participant[] contestants = new Participant[model.Players.Length];
            for (int i = 0; i < model.Players.Length; i++)
            {
                contestants[i] = ConvertParticipantModel(model.Players[i]);
            }

            Competition eventData = new Competition(officials);
            eventData.Add(contestants);
            return eventData;
        }

        public override T DeserializePurple1<T>(string filePath)
        {
            SelectFile(filePath);
            if (typeof(T) == typeof(Competition))
            {
                return (T)(object)ConvertCompetitionModel(LoadXml<Purple1CompetitionModel>());
            }
            else if (typeof(T) == typeof(Judge))
            {
                return (T)(object)ConvertJudgeModel(LoadXml<Purple1JudgeModel>());
            }
            else if (typeof(T) == typeof(Participant))
            {
                return (T)(object)ConvertParticipantModel(LoadXml<Purple1ParticipantModel>());
            }
            return default(T);
        }
        #endregion

        #region Purple_2
        [XmlRoot("SkiJumpingEvent")]
        public class Purple2EventModel
        {
            public string EventName { get; set; }
            public int TargetDistance { get; set; }
            [XmlArray("Athletes")]
            [XmlArrayItem("Athlete")]
            public Purple2AthleteModel[] Jumpers { get; set; }
        }

        [XmlType("Athlete")]
        public class Purple2AthleteModel
        {
            public string GivenName { get; set; }
            public string FamilyName { get; set; }
            public int JumpLength { get; set; }
            [XmlArrayItem("Score")]
            public int[] Evaluations { get; set; }
        }

        public override void SerializePurple2SkiJumping<T>(T eventData, string filePath)
        {
            SelectFile(filePath);
            if (eventData is Purple_2.SkiJumping jumpData)
            {
                var model = new Purple2EventModel
                {
                    EventName = jumpData.Name,
                    TargetDistance = jumpData.Standard,
                    Jumpers = jumpData.Participants.Select(j => new Purple2AthleteModel
                    {
                        GivenName = j.Name,
                        FamilyName = j.Surname,
                        JumpLength = j.Distance,
                        Evaluations = j.Marks
                    }).ToArray()
                };
                SaveModel(model);
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string filePath)
        {
            SelectFile(filePath);
            var model = LoadXml<Purple2EventModel>();
            return (T)(object)ReconstructSkiJumping(model);
        }

        private Purple_2.SkiJumping ReconstructSkiJumping(Purple2EventModel model)
        {
            Purple_2.SkiJumping eventData;
            if (model.TargetDistance == 100)
                eventData = new Purple_2.JuniorSkiJumping();
            else if (model.TargetDistance == 150)
                eventData = new Purple_2.ProSkiJumping();
            else
                return null;

            Purple_2.Participant[] athletes = new Purple_2.Participant[model.Jumpers.Length];
            for (int i = 0; i < model.Jumpers.Length; i++)
            {
                athletes[i] = CreateSkiJumper(model.Jumpers[i], model.TargetDistance);
            }
            eventData.Add(athletes);
            return eventData;
        }

        private Purple_2.Participant CreateSkiJumper(Purple2AthleteModel model, int target)
        {
            var jumper = new Purple_2.Participant(model.GivenName, model.FamilyName);
            jumper.Jump(model.JumpLength, model.Evaluations, target);
            return jumper;
        }
        #endregion

        #region Purple_3
        [XmlRoot("SkatingEvent")]
        public class Purple3EventModel
        {
            [XmlArrayItem("MoodValue")]
            public double[] Atmosphere { get; set; }
            public string EventType { get; set; }
            [XmlArray("Performers")]
            [XmlArrayItem("Performer")]
            public Purple3PerformerModel[] Skaters { get; set; }
        }

        [XmlType("Performer")]
        public class Purple3PerformerModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [XmlArrayItem("Evaluation")]
            public double[] Scores { get; set; }
            [XmlArrayItem("Position")]
            public int[] Rankings { get; set; }
        }

        public override void SerializePurple3Skating<T>(T eventData, string filePath)
        {
            SelectFile(filePath);
            if (eventData is Purple_3.Skating skatingData)
            {
                var model = new Purple3EventModel
                {
                    Atmosphere = skatingData.Moods,
                    EventType = skatingData.GetType().Name,
                    Skaters = skatingData.Participants.Select(s => new Purple3PerformerModel
                    {
                        FirstName = s.Name,
                        LastName = s.Surname,
                        Scores = s.Marks,
                        Rankings = s.Places
                    }).ToArray()
                };
                SaveModel(model);
            }
        }

        public override T DeserializePurple3Skating<T>(string filePath)
        {
            SelectFile(filePath);
            var model = LoadXml<Purple3EventModel>();
            return (T)(object)ReconstructSkating(model);
        }

        private Purple_3.Skating ReconstructSkating(Purple3EventModel model)
        {
            Purple_3.Skating eventData;
            if (model.EventType.Contains("FigureSkating"))
                eventData = new Purple_3.FigureSkating(model.Atmosphere, false);
            else
                eventData = new Purple_3.IceSkating(model.Atmosphere, false);

            Purple_3.Participant[] skaters = new Purple_3.Participant[model.Skaters.Length];
            for (int i = 0; i < model.Skaters.Length; i++)
            {
                skaters[i] = CreateSkater(model.Skaters[i]);
            }
            eventData.Add(skaters);
            return eventData;
        }

        private Purple_3.Participant CreateSkater(Purple3PerformerModel model)
        {
            var skater = new Purple_3.Participant(model.FirstName, model.LastName);
            for (int i = 0; i < model.Scores.Length; i++)
            {
                skater.Evaluate(model.Scores[i]);
            }
            return skater;
        }
        #endregion

        #region Purple_4
        [XmlRoot("GroupData")]
        public class Purple4GroupModel
        {
            public string TeamName { get; set; }
            [XmlArray("Members")]
            [XmlArrayItem("Member")]
            public Purple4MemberModel[] TeamMembers { get; set; }
        }

        [XmlType("Member")]
        public class Purple4MemberModel
        {
            public string GivenName { get; set; }
            public string FamilyName { get; set; }
            public double ResultTime { get; set; }
        }

        public override void SerializePurple4Group(Purple_4.Group groupData, string filePath)
        {
            SelectFile(filePath);
            var model = new Purple4GroupModel
            {
                TeamName = groupData.Name,
                TeamMembers = groupData.Sportsmen.Select(m => new Purple4MemberModel
                {
                    GivenName = m.Name,
                    FamilyName = m.Surname,
                    ResultTime = m.Time
                }).ToArray()
            };
            SaveModel(model);
        }

        public override Purple_4.Group DeserializePurple4Group(string filePath)
        {
            SelectFile(filePath);
            var model = LoadXml<Purple4GroupModel>();
            return ReconstructGroup(model);
        }

        private Purple_4.Group ReconstructGroup(Purple4GroupModel model)
        {
            Purple_4.Sportsman[] members = new Purple_4.Sportsman[model.TeamMembers.Length];
            for (int i = 0; i < model.TeamMembers.Length; i++)
            {
                members[i] = CreateSportsman(model.TeamMembers[i]);
            }

            Purple_4.Group team = new Purple_4.Group(model.TeamName);
            team.Add(members);
            return team;
        }

        private Purple_4.Sportsman CreateSportsman(Purple4MemberModel model)
        {
            var athlete = new Purple_4.Sportsman(model.GivenName, model.FamilyName);
            athlete.Run(model.ResultTime);
            return athlete;
        }
        #endregion

        #region Purple_5
        [XmlRoot("ResearchReport")]
        public class Purple5ReportModel
        {
            [XmlArray("Studies")]
            [XmlArrayItem("Study")]
            public Purple5StudyModel[] Investigations { get; set; }
        }

        [XmlType("Study")]
        public class Purple5StudyModel
        {
            public string StudyName { get; set; }
            [XmlArray("Responses")]
            [XmlArrayItem("Answer")]
            public Purple5ResponseModel[] Feedback { get; set; }
        }

        [XmlType("Answer")]
        public class Purple5ResponseModel
        {
            public string AnimalResponse { get; set; }
            public string TraitResponse { get; set; }
            public string ConceptResponse { get; set; }
        }

        public override void SerializePurple5Report(Purple_5.Report reportData, string filePath)
        {
            SelectFile(filePath);
            var model = new Purple5ReportModel
            {
                Investigations = reportData.Researches.Select(r => new Purple5StudyModel
                {
                    StudyName = r.Name,
                    Feedback = r.Responses.Select(res => new Purple5ResponseModel
                    {
                        AnimalResponse = res.Animal,
                        TraitResponse = res.CharacterTrait,
                        ConceptResponse = res.Concept
                    }).ToArray()
                }).ToArray()
            };
            SaveModel(model);
        }

        public override Purple_5.Report DeserializePurple5Report(string filePath)
        {
            SelectFile(filePath);
            var model = LoadXml<Purple5ReportModel>();
            return ReconstructReport(model);
        }

        private Purple_5.Report ReconstructReport(Purple5ReportModel model)
        {
            Purple_5.Report report = new Purple_5.Report();
            for (int i = 0; i < model.Investigations.Length; i++)
            {
                report.AddResearch(ProcessStudy(model.Investigations[i]));
            }
            return report;
        }

        private Purple_5.Research ProcessStudy(Purple5StudyModel model)
        {
            Purple_5.Research study = new Purple_5.Research(model.StudyName);
            for (int i = 0; i < model.Feedback.Length; i++)
            {
                study.Add(ExtractResponse(model.Feedback[i]));
            }
            return study;
        }

        private string[] ExtractResponse(Purple5ResponseModel model)
        {
            return new string[] {
                string.IsNullOrEmpty(model.AnimalResponse) ? null : model.AnimalResponse,
                string.IsNullOrEmpty(model.TraitResponse) ? null : model.TraitResponse,
                string.IsNullOrEmpty(model.ConceptResponse) ? null : model.ConceptResponse
            };
        }
        #endregion
    }
}