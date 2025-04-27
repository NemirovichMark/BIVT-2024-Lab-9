using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_9;
using Lab_7;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer{
        public override string Extension => "XML";
        public override void SerializePurple1<T>(T obj, string fileName){
            throw new NotImplementedException();
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName){
            throw new NotImplementedException();
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName){
            throw new NotImplementedException();
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName){
            throw new NotImplementedException();
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName){
            throw new NotImplementedException();
        }

        public override T DeserializePurple1<T>(string fileName){
            throw new NotImplementedException();
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName){
            throw new NotImplementedException();
        }
        public override T DeserializePurple3Skating<T>(string fileName){
            throw new NotImplementedException();
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName){
            throw new NotImplementedException();
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName){
            throw new NotImplementedException();
        }
    }
}