using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace FileComparer.Scan
{
    public class DirectoryScanner
    {
        private Dictionary<string,string> scannedInfo = new Dictionary<string,string>();
        private string _path;

        public Dictionary<string, string> ScannedInfo
        { 
            get { return scannedInfo; } 
        }

        public DirectoryInfo SourceDirectory
        {
            get { return new DirectoryInfo(_path); }
        }

        public DirectoryScanner(string path)
        {
            _path = path;
        }

        public void ScanDirectory(DirectoryInfo source)
        {
            DirectoryInfo[] subDirectories = source.GetDirectories();
            foreach (DirectoryInfo directory in subDirectories)
            {
                AddToScannedInfo(directory.Name,directory.FullName);
                ScanDirectory(directory);
                ScanFiles(directory);
            }
        }

        private void ScanFiles(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles();
            
            foreach (FileInfo file in files)
            {
                if (GetFileSize(file.Length) > 500)
                {
                    string key = file.Name.Replace("." + file.Extension, "");
                    if (DirectoryNameMatchesFileName(directory, file)) { break; }
                    AddToScannedInfo(key, file.FullName);
                }
            }
        }

        private int GetFileSize(long p)
        {
            return (int)p >> 30;
        }

        private bool DirectoryNameMatchesFileName(DirectoryInfo directory, FileInfo file)
        {
            Regex regex = new Regex(file.Name, RegexOptions.IgnoreCase);
            Match match = regex.Match(directory.Name);
            return match.Success;
        }

        private void AddToScannedInfo(string key, string value)
        {
            if (scannedInfo.ContainsKey(key.ToLower()))
            { return; }
            else
            { scannedInfo.Add(key.ToLower(), value); }
        }
    }
}
