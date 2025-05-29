using Lab_7;
namespace Lab_9;

public class Purple_1_Participant_DTO
{
    private const int MARKS_COL_COUNT = 7;
    private const int MARKS_ROW_COUNT = 4;


    public Purple_1_Participant_DTO() { }

    public Purple_1_Participant_DTO(string name, string surname, double[] coefs, int[,] marks)
    {
        Name = name;
        Surname = surname;
        Coefs = coefs;
        Marks = new int[4][];

        for (int i = 0; i < 4; i++)
        {
            Marks[i] = new int[7];
            for (int j = 0; j < 7; j++)
            {
                Marks[i][j] = marks[i, j];
            }
        }
    }

    public Purple_1_Participant_DTO(Purple_1.Participant participant)
    {
        Name = participant.Name;
        Surname = participant.Surname;
        Coefs = participant.Coefs;
        Marks = new int[4][];

        for (int i = 0; i < 4; i++)
        {
            Marks[i] = new int[7];
            for (int j = 0; j < 7; j++)
            {
                Marks[i][j] = participant.Marks[i, j];
            }
        }
    }

    public Purple_1.Participant ToObject(bool doJump = true)
    {
        var p = new Purple_1.Participant(Name, Surname);
        p.SetCriterias(Coefs);
        if (doJump)
        {
            foreach (int[] marks in Marks)
            {
                p.Jump(marks);
            }
        }
        return p;
    }

    public string Name { get; set; }
    public string Surname { get; set; }
    public double[] Coefs { get; set; }
    public int[][] Marks { get; set; }
}

public class Purple_1_Judge_DTO
{
    public Purple_1_Judge_DTO() { }
    public Purple_1_Judge_DTO(string name, int[] marks)
    {
        Name = name;
        FavMarks = marks;
    }

    public Purple_1_Judge_DTO(Purple_1.Judge judge)
    {
        Name = judge.Name;
        FavMarks = judge.Marks;
    }

    public Purple_1.Judge ToObject()
    {
        return new Purple_1.Judge(Name, FavMarks);
    }

    public string Name { get; set; }
    public int[] FavMarks { get; set; }
}

public class Purple_1_Competition_DTO
{
    public Purple_1_Competition_DTO() { }
    public Purple_1_Competition_DTO(Purple_1_Participant_DTO[] participants, Purple_1_Judge_DTO[] judges)
    {
        Participants = participants;
        Judges = judges;
    }

    public Purple_1_Competition_DTO(Purple_1.Competition competition)
    {
        // Console.WriteLine("TO SERIALIZE");
        // competition.Print();
        Participants = competition.Participants.Select(p => new Purple_1_Participant_DTO(p)).ToArray();
        Judges = competition.Judges.Select(j => new Purple_1_Judge_DTO(j)).ToArray();
    }

    public Purple_1.Competition ToObject()
    {
        var c = new Purple_1.Competition(Judges.Select(j => j.ToObject()).ToArray());
        foreach (var participant in Participants)
        {
            var p = participant.ToObject(false);
            p.Jump(participant.Marks[0]);
            c.Add(p);
        }
        // Console.WriteLine("DESERIALIZED");
        // c.Print();
        return c;
    }

    public Purple_1_Participant_DTO[] Participants { get; set; }
    public Purple_1_Judge_DTO[] Judges { get; set; }
}


public class Purple_2_Participant_DTO
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int[] Marks { get; set; }
    public int Distance { get; set; }

    public Purple_2_Participant_DTO() { }
    public Purple_2_Participant_DTO(string name, string surname, int[] marks, int distance)
    {
        Name = name;
        Surname = surname;
        Marks = marks;
        Distance = distance;
    }

    public Purple_2_Participant_DTO(Purple_2.Participant participant)
    {
        Name = participant.Name;
        Surname = participant.Surname;
        Marks = participant.Marks;
        Distance = participant.Distance;
    }

    public Purple_2.Participant ToObject(int target = 0)
    {
        var p = new Purple_2.Participant(Name, Surname);
        p.Jump(Distance, Marks, target);
        return p;
    }

}
public class Purple_2_Ski_Jumping_DTO
{
    public Purple_2_Ski_Jumping_DTO() { }
    public Purple_2_Ski_Jumping_DTO(string name, int distance, Purple_2_Participant_DTO[] participants)
    {
        Name = name;
        Distance = distance;
        Participants = participants;
    }
    public string Name { get; set; }
    public int Distance { get; set; }
    public Purple_2_Participant_DTO[] Participants { get; set; }

    public Purple_2.SkiJumping ToObject()
    {
        Purple_2.SkiJumping sj;
        if (Name == "100m")
        {
            sj = new Purple_2.JuniorSkiJumping();
        }
        else if (Name == "150m")
        {
            sj = new Purple_2.ProSkiJumping();

        }
        else { return default; }
        sj.Add(Participants.Select(p => p.ToObject(Distance)).ToArray());
        return sj;
    }

}



public class Purple_3_Participant_DTO
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public double[] Marks { get; set; }

    public Purple_3_Participant_DTO() { }
    public Purple_3_Participant_DTO(Purple_3.Participant p)
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
        return p;
    }
}

public class Purple_3_Scating_DTO<T> where T : Purple_3.Skating
{
    public Purple_3_Participant_DTO[] Participants { get; set; }
    public double[] JudgeMood { get; set; }
    public string SkatingKind { get; set; }

    public Purple_3_Scating_DTO() { }
    public Purple_3_Scating_DTO(T skating)
    {
        Participants = skating.Participants.Select(p => new Purple_3_Participant_DTO(p)).ToArray();
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
        Purple_3.Participant[] participants = Participants.Select(p => p.ToObject()).ToArray();
        s.Add(participants);
        return (T)s;
    }
}



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



public class Purple_5_Response_DTO
{
    public string Animal { get; set; }
    public string CharacterTrait { get; set; }
    public string Concept { get; set; }

    public Purple_5_Response_DTO() { }
    public Purple_5_Response_DTO(Purple_5.Response response)
    {
        Animal = response.Animal;
        CharacterTrait = response.CharacterTrait;
        Concept = response.Concept;
    }

    public Purple_5.Response ToObject()
    {
        return new Purple_5.Response(Animal, CharacterTrait, Concept);
    }
}

public class Purple_5_Research_DTO
{
    public string Name { get; set; }
    public Purple_5_Response_DTO[] Responses { get; set; }

    public Purple_5_Research_DTO() { }
    public Purple_5_Research_DTO(Purple_5.Research research)
    {
        Name = research.Name;
        Responses = research.Responses.Select(r => new Purple_5_Response_DTO(r)).ToArray();
    }

    public Purple_5.Research ToObject()
    {
        var r = new Purple_5.Research(Name);
        // Responses.Select(resp =>
        // {
        //     r.Add(new string[] { resp.Animal, resp.CharacterTrait, resp.Concept });
        //     return 0;
        // });
        foreach (var resp in Responses)
        {
            r.Add(new string[] { resp.Animal, resp.CharacterTrait, resp.Concept });
        }

        return r;
    }
}

public class Purple_5_Report_DTO
{
    public Purple_5_Research_DTO[] Researches { get; set; }

    public Purple_5_Report_DTO() { }
    public Purple_5_Report_DTO(Purple_5.Report report)
    {
        Researches = report.Researches.Select(r => new Purple_5_Research_DTO(r)).ToArray();
    }

    public Purple_5.Report ToObject()
    {
        var report = new Purple_5.Report();
        foreach (var rDto in Researches)
        {
            var research = rDto.ToObject();
            report.AddResearch(research);
        }
        return report;
    }
}
