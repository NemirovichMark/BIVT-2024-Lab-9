using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Lab_9.BlueSerializer;  // статик для использования членов класса без указания названия класса

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

        XmlSerializer XML_Blue_1 = new XmlSerializer(typeof(ResponseOutput)); // создаем экземпляр класса, передаем в него тип
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_1.Serialize(fs, new ResponseOutput(participant));
            }
        }


        XmlSerializer XML_Blue_2 = new XmlSerializer(typeof(WaterJumpOutput));
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_2.Serialize(fs, new WaterJumpOutput(participant));
            }
        }

        XmlSerializer XML_Blue_3 = new XmlSerializer(typeof(Blue_3_ParticipantOutput));
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_3.Serialize(fs, new Blue_3_ParticipantOutput((Blue_3.Participant)student));
            }
        }

        XmlSerializer XML_Blue_4 = new XmlSerializer(typeof(Blue_4_GroupOutput));
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_4.Serialize(fs, new Blue_4_GroupOutput(participant));
            }
        }

        XmlSerializer XML_Blue_5 = new XmlSerializer(typeof(Blue_5_TeamOutput));
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                XML_Blue_5.Serialize(fs, new Blue_5_TeamOutput(group));
            }
        }

        // <--->
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);

            ResponseOutput participant;
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                participant = XML_Blue_1.Deserialize(fs) as ResponseOutput;
            }

            if (participant == null) return null;
            if (participant.Name == null) return null;
            if (participant.Surname == null) 
                return new Blue_1.Response(participant.Name, participant.Votes);

            return new Blue_1.HumanResponse(participant.Name, participant.Surname, participant.Votes);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);

            WaterJumpOutput participant;
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                participant = XML_Blue_2.Deserialize(fs) as WaterJumpOutput;
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
                var student = XML_Blue_3.Deserialize(fs) as Blue_3_ParticipantOutput;

                if (student == null) return default(T);

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
                var groupOutput = XML_Blue_4.Deserialize(fs) as Blue_4_GroupOutput;

                if (groupOutput == null) return null;

                var group = new Blue_4.Group(groupOutput.Name);
                foreach (var t in groupOutput.Teams)
                {
                    var team = GetTeam(t);
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
                var teamFromOutput = XML_Blue_5.Deserialize(fs) as Blue_5_TeamOutput;

                if (teamFromOutput == null) return default(T);

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
