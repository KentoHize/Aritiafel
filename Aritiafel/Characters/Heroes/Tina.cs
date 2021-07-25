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
        private static Dictionary<ProjectChoice, string> ProjectFolderPath
            = new Dictionary<ProjectChoice, string>()
        {
           { ProjectChoice.Aritiafel, @"C:\Programs\Standard\Aritiafel" },
           { ProjectChoice.AritiafelJS, @"C:\Programs\Javascript\AritiafelJS" },
           { ProjectChoice.TinaValidator, @"C:\Programs\Standard\TinaValidator" },
           { ProjectChoice.JsonEditorV2, @"C:\Programs\WinForm\JsonEditorV2" },
           { ProjectChoice.ArinaWebsiteManager, @"C:\Programs\WinForm\ArinaWebsiteManager" },
           { ProjectChoice.RaeriharUniversity, @"C:\Programs\Standard\Raerihar" },
           { ProjectChoice.NinjaSato, @"C:\Programs\WinForm\NinjaSato" },
           { ProjectChoice.NSBattle, @"C:\Programs\WinForm\NSBattle" },
           { ProjectChoice.IdealWorld, @"C:\Programs\Web\WF\IdealWorld" },
           { ProjectChoice.DeepMind, @"C:\Programs\WPF\DeepMind" },
           { ProjectChoice.ASON, @"C:\Programs\Standard\ArinaStandardObjectNotation" }
        };

        public static void SaveProject(ProjectChoice pc = ProjectChoice.Aritiafel, string backupDrive = "E")
        {
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            rs.SaveVSSolution(ProjectFolderPath[pc], true, new string[] { "Data" });
        }
        public static void SaveProject(string subFolderName, string projectName, string backupDrive = "E")
        {
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            rs.SaveVSSolution($@"C:\Programs\{subFolderName}\{projectName}", true, new string[] { "Data" });
        }

        public static void SaveProjectData(ProjectChoice pc = ProjectChoice.Aritiafel, string backupDrive = "E")
        {
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            rs.SaveVSSolutionData(ProjectFolderPath[pc]);
        }

        public static void SaveProjectData(string subFolderName, string projectName, string dataFolderName, string backupDrive = "E")
        {
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            rs.SaveVSSolutionData($@"C:\Programs\{subFolderName}\{projectName}\{dataFolderName}");
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
        DeepMind,
        ASON
    }
}
