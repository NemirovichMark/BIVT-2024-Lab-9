using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Lab_7;
using static Lab_9.BlueSerializer;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new ResponseOutput(participant)));
        }
        

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new WaterJumpOutput(participant)));
        }
        public override void SerializeBlue3Participant<T>(T participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new Blue_3_ParticipantOutput(participant)));
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new Blue_4_GroupOutput(participant)));
        }
        public override void SerializeBlue5Team<T>(T participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new Blue_5_TeamOutput(participant)));
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            string str = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(str)) return null;

            var participant = JsonSerializer.Deserialize<ResponseOutput>(str);

            if (participant == null || participant.Name == null) return null;
            if (participant.Surname == null)
            {
                return new Blue_1.Response(participant.Name, participant.Votes);
            }

            return new Blue_1.HumanResponse(participant.Name, participant.Surname, participant.Votes);
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            string str = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(str)) return null;

            var participants = JsonSerializer.Deserialize<WaterJumpOutput>(str);
            if (participants == null) return null;
            var answer = GetWaterJump(participants);
            foreach (var participant in participants.Participants)
            {
                var jumper = new Blue_2.Participant(participant.Name, participant.Surname);
                jumper.Jump(participant.FirstJump);
                jumper.Jump(participant.SecondJump);
                answer.Add(jumper);
            }
            return answer;
        }
        
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            string str = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(str)) return null;

            var participant = JsonSerializer.Deserialize<Blue_3_ParticipantOutput>(str);
            if (participant == null) return null;
            var jumper = (T)GetPlayer(participant);
            foreach (var penalty in participant.Penalties)
            {
                jumper.PlayMatch(penalty);
            }

            return jumper;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);

            if (!File.Exists(FilePath)) return null;

            var str = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(str)) return null;

            var groupOutput = JsonSerializer.Deserialize<Blue_4_GroupOutput>(str);

            if (groupOutput == null || groupOutput.Teams == null) return null;

            var group = new Blue_4.Group(groupOutput.Name);
            foreach (var team in groupOutput.Teams)
            {
                if (team == null) continue;

                var t = GetTeam(team);
                if (t == null) continue;

                if (team.Scores != null)
                {
                    foreach (var score in team.Scores)
                    {
                        t.PlayMatch(score);
                    }
                }
                group.Add(t);
            }

            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);

            if (!File.Exists(FilePath)) return default;

            var Json = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(Json)) return default;

            var teamOutput = JsonSerializer.Deserialize<Blue_5_TeamOutput>(Json);
            var team = (T)GetTeam(teamOutput);
            if (team == null) return default;

            foreach (var player in teamOutput.Sportsman.Where(p => !string.IsNullOrEmpty(p.Name)))
            {
                var sportsman = new Blue_5.Sportsman(player.Name, player.Surname);
                sportsman.SetPlace(player.Place);
                team.Add(sportsman);
            }

            return team;
        }

    }
}
