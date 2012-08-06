using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileComparer.Move.Rules
{
    public class DestinationFileLarger : IRule
    {
        string source, dest;

        public DestinationFileLarger(string source, string destination)
        {
            this.source = source;
            this.dest = destination;
        }
        

        public override void ApplyRule()
        {

            CopyDestinationToSource(source, dest);
            Delete(source);
            Delete(dest);
            
            FileInfo sourceFile = new FileInfo(source);
            
            File.Copy(dest, sourceFile.Directory.Parent.FullName);
            File.Delete(source);
            File.Delete(dest);
        }

        private void Delete(string source)
        {
            if (IsDirectory(source))
            {
                Directory.Delete(source, true);
                return;
            }
            File.Delete(source);
        }

        private void CopyDestinationToSource(string source, string dest)
        {
            if (IsDirectory(source))
            {
                string sourceParentFolder = GetParentFolder(source);
                Directory.Move(sourceParentFolder, dest);
            }
        }

        private string GetParentFolder(string source)
        {
            if (IsDirectory(source))
            {
                return Directory.GetParent(source).FullName;
            }
            return new FileInfo(source).Directory.FullName;
        }


        
    }
}
