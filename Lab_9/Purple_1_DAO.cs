using System.Text.Json.Serialization;
using Lab_7;
namespace Lab_9
{
    public class Purple_1_Participant_DAO
    {
        private const int MARKS_COL_COUNT = 7;
        private const int MARKS_ROW_COUNT = 4;


        public Purple_1_Participant_DAO() { }

        public Purple_1_Participant_DAO(string name, string surname, double[] coefs, int[,] marks)
        {
            Name = name;
            Surname = surname;
            Coefs = coefs;
            Marks = new int[4][];

            for (int i = 0; i < 4; i++)
            {
                Marks[i] = new int[7];
                for (int j = 0; j < 7; j++)
                {
                    Marks[i][j] = marks[i, j];
                }
            }
        }

        public Purple_1_Participant_DAO(Purple_1.Participant participant)
        {
            Name = participant.Name;
            Surname = participant.Surname;
            Coefs = participant.Coefs;
            Marks = new int[4][];

            for (int i = 0; i < 4; i++)
            {
                Marks[i] = new int[7];
                for (int j = 0; j < 7; j++)
                {
                    Marks[i][j] = participant.Marks[i, j];
                }
            }
        }

        public Purple_1.Participant ToObject(bool doJump = true)
        {
            var p = new Purple_1.Participant(Name, Surname);
            p.SetCriterias(Coefs);
            if (doJump)
            {
                foreach (int[] marks in Marks)
                {
                    p.Jump(marks);
                }
            }
            return p;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public double[] Coefs { get; set; }
        public int[][] Marks { get; set; }
    }

    public class Purple_1_Judge_DAO
    {
        public Purple_1_Judge_DAO() { }
        public Purple_1_Judge_DAO(string name, int[] marks)
        {
            Name = name;
            FavMarks = marks;
        }

        public Purple_1_Judge_DAO(Purple_1.Judge judge)
        {
            Name = judge.Name;
            FavMarks = judge.Marks;
        }

        public Purple_1.Judge ToObject()
        {
            return new Purple_1.Judge(Name, FavMarks);
        }

        public string Name { get; set; }
        public int[] FavMarks { get; set; }
    }

    public class Purple_1_Competition_DAO
    {
        public Purple_1_Competition_DAO() { }
        public Purple_1_Competition_DAO(Purple_1_Participant_DAO[] participants, Purple_1_Judge_DAO[] judges)
        {
            Participants = participants;
            Judges = judges;
        }

        public Purple_1_Competition_DAO(Purple_1.Competition competition)
        {
            Participants = competition.Participants.Select(p => new Purple_1_Participant_DAO(p)).ToArray();
            Judges = competition.Judges.Select(j => new Purple_1_Judge_DAO(j)).ToArray();
        }

        public Purple_1.Competition ToObject()
        {
            var c = new Purple_1.Competition(Judges.Select(j => j.ToObject()).ToArray());
            var participants = Participants.Select(p => p.ToObject(false)).ToArray();
            foreach (var p in participants)
            {
                for (int i = 0; i < 4; i++)
                {
                    c.Evaluate(p);
                    for (int j = 0; j < 7; j++) Console.Write($"{p.Marks[i, j]}, ");
                    Console.WriteLine();
                }
                c.Add(p);
                Console.WriteLine();
            }
            return c;
        }

        public Purple_1_Participant_DAO[] Participants { get; set; }
        public Purple_1_Judge_DAO[] Judges { get; set; }



    }
}
