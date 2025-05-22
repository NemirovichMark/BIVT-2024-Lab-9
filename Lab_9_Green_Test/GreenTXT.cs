using Lab_7;
using Lab_9;
using System.IO;
using Lab_9_Green_Test;
using static Lab_7.Green_4;
using Mono.Cecil;

namespace Lab_9_GreenTXT
{
    [TestClass]
    public sealed class GreenJSON
    {
        private Random _rand = new Random();
        GreenSerializer _serializer = new GreenTXTSerializer();

        [TestMethod]
        public void Test_00_OOP()
        {
            var props = new (string, string[], Type)[]
            {
                new ("FolderPath", new string[] { "virtual" }, typeof(string)),
                new ("FilePath", new string[] { "virtual" }, typeof(string)),
                new ("Extension", new string[] {"abstract" }, typeof(string))
            };
            var methods = new (string, string[], Type, Type[])[]
            {
                new ("SelectFile", new string[] { "virtual" }, typeof(void), new Type[] {typeof(string) }),
                new ("SelectFolder", new string[] { "virtual" }, typeof(void), new Type[] {typeof(string) }),
                new ("SerializeGreen1Participant", new string[] { "virtual" }, typeof(void), new Type[] {typeof(Green_1.Participant), typeof(string) }),
                new ("SerializeGreen2Human", new string[] { "virtual" }, typeof(void), new Type[] { typeof(Green_2.Human), typeof(string) }),
                new ("SerializeGreen3Student", new string[] { "virtual" }, typeof(void), new Type[] { typeof(Green_3.Student), typeof(string) }),
                new ("SerializeGreen4Discipline", new string[] { "virtual" }, typeof(void), new Type[] { typeof(Green_4.Discipline), typeof(string) }),
                new ("SerializeGreen5Group", new string[] { "virtual", "generic" }, typeof(void), new Type[] { typeof(Green_5.Group), typeof(string) }),
                new ("DeserializeGreen1Participant", new string[] { "virtual" }, typeof(Green_1.Participant), new Type[] {typeof(string) }),
                new ("DeserializeGreen2Human", new string[] { "virtual" }, typeof(Green_2.Human), new Type[] {typeof(string) }),
                new ("DeserializeGreen3Student", new string[] { "virtual" }, typeof(Green_3.Student), new Type[] {typeof(string) }),
                new ("DeserializeGreen4Discipline", new string[] { "virtual" }, typeof(Green_4.Discipline), new Type[] {typeof(string) }),
                new ("DeserializeGreen5Group", new string[] {"virtual", "generic" }, typeof(Green_5.Group), new Type[] {typeof(string) }),
            };
            General.CheckOOP(_serializer, props, methods);
        }
        [TestMethod]
        public void Test_01_Hierarchy()
        {
            Assert.IsTrue(_serializer is IFileManager);
            Assert.IsTrue(_serializer is FileSerializer);
            Assert.IsTrue(_serializer is GreenSerializer);
            Assert.IsTrue(_serializer is GreenTXTSerializer);
            Assert.IsFalse(_serializer is GreenJSONSerializer);
        }
        [TestMethod]
        public void Test_02_CreateFolder()
        {
            Assert.IsTrue(IFileManagerTest.Check_CreateFolder(_serializer, $"Green_TXT"));
            CheckFolder();
        }
        [TestMethod]
        public void Test_03_CreateFile()
        {
            Assert.IsTrue(IFileManagerTest.Check_CreateFile(_serializer, $"Green_TXT", $"Green_task", "txt"));
            CheckFile();
        }
        [TestMethod]
        public void Test_04_Green1_TXT()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Green1_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeGreen1Participant(original[i], $"Green_1_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Green_1.Participant[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeGreen1Participant($"Green_1_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_05_Green2_TXT()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Green2_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeGreen2Human(original[i], $"Green_2_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Green_2.Human[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeGreen2Human($"Green_2_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_06_Green3_TXT()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Green3_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeGreen3Student(original[i], $"Green_3_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Green_3.Student[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeGreen3Student($"Green_3_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_07_Green4_TXT()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Green4_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeGreen4Discipline(original[i], $"Green_4_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Green_4.Discipline[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeGreen4Discipline($"Green_4_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_08_Green5_TXT()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Green5_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeGreen5Group(original[i], $"Green_5_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Green_5.Group[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeGreen5Group<Green_5.Group>($"Green_5_{i + 1}");
            }
            Check(original, restored);
        }
        private void Init()
        {
            _serializer = new GreenTXTSerializer();
        }
        private void CheckFolder()
        {
            var pathes = IFileManagerTest.Check_Properties(_serializer);
            Assert.AreEqual(pathes.folder, Path.Combine(IFileManagerTest.GeneralPath, $"Green_TXT"));
            Assert.AreEqual(pathes.file, null);
        }

        private void CheckFile()
        {
            var pathes = IFileManagerTest.Check_Properties(_serializer);
            Assert.AreEqual(pathes.folder, Path.Combine(IFileManagerTest.GeneralPath, $"Green_TXT"));
            Assert.AreEqual(pathes.file, Path.Combine(IFileManagerTest.GeneralPath, $"Green_TXT", $"Green_task.txt"));
        }
        private Green_1.Participant[] Create_Green1_Array()
        {
            var participants = new Green_1.Participant[10]
            {
                new Green_1.Participant100M("Petrovich", "CSKA", "Roman"),
                new Green_1.Participant100M("Nikolayevich", "CSKA", "Roman"),
                new Green_1.Participant100M("Egorovich", "Gorniy", "Sergey"),
                new Green_1.Participant100M("Vasiliyevich", "Gorniy", "Sergey"),
                new Green_1.Participant100M("Nikolayevna", "Dinamo", "Sergey"),
                new Green_1.Participant500M("Vadimovna", "Dinamo", "Peter"),
                new Green_1.Participant500M("Petrovich", "CSKA", "Roman"),
                new Green_1.Participant500M("Nikolayevich", "CSKA", "Roman"),
                new Green_1.Participant500M("Egorovich", "Gorniy", "Sergey"),
                new Green_1.Participant500M("Vasiliyevich", "Gorniy", "Sergey")
            };
            foreach (var item in participants)
            {
                item.Run(Math.Round(_rand.NextDouble() * 10, 2));
                item.Run(Math.Round(_rand.NextDouble() * 10, 2));
            }
            return participants;
        }
        private Green_2.Human[] Create_Green2_Array()
        {
            var students = new Green_2.Human[10]
            {
                new Green_2.Human("Vasya", "Petrovich"),
                new Green_2.Human("Petya", "Nikolayevich"),
                new Green_2.Human("Kolya", "Vadimovich"),
                new Green_2.Human("Vadim", "Maratovich"),
                new Green_2.Human("Marat", "Danilovich"),
                new Green_2.Student("Danil", "Romanovich"),
                new Green_2.Student("Roma", "Egorovich"),
                new Green_2.Student("Egor", "Vasiliyevich"),
                new Green_2.Student("Masha", "Nikolayevna"),
                new Green_2.Student("Dasha", "Vadimovna")
            };
            foreach (var item in students)
            {
                if (item is Green_2.Student student)
                    for (global::System.Int32 i = 0; i < 10; i++)
                    {
                        var mark = _rand.Next(0, 6);
                        student.Exam(mark > 1 ? mark : 0);
                    }
            }
            return students;
        }
        private Green_3.Student[] Create_Green3_Array()
        {
            var students = new Green_3.Student[10]
            {
                new Green_3.Student("Vasya", "Petrovich"),
                new Green_3.Student("Petya", "Nikolayevich"),
                new Green_3.Student("Kolya", "Vadimovich"),
                new Green_3.Student("Vadim", "Maratovich"),
                new Green_3.Student("Marat", "Danilovich"),
                new Green_3.Student("Danil", "Romanovich"),
                new Green_3.Student("Roma", "Egorovich"),
                new Green_3.Student("Egor", "Vasiliyevich"),
                new Green_3.Student("Masha", "Nikolayevna"),
                new Green_3.Student("Dasha", "Vadimovna")
            };
            foreach (var item in students)
            {
                for (global::System.Int32 i = 0; i < 10; i++)
                {
                    var mark = _rand.Next(0, 6);
                    item.Exam(mark > 1 ? mark : 0);
                }
            }
            return students;
        }
        private Green_4.Discipline[] Create_Green4_Array()
        {
            var participants = new Green_4.Participant[10]
            {
                new Green_4.Participant("Vasya", "Petrovich"),
                new Green_4.Participant("Petya", "Nikolayevich"),
                new Green_4.Participant("Kolya", "Vadimovich"),
                new Green_4.Participant("Vadim", "Maratovich"),
                new Green_4.Participant("Marat", "Danilovich"),
                new Green_4.Participant("Danil", "Romanovich"),
                new Green_4.Participant("Roma", "Egorovich"),
                new Green_4.Participant("Egor", "Vasiliyevich"),
                new Green_4.Participant("Masha", "Nikolayevna"),
                new Green_4.Participant("Dasha", "Vadimovna")
            };
            foreach (var item in participants)
            {
                for (global::System.Int32 i = 0; i < 10; i++)
                {
                    item.Jump(Math.Round(_rand.NextDouble() * 10, 2));
                    item.Jump(Math.Round(_rand.NextDouble() * 10, 2));
                    item.Jump(Math.Round(_rand.NextDouble() * 10, 2));
                }
            }
            var disciplines = new Green_4.Discipline[10]
            {
                new Green_4.HighJump(),
                new Green_4.HighJump(),
                new Green_4.HighJump(),
                new Green_4.HighJump(),
                new Green_4.HighJump(),
                new Green_4.LongJump(),
                new Green_4.LongJump(),
                new Green_4.LongJump(),
                new Green_4.LongJump(),
                new Green_4.LongJump()
            };
            for (int i = 0; i < disciplines.Length; i++)
            {
                disciplines[i].Add(participants.Take(i).ToArray());
            }
            return disciplines;
        }
        private Green_5.Group[] Create_Green5_Array()
        {
            var students = new Green_5.Student[10]
            {
                new Green_5.Student("Vasya", "Petrovich"),
                new Green_5.Student("Petya", "Nikolayevich"),
                new Green_5.Student("Kolya", "Vadimovich"),
                new Green_5.Student("Vadim", "Maratovich"),
                new Green_5.Student("Marat", "Danilovich"),
                new Green_5.Student("Danil", "Romanovich"),
                new Green_5.Student("Roma", "Egorovich"),
                new Green_5.Student("Egor", "Vasiliyevich"),
                new Green_5.Student("Masha", "Nikolayevna"),
                new Green_5.Student("Dasha", "Vadimovna")
            };
            foreach (var item in students)
            {
                for (global::System.Int32 i = 0; i < 10; i++)
                {
                    var mark = _rand.Next(0, 6);
                    item.Exam(mark > 1 ? mark : 0);
                }
            }
            var group = new Green_5.Group[10]
            {
                new Green_5.Group("CSKA"),
                new Green_5.Group("Sparta"),
                new Green_5.Group("Meteor"),
                new Green_5.Group("Gorniy"),
                new Green_5.EliteGroup("Dinamo"),
                new Green_5.EliteGroup("Bars"),
                new Green_5.EliteGroup("Unics"),
                new Green_5.SpecialGroup("Tracktor"),
                new Green_5.SpecialGroup("Bulls"),
                new Green_5.SpecialGroup("Chikago")
            };
            for (int i = 0; i < group.Length; i++)
            {
                group[i].Add(students.Take(i).ToArray());
            }
            return group;
        }
        private void Check(Green_1.Participant[] original, Green_1.Participant[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                Assert.AreEqual(original[i].Trainer, restored[i].Trainer);
                Assert.AreEqual(original[i].Result, restored[i].Result, 0.0001);
                Assert.AreEqual(original[i].HasPassed, restored[i].HasPassed);
            }
        }
        private void Check(Green_2.Human[] original, Green_2.Human[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                if (original[i] is Green_2.Student or)
                {
                    Assert.IsTrue(restored[i] is Green_2.Student);
                    var re = restored[i] as Green_2.Student;
                    Assert.AreEqual(or.Marks.Length, re.Marks.Length);
                    for (int j = 0; j < or.Marks.Length; j++)
                    {
                        Assert.AreEqual(or.Marks[j], re.Marks[j]);
                    }
                    Assert.AreEqual(or.AvgMark, re.AvgMark, 0.0001);
                    Assert.AreEqual(or.IsExcellent, re.IsExcellent);
                }
            }
        }
        private void Check(Green_3.Student[] original, Green_3.Student[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].ID, restored[i].ID);
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                Assert.AreEqual(original[i].Marks.Length, restored[i].Marks.Length);
                for (int j = 0; j < original[i].Marks.Length; j++)
                {
                    Assert.AreEqual(original[i].Marks[j], restored[i].Marks[j]);
                }
                Assert.AreEqual(original[i].AvgMark, restored[i].AvgMark, 0.0001);
            }
        }
        private void Check(Green_4.Participant[] original, Green_4.Participant[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                Assert.AreEqual(original[i].Jumps.Length, restored[i].Jumps.Length);
                for (int j = 0; j < original[i].Jumps.Length; j++)
                {
                    Assert.AreEqual(original[i].Jumps[j], restored[i].Jumps[j]);
                }
                Assert.AreEqual(original[i].BestJump, restored[i].BestJump, 0.0001);
            }
        }
        private void Check(Green_4.Discipline[] original, Green_4.Discipline[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Check(original[i].Participants, restored[i].Participants);
            }
        }
        private void Check(Green_5.Student[] original, Green_5.Student[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                Assert.AreEqual(original[i].Marks.Length, restored[i].Marks.Length);
                for (int j = 0; j < original[i].Marks.Length; j++)
                {
                    Assert.AreEqual(original[i].Marks[j], restored[i].Marks[j]);
                }
                Assert.AreEqual(original[i].AvgMark, restored[i].AvgMark, 0.0001);
            }
        }
        private void Check(Green_5.Group[] original, Green_5.Group[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].AvgMark, restored[i].AvgMark, 0.0001);
                Check(original[i].Students, restored[i].Students);
            }
        }
    }
}
