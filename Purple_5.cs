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
			//поля
			private string _animal;
			private string _character_trait;
			private string _concept;

			//свойства
			public string Animal => _animal;
			public string CharacterTrait => _character_trait;
			public string Concept => _concept;


			//конструктор
			public Response(string animal, string trait, string concept)
			{
				_animal = animal;
				_character_trait = trait;
				_concept = concept;
			}

			//методы
			public int CountVotes(Response[] responses, int questionNumber)
			{
				if (responses == null) return 0;

				var entity = this;
				if (questionNumber == 1)
				{
					return responses.Count(x => x.Animal != null && x.Animal == entity.Animal);
				}
				else if (questionNumber == 2)
				{
					return responses.Count(x => x.CharacterTrait != null && x.CharacterTrait== entity.CharacterTrait);
				}
				else if (questionNumber == 3)
				{
					return responses.Count(x => x.Concept != null && x.Concept == entity.Concept);
				}
				else
				{
					return 0;
				}
			}

			public void Print()
			{
				Console.WriteLine(this.Animal + " " + this.CharacterTrait + " " + this.Concept);
			}
		}

		public struct Research
		{
			//поля
			private string _name;
			private Response[] _responses;

			//свойства
			public string Name => _name;
			public Response[] Responses
			{
				get
				{
					if (_responses == null)
					{
						return null;
					}
					Response[] resps = new Response[_responses.Length];
					Array.Copy(_responses, resps, _responses.Length);
					return resps;
				}
			}


			//конструкторы
			public Research(string name)
			{
				_name = name;
				_responses = new Response[0];
			}

			//методы
			public void Add(string[] answers)
			{
				if (_responses == null || answers == null )
				{
					return;
				}
				var new_resp = new Response(answers[0], answers[1], answers[2]);
				Array.Resize(ref _responses, _responses.Length + 1);
				_responses[_responses.Length - 1] = new_resp;
			}


			public string[] GetTopResponses(int question)
			{
				if (_responses == null) { return null; }
				if (question == 1)
				{
					return _responses.GroupBy(x => x.Animal).Where(x => x.Key != null && x.Key.Length != 0).OrderByDescending(x => x.Count()).Take(5).Select(x => x.Key).ToArray();

				}
				else if (question == 2)
				{
					return _responses.GroupBy(x => x.CharacterTrait).Where(x => x.Key != null && x.Key.Length != 0).OrderByDescending(x => x.Count()).Take(5).Select(x => x.Key).ToArray();

				}
				else if (question == 3)
				{
					return _responses.GroupBy(x => x.Concept).Where(x => x.Key != null && x.Key.Length != 0).OrderByDescending(x => x.Count()).Take(5).Select(x => x.Key).ToArray();

				}
				else
				{
					return null;
				}
			}

			public void Print()
			{
				if (_responses == null)
				{
					Console.WriteLine(this.Name + " Null");
					return;
				}

				Console.WriteLine(this.Name);
				int i = 1;
				foreach (var resp in this.Responses)
				{
					Console.Write(i++ + " ");
					resp.Print();
				}
			}
		}
		public class Report
		{
			private Research[] _researches;
			private static int _researchID;
			public Research[] Researches => _researches;
			static Report()
			{
				_researchID = 1;
			}

			public Report()
			{
				_researches = new Research[0];
			}
			public Research MakeResearch()
			{
				var MM = DateTime.Now.ToString("MM");
				var YY = DateTime.Now.ToString("yy");

				Research newresearch = new Research($"No_{_researchID++}_{MM}/{YY}");
				Array.Resize(ref _researches, _researches.Length + 1);
				_researches[_researches.Length - 1] = newresearch;
				return newresearch;
			}

			private string GetData(Response rsp , int ques)
			{
				switch (ques)
				{
					case 1: return rsp.Animal;
					case 2: return rsp.CharacterTrait;
					case 3: return rsp.Concept;
					default:
						return null;
				}
			}
			public (string, double)[] GetGeneralReport(int question)
			{
				if(question < 1 || question > 3 || _researches == null) { return null; }
				var target = _researches.SelectMany(rs => rs.Responses).Where(rsp => GetData(rsp, question) != null);
				int trc = target.Count();
				var group_of_resps = target.GroupBy(rsp => GetData(rsp, question));
				return group_of_resps.Select(g => (g.Key, (double)g.Count() / trc * 100)).ToArray();
			}

			public void AddResearch(Research newResearch)
			{
				if (_researches == null) return;
				Array.Resize(ref _researches, _researches.Length + 1);
				_researches[^1] = newResearch;
			}

            public void AddResearch(Research[] newResearch)
            {
                if (_researches == null || newResearch == null ) return;
                for(int i = 0; i < newResearch.Length; ++i)
				{
					AddResearch(newResearch[i]);
				}
            }

        }
	}
}
