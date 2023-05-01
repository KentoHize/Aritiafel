using Aritiafel.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Aritiafel.Organizations;

namespace Aritiafel.Locations
{
    //
    //處理Setting，將設定值轉為Ini檔案、Json檔案或是其他輸出格式，以及讀取
    //
    public static class SettingShop
    {
        public static void SaveIniFile(object varObject, string file)
        {
            ArSettingGroup arg = new ArSettingGroup();
            PropertyInfo[] pis;
            if (varObject is Type)
                pis = ((Type)varObject).GetProperties();
            else
                pis = varObject.GetType().GetProperties();
            
            foreach(PropertyInfo pi in pis)
            {    
                arg.Add(pi.Name, pi.GetValue(varObject));
            }
            SaveIniFile(arg, file);
        }

        public static void SaveIniFile(ArSettingGroup arSettingGroup, string file)
        {           
            string currentSection = null;
            using (StreamWriter sw = new StreamWriter(file))
            {
                foreach (ArSetting ars in arSettingGroup)
                {
                    if (currentSection != ars.Section)
                    {   
                        sw.WriteLine($"[{ars.Section}]");
                        currentSection = ars.Section;
                    }
                    if (ars.Description != null)
                    {
                        sw.Write($"#{ars.Description.Replace("\n", "\r\n#")}");
                        sw.WriteLine();
                    }
                    if(ars.Value != null)
                        sw.WriteLine($"{ars.Key}={ars.Value.ToArString()}");
                }
            }
        }

        public static void LoadIniFile(object varObject, string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {   
                while(!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    if (s[0] == '#' || s[0] == ';' || s[0] == '[')
                        continue;                    
                    string[] s2 = s.Split('=');
                    PropertyInfo pi;
                    if (varObject is Type)
                        pi = ((Type)varObject).GetProperty(s2[0]);
                    else
                        pi = varObject.GetType().GetProperty(s2[0]);

                    if (pi != null)
                        pi.SetValue(varObject, s2[1].ParseArString(pi.PropertyType));
                    else
                        throw new KeyNotFoundException(s2[0]);
                }   
            }
        }
      
    }   
}
