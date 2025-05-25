using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
	public class Purple_2
	{
		public struct Participant
		{
			// поля
			private string _name;
			private string _surname;
			private int _dictance;
			private int[] _marks;
			private int _target;
			private bool _jumped;

			// свойства
			public string Name { get { return _name; } }
			public string Surname { get { return _surname; } }
			public int Distance { get { return _dictance; } }
			public int[] Marks
			{
				get
				{
					if (_marks == null) { return null; }
					var newarr = new int[_marks.Length];
					Array.Copy(_marks, newarr, _marks.Length);
					return newarr;
				}
			}

			public int Result
			{
				get
				{
					if (_marks == null || _dictance == 0) { return 0; }
					
					int total = Math.Max(0, _marks.Sum() - _marks.Max() - Marks.Min() + 60 + (_dictance - _target) * 2);
					return total;

				}
			}

			// конструкторы
			public Participant(string name, string surname)
			{
				_name = name;
				_surname = surname;
				_dictance = 0;
				_marks = new int[5];
				_target = 0;
			}

			// методы
			public void Jump(int distance, int[] marks, int target)
			{
				if (_dictance != 0 || _marks == null) return;
				if (marks == null) return;
				if (marks.Length !=5 ) return;
				_dictance = distance;
				_target = target;
				Array.Copy(marks, _marks, marks.Length);
				
			}

			public static void Sort(Participant[] array)
			{
				if (array == null) { return; }
				Participant[] sorted = array.OrderByDescending(x => x.Result).ToArray();
				Array.Copy(sorted, array, sorted.Length);
			}

			public void Print()
			{
				Console.WriteLine(this.Name + " " + this.Surname + " " + this.Result);
			}

		}
		public abstract class SkiJumping
		{
			private string _name;
			private int _standard;
			private Participant[] _participants;

			public string Name => _name;
			public int Standard => _standard;
			public Participant[] Participants => _participants;

			// конструктор
			public SkiJumping(string name, int standard)
			{
				_name = name;
				_standard = standard;
				_participants = new Participant[0];
			}

			//методы
			public void Add(Participant sportsman)
			{
				Array.Resize(ref _participants, _participants.Length + 1);
				_participants[_participants.Length - 1] = sportsman;
			}

			public void Add(Participant[] sportsmen)
			{
				foreach(Participant el in sportsmen)
				{
					Add(el);
				}
			}
			public void Jump(int distance, int[] marks)
			{
				for(int i = 0; i < _participants.Length; ++i)
				{
					if (_participants[i].Distance == 0)
					{
						Participant sportsman = _participants[i];
						sportsman.Jump(distance, marks, _standard);
						_participants[i] = sportsman;
						break;
					}
				}
			}
			public void Print()
			{
                Console.WriteLine(Name);
                Console.WriteLine(Standard);
				for(int i = 0; i < _participants.Length; ++i)
				{
					_participants[i].Print();
				}
			}
		}
		public class JuniorSkiJumping: SkiJumping
		{
			public JuniorSkiJumping() : base("100m", 100) { }

		}
		public class ProSkiJumping : SkiJumping
		{
			public ProSkiJumping() : base("150m", 150) { }
		}

    }
}
