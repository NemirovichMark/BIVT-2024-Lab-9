using System;
using System.Xml;
using System.Xml.Serialization;
using Lab_7;
namespace Lab_9;

public class BlueXMLSerializer : BlueSerializer
{
    public override string Extension => "xml";
    
    public class Blue_1_ResponseDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Votes { get; set; }
        public string Surname { get; set; }
        public Blue_1_ResponseDTO() { }
        public Blue_1_ResponseDTO(Blue_1.Response response)
        {
            Type = response.GetType().Name;
            Name = response.Name;                
            Votes = response.Votes;
            if (response is Blue_1.HumanResponse human)
                Surname = human.Surname;
        }
    }
    public class Blue_2_ParticipantDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int[][] Marks { get; set; }
        public Blue_2_ParticipantDTO() { }
        public Blue_2_ParticipantDTO(Blue_2.Participant participant)
        {
            Name = participant.Name;
            Surname = participant.Surname;
            Marks = ToJaggedArray(participant.Marks);
        }
    }
    public class Blue_2_WaterJumpDTO
    {
        public string Type {get; set;}
        public string Name {get; set;}
        public int Bank {get; set;}
        public Blue_2_ParticipantDTO[] Participants {get; set;}
        public Blue_2_WaterJumpDTO() { }
        public Blue_2_WaterJumpDTO(Blue_2.WaterJump jump)
        {
            Type = jump.GetType().Name;
            Name = jump.Name;
            Bank = jump.Bank;
            Participants = jump.Participants.Select(p => new Blue_2_ParticipantDTO(p)).ToArray();
        }
    }
    public class Blue_3_ParticipantDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int[] Penalties { get; set; }
        public Blue_3_ParticipantDTO() { }
        public Blue_3_ParticipantDTO(Blue_3.Participant participant) 
        {
            Type = participant.GetType().Name;
            Name = participant.Name;
            Surname = participant.Surname;
            Penalties = participant.Penalties;
        }
    }
    public class Blue_4_TeamDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int[] Scores { get; set; }
        public Blue_4_TeamDTO() { }
        public Blue_4_TeamDTO(Blue_4.Team team) 
        {
            Type = team.GetType().Name;
            Name = team.Name;
            Scores = team.Scores;
        }
    }
    public class Blue_4_GroupDTO
    {
        public string Name { get; set; } 
        public Blue_4_TeamDTO[] ManTeams { get; set; }
        public Blue_4_TeamDTO[] WomanTeams { get; set; }

        public Blue_4_GroupDTO() { }
        public Blue_4_GroupDTO(Blue_4.Group group) 
        {
            Name = group.Name;
            ManTeams = group.ManTeams.Select(t => t == null ? null : new Blue_4_TeamDTO(t)).ToArray();
            WomanTeams = group.WomanTeams.Select(t => t == null ? null : new Blue_4_TeamDTO(t)).ToArray();
        }
    }
    public class Blue_5_ParticipantDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Place {  get; set; }

        public Blue_5_ParticipantDTO() { }
        public Blue_5_ParticipantDTO(Blue_5.Sportsman sportsman) 
        {
            Name= sportsman.Name;
            Surname= sportsman.Surname;
            Place= sportsman.Place;
        }
    }
    public class Blue_5_TeamDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public Blue_5_ParticipantDTO[] Sporsman { get; set; }
        public Blue_5_TeamDTO() { }
        public Blue_5_TeamDTO(Blue_5.Team team) 
        {
            Type = team.GetType().Name;
            Name = team.Name;
            Sporsman = team.Sportsmen.Select(p => p == null ? null : new Blue_5_ParticipantDTO(p)).ToArray();
        }
    }

    public override void SerializeBlue1Response(Blue_1.Response participant, string fileName) 
    {
        if (participant == null || string.IsNullOrEmpty(fileName))
            return;
         
        SelectFile(fileName);
        // не можем использовать анонимки так как к ним неприменимо typeof, делаем DTO
        Blue_1_ResponseDTO dto = new Blue_1_ResponseDTO(participant);
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            serializer.Serialize(writer, dto);
        }
    }
    public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName) 
    {
        if (participant == null || string.IsNullOrEmpty(fileName))
            return;
        
        SelectFile(fileName);
        Blue_2_WaterJumpDTO dto = new Blue_2_WaterJumpDTO(participant);
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            serializer.Serialize(writer, dto);
        }
    }
    private static int[][] ToJaggedArray(int[,] arr)
    {
        if (arr == null) 
            return null;
        int[][] newArr = new int[arr.GetLength(0)][];
        for (int i = 0; i<newArr.Length; i++)
        {
            newArr[i] = new int[arr.GetLength(1)];
            for (int j = 0; j < newArr[i].Length; j++)
                newArr[i][j] = arr[i, j];
        }
        return newArr;
    }
    public override void SerializeBlue3Participant<T>(T student, string fileName) 
    {
        if (student == null || string.IsNullOrEmpty(fileName))
            return;
        
        SelectFile(fileName);
        Blue_3_ParticipantDTO dto = new Blue_3_ParticipantDTO(student);
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            serializer.Serialize(writer, dto);
        }
    }
    public override void SerializeBlue4Group(Blue_4.Group participant, string fileName) 
    {
        if (participant == null || string.IsNullOrEmpty(fileName))
            return;
        SelectFile(fileName);
        Blue_4_GroupDTO dto = new Blue_4_GroupDTO(participant);
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            serializer.Serialize(writer, dto);
        }
    }
    public override void SerializeBlue5Team<T>(T group, string fileName) 
    {
        if (group == null || string.IsNullOrEmpty(fileName))
            return;
        SelectFile(fileName);
        Blue_5_TeamDTO dto = new Blue_5_TeamDTO(group);
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_5_TeamDTO));
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            serializer.Serialize(writer, dto);
        }
    }


    public override Blue_1.Response DeserializeBlue1Response(string fileName) 
    {
        // typeof() response humanresponse - Name: (Surname:) Votes:
        if (string.IsNullOrEmpty(fileName))
            return default(Blue_1.Response);
        
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));

        Blue_1_ResponseDTO dto;
        using (StreamReader reader = new StreamReader(fileName))
        {
            dto = (Blue_1_ResponseDTO)serializer.Deserialize(reader);
        }
        Blue_1.Response res;
        if (dto.Surname != null)
            res = new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes);
        else
            res = new Blue_1.Response(dto.Name, dto.Votes);
        return res;
    }
    public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName) 
    {
        // participant abstr 3m 5m - name bank part[]
        if (string.IsNullOrEmpty(fileName))
            return default(Blue_2.WaterJump);
        
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));
        Blue_2_WaterJumpDTO dto;
        using (StreamReader reader = new StreamReader(fileName))
        {
            dto = (Blue_2_WaterJumpDTO)serializer.Deserialize(reader);
        }

        Blue_2.WaterJump jump;
        if (dto.Type == "WaterJump3m")
            jump = new Blue_2.WaterJump3m(dto.Name, dto.Bank);
        else 
            jump = new Blue_2.WaterJump5m(dto.Name, dto.Bank);

        foreach (Blue_2_ParticipantDTO part in dto.Participants)
        {
            Blue_2.Participant participant = new Blue_2.Participant(part.Name, part.Surname);
            foreach (var j in part.Marks)
                participant.Jump(j);
            jump.Add(participant);
        }
        return jump;
    }
    public override T DeserializeBlue3Participant<T>(string fileName) 
    {
        if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath))
            return default(T);
        
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));
        Blue_3_ParticipantDTO dto;
        using (StreamReader reader = new StreamReader(fileName))
        {
            dto = (Blue_3_ParticipantDTO)serializer.Deserialize(reader);
        }

        Blue_3.Participant student;
        if (dto.Type == "BasketballPlayer")
            student = new Blue_3.BasketballPlayer(dto.Name, dto.Surname);
        else if (dto.Type == "HockeyPlayer")
            student = new Blue_3.HockeyPlayer(dto.Name, dto.Surname);
        else
            student = new Blue_3.Participant(dto.Name, dto.Surname);
        foreach (int penalty in dto.Penalties)
            student.PlayMatch(penalty);
        return (T)(object)student;
    }
    public override Blue_4.Group DeserializeBlue4Group(string fileName) 
    {
        // name manteam womanteam ; team - name scores[]
        if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath))
            return default(Blue_4.Group);
        
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));
        Blue_4_GroupDTO dto;
        using (StreamReader reader = new StreamReader(fileName))
        {
            dto = (Blue_4_GroupDTO)serializer.Deserialize(reader);
        }

        Blue_4.Group group = new Blue_4.Group(dto.Name);
        foreach (var man in dto.ManTeams)
        {
            if (man == null) continue;
            Blue_4.ManTeam manTeam = new Blue_4.ManTeam(man.Name);
            foreach (int score in man.Scores)
                manTeam.PlayMatch(score);
            group.Add(manTeam);
        }
        foreach (var woman in dto.WomanTeams)
        {
            if (woman == null) continue;
            Blue_4.WomanTeam womanTeam = new Blue_4.WomanTeam(woman.Name);
            foreach (int score in woman.Scores)
                womanTeam.PlayMatch(score);
            group.Add(womanTeam);
        }
        return group;
    }
    public override T DeserializeBlue5Team<T>(string fileName) 
    {
        // abstr manteam womanteam - name sportsman[], sportsmen - name surname place
        if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath))
            return default(T);
        
        XmlSerializer serializer = new XmlSerializer(typeof(Blue_5_TeamDTO));
        Blue_5_TeamDTO dto;
        using(StreamReader reader = new StreamReader(fileName))
        {
            dto = (Blue_5_TeamDTO)serializer.Deserialize(reader);
        }

        Blue_5.Team team;
        if (dto.Type == "ManTeam")
            team = new Blue_5.ManTeam(dto.Name);
        else
            team = new Blue_5.WomanTeam(dto.Name);
        foreach (var sport in dto.Sporsman)
        {
            if (sport == null) continue;
            Blue_5.Sportsman sportsman = new Blue_5.Sportsman(sport.Name, sport.Surname);
            sportsman.SetPlace(sport.Place);
            team.Add(sportsman);
        }
        return (T)(object)team;
    }
}