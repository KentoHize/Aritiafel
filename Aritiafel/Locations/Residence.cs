using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aritiafel;
using System.Text.RegularExpressions;
using Aritiafel.Locations.StorageHouse;

namespace Aritiafel.Locations
{
    public class Residence
    {
        public string Address { get; set; }
        public Residence(string directoryPath = "")
        {
            Address = directoryPath;
        }

        public void SaveVSSolution(string SolutionDirectoryPath, bool excludeTestResults = true, string[] addtionalIgnoreDirNames = null)
        {
            if (string.IsNullOrEmpty(Address))
                throw new ArgumentNullException(nameof(Address));

            if (string.IsNullOrEmpty(SolutionDirectoryPath))
                throw new ArgumentNullException(nameof(SolutionDirectoryPath));

            List<string> ignoreDirNames = new List<string>
            {
                "bin",
                "obj",
                ".vs",
                ".git",
                "packages"
            };

            if (excludeTestResults)
                ignoreDirNames.Add("TestResults");

            if (addtionalIgnoreDirNames != null)
                ignoreDirNames.AddRange(addtionalIgnoreDirNames);

            DirectoryCopy(SolutionDirectoryPath, Path.Combine(Address, Path.GetFileName(SolutionDirectoryPath)), true, ignoreDirNames.ToArray());
        }

        public static string ReadTextFile(string path)
        {
            string result;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                result = sr.ReadToEnd();
                sr.Close();
            }
            return result;
        }
        public string ReadLocalTextFile(string fileName)
           => ReadTextFile(Path.Combine(Address, fileName));
        
        public static void SaveTextFile(string path, string content)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(content);
                sw.Close();
            }
        }
        public void SaveLocalTextFile(string fileName, string content)
            => SaveTextFile(Path.Combine(Address, fileName), content);

        public static T LoadJsonFile<T>(string path, ReferenceTypeReadAndWritePolicy rwPolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName)
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            DefaultJsonConverterFactory djcf = new DefaultJsonConverterFactory {                
                ReferenceTypeReadAndWritePolicy = rwPolicy
            };            
            jso.Converters.Add(djcf);
            return JsonSerializer.Deserialize<T>(ReadTextFile(path), jso);
        }

        public T LoadLocalJsonFile<T>(string fileName, ReferenceTypeReadAndWritePolicy rwPolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName)
            => LoadJsonFile<T>(Path.Combine(Address, fileName), rwPolicy);

        public static object LoadJsonFile(string path, ReferenceTypeReadAndWritePolicy rwPolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName)
            => LoadJsonFile<object>(path, rwPolicy);

        public object LoadLocalJsonFile(string fileName, ReferenceTypeReadAndWritePolicy rwPolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName)
            => LoadJsonFile<object>(Path.Combine(Address, fileName), rwPolicy);

        public static void SaveJsonFile(string path, object content, bool writeIntent = false, ReferenceTypeReadAndWritePolicy rwPolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName)
        {
            JsonSerializerOptions jso = new JsonSerializerOptions
            { WriteIndented = true };
            DefaultJsonConverterFactory djcf = new DefaultJsonConverterFactory {
                ReferenceTypeReadAndWritePolicy = rwPolicy
            };
            jso.Converters.Add(djcf);
            SaveTextFile(path, JsonSerializer.Serialize(content, content.GetType(), jso));
        }

        public void SaveLocalJsonFile(string fileName, object content, bool writeIntent = false, ReferenceTypeReadAndWritePolicy rwPolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName)
            => SaveJsonFile(Path.Combine(Address, fileName), content, writeIntent, rwPolicy);

        public static void DeleteFile(string path)
            => File.Delete(path);

        public void DeleteLocalFile(string fileName)
            => File.Delete(Path.Combine(Address, fileName));

        public void DeleteAllLocalFiles()
        {
            if (string.IsNullOrEmpty(Address))
                throw new ArgumentNullException(nameof(Address));
            string[] files = Directory.GetFiles(Address);
            for (int i = 0; i < files.Length; i++)
                File.Delete(files[i]);
        }

        private bool FitsMask(string fileName, string fileMask)
        {
            Regex mask = new Regex(
                '^' +
                fileMask
                    .Replace(".", "[.]")
                    .Replace("*", ".*")
                    .Replace("?", ".")
                + '$',
                RegexOptions.IgnoreCase);
            return mask.IsMatch(fileName);
        }

        private void DirectoryCopy(string sourceDirectory, string targetDirectory, bool includeSubDirectory = true, string[] ignoreDirectoryNames = null, string[] ignoreFileFilters = null)
        {
            if (!Directory.Exists(sourceDirectory))
                throw new DirectoryNotFoundException();

            string[] files = Directory.GetFiles(sourceDirectory);
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                if (ignoreFileFilters != null)
                    foreach (string filter in ignoreFileFilters)
                        if (FitsMask(fileName, filter))
                            goto BreakPoint;
                File.Copy(file, Path.Combine(targetDirectory, fileName), true);
            BreakPoint:;
            }

            if (includeSubDirectory)
            {
                string[] subDirectories = Directory.GetDirectories(sourceDirectory);
                foreach (string subDir in subDirectories)
                {
                    string subDirName = Path.GetFileName(subDir);
                    if (ignoreDirectoryNames != null)
                        foreach (string dirName in ignoreDirectoryNames)
                            if (dirName == subDirName)
                                goto BreakPoint2;
                    DirectoryCopy(subDir, Path.Combine(targetDirectory, subDirName), includeSubDirectory, ignoreDirectoryNames, ignoreFileFilters);
                BreakPoint2:;
                }
            }
        }
    }
}
