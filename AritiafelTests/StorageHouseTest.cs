using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.Json;
using Aritiafel.Locations.StorageHouse;
using System.Collections.Generic;

namespace AritiafelTestFormTests
{
    [TestClass]
    public class StorageHouseTest
    {
        public TestContext TestContext { get; set; }
        public class Cat : Person
        {
            public bool IsLazy { get; set; }
        }

        public class Person
        {
            public string Name { get; set; }

            public string Sex { get; set; }

            public List<Person> Friends { get; set; } = new List<Person>();
        }

        [TestMethod]
        public void JsonText()
        {
            Cat catFrat = new Cat();
            catFrat.Name = "Frat";
            catFrat.Sex = "M";
            catFrat.IsLazy = true;
            Cat catValin = new Cat();
            catValin.Name = "Valin";
            catValin.Sex = "F";
            catValin.IsLazy = false;
            Person Tom = new Person();
            Tom.Name = "Tom";
            Tom.Sex = "M";            
            Person John = new Person();
            John.Name = "John";
            John.Sex = "M";
            Tom.Friends.Add(John);
            Tom.Friends.Add(catFrat);
            John.Friends.Add(catValin);
            
            JsonSerializerOptions jso = new JsonSerializerOptions
            { WriteIndented = true };
            jso.Converters.Add(new DefalutJsonConverter());
            string s = JsonSerializer.Serialize(Tom, jso);
            TestContext.WriteLine(s);
            object o = JsonSerializer.Deserialize<Person>(s, jso);
            TestContext.WriteLine(o.ToString());
        }
    }
}
