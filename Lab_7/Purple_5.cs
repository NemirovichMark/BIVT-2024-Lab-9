using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Lab_7
{
    public class Purple_5
    {
        public struct Response {
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

                var curInstance = this;

                switch (questionNumber)
                {
                    case 1:
                        return responses.Count(r => r.Animal != null && r.Animal == curInstance.Animal);
                    case 2:
                        return responses.Count(r => r.CharacterTrait != null && r.CharacterTrait == curInstance.CharacterTrait);
                    case 3:
                        return responses.Count(r => r.Concept != null && r.Concept == curInstance.Concept);
                    default:
                        return 0;
                }
            }

            public void Print() {
                Console.WriteLine($"Animal: {_animal ?? "N/A"}");
                Console.WriteLine($"Character Trait: {_characterTrait ?? "N/A"}");
                Console.WriteLine($"Concept: {_concept ?? "N/A"}");
            }
        }

        public struct Research {
            private string _name;
            private Response[] _responses;

            public string Name => _name;
            public Response[] Responses => (_responses == null) ? null : (Response[])_responses.Clone();

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0];
            }

            public void Add(string[] answers)
            {
                if (_responses == null || answers == null) return;

                var responseAnimal = answers[0];
                var responseCharacterTrait = answers[1];
                var responseConcept = answers[2];

                var newResponse = new Response(responseAnimal, responseCharacterTrait, responseConcept);
                Array.Resize(ref _responses, _responses.Length + 1);
                _responses[_responses.Length - 1] = newResponse;    
            }

            public string[] GetTopResponses(int question) {
                if (_responses == null) return null;

                switch (question)
                {
                    case 1:
                        return _responses.GroupBy(r => r.Animal)
                                .Where(r => r.Key != null && r.Key.Length > 0)
                                .OrderByDescending(r => r.Count())
                                .Take(5)
                                .Select(r => r.Key)
                                .ToArray();
                    case 2:
                        return _responses.GroupBy(r => r.CharacterTrait)
                                .Where(r => r.Key != null && r.Key.Length > 0)
                                .OrderByDescending(r => r.Count())
                                .Take(5)
                                .Select(r => r.Key)
                                .ToArray();
                    case 3:
                        return _responses.GroupBy(r => r.Concept)
                                .Where(r => r.Key != null && r.Key.Length > 0)
                                .OrderByDescending(r => r.Count())
                                .Take(5)
                                .Select(r => r.Key)
                                .ToArray();
                    default:
                        return null;
                }
            }
            
            private void PrintArray(Response[] array, string label) {
                if (array == null) {
                    Console.WriteLine($"{label} N/A");
                    return;
                }

                Console.WriteLine(label);
                for (int i = 0; i < array.Length; i++) {
                    Console.WriteLine($"{i + 1} response:");
                    array[i].Print();
                    Console.WriteLine();
                }
            }
            public void Print() {
                Console.WriteLine($"Name: {_name ?? "N/A"}");
                PrintArray(_responses, "Responses:");
            }
        }

        public class Report {
            private Research[] _researches; 
            private static int IDCounter;
            private int ID;
            public Research[] Researches => (_researches == null) ? _researches : (Research[])_researches.Clone();

            static Report() {
                IDCounter = 1;
            }

            public Report() {
                ID = IDCounter++;
                _researches = new Research[0]; // what if null needed?
            }

            public Research MakeResearch() {
                var MM = DateTime.Now.ToString("MM");
                var YY = DateTime.Now.ToString("yy");

                var newResearch = new Research($"No_{ID}_{MM}/{YY}");
                
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[^1] = newResearch;

                return newResearch;
            }

            private string Property(Response response, int question) {
                    switch (question) {
                        case 1: return response.Animal;
                        case 2: return response.CharacterTrait;
                        case 3: return response.Concept;
                        default: return null;
                    }   
            }

            public (string, double)[] GetGeneralReport(int question) {
                if (question < 1 || question > 3) return null;

                var flattenedResponses = _researches.SelectMany(rsrch => rsrch.Responses);

                var targetResponses = flattenedResponses.Where(rsp => Property(rsp, question) != null);
                int targetResponsesCount = targetResponses.Count();

                var groupedResponses = targetResponses.GroupBy(rsp => Property(rsp, question));

                return groupedResponses.Select(g => (g.Key, (double) g.Count() / targetResponsesCount * 100)).ToArray();
            }

            public void AddResearch(Research newResearch) {
                if (_researches == null) return;
                Array.Resize(ref _researches, _researches.Length + 1);
                _researches[^1] = newResearch;
            }

            public void AddResearch(Research[] newResearches) {
                if (_researches == null || newResearches == null) return;
                
                for (int i = 0; i < newResearches.Length; i++)
                    AddResearch(newResearches[i]);
            }
        }   
    }
}
