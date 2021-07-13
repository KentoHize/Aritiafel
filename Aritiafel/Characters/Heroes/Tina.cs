using Aritiafel.Locations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aritiafel.Characters.Heroes
{
    /// <summary>
    /// 法師，英雄，人類
    /// </summary>
    public static class Tina
    {
        public static void SaveProject(ProjectChoice pc = ProjectChoice.Aritiafel, string backupDrive = "E")
        {
            string sourceDir;
            switch(pc)
            {
                case ProjectChoice.Aritiafel:
                    sourceDir = @"C:\Programs\Standard\Aritiafel";
                    break;
                case ProjectChoice.AritiafelJS:
                    sourceDir = @"C:\Programs\Javascript\AritiafelJS";
                    break;
                case ProjectChoice.TinaValidator:
                    sourceDir = @"C:\Programs\Standard\TinaValidator";
                    break;
                case ProjectChoice.JsonEditorV2:
                    sourceDir = @"C:\Programs\WinForm\JsonEditorV2";
                    break;
                case ProjectChoice.ArinaWebsiteManager:
                    sourceDir = @"C:\Programs\WinForm\ArinaWebsiteManager";
                    break;
                case ProjectChoice.RaeriharUniversity:
                    sourceDir = @"C:\Programs\Standard\Raerihar";
                    break;
                case ProjectChoice.NinjaSato:
                    sourceDir = @"C:\Programs\WinForm\NinjaSato";
                    break;
                case ProjectChoice.NSBattle:
                    sourceDir = @"C:\Programs\WinForm\NSBattle";
                    break;
                case ProjectChoice.IdealWorld:
                    sourceDir = @"C:\Programs\Web\WF\IdealWorld";
                    break;
                case ProjectChoice.DeepMind:
                    sourceDir = @"C:\Programs\WPF\DeepMind";
                    break;
                
                default:
                    throw new ArgumentException();
            }
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            rs.SaveVSSolution(sourceDir);
        }
        public static void SaveProject(string subFolderName, string projectName, string backupDrive = "E")
        {
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            rs.SaveVSSolution($@"C:\Programs\{subFolderName}\{projectName}");
        }
        public static void SaveTextFile(string folderName, string fileName, string content)
        {
            Residence rs = new Residence(folderName);
            rs.SaveLocalTextFile(fileName, content);            
        }

        public static void SaveTextFile(string path, string content)
            => Residence.SaveTextFile(path, content);

        public static string ReadTextFile(string folderName, string fileName)
        {
            Residence rs = new Residence(folderName);
            return rs.ReadLocalTextFile(fileName);
        }

        public static string ReadTextFile(string path)
            => Residence.ReadTextFile(path);

        public static void DeleteFile(string folderName, string fileName)
        {
            Residence rs = new Residence(folderName);
            rs.DeleteLocalFile(fileName);
        }

        public static void DeleteFile(string path)
            => Residence.DeleteFile(path);

        public static void DeleteFolderFiles(string folderName)
        {
            Residence rs = new Residence(folderName);
            rs.DeleteAllLocalFiles();
        }

        public static T LoadJsonFile<T>(string path)
            => Residence.LoadJsonFile<T>(path);

        public static object LoadJsonFile(string path)
            => Residence.LoadJsonFile(path);

        public static void SaveJsonFile(string path, object content, bool writeIntent = false)
            => Residence.SaveJsonFile(path, content, writeIntent);

    }

    public enum ProjectChoice
    {
        Aritiafel,
        AritiafelJS,
        TinaValidator,
        JsonEditorV2,
        ArinaWebsiteManager,
        RaeriharUniversity,
        NinjaSato,
        NSBattle,
        IdealWorld,
        DeepMind
    }
}
