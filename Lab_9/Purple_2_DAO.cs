using Lab_7;
namespace Lab_9
{
    public class Purple_2_Participant_DAO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int[] Marks { get; set; }
        public int Distance { get; set; }

        public Purple_2_Participant_DAO() { }
        public Purple_2_Participant_DAO(string name, string surname, int[] marks, int distance)
        {
            Name = name;
            Surname = surname;
            Marks = marks;
            Distance = distance;
        }

        public Purple_2_Participant_DAO(Purple_2.Participant participant)
        {
            Name = participant.Name;
            Surname = participant.Surname;
            Marks = participant.Marks;
            Distance = participant.Distance;
        }

        public Purple_2.Participant ToObject(int target = 0)
        {
            var p = new Purple_2.Participant(Name, Surname);
            p.Jump(Distance, Marks, target);
            return p;
        }

    }
    public class Purple_2_Ski_Jumping_DAO
    {
        public Purple_2_Ski_Jumping_DAO() { }
        public Purple_2_Ski_Jumping_DAO(string name, int distance, Purple_2_Participant_DAO[] participants)
        {
            Name = name;
            Distance = distance;
            Participants = participants;
        }
        public string Name { get; set; }
        public int Distance { get; set; }
        public Purple_2_Participant_DAO[] Participants { get; set; }

        public Purple_2.SkiJumping ToObject()
        {
            Purple_2.SkiJumping sj;
            if (Name == "100m")
            {
                sj = new Purple_2.JuniorSkiJumping();
            }
            else if (Name == "150m")
            {
                sj = new Purple_2.ProSkiJumping();

            }
            else { return default; }
            sj.Add(Participants.Select(p => p.ToObject(Distance)).ToArray());
            return sj;
        }

    }
}
