using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Locations;

namespace Aritiafel.Characters.Heroes
{
    public static class Sonia
    {
        public static string LogDirectory { get; set; } = @"C:\Programs\Log";

        public const string DefaultBackupDrive = "E";

        // private static Dictionary<ProjectChoice, string> ProjectFolderPath
        //    = new Dictionary<ProjectChoice, string>()
        //{
        //    { ProjectChoice.Aritiafel, @"C:\Programs\Standard\Aritiafel" }
        //}

        public static void BackupGameSave (string rootFolder, string subFolder, string backupDrive = DefaultBackupDrive)
        {
            Residence rs = new Residence($"{backupDrive}:\\GameSave");
            rs.BackupGameData(rootFolder, subFolder);
        }
    }
}
