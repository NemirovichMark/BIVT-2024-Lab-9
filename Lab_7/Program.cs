using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7 {
    class Program {
        public static void Main(string[] args) {
            var p = new int[10];
            var p1 = new int[11];

            var pp = p.Concat(p1);

            foreach (var i in pp)
                System.Console.WriteLine(i);
        }
    }
}