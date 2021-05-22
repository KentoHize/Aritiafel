using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using System.Xml;
using System.Text.Json;
using Aritiafel.Locations.StorageHouse;
using Aritiafel.Artifacts;
using System.Linq;

namespace Aritiafel.Organizations
{
    /// <summary>
    /// 法師公會，目前提供繁轉簡翻譯服務和中文姓名服務
    /// </summary>
    public static class WizardGuild
    {
        public static string TranslateTextFromTraditionalChineseToSimplifiedChinese(string text)
        {
            return Strings.StrConv(text, VbStrConv.SimplifiedChinese, 2052);
        }

        public static void ProduceSimplifiedChineseResourceFile(string traditionalChineseFileName)
        {
            XmlDocument xmlD = new XmlDocument();

            if (!traditionalChineseFileName.Contains(".resx"))
                throw new ArgumentException();

            xmlD.Load(traditionalChineseFileName);
            foreach (XmlNode xd in xmlD.ChildNodes[1].ChildNodes)
                if (xd.Name == "data")
                    foreach (XmlNode xd2 in xd.ChildNodes)
                        if (xd2.Name == "value")
                            xd.InnerText = TranslateTextFromTraditionalChineseToSimplifiedChinese(xd.InnerText);

            string outputFileName = $"{Path.GetDirectoryName(traditionalChineseFileName)}\\{Path.GetFileName(traditionalChineseFileName).Split('.')[0]}.zh-CN.resx";
            using (TextWriter sw = new StreamWriter(outputFileName, false, Encoding.UTF8)) {
                xmlD.Save(sw);
            }
        }

        private static List<ChineseNameWord> NameWords;
        private static List<ChineseSurname> Surnames;

        private static void ReadChineseNameData()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions
            { WriteIndented = true };
            DefaultJsonConverterFactory djcf = new DefaultJsonConverterFactory();
            djcf.ReferenceTypeReadAndWritePolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName;
            jso.Converters.Add(djcf);

            string file = @"C:\Programs\Standard\Aritiafel\Aritiafel\Data\NameOfChinese.json";
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string s = sr.ReadToEnd();
                    NameWords = JsonSerializer.Deserialize<List<ChineseNameWord>>(s, jso);
                }
            }

            file = @"C:\Programs\Standard\Aritiafel\Aritiafel\Data\SurnameOfChinese.json";
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string s = sr.ReadToEnd();
                    Surnames = JsonSerializer.Deserialize<List<ChineseSurname>>(s, jso);
                }
            }
        }

        public static string RandomChineseFemaleName(string surname = "")
        {
            ReadChineseNameData();
            ChaosBox cb = new ChaosBox();

            if (string.IsNullOrEmpty(surname))
                surname = Surnames[cb.DrawOutInteger(0, 99)].Surname;
            string name = "";

            var query = (from n in NameWords
                        where n.Gender == "f" || n.Gender == "n"
                        select n).ToList();
            name += query[cb.DrawOutInteger(0, query.Count - 1)].Word;
            query =     (from n in NameWords
                         where n.Gender == "f"
                         select n).ToList();
            name += query[cb.DrawOutInteger(0, query.Count - 1)].Word;
            return string.Concat(surname, name);
        }

        public static string RandomChineseMaleName(string surname = "")
        {
            ReadChineseNameData();
            ChaosBox cb = new ChaosBox();

            if (string.IsNullOrEmpty(surname))
                surname = Surnames[cb.DrawOutInteger(0, 99)].Surname;
            string name = "";

            var query = (from n in NameWords
                         where n.Gender == "m" || n.Gender == "n"
                         select n).ToList();
            name += query[cb.DrawOutInteger(0, query.Count - 1)].Word;
            query = (from n in NameWords
                     where n.Gender == "m" || n.Gender == "n"
                     select n).ToList();
            name += query[cb.DrawOutInteger(0, query.Count - 1)].Word;
            return string.Concat(surname, name);
        }

        public static string RandomChineseNeutralName(string surname = "")
        {
            ReadChineseNameData();
            ChaosBox cb = new ChaosBox();

            if (string.IsNullOrEmpty(surname))
                surname = Surnames[cb.DrawOutInteger(0, 99)].Surname;
            string name = "";

            var query = (from n in NameWords
                         where n.Gender == "n"
                         select n).ToList();
            name += query[cb.DrawOutInteger(0, query.Count - 1)].Word;
            name += query[cb.DrawOutInteger(0, query.Count - 1)].Word;
            return string.Concat(surname, name);
        }

        private class ChineseNameWord
        {
            public string Word { get; set; }
            public string Gender { get; set; }
        }

        private class ChineseSurname
        {
            public string Surname { get; set; }
        }
    }
}
