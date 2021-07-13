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
        public static void SaveTextFile(string folderName, string fileName, string content)
        {
            Residence rs = new Residence(folderName);
            rs.SaveTextFile(fileName, content);            
        }

        public static void SaveTextFile(string path, string content)
            => SaveTextFile(Path.GetFullPath(path), Path.GetFileName(path), content);

        public static string LoadTextFile(string folderName, string fileName)
        {
            Residence rs = new Residence(folderName);
            return rs.LoadTextFile(fileName);
        }

        public static string LoadTextFile(string path)
            => LoadTextFile(Path.GetFullPath(path), Path.GetFileName(path));
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
