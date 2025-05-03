using System;
using System.Linq;

namespace Lab_7 {
    public class Purple_5 {
        public struct Response {
            public Response(string animal, string characterTrait, string concept) {
                _animal = animal;
                _characterTrait = characterTrait;
                _concept = concept;
            }

            private string _animal;
            private string _characterTrait;
            private string _concept;

            public string Animal => _animal;
            public string CharacterTrait => _characterTrait;
            public string Concept => _concept;

            public int CountVotes(Response[] responses, int questionNumber) {
                if (responses == null || !(questionNumber >= 1 && questionNumber <= 3)) return 0;

                switch (questionNumber) {
                    case 1:
                        string question = _animal;
                        return responses.Count(resp => resp.Animal == question && question != null);
                    case 2:
                        string question2 = _characterTrait;
                        return responses.Count(resp => resp.CharacterTrait == question2 && question2 != null);
                    case 3:
                        string question3 = _concept;
                        return responses.Count(resp => resp.Concept == question3 && question3 != null);
                    default:
                        return 0;
                }
            }

            public void Print() {
                Console.WriteLine($"{_animal, 15} {_characterTrait, 15} {_concept, 15}");
            }
        }

        public struct Research {
            public Research(string name) {
                _name = name;
                _responses = new Response[0];
            }

            private string _name;
            private Response[] _responses;

            public string Name => _name;
            public Response[] Responses {
                get {
                    // var copy = new Response[_responses.Length];
                    // Array.Copy(_responses, copy, _responses.Length);
                    // return copy;
                    return _responses;
                }
            }

            public void Add(string[] answers) {
                if (_responses == null || answers == null || answers.Length <= 2) return;

                var resp = new Response(answers[0], answers[1], answers[2]);
                Array.Resize(ref _responses, _responses.Length + 1);
                _responses[_responses.Length - 1] = resp;
            }

            public string[] GetTopResponses(int question) {
                if (_responses == null || _responses.Length == 0  || !(question >= 1 && question <= 3)) return new string[0];
                
                string[] questionResponses = new string[_responses.Length];

                int counter = 0;

                for (int i = 0; i < _responses.Length; i++) {
                    string answer = Research.GetQuestionByNumber(_responses[i], question);
                    if (answer == null) continue;

                    questionResponses[counter++] = answer;
                }

                Array.Resize(ref questionResponses, counter);

                string[] uniqueResponses = questionResponses.Distinct().ToArray();

                int[] everyQuestionCounters = new int[uniqueResponses.Length]; // массив для замены словаря вопрос -> количество

                for (int i = 0; i < everyQuestionCounters.Length; i++) {
                    int c = 0;
                    for (int j = 0; j < questionResponses.Length; j++) {
                        if (questionResponses[j] == uniqueResponses[i]) c++;
                    }
                    everyQuestionCounters[i] = c;
                }

                for (int i = 1; i < everyQuestionCounters.Length; i++) {
                    int key = everyQuestionCounters[i];
                    string keyStr = uniqueResponses[i];
                    int j = i - 1;
                    
                    while (j >= 0 && everyQuestionCounters[j] < key)
                    {
                        everyQuestionCounters[j + 1] = everyQuestionCounters[j];
                        uniqueResponses[j + 1] = uniqueResponses[j];
                        j--;
                    }
                    
                    everyQuestionCounters[j + 1] = key;
                    uniqueResponses[j + 1] = keyStr;
                }

                if (uniqueResponses.Length > 5)
                    Array.Resize(ref uniqueResponses, 5);

                return uniqueResponses;
            }

            private static string GetQuestionByNumber(Response resp, int questionNumber) {
                switch (questionNumber) {
                    case 1:
                        return resp.Animal;
                    case 2:
                        return resp.CharacterTrait;
                    case 3:
                        return resp.Concept;
                    default:
                        return null;
                }
            }

            private int GetAnswerCount(int question, string answer) {
                if (answer == null || _responses == null || !(question >= 1 && question <=3)) return 0;

                int res = 0;

                foreach (var resp in _responses) {
                    if (Research.GetQuestionByNumber(resp, question) == answer) res++;
                }

                return res;

            }

            public void Print() {
                if (_responses == null) return;
                
                for (int i = 1; i <= 3; i++) {
                    string[] topResp = GetTopResponses(i);
                    if (topResp == null) continue;

                    for (int j = 0; j < topResp.Length; j++) {
                        Console.Write($"{topResp[j]}\t");
                        Console.WriteLine(GetAnswerCount(i, topResp[j]));
                    }
                    Console.WriteLine();
                }
            }
        }
    
        public class Report {
            public Report() {
                _ID = _IDcounter++;
                _researches = new Research[0];
            }

            static Report() {
                _IDcounter = 1;
            }

            private static int _IDcounter;
            private int _ID;
            private Research[] _researches;

            // the same as in struct Research
            private static string GetQuestionByNumber(Response resp, int questionNumber) {
                switch (questionNumber) {
                    case 1:
                        return resp.Animal;
                    case 2:
                        return resp.CharacterTrait;
                    case 3:
                        return resp.Concept;
                    default:
                        return null;
                }
            }

            public Research[] Researches => _researches;

            public Research MakeResearch() {
                Research newResearch = new Research($"No_{_ID}_{DateTime.Now.ToString("MM")}/{DateTime.Now.ToString("yy")}");   

                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[_researches.Length - 1] = newResearch;

                return newResearch;
            }

            public void AddResearch(Research research)
            {
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[_researches.Length - 1] = research;
            }

            public void AddResearch(Research[] researches)
            {
                foreach (var research in researches)
                {
                    this.AddResearch(research);
                }
            }

            public (string, double)[] GetGeneralReport(int question) {
                if (!(question >= 1 && question <= 3)) return null;

                Response[] allResponses = _researches.SelectMany(research => research.Responses)
                                                    .Where(response => Report.GetQuestionByNumber(response, question) != null).ToArray();

                int counter = allResponses.Count();

                return allResponses.GroupBy(response => Report.GetQuestionByNumber(response, question))
                                    .Select(group => (group.Key, (double) (group.Count() * 100) / counter)).ToArray();
            }
        }
    }
}
