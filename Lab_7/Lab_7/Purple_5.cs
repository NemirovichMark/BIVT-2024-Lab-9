using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
                if (responses == null || questionNumber > 3 || questionNumber < 1) return 0;
                string elem = (questionNumber == 1 ? _animal : questionNumber == 2 ? _characterTrait : _concept);
                if(elem == null) return 0;
                return responses.Count(r => 
                    (questionNumber == 1 ? r.Animal : questionNumber == 2 ? r.CharacterTrait : r.Concept) == elem);
            }

            public void Print()
            {
                Console.WriteLine($"Animal: {_animal}");
                Console.WriteLine($"Trait: {_characterTrait}");
                Console.WriteLine($"Concept: {_concept}\n");
            }
        }

        public struct Research
        {
            private string _name;
            private Response[] _responses;

            public string Name => _name;

            // [JsonProperty]
            public Response[] Responses
            {
                get
                {
                    if (_responses == null) return null;
                    Response[] responses = new Response[_responses.Length];
                    Array.Copy(_responses, responses, _responses.Length);
                    return responses;
                }
                private set
                {
                    if (value == null) _responses = null;
                    else _responses = (Response[])value.Clone();
                }
            }

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0];
            }

            public void Add(string[] answers)
            {
                if (answers == null || _responses == null || answers.Length < 3) return;
                Array.Resize(ref _responses, _responses.Length + 1);
                _responses[_responses.Length - 1] = new Response(answers[0], answers[1], answers[2]);
            }

            public string[] GetTopResponses(int question)
            {
                if (_responses == null || _responses.Length == 0 || question > 3 || question < 1) return null;
                return _responses.GroupBy(r => question == 1 ? r.Animal : question == 2 ? r.CharacterTrait : r.Concept)
                    .Where(r => r.Key != null && r.Key.Length > 0)
                    .OrderByDescending(r => r.Count()).Take(5)
                    .Select(r => r.Key).ToArray();
            }

            public void Print()
            {
                Console.WriteLine($"Name: {_name}");
                if (_responses == null) return;
                Console.WriteLine($"Answers:");
                foreach (Response response in _responses) response.Print();
                Console.WriteLine();
            }

            // странно, что нигде в задании не взаимодействуется с долей ответов в %, которые просится в том числе найти,
            // нигде она не хранится, поэтому если что вывел тут, в формате выходных данных как приводится в примере
            // public static void PrintTable(Research research, int question)
            // {
            //     if (question > 3 || question < 1) return;
            //     Console.WriteLine("Animal\tTrait\tConcept\tAmount\tShare");
            //     string[] topAnswers = research.GetTopResponses(question);
            //     var selectedResponses = topAnswers
            //         .Select(elem => research.Responses.FirstOrDefault(r => 
            //             (question == 1 ? r.Animal : question == 2 ? r.CharacterTrait : r.Concept) == elem))
            //         .ToArray();
            //     foreach (Response response in selectedResponses)
            //     {
            //         int votes = response.CountVotes(research.Responses, question);
            //         Console.WriteLine($"{response.Animal}\t{response.CharacterTrait}\t{response.Concept}" +
            //                           $"\t{votes}\t{Math.Round(100.0 * votes / research.Responses.Length, 2)}");
            //     }
            //     Console.WriteLine();
            // }
        }
        
        public class Report {
            private Research[] _researches;
            
            private static int _counter;
            private int _id;
            
            // [JsonProperty]
            public Research[] Researches
            {
                get
                {
                    if (_researches == null) return null;
                    Research[] researches = new Research[_researches.Length];
                    Array.Copy(_researches, researches, _researches.Length);
                    return researches;
                }
                private set
                {
                    if(_researches == null) _researches = value;
                    else _researches = (Research[])value.Clone();
                }
            }

            static Report()
            {
                _counter = 1;
            }

            public Report()
            {
                _researches = new Research[0];
                _id = _counter++;
            }
            
            public void AddResearch(Research research)
            {
                if (_researches == null) return;
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[_researches.Length - 1] = research;
            }
            public void AddResearch(Research[] researches)
            {
                if (researches == null || _researches == null) return;
                int n = _researches.Length;
                Array.Resize(ref _researches, n + researches.Length);
                Array.Copy(researches, 0, _researches, n, researches.Length);
            }

            public Research MakeResearch()
            {
                string month = DateTime.Now.ToString("MM");
                string year = DateTime.Now.ToString("yy");
                Research research = new Research($"No_{_id}_{month}/{year}");
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[_researches.Length - 1] = research;
                return research;
            }

            public (string, double)[] GetGeneralReport(int question) {
                if (_researches == null || question > 3 || question < 1) return null;
                
                IEnumerable<Response> responses = _researches
                    .SelectMany(r => r.Responses)
                    .Where(r => (question == 1 ? r.Animal : question == 2 ? r.CharacterTrait : r.Concept) != null);
                IEnumerable<IGrouping<string,Response>> groupedResponses = 
                    responses.GroupBy(r => question == 1 ? r.Animal : question == 2 ? r.CharacterTrait : r.Concept);
                
                return groupedResponses.Select(r => (r.Key, 100.0 * r.Count() / responses.Count())).ToArray();
            }
        }
    }
}