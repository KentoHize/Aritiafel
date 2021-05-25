using Aritiafel.Locations;
using System;
using System.Collections.Generic;
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
                default:
                    throw new ArgumentException();
            }
            Residence rs = new Residence($"{backupDrive}:\\Backup");
            rs.SaveVSSolution(sourceDir);
        }
    }

    public enum ProjectChoice
    {
        Aritiafel,
        TinaValidator,
        JsonEditorV2,
        ArinaWebsiteManager,
        RaeriharUniversity,
        NinjaSato,
        NSBattle
    }
}
