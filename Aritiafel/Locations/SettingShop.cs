using Aritiafel.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Aritiafel.Locations
{
    //
    //處理Setting，將設定值轉為Ini檔案、Json檔案或是其他輸出格式，以及讀取
    //
    public static class SettingShop
    {
        public static void SaveIniFile(ArSettingGroup arSettingGroup, string file)
        {
            //for(int i = 0; i < arSettingGroup.Count; i++)
            foreach(ArSetting ars in arSettingGroup)
            {
                StreamWriter sw = new StreamWriter(file);
                //sw.WriteLine(ars.)
            }
        }

        public static ArSettingGroup LoadIniFile(string file)
        {
            return null;
        }
    }
}
