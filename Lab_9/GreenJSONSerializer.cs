



using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using Newtonsoft.Json;


namespace Lab_9
{
  public class GreenJSONSerializer : GreenSerializer
  {
    public override string Extension => "json";
    private class ParticipantDTO
    {
      public string Type { get; set; }
      public string Surname { get; set; }
      public string Group { get; set; }
      public string Trainer { get; set; }
      public double Result { get; set; }
      public bool HasPassed { get; set; }

      public ParticipantDTO() { }

      public ParticipantDTO(Green_1.Participant participant)
      {
        Type = participant.GetType().Name;
        Surname = participant.Surname;
        Group = participant.Group;
        Trainer = participant.Trainer;
        Result = participant.Result;
        HasPassed = participant.HasPassed;
      }
    }


    public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
    {
      if (participant == null || String.IsNullOrEmpty(fileName)) { return; };
      SelectFile(fileName);
      var participantDTO = new ParticipantDTO(participant);
      var json = JsonConvert.SerializeObject(participantDTO, Formatting.Indented);
      File.WriteAllText(FilePath, json);
    }











    private static int[][] MatrixToJaggedArr(int[,] matrix)
    {
      int rows = matrix.GetLength(0);
      int cols = matrix.GetLength(1);
      int[][] jagged = new int[rows][];
      for (int i = 0; i < rows; i++)
      {
        jagged[i] = new int[cols];
        for (int j = 0; j < cols; j++)
          jagged[i][j] = matrix[i, j];
      }
      return jagged;
    }

    private static int[,] JaggedArrToMatrix(int[][] arr)
    {
      int rows = arr.Length;
      int cols = arr[0].Length;
      int[,] matrix = new int[rows, cols];
      for (int i = 0; i < rows; i++)
      {
        for (int j = 0; j < cols; j++)
        {
          matrix[i, j] = arr[i][j];
        }
      }

      return matrix;
    }

    private class HumanDTO
    {

      public string Type { get; set; }
      public string Name { get; set; }
      public string Surname { get; set; }
      public double? AvgMark { get; set; }
      public bool? IsExcellent { get; set; }
      public int[]? Marks { get; set; }
      public HumanDTO() { }

      public HumanDTO(Green_2.Human h)
      {
        Type = h.GetType().Name;
        Name = h.Name;
        Surname = h.Surname;
        if (h is Green_2.Student s)
        {
          AvgMark = s.AvgMark;
          IsExcellent = s.IsExcellent;
          Marks = s.Marks;
        }
      }
    }

    public override void SerializeGreen2Human(Green_2.Human human, string fileName)
    {
      if (human == null || String.IsNullOrEmpty(fileName)) { return; };
      SelectFile(fileName);
      var human_dto = new HumanDTO(human);
      var json = JsonConvert.SerializeObject(human_dto);
      File.WriteAllText(FilePath, json);
    }









    private class StudentDTO
    {
      public string Type { get; set; }
      public string Name { get; set; }
      public string Surname { get; set; }
      public int ID { get; set; }
      public int[] Marks { get; set; }
      public bool IsExpelled { get; set; }
      public StudentDTO() { }

      public StudentDTO(Green_3.Student s)
      {
        Type = s.GetType().Name;
        Name = s.Name;
        Surname = s.Surname;
        Marks = s.Marks;
        ID = s.ID;
        IsExpelled = s.IsExpelled;
      }
    }

    public override void SerializeGreen3Student(Green_3.Student student, string fileName)
    {
      if (student == null || String.IsNullOrEmpty(fileName)) { return; }
      SelectFile(fileName);
      var student_dto = new StudentDTO(student);
      var json = JsonConvert.SerializeObject(student_dto, Formatting.Indented);
      File.WriteAllText(FilePath, json);
    }



    private class DisciplineDTO
    {
      public string Type { get; set; }
      public string Name { get; set; }
      public Participant2DTO[] Participants { get; set; }
      public DisciplineDTO() { }
      public DisciplineDTO(Green_4.Discipline d)
      {
        Type = d.GetType().Name;
        Name = d.Name;
        Participants = d.Participants.Select(p => new Participant2DTO(p)).ToArray(); ;
      }
    }
    private class Participant2DTO
    {
      public string Type { get; set; }
      public string Name { get; set; }
      public string Surname { get; set; }
      public double[] Jumps { get; set; }
      public double BestJump { get; set; }

      public Participant2DTO() { }
      public Participant2DTO(Green_4.Participant p)
      {
        Type = p.GetType().Name;
        Name = p.Name;
        Surname = p.Surname;
        Jumps = p.Jumps;
        BestJump = p.BestJump;
      }
    }

    public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
    {
      if (discipline == null || String.IsNullOrEmpty(fileName)) { return; }
      SelectFile(fileName);
      var dto = new DisciplineDTO(discipline);
      var json = JsonConvert.SerializeObject(dto);
      File.WriteAllText(FilePath, json);
    }



    private class Student2DTO
    {
      public string Type { get; set; }
      public string Name { get; set; }
      public string Surname { get; set; }
      public int[] Marks { get; set; }
      public double AvgMark { get; set; }

      public Student2DTO() { }
      public Student2DTO(Green_5.Student s)
      {
        Type = s.GetType().Name;
        Name = s.Name;
        Surname = s.Surname;
        Marks = s.Marks;
        AvgMark = s.AvgMark;
      }
    }


    private class GroupDTO
    {
      public string Type { get; set; }
      public string Name { get; set; }
      public double AvgMark { get; set; }
      public Student2DTO[] Students { get; set; }
      public GroupDTO() { }
      public GroupDTO(Green_5.Group g)
      {
        Type = g.GetType().Name;
        Name = g.Name;
        Students = g.Students.Select(s => new Student2DTO(s)).ToArray();
        if (g is Green_5.SpecialGroup g1)
          AvgMark = g1.AvgMark;
        else if (g is Green_5.EliteGroup g2)
        {
          AvgMark = g2.AvgMark;
        }
        else
        {
          AvgMark = g.AvgMark;
        }
      }
    }




    public override void SerializeGreen5Group<T>(T group, string fileName) where T : class
    {
      if (group == null || string.IsNullOrEmpty(fileName)) return;
      SelectFile(fileName);
      if (group is Green_5.Group g)
      {
        var dto = new GroupDTO(g);
        var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
        File.WriteAllText(FilePath, json);
      }
    }


    public override Green_1.Participant DeserializeGreen1Participant(string fileName)
    {
      SelectFile(fileName);
      Green_1.Participant deserialize1d = default(Green_1.Participant);
      string json = File.ReadAllText(FilePath);
      var dto = JsonConvert.DeserializeObject<ParticipantDTO>(json);
      if (dto.Type == "Participant100M")
      {
        deserialize1d = new Green_1.Participant100M(dto.Surname, dto.Group, dto.Trainer);
      }
      else
      {
        deserialize1d = new Green_1.Participant500M(dto.Surname, dto.Group, dto.Trainer);
      }

      deserialize1d.Run(dto.Result);

      return deserialize1d;
    }



    public override Green_2.Human DeserializeGreen2Human(string fileName)
    {
      SelectFile(fileName);
      string json = File.ReadAllText(FilePath);
      var dto = JsonConvert.DeserializeObject<HumanDTO>(json);
      Green_2.Human deserialized;
      if (dto.Type == "Student")
      {
        var student = new Green_2.Student(dto.Name, dto.Surname);
        if (dto.Marks != null)
        {
          foreach (var mark in dto.Marks)
          {
            student.Exam(mark);
          }
        }
        deserialized = student;
      }
      else
      {
        deserialized = new Green_2.Human(dto.Name, dto.Surname);
      }
      return deserialized;
    }
    public override Green_3.Student DeserializeGreen3Student(string fileName)
    {
      SelectFile(fileName);
      string json = File.ReadAllText(FilePath);
      var dto = JsonConvert.DeserializeObject<StudentDTO>(json);
      var student = new Green_3.Student(dto.Name, dto.Surname, dto.ID);
      if (dto.Marks != null)
      {
        foreach (var mark in dto.Marks)
        {
          student.Exam(mark);
        }
      }

      return student;
    }

    public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
    {
      SelectFile(fileName);
      string json = File.ReadAllText(FilePath);
      var dto = JsonConvert.DeserializeObject<DisciplineDTO>(json);
      Green_4.Discipline discipline;

      if (dto.Type == "LongJump")
      {
        discipline = new Green_4.LongJump();
      }
      else
      {
        discipline = new Green_4.HighJump();
      }

      if (dto.Participants != null)
      {
        foreach (var p in dto.Participants)
        {
          var participant = new Green_4.Participant(p.Name, p.Surname);
          if (p.Jumps != null)
          {
            foreach (var jump in p.Jumps)
            {
              participant.Jump(jump);
            }
          }
          discipline.Add(participant);
        }
      }

      return discipline;
    }

    public override T DeserializeGreen5Group<T>(string fileName) where T : class
    {
      SelectFile(fileName);
      string json = File.ReadAllText(FilePath);
      var dto = JsonConvert.DeserializeObject<GroupDTO>(json);
      Green_5.Group group;
      if (dto.Type == "EliteGroup")
      {
        group = new Green_5.EliteGroup(dto.Name);
      }
      else if (dto.Type == "SpecialGroup")
      {
        group = new Green_5.SpecialGroup(dto.Name);
      }
      else
      {
        group = new Green_5.Group(dto.Name);
      }
      if (dto.Students != null)
      {
        foreach (var studentDto in dto.Students)
        {
          var student = new Green_5.Student(studentDto.Name, studentDto.Surname);
          if (studentDto.Marks != null)
          {
            foreach (var mark in studentDto.Marks)
            {
              student.Exam(mark);
            }
          }
          group.Add(student);
        }
      }
      return group as T;
    }
  }
}