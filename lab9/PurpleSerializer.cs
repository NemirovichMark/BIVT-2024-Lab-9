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
    public abstract class PurpleSerializer : FileSerializer{
        public abstract void SerializePurple1<T>(T obj, string fileName) where T : class;
        public abstract void SerializePurple2SkiJumping<T>(T jumping, string fileName) where T : Purple_2.SkiJumping;
        public abstract void SerializePurple3Skating<T>(T skating, string fileName) where T : Purple_3.Skating;
        public abstract void SerializePurple4Group(Purple_4.Group participant, string fileName);
        public abstract void SerializePurple5Report(Purple_5.Report group, string fileName);

        public abstract T DeserializePurple1<T>(string fileName) where T : class;
        public abstract T DeserializePurple2SkiJumping<T>(string fileName) where T : Purple_2.SkiJumping;
        public abstract T DeserializePurple3Skating<T>(string fileName) where T : Purple_3.Skating;
        public abstract Purple_4.Group DeserializePurple4Group(string fileName);
        public abstract Purple_5.Report DeserializePurple5Report(string fileName); 
    }

}