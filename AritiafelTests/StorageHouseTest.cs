using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.Json;
using Aritiafel.Locations.StorageHouse;
using System.Collections.Generic;
using CChessEngine;
using Aritiafel.Characters.Heroes;

namespace AritiafelTest
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

            public FarClass FarClass { get; set; } = new FarClass();
        }

        public class FarClass
        {
            public List<char> escpedChar { get; set; } = new List<char>();
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
            catFrat.Name = "Frat\r";
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
            dogGray.FarClass.escpedChar.Add('\"');
            dogGray.FarClass.escpedChar.Add('\"');
            dogGray.FarClass.escpedChar.Add('\u0019');
            dogGray.FarClass.escpedChar.Add('\u001C');
            dogGray.FarClass.escpedChar.Add(' ');
            Cat catAlone = new Cat();
            catAlone.Name = "Alone\"";
            catAlone.IsLazy = true;
            catAlone.Sex = "F";
            dogGray.LovedCats.Add(catAlone);

            JsonSerializerOptions jso = new JsonSerializerOptions
            { WriteIndented = true };
            DefaultJsonConverterFactory djcf = new DefaultJsonConverterFactory();
            djcf.ReferenceTypeReadAndWritePolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName;
            jso.Converters.Add(djcf);
            string s = JsonSerializer.Serialize(Tom, jso);
            //TestContext.WriteLine(s);
            object o = JsonSerializer.Deserialize<Person>(s, jso);
            string s2 = JsonSerializer.Serialize(o, jso);
            //TestContext.WriteLine(s2);
            Assert.IsTrue(s.CompareTo(s2) == 0);

            List<Person> listTest = new List<Person>();
            listTest.Add(Tom);
            listTest.Add(John);

            s = JsonSerializer.Serialize(listTest, jso);
            o = JsonSerializer.Deserialize<List<Person>>(s, jso);
            s2 = JsonSerializer.Serialize(o, jso);
            TestContext.WriteLine(s2);
            Assert.IsTrue(s.CompareTo(s2) == 0);
            Console.WriteLine("OK");
        }



        [TestMethod]
        public void SpecificJsonText()
        {
            string jsonString = "[ { \"ID\": 1, \"Name\": \"Area1\", \"InX\":" +
                " 0, \"InY\": 0, \"OutX\": 50, \"OutY\": 100, \"Length\": 100, \"Width\": 100 } ]";


            JsonSerializerOptions jso = new JsonSerializerOptions();
            DefaultJsonConverterFactory djcf = new DefaultJsonConverterFactory();
            //djcf.ReferenceTypeReadAndWritePolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName;
            jso.Converters.Add(djcf);
            object o = JsonSerializer.Deserialize<List<NSBattle.Area>>(jsonString, jso);
            TestContext.WriteLine(o.ToString());
        }

        public class Car
        {
            public int? ID { get; set; }

            public int? Password { get; set; }

            public bool? CanDrive { get; set; }
        }

        [TestMethod]
        public void NullableTest ()
        {
            Car c = new Car();
            c.ID = null;
            c.Password = 111;
            c.CanDrive = null;
            JsonSerializerOptions jso = new JsonSerializerOptions();
            DefaultJsonConverterFactory djcf = new DefaultJsonConverterFactory();
            //djcf.ReferenceTypeReadAndWritePolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName;
            jso.Converters.Add(djcf);
            string s = JsonSerializer.Serialize(c, jso);
            object o = JsonSerializer.Deserialize<Car>(s, jso);
            string s2 = JsonSerializer.Serialize(o, jso);
            Assert.IsTrue(s.CompareTo(s2) == 0);
        }

        [TestMethod]
        public void SpecificTest()
        {
            CChessBoardNode ccbn = new CChessBoardNode(CChessBoard.StartingBoard);
            Tina.SaveJsonFile(@"C:\Programs\Reports\ccbn.json", ccbn, true);
        }

    }
}

namespace NSBattle
{
    public class Area
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int InX { get; set; }
        public int InY { get; set; }
        public int OutX { get; set; }
        public int OutY { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
    }

}
