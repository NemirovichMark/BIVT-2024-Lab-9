using Lab_7;
using Lab_9;
using Lab_9_Purple_Test;
using System.Security.Cryptography.X509Certificates;

namespace Lab_9_Purple_Test
{
    [TestClass]
    public sealed class PurpleJSON
    {
        private Random _rand = new Random();
        PurpleSerializer _serializer = new PurpleJSONSerializer();

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
                new ("SerializePurple1", new string[] { "virtual", "generic" }, typeof(void), new Type[] {typeof(Purple_1.Competition), typeof(string) }),
                new ("SerializePurple2SkiJumping", new string[] { "virtual", "generic" }, typeof(void), new Type[] { typeof(Purple_2.SkiJumping), typeof(string) }),
                new ("SerializePurple3Skating", new string[] { "virtual", "generic"  }, typeof(void), new Type[] { typeof(Purple_3.Skating), typeof(string) }),
                new ("SerializePurple4Group", new string[] { "virtual" }, typeof(void), new Type[] { typeof(Purple_4.Group), typeof(string) }),
                new ("SerializePurple5Report", new string[] { "virtual" }, typeof(void), new Type[] { typeof(Purple_5.Report), typeof(string) }),
                new ("DeserializePurple1", new string[] { "virtual", "generic" }, typeof(Purple_1.Competition), new Type[] {typeof(string) }),
                new ("DeserializePurple2SkiJumping", new string[] { "virtual", "generic" }, typeof(Purple_2.SkiJumping), new Type[] {typeof(string) }),
                new ("DeserializePurple3Skating", new string[] { "virtual", "generic" }, typeof(Purple_3.Skating), new Type[] {typeof(string) }),
                new ("DeserializePurple4Group", new string[] { "virtual" }, typeof(Purple_4.Group), new Type[] {typeof(string) }),
                new ("DeserializePurple5Report", new string[] {"virtual" }, typeof(Purple_5.Report), new Type[] {typeof(string) }),
            };
            General.CheckOOP(_serializer, props, methods);
        }
        [TestMethod]
        public void Test_01_Hierarchy()
        {
            Assert.IsTrue(_serializer is IFileManager);
            Assert.IsTrue(_serializer is FileSerializer);
            Assert.IsTrue(_serializer is PurpleSerializer);
            Assert.IsFalse(_serializer is PurpleTXTSerializer);
            Assert.IsTrue(_serializer is PurpleJSONSerializer);
            Assert.IsFalse(_serializer is PurpleXMLSerializer);
        }
        [TestMethod]
        public void Test_02_CreateFolder()
        {
            Assert.IsTrue(IFileManagerTest.Check_CreateFolder(_serializer, $"Purple_JSON"));
            CheckFolder();
        }
        [TestMethod]
        public void Test_03_CreateFile()
        {
            Assert.IsTrue(IFileManagerTest.Check_CreateFile(_serializer, $"Purple_JSON", $"Purple_task", "json"));
            CheckFile();
        }
        [TestMethod]
        public void Test_04_Purple1_JSON_1()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Purple1_Array_1();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializePurple1(original[i], $"Purple_1_{i + 1}_1");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Purple_1.Participant[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializePurple1<Purple_1.Participant>($"Purple_1_{i + 1}_1");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_04_Purple1_JSON_2()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Purple1_Array_2();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializePurple1(original[i], $"Purple_1_{i + 1}_2");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Purple_1.Judge[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializePurple1<Purple_1.Judge>($"Purple_1_{i + 1}_2");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_04_Purple1_JSON_3()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Purple1_Array_3();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializePurple1(original[i], $"Purple_1_{i + 1}_3");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Purple_1.Competition[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializePurple1<Purple_1.Competition>($"Purple_1_{i + 1}_3");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_05_Purple2_JSON()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Purple2_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializePurple2SkiJumping(original[i], $"Purple_2_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Purple_2.SkiJumping[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializePurple2SkiJumping<Purple_2.SkiJumping>($"Purple_2_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_06_Purple3_JSON()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Purple3_Array();
            foreach (var item in original)
                Purple_3.Participant.SetPlaces(item.Participants);

            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializePurple3Skating(original[i], $"Purple_3_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Purple_3.Skating[original.Length];
            for (int i = 0; i < restored.Length; i++)
            {
                restored[i] = _serializer.DeserializePurple3Skating<Purple_3.Skating>($"Purple_3_{i + 1}");
            }
            foreach (var item in restored)
                Purple_3.Participant.SetPlaces(item.Participants);
            Check(original, restored);
        }
        [TestMethod]
        public void Test_07_Purple4_JSON()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Purple4_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializePurple4Group(original[i], $"Purple_4_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Purple_4.Group[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializePurple4Group($"Purple_4_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_08_Purple5_JSON()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Purple5_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializePurple5Report(original[i], $"Purple_5_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Purple_5.Report[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializePurple5Report($"Purple_5_{i + 1}");
            }
            Check(original, restored);
        }
        private void Init()
        {
            _serializer = new PurpleJSONSerializer();
        }
        private void CheckFolder()
        {
            var pathes = IFileManagerTest.Check_Properties(_serializer);
            Assert.AreEqual(pathes.folder, Path.Combine(IFileManagerTest.GeneralPath, $"Purple_JSON"));
            Assert.AreEqual(pathes.file, null);
        }

        private void CheckFile()
        {
            var pathes = IFileManagerTest.Check_Properties(_serializer);
            Assert.AreEqual(pathes.folder, Path.Combine(IFileManagerTest.GeneralPath, $"Purple_JSON"));
            Assert.AreEqual(pathes.file, Path.Combine(IFileManagerTest.GeneralPath, $"Purple_JSON", $"Purple_task.json"));
        }
        private Purple_1.Participant[] Create_Purple1_Array_1()
        {
            var participants = new Purple_1.Participant[10]
            {
                new Purple_1.Participant("Sergey", "Sergeevich"),
                new Purple_1.Participant("Sergey", "Ivanovich"),
                new Purple_1.Participant("Roman", "Ivanovich"),
                new Purple_1.Participant("Ivan", "Nikolayevich"),
                new Purple_1.Participant("Sergey", "Vadimovich"),
                new Purple_1.Participant("Peter", "Vadimovich"),
                new Purple_1.Participant("Roman", "Petrovich"),
                new Purple_1.Participant("Roman", "Nikolayevich"),
                new Purple_1.Participant("Sergey", "Egorovich"),
                new Purple_1.Participant("Sergey", "Nikolayevich")
            };
            double[] coefs = new double[4];
            int[] jumps = new int[7];
            foreach (var item in participants)
            {
                for (int i = 0; i < coefs.Length; i++)
                    coefs[i] = Math.Round(_rand.NextDouble() + 2.5, 2);
                item.SetCriterias(coefs);
                for (int c = 0; c < 4; c++)
                {
                    for (int i = 0; i < jumps.Length; i++)
                        jumps[i] = _rand.Next(10);
                    item.Jump(jumps);
                }
            }
            return participants;
        }
        private Purple_1.Judge[] Create_Purple1_Array_2()
        {
            var judges = new Purple_1.Judge[10]
            {
                new Purple_1.Judge("CSKA", new int[] {_rand.Next(10),_rand.Next(10),_rand.Next(10),_rand.Next(10),_rand.Next(10)}),
                new Purple_1.Judge("Roman", new int[] {_rand.Next(10),_rand.Next(10),_rand.Next(10),_rand.Next(10),_rand.Next(10)}),
                new Purple_1.Judge("Gorniy", new int[] { _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10) }),
                new Purple_1.Judge("Gorniy", new int[] { _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10) }),
                new Purple_1.Judge("Sergey", new int[] { _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10) }),
                new Purple_1.Judge("Peter", new int[] { _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10) }),
                new Purple_1.Judge("Roman", new int[] { _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10) }),
                new Purple_1.Judge("Roman", new int[] { _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10) }),
                new Purple_1.Judge("Sergey", new int[] { _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10) }),
                new Purple_1.Judge("Sergey", new int[] { _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10), _rand.Next(10) })
            };
            return judges;
        }
        private Purple_1.Competition[] Create_Purple1_Array_3()
        {
            var participants = Create_Purple1_Array_1();
            var judges = Create_Purple1_Array_2();
            var competitions = new Purple_1.Competition[10]
            {
                new Purple_1.Competition(judges),
                new Purple_1.Competition(judges),
                new Purple_1.Competition(judges),
                new Purple_1.Competition(judges),
                new Purple_1.Competition(judges),
                new Purple_1.Competition(judges),
                new Purple_1.Competition(judges),
                new Purple_1.Competition(judges),
                new Purple_1.Competition(judges),
                new Purple_1.Competition(judges)
            };
            for (int i = 0; i < competitions.Length; i++)
            {
                competitions[i].Add(participants.Take(i).ToArray());
            }
            return competitions;
        }
        private Purple_2.SkiJumping[] Create_Purple2_Array()
        {
            var competitions = new Purple_2.SkiJumping[10]
            {
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.JuniorSkiJumping(),
                new Purple_2.ProSkiJumping(),
                new Purple_2.ProSkiJumping(),
                new Purple_2.ProSkiJumping(),
                new Purple_2.ProSkiJumping(),
                new Purple_2.ProSkiJumping()
            };
            for (int i = 0; i < competitions.Length; i++)
            {
                var participants = new Purple_2.Participant[10]
                {
                new Purple_2.Participant("Vasya", "Petrovich"),
                new Purple_2.Participant("Petya", "Nikolayevich"),
                new Purple_2.Participant("Kolya", "Vadimovich"),
                new Purple_2.Participant("Vadim", "Maratovich"),
                new Purple_2.Participant("Marat", "Danilovich"),
                new Purple_2.Participant("Danil", "Romanovich"),
                new Purple_2.Participant("Roma", "Egorovich"),
                new Purple_2.Participant("Egor", "Vasiliyevich"),
                new Purple_2.Participant("Masha", "Nikolayevna"),
                new Purple_2.Participant("Dasha", "Vadimovna")
                };
                var p = participants.Take(i).ToArray();
                int[] jumps = new int[5];
                foreach (var item in p)
                {
                    for (int j = 0; j < jumps.Length; j++)
                        jumps[j] = _rand.Next(20);
                    competitions[i].Add(item);
                    competitions[i].Jump(_rand.Next(50, 200), jumps);
                }
            }
            return competitions;
        }
        private Purple_3.Skating[] Create_Purple3_Array()
        {
            var competitions = new Purple_3.Skating[10]
            {
                new Purple_3.IceSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()}),
                new Purple_3.IceSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()}),
                new Purple_3.IceSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()}),
                new Purple_3.IceSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()}),
                new Purple_3.IceSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()}),
                new Purple_3.FigureSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()}),
                new Purple_3.FigureSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()}),
                new Purple_3.FigureSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()}),
                new Purple_3.FigureSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()}),
                new Purple_3.FigureSkating(new double[7] {_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),
                    _rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble(),_rand.NextDouble()})
            };
            for (int i = 0; i < competitions.Length; i++)
            {
                for (global::System.Int32 j = 0; j < i; j++)
                {
                    var participants = new Purple_3.Participant[10]
                    {
                new Purple_3.Participant("Vasya", "Petrovich"),
                new Purple_3.Participant("Petya", "Nikolayevich"),
                new Purple_3.Participant("Kolya", "Vadimovich"),
                new Purple_3.Participant("Vadim", "Maratovich"),
                new Purple_3.Participant("Roma", "Egorovich"),
                new Purple_3.Participant("Egor", "Vasiliyevich"),
                new Purple_3.Participant("Masha", "Nikolayevna"),
                new Purple_3.Participant("Marat", "Danilovich"),
                new Purple_3.Participant("Danil", "Romanovich"),
                new Purple_3.Participant("Dasha", "Vadimovna")
                    };
                    var p = participants.Take(j + 1).ToArray();
                    for (int e = 0; e < 7; e++)
                        p[j].Evaluate(_rand.NextDouble() * 10 + 2.5);
                    competitions[i].Add(p[j]);
                }
            }
            return competitions;
        }
        private Purple_4.Group[] Create_Purple4_Array()
        {
            var participants = new Purple_4.Sportsman[10]
            {
                new Purple_4.Sportsman("Vasya", "Petrovich"),
                new Purple_4.Sportsman("Petya", "Nikolayevich"),
                new Purple_4.Sportsman("Kolya", "Vadimovich"),
                new Purple_4.Sportsman("Vadim", "Maratovich"),
                new Purple_4.Sportsman("Roma", "Egorovich"),
                new Purple_4.Sportsman("Egor", "Vasiliyevich"),
                new Purple_4.Sportsman("Masha", "Nikolayevna"),
                new Purple_4.Sportsman("Marat", "Danilovich"),
                new Purple_4.Sportsman("Danil", "Romanovich"),
                new Purple_4.Sportsman("Dasha", "Vadimovna")
            };
            foreach (var item in participants)
            {
                for (global::System.Int32 i = 0; i < 10; i++)
                {
                    item.Run(_rand.NextDouble() * 50 + 10);
                }
            }
            var groups = new Purple_4.Group[5]
            {
                new Purple_4.Group("West"),
                new Purple_4.Group("East"),
                new Purple_4.Group("Europe"),
                new Purple_4.Group("Asia"),
                new Purple_4.Group("Global")
            };
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i].Add(participants.Take(2 * i).ToArray());
            }
            return groups;
        }
        private Purple_5.Report[] Create_Purple5_Array()
        {
            string[] _animals = new string[]
            {
            "Коала", "Панда", "Макака", "Тануки", "Серау", "Кошка", "Сима энага", null
            };
            string[] _characterTrait = new string[]
            {
            "Амбициозность", "Внимательность", "Дружелюбность", "Скромность", "Проницательность", "Целеустремленность", "Уважительность", null
            };
            string[] _concept = new string[]
            {
            "Сакура", "Кимоно", "Суши", "Аниме", "Манга", "Фудзияма", "Самурай", null
            };
            var reports = new Purple_5.Report[10]
                {
                new Purple_5.Report(),
                new Purple_5.Report(),
                new Purple_5.Report(),
                new Purple_5.Report(),
                new Purple_5.Report(),
                new Purple_5.Report(),
                new Purple_5.Report(),
                new Purple_5.Report(),
                new Purple_5.Report(),
                new Purple_5.Report()
                };
            foreach (var rep in reports)
            {
                rep.MakeResearch();
                rep.MakeResearch();
                rep.MakeResearch();
                for (int i = 0; i < rep.Researches.Length; i++)
                {
                    rep.Researches[i].Add(new string[] {
                        _animals[_rand.Next(0, _animals.Length)],
                        _characterTrait[_rand.Next(0, _characterTrait.Length)],
                        _concept[_rand.Next(0, _concept.Length)] });
                }
            }
            return reports;
        }
        private void Check(Purple_1.Participant[] original, Purple_1.Participant[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].GetType(), restored[i].GetType());
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                Assert.AreEqual(original[i].Coefs.Length, restored[i].Coefs.Length);
                for (int j = 0; j < original[i].Coefs.Length; j++)
                {
                    Assert.AreEqual(original[i].Coefs[j], restored[i].Coefs[j], 0.0001);
                }
                for (int j = 0; j < original[i].Marks.GetLength(0); j++)
                {
                    for (int k = 0; k < original[i].Marks.GetLength(1); k++)
                    {
                        Assert.AreEqual(original[i].Marks[j, k], restored[i].Marks[j, k]);
                    }
                }
                Assert.AreEqual(original[i].TotalScore, restored[i].TotalScore);
            }
        }
        private void Check(Purple_1.Judge[] original, Purple_1.Judge[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].GetType(), restored[i].GetType());
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Marks.Length, restored[i].Marks.Length);
                for (int j = 0; j < original[i].Marks.Length; j++)
                {
                    Assert.AreEqual(original[i].Marks[j], restored[i].Marks[j], 0.0001);
                }
            }
        }
        private void Check(Purple_1.Competition[] original, Purple_1.Competition[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].GetType(), restored[i].GetType());
                Check(original[i].Participants, restored[i].Participants);
                Check(original[i].Judges, restored[i].Judges);
            }
        }
        private void Check(Purple_2.Participant[] original, Purple_2.Participant[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                Assert.AreEqual(original[i].Distance, restored[i].Distance);
                Assert.AreEqual(original[i].Result, restored[i].Result);
                Assert.AreEqual(original[i].Marks.Length, restored[i].Marks.Length);
                for (int j = 0; j < original[i].Marks.Length; j++)
                {
                    Assert.AreEqual(original[i].Marks[j], restored[i].Marks[j]);
                }
            }
        }
        private void Check(Purple_2.SkiJumping[] original, Purple_2.SkiJumping[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].GetType(), restored[i].GetType());
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Standard, restored[i].Standard);
                Check(original[i].Participants, restored[i].Participants);
            }
        }
        private void Check(Purple_3.Participant[] original, Purple_3.Participant[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                Assert.AreEqual(original[i].Score, restored[i].Score);
                Assert.AreEqual(original[i].Marks.Length, restored[i].Marks.Length);
                for (int j = 0; j < original[i].Marks.Length; j++)
                {
                    Assert.AreEqual(original[i].Marks[j], restored[i].Marks[j]);
                }
                Assert.AreEqual(original[i].Places.Length, restored[i].Places.Length);
                for (int j = 0; j < original[i].Places.Length; j++)
                {
                    Assert.AreEqual(original[i].Places[j], restored[i].Places[j]);
                }
            }
        }
        private void Check(Purple_3.Skating[] original, Purple_3.Skating[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].GetType(), restored[i].GetType());
                Assert.AreEqual(original[i].Moods.Length, restored[i].Moods.Length);
                for (int j = 0; j < original[i].Moods.Length; j++)
                {
                    Assert.AreEqual(original[i].Moods[j], restored[i].Moods[j], 0.0001);
                }
                Check(original[i].Participants, restored[i].Participants);
            }
        }
        private void Check(Purple_4.Sportsman[] original, Purple_4.Sportsman[] restored)
        {
            if (original == null)
            {
                Assert.IsNull(restored);
                return;
            }
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].GetType(), restored[i].GetType());
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                Assert.AreEqual(original[i].Time, restored[i].Time);
            }
        }
        private void Check(Purple_4.Group[] original, Purple_4.Group[] restored)
        {
            if (original == null)
            {
                Assert.IsNull(restored);
                return;
            }
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Assert.AreEqual(original[i].GetType(), restored[i].GetType());
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Check(original[i].Sportsmen, restored[i].Sportsmen);
            }
        }
        private void Check(Purple_5.Response[] original, Purple_5.Response[] restored)
        {
            if (original == null)
            {
                Assert.IsNull(restored);
                return;
            }
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                Assert.AreEqual(original[i].Animal, restored[i].Animal);
                Assert.AreEqual(original[i].CharacterTrait, restored[i].CharacterTrait);
                Assert.AreEqual(original[i].Concept, restored[i].Concept);
            }
        }
        private void Check(Purple_5.Research[] original, Purple_5.Research[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Check(original[i].Responses, restored[i].Responses);
            }
        }
        private void Check(Purple_5.Report[] original, Purple_5.Report[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == null)
                {
                    Assert.IsNull(restored[i]);
                    continue;
                }
                Check(original[i].Researches, restored[i].Researches);
            }
        }
    }
}
