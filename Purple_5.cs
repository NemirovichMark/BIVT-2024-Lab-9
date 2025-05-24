using System;
using System.Collections.Generic;
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

            public Response(string animal, string characterTrait, string concept)
            {
                _animal = animal;
                _characterTrait = characterTrait;
                _concept = concept;
            }

            public int CountVotes(Response[] responses, int questionNumber)
            {
                if (responses == null || questionNumber < 1 || questionNumber > 3) return 0;
                int count = 0;
                for (int i = 0; i < responses.Length; i++)
                {
                    switch (questionNumber)
                    {
                        case 1:
                            if (responses[i].Animal != null && responses[i].Animal == _animal)
                                count++;
                            break;
                        case 2:
                            if (responses[i].CharacterTrait != null && responses[i].CharacterTrait == _characterTrait)
                                count++;
                            break;
                        case 3:
                            if (responses[i].Concept != null && responses[i].Concept == _concept)
                                count++;
                            break;
                    }
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
            public Response[] Responses => _responses;

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0];
            }

            public void Add(string[] answers)
            {
                if (answers == null || _responses == null) return;
                Array.Resize(ref _responses, _responses.Length + 1);
                _responses[_responses.Length - 1] = new Response(answers[0], answers[1], answers[2]);
            }


            public string[] GetTopResponses(int question)
            {
                if (_responses == null) { return null; }
                switch (question)
                {
                    case 1: return _responses.GroupBy(x => x.Animal).Where(x => x.Key != null && x.Key.Length != 0).OrderByDescending(x => x.Count()).Take(5).Select(x => x.Key).ToArray();

                    case 2: return _responses.GroupBy(x => x.CharacterTrait).Where(x => x.Key != null && x.Key.Length != 0).OrderByDescending(x => x.Count()).Take(5).Select(x => x.Key).ToArray();

                    case 3: return _responses.GroupBy(x => x.Concept).Where(x => x.Key != null && x.Key.Length != 0).OrderByDescending(x => x.Count()).Take(5).Select(x => x.Key).ToArray();

                }

                return null;

            }

            public void Print()
            {
                Console.WriteLine(_name);
                foreach (var response in _responses)
                {
                    response.Print();
                }
            }
        }
        public class Report
        {
            private Research[] _researches;
            private static int _nextID;

            public Research[] Researches => _researches;

            static Report()
            {
                _nextID = 1;
            }

            public Report()
            {
                _researches = new Research[0];
            }

            public Research MakeResearch()
            {
                string today = DateTime.Today.ToString();
                string MM = today.Substring(5, 2);
                string YY = today.Substring(2, 2);
                Research research = new Research($"No_{_nextID++}_{MM}/{YY}");
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[_researches.Length - 1] = research;
                return research;
            }
            public void AddResearch(Research research)
            {
                if (research.Equals(default(Research))) return;
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[^1] = research;
            }

            public void AddResearch(Research[] researches)
            {
                if (researches == null) return;
                foreach (var r in researches) AddResearch(r);
            }
            private string Property(Response response, int question)
            {
                switch (question)
                {
                    case 1: return response.Animal;
                    case 2: return response.CharacterTrait;
                    case 3: return response.Concept;
                    default: return null;
                }
            }

            public (string, double)[] GetGeneralReport(int question)
            {
                if (question < 1 || question > 3) return null;

                var flattenedResponses = _researches.SelectMany(rsrch => rsrch.Responses);
                var targetResponses = flattenedResponses.Where(rsp => Property(rsp, question) != null);
                int targetResponsesCount = targetResponses.Count();
                var groupedResponses = targetResponses.GroupBy(rsp => Property(rsp, question));

                return groupedResponses.Select(g => (g.Key, (double)g.Count() / targetResponsesCount * 100)).ToArray();
            }

        }

    }
}