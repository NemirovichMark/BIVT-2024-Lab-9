using Lab_7;
namespace Lab_9;

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
        Responses.Select(resp =>
        {
            r.Add(new string[] { resp.Animal, resp.CharacterTrait, resp.Concept });
            return 0;
        });
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
        report.AddResearch(Researches.Select(r => r.ToObject()).ToArray());
        return report;
    }
}
