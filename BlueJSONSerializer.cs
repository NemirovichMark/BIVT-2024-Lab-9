using Lab_7;
using System.Text.Json;
using static Lab_9.SerializeObject;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer 
    {
        
        public override string Extension => "json";



        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            
            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new ResponseSerialize(participant)));
        }


        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new SerializeWaterJump(participant)));
        }
        public override void SerializeBlue3Participant<T>(T participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new OtherPart(participant)));
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new SerializeGroup(participant)));
        }
        public override void SerializeBlue5Team<T>(T participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(new SerializeOtherTeam(participant)));
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            string str = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(str)) return null;
            
            var participant = JsonSerializer.Deserialize<ResponseSerialize>(str);

            if (participant == null) return null;
            if (participant.Name == null) return null;
            if (participant.Surname == null)
                return new Blue_1.Response(participant.Name, participant.Votes);

            return new Blue_1.HumanResponse(participant.Name, participant.Surname, participant.Votes);
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            string str = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(str)) return null;

            var participants = JsonSerializer.Deserialize<SerializeWaterJump>(str);
            if (participants == null) return null;
            var answ = GetWaterJump(participants);
            foreach (var participant in participants.Participants)
            {
                var jumper = new Blue_2.Participant(participant.Name, participant.Surname);
                jumper.Jump(participant.FirstJump);
                jumper.Jump(participant.SecondJump);
                answ.Add(jumper);
            }
            return answ;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            string str = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(str)) return null;

            var participant = JsonSerializer.Deserialize<OtherPart>(str);
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

            var sgroup = JsonSerializer.Deserialize<SerializeGroup>(str);

            if (sgroup == null || sgroup.Teams == null) return null;

            var group = new Blue_4.Group(sgroup.Name);
            foreach (var team in sgroup.Teams)
            {
                if (team == null) continue;

                var t = GetTeami(team);
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

            var steam = JsonSerializer.Deserialize<SerializeOtherTeam>(Json);
            var team = (T)GetTeam(steam);
            if (team == null) return default;

            foreach (var player in steam.Sportsman.Where(p => !string.IsNullOrEmpty(p.Name)))
            {
                var sportsman = new Blue_5.Sportsman(player.Name, player.Surname);
                sportsman.SetPlace(player.Place);
                team.Add(sportsman);
            }

            return team;
        }

    }
}
