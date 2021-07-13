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

            //if (string.IsNullOrEmpty(SolutionDirectoryPath))
            //    throw new ArgumentNullException("SolutionDirectoryPath");

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

        public string LoadTextFile(string fileName)
        {
            if (string.IsNullOrEmpty(Address))
                throw new ArgumentNullException(nameof(Address));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            string result;
            using (FileStream fs = new FileStream(Path.Combine(Address, fileName), FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                result = sr.ReadToEnd();
                sr.Close();
            }
            return result;
        }

        public void SaveTextFile(string fileName, string content)
        {
            if (string.IsNullOrEmpty(Address))            
                throw new ArgumentNullException(nameof(Address));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            using (FileStream fs = new FileStream(Path.Combine(Address, fileName), FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(content);
                sw.Close();
            }
        }

        public T LoadJsonFile<T>(string fileName, ReferenceTypeReadAndWritePolicy rwPolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName)
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            DefaultJsonConverterFactory djcf = new DefaultJsonConverterFactory {                
                ReferenceTypeReadAndWritePolicy = rwPolicy
            };            
            jso.Converters.Add(djcf);
            return JsonSerializer.Deserialize<T>(LoadTextFile(fileName), jso);
        }

        public object LoadJsonFile(string fileName, ReferenceTypeReadAndWritePolicy rwPolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName)
            => LoadJsonFile<object>(fileName, rwPolicy);

        public void SaveJsonFile(string fileName, object content, bool WriteIntent = false, ReferenceTypeReadAndWritePolicy rwPolicy = ReferenceTypeReadAndWritePolicy.TypeNestedName)
        {
            JsonSerializerOptions jso = new JsonSerializerOptions
            { WriteIndented = true };
            DefaultJsonConverterFactory djcf = new DefaultJsonConverterFactory {
                ReferenceTypeReadAndWritePolicy = rwPolicy
            };
            jso.Converters.Add(djcf);
            SaveTextFile(fileName, JsonSerializer.Serialize(content, content.GetType(), jso));
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
