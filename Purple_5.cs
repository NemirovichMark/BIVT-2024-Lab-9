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
            private string _charactertrait;
            private string _concept;

            public string Animal => _animal;
            public string CharacterTrait => _charactertrait;
            public string Concept => _concept;

            private string[] Massive_of_answers // dop
            {
                get
                {
                    return new string[3] { _animal, _charactertrait, _concept };
                }                          // 0 == 1     1 == 2                 2 == 3
            }

            public Response(string animal, string charactertrait, string concept)
            {
                _animal = animal; _charactertrait = charactertrait; _concept = concept;
            }

            public int CountVotes(Response[] responses, int questionNumber)
            {
                if (responses == null || questionNumber < 1 || questionNumber > 3) return default(int);
                int count_question = 0;
                int k = questionNumber;
                k--;
                if (Massive_of_answers[k] == null) return default(int);
                foreach (Response response in responses)
                {
                    if (response.Massive_of_answers[k] == Massive_of_answers[k]) count_question++;
                }
                return count_question;
            }

            public void Print()
            {
                Console.WriteLine($"{_animal} {_charactertrait} {_concept}");
            }


        }
        public struct Research
        {
            private string _name; private Response[] _responses;

            public string Name => _name;
            public Response[] Responses
            {
                get
                {
                    if (_responses == null) return default;

                    Response[] copy_of_answers = new Response[_responses.Length];
                    Array.Copy(_responses, copy_of_answers, _responses.Length);
                    return copy_of_answers;
                }
            }

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0] { };
            }
            public void Add(string[] answers)
            {
                if (answers == null || _responses == null) return;

                string[] new_answer = new string[3];

                for (int i = 0; i < Math.Min(answers.Length, 3); i++)
                {
                    new_answer[i] = answers[i];
                }

                Response[] responses_1 = new Response[_responses.Length + 1];
                Array.Copy(_responses, responses_1, _responses.Length);
                responses_1[responses_1.Length - 1] = new Response(new_answer[0], new_answer[1], new_answer[2]);
                // animal        char             conc
                _responses = responses_1;
            }

            public string[] GetTopResponses(int question)
            {
                if (_responses == null) return default;

                int question_num = question;
                question_num -= 1;
                int differ_question = 0;

                for (int i = 0; i < _responses.Length; i++)
                {
                    int notrepeated = 0;
                    for (int j = 0; j < i; j++)
                    {
                        string[] xz_kakoy_massive_answer_number = new string[] { _responses[i].Animal, _responses[i].CharacterTrait, _responses[i].Concept };
                        string[] xz_kakoy_massive_answer_number2 = new string[] { _responses[j].Animal, _responses[j].CharacterTrait, _responses[j].Concept };

                        if (xz_kakoy_massive_answer_number[question_num] == xz_kakoy_massive_answer_number2[question_num])
                        {
                            notrepeated++;
                        }
                    }
                    differ_question = notrepeated == 0 ? differ_question += 1 : differ_question;
                }


                Resulting[] result_of_research = new Resulting[differ_question];

                for (int i = 0; i < _responses.Length; i++)
                {
                    var response = _responses[i];

                    for (int j = 0; j < differ_question; j++)
                    {
                        string[] massive_of_answers = new string[] { response.Animal, response.CharacterTrait, response.Concept };

                        if (result_of_research[j].Count == 0)
                        {
                            result_of_research[j] = new Resulting(massive_of_answers[question_num]);
                            break;
                        }
                        else if (result_of_research[j].Meaning == massive_of_answers[question_num])
                        {
                            result_of_research[j].Increasing_of_words();
                            break;
                        }
                    }
                }

                Array.Sort(result_of_research, (a, b) => { return b.Count - a.Count; });

                int not_empty = differ_question;
                foreach (var result in result_of_research)
                {
                    if (result.Meaning == null)
                    {
                        not_empty -= 1;
                        break;
                    }
                }
                string[] res = new string[Math.Min(5, not_empty)];
                int tool = 0;
                for (int i = 0; i < res.Length; i++)
                {
                    if (result_of_research[i].Meaning == null) { tool = 1; }
                    ;
                    res[i] = result_of_research[i + tool].Meaning;
                }
                return res;
            }

            private struct Resulting
            {
                private string _meaning;
                private int _count;

                public string Meaning => _meaning;
                public int Count => _count;

                public Resulting(string word)
                {
                    _meaning = word;
                    _count = 1;
                }

                public void Increasing_of_words()
                {
                    _count++;
                }
            }

            public void Print()
            {
                Console.WriteLine(_name);
                for (int i = 0; i < _responses.Length; i++)
                {
                    _responses[i].Print();
                }
            }
        }
        public class Report
        {
            private Research[] _researches;
            private static int _numeration;
            public Research[] Researches => _researches;

            // not sure enough:

            static Report()
            {
                _numeration = 1;
            }
            public Report()
            {
                _researches = new Research[0];
            }

            public void AddResearch(Research research) // NEW METHOD
            {
                if (research.Responses == null) return;
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[_researches.Length - 1] = research;
            }
            public void AddResearch(Research[] researchings) // NEW METHOD
            {
                if (researchings == null) return;
                foreach (var one_res in researchings)
                {
                    AddResearch(one_res);
                }
            }
            public Research MakeResearch() // No_X_MM/YY
            {
                if (_researches == null) return default;
                var NEWresearch = new Research($"No_{_numeration++}_{DateTime.Now.ToString("MM/yy", CultureInfo.InvariantCulture)}");
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[_researches.Length - 1] = NEWresearch;
                return NEWresearch;
            }

            public (string, double)[] GetGeneralReport(int question)
            {
                if (_researches == null || _researches.Length == 0 || question < 1 || question > 3) return null;

                string[] answer = new string[0];

                foreach (var researche in _researches)
                {
                    foreach (var response in researche.Responses)
                    {
                        {
                            if (question == 1)
                            {
                                if (response.Animal != null)
                                {
                                    Array.Resize(ref answer, answer.Length + 1);
                                    answer[answer.Length - 1] = response.Animal;
                                }
                            }
                            else if (question == 2)
                            {
                                if (response.CharacterTrait != null)
                                {
                                    Array.Resize(ref answer, answer.Length + 1);
                                    answer[answer.Length - 1] = response.CharacterTrait;
                                }
                            }
                            else if (question == 3)
                            {
                                if (response.Concept != null)
                                {
                                    Array.Resize(ref answer, answer.Length + 1);
                                    answer[answer.Length - 1] = response.Concept;
                                }
                            }

                        }
                    }
                }

                /*cortage*/
                (string, double)[] result = answer
                    .GroupBy(k => k)
                    .Select(r => (r.Key, (double)(r.Count() * 100) / answer.Length))
                    .ToArray();
                return result;
            }
        }
    }
}