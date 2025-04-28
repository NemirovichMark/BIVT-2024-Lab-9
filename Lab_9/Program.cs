using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Lab_7;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace Lab_9 {

    public class Dog {
        public string Name {get; }
        public int Age {get; }
        public Dog(string Name, int Age) {
            this.Name = Name;
            this.Age = Age;
        }
    }

    public class DogDTO {
        public string Name {get; set; }
        public int Age {get; set; }
    }
    public class Animal {
        public string Enemy {get; }
        public int Rank {get; }
        public Dog[] Dogs {get; }
        public int[,] tmp {get; }

        public Animal(string Enemy, int Rank, Dog[] Dogs) {
            this.Enemy = Enemy;
            this.Rank = Rank;
            this.Dogs = Dogs;
            tmp = new int[,] {{1,2}, {3, 4}};
        }
    }

    public class AnimalDTO {
        public string Enemy {get; set; }
        public int Rank {get; set; }
        public DogDTO[] Dogs {get; set; }
        public int[][] tmp {get; set; }
    }


    public class Program {

        public static void Main(string[] args) {

            var animal = new Animal("nobody", 100, [new Dog("archie", 1), new Dog("lol", 12)]);

            var daDogs = animal.Dogs.Select(d => new DogDTO {Name = d.Name, Age = d.Age}).ToArray();

            int n = animal.tmp.GetLength(0);
            int m = animal.tmp.GetLength(1);

            var datmp = new int[n][];
            for (int i = 0; i < n; i++) {
                datmp[i] = new int[m];
                for (int j = 0; j < m; j++) datmp[i][j] = animal.tmp[i, j];
            }

            var animalDTO = new AnimalDTO {Enemy = animal.Enemy, Rank = animal.Rank, Dogs = daDogs, tmp = datmp};

            var serialized = new XmlSerializer(typeof(AnimalDTO));

            using (var fs = File.Create("animal.xml")) {
                serialized.Serialize(fs, animalDTO);
            }
            
            var root = XDocument.Parse(File.ReadAllText("animal.xml")).Root;
            
            var tmp = root.Element("tmp").Elements().Select(x => x.Elements().Select(x => (int)x).ToArray()).ToArray();
            var nBack = tmp.Length;
            var mBack = tmp[0].Length;

            var tmpDaBack = new int[n, m];
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++)
                    tmpDaBack[i, j] = tmp[i][j];
            }


            var daDogss = root.Element("Dogs").Elements().Select(x => new Dog(x.Element("Name").Value, (int)x.Element("Age"))).ToArray();

            using (var fs = new StreamReader("animal.xml")) {
                var x = (AnimalDTO)serialized.Deserialize(fs);
                
                foreach (var dog in x.Dogs)
                    Console.WriteLine(dog.GetType());
            }
        }
    }
}