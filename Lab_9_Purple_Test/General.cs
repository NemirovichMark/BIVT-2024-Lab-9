using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9_Purple_Test
{
    internal class General
    {
        internal static void CheckOOP<T>(T obj, (string, string[], Type)[] ps, (string, string[], Type, Type[])[] ms)
        {
            var type = obj.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            Check(fields);
            Check(properties, ps);
            Check(methods, ms);

            // Список запрещенных типов из System.Reflection
            var forbiddenTypes = new HashSet<Type>
    {
        typeof(FieldInfo), typeof(PropertyInfo), typeof(MethodInfo),
        typeof(Type), typeof(Assembly), typeof(BindingFlags)
    };

            // Проверка всех используемых типов в классе
            var usedTypes = GetUsedTypes(
                type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static),
                type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static),
                type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            );

            var usesReflection = usedTypes.Any(t => forbiddenTypes.Contains(t));
            Assert.IsFalse(usesReflection, "Class should not use types from System.Reflection");
        }
        private static void Check(FieldInfo[] f)
        {
            Assert.AreEqual(f.Length, 0);
        }
        private static void Check(PropertyInfo[] p, (string, string[], Type)[] ps)
        {
            Assert.AreEqual(p.Length, ps.Length);
            if (p.Length > 0)
            {
                p = p.OrderBy(_ => _.Name).ThenBy(_ => _.PropertyType).ToArray();
                ps = ps.OrderBy(_ => _.Item1).ThenBy(_ => _.Item3).ToArray();
            }
            for (int i = 0; i < p.Length; i++)
            {
                Assert.IsTrue(p[i].CanRead);
                Assert.IsFalse(p[i].CanWrite || p[i].SetMethod != null && p[i].SetMethod.IsPublic);
                Assert.AreEqual(p[i].Name, ps[i].Item1);
                if (ps[i].Item2 != null)
                {
               //     Assert.AreEqual(p[i].GetMethod?.IsAbstract, ps[i].Item2.Contains("abstract"));
                    Assert.AreEqual(p[i].GetMethod?.IsStatic, ps[i].Item2?.Contains("static"));
                    Assert.AreEqual(p[i].GetMethod?.IsVirtual, ps[i].Item2?.Intersect(new string[] { "abstract", "virtual" }).Any());
                }
                Assert.AreEqual(p[i].PropertyType, ps[i].Item3);
            }
        }
        private static void Check(MethodInfo[] m, (string, string[], Type, Type[])[] ms)
        {
            if (m.Length > 0)
            {
                m = m.OrderBy(_ => _.Name).ThenBy(_ => _.ReturnType).ThenBy(_ => _.GetParameters().Length).ToArray();
                ms = ms.OrderBy(_ => _.Item1).ThenBy(_ => _.Item3).ThenBy(_ => _.Item4.Length).ToArray();
            }
            for (int i = 0, j = 0; i < m.Length && j < ms.Length; i++)
            {
                if (m[i].Name == "Equals" || m[i].Name.Contains("get", StringComparison.InvariantCultureIgnoreCase)) continue;
                if (m[i].IsConstructor)
                {
                    Assert.AreEqual(m[i].GetParameters().Length, 0);
                    continue;
                }
                Assert.IsTrue(m[i].IsPublic);
                Assert.AreEqual(m[i].Name, ms[j].Item1);
                if (ms[j].Item2 != null)
                {
                    Assert.AreEqual(m[i].IsAbstract, ms[j].Item2.Contains("abstract"));
                    Assert.AreEqual(m[i].IsStatic, ms[j].Item2.Contains("static"));
                    Assert.AreEqual(m[i].IsVirtual, ms[j].Item2.Contains("virtual"));
                    Assert.AreEqual(m[i].IsGenericMethod, ms[j].Item2.Contains("generic"));
                }
                if (!m[i].IsGenericMethod)
                {
                    Assert.AreEqual(m[i].ReturnType, ms[j].Item3);
                    var mp = m[i].GetParameters();
                    Assert.AreEqual(mp.Length, ms[j].Item4.Length);
                    for (int p = 0; p < mp.Length; p++)
                    {
                        Assert.IsTrue(ms[j].Item4.Any(t => t.Equals(mp[p].ParameterType)));
                    }
                }
                j++;
            }
        }
        private static IEnumerable<Type> GetUsedTypes(FieldInfo[] fields, PropertyInfo[] properties, MethodInfo[] methods)
        {
            var mTypes = new List<Type>();

            foreach (var field in fields)
            {
                mTypes.Add(field.FieldType);
            }
            foreach (var property in properties)
            {
                mTypes.Add(property.PropertyType);
            }
            foreach (var method in methods)
            {
                mTypes.Add(method.ReturnType);
                foreach (var parameter in method.GetParameters())
                {
                    mTypes.Add(parameter.ParameterType);
                }
            }
            return mTypes.Distinct();
        }
    }
}
