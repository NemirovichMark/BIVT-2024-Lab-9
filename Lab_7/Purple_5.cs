using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

                int result = 0;
                foreach (Response response in responses)
                {
                    switch (questionNumber)
                    {
                        case 1:
                            if (response.Animal != null && response.Animal.Length > 0 && response.Animal == _animal)
                                result++;
                            break;
                        case 2:
                            if (response.CharacterTrait != null && response.CharacterTrait.Length > 0 && response.CharacterTrait == _characterTrait)
                                result++;
                            break;
                        case 3:
                            if (response.Concept != null && response.Concept.Length > 0 && response.Concept == _concept)
                                result++;
                            break;
                    }
                }
                return result;
            }
            public void Print()
            {
                Console.WriteLine(Animal);
                Console.WriteLine(CharacterTrait);
                Console.WriteLine(Concept);
                Console.WriteLine();
            }

        }
        public class Research
        {
            private string _name;
            private Response[] _responses;
            private int[] _data;

            public string Name => _name;
            public Response[] Responses
            {
                get
                {
                    if (_responses == null) return null;

                    Response[] responsesCopy = new Response[_responses.Length];
                    Array.Copy(_responses, responsesCopy, _responses.Length);
                    return responsesCopy;
                }
            }

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0];
            }

            public void Add(string[] answers)
            {
                if (answers == null) return;
                string animal = answers[0];
                string characterTrait = answers[1];
                string concept = answers[2];

                Response response = new Response(animal, characterTrait, concept);
                Array.Resize(ref _responses, _responses.Length + 1);
                _responses[_responses.Length - 1] = response;
            }
            public string[] GetTopResponses(int question)
            {
                string[] answers = new string[_responses.Length];

                for (int i = 0; i < _responses.Length; i++)
                {
                    Response response = _responses[i];
                    switch (question)
                    {
                        case 1:
                            answers[i] = response.Animal;
                            break;
                        case 2:
                            answers[i] = response.CharacterTrait;
                            break;
                        case 3:
                            answers[i] = response.Concept;
                            break;
                    }
                }

                string[] uniqueAnswers = _uniqueAnswers(answers);
                int[] countAnswers = _calculateCountAnswers(answers, uniqueAnswers);
                _bubbleSort(countAnswers, uniqueAnswers);
                int lenNeed = 5;
                if (uniqueAnswers.Length > lenNeed)
                {
                    Array.Resize(ref uniqueAnswers, lenNeed);
                    Array.Resize(ref countAnswers, lenNeed);
                }
                _data = countAnswers;
                return uniqueAnswers;
            }

            private int[] _calculateCountAnswers(string[] answers, string[] uniqueAnswers)
            {
                int[] result = new int[uniqueAnswers.Length];

                for (int i = 0; i < uniqueAnswers.Length; i++)
                    for (int j = 0; j < answers.Length; j++)
                        if (answers[j] == uniqueAnswers[i]) result[i] += 1;

                return result;
            }
            private string[] _uniqueAnswers(string[] answers)
            {
                string[] result = new string[answers.Length];

                int index = 0;
                foreach (string answer in answers)
                    if (!result.Contains(answer) && answer != null && answer != "") result[index++] = answer;

                Array.Resize(ref result, index);

                return result;
            }
            private void _bubbleSort(int[] array1, string[] array2)
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    for (int j = 0; j < array2.Length - i - 1; j++)
                    {
                        if (array1[j] < array1[j + 1])
                        {
                            int p1 = array1[j];
                            array1[j] = array1[j + 1];
                            array1[j + 1] = p1;

                            string p2 = array2[j];
                            array2[j] = array2[j + 1];
                            array2[j + 1] = p2;
                        }
                    }
                }
            }
            private static void _printHead(int columnIndex)
            {
                string[] columns = new string[] { "Animal", "Character trait", "Concept" };
                Console.WriteLine(_printItem(columns[columnIndex]) + _printItem("Amount"));
            }
            private static string _printItem(string item)
            {
                return item + new string(' ', 20 - item.Length);
            }
            public void Print()
            {
                for (int i = 0; i < 3; i++)
                {
                    _printHead(i);

                    string[] result = GetTopResponses(i + 1);
                    for (int j = 0; j < result.Length; j++)
                        Console.WriteLine(_printItem(result[j]) + _printItem(_data[j].ToString()));
                    Console.WriteLine();
                }
            }
        }
        public class Report
        {
            private Research[] _researches;
            private static int _countReportes;

            public Research[] Researches => _researches == null ? null : (Research[])_researches.Clone();

            static Report() { _countReportes = 1; }
            public Report() { _researches = new Research[0]; }

            public Research MakeResearch()
            {
                DateTime now = DateTime.Today;
                int currentMonth = now.Month;
                int yearLastTwoDigits = now.Year % 100;

                Research research = new Research($"No_{_countReportes}_{currentMonth}/{yearLastTwoDigits}");
                _countReportes++;

                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[^1] = research;
                return research;
            }
            private static string _GetAnswer(Response response, int question)
            {
                switch (question)
                {
                    case 1: return response.Animal;
                    case 2: return response.CharacterTrait;
                    case 3: return response.Concept;
                    default: return string.Empty;
                }
            }
            public (string, double)[] GetGeneralReport(int question)
            {
                if (question < 1 || question > 3) return null;
                Response[] responses = new Response[0];
                foreach (Research research in _researches)
                {
                    responses = responses.Concat(research.Responses).ToArray();
                }

                (string, double)[] dataAnswers = new (string, double)[responses.Length];
                int currentIndex = 0;
                int countAllAnswers = 0;

                foreach (Response response in responses)
                {
                    string answer = _GetAnswer(response, question);

                    if (answer.Length > 0 && !dataAnswers.Any(x => x.Item1 == answer))
                    {
                        int countAnswers = response.CountVotes(responses, question);

                        countAllAnswers += countAnswers;
                        dataAnswers[currentIndex] = (answer, countAnswers);
                        currentIndex++;
                    }
                }

                Array.Resize(ref dataAnswers, currentIndex);

                for (int i = 0; i < dataAnswers.Length; i++)
                {
                    dataAnswers[i].Item2 /= countAllAnswers / 100.0;
                }
                return dataAnswers;
            }
            public void AddResearch(Research research)
            {
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[^1] = research;
            }
            public void AddResearch(Research[] researches)
            {
                if (researches == null) return;

                for (int i = 0; i < researches.Length;i++) AddResearch(researches[i]);
            }
        }
    }
}
