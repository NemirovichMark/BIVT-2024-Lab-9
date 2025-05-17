using Lab_7;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Lab_7.Purple_1;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";
        private void SerializeObject<T>(T obj, string fileName) where T: class
        {
            SelectFile(fileName);
            if(obj == null || FilePath == null)
            {
                return;
            }
            var dict = JObject.FromObject(obj);
            dict.Add("Type", obj.GetType().ToString());
            File.WriteAllText(FilePath, dict.ToString());

        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SerializeObject(obj, fileName);
        }


        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SerializeObject(jumping, fileName);
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SerializeObject(skating, fileName);
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SerializeObject(participant, fileName);
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SerializeObject(report, fileName);
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);
            string type = content["Type"].ToString();


            if(type == typeof(Purple_1.Participant).ToString())
            {
                return DeserializePatricipant(content) as T;
            }
            else if( type == typeof(Purple_1.Judge).ToString())
            {
                return DeserializeJudge(content) as T;
            }
            else if( type == typeof(Purple_1.Competition).ToString())
            {
                return DeserializeCompetition(content) as T;
            }
            return null;
        }

        private Purple_1.Participant DeserializePatricipant(JObject content)
        {

            var Name = content["Name"].Value<string>();
            var Surname = content["Surname"].Value<string>();
            var Coefs = content["Coefs"].ToObject<double[]>();
            var Marks = content["Marks"].ToObject<int[,]>();

            var participant = new Purple_1.Participant(Name, Surname);


            participant.SetCriterias(Coefs);
            for(int i = 0; i < Marks.GetLength(0); ++i)
            {
                var arr = new int[Marks.GetLength(1)];
                for(int j =0; j < Marks.GetLength(1); ++j)
                {
                    arr[i] = Marks[i, j];
                }
                participant.Jump(arr);
            }
            return participant;
        }

        private Purple_1.Judge DeserializeJudge(JObject content)
        {
            var Name = content["Name"].Value<string>();
            var Marks = content["Marks"].ToObject<int[]>();

            var judge = new Purple_1.Judge(Name, Marks);
            return judge;
        }

        private Purple_1.Competition DeserializeCompetition(JObject content)
        {
            var Judges = content.ToObject<Purple_1.Judge[]>();
            var comp = new Purple_1.Competition(Judges);
            foreach(var participant in content["Participants"])
            {
                var newobj = JObject.Parse(participant.ToString());
                comp.Add(DeserializePatricipant(newobj));
            }
            return comp;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);
            string type = content["Type"].ToString();
            Purple_2.SkiJumping jumping;
            if(type == typeof(Purple_2.JuniorSkiJumping).ToString())
            {
                jumping = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                jumping = new Purple_2.ProSkiJumping();
            }

            foreach(var p in content["Participants"])
            {
                var marks = p["Marks"].ToObject<int[]>();
                var res = p["Result"].Value<int>();
                var dist = p["Distance"].Value<int>();

                var part = p.ToObject<Purple_2.Participant>();

                int sumWithoutMaxes = marks.Sum() - marks.Min() - marks.Max();

                double target;

                if (res == 0) target = dist + (sumWithoutMaxes + 60) / 2.0;
                else target = dist - (res - sumWithoutMaxes - 60) / 2.0;

                int targ = (int)Math.Ceiling(target);
                part.Jump(dist, marks, targ);
                jumping.Add(part);
            }
            return jumping as T;
        }


        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);

            string type = content["Type"].ToString();

            Purple_3.Skating skating;

            if(type == typeof(Purple_3.FigureSkating).ToString())
            {
                skating = new Purple_3.FigureSkating(content["Moods"].ToObject<double[]>(), false);

            }
            else
            {
                skating = new Purple_3.IceSkating(content["Moods"].ToObject<double[]>(), false);
            }

            var parts = new Purple_3.Participant[0];

            foreach(var p in content["Participants"])
            {
                var marks = p["Marks"].ToObject<double[]>();
                var participant = p.ToObject<Purple_3.Participant>();
                foreach(var mark in marks)
                {
                    participant.Evaluate(mark);
                }
                Array.Resize(ref parts, parts.Length + 1);
                parts[^1] = participant;
            }

            Purple_3.Participant.SetPlaces(parts);
            skating.Add(parts);

            return skating as T;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);

            var gp = new Purple_4.Group(content["Name"].Value<string>());
            return null;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            throw new NotImplementedException();
        }
    }


}
