using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;

namespace Lab_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TXTBlue5();
        }

        public static void XMLBlue4()
        {
            Console.WriteLine("What XML");
            var te = new Blue_4.Group("Ut");
            System.Console.WriteLine("-");
            var p = new Blue_4.ManTeam("CSKA1");
            p.PlayMatch(5);
            te.Add(p);
            var p2 = new Blue_4.ManTeam("SPARTAK1");
            //te.Add(p2);
            var p3 = new Blue_4.ManTeam("Zenit1");
            te.Add(p3);
            var p4 = new Blue_4.ManTeam("Ural1");
            te.Add(p4);
            var pw3 = new Blue_4.WomanTeam("ASKA1");
            te.Add(pw3);
            System.Console.WriteLine(te.WomanTeams[0].Name);
            //System.Console.WriteLine(te.Prize[0]);
            //System.Console.WriteLine(te.Prize.Length);
            //System.Console.WriteLine(te.Prize[0]);
            var json = new BlueXMLSerializer();
            json.SerializeBlue4Group(te, "newT");

            var ans = json.DeserializeBlue4Group("newT");
            System.Console.Write("Len? ");
            System.Console.WriteLine(ans.ManTeams.Length);
            for(int i = 0; i < ans.ManTeams.Length; i++){
                //System.Console.Write(i);
                //System.Console.Write("  ");
                //System.Console.WriteLine(ans.ManTeams[i]);
                if(ans.ManTeams[i] != null){
                    System.Console.WriteLine(ans.ManTeams[i].Name);
                    for(int j = 0; j < ans.ManTeams[i].Scores.Length;j++){
                        System.Console.Write("     ");
                        System.Console.WriteLine(ans.ManTeams[i].Scores[j]);
                    }
                }
                
            }
            //ans.Print();
        }


        public static void TXTBlue4()
        {
            Console.WriteLine("What");
            var te = new Blue_4.Group("Asia");
            System.Console.WriteLine("-");
            var p = new Blue_4.ManTeam("CSKA");
            p.PlayMatch(4);
            p.PlayMatch(6);
            p.PlayMatch(0);
            te.Add(p);
            var p2 = new Blue_4.ManTeam("Dinamo");
            p2.PlayMatch(6);
            p2.PlayMatch(9);
            p2.PlayMatch(2);
            te.Add(p2);
            var p3 = new Blue_4.ManTeam("Metalurg");
            p3.PlayMatch(4);
            p3.PlayMatch(6);
            p3.PlayMatch(4);
            te.Add(p3);
            var p4 = new Blue_4.ManTeam("Gorniy");
            p4.PlayMatch(6);
            p4.PlayMatch(8);
            p4.PlayMatch(1);
            te.Add(p4);
            var p5 = new Blue_4.ManTeam("Bulls");
            p5.PlayMatch(3);
            p5.PlayMatch(0);
            p5.PlayMatch(6);
            te.Add(p5);
            var pw3 = new Blue_4.WomanTeam("Bars");
            pw3.PlayMatch(3);
            pw3.PlayMatch(5);
            pw3.PlayMatch(8);
            te.Add(pw3);
            //System.Console.WriteLine(te.WomanTeams[0].Name);
            //System.Console.WriteLine(te.Prize[0]);
            //System.Console.WriteLine(te.Prize.Length);
            //System.Console.WriteLine(te.Prize[0]);
            var json = new BlueTXTSerializer();
            json.SerializeBlue4Group(te, "newT");

           var ans = json.DeserializeBlue4Group("newT");
           System.Console.WriteLine("--------");
           
            for(int i = 0; i < ans.ManTeams.Length; i++){
                if(ans.ManTeams[i] != null){
                    System.Console.WriteLine(ans.ManTeams[i].Name);
                    if(ans.ManTeams[i].Scores != null){
                        System.Console.WriteLine(ans.ManTeams[i].Scores.Length);
                        //System.Console.WriteLine(ans.ManTeams[i].Scores[0]);
                    }
                    
                }
                
            }

            System.Console.WriteLine("WOOOMMMENN");
            for(int i = 0; i < ans.WomanTeams.Length; i++){
                if(ans.WomanTeams[i] != null){
                    System.Console.Write(i);
                    System.Console.WriteLine(ans.WomanTeams[i].Name);
                    if(ans.WomanTeams[i].Scores != null){
                        //System.Console.WriteLine(ans.WomanTeams[i].Scores.Length);
                        //System.Console.WriteLine(ans.ManTeams[i].Scores[0]);
                    }
                    
                }
                
            }
            //ans.Print();
        }


        public static void JsonBlue4()
        {
            Console.WriteLine("What");
            var te = new Blue_4.Group("Ut");
            System.Console.WriteLine("-");
            var p = new Blue_4.ManTeam("CSKA1");
            te.Add(p);
            var p2 = new Blue_4.ManTeam("SPARTAK1");
            te.Add(p2);
            var p3 = new Blue_4.ManTeam("Zenit1");
            te.Add(p3);
            var p4 = new Blue_4.ManTeam("Ural1");
            te.Add(p4);
            var pw3 = new Blue_4.WomanTeam("ASKA1");
            te.Add(pw3);
            System.Console.WriteLine(te.WomanTeams[0].Name);
            //System.Console.WriteLine(te.Prize[0]);
            //System.Console.WriteLine(te.Prize.Length);
            //System.Console.WriteLine(te.Prize[0]);
            var json = new BlueJSONSerializer();
            json.SerializeBlue4Group(te, "newT");

            var ans = json.DeserializeBlue4Group("newT");
            for(int i = 0; i < ans.ManTeams.Length; i++){
                if(ans.ManTeams[i] != null){
                    System.Console.WriteLine(ans.ManTeams[i].Name);
                }
                
            }
            //ans.Print();
        }

        public static void JsonBlue2()
        {
            Console.WriteLine("What");
            var te = new Blue_2.WaterJump5m("Ut", 14);
            System.Console.WriteLine("-");
            var p = new Blue_2.Participant("1", "2");
            te.Add(p);
            var p2 = new Blue_2.Participant("2", "4");
            te.Add(p2);
            var p3 = new Blue_2.Participant("3", "5");
            te.Add(p3);
            //System.Console.WriteLine(te.Prize[0]);
            System.Console.WriteLine(te.Prize.Length);
            //System.Console.WriteLine(te.Prize.Length);
            //System.Console.WriteLine(te.Prize[0]);
            var json = new BlueJSONSerializer();
            json.SerializeBlue2WaterJump(te, "newT");

            var ans = json.DeserializeBlue2WaterJump("newT");
            System.Console.WriteLine(ans);
            System.Console.WriteLine(ans.Prize.Length);
            ans.Print();
        }

         public static void TXTBlue2()
        {
            Console.WriteLine("What");
            var te = new Blue_2.WaterJump5m("Ut", 14);
            System.Console.WriteLine("-");
            var p = new Blue_2.Participant("1", "2");
            te.Add(p);
            var p2 = new Blue_2.Participant("2", "4");
            te.Add(p2);
            var p3 = new Blue_2.Participant("3", "5");
            te.Add(p3);
            //System.Console.WriteLine(te.Prize[0]);
            System.Console.WriteLine(te.Prize.Length);
            //System.Console.WriteLine(te.Prize.Length);
            //System.Console.WriteLine(te.Prize[0]);
            var json = new BlueTXTSerializer();
            json.SerializeBlue2WaterJump(te, "newT");

            var ans = json.DeserializeBlue2WaterJump("newT");
            System.Console.WriteLine(ans);
            System.Console.WriteLine(ans.Prize.Length);
            ans.Print();
        }


        public static void JsonBlue1()
        {
            Console.WriteLine("What");
            var te = new Blue_1.Response("Ut", 3);

            var json = new BlueJSONSerializer();
            json.SerializeBlue1Response(te, "newT");

            var ans = json.DeserializeBlue1Response("newT");
            Console.WriteLine(ans.Name);
        }
        public static void TXTBlue1()
        {
            Console.WriteLine("What");
            var te = new Blue_1.Response("Ut", 3);

            var json = new BlueTXTSerializer();
            json.SerializeBlue1Response(te, "newT");

            var ans = json.DeserializeBlue1Response("newT");
            Console.WriteLine(ans.Name);
        }
        public static void XMLBlue1()
        {
            Console.WriteLine("What");
            var te = new Blue_1.Response("Ut", 3);

            var json = new BlueXMLSerializer();
            json.SerializeBlue1Response(te, "newT");

            var ans = json.DeserializeBlue1Response("newT");
            Console.WriteLine(ans.Name);
        }

        public static void XMLBlue2()
        {
             Console.WriteLine("What");
            var te = new Blue_2.WaterJump5m("Ut", 14);
            System.Console.WriteLine("-");
            var p = new Blue_2.Participant("1", "2");
            te.Add(p);
            var p2 = new Blue_2.Participant("2", "4");
            te.Add(p2);
            var p3 = new Blue_2.Participant("3", "5");
            te.Add(p3);
            //System.Console.WriteLine(te.Prize[0]);
            System.Console.Write("Len: ");
            System.Console.WriteLine(te.Prize.Length);
            //System.Console.WriteLine(te.Prize.Length);
            //System.Console.WriteLine(te.Prize[0]);
            var json = new BlueXMLSerializer();
            json.SerializeBlue2WaterJump(te, "newT");

            var ans = json.DeserializeBlue2WaterJump("newT");
            System.Console.WriteLine(ans);
            System.Console.WriteLine(ans.Prize.Length);
            ans.Print();
        }


        public static void TXTBlue3()
        {
             Console.WriteLine("What");
            var te = new Blue_3.HockeyPlayer("Ut", "fsdFF");
            System.Console.WriteLine("-");
            te.PlayMatch(4);
            te.PlayMatch(47);
            te.PlayMatch(2);
            var json = new BlueTXTSerializer();
            json.SerializeBlue3Participant(te, "newT");
            System.Console.WriteLine("----------------------------------");
            var ans = json.DeserializeBlue3Participant<Blue_3.Participant>("newT");
            ans.Print(); 
        }

         public static void TXTBlue5()
        {

            Blue_5.Team res = new Blue_5.ManTeam("2323");
            var d1 = new Blue_5.Sportsman("first", "sur");
            var d2 = new Blue_5.Sportsman("second", "rus");

              
                
            res.Add(d1);
            res.Add(d2);
            
           

            var json = new BlueTXTSerializer();
            json.SerializeBlue5Team(res, "newT");

            System.Console.WriteLine("----------------------------------");
           var ans = json.DeserializeBlue5Team<Blue_5.Team>("newT");
           System.Console.WriteLine("!!!!!!!!!!!!");
           //System.Console.WriteLine(ans.Name);
            ans.Print(); 
        }

    }
}
