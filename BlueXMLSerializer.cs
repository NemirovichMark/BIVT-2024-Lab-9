using Lab_7;
using System.Xml.Serialization;
using static Lab_9.SerializeObject;


namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        
        public override string Extension => "xml";

        XmlSerializer XML_Blue_1 = new XmlSerializer(typeof(ResponseSerialize)); // создаем экземпляр класса, передаем в него тип
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_1.Serialize(fs, new ResponseSerialize(participant));
            }
        }


        XmlSerializer XML_Blue_2 = new XmlSerializer(typeof(SerializeWaterJump));
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_2.Serialize(fs, new SerializeWaterJump(participant));
            }
        }

        XmlSerializer XML_Blue_3 = new XmlSerializer(typeof(OtherPart));
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
           
            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_3.Serialize(fs, new OtherPart(student));
            }
        }

        XmlSerializer XML_Blue_4 = new XmlSerializer(typeof(SerializeGroup));
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_4.Serialize(fs, new SerializeGroup(participant));
            }
        }

        XmlSerializer XML_Blue_5 = new XmlSerializer(typeof(SerializeOtherTeam));
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_5.Serialize(fs, new SerializeOtherTeam(group));
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);

            ResponseSerialize? participant;
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                participant = XML_Blue_1.Deserialize(fs) as ResponseSerialize;
            }

            if (participant == null) return null;
            if (participant.Name == null) return null;
            if (participant.Surname == null)
                return new Blue_1.Response(participant.Name, participant.Votes);

            return new Blue_1.HumanResponse(participant.Name, participant.Surname, participant.Votes);
        }

        public override Blue_2.WaterJump? DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);

            SerializeWaterJump? participant;
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                participant = XML_Blue_2.Deserialize(fs) as SerializeWaterJump;
            }

            if (participant == null) return null;

            var answer = GetWaterJump(participant);
            foreach (var p in participant.Participants)
            {
                var jumper = new Blue_2.Participant(p.Name, p.Surname);
                jumper.Jump(p.FirstJump);
                jumper.Jump(p.SecondJump);
                answer.Add(jumper);
            }
            return answer;
        }
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);

            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                var student = XML_Blue_3.Deserialize(fs) as OtherPart;

                if (student == null) return default;

                var player = (T)GetPlayer(student);
                foreach (var penalty in student.Penalties)
                {
                    player.PlayMatch(penalty);
                }

                return player;
            }
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);

            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                var sgroup = XML_Blue_4.Deserialize(fs) as SerializeGroup;

                if (sgroup == null) return null;

                var group = new Blue_4.Group(sgroup.Name);
                foreach (var t in sgroup.Teams)
                {
                    var team = GetTeami(t);
                    if (team == null) continue;

                    foreach (int score in t.Scores)
                    {
                        team.PlayMatch(score);
                    }
                    group.Add(team);
                }

                return group;
            }
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);

            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                var teamFromOutput = XML_Blue_5.Deserialize(fs) as SerializeOtherTeam;

                if (teamFromOutput == null) return default;

                var team = (T)GetTeam(teamFromOutput);
                foreach (var sportsman in teamFromOutput.Sportsman)
                {
                    if (sportsman.Name == null) continue;

                    var s = new Blue_5.Sportsman(sportsman.Name, sportsman.Surname);
                    s.SetPlace(sportsman.Place);
                    team.Add(s);
                }

                return team;
            }
        }
    }
}

