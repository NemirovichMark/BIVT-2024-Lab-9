using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
	public class Purple_1
	{
		public class Participant
		{
			// поля
			private string _name;
			private string _surname;
			private double[] _coefs;
			private int[,] _marks;
			private int _index_of_jump;
			// свойствы
			public string Name { get { return _name; } }
			public string Surname { get { return _surname; } }

			public double[] Coefs
			{
				get
				{
					if (_coefs == null) { return null; }
					double[] cfs = new double[_coefs.Length];
					Array.Copy(_coefs, cfs, _coefs.Length);
					return cfs;
				}
			}

			public int[,] Marks
			{
				get
				{
					if (_marks == null) { return null; }
					int[,] mks = new int[_marks.GetLength(0), _marks.GetLength(1)];
					for (int i = 0; i < mks.GetLength(0); ++i)
					{
						for (int j = 0; j < mks.GetLength(1); ++j)
						{
							mks[i, j] = _marks[i, j];
						}
					}
					return mks;
				}
			}

			public double TotalScore
			{
				get
				{
					if (_marks == null || _coefs == null)
					{
						return 0;
					}
					double sm = 0, sall = 0;
					int mi = 1000, ma = 0;
					for (int i = 0; i < _marks.GetLength(0); i++)
					{
						for (int j = 0; j < _marks.GetLength(1); j++)
						{
							sm += _marks[i, j];
							if (_marks[i, j] >= ma)
							{
								ma = _marks[i, j];
							}
							if (_marks[i, j] <= mi)
							{
								mi = _marks[i, j];
							}
						}
						sm -= mi;
						sm -= ma;
						sm *= _coefs[i];
						sall += sm;
						sm = 0;
						mi = 1000;
						ma = 0;
					}
					return sall;
				}
			}

			//конструкторы
			public Participant(string name, string surname)
			{
				_name = name;
				_surname = surname;
				_coefs = new double[] { 2.5, 2.5, 2.5, 2.5 };
				_marks = new int[4, 7];
				_index_of_jump = 0;
			}

			//методы
			public void SetCriterias(double[] coefs)
			{
				if (_coefs == null || coefs == null) { return; }
				Array.Copy(coefs, _coefs, coefs.Length);
			}

			public void Jump(int[] marks)
			{
				if (marks == null || _marks == null || _coefs == null)
				{
					return;
				}
				if (_index_of_jump > 3)
				{
					return;
				}
				for (int i = 0; i < 7; i++)
				{
					_marks[_index_of_jump, i] = marks[i];
				}
				_index_of_jump += 1;
			}

			public static void Sort(Participant[] array)
			{

				if (array == null) { return; }
				for (int i = 1; i < array.Length; i++)
				{
					Participant k = array[i];
					int j = i - 1;
					while (j >= 0 && array[j].TotalScore < k.TotalScore)
					{
						array[j + 1] = array[j];
						array[j] = k;
						j--;
					}
				}
			}

			public void Print()
			{
				Console.WriteLine(this.Name + " " + this.Surname + " " + this.TotalScore);

			}
		}

		public class Judge
		{
			//поля и свойства
			private string _name;
			private int[] _marks;
			public string Name => _name;
			public int[] Marks => (int[])_marks?.Clone();
			private int _cpos;


			//конструктор
			public Judge(string name, int[] marks)
			{
				_name = name;

				if(marks != null) {
					_marks = new int[marks.Length];
					Array.Copy(marks, _marks, marks.Length);
				}
			}

			public int CreateMark()
			{
				if(_marks== null || _marks.Length == 0)
				{
					return 0;
				}
				int res = _marks[_cpos++];
				_cpos %= _marks.Length;
				return res;
			}

			public void Print()
			{
				Console.Write("Name: "+ _name+ " "); 
				foreach(int el in _marks)
				{
					Console.Write(el + " ");
				}
                Console.WriteLine();
			}
		}

		public class Competition
		{
			// поля и свойства
			private Judge[] _judges;
			private Participant[] _participants;
			public Judge[] Judges => _judges;
			public Participant[] Participants => _participants;
			

			//конструктор
			public Competition(Judge[] judges)
			{
				_participants = new Participant[0];
				if(judges == null)
				{
					return;
				}
				_judges = new Judge[judges.Length];
				Array.Copy(judges, _judges, judges.Length);
			}

			//методы
			public void Evaluate(Participant jumper)
			{
				if(_judges == null) { return; }

				int[] mks = new int[7];
				int k = 0;


				foreach (var judge in _judges)
				{
					if (k >= 7) { break; }
					if (judge != null)
					{
						mks[k++] = judge.CreateMark();
					}
				}
				jumper.Jump(mks);
			}

			public void Add(Participant jumper)
			{
				if(jumper == null)
				{
					return;
				}
				if(_participants == null)
				{
					_participants = new Participant[0];
				}

				Array.Resize(ref _participants, _participants.Length + 1);
				_participants[_participants.Length - 1] = jumper;

				Evaluate(_participants[^1]);

            }

            public void Add(Participant[] jumpers)
			{
				if(jumpers == null)
				{
					return;
				}
				if(_participants == null)
				{
					_participants = new Participant[0];
				}
				int len = _participants.Length;

				Array.Resize(ref _participants, _participants.Length + jumpers.Length);
				Array.Copy(jumpers, 0, _participants, len, jumpers.Length);

				for(int i = len; i < _participants.Length; ++i)
				{
					Evaluate(_participants[i]);
				}
			}

			public void Sort()
			{
				Participant[] sorted = _participants.OrderByDescending(t => t.TotalScore).ToArray();
				Array.Copy(sorted, _participants, sorted.Length);
			}

        }
	}
}

