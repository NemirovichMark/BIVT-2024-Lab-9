using Lab_7;
using Lab_9;
using Lab_9_Blue_Test;

namespace Lab_9_Blue_Test
{
    [TestClass]
    public sealed class BlueJSON
    {
        private Random _rand = new Random();
        BlueSerializer _serializer = new BlueJSONSerializer();

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
                new ("SerializeBlue1Response", new string[] { "virtual" }, typeof(void), new Type[] {typeof(Blue_1.Response), typeof(string) }),
                new ("SerializeBlue2WaterJump", new string[] { "virtual" }, typeof(void), new Type[] { typeof(Blue_2.WaterJump), typeof(string) }),
                new ("SerializeBlue3Participant", new string[] { "virtual", "generic"  }, typeof(void), new Type[] { typeof(Blue_3.Participant), typeof(string) }),
                new ("SerializeBlue4Group", new string[] { "virtual" }, typeof(void), new Type[] { typeof(Blue_4.Group), typeof(string) }),
                new ("SerializeBlue5Team", new string[] { "virtual", "generic" }, typeof(void), new Type[] { typeof(Blue_5.Team), typeof(string) }),
                new ("DeserializeBlue1Response", new string[] { "virtual" }, typeof(Blue_1.Response), new Type[] {typeof(string) }),
                new ("DeserializeBlue2WaterJump", new string[] { "virtual" }, typeof(Blue_2.WaterJump), new Type[] {typeof(string) }),
                new ("DeserializeBlue3Participant", new string[] { "virtual", "generic" }, typeof(Blue_3.Participant), new Type[] {typeof(string) }),
                new ("DeserializeBlue4Group", new string[] { "virtual" }, typeof(Blue_4.Group), new Type[] {typeof(string) }),
                new ("DeserializeBlue5Team", new string[] {"virtual", "generic" }, typeof(Blue_5.Team), new Type[] {typeof(string) }),
            };
            General.CheckOOP(_serializer, props, methods);
        }
        [TestMethod]
        public void Test_01_Hierarchy()
        {
            Assert.IsTrue(_serializer is IFileManager);
            Assert.IsTrue(_serializer is FileSerializer);
            Assert.IsTrue(_serializer is BlueSerializer);
            Assert.IsFalse(_serializer is BlueTXTSerializer);
            Assert.IsTrue(_serializer is BlueJSONSerializer);
            Assert.IsFalse(_serializer is BlueXMLSerializer);
        }
        [TestMethod]
        public void Test_02_CreateFolder()
        {
            Assert.IsTrue(IFileManagerTest.Check_CreateFolder(_serializer, $"Blue_JSON"));
            CheckFolder();
        }
        [TestMethod]
        public void Test_03_CreateFile()
        {
            Assert.IsTrue(IFileManagerTest.Check_CreateFile(_serializer, $"Blue_JSON", $"Blue_task", "json"));
            CheckFile();
        }
        [TestMethod]
        public void Test_04_Blue1_JSON()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Blue1_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeBlue1Response(original[i], $"Blue_1_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Blue_1.Response[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeBlue1Response($"Blue_1_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_05_Blue2_JSON()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Blue2_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeBlue2WaterJump(original[i], $"Blue_2_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Blue_2.WaterJump[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeBlue2WaterJump($"Blue_2_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_06_Blue3_JSON()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Blue3_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeBlue3Participant(original[i], $"Blue_3_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Blue_3.Participant[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeBlue3Participant<Blue_3.Participant>($"Blue_3_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_07_Blue4_JSON()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Blue4_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeBlue4Group(original[i], $"Blue_4_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Blue_4.Group[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeBlue4Group($"Blue_4_{i + 1}");
            }
            Check(original, restored);
        }
        [TestMethod]
        public void Test_08_Blue5_JSON()
        {
            Init();
            Test_03_CreateFile();
            var original = Create_Blue5_Array();
            for (int i = 0; i < original.Length; i++)
            {
                _serializer.SerializeBlue5Team(original[i], $"Blue_5_{i + 1}");
            }
            Init();
            Test_03_CreateFile();
            var restored = new Blue_5.Team[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                restored[i] = _serializer.DeserializeBlue5Team<Blue_5.Team>($"Blue_5_{i + 1}");
            }
            Check(original, restored);
        }
        private void Init()
        {
            _serializer = new BlueJSONSerializer();
        }
        private void CheckFolder()
        {
            var pathes = IFileManagerTest.Check_Properties(_serializer);
            Assert.AreEqual(pathes.folder, Path.Combine(IFileManagerTest.GeneralPath, $"Blue_JSON"));
            Assert.AreEqual(pathes.file, null);
        }

        private void CheckFile()
        {
            var pathes = IFileManagerTest.Check_Properties(_serializer);
            Assert.AreEqual(pathes.folder, Path.Combine(IFileManagerTest.GeneralPath, $"Blue_JSON"));
            Assert.AreEqual(pathes.file, Path.Combine(IFileManagerTest.GeneralPath, $"Blue_JSON", $"Blue_task.json"));
        }
        private Blue_1.Response[] Create_Blue1_Array()
        {
            var participants = new Blue_1.Response[10]
            {
                new Blue_1.Response("CSKA"),
                new Blue_1.Response("Roman"),
                new Blue_1.Response("Gorniy"),
                new Blue_1.Response("Gorniy"),
                new Blue_1.Response("Sergey"),
                new Blue_1.HumanResponse("Peter", "Vadimovich"),
                new Blue_1.HumanResponse("Roman", "Petrovich"),
                new Blue_1.HumanResponse("Roman", "Nikolayevich"),
                new Blue_1.HumanResponse("Sergey", "Egorovich"),
                new Blue_1.HumanResponse("Sergey", "Egorovich")
            };
            foreach (var item in participants)
            {
                item.CountVotes(participants);
            }
            return participants;
        }
        private Blue_2.WaterJump[] Create_Blue2_Array()
        {
            var participants = new Blue_2.Participant[10]
            {
                new Blue_2.Participant("Vasya", "Petrovich"),
                new Blue_2.Participant("Petya", "Nikolayevich"),
                new Blue_2.Participant("Kolya", "Vadimovich"),
                new Blue_2.Participant("Vadim", "Maratovich"),
                new Blue_2.Participant("Marat", "Danilovich"),
                new Blue_2.Participant("Danil", "Romanovich"),
                new Blue_2.Participant("Roma", "Egorovich"),
                new Blue_2.Participant("Egor", "Vasiliyevich"),
                new Blue_2.Participant("Masha", "Nikolayevna"),
                new Blue_2.Participant("Dasha", "Vadimovna")
            };
            int[] jumps = new int[5];
            foreach (var s in participants)
            {
                for (int i = 0; i < jumps.Length; i++)
                    jumps[i] = _rand.Next(10);
                s.Jump(jumps);
                for (int i = 0; i < jumps.Length; i++)
                    jumps[i] = _rand.Next(10);
                s.Jump(jumps);
            }
            var competitions = new Blue_2.WaterJump[10]
            {
                new Blue_2.WaterJump3m("1", 100),
                new Blue_2.WaterJump3m("2", 1000),
                new Blue_2.WaterJump3m("3", 10000),
                new Blue_2.WaterJump3m("4", 100000),
                new Blue_2.WaterJump3m("5", 1000000),
                new Blue_2.WaterJump5m("6", 100),
                new Blue_2.WaterJump5m("7", 1000),
                new Blue_2.WaterJump5m("8", 10000),
                new Blue_2.WaterJump5m("9", 100000),
                new Blue_2.WaterJump5m("0", 1000000)
            };
            for (int i = 0; i < competitions.Length; i++)
            {
                competitions[i].Add(participants.Take(i).ToArray());
            }
            return competitions;
        }
        private Blue_3.Participant[] Create_Blue3_Array()
        {
            var participants = new Blue_3.Participant[10]
            {
                new Blue_3.Participant("Vasya", "Petrovich"),
                new Blue_3.Participant("Petya", "Nikolayevich"),
                new Blue_3.Participant("Kolya", "Vadimovich"),
                new Blue_3.Participant("Vadim", "Maratovich"),
                new Blue_3.BasketballPlayer("Roma", "Egorovich"),
                new Blue_3.BasketballPlayer("Egor", "Vasiliyevich"),
                new Blue_3.BasketballPlayer("Masha", "Nikolayevna"),
                new Blue_3.HockeyPlayer("Marat", "Danilovich"),
                new Blue_3.HockeyPlayer("Danil", "Romanovich"),
                new Blue_3.HockeyPlayer("Dasha", "Vadimovna")
            };
            foreach (var item in participants)
            {
                for (global::System.Int32 i = 0; i < 10; i++)
                {
                    item.PlayMatch(_rand.Next(0, 10));
                }
            }
            return participants;
        }
        private Blue_4.Group[] Create_Blue4_Array()
        {
            var teams = new Blue_4.Team[10]
            {
                new Blue_4.ManTeam("CSKA"),
                new Blue_4.ManTeam("Dinamo"),
                new Blue_4.ManTeam("Metalurg"),
                new Blue_4.ManTeam("Gorniy"),
                new Blue_4.ManTeam("Bulls"),
                new Blue_4.WomanTeam("Bars"),
                new Blue_4.WomanTeam("Roma"),
                new Blue_4.WomanTeam("Zenit"),
                new Blue_4.WomanTeam("Unics"),
                new Blue_4.WomanTeam("Torpedo")
            };
            foreach (var item in teams)
            {
                for (global::System.Int32 i = 0; i < 10; i++)
                {
                    item.PlayMatch(_rand.Next(0, 10));
                }
            }
            var groups = new Blue_4.Group[5]
            {
                new Blue_4.Group("West"),
                new Blue_4.Group("East"),
                new Blue_4.Group("Europe"),
                new Blue_4.Group("Asia"),
                new Blue_4.Group("Global")
            };
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i].Add(teams.Take(2 * i).ToArray());
            }
            return groups;
        }
        private Blue_5.Team[] Create_Blue5_Array()
        {
            var sportsmen = new Blue_5.Sportsman[10]
            {
                new Blue_5.Sportsman("Vasya", "Petrovich"),
                new Blue_5.Sportsman("Petya", "Nikolayevich"),
                new Blue_5.Sportsman("Kolya", "Vadimovich"),
                new Blue_5.Sportsman("Vadim", "Maratovich"),
                new Blue_5.Sportsman("Marat", "Danilovich"),
                new Blue_5.Sportsman("Danil", "Romanovich"),
                new Blue_5.Sportsman("Roma", "Egorovich"),
                new Blue_5.Sportsman("Egor", "Vasiliyevich"),
                new Blue_5.Sportsman("Masha", "Nikolayevna"),
                new Blue_5.Sportsman("Dasha", "Vadimovna")
            };
            foreach (var item in sportsmen)
            {
                item.SetPlace(_rand.Next(0, 10));
            }
            var group = new Blue_5.Team[10]
            {
                new Blue_5.ManTeam("CSKA"),
                new Blue_5.ManTeam("Sparta"),
                new Blue_5.ManTeam("Meteor"),
                new Blue_5.ManTeam("Gorniy"),
                new Blue_5.ManTeam("Dinamo"),
                new Blue_5.WomanTeam("Bars"),
                new Blue_5.WomanTeam("Unics"),
                new Blue_5.WomanTeam("Tracktor"),
                new Blue_5.WomanTeam("Bulls"),
                new Blue_5.WomanTeam("Chikago")
            };
            for (int i = 0; i < group.Length; i++)
            {
                group[i].Add(sportsmen.Take(i).ToArray());
            }
            return group;
        }
        private void Check(Blue_1.Response[] original, Blue_1.Response[] restored)
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
                Assert.AreEqual(original[i].Votes, restored[i].Votes);
                if (original[i] is Blue_1.HumanResponse or)
                {
                    Assert.IsTrue(restored[i] is Blue_1.HumanResponse);
                    var re = restored[i] as Blue_1.HumanResponse;
                    Assert.AreEqual(or.Surname, re.Surname);
                }
            }
        }
        private void Check(Blue_2.Participant[] original, Blue_2.Participant[] restored)
        {
            Assert.AreEqual(original.Length, restored.Length);
            for (int i = 0; i < original.Length; i++)
            {
                Assert.AreEqual(original[i].Name, restored[i].Name);
                Assert.AreEqual(original[i].Surname, restored[i].Surname);
                Assert.AreEqual(original[i].Marks.Length, restored[i].Marks.Length);
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
        private void Check(Blue_2.WaterJump[] original, Blue_2.WaterJump[] restored)
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
                Assert.AreEqual(original[i].Bank, restored[i].Bank);
                Check(original[i].Participants, restored[i].Participants);
                if (original[i].Prize == null)
                {
                    Assert.IsNull(restored[i].Prize);
                    continue;
                }
                Assert.AreEqual(original[i].Prize.Length, restored[i].Prize.Length);
                for (int j = 0; j < original[i].Prize.Length; j++)
                {
                    Assert.AreEqual(original[i].Prize[j], restored[i].Prize[j], 0.0001);
                }
            }
        }
        private void Check(Blue_3.Participant[] original, Blue_3.Participant[] restored)
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
                Assert.AreEqual(original[i].Penalties.Length, restored[i].Penalties.Length);
                for (int j = 0; j < original[i].Penalties.Length; j++)
                {
                    Assert.AreEqual(original[i].Penalties[j], restored[i].Penalties[j]);
                }
                Assert.AreEqual(original[i].Total, restored[i].Total);
                Assert.AreEqual(original[i].IsExpelled, restored[i].IsExpelled);
            }
        }
        private void Check(Blue_4.Team[] original, Blue_4.Team[] restored)
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
                Assert.AreEqual(original[i].Scores.Length, restored[i].Scores.Length);
                for (int j = 0; j < original[i].Scores.Length; j++)
                {
                    Assert.AreEqual(original[i].Scores[j], restored[i].Scores[j]);
                }
                Assert.AreEqual(original[i].TotalScore, restored[i].TotalScore);
            }
        }
        private void Check(Blue_4.Group[] original, Blue_4.Group[] restored)
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
                Check(original[i].ManTeams, restored[i].ManTeams);
                Check(original[i].WomanTeams, restored[i].WomanTeams);
            }
        }
        private void Check(Blue_5.Sportsman[] original, Blue_5.Sportsman[] restored)
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
                Assert.AreEqual(original[i].Place, restored[i].Place);
            }
        }
        private void Check(Blue_5.Team[] original, Blue_5.Team[] restored)
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
                Assert.AreEqual(original[i].TopPlace, restored[i].TopPlace);
                Assert.AreEqual(original[i].SummaryScore, restored[i].SummaryScore);
                Check(original[i].Sportsmen, restored[i].Sportsmen);
            }
        }
    }
}