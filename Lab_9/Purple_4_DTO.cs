using Lab_7;
namespace Lab_9;

public class Purple_4_Sportsman_DTO
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public double Time { get; set; }
    public string SportmanKind { get; set; }

    public Purple_4_Sportsman_DTO() { }
    public Purple_4_Sportsman_DTO(Purple_4.Sportsman sportsman)
    {
        Name = sportsman.Name;
        Surname = sportsman.Surname;
        Time = sportsman.Time;
        if (sportsman is Purple_4.SkiMan) SportmanKind = nameof(Purple_4.SkiMan);
        else if (sportsman is Purple_4.SkiWoman) SportmanKind = nameof(Purple_4.SkiWoman);
        else SportmanKind = "unknown";
    }

    public Purple_4.Sportsman ToObject()
    {
        Purple_4.Sportsman s;
        if (SportmanKind == nameof(Purple_4.SkiMan))
        {
            s = new Purple_4.SkiMan(Name, Surname);
        }
        else if (SportmanKind == nameof(Purple_4.SkiWoman))
        {
            s = new Purple_4.SkiWoman(Name, Surname);
        }
        else
        {
            s = new Purple_4.Sportsman(Name, Surname);
        }
        s.Run(Time);
        return s;
    }
}
public class Purple_4_Group_DTO
{
    public string Name { get; set; }
    public Purple_4_Sportsman_DTO[] Sportsmen { get; set; }

    public Purple_4_Group_DTO() { }
    public Purple_4_Group_DTO(Purple_4.Group group)
    {
        Name = group.Name;
        Sportsmen = group.Sportsmen.Select(s => new Purple_4_Sportsman_DTO(s)).ToArray();
    }

    public Purple_4.Group ToObject()
    {
        var g = new Purple_4.Group(Name);
        g.Add(Sportsmen.Select(s => s.ToObject()).ToArray());
        return g;
    }
}
