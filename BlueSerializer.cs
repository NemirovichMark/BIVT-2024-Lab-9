using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using static Lab_9.SerializeObject;

namespace Lab_9
{
    public abstract class BlueSerializer : FileSerializer 
    {
        protected BlueSerializer() { }
        public abstract void SerializeBlue1Response(Blue_1.Response participant, string fileName);
        public abstract void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName);
        public abstract void SerializeBlue3Participant<T>(T student, string fileName) where T: Blue_3.Participant;
        public abstract void SerializeBlue4Group(Blue_4.Group participant, string fileName);
        public abstract void SerializeBlue5Team<T>(T group, string fileName) where T : Blue_5.Team;

        public abstract Blue_1.Response DeserializeBlue1Response(string fileName);
        public abstract Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName);
        public abstract T DeserializeBlue3Participant<T>(string fileName) where T : Blue_3.Participant;
        public abstract Blue_4.Group DeserializeBlue4Group(string fileName);
        public abstract T DeserializeBlue5Team<T>(string fileName) where T : Blue_5.Team;

        public static Blue_2.WaterJump GetWaterJump(SerializeWaterJump waterjump)
        {
            if (waterjump.Len == 3) return new Blue_2.WaterJump3m(waterjump.Name, waterjump.Bank);
            else return new Blue_2.WaterJump5m(waterjump.Name, waterjump.Bank);
        }
        public static Blue_3.Participant GetPlayer(OtherPart participant)
        {
            if (participant.Sport == 1)
            {
                return new Blue_3.BasketballPlayer(participant.Name, participant.Surname);
            }
            if (participant.Sport == 2)
            {
                return new Blue_3.HockeyPlayer(participant.Name, participant.Surname);
            }
            return new Blue_3.Participant(participant.Name, participant.Surname);
        }
        public static Blue_4.Team GetTeami(SerializeTeam team)
        {
            if (team.Name == null) return null;
            if (team.ManTeamOrWoman)
            {
                return new Blue_4.ManTeam(team.Name);
            }
            return new Blue_4.WomanTeam(team.Name);
        }
        public static Blue_5.Team GetTeam(SerializeOtherTeam team)
        {
            if (team.Name == null) return null;

            return team.ManTeamOrWoman ? new Blue_5.ManTeam(team.Name) : new Blue_5.WomanTeam(team.Name);
        }
    }
}
