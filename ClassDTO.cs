using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class ClassDTO
    {
        public class ResponseSD //DTO-класс для Blue_1.Response
        {
            public string Type { get; set; } 
            public string Name { get; set; }
            public int Votes { get; set; }
            public string? Surname { get; set; } // фамилия только для HumanResponse

            public ResponseSD() { }

            public ResponseSD(Blue_1.Response person)
            {
                Type = person.GetType().Name; // определяет тип объекта Response или HumanResponse
                Name = person.Name;
                Votes = person.Votes;
                if (person is Blue_1.HumanResponse newperson)// проверка на наследника
                {
                    Surname = newperson.Surname;
                }
            }
        }
        public class ParticipantSD //DTO-класс для Blue_2
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; } // двумерный массив оценок преобразованный в int[][] для сериализации
            public ParticipantSD() { }
            public ParticipantSD(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;

                Marks = newMarks(participant.Marks); // преобразует int[,] в int[][]
            }
            // преобразует int[,] в int[][] так как XML-сериализатор не поддерживает int[,]
            private int[][] newMarks(int[,] marks)
            {
                if (marks == null) return null;
                int[][] newmarks = new int[marks.GetLength(0)][];
                for (int i = 0; i < newmarks.Length; i++)
                {
                    newmarks[i] = new int[marks.GetLength(1)];
                    for (int j = 0; j < newmarks[i].Length; j++)
                    {
                        newmarks[i][j] = marks[i, j]; // Копирует оценки
                    }
                }
                return newmarks;
            }
        }
        public class WaterJumpSD //DTO-класс для Blue_2
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; } // призовой фонд
            public ParticipantSD[] Participants { get; set; }
            public WaterJumpSD() { }
            public WaterJumpSD(Blue_2.WaterJump waterjump)
            {
                Type = waterjump.GetType().Name;
                Name = waterjump.Name;
                Bank = waterjump.Bank;
                if (waterjump.Participants != null)
                {
                    Participants = new ParticipantSD[waterjump.Participants.Length]; // создаётся новый массив Participants
                    for (int i = 0; i < waterjump.Participants.Length; i++) 
                    {
                        // каждый элемент массива участников инициализируется новым объектом ParticipantSD на основе элемента из массив waterjump.Participants
                        Participants[i] = new ParticipantSD(waterjump.Participants[i]);
                    }
                }
            }
        }
        public class Participant3SD //DTO-класс для Blue_3.Participant
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
            public Participant3SD() { }
            public Participant3SD(Blue_3.Participant person)
            {
                Type = person.GetType().Name;
                Name = person.Name;
                Surname = person.Surname;
                Penalties = person.Penalties;
            }
        }
        public class TeamSD //DTO-класс ддля Blue_4
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; } // массив очков
            public TeamSD() { }
            public TeamSD(Blue_4.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Scores = team.Scores;
            }
        }
        public class GroupSD //DTO-класс для Blue_4
        {
            public string Name { get; set; }
            public TeamSD[] ManTeams { get; set; }
            public TeamSD[] WomanTeams { get; set; }
            public GroupSD() { }
            public GroupSD(Blue_4.Group group)
            {
                Name = group.Name;

                //ManTeams
                if (group.ManTeams != null)
                {
                    ManTeams = new TeamSD[group.ManTeams.Length]; // создается новый массив ManTeams типа TeamSD
                    for (int i = 0; i < group.ManTeams.Length; i++) //для каждого индекса i создается новый элемент ManTeams[i]
                    {
                        //если group.ManTeams[i] не равен null создается новый объект TeamSD принимающий в конструктор этот элемент
                        ManTeams[i] = group.ManTeams[i] == null ? null : new TeamSD(group.ManTeams[i]);
                    }
                }
                //WomanTeams
                if (group.WomanTeams != null)
                {
                    WomanTeams = new TeamSD[group.WomanTeams.Length];// создается новый массив WomanTeams типа TeamSD
                    for (int i = 0; i < group.WomanTeams.Length; i++)//для каждого индекса i создается новый элемент WomanTeams[i]
                    {
                        //если group.WomanTeams[i] не равен null создается новый объект TeamSD принимающий в конструктор этот элемент
                        WomanTeams[i] = group.WomanTeams[i] == null ? null : new TeamSD(group.WomanTeams[i]);
                    }
                }
            }
        }
        public class Sportsman5SD //DTO-класс для Blue_5
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }

            public Sportsman5SD() { }
            public Sportsman5SD(Blue_5.Sportsman person)
            {
                Name = person.Name;
                Surname = person.Surname;
                Place = person.Place;
            }
        }
        public class Team5SD //DTO-класс для Blue_5
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Sportsman5SD[] Sporsman { get; set; }
            public Team5SD() { }
            public Team5SD(Blue_5.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                if (team.Sportsmen != null)
                {
                    Sporsman = new Sportsman5SD[team.Sportsmen.Length]; // создается новый массив Sporsman типа Sportsman5SD
                    for (int i = 0; i < team.Sportsmen.Length; i++)//для каждого индекса i создается новый элемент Sporsman[i]
                    {
                        //если team.Sporsman[i] не равен null создается новый объект Sportsman5SD принимающий в конструктор этот элемент
                        Sporsman[i] = team.Sportsmen[i] == null ? null : new Sportsman5SD(team.Sportsmen[i]);
                    }
                }
            }
        }
    }
}
