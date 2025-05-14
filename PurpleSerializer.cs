using Lab_7;

namespace Lab_9
{
    public class PurpleSerializer : FileSerializer
    {
        public override string Extension => "txt";

        void SerializePurple1<T>(T obj, string fileName) where T : class
        {
        }
        void SerializePurple2SkiJumping<T>(T jumping, string fileName) where T : Purple_2.SkiJumping;
        void SerializePurple3Skating<T>(T skating, string fileName) where T : Purple_3.Skating;
        void SerializePurple4Group(Purple_4.Group group, string fileName);
        void SerializePurple5Report(Purple_5.Report report, string fileName);

        void SerializePurple1Participant(Purple_1.Participant participant, string fileName)
        {
            int[] marksArray = new int[participant.Marks.GetLength(0) * participant.Marks.GetLength(1)];
            for (int i = 0; i < participant.Marks.GetLength(1); i++)
            {
                for (int j = 0; j < participant.Marks.GetLength(0); j++)
                {
                    marksArray[i * participant.Marks.GetLength(0) + j] = participant.Marks[i, j];
                }
            }
            string[] lines = {
            $"name: {participant.Name}",
            $"surname: {participant.Surname}",
            $"marks: {string.Join(',', marksArray)}",
            $"coefs: {string.Join(',', participant.Coefs)}",
          };

            int fileNameStartAfter = fileName.LastIndexOf('/');
            string filePath = fileName.Take(fileNameStartAfter).ToString();
            fileName = fileName.TakeLast(fileName.Length)
        }
    }
}
