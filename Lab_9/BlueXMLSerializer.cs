using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension
        {
            get
            {
                return "xml";
            }
        }

        // Blue_1
        public class Blue_1_ResponseDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }

            public int Votes { get; set; }

            public Blue_1_ResponseDTO() { }

            public Blue_1_ResponseDTO(Blue_1.Response response)
            {
                Type = response.GetType().Name;
                Name = response.Name;
                if (response is Blue_1.HumanResponse h)
                    Surname = h.Surname;
                Votes = response.Votes;

            }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            // преобразуем объект participant в DTO, чтобы сериализовать только нужные данные
            Blue_1_ResponseDTO responseDTO = new Blue_1_ResponseDTO(participant);

            // создаем XML-сериализатор, который будет уметь сохранять объекты типа Blue_1_ResponseDTO
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));

            // Открываем поток в указанный файл fileName, и сериализуем туда responseDTO в формате XML.
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(writer, responseDTO);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));


            Blue_1_ResponseDTO responseDTO;

            using (StreamReader reader = new StreamReader(fileName))
            {
                responseDTO = (Blue_1_ResponseDTO)xmlSerializer.Deserialize(reader);
            }


            Blue_1.Response response;

            if (responseDTO.Surname != null)
            {
                response = new Blue_1.HumanResponse(responseDTO.Name, responseDTO.Surname, responseDTO.Votes);
            }
            else
            {
                response = new Blue_1.Response(responseDTO.Name, responseDTO.Votes);
            }

            return response;

        }


        // Blue_2
        public class Blue_2_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }  // Изменили на int[][]

            public Blue_2_ParticipantDTO() { }

            public Blue_2_ParticipantDTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = ConvertToJaggedArray(participant.Marks);
            }

            private int[][] ConvertToJaggedArray(int[,] multiArray)
            {
                if (multiArray == null) return null;

                int rows = multiArray.GetLength(0);
                int cols = multiArray.GetLength(1);
                int[][] jagged = new int[rows][];

                for (int i = 0; i < rows; i++)
                {
                    jagged[i] = new int[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        jagged[i][j] = multiArray[i, j];
                    }
                }
                return jagged;
            }
        }

        public class Blue_2_WaterDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }

            public Blue_2_ParticipantDTO[] Participant { get; set; }

            public Blue_2_WaterDTO() { }
            public Blue_2_WaterDTO(Blue_2.WaterJump waterpart)
            {
                Type = waterpart.GetType().Name;
                Name = waterpart.Name;
                Bank = waterpart.Bank;
                Participant = waterpart.Participants.Select(p => new Blue_2_ParticipantDTO(p)).ToArray();
            }
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {

            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            Blue_2_WaterDTO waterDTO = new Blue_2_WaterDTO(participant);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue_2_WaterDTO));

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(writer, waterDTO);
            }

        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {

            if (String.IsNullOrEmpty(fileName)) return null;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue_2_WaterDTO));


            Blue_2_WaterDTO waterDTO;

            using (StreamReader reader = new StreamReader(fileName))
            {
                waterDTO = (Blue_2_WaterDTO)xmlSerializer.Deserialize(reader);
            }

            Blue_2.WaterJump waterJump;
            if (waterDTO.Type == "WaterJump3m")
                waterJump = new Blue_2.WaterJump3m(waterDTO.Name, waterDTO.Bank);
            else
                waterJump = new Blue_2.WaterJump5m(waterDTO.Name, waterDTO.Bank);


            foreach (Blue_2_ParticipantDTO part in waterDTO.Participant)
            {
                Blue_2.Participant participant = new Blue_2.Participant(part.Name, part.Surname);

                if (part.Marks != null)
                {
                    foreach (var jumpMarks in part.Marks)
                    {
                        participant.Jump(jumpMarks);
                    }
                }

                waterJump.Add(participant);
            }

            return waterJump;
        }



        // Blue_3

        // DTO класс для участника соревнования Blue_3
        public class Blue_3_ParticipantDTO
        {
            public string Type { get; set; }     // Тип участника (например, BasketballPlayer, HockeyPlayer, или базовый Participant)
            public string Name { get; set; }     // Имя участника
            public string Surname { get; set; }  // Фамилия участника

            public int[] Penalties { get; set; } // Массив штрафов

            // Пустой конструктор необходим для XML-сериализации/десериализации
            public Blue_3_ParticipantDTO() { }

            // Конструктор для создания DTO 
            public Blue_3_ParticipantDTO(Blue_3.Participant participant)
            {
                Type = participant.GetType().Name;  
                Name = participant.Name;             
                Surname = participant.Surname;       
                Penalties = participant.Penalties;   
            }
        }

        
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : Blue_3.Participant;
        {
            // приведение переданный объект к типу Blue_3.Participant
            var st = student as Blue_3.Participant;
       
            if (st == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName); 

            // Создаем DTO из участника
            Blue_3_ParticipantDTO participantDTO = new Blue_3_ParticipantDTO(st);

            // Создаем XML сериализатор для DTO участника
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));

            // Записываем сериализованные данные в файл
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(writer, participantDTO);
            }
        }

      
        public override T DeserializeBlue3Participant<T>(string fileName) // where T : Blue_3.Participant;
        {
          
            if (String.IsNullOrEmpty(fileName)) return null;

            // Создаем XML сериализатор для DTO участника
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));

            Blue_3_ParticipantDTO participantDTO;

            // Считываем XML из файла и десериализуем в DTO
            using (StreamReader reader = new StreamReader(fileName))
            {
                participantDTO = (Blue_3_ParticipantDTO)xmlSerializer.Deserialize(reader);
            }

            Blue_3.Participant part;

        
            if (participantDTO.Type == "BasketballPlayer")
            {
                // Создаем объект игрока баскетбола
                part = new Blue_3.BasketballPlayer(participantDTO.Name, participantDTO.Surname);
            }
            else if (participantDTO.Type == "HockeyPlayer")
            {
                // Создаем объект хоккеиста
                part = new Blue_3.HockeyPlayer(participantDTO.Name, participantDTO.Surname);
            }
            else
            {
                // Создаем базового участника, если тип не известен
                part = new Blue_3.Participant(participantDTO.Name, participantDTO.Surname);
            }

            // Восстанавливаем штрафы, вызывая метод PlayMatch для каждого значения
            foreach (var penalti in participantDTO.Penalties)
            {
                part.PlayMatch(penalti);
            }

            // Возвращаем объект нужного типа (приводим к T)
            return (T)(object)part;
        }



        // Blue_4

        public class Blue_4_GroupDTO
        {
            public string Name { get; set; }
            public Blue_4_TeamDTO[] ManTeam { get; set; }
            public Blue_4_TeamDTO[] WomanTeam { get; set; }

            public Blue_4_GroupDTO() { }
            public Blue_4_GroupDTO(Blue_4.Group group)
            {
                Name = group.Name;
                ManTeam = group.ManTeams.Select(m => m == null ? null : new Blue_4_TeamDTO(m)).ToArray();
                WomanTeam = group.WomanTeams.Select(w => w == null ? null : new Blue_4_TeamDTO(w)).ToArray();
            }
        }

        public class Blue_4_TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }

            public Blue_4_TeamDTO() { }
            public Blue_4_TeamDTO(Blue_4.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Scores = team.Scores;
            }
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            Blue_4_GroupDTO waterDTO = new Blue_4_GroupDTO(participant);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue_4_GroupDTO));

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(writer, waterDTO);
            }
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            XmlSerializer serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));
            Blue_4_GroupDTO groupDTO;
            using (StreamReader reader = new StreamReader(fileName))
            {
                groupDTO = (Blue_4_GroupDTO)serializer.Deserialize(reader);
            }

            Blue_4.Group group = new Blue_4.Group(groupDTO.Name);
            foreach (var man in groupDTO.ManTeam)
            {
                if (man == null) continue;
                Blue_4.ManTeam manTeam = new Blue_4.ManTeam(man.Name);
                foreach (int score in man.Scores)
                    manTeam.PlayMatch(score);
                group.Add(manTeam);
            }


            foreach (var woman in groupDTO.WomanTeam)
            {
                if (woman == null) continue;
                Blue_4.WomanTeam womanTeam = new Blue_4.WomanTeam(woman.Name);
                foreach (int score in woman.Scores)
                    womanTeam.PlayMatch(score);
                group.Add(womanTeam);
            }

            return group;

        }



        // Blue_5
        public class Blue_5_SportsmanDTO
        {
            public string Name { get; set; }      // Имя спортсмена
            public string Surname { get; set; }   // Фамилия спортсмена
            public int Place { get; set; }        // Место спортсмена

            // Пустой конструктор нужен для сериализации/десериализации
            public Blue_5_SportsmanDTO() { }

            // Конструктор для создания DTO из объекта модели Sportsman
            public Blue_5_SportsmanDTO(Blue_5.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }
        }

        // DTO для команды
        public class Blue_5_TeamDTO
        {
            public string Type { get; set; }             
            public string Name { get; set; }            
            public Blue_5_SportsmanDTO[] Sportsman { get; set; } 

            // Пустой конструктор для сериализации/десериализации
            public Blue_5_TeamDTO() { }

            // Конструктор для создания DTO из объекта модели Team
            public Blue_5_TeamDTO(Blue_5.Team team)
            {
                Type = team.GetType().Name; 
                Name = team.Name;          
                                            
                Sportsman = team.Sportsmen.Select(p => p == null ? null : new Blue_5_SportsmanDTO(p)).ToArray();
            }
        }

        // Метод сериализации команды в XML-файл
        public override void SerializeBlue5Team<T>(T group, string fileName) // where T : Blue_5.Team;
        {
            
            if (group == null || group as Blue_5.Team == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName); // Вероятно, устанавливаем путь для записи файла

            // Создаем DTO из объекта группы
            Blue_5_TeamDTO teamDTO = new Blue_5_TeamDTO(group);

            // Создаем XML сериализатор для типа Blue_5_TeamDTO
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue_5_TeamDTO));

            // Используем StreamWriter для записи сериализованных данных в файл
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(writer, teamDTO);
            }
        }

        // Метод десериализации XML-файла обратно в объект команды
        public override T DeserializeBlue5Team<T>(string fileName) // where T : Blue_5.Team;
        {
           
            if (string.IsNullOrEmpty(fileName)) return null;

            // Создаем XML сериализатор для типа Blue_5_TeamDTO
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blue_5_TeamDTO));
            Blue_5_TeamDTO teamDTO;

            // Считываем данные из файла и десериализуем в DTO
            using (StreamReader reader = new StreamReader(fileName))
            {
                teamDTO = (Blue_5_TeamDTO)xmlSerializer.Deserialize(reader);
            }

            // Создаем объект команды нужного типа в зависимости от строки Type
            Blue_5.Team team;
            if (teamDTO.Type == "WomanTeam")
                team = new Blue_5.WomanTeam(teamDTO.Name);
            else
                team = new Blue_5.ManTeam(teamDTO.Name);

            // Проходим по массиву спортсменов в DTO, создаем объекты Sportsman и добавляем в команду
            foreach (var sp in teamDTO.Sportsman)
            {
                if (sp == null) continue;
                Blue_5.Sportsman sportsman = new Blue_5.Sportsman(sp.Name, sp.Surname);
                sportsman.SetPlace(sp.Place);  
                team.Add(sportsman);
            }

          
            return (T)(object)team;
        }

    }
}
