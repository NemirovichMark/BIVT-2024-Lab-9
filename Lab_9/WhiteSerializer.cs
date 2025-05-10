using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_9;
using Lab_7;

namespace Lab_9 {
    public abstract class WhiteSerializer : FileSerializer
    {
        public abstract void SerializeWhite1Participant(White_1.Participant participant, string fileName);
        public abstract void SerializeWhite2Participant(White_2.Participant participant, string fileName); 
        public abstract void SerializeWhite3Student(White_3.Student student, string fileName); 
        public abstract void SerializeWhite4Human(White_4.Human human, string fileName); 
        public abstract void SerializeWhite5Team(White_5.Team team, string fileName);

        public abstract White_1.Participant DeserializeWhite1Participant(string fileName);
        public abstract White_2.Participant DeserializeWhite2Participant(string fileName);
        public abstract White_3.Student DeserializeWhite3Student(string fileName);
        public abstract White_4.Human DeserializeWhite4Human(string fileName);
        public abstract White_5.Team DeserializeWhite5Team(string fileName);
    }
}