using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        // Blue_1
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName) 
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            
            string text = JsonSerializer.Serialize(new ResponseDTO(participant));

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;

            var part = JsonSerializer.Deserialize<ResponseDTO>(text);
            if (part == null) return null;
            if(part.Surname == null) return new Blue_1.Response(part.Name, part.Votes);
            return new Blue_1.HumanResponse(part.Name, part.Surname, part.Votes);   
        }
        
        // Blue_2
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            string text = JsonSerializer.Serialize(new WaterJumpDTO(participant));

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;

            var p = JsonSerializer.Deserialize<WaterJumpDTO>(text);
            if (p == null) return null;
            var ans = GetWaterJump(p);
            foreach (var participant in p.Participants)
            {
                var jumper = new Blue_2.Participant(participant.Name, participant.Surname);
                jumper.Jump(participant.Jump_1);
                jumper.Jump(participant.Jump_2);
                ans.Add(jumper);
            }
            return ans;
        }

        // Blue_3
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || String.IsNullOrEmpty(fileName)) return;

            string text = JsonSerializer.Serialize(new Blue_3_ParticipantDTO(student));

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override T DeserializeBlue3Participant<T>(string fileName) 
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;

            var student = JsonSerializer.Deserialize<Blue_3_ParticipantDTO>(text);
            if (student == null) return null;
            var player = (T)GetPlayer(student);
            foreach (var penalty in student.Penalties)
            {
                player.PlayMatch(penalty);
            }

            return player;
        }

        // Blue_4
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName) 
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            string text = JsonSerializer.Serialize(new Blue_4_GroupDTO(participant));

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;

            var groupDTO = JsonSerializer.Deserialize<Blue_4_GroupDTO>(text);
            if (groupDTO == null) return null;

            var group = new Blue_4.Group(groupDTO.Name);
            foreach (var teamDTO in groupDTO.Teams)
            {
                var team = GetTeam(teamDTO);
                if (team == null) continue;
                foreach (int score in teamDTO.Scores)
                    team.PlayMatch(score);
                group.Add(team);
            }

            return group;
        }

        // Blue_5
        public override void SerializeBlue5Team<T>(T group, string fileName) 
        {
            if (group == null || String.IsNullOrEmpty(fileName)) return;

            string text = JsonSerializer.Serialize(new Blue_5_TeamDTO(group));

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;
            var teamDTO = JsonSerializer.Deserialize<Blue_5_TeamDTO>(text);
            if (teamDTO == null) return null;

            var team = (T)GetTeam(teamDTO);
            foreach (var sportsmanDTO in teamDTO.Sportsmen)
            {
                if (sportsmanDTO.Name == null) continue;
                var sportsman = new Blue_5.Sportsman(sportsmanDTO.Name, sportsmanDTO.Surname);
                sportsman.SetPlace(sportsmanDTO.Place);
                team.Add(sportsman);
            }

            return team;
        }
    }
}
