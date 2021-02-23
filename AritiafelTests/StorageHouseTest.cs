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

            public double Weight { get; set; }
        }

        public class Dog : Person
        {
            public bool LikedToBeDog { get; set; }
            public List<Cat> LovedCats { get; set; } = new List<Cat>();
            public double Weight { get; set; }
        }

        public class Person
        {
            public string Name { get; set; }

            public decimal Score { get; set; }

            public string Sex { get; set; }

            public List<Person> Friends { get; set; } = new List<Person>();
        }

        [TestMethod]
        public void JsonText()
        {
            Cat catFrat = new Cat();
            catFrat.Name = "Frat";
            catFrat.Sex = "M";
            catFrat.Weight = -3.5;
            catFrat.IsLazy = true;
            Cat catValin = new Cat();
            catValin.Name = "Valin";
            catValin.Sex = "F";
            catValin.Weight = 3.5;
            catValin.IsLazy = false;
            Person Tom = new Person();
            Tom.Name = "Tom";
            Tom.Sex = "M";
            Tom.Score = 78616479823149761487946549898m;
            Person John = new Person();
            John.Name = "John";
            John.Sex = "M";
            John.Score = 78616479823149761487946549.588m;
            Tom.Friends.Add(John);
            Tom.Friends.Add(catFrat);
            John.Friends.Add(catValin);

            Dog dogGray = new Dog();
            catValin.Friends.Add(dogGray);
            dogGray.Weight = 3.698494854987486541878954897484564897685648979845498797668598549887989;
            dogGray.Name = "Gray";
            dogGray.Sex = "M";
            dogGray.LikedToBeDog = true;
            Cat catAlone = new Cat();
            catAlone.Name = "Alone";
            catAlone.IsLazy = true;
            catAlone.Sex = "F";
            dogGray.LovedCats.Add(catAlone);
            
            JsonSerializerOptions jso = new JsonSerializerOptions
            { WriteIndented = true };
            DefaultJsonConverterFactory djcf = new DefaultJsonConverterFactory();
            djcf.ReferenceTypeReadAndWritePolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName;
            jso.Converters.Add(djcf);
            string s = JsonSerializer.Serialize(Tom, jso);
            object o = JsonSerializer.Deserialize<Person>(s, jso);            
            string s2 = JsonSerializer.Serialize(o, jso);
            TestContext.WriteLine(s2);
            Assert.IsTrue(s.CompareTo(s2) == 0);            
        }
    }
}

