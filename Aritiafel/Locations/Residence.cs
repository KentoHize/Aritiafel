﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Aritiafel;
using System.Text.RegularExpressions;

namespace Aritiafel.Locations
{
    public class Residence
    {
        public string Address { get; set; }
        public Residence(string backupDirectoryPath)
        {
            Address = backupDirectoryPath;
        }

        public void SaveVSSolution(string SolutionDirectoryPath, string[] ignoreDirName = null)
        {
            if (string.IsNullOrEmpty(Address))
                throw new ArgumentNullException("Address");

            if (string.IsNullOrEmpty(SolutionDirectoryPath))
                throw new ArgumentNullException("SolutionDirectoryPath");



            DirectoryCopy(SolutionDirectoryPath, Address);
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