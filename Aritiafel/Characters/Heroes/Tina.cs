using Aritiafel.Locations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aritiafel.Characters.Heroes
{
    ///程式備份、檔案IO
    /// <summary>
    /// 法師，英雄，人類
    /// </summary>
    public static class Tina
    {
        public static string LogDirectory { get; set; } = @"C:\Programs\Log";

        public const string DefaultBackupDrive = "E";

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
           { ProjectChoice.ASON, @"C:\Programs\Standard\ArinaStandardObjectNotation" },
           { ProjectChoice.Elibrar, @"C:\Programs\WinForm\Elibrar"},
           { ProjectChoice.ArinaGameGuide, @"C:\Programs\ArinaGameGuide"},
           { ProjectChoice.ArinaWorld, @"C:\Programs\ArinaWorld"}

        };

        public static void SaveProject(ProjectChoice pc = ProjectChoice.Aritiafel, string backupDrive = DefaultBackupDrive)
        {
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            rs.SaveVSSolution(ProjectFolderPath[pc], true, new string[] { "Data" });
        }

        public static void SaveProjectWithData(string subFolderName, string projectName, string dataFolderName = "Data", string backupDrive = DefaultBackupDrive)
        {
            SaveProject(subFolderName, projectName, backupDrive);
            SaveProjectData(subFolderName, projectName, dataFolderName, backupDrive);
        }

        public static void SaveProject(string subFolderName, string projectName, string backupDrive = DefaultBackupDrive)
        {
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            if (string.IsNullOrEmpty(subFolderName))
                rs.SaveVSSolution($@"C:\Programs\{projectName}", true, new string[] { "Data" });
            else
                rs.SaveVSSolution($@"C:\Programs\{subFolderName}\{projectName}", true, new string[] { "Data" });
        }

        public static void SaveProjectData(ProjectChoice pc = ProjectChoice.Aritiafel, string backupDrive = DefaultBackupDrive)
        {
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            rs.SaveVSSolutionData(ProjectFolderPath[pc]);
        }

        public static void SaveProjectData(string subFolderName, string projectName, string dataFolderName = "Data", string backupDrive = DefaultBackupDrive)
        {
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            if(string.IsNullOrEmpty(subFolderName))
                rs.SaveVSSolutionData($@"C:\Programs\{projectName}\{dataFolderName}");
            else
                rs.SaveVSSolutionData($@"C:\Programs\{subFolderName}\{projectName}\{dataFolderName}");
        }

        public static void SaveTextFile(string folderName, string fileName, string content)
        {
            Residence rs = new Residence(folderName);
            rs.SaveLocalTextFile(fileName, content);            
        }

        public static void SaveTextFile(string path, string content)
            => Residence.SaveTextFile(path, content);

        public static void SaveTextFile(string content)
            => Residence.SaveTextFile(Path.Combine(LogDirectory, $"{DateTime.Now.ToString("yyyy-MM-dd-HHmmss")}.txt"), content);

        public static void AppendTextFile(string folderName, string fileName, string content)
        {
            Residence rs = new Residence(folderName);
            rs.AppendLocalTextFile(fileName, content);
        }

        public static void AppendTextFile(string path, string content)
            => Residence.AppendTextFile(path, content);

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
        ASON,
        Elibrar,
        ArinaGameGuide,
        ArinaWorld
    }
}
