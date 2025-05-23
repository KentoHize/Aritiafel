﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Aritiafel.Characters.Heroes;
using Aritiafel.Items;
using Aritiafel.Organizations.ArinaOrganization;
using Microsoft.VisualBasic;

namespace Aritiafel.Locations
{
    //
    //處理Setting，將設定值轉為Ini檔案、Json檔案或是其他輸出格式，以及讀取
    //
    public static class SettingShop
    {
        public static string DefaultSettingFilePath = Path.Combine(Application.UserAppDataPath, "setting.ini");
        public static ArSettingGroup GetArSettinGroup(object typeOrInstance)
        {
            if (typeOrInstance == null)
                throw new ArgumentNullException(nameof(typeOrInstance));
            ArSettingGroup arg = new ArSettingGroup();
            PropertyInfo[] pis;
            if (typeOrInstance is Type)
                pis = ((Type)typeOrInstance).GetProperties();
            else
                pis = typeOrInstance.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.GetCustomAttribute(typeof(ArIgnoreAttribute)) != null)
                    continue;
                if (pi.PropertyType.IsClass && !pi.PropertyType.Namespace.StartsWith("System"))
                    continue;
                string section = null, desc = null;
                Attribute a = pi.GetCustomAttribute(typeof(ArSectionAttribute));
                if (a != null)
                    section = ((ArSectionAttribute)a).Name;
                a = pi.GetCustomAttribute(typeof(ArDescriptionAttribute));
                if (a != null)
                    desc = ((ArDescriptionAttribute)a).Text;
                arg.Add(pi.Name, pi.GetValue(typeOrInstance), section, desc);
            }
            return arg;
        }
        public static void ResetIniFile()
            => Tina.DeleteFile(DefaultSettingFilePath);

        public static void SaveIniFile(object typeOrInstance, string file = null)
        {
            SaveIniFile(GetArSettinGroup(typeOrInstance), file);
        }

        public static void SaveIniFile(ArSettingGroup arSettingGroup, string file = null)
        {
            string currentSection = null;
            if (arSettingGroup == null)
                throw new ArgumentNullException(nameof(arSettingGroup));
            if (file == null)
                file = DefaultSettingFilePath;
            using (StreamWriter sw = new StreamWriter(file))
            {
                if (!string.IsNullOrEmpty(arSettingGroup.StartComment))
                {
                    sw.Write($"#{arSettingGroup.StartComment.Replace("\n", "\r\n#")}");
                    sw.WriteLine();
                }
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
                    if (ars.Value != null)
                        sw.WriteLine($"{ars.Key}={ars.Value.ToArString()}");
                }
                if (!string.IsNullOrEmpty(arSettingGroup.EndComment))
                {
                    sw.Write($"#{arSettingGroup.EndComment.Replace("\n", "\r\n#")}");
                    sw.WriteLine();
                }
            }
        }

        public static void LoadIniFile(object typeOrInstance, string file = null, bool errorIfKeyNotFound = false)
        {
            if (typeOrInstance == null)
                throw new ArgumentNullException(nameof(typeOrInstance));
            if (file != null && !File.Exists(file))
                throw new FileNotFoundException(file);
            if (file == null)
            {
                file = DefaultSettingFilePath;
                if (!File.Exists(file))
                    return;
            }

            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    if (s[0] == '#' || s[0] == ';' || s[0] == '[')
                        continue;
                    string[] s2 = s.Split('=');
                    PropertyInfo pi;
                    if (typeOrInstance is Type)
                        pi = ((Type)typeOrInstance).GetProperty(s2[0]);
                    else
                        pi = typeOrInstance.GetType().GetProperty(s2[0]);

                    //if (pi.PropertyType.IsClass && !pi.PropertyType.Namespace.StartsWith("System"))
                    //    continue;
                    if (pi != null)
                        pi.SetValue(typeOrInstance, s2[1].ParseArString(pi.PropertyType));
                    else
                    {
                        if (errorIfKeyNotFound)
                            throw new KeyNotFoundException(s2[0]);
                    }
                }
            }
        }

    }
}
