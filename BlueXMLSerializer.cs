using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Lab_9.ClassDTO;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            ResponseSD part = new ResponseSD(participant);
            XmlSerializer xml = new XmlSerializer(typeof(ResponseSD));
            using (var file = new StreamWriter(fileName))
            {
                xml.Serialize(file, part);
            }
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            WaterJumpSD part = new WaterJumpSD(participant);
            XmlSerializer xml = new XmlSerializer(typeof(WaterJumpSD));
            using (var file = new StreamWriter(fileName))
            {
                xml.Serialize(file, part);
            }
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            Participant3SD part = new Participant3SD(student);
            XmlSerializer xml = new XmlSerializer(typeof(Participant3SD));
            using (var file = new StreamWriter(fileName))
            {
                xml.Serialize(file, part);
            }
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            GroupSD part = new GroupSD(participant);
            XmlSerializer xml = new XmlSerializer(typeof(GroupSD));
            using (var file = new StreamWriter(fileName))
            {
                xml.Serialize(file, part);
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            Team5SD part = new Team5SD(group);
            XmlSerializer xml = new XmlSerializer(typeof(Team5SD));
            using (var file = new StreamWriter(fileName))
            {
                xml.Serialize(file, part);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            XmlSerializer xml = new XmlSerializer(typeof(ResponseSD));
            ResponseSD part;
            using (var file = new StreamReader(fileName))
            {
                part = (ResponseSD)xml.Deserialize(file);
            }
            Blue_1.Response result = new Blue_1.Response(part.Name, part.Votes);
            if (part.Surname != null)
            {
                result = new Blue_1.HumanResponse(part.Name, part.Surname, part.Votes);
            }
            return result;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            XmlSerializer xml = new XmlSerializer(typeof(WaterJumpSD));
            WaterJumpSD part;
            using (var file = new StreamReader(fileName))
            {
                part = (WaterJumpSD)xml.Deserialize(file);
            }

            Blue_2.WaterJump result = new Blue_2.WaterJump3m(part.Name, part.Bank);
            if (part.Type == "WaterJump5m")
            {
                result = new Blue_2.WaterJump5m(part.Name, part.Bank);
            }
            foreach (ParticipantSD i in part.Participants)
            {
                Blue_2.Participant part2 = new Blue_2.Participant(i.Name, i.Surname);
                foreach (var j in i.Marks)
                {
                    part2.Jump(j);
                }

                result.Add(part2);
            }
            return result;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return default(T);

            XmlSerializer xml = new XmlSerializer(typeof(Participant3SD));
            Participant3SD part;
            using (var file = new StreamReader(fileName))
            {
                part = (Participant3SD)xml.Deserialize(file);
            }

            Blue_3.Participant result = new Blue_3.Participant(part.Name, part.Surname);
            if (part.Type == "BasketballPlayer")
            {
                result = new Blue_3.BasketballPlayer(part.Name, part.Surname);
            }
            else if (part.Type == "HockeyPlayer")
            {
                result = new Blue_3.HockeyPlayer(part.Name, part.Surname);
            }
            foreach (var i in part.Penalties)
            {
                result.PlayMatch(i);
            }
            return (T)result;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            XmlSerializer xml = new XmlSerializer(typeof(GroupSD));
            GroupSD part;
            using (var file = new StreamReader(fileName))
            {
                part = (GroupSD)xml.Deserialize(file);
            }

            Blue_4.Group result = new Blue_4.Group(part.Name);

            if (part.ManTeams != null)
            {
                foreach (var part2 in part.ManTeams)
                {
                    if (part2 == null) continue;

                    Blue_4.Team team = new Blue_4.ManTeam(part2.Name);
                    if (part2.Scores != null)
                    {
                        foreach (var score in part2.Scores)
                        {
                            team.PlayMatch(score);
                        }
                    }
                    result.Add(team);
                }
            }

            if (part.WomanTeams != null)
            {
                foreach (var part2 in part.WomanTeams)
                {
                    if (part2 == null) continue;

                    Blue_4.Team team = new Blue_4.WomanTeam(part2.Name);
                    {
                        foreach (var score in part2.Scores)
                        {
                            team.PlayMatch(score);
                        }
                    }
                    result.Add(team);
                }
            }

            return result;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return default(T);

            XmlSerializer xml = new XmlSerializer(typeof(Team5SD));
            Team5SD part;
            using (var file = new StreamReader(fileName))
            {
                part = (Team5SD)xml.Deserialize(file);
            }

            Blue_5.Team result = new Blue_5.WomanTeam(part.Name);
            if (part.Type == "ManTeam")
            {
                result = new Blue_5.ManTeam(part.Name);
            }

            if (part.Sporsman != null)
            {
                foreach (var part2 in part.Sporsman)
                {
                    if (part2 == null) continue;

                    var sportsman = new Blue_5.Sportsman(part2.Name, part2.Surname);
                    sportsman.SetPlace(part2.Place);
                    result.Add(sportsman);
                }
            }

            return (T)result;
        }
    }
}
