using System;
using System.Collections.Generic;
using System.Text;
using FileComparer.Move.Rules;
using System.IO;

namespace FileComparer.Compare
{
    public delegate void ComparisonCompletionHandler(List<IRule> rules);
    public class FileComparer
    {
        private Dictionary<string, string> source, destination;
        private List<IRule> moveRules = new List<IRule>();
        public event ComparisonCompletionHandler OnComparisonCompleted; 
        public FileComparer(Dictionary<string, string> source, Dictionary<string, string> destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public void Compare()
        {
            foreach (string key in source.Keys)
            {
                if (destination.ContainsKey(key))
                {
                    IRule rule = GetRule(source[key], destination[key]);
                    moveRules.Add(rule);
                    source.Remove(key);
                    destination.Remove(key);
                }
            }

            ActOnLeftOvers();
            if (OnComparisonCompleted != null)
            {
                OnComparisonCompleted(moveRules);
            }
            // if destination is left with files/ folders add them to an new rule
        }

        private void ActOnLeftOvers()
        {
            foreach (string key in destination.Keys)
            { 
                IRule rule = new OnlyDestinationFileAvailable(LeftOverLocation, key);
                moveRules.Add(rule);
            }
        }

        private IRule GetRule(string sourcePath, string destPath)
        {
            IRule rule = null;
            if (GetDataSize(sourcePath) >= GetDataSize(destPath))
            {
                rule = new SourceFileSizeLarger(sourcePath, destPath);
            }
            else
            {
                rule = new DestinationFileLarger(sourcePath, destPath);
            }
            // If sourcepath file size is lesser than destpath file size
            return rule;
        }

        private long GetDataSize(string sourcePath)
        {
            long size = 0;
            if (File.GetAttributes(sourcePath) == FileAttributes.Directory)
            {
                FileInfo[] files = new DirectoryInfo(sourcePath).GetFiles();
                foreach (FileInfo file in files)
                {
                    size += file.Length;
                }
                
            }else{
                size = new FileInfo(sourcePath).Length;
            }

            return size;
        }

        public string LeftOverLocation { get; set; }
    }
}
