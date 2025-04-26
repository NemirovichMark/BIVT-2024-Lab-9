using Lab_7;

namespace Lab_9{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        public override void SerializePurple1<T>(T obj, string fileName) where T : class{}
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName) {}
        public override void SerializePurple3Skating<T>(T skating, string fileName){}
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName){}
        public override void SerializePurple5Report(Purple_5.Report group, string fileName){}

        public override T DeserializePurple1<T>(string fileName) where T : class{return null;}
        public override T DeserializePurple2SkiJumping<T>(string fileName){ return null;}
        public override T DeserializePurple3Skating<T>(string fileName) { return null;}
        public override Purple_4.Group DeserializePurple4Group(string fileName){ return null;}
        public override Purple_5.Report DeserializePurple5Report(string fileName){ return null;}
    }
}