using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        private void Serial_Process<T>(T obj, string name)
        {
            SelectFile(name);
            if (obj == null) return;
            if (FilePath == null) return;


            try
            {
                var jObject = JObject.FromObject(obj);
                jObject["Type"] = obj.GetType().Name;

                var jsstring = jObject.ToString();

                File.WriteAllText(FilePath, jsstring);
            }
            catch
            {
                Console.WriteLine("Error?/>,./.");
            }
        }
        public override void SerializePurple1<T>(T obj, string fileName) // where T : class
        {
            Serial_Process(obj, fileName);
        }


        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName) // where T : Purple_2.SkiJumping
        {
            Serial_Process(jumping, fileName);
        }


        public override void SerializePurple3Skating<T>(T skating, string fileName) //where T : Purple_3.Skating
        {
            Serial_Process(skating, fileName);
        }


        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            Serial_Process(group, fileName);
        }


        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            Serial_Process(report, fileName);
        }
        public override T DeserializePurple1<T>(string fileName) //where T : class
        {

            SelectFile(fileName);

            var Information = File.ReadAllText(FilePath);
            var js_info = JObject.Parse(Information);

            var type_of_js_info = js_info["Type"].ToString();

            switch (type_of_js_info)
            {
                case "Participant":

                    var Participant = new Purple_1.Participant(js_info["Name"].ToString(), js_info["Surname"].ToString());
                    var coefs = js_info["Coefs"].ToObject<double[]>();
                    var marks = js_info["Marks"].ToObject<int[][]>();

                    Participant.SetCriterias(coefs);


                    foreach (var mark in marks)
                    {
                        Participant.Jump(mark);

                    }
                    return Participant as T;
                case "Judge":
                    var Judge = new Purple_1.Judge(js_info["Name"].ToString(), js_info["Marks"].ToObject<int[]>());
                    
                    return  Judge as T;

                case "Competition":

                    var Competition = new Purple_1.Competition(
                           js_info["Judges"].ToObject<JObject[]>().Select(inf =>
                               new Purple_1.Judge(inf["Name"].ToString(), inf["Marks"].ToObject<int[]>())
                               ).ToArray()
                           );
                    Competition.Add(
                        js_info["Participants"].ToObject<JObject[]>().Select(inf =>
                        {
                            var Participant_1 = new Purple_1.Participant(
                                    inf["Name"].ToString(),
                                    inf["Surname"].ToString()
                                );
                            Participant_1.SetCriterias(inf["Coefs"].ToObject<double[]>());
                            foreach (var mark in inf["Marks"].ToObject<int[][]>())
                            {
                                Participant_1.Jump(mark);
                            }
                            return Participant_1;
                        }
                        ).ToArray());
                    return Competition as T;

            }

            return null;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName) //where T : Purple_2.SkiJumping
        {
            SelectFile(fileName);

            var info = File.ReadAllText(FilePath);
            var js_info = JObject.Parse(info);

            Purple_2.SkiJumping jump;

            switch (js_info["Type"].ToString())
            {
                case "JuniorSkiJumping":
                    jump = new Purple_2.JuniorSkiJumping();
                    break;
                case "ProSkiJumping":
                    jump = new Purple_2.ProSkiJumping();
                    break;
                default:
                    return null;
            }

            jump.Add(js_info["Participants"].ToObject<JObject[]>().Select(participant =>
            {
                Purple_2.Participant person = new Purple_2.Participant(participant["Name"].ToString(), participant["Surname"].ToString());

                person.Jump(participant["Distance"].ToObject<int>(), participant["Marks"].ToObject<int[]>(), js_info["Standard"].ToObject<int>());
                return person;
            }).ToArray());

            return (T) jump ;

        }


        public override T DeserializePurple3Skating<T>(string fileName) //where T : Purple_3.Skating
        {
            SelectFile(fileName);

            var knowledge = File.ReadAllText(FilePath);
            var info = JObject.Parse(knowledge);

            Purple_3.Skating skating;

            switch (info["Type"].ToString())
            {
                case "FigureSkating":
                    skating = new Purple_3.FigureSkating(info["Moods"].ToObject<double[]>(), false);
                    break;
                case "IceSkating":
                    skating = new Purple_3.IceSkating(info["Moods"].ToObject<double[]>(), false);
                    break;
                default: return null;
            }

            skating.Add(info["Participants"].ToObject<JObject[]>().Select(PARTICIPANT =>
            {

                var person = new Purple_3.Participant(PARTICIPANT["Name"].ToString(), PARTICIPANT["Surname"].ToString());
                var marks = PARTICIPANT["Marks"].ToObject<double[]>();

                foreach (var mark in marks)
                {
                    person.Evaluate(mark);
                }

                return person;
            }

            ).ToArray());
            Purple_3.Participant.SetPlaces(skating.Participants);
            return (T) skating;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var route = File.ReadAllText(FilePath);
            var info = JObject.Parse(route);

            Purple_4.Group group = new Purple_4.Group(info["Name"].ToString());

            group.Add(info["Sportsmen"].ToObject<JObject[]>().Select(sport =>
            {
                var sportsman = new Purple_4.Sportsman(sport["Name"].ToString(), sport["Surname"].ToString());
                sportsman.Run(sport["Time"].ToObject<double>());
                return sportsman;
            }).ToArray());
            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var something = File.ReadAllText(FilePath);
            var info_5 = JObject.Parse(something);

            Purple_5.Report answer = new Purple_5.Report();

            answer.AddResearch(info_5["Researches"].ToObject<JObject[]>().Select(res =>
            {
                var ress = new Purple_5.Research(res["Name"].ToString());

                var responses = res["Responses"].ToObject<JObject[]>();

                foreach (var one_resp in responses)
                {
                    var massive = new string[3];
                    for (int i = 0; i < massive.Length; i++)
                    {
                        if (i == 0) massive[i] = one_resp["Animal"].ToObject<String>();
                        else if (i == 1) massive[i] = one_resp["CharacterTrait"].ToObject<String>();
                        else
                        {
                            massive[i] = one_resp["Concept"].ToObject<String>();
                        }
                    }
                    ress.Add(massive);
                }
                return ress;
            }).ToArray());

            return answer;
        }

        public override string Extension => "json";



    }
}
