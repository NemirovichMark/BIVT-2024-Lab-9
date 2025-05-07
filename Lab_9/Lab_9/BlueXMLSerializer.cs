using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

        // Blue_1
        XmlSerializer xs_Blue_1 = new XmlSerializer(typeof(ResponseDTO));
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName) {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            xs_Blue_1.Serialize(fs, new ResponseDTO(participant));
            fs.Close();
        }
        public override Blue_1.Response DeserializeBlue1Response(string fileName) {
            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            var p = xs_Blue_1.Deserialize(fs) as ResponseDTO;
            fs.Close();

            if (p == null) return null;
            if (p.Surname == null)
                return new Blue_1.Response(p.Name, p.Votes);
            return new Blue_1.HumanResponse(p.Name, p.Surname, p.Votes);
        }
        

        // Blue_2
        XmlSerializer xs_Blue_2 = new XmlSerializer(typeof(WaterJumpDTO));
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName) {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            xs_Blue_2.Serialize(fs, new WaterJumpDTO(participant));
            fs.Close();
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            var p = xs_Blue_2.Deserialize(fs) as WaterJumpDTO;
            fs.Close();

            if (p == null) return null;
            var ans = GetWaterJump(p);
            foreach ( var participant in p.Participants)
            {
                var jumper = new Blue_2.Participant(participant.Name, participant.Surname);
                jumper.Jump(participant.Jump_1);
                jumper.Jump(participant.Jump_2);
                ans.Add(jumper);
            }
            return ans;
        } 
        

        // Blue_3
        XmlSerializer xs_Blue_3 = new XmlSerializer(typeof(Blue_3_ParticipantDTO));
        public override void SerializeBlue3Participant<T>(T student, string fileName) 
        {
            if (student == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            xs_Blue_3.Serialize(fs, new Blue_3_ParticipantDTO((Blue_3.Participant)student));
            fs.Close();
        }
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            var student = xs_Blue_3.Deserialize(fs) as Blue_3_ParticipantDTO;
            fs.Close();

            if (student == null) return null;
            var player = (T)GetPlayer(student);
            foreach(var penalty in student.Penalties)
            {
                player.PlayMatch(penalty);
            }
            
            return player; 
        }


        // Blue_4
        XmlSerializer xs_Blue_4 = new XmlSerializer(typeof(Blue_4_GroupDTO));
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName) 
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            xs_Blue_4.Serialize(fs, new Blue_4_GroupDTO(participant));
            fs.Close();
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            var groupDTO = xs_Blue_4.Deserialize(fs) as Blue_4_GroupDTO;
            fs.Close();

            if (groupDTO == null) return null;
            var group = new Blue_4.Group(groupDTO.Name);
            foreach(var teamDTO in groupDTO.Teams)
            {
                var team = GetTeam(teamDTO);
                if(team == null) continue;
                foreach(int score in teamDTO.Scores)
                    team.PlayMatch(score);
                group.Add(team);
            }

            return group;
        }
        
        
        // Blue_5
        XmlSerializer xs_Blue_5 = new XmlSerializer(typeof(Blue_5_TeamDTO));
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            xs_Blue_5.Serialize(fs, new Blue_5_TeamDTO(group));
            fs.Close();
        }
        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            var teamDTO = xs_Blue_5.Deserialize(fs) as Blue_5_TeamDTO;
            fs.Close();

            if (teamDTO == null) return null;
            var team = (T) GetTeam(teamDTO);
            foreach(var sportsmanDTO in teamDTO.Sportsmen)
            {
                if(sportsmanDTO.Name == null) continue;
                var sportsman = new Blue_5.Sportsman(sportsmanDTO.Name, sportsmanDTO.Surname);
                sportsman.SetPlace(sportsmanDTO.Place);
                team.Add(sportsman);
            }

            return team; 
        }
    }
}
