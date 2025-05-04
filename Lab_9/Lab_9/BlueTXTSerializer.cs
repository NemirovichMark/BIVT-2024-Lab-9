using Lab_7;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static Lab_7.Blue_2;
using static Lab_7.Blue_3;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        private int[] GetMarks(string marks)
        {
            string[] sMarks = marks.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            int[] iMarks = new int[sMarks.Length];
            for (int k = 0; k < sMarks.Length; k++)
            {
                iMarks[k] = int.Parse(sMarks[k]);
            }
            return iMarks;
        }

        // Blue 1
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            var human = participant as Blue_1.HumanResponse;
            string text;
            if (human == null)
                text = $"{participant.Name} {participant.Votes}";
            else
                text = $"{human.Name} {human.Surname} {human.Votes}";

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            //string text = File.ReadAllText(fileName);
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null; 

            Blue_1.Response response;
            var words = text.Split(' ');
            if (words.Length == 2)
            {
                response = new Blue_1.Response(words[0], int.Parse(words[1]));
            }
            else
            {
                response = new Blue_1.HumanResponse(words[0], words[1], int.Parse(words[2]));
            }
            return response;
        }


        // Blue 2
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName){
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            string text;
            if (participant is Blue_2.WaterJump3m)
                text = "3 ";
            else // participant is Blue_2.WaterJump5m
                text = "5 ";
            text += $"{participant.Name} {participant.Bank}";
            foreach (var part in participant.Participants)
            {
                text += $"{Environment.NewLine}{part.Name} {part.Surname}";
                text += $"{Environment.NewLine}"; 
                for(int jump = 0; jump < 2; jump++)
                {
                    for(int m = 0; m < 5; m++)
                    {
                        text += $" {part.Marks[jump, m]}";
                    }
                    text += $"{Environment.NewLine}";
                }
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;

            var strings = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var forInit = strings[0].Split(" ");

            var jump = GetWaterJump(forInit[0], forInit[1], forInit[2]);
            if (strings.Length == 1) return jump;

            for (int k = 1; k < strings.Length; k += 3)
            {
                string[] personInfo = strings[k].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int[] marks_1 = GetMarks(strings[k + 1]);
                int[] marks_2 = GetMarks(strings[k + 2]);

                var participant = new Blue_2.Participant(personInfo[0], personInfo[1]);
                participant.Jump(marks_1);
                participant.Jump(marks_2);

                jump.Add(participant);
            }

            return jump;
        }
        //
        private Blue_2.WaterJump GetWaterJump(string type, string name, string bank)
        {
            if (type == "3")
            {
                var jump = new Blue_2.WaterJump3m(name, int.Parse(bank));
                return jump;
            }
            else if (type == "5")
            {
                var jump = new Blue_2.WaterJump5m(name, int.Parse(bank));
                return jump;
            }
            return null;
        }
        

        // Blue 3      
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || String.IsNullOrEmpty(fileName))return;

            string text = "";
            if (student is Blue_3.BasketballPlayer)
                text += "BasketballPlayer";
            else if (student is Blue_3.HockeyPlayer)
                text += "HockeyPlayer";
            else
                text += "Participant";

            text += Environment.NewLine +
                student.Name + " " + student.Surname + Environment.NewLine;
            foreach(int p in student.Penalties)
            {
                text += $"{p} ";
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override T DeserializeBlue3Participant<T>(string fileName) // where T : Blue_3.Participant
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;

            string[] strings = text.Split(Environment.NewLine);
            string[] personInfo = strings[1].Split(" ");
            T student = (T) GetParticipant(strings[0], personInfo[0], personInfo[1]);
            if (strings.Length == 2) return student;

            int[] penalties = GetMarks(strings[2]);
            foreach (int p in penalties)
                student.PlayMatch(p);

            return student; 
        }
        // 
        private Blue_3.Participant GetParticipant(string type, string name, string surname) 
        {
            switch (type)
            {
                case "BasketballPlayer":
                    var studentB = new Blue_3.BasketballPlayer(name, surname) ;
                    return studentB;
                case "HockeyPlayer":
                    var studentH = new Blue_3.HockeyPlayer(name, surname);
                    return studentH;
                case "Participant":
                    var studentP = new Blue_3.Participant(name, surname);
                    return studentP;
                default:
                    return null;
            }
        }


        // Blue 4
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName){
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            string text = participant.Name + Environment.NewLine + "*" + Environment.NewLine;
            WriteTeamsInfo(participant.ManTeams, ref text);
            text += Environment.NewLine + "~" + Environment.NewLine;
            WriteTeamsInfo(participant.WomanTeams, ref text);

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;

            string[] info = text.Split("*", StringSplitOptions.RemoveEmptyEntries);

            var group = new Blue_4.Group(info[0].Replace(Environment.NewLine, ""));
            if(info.Length == 1) return group;

            string[] teams = info[1].Split("~"); //0 - men   1 - women 
            string[] manTeams = teams[0].Split("#", StringSplitOptions.RemoveEmptyEntries);
            string[] womanTeams = teams[1].Split("#", StringSplitOptions.RemoveEmptyEntries);
            AddTeams(manTeams, "man", group);
            AddTeams(womanTeams, "woman", group);
            
            return group; ;
        }
        //
        private string TeamInfo(Blue_4.Team team)
        {
            if (team == null) return "";

            string text =  team.Name + Environment.NewLine ;
            foreach (int score in team.Scores)
                text += $"{score} ";
            return text;
        }
        private void WriteTeamsInfo(Blue_4.Team[] teams, ref string text)
        {
            string[] sTeams = new string[teams.Length];
            for(int k = 0; k < teams.Length; k++)
            {
                sTeams[k] = TeamInfo(teams[k]);
            }
            text += String.Join($"{Environment.NewLine}#{Environment.NewLine}", sTeams);
        }
        private Blue_4.Team GetTeam(string name, string type)
        {
            switch(type)
            {
                case "man":
                    var mTeam = new Blue_4.ManTeam(name);
                    return mTeam;
                case "woman":
                    var wTeam = new Blue_4.WomanTeam(name);
                    return wTeam;
                default:
                    return null;
            }
        }
        private void AddTeams(string[] teams, string type, Blue_4.Group group)
        {
            foreach (string sTeam in teams)
            {
                if(String.IsNullOrWhiteSpace(sTeam)) continue;

                var teamInfo = sTeam.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                var team = GetTeam(teamInfo[0].Replace(Environment.NewLine, ""), type);
                int[] scores = GetMarks(teamInfo[1]);
                foreach (int score in scores)
                    team.PlayMatch(score);

                group.Add(team);
            }
        }

        // Blue 5
        public override void SerializeBlue5Team<T>(T group, string fileName) {
            if (group == null || String.IsNullOrEmpty(fileName)) return;

            string text = "";
            if (group is Blue_5.ManTeam)
                text = "man ";
            else if (group is Blue_5.WomanTeam)
                text = "woman ";
            text += group.Name + Environment.NewLine;
            foreach (var sportsman in group.Sportsmen)
            {
                if (sportsman == null) break;
                text += sportsman.Name + " " + sportsman.Surname + " " + sportsman.Place + Environment.NewLine;
            } 
            
            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override T DeserializeBlue5Team<T>(string fileName) 
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;

            string[] strings = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            string[] groupInfo = strings[0].Split(" ");
            T group = (T)GetGroup(groupInfo[0], groupInfo[1]);
            if (strings.Length == 1) return group;

            for(int k = 1; k < strings.Length; k++)
            {
                string[] sportsmanInfo = strings[k].Split(" ");
                var sportsman = new Blue_5.Sportsman(sportsmanInfo[0], sportsmanInfo[1]);
                sportsman.SetPlace(int.Parse(sportsmanInfo[2]));

                group.Add(sportsman);
            }

            return group; 
        }
        private Blue_5.Team GetGroup(string type, string name)
        {
            switch (type)
            {
                case "man":
                    var mTeam = new Blue_5.ManTeam(name);
                    return mTeam;
                case "woman":
                    var wTeam = new Blue_5.WomanTeam(name);
                    return wTeam;
                default:
                    return null;
            }
        }

        
        
    }
}
