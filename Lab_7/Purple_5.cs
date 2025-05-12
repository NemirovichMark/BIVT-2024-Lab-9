using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_5
    {
        public struct Response
        {
            private string _animal;
            private string _characterTrait;
            private string _concept;

            public string Animal => _animal;
            public string CharacterTrait => _characterTrait;
            public string Concept => _concept;
            private string[] AnsArr
            {
                get
                {
                    return new string[] { _animal, _characterTrait, _concept };
                }
            }

            public Response(string animal, string characterTrait, string concept)
            {
                _animal = animal;
                _characterTrait = characterTrait;
                _concept = concept;
            }

            public int CountVotes(Response[] responses, int questionNumber)
            {
                if (responses == null) return 0;
                questionNumber--;
                if (AnsArr[questionNumber] == null) return 0;
                int count = 0;
                foreach (var r in responses)
                {
                    if (r.AnsArr[questionNumber] == AnsArr[questionNumber]) count++;
                }
                return count;
            }

            public void Print()
            {
                Console.WriteLine(_animal + " " + _characterTrait + " " + _concept);
            }
        }

        public struct Research
        {
            private string _name;
            private Response[] _responses;

            public string Name => _name;
            public Response[] Responses
            {
                get
                {
                    if (_responses == null) return null;

                    var responses = new Response[_responses.Length];
                    Array.Copy(_responses, responses, responses.Length);
                    return responses;
                }
            }

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0];
            }

            public void Add(string[] answers)
            {
                if (answers == null || _responses == null) return;

                string[] ans = new string[] { null, null, null };
                for (int i = 0; i < Math.Min(3, answers.Length); i++)
                    ans[i] = answers[i];

                var resp = new Response[_responses.Length + 1];
                Array.Copy(_responses, resp, resp.Length - 1);
                resp[resp.Length - 1] = new Response(ans[0], ans[1], ans[2]);
                _responses = resp;
            }

            public string[] GetTopResponses(int question)
            {
                if (_responses == null) return null;
                question--;
                int difAns = 0;
                for (int i = 0; i < _responses.Length; i++)
                {
                    bool newAns = true;
                    for (int j = 0; j < i; j++)
                    {
                        var ansArr = new string[] { _responses[i].Animal, _responses[i].CharacterTrait, _responses[i].Concept };
                        var ansArr2 = new string[] { _responses[j].Animal, _responses[j].CharacterTrait, _responses[j].Concept };
                        if (ansArr[question] == ansArr2[question])
                        {
                            newAns = false;
                            break;
                        }
                    }
                    if (newAns)
                    {
                        difAns++;
                    }
                }
                var answers = new Answer[difAns];
                foreach (var r in _responses)
                {
                    for (int i = 0; i < difAns; i++)
                    {
                        var ansArr = new string[] { r.Animal, r.CharacterTrait, r.Concept };
                        if (answers[i].Count == 0)
                        {
                            answers[i] = new Answer(ansArr[question]);
                            break;
                        }
                        if (answers[i].Value == ansArr[question])
                        {
                            answers[i].Inc();
                            break;
                        }
                    }
                }
                Array.Sort(answers, (a, b) =>
                {
                    return b.Count - a.Count;
                });

                int n = difAns;
                foreach (var a in answers)
                    if (a.Value == null)
                    {
                        n--;
                        break;
                    }
                string[] ans = new string[Math.Min(n, 5)];
                int step = 0;
                for (int i = 0; i < ans.Length; i++)
                {
                    if (answers[i].Value == null) step = 1;
                    ans[i] = answers[i + step].Value;
                }
                return ans;
            }

            public void Print()
            {
                Console.WriteLine(_name);
                foreach (var r in _responses)
                    r.Print();
            }
        }

        private struct Answer
        {
            private string _value;
            private int _count;

            public string Value => _value;
            public int Count => _count;

            public Answer(string value)
            {
                _value = value;
                _count = 1;
            }

            public void Inc()
            {
                _count++;
            }
        }

        public class Report
        {
            private static int indexer;
            public Research[] Researches { get; private set; }
            static Report()
            {
                indexer = 1;
            }
            public Report()
            {
                Researches = new Research[0];
            }

            public Research MakeResearch()
            {
                var res = new Research($"No_{indexer++}_{DateTime.Now.ToString("MM/yy", CultureInfo.InvariantCulture)}");
                Researches = Researches.Append(res).ToArray();
                return res;
            }

            public (string, double)[] GetGeneralReport(int question)
            {
                if (Researches == null || question < 1 || question > 3) return null;
                Research sumR = new Research("");
                foreach (var res in Researches)
                {
                    foreach (var resp in res.Responses)
                    {
                        sumR.Add(new string[] { resp.Animal, resp.CharacterTrait, resp.Concept });
                    }
                }

                var qCount = 0;
                foreach (var ans in sumR.Responses)
                {
                    string[] anwers = new string[] { ans.Animal, ans.CharacterTrait, ans.Concept };
                    if (anwers[question - 1] != null) qCount++;
                }

                if (qCount == 0) return new (string, double)[0];

                var cort = sumR.Responses.GroupBy(resp =>
                {
                    switch (question)
                    {
                        case 1:
                            return resp.Animal;
                        case 2:
                            return resp.CharacterTrait;
                        default:
                            return resp.Concept;
                    }
                }).Select(x => (x.Key.ToString(), x.Count() * 100.0 / qCount)).ToArray();
                return cort;
            }

            public void AddResearch(Research r)
            {
                if (Researches == null) return;
                Researches = Researches.Append(r).ToArray();
            }
            public void AddResearch(Research[] r)
            {
                if (r == null || Researches == null) return;
                Researches = Researches.Concat(r).ToArray();
            }
        }
    }
}
