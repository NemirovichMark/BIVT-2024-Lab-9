using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7;

public class Program
{
    static void Main(string[] args)
    {
        // Purple_1.Participant[] participants = new Purple_1.Participant[10];
        // for (int i = 0; i < 10; i++)
        // {
        //     string name = Console.ReadLine(), surname = Console.ReadLine();
        //     Purple_1.Participant p = new Purple_1.Participant(name, surname);
        //     double[] coefs = new double[4];
        //     int[] marks = new int[7];
        //     for (int j = 0; j < 4; j++) double.TryParse(Console.ReadLine(), out coefs[j]);
        //     p.SetCriterias(coefs);
        //     for (int j = 0; j < 4; j++)
        //     {
        //         for (int k = 0; k < 7; k++) int.TryParse(Console.ReadLine(), out marks[k]);
        //         p.Jump(marks);
        //     }
        //     participants[i] = p;
        // }
        // Purple_1.Participant.Sort(participants);
        // Purple_1.Participant.PrintTable(participants);
        
        // Purple_2.Participant[] participants = new Purple_2.Participant[10];
        // for (int i = 0; i < 10; i++)
        // {
        //     string name = Console.ReadLine(), surname = Console.ReadLine();
        //     Purple_2.Participant p = new Purple_2.Participant(name, surname);
        //     int.TryParse(Console.ReadLine(), out int distance);
        //     int[] marks = new int[5];
        //     for (int j = 0; j < 5; j++) int.TryParse(Console.ReadLine(), out marks[j]);
        //     p.Jump(distance, marks);
        //     participants[i] = p;
        // }
        // Purple_2.Participant.Sort(participants);
        // Purple_2.Participant.PrintTable(participants);
        
        // Purple_3.Participant[] participants = new Purple_3.Participant[10];
        // for (int i = 0; i < 10; i++)
        // {
        //     string name = Console.ReadLine(), surname = Console.ReadLine();
        //     Purple_3.Participant p = new Purple_3.Participant(name, surname);
        //     double[] marks = new double[7];
        //     for (int j = 0; j < 7; j++)
        //     {
        //         double.TryParse(Console.ReadLine(), out marks[j]);
        //         p.Evaluate(marks[j]);
        //     }
        //     participants[i] = p;
        // }
        // Purple_3.Participant.SetPlaces(participants);
        // Purple_3.Participant.Sort(participants);
        // Purple_3.Participant.PrintTable(participants);
        
        // Purple_4.Group group1 = new Purple_4.Group("Group1");
        // Purple_4.Group group2 = new Purple_4.Group("Group2");
        //
        // Purple_4.Sportsman[] sportsmen = new Purple_4.Sportsman[15];
        // for (int i = 0; i < 15; i++)
        // {
        //     string name = Console.ReadLine();
        //     string surname = Console.ReadLine();
        //     double.TryParse(Console.ReadLine(), out double time);
        //     sportsmen[i] = new Purple_4.Sportsman(name, surname);
        //     sportsmen[i].Run(time);
        // }
        // group1.Add(sportsmen);
        // for (int i = 0; i < 15; i++)
        // {
        //     string name = Console.ReadLine();
        //     string surname = Console.ReadLine();
        //     double.TryParse(Console.ReadLine(), out double time);
        //     sportsmen[i] = new Purple_4.Sportsman(name, surname);
        //     sportsmen[i].Run(time);
        // }
        // group2.Add(sportsmen);
        //
        // group1.Sort();
        // group2.Sort();
        // Purple_4.Group mergedGroup = Purple_4.Group.Merge(group1, group2);
        // Purple_4.Group.PrintTable(mergedGroup);
        
        // Purple_5.Research research = new Purple_5.Research("My Research");
        // for (int i = 0; i < 20; i++)
        // {
        //     string animal = Console.ReadLine();
        //     string trait = Console.ReadLine();
        //     string concept = Console.ReadLine();
        //     string[] answers = { animal, trait, concept };
        //     research.Add(answers);
        // }
        // for(int question = 1; question <= 3; question++) Purple_5.Research.PrintTable(research, question);
    }
}
