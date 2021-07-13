using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Locations;
using Aritiafel.Characters.Heroes;

namespace AritiafelTest
{
    [TestClass]
    public class TinaTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SaveProject()
        {
            Tina.SaveProject();
        }

        [TestMethod]
        public void TextFileSaveLoadDelete()
        {
            Tina.SaveTextFile(@"C:\Programs\Reports\test.txt", "abc");
            Tina.SaveTextFile(@"C:\Programs\Reports\", "test2.txt", "def");
            TestContext.WriteLine(Tina.ReadTextFile(@"C:\Programs\Reports\test.txt"));
            TestContext.WriteLine(Tina.ReadTextFile(@"C:\Programs\Reports", "test2.txt"));
            Tina.DeleteFile(@"C:\Programs\Reports\test.txt");
            Tina.DeleteFile(@"C:\Programs\Reports\test2.txt");
        }

        [TestMethod]
        public void JsonFileSaveLoad()
        {
            Tina.SaveJsonFile(@"C:\Programs\Reports\test.json", new { a = 3, b = new[] { 4, 5 } });
            object o = Tina.LoadJsonFile(@"C:\Programs\Reports\test.json");
            TestContext.WriteLine(o.ToString());

            Person p = new Person();
            p.Name = "Tom";
            p.Money = 3000;
            p.Gender = "Male";

            Tina.SaveJsonFile(@"C:\Programs\Reports\person.json", p);
            Person p2 = Tina.LoadJsonFile<Person>(@"C:\Programs\Reports\person.json");
            TestContext.WriteLine(p2.Money.ToString());
        }

        public class Person
        {
            public string Name { get; set; }
            public string Gender { get; set; }

            public int Money { get; set; }
        }
    }
}
