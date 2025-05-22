// See https://aka.ms/new-console-template for more information
using Lab_7;

var participant = new Blue_2.Participant("Олег", "Шепс");
participant.Jump(new int[] { 1, 2, 3, 4, 5 });
participant.Jump(new int[] { 2, 2, 3, 4, 6 });
var team = new Blue_2.WaterJump3m("Название", 10000);
team.Add(participant);

