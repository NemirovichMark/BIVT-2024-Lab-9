using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Remoting.Contexts;
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

            public Response(string animal, string characterTrait, string concept)
            {
                _animal = animal;
                _characterTrait = characterTrait;
                _concept = concept;
            }

            public int CountVotes(Response[] responses, int questionNumber)
            {
                if (responses == null) return 0;
                int res = 0;
                foreach (Response x in responses)
                {
                    if (questionNumber == 1)
                    {
                        if (x.Animal != null && _animal == x.Animal) res++;
                    }
                    else if (questionNumber == 2 && x.CharacterTrait != null && CharacterTrait == x.CharacterTrait) res++;
                    else if (questionNumber == 3 && x.Concept != null && Concept == x.Concept) res++;
                }
                return res;
            }
            public void Print()
            {
                Console.Write($"{Animal,10} {CharacterTrait,10} {Concept,10} ");

            }

        }
        public struct Research
        {
            private string _name;
            private Response[] _responses;

            public string Name => _name;
            public Response[] Responses => _responses;

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0];
            }

            public void Add(string[] answers)
            {
                if (_responses == null || answers == null || answers.Length < 3) return;
                Response[] responses = new Response[_responses.Length + 1];
                Array.Copy(_responses, responses, _responses.Length);
                responses[responses.Length - 1] = new Response(answers[0], answers[1], answers[2]);

                _responses = responses;
            }

            public string[] GetTopResponses(int question)
            {
                if (_responses == null || _responses.Length == 0) return null;
                string[] arr = new string[_responses.Length];
                int[] count = new int[arr.Length];
                if (question == 1)
                {

                    arr[0] = _responses[0].Animal == null ? "" : _responses[0].Animal;
                    count[0] = 1;
                    for (int i = 1, curr = 1; i < arr.Length; i++)
                    {
                        string s = _responses[i].Animal == null ? "" : _responses[i].Animal;
                        int j;
                        for (j = 0; j < curr; j++)
                        {
                            if (arr[j] == s)
                            {
                                break;
                            }
                        }
                        if (j == curr)
                        {
                            arr[curr++] = s;
                        }
                        count[j]++;
                    }
                }
                else if (question == 2)
                {

                    arr[0] = _responses[0].CharacterTrait == null ? "" : _responses[0].CharacterTrait;
                    count[0] = 1;
                    for (int i = 1, curr = 1; i < arr.Length; i++)
                    {
                        string s = _responses[i].CharacterTrait == null ? "" : _responses[i].CharacterTrait;
                        int j;
                        for (j = 0; j < curr; j++)
                        {
                            if (arr[j] == s)
                            {
                                break;
                            }
                        }
                        if (j == curr)
                        {
                            arr[curr++] = s;
                        }
                        count[j]++;
                    }
                }

                else if (question == 3)
                {

                    arr[0] = _responses[0].Concept == null ? "" : _responses[0].Concept;
                    count[0] = 1;
                    for (int i = 1, curr = 1; i < arr.Length; i++)
                    {
                        string s = _responses[i].Concept == null ? "" : _responses[i].Concept;
                        int j;
                        for (j = 0; j < curr; j++)
                        {
                            if (arr[j] == s)
                            {
                                break;
                            }
                        }
                        if (j == curr)
                        {
                            arr[curr++] = s;
                        }
                        count[j]++;
                    }
                }
                for (int i = 1, j = 2; i < arr.Length;)
                {
                    if (i == 0 || count[i] <= count[i - 1])
                    {
                        i = j; j++;
                    }
                    else
                    {
                        (count[i], count[i - 1]) = (count[i - 1], count[i]);
                        (arr[i], arr[i - 1]) = (arr[i - 1], arr[i]);
                        i--;
                    }
                }
                int c;
                for (c = 0; c < Math.Min(6, arr.Length); c++)
                {
                    if (count[c] == 0) break;
                    if (arr[c] == "" || arr[c] == "-")
                    {
                        for (int i = c; i < Math.Min(6, arr.Length); i++)
                        {
                            arr[i] = arr[i + 1];
                            count[i] = count[i + 1];
                        }
                    }
                }
                string[] result = new string[Math.Min(c, 5)];
                Array.Copy(arr, result, Math.Min(c, 5));
                return result;

            }
            public void Print()
            {
                foreach (string s in GetTopResponses(1))
                {
                    foreach (Response x in Responses)
                    {
                        if (x.Animal == s)
                        {
                            x.Print();
                            Console.WriteLine(x.CountVotes(Responses, 1));
                            break;
                        }
                    }
                }
                Console.WriteLine("Character:");
                foreach (string s in GetTopResponses(2))
                {
                    foreach (Response x in Responses)
                    {
                        if (x.CharacterTrait == s)
                        {
                            x.Print();
                            Console.WriteLine(x.CountVotes(Responses, 2));
                            break;
                        }
                    }
                }
                Console.WriteLine("Concept:");
                foreach (string s in GetTopResponses(3))
                {
                    foreach (Response x in Responses)
                    {
                        if (x.Concept == s)
                        {
                            x.Print();
                            Console.WriteLine(x.CountVotes(Responses, 3));
                            break;
                        }
                    }
                }
            }
        }
        public class Report
        {
            private Research[] _researches;
            private static int _id;

            public Research[] Researches => _researches;
            static Report()
            {
                _id = 1;
            }
            public Report()
            {
                _researches = new Research[0];
            }
            public Research MakeResearch()
            {
                string s = $"No_{_id++}_{DateTime.Now.ToString("MM")}/{DateTime.Now.ToString("yy")}";
                Research ans = new Research(s);
                var copy = new Research[_researches.Length + 1];
                Array.Copy(_researches, copy, _researches.Length);
                copy[copy.Length - 1] = ans;
                _researches = copy;
                return ans;
            }

            public (string, double)[] GetGeneralReport(int question)
            {
                if (question < 1 || question > 3) return null;
                var research = new Research("res");
                foreach (var x in _researches)
                {
                    if (x.Responses != null)
                    {
                        foreach (var y in x.Responses)
                        {
                            research.Add(new string[] { y.Animal, y.CharacterTrait, y.Concept });
                        }
                    }
                }
                string[] answers = new string[research.Responses.Length];
                double[] counts = new double[research.Responses.Length];
                int c = 0, i = 0, sum = 0;
                foreach (var x in research.Responses)
                {
                    string s = "";
                    switch (question)
                    {
                        case 1: s = x.Animal; break;
                        case 2: s = x.CharacterTrait; break;
                        default: s = x.Concept; break;

                    }
                    if (s == null || s == "" || s == "-") continue;
                    sum++;
                    for (i = 0; i < c; i++)
                    {
                        if (s == answers[i]) break;
                    }
                    if (i == c) answers[c++] = s;
                    counts[i]++;
                }
                (string, double)[] ans = new (string, double)[c];
                for (i = 0; i < c; i++)
                {
                    ans[i] = (answers[i], counts[i] * 100.0 / sum);
                }

                return ans;
            }
            public void AddResearch(Research res)
            {
                if (_researches == null) return;
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[_researches.Length - 1] = res;
            }
            public void AddResearch(Research[] res)
            {
                if (_researches == null || res == null) return;
                Array.Resize(ref _researches, _researches.Length + res.Length);
                Array.Copy(res, 0, _researches, _researches.Length - res.Length, res.Length);

            }
        }

    }
}

