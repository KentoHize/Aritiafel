using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aritiafel.Locations;

namespace Aritiafel.Characters.Heroes
{
    //遊戲備份
    public static class Sonia
    {
        public static string LogDirectory { get; set; } = @"C:\Programs\Log";

        public const string DefaultBackupDrive = "E";

        // private static Dictionary<ProjectChoice, string> ProjectFolderPath
        //    = new Dictionary<ProjectChoice, string>()
        //{
        //    { ProjectChoice.Aritiafel, @"C:\Programs\Standard\Aritiafel" }
        //}

        public static long BackupGameSave (string rootFolder, string subFolder, string backupDrive = DefaultBackupDrive)
        {
            Residence rs = new Residence($"{backupDrive}:\\GameSave");
            return rs.BackupGameData(rootFolder, subFolder);
        }

        public static long CopyGameSave(string rootFolder, string subFolder, string targetFolder)
            => CopyLatestGameSave(rootFolder, subFolder, targetFolder, 0);

        public static long CopyLatestGameSave(string rootFolder, string subFolder, string targetFolder, long inMinutes = 30)
        {
            Residence rs = new Residence(Path.Combine(targetFolder, "GameSave"));
            return rs.BackupGameData(rootFolder, subFolder, inMinutes);
        }
    }
}
