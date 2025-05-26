using Lab_7;
namespace Lab_9;

public class Purple_3_Participant_DAO
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public double[] Marks { get; set; }

    public Purple_3_Participant_DAO() { }
    public Purple_3_Participant_DAO(Purple_3.Participant p)
    {
        Name = p.Name;
        Surname = p.Surname;
        Marks = p.Marks;
    }

    public Purple_3.Participant ToObject()
    {
        Purple_3.Participant p = new Purple_3.Participant(Name, Surname);
        foreach (double mark in Marks)
        {
            p.Evaluate(mark);
        }
        Console.WriteLine(string.Join(' ', p.Marks));
        return p;
    }
}

public class Purple_3_Scating_DAO<T> where T : Purple_3.Skating
{
    public Purple_3_Participant_DAO[] Participants { get; set; }
    public double[] JudgeMood { get; set; }
    public string SkatingKind { get; set; }

    public Purple_3_Scating_DAO() { }
    public Purple_3_Scating_DAO(T skating)
    {
        Participants = skating.Participants.Select(p => new Purple_3_Participant_DAO(p)).ToArray();
        JudgeMood = skating.Moods;
        if (skating is Purple_3.IceSkating)
        {
            SkatingKind = nameof(Purple_3.IceSkating);
        }
        else if (skating is Purple_3.FigureSkating)
        {
            SkatingKind = nameof(Purple_3.FigureSkating);
        }
    }

    public T ToObject()
    {
        Purple_3.Skating s;
        if (SkatingKind == nameof(Purple_3.IceSkating))
        {
            s = new Purple_3.IceSkating(JudgeMood, false);
        }
        else if (SkatingKind == nameof(Purple_3.FigureSkating))
        {
            s = new Purple_3.FigureSkating(JudgeMood, false);
        }
        else return default;
        s.Add(Participants.Select(p => p.ToObject()).ToArray());
        return (T)s;
    }
}
