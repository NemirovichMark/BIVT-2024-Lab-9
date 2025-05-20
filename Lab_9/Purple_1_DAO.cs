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
            (string, string)[] props = lines.Select(l => l.Split('='))
                .Select(l => (l[0], l[1]))
                .ToArray();

            foreach ((string prop_name, string prop_val) in props)
            {
                switch (prop_name)
                {
                    case "name":
                        Name = prop_val;
                        break;
                    case "surname":
                        Surname = prop_val;
                        break;
                    case "coefs":
                        Coefs = prop_val.Split(',').Select(v => double.Parse(v)).ToArray();
                        break;
                    case "marks":
                        int[][] rows = prop_val.Split(';').Select(r => r.Split(',')).Select(r => r.Select(e => int.Parse(e)).ToArray()).ToArray();
                        int[,] matrix = new int[MARKS_ROW_COUNT, MARKS_COL_COUNT];

                        for (int row = 0; row < MARKS_ROW_COUNT; row++)
                        {
                            for (int col = 0; col < MARKS_COL_COUNT; col++)
                            {
                                matrix[row, col] = rows[row][col];
                            }
                        }
                        Marks = matrix;
                        break;
                    default: break;
                }
            }
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

}
