using Lab_7;
namespace Lab_9
{
    public class Purple_2_Participant_DAO
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }

        public Purple_2_Participant_DAO(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public string[] SerializeToTXT()
        {
            return new string[] { $"{Name}&{Surname}" };
        }

        public Purple_2_Participant_DAO(string[] lines)
        {
            string[] nameAndSurname = lines[0].Split('&');
            Name = nameAndSurname[0];
            Surname = nameAndSurname[1];
        }
    }
    public class Purple_2_Ski_Jumping_DAO
    {
        public Purple_2_Ski_Jumping_DAO(string name, int distance, Purple_2_Participant_DAO[] participants)
        {
            Name = name;
            Distance = distance;
            Participants = participants;
        }
        public string Name { get; private set; }
        public int Distance { get; private set; }
        public Purple_2_Participant_DAO[] Participants { get; private set; }

        public string[] SerializeToTXT()
        {
            return new string[] {
            $"{Name}",
            $"{Distance}",
            $"{string.Join(';', Participants.Select(p => p.SerializeToTXT()[0]))}"
          };
        }

        public Purple_2_Ski_Jumping_DAO(string[] lines)
        {
            Name = lines[0];
            Distance = int.Parse(lines[1]);
            Participants = lines[2].Split(';').Select(info => new Purple_2_Participant_DAO(new string[] { info })).ToArray();
        }
    }
}
