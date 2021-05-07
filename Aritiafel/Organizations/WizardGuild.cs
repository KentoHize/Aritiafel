using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using System.Xml;

namespace Aritiafel.Organizations
{
    /// <summary>
    /// 法師公會，目前提供繁轉簡翻譯服務(擴增中)
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
            foreach(XmlNode xd in xmlD.ChildNodes[1].ChildNodes)
                if(xd.Name == "data")
                    foreach(XmlNode xd2 in xd.ChildNodes)
                        if(xd2.Name == "value")
                            xd.InnerText = TranslateTextFromTraditionalChineseToSimplifiedChinese(xd.InnerText);

            string outputFileName = $"{Path.GetDirectoryName(traditionalChineseFileName)}\\{Path.GetFileName(traditionalChineseFileName).Split('.')[0]}.zh-CN.resx";
            using (TextWriter sw = new StreamWriter(outputFileName, false, Encoding.UTF8)) { 
                xmlD.Save(sw);
            }
        }

        //public static string NewChinese
    }
}
