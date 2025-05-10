using System.Xml.Serialization;
using Lab_7;
using Lab_9;

namespace Lab_9 {
    public class Program {
        public static void Main(string[] args) {
            string folderPath = @"/Users/avolon/Documents/C#/BIVT-2024-Lab-9/TestSerialize";
            string fileName = "white1_participant";
            var serializer = new WhiteTXTSerializer();
            serializer.SelectFolder(folderPath);
            
            var participant = new White_1.Participant("John Doe", "12345");
            serializer.SerializeWhite1Participant(participant, fileName);
        }
    }
}