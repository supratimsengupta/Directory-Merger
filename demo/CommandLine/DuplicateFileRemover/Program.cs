using System;
using System.Collections.Generic;
using System.Text;

using FileComparer;
using FileComparer.Move.Rules;
using System.Threading;

namespace DuplicateFileRemover
{
    class Program
    {
        static void Main(string[] args)
        {
            FileComparer.Scan.DirectoryScanner sourceScanner = new FileComparer.Scan.DirectoryScanner(args[0]);
            FileComparer.Scan.DirectoryScanner destScanner = new FileComparer.Scan.DirectoryScanner(args[1]);

            sourceScanner.ScanDirectory(sourceScanner.SourceDirectory);
            destScanner.ScanDirectory(destScanner.SourceDirectory);

            FileComparer.Compare.FileComparer fileComparer = new FileComparer.Compare.FileComparer(sourceScanner.ScannedInfo, destScanner.ScannedInfo);
            fileComparer.LeftOverLocation = "";
            fileComparer.OnComparisonCompleted += new FileComparer.Compare.ComparisonCompletionHandler(fileComparer_OnComparisonCompleted);
            fileComparer.Compare();

        }

        static void fileComparer_OnComparisonCompleted(List<FileComparer.Move.Rules.IRule> rules)
        {
            MultiFileMover fileMover = new MultiFileMover();
            foreach (IRule rule in rules)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(fileMover.MoveFile));
                thread.Start(rule);
            }
        }
    }
}
