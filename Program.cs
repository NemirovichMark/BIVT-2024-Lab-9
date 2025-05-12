
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Console;
using Lab_7;

namespace Lab_9;
internal class Program
{
    public static void Main()
    {
        BlueTXTSerializer blueTXTSerializer = new BlueTXTSerializer();
        blueTXTSerializer.SelectFolder(@"C:\Proga");

        Blue_2.Participant a = new Blue_2.Participant("Дум", "fl");
        Blue_2.Participant b = new Blue_2.Participant("Kok-Pok", "sos");
        a.Jump([1,2,3,4,5]);

        Blue_2.WaterJump3m jump = new Blue_2.WaterJump3m("jump", 30);
        jump.Add([a,b]);
        
        blueTXTSerializer.SerializeBlue2WaterJump(jump, @"yexy");
        var res = blueTXTSerializer.DeserializeBlue2WaterJump(blueTXTSerializer.FilePath);
        Console.WriteLine(res.Participants.Length);
        foreach (int item in res.Participants[0].Marks)
        {
            // Console.WriteLine(item);
        }
        /*

        Blue_5.Sportsman a = new Blue_5.Sportsman("Aboba", "Abobov");
        a.SetPlace(5);
        Blue_5.ManTeam manTeam = new Blue_5.ManTeam("Win");
        manTeam.Add(a);
        blueTXTSerializer.SerializeBlue5Team(manTeam, "lol");
        var res = blueTXTSerializer.DeserializeBlue5Team<Blue_5.ManTeam>(blueTXTSerializer.FilePath);
        Console.WriteLine(res.Name);
        // Console.WriteLine($"{res.Sportsmen[0]}")

        */
    }
}
