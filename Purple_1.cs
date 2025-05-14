using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_1
    {
        public class Participant
        {
            //fields
            private string _name;
            private string _surname;
            private double[] _coefs;
            private int[,] _marks;
            private int _amount_jumps;

            //svoystva
            public string Name => _name;
            public string Surname => _surname;

            public double[] Coefs
            {
                get
                {
                    if (_coefs == null || _coefs.Length == 0) return default(double[]);
                    double[] copy_reader = new double[_coefs.Length];
                    Array.Copy(_coefs, copy_reader, _coefs.Length);
                    return copy_reader;
                }
            }

            public int[,] Marks
            {
                get
                {
                    if (_marks == null || _marks.GetLength(0) != 4 || _marks.GetLength(1) != 7) return default(int[,]);
                    int[,] copy_reader = new int[_marks.GetLength(0), _marks.GetLength(1)];
                    Array.Copy(_marks, copy_reader, _marks.Length);
                    return copy_reader;
                }
            }
            public double TotalScore
            {
                get
                {
                    if (_marks == null || _coefs == null) return 0;

                    double score_of_jumper = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        score_of_jumper += CountJumpScore(i);
                    }
                    return score_of_jumper;
                }
            }
            private double CountJumpScore(int index_jump)
            {
                if (_marks == null || _coefs == null) return 0;

                double curr_score = 0;
                int[] marks = new int[_marks.GetLength(1)];
                for (int i = 0; i < _marks.GetLength(1); i++)
                {
                    marks[i] = _marks[index_jump, i];
                    curr_score += marks[i];
                }
                int im = 0, imx = 0;
                for (int i = 0; i < marks.Length; i++)
                {
                    if (marks[i] < marks[im]) im = i;
                    if (marks[i] > marks[imx]) imx = i;
                }
                curr_score -= marks[im]; curr_score -= marks[imx];
                curr_score *= _coefs[index_jump];
                return curr_score;
            }
            // constructor
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[] { 2.5, 2.5, 2.5, 2.5 };
                _marks = new int[,]
                {
                    {0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0}
                };
                _amount_jumps = 0;
            }

            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || _coefs == null || coefs.Length != 4) return;

                for (int i = 0; i < coefs.Length; i++)
                {
                    if (coefs[i] > 3.5 || coefs[i] < 2.5)
                    {
                        return;
                    }
                }
                Array.Copy(coefs, _coefs, 4);
            }

            public void Jump(int[] marks)
            {
                if (marks == null || _marks == null || marks.Length != 7 || _amount_jumps >= 4) return;
                foreach (int mark in marks)
                {
                    if (mark < 1 || mark > 6) return;
                }
                for (int i = 0; i < marks.Length; i++)
                {
                    _marks[_amount_jumps, i] = marks[i];
                }
                _amount_jumps++;
            }
            public static void Sort(Participant[] array)
            {
                if (array == null || array.Length == 0) return;
                Array.Sort(array, (a, b) => {
                    if (b.TotalScore - a.TotalScore > 0) return 1;
                    else if (b.TotalScore - a.TotalScore < 0) return -1;
                    else return 0;
                });
            }
            public void Print() { }
        }
        public class Judge
        {
            private string _name;
            private int[] _marks;

            // extra counter
            private int _ind_of_mark;
            public string Name => _name;
            public int[] Marks => _marks; // NEW LINE
            public Judge(string name, int[] marks)
            {
                _name = name;
                _marks = marks;
                _ind_of_mark = 0;
            }

            public int CreateMark()
            {
                if (_marks == null || _marks.Length == 0) return 0;

                if (_ind_of_mark < _marks.Length)
                {
                    int trash = _marks[_ind_of_mark];
                    _ind_of_mark++;
                    return trash;
                }
                else
                {
                    _ind_of_mark = 0;
                    return _marks[_ind_of_mark];
                }
            }

            public void Print()
            {
                Console.WriteLine(Name);
                for (int i = 0; i < _marks.Length; i++)
                {
                    Console.Write(_marks[i] + " ");
                }
            }
        }
        public class Competition
        {
            private Judge[] _judges;
            private Participant[] _participants;

            public Judge[] Judges => _judges;
            public Participant[] Participants => _participants;

            public Competition(Judge[] judges)
            {
                _judges = judges;
                _participants = new Participant[0];
            }

            public void Evaluate(Participant jumper)
            {
                if (jumper == null || _judges == null || _judges.Length == 0) return;

                int[] marks = new int[_judges.Length];

                for (int i = 0; i < _judges.Length; i++)
                {
                    marks[i] = _judges[i].CreateMark();
                }
                jumper.Jump(marks);
            }
            public void Add(Participant participant)
            {
                if (participant == null || _participants == null) return;
                Participant[] copy = new Participant[_participants.Length + 1];
                for (int i = 0; i < _participants.Length; i++)
                {
                    copy[i] = _participants[i];
                }
                Evaluate(participant);
                copy[copy.Length - 1] = participant;
                _participants = copy;

            }
            public void Add(Participant[] participants)
            {
                if (participants == null || _participants == null) return;
                foreach (var participant in participants) { Add(participant); }
            }
            public void Sort()
            {
                Participant.Sort(_participants);
            }

        }
    }
}