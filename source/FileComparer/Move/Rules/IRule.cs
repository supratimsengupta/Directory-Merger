using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileComparer.Move.Rules
{
    public abstract class IRule
    {
        public abstract void ApplyRule();
        public bool IsDirectory(string fullPath)
        {
            return (File.GetAttributes(fullPath) == FileAttributes.Directory);
        }
    }
}
