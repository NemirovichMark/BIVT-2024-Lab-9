using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using static Lab_7.Blue_1;
using static Lab_7.Blue_2;
using static Lab_7.Blue_5;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";
        //Реализовать абстрактные методы класса-родителя
        //так, чтобы они сериализовывали/десериализовывали
        //объекты в формате json.
        //При сериализации сохранять только публичные
        //нестатические свойства объекта! При
        //десериализации использовать имеющийся в
        //классе конструктор и методы для заполнения
        //данных объекта аналогично созданию нового
        //объекта.Значения свойств десериализованного
        //объекта должны полностью совпадать со
        //значениями свойств базового объекта.

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            var temp = new
            {
                ResponseType = participant.GetType().Name, //Response or HumanResponse
                participant.Name,
                participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname
                //Будет null, если не HumanResponse
            };
            string json = JsonConvert.SerializeObject(temp);

            File.WriteAllText(fileName, json);

        }
        private int[][] ConvertToJaggedArray(int[,] array)
        {
            if (array == null || array.Length == 0) return null;

            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            int[][] jaggedArray = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    jaggedArray[i][j] = array[i, j];
            }
            return jaggedArray;
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            var temp = new
            {
                ParticipantType = participant.GetType().Name,  //3m or 5m
                participant.Name,
                participant.Bank,
                Participants = participant.Participants.Select(p => new
                {
                    p.Name,
                    p.Surname,
                    Marks = ConvertToJaggedArray(p.Marks) //int[][]
                }).ToArray()
            };
            string json = JsonConvert.SerializeObject(temp);
            File.WriteAllText(fileName, json);
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName) // where T : Blue_3.Participant
        {
            var temp = new
            {
                ParticipantType = student.GetType().Name,
                student.Name,
                student.Surname,
                student.Penalties //int[]
            };
            string json = JsonConvert.SerializeObject(temp);
            File.WriteAllText(fileName, json);
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            var temp = new
            {
                GroupType = participant.GetType().Name,
                participant.Name,
                womanParticipant = participant.WomanTeams.Where(t => t != null).Select(p => new  //null reference exception
                {
                    TeamType = p.GetType().Name,
                    p.Name,
                    p.Scores //int[]
                }).ToArray(),
                manParticipant = participant.ManTeams.Where(t => t != null).Select(p => new
                {
                    TeamType = p.GetType().Name,
                    p.Name,
                    p.Scores
                })
            };
            string json = JsonConvert.SerializeObject(temp);
            File.WriteAllText(fileName, json);
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) // where T : Blue_5.Team
        {
            var temp = new
            {
                TeamType = group.GetType().Name, //man or woman
                group.Name,
                sportsman = group.Sportsmen.Where(t => t != null).Select(p => new
                {
                    p.Name,
                    p.Surname,
                    p.Place  //int
                }).ToArray()
            };
            string json = JsonConvert.SerializeObject(temp);
            File.WriteAllText(fileName, json);
        }











        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            string content = File.ReadAllText(fileName);
            var contentToJson = JsonConvert.DeserializeObject<dynamic>(content);

            if ((string)contentToJson.ResponseType == "HumanResponse")
            {
                return new Blue_1.HumanResponse(
                    (string)contentToJson.Name,
                    (string)contentToJson.Surname,
                    (int)contentToJson.Votes
                );
            }
            else // Response
            {
                return new Blue_1.Response(
                    (string)contentToJson.Name,
                    (int)contentToJson.Votes
                );
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            string content = File.ReadAllText(fileName);
            var contentToJson = JsonConvert.DeserializeObject<dynamic>(content);

            Blue_2.WaterJump waterJump;
            if ((string)contentToJson.ParticipantType == "WaterJump3m")
                waterJump = new Blue_2.WaterJump3m((string)contentToJson.Name, (int)contentToJson.Bank);
            else
                waterJump = new Blue_2.WaterJump5m((string)contentToJson.Name, (int)contentToJson.Bank); //не работает без (string)

            //участники
            foreach (dynamic participantData in contentToJson.Participants)
            {
                if (participantData == null) continue;
                var participant = new Blue_2.Participant(
                    (string)participantData.Name,
                    (string)participantData.Surname
                );
                //еще оценки
                // jagged into [,] but not really
                int[][] marks = participantData.Marks.ToObject<int[][]>();
                //ToObject<> - специальный метод Newtonsoft.Json для преобразования JSON-данных в нужный тип

                for (int i = 0; i < 2; i++)
                {
                    if (marks[i].Length == 5)
                        participant.Jump(marks[i]);
                }
                waterJump.Add(participant);
            }
            return waterJump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName) //where T : Blue_3.Participant
        {
            string content = File.ReadAllText(fileName);
            var contentToJson = JsonConvert.DeserializeObject<dynamic>(content);

            Blue_3.Participant participant;
            if ((string)contentToJson.ParticipantType == "HockeyPlayer")
                participant = new Blue_3.HockeyPlayer((string)contentToJson.Name, (string)contentToJson.Surname);
            else if ((string)contentToJson.ParticipantType == "BasketballPlayer")
                participant = new Blue_3.BasketballPlayer((string)contentToJson.Name, (string)contentToJson.Surname);
            else
                participant = new Blue_3.Participant((string)contentToJson.Name, (string)contentToJson.Surname);


            int[] penalty = contentToJson.Penalties.ToObject<int[]>();
            foreach (var time in penalty)
                participant.PlayMatch(time);
            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            string content = File.ReadAllText(fileName);
            var contentToJson = JsonConvert.DeserializeObject<dynamic>(content);

            Blue_4.Group group = new Blue_4.Group((string)contentToJson.Name);
            //божественные женские команды
            foreach (dynamic woman in contentToJson.womanParticipant)
            {
                var womanGroup = new Blue_4.WomanTeam((string)woman.Name);
                int[] scores = woman.Scores.ToObject<int[]>();
                foreach (var score in scores)
                {
                    womanGroup.PlayMatch(score);
                }
                group.Add(womanGroup);
            }
            //man
            foreach (dynamic man in contentToJson.manParticipant)
            {
                var manGroup = new Blue_4.ManTeam((string)man.Name);
                int[] scores = man.Scores.ToObject<int[]>();
                foreach (int score in scores)
                {
                    manGroup.PlayMatch(score);
                }
                group.Add(manGroup);
            }
            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName) // where T : Blue_5.Team
        {
            string content = File.ReadAllText(fileName);
            var contentToJson = JsonConvert.DeserializeObject<dynamic>(content);

            Blue_5.Team team;
            if ((string)contentToJson.TeamType == "ManTeam")
                team = new Blue_5.ManTeam((string)contentToJson.Name);
            else //woman
                team = new Blue_5.WomanTeam((string)contentToJson.Name);

            //sport
            foreach (dynamic player in contentToJson.sportsman)
            {
                var sportsman = new Blue_5.Sportsman((string)player.Name, (string)player.Surname);
                
                sportsman.SetPlace((int)player.Place);
                team.Add(sportsman);
            }

            return (T)team;
            //Мы явно проверили тип через typeof(T)
            //и создали соответствующий экземпляр (ManTeam или WomanTeam)
            //Компилятор знает, что team точно является подтипом T
        }


    }
}
