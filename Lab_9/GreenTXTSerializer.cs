
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
// using Internal;
using System.Globalization;

using Lab_7;

namespace Lab_9
{

  public class GreenTXTSerializer : GreenSerializer
  {
    public override string Extension => "txt";

    public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
    {
      if (participant == null || string.IsNullOrEmpty(fileName)) return;
      SelectFile(fileName);
      File.WriteAllText(FilePath, string.Empty);

      using (StreamWriter writer = File.AppendText(FilePath))
      {
        writer.WriteLine($"Type: {participant.GetType().Name}");
        writer.WriteLine($"Surname: {participant.Surname}");
        writer.WriteLine($"Group: {participant.Group}");
        writer.WriteLine($"Trainer: {participant.Trainer}");
        writer.WriteLine($"Result: {participant.Result}");
        writer.WriteLine($"HasPassed: {participant.HasPassed}");
      }
    }

    public override void SerializeGreen2Human(Green_2.Human human, string fileName)
    {
      if (human == null || string.IsNullOrEmpty(fileName)) return;
      SelectFile(fileName);
      File.WriteAllText(FilePath, string.Empty);

      using (StreamWriter writer = File.AppendText(FilePath))
      {
        writer.WriteLine($"Type: {human.GetType().Name}");
        writer.WriteLine($"Name: {human.Name}");
        writer.WriteLine($"Surname: {human.Surname}");

        if (human is Green_2.Student h)
        {
          writer.WriteLine($"AvgMark: {h.AvgMark}");
          writer.WriteLine($"IsExcellent: {h.IsExcellent}");

          var marks = h.Marks;
          string markStr = marks != null && marks.Length > 0
              ? "[" + string.Join(",", marks) + "]"
              : "[]";
          writer.WriteLine($"Marks: {markStr}");
        }
      }
    }


    public override void SerializeGreen3Student(Green_3.Student student, string fileName)
    {
      if (student == null || string.IsNullOrEmpty(fileName)) return;
      SelectFile(fileName);
      File.WriteAllText(FilePath, string.Empty);

      using (StreamWriter writer = File.AppendText(FilePath))
      {
        writer.WriteLine($"Type: {student.GetType().Name}");
        writer.WriteLine($"Name: {student.Name}");
        writer.WriteLine($"Surname: {student.Surname}");
        writer.WriteLine($"ID: {student.ID}");
        writer.WriteLine($"IsExpelled: {student.IsExpelled}");
        writer.WriteLine($"AvgMark: {student.AvgMark}");

        var marks = student.Marks;
        string markStr = marks != null && marks.Length > 0
            ? "[" + string.Join(",", marks) + "]"
            : "[]";
        writer.WriteLine($"Marks: {markStr}");
      }
    }


    private void SerializeGreen4Participant(Green_4.Participant p, StreamWriter writer)
    {
      writer.WriteLine("Participant:");
      writer.WriteLine($"Name: {p.Name}");
      writer.WriteLine($"Surname: {p.Surname}");
      writer.WriteLine($"BestJump: {p.BestJump}");
      var jumps = p.Jumps ?? Array.Empty<double>();
      string jumpStr = "[" + string.Join(",", jumps.Select(j => j.ToString(CultureInfo.InvariantCulture))) + "]";
      writer.WriteLine($"Jumps: {jumpStr}");
      writer.WriteLine(); // пустая строка для разделения
    }

    public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
    {
      if (discipline == null || string.IsNullOrEmpty(fileName)) return;
      SelectFile(fileName);
      File.WriteAllText(FilePath, string.Empty);

      using (StreamWriter writer = File.AppendText(FilePath))
      {
        writer.WriteLine($"Type: {discipline.GetType().Name}");
        writer.WriteLine($"Name: {discipline.Name}");
        writer.WriteLine();

        foreach (var p in discipline.Participants)
        {
          SerializeGreen4Participant(p, writer);
        }
      }
    }

    private void SerializeGreen5Student(Green_5.Student s, StreamWriter writer)
    {
      writer.WriteLine("Student:");
      writer.WriteLine($"Name: {s.Name}");
      writer.WriteLine($"Surname: {s.Surname}");

      string marksStr = s.Marks != null && s.Marks.Length > 0
          ? "[" + string.Join(",", s.Marks) + "]"
          : "[]";

      writer.WriteLine($"Marks: {marksStr}");
      writer.WriteLine(); // Пустая строка между студентами
    }

    public override void SerializeGreen5Group<T>(T group, string fileName) where T : class
    {
      if (group == null || string.IsNullOrEmpty(fileName)) return;
      SelectFile(fileName);
      File.WriteAllText(FilePath, string.Empty);

      using (StreamWriter writer = File.AppendText(FilePath))
      {
        writer.WriteLine($"Type: {group.GetType().Name}");
        writer.WriteLine($"Name: {group.Name}");
        writer.WriteLine(); // отделим заголовок

        foreach (var s in group.Students)
        {
          SerializeGreen5Student(s, writer);
        }
      }
    }



    public override Green_1.Participant DeserializeGreen1Participant(string fileName)
    {
      SelectFile(fileName);
      string text = File.ReadAllText(FilePath);
      Dictionary<string, string> participant = new Dictionary<string, string>();
      foreach (var row in text.Split(Environment.NewLine))
      {
        if (row.Contains(':'))
        {
          var field = row.Split(new[] { ':' }, 2);
          if (field.Length == 2)
          {
            participant[field[0].Trim()] = field[1].Trim();
          }
        }
      }

      if (!participant.TryGetValue("Type", out var type)) return null;
      if (!participant.TryGetValue("Surname", out var surname)) return null;
      if (!participant.TryGetValue("Group", out var group)) return null;
      if (!participant.TryGetValue("Trainer", out var trainer)) return null;

      Green_1.Participant p = type switch
      {
        "Participant100M" => new Green_1.Participant100M(surname, group, trainer),
        "Participant500M" => new Green_1.Participant500M(surname, group, trainer),
        _ => null
      };

      if (p != null && participant.TryGetValue("Result", out var resultStr) && double.TryParse(resultStr, out var result))
      {
        p.Run(result);
      }
      return p;
    }


    public override Green_2.Human DeserializeGreen2Human(string fileName)
    {
      SelectFile(fileName);
      string[] text = File.ReadAllLines(FilePath);
      Dictionary<string, string> human = new Dictionary<string, string>();
      foreach (var row in text)
      {
        if (row.Contains(":"))
        {
          var field = row.Split(':', 2);
          if (field.Length == 2)
            human[field[0].Trim()] = field[1].Trim();
        }
      }

      if (!human.TryGetValue("Type", out var type)) return null;
      if (!human.TryGetValue("Name", out var name)) return null;
      if (!human.TryGetValue("Surname", out var surname)) return null;
      if (type == "Student")
      {
        Green_2.Student student = new Green_2.Student(name, surname);

        if (human.TryGetValue("Marks", out var marksRaw) &&
            marksRaw.StartsWith("[") && marksRaw.EndsWith("]"))
        {
          var inner = marksRaw.Substring(1, marksRaw.Length - 2); // убираем []
          var markStrings = inner.Split(',', StringSplitOptions.RemoveEmptyEntries);

          foreach (var markStr in markStrings)
          {
            if (int.TryParse(markStr, out int mark))
              student.Exam(mark);
          }
        }

        return student;
      }
      return new Green_2.Human(name, surname);
    }

    public override Green_3.Student DeserializeGreen3Student(string fileName)
    {
      SelectFile(fileName);
      string[] text = File.ReadAllLines(FilePath);
      Dictionary<string, string> studentDict = new Dictionary<string, string>();
      foreach (string row in text)
      {
        if (row.Contains(":"))
        {
          var field = row.Split(new[] { ':' }, 2);
          if (field.Length == 2)
          {
            studentDict[field[0].Trim()] = field[1].Trim();
          }
        }
      }

      if (!studentDict.TryGetValue("Name", out string name)) return null;
      if (!studentDict.TryGetValue("Surname", out string surname)) return null;
      if (!studentDict.TryGetValue("ID", out string idStr) || !int.TryParse(idStr, out int id)) return null;

      var student = new Green_3.Student(name, surname, id);

      if (studentDict.TryGetValue("Marks", out string markLine))
      {
        markLine = markLine.Trim('[', ']');
        var markStrings = markLine.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var markStr in markStrings)
        {
          if (int.TryParse(markStr, out int mark))
          {
            student.Exam(mark);
          }
        }
      }

      return student;
    }


    public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
    {
      SelectFile(fileName);
      string[] lines = File.ReadAllLines(FilePath);

      Green_4.Discipline discipline = null;
      int i = 0;

      if (lines[i].StartsWith("Type: "))
      {
        string type = lines[i++].Substring("Type: ".Length).Trim();
        if (type == nameof(Green_4.LongJump))
          discipline = new Green_4.LongJump();
        else if (type == nameof(Green_4.HighJump))
          discipline = new Green_4.HighJump();
        else
          return null;
      }

      if (i < lines.Length && lines[i].StartsWith("Name: "))
        i++;

      while (i < lines.Length)
      {
        if (lines[i].Trim() == "Participant:")
        {
          if (i + 4 >= lines.Length) break;

          string nameLine = lines[++i];
          string surnameLine = lines[++i];
          string bestJumpLine = lines[++i];
          string jumpsLine = lines[++i];
          i++; // пустая строка

          if (!nameLine.StartsWith("Name: ") ||
              !surnameLine.StartsWith("Surname: ") ||
              !bestJumpLine.StartsWith("BestJump: ") ||
              !jumpsLine.StartsWith("Jumps: "))
            continue;

          string name = nameLine.Substring("Name: ".Length).Trim();
          string surname = surnameLine.Substring("Surname: ".Length).Trim();

          string jumpsRaw = jumpsLine.Substring("Jumps: ".Length).Trim('[', ']');
          double[] jumps = jumpsRaw
              .Split(',', StringSplitOptions.RemoveEmptyEntries)
              .Select(s =>
              {
                if (double.TryParse(s.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                  return d;
                return 0;
              })
              .ToArray();

          Array.Resize(ref jumps, 3);

          var participant = new Green_4.Participant(name, surname);
          int written = 0;

          for (int j = 0; j < 3; j++)
          {
            if (jumps[j] != 0 || written < j)
            {
              participant.Jump(jumps[j] != 0 ? jumps[j] : 0.000001);
              written++;
            }
          }
          discipline?.Add(participant);
        }
        else
        {
          i++;
        }
      }
      return discipline;
    }

    public override T DeserializeGreen5Group<T>(string fileName)
    {
      SelectFile(fileName);
      if (!File.Exists(FilePath)) return null;

      string[] lines = File.ReadAllLines(FilePath);
      string groupType = "";
      string groupName = "";
      int i = 0;

      while (i < lines.Length)
      {
        string line = lines[i].Trim();

        if (line.StartsWith("Type:"))
        {
          groupType = line.Split(':', 2)[1].Trim();
        }
        else if (line.StartsWith("Name:"))
        {
          groupName = line.Split(':', 2)[1].Trim();
        }
        else if (string.IsNullOrWhiteSpace(line))
        {
          i++;
          break;
        }
        i++;
      }

      Green_5.Group group = groupType switch
      {
        "Group" => new Green_5.Group(groupName),
        "EliteGroup" => new Green_5.EliteGroup(groupName),
        "SpecialGroup" => new Green_5.SpecialGroup(groupName),
        _ => null
      };

      if (group == null) return null;

      while (i < lines.Length)
      {
        if (lines[i].Trim() == "Student:")
        {
          string name = "";
          string surname = "";
          int[] marks = Array.Empty<int>();

          i++;
          while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
          {
            string line = lines[i].Trim();

            if (line.StartsWith("Name:"))
            {
              name = line.Split(':', 2)[1].Trim();
            }
            else if (line.StartsWith("Surname:"))
            {
              surname = line.Split(':', 2)[1].Trim();
            }
            else if (line.StartsWith("Marks:"))
            {
              string raw = line.Split(':', 2)[1].Trim().Trim('[', ']');
              string[] markParts = raw.Split(',', StringSplitOptions.RemoveEmptyEntries);
              marks = new int[markParts.Length];
              for (int j = 0; j < markParts.Length; j++)
              {
                int.TryParse(markParts[j], out marks[j]);
              }
            }
            i++;
          }
          Green_5.Student student = new Green_5.Student(name, surname);
          foreach (int mark in marks)
          {
            student.Exam(mark);
          }
          group.Add(student);
        }
        i++;
      }
      return group as T;
    }
  }
}