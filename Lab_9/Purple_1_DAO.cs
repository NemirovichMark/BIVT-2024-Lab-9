using System.Text;
using Lab_7;
namespace Lab_9
{
    public class Purple_1_Participant_DAO
    {
        private const int MARKS_COL_COUNT = 4;
        private const int MARKS_ROW_COUNT = 7;


        public Purple_1_Participant_DAO(string name, string surname, double[] coefs, int[,] marks)
        {
            Name = name;
            Surname = surname;
            Coefs = coefs;
            Marks = marks;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public double[] Coefs { get; private set; }
        public int[,] Marks { get; private set; }

        public Purple_1_Participant_DAO(string[] lines)
        {
            string[] props = lines.Select(l => l.Split('='))
                .Select(l => l[1])
                .ToArray();

            if (props[0] != nameof(Purple_1.Participant)) return;

            Name = props[1];
            Surname = props[2];
            Coefs = props[3].Split(',').Select(v => double.Parse(v)).ToArray();

            int[][] rows = props[4].Split(';').Select(r => r.Split(',')).Select(r => r.Select(e => int.Parse(e)).ToArray()).ToArray();
            int[,] matrix = new int[MARKS_ROW_COUNT, MARKS_COL_COUNT];

            for (int row = 0; row < MARKS_ROW_COUNT; row++)
            {
                for (int col = 0; col < MARKS_COL_COUNT; col++)
                {
                    matrix[row, col] = rows[row][col];
                }
            }
            Marks = matrix;
        }

        public string[] SerializeToTXT()
        {
            string[] lines = new string[0];

            StringBuilder sb = new StringBuilder();

            for (int row = 0; row < MARKS_ROW_COUNT; row++)
            {
                for (int col = 0; col < MARKS_COL_COUNT; col++)
                {
                    sb.Append(Marks[row, col]);

                    if (col < MARKS_COL_COUNT - 1) sb.Append(',');
                }
                if (row < MARKS_ROW_COUNT - 1) sb.Append(';');
            }

            Helpers.AppendToArray(lines, $"type={nameof(Purple_1.Participant)}");
            Helpers.AppendToArray(lines, $"name={Name}");
            Helpers.AppendToArray(lines, $"surname={Surname}");
            Helpers.AppendToArray(lines, $"coefs={string.Join(',', Coefs)}");
            Helpers.AppendToArray(lines, $"marks={sb.ToString()}");

            return lines;
        }
    }

    public class Purple_1_Judge_DAO
    {
        public Purple_1_Judge_DAO(string name, int[] marks)
        {
            Name = name;
            Marks = marks;
        }

        public string Name { get; private set; }
        public int[] Marks { get; private set; }

        public string[] SerializeToTXT()
        {
            string[] lines = new string[0];

            Helpers.AppendToArray(lines, $"type={nameof(Purple_1.Judge)}");
            Helpers.AppendToArray(lines, $"name={Name}");
            Helpers.AppendToArray(lines, $"marks={string.Join(',', Marks)}");

            return lines;
        }

        public Purple_1_Judge_DAO(string[] lines)
        {
            string[] props = lines.Select(l => l.Split('=')[1]).ToArray();

            if (props[0] != nameof(Purple_1.Judge)) return;

            Name = props[1];
            Marks = props[2].Split(',').Select(v => int.Parse(v)).ToArray();
        }

    }

    public class Purple_1_Competition_DAO
    {
        public Purple_1_Competition_DAO(Purple_1_Participant_DAO[] participants, Purple_1_Judge_DAO[] judges)
        {
            Participants = participants;
            Judges = judges;
        }
        public Purple_1_Participant_DAO[] Participants { get; private set; }
        public Purple_1_Judge_DAO[] Judges { get; private set; }

        public string[] SerializeToTXT()
        {
            string[] lines = new string[0];
            Helpers.AppendToArray(lines, $"type={nameof(Purple_1.Judge)}");
            foreach (var p in Participants)
            {
                Helpers.AppendToArray(lines, p.SerializeToTXT());
            }

            foreach (var j in Judges)
            {
                Helpers.AppendToArray(lines, j.SerializeToTXT());
            }
            return lines;
        }

        public Purple_1_Competition_DAO(string[] lines)
        {
            if (!lines[0].EndsWith(nameof(Purple_1.Competition))) return;

            Judges = new Purple_1_Judge_DAO[0];
            Participants = new Purple_1_Participant_DAO[0];

            string[] curLines = new string[0];

            lines = lines.AsSpan(1).ToArray();
            foreach (string l in lines)
            {
                if (l.StartsWith("type") && curLines.Length > 0)
                {
                    if (curLines[0].EndsWith(nameof(Purple_1.Participant)))
                    {
                        Purple_1_Participant_DAO dao = new Purple_1_Participant_DAO(curLines);
                        Helpers.AppendToArray(Participants, dao);
                    }
                    if (curLines[0].EndsWith(nameof(Purple_1.Judge)))
                    {
                        Purple_1_Judge_DAO dao = new Purple_1_Judge_DAO(curLines);
                        Helpers.AppendToArray(Judges, dao);
                    }

                    curLines = new string[0];
                }

                Helpers.AppendToArray(curLines, l);
            }
        }
    }
}
