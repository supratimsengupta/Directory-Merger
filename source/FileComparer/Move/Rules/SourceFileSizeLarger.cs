using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileComparer.Move.Rules
{
    public class SourceFileSizeLarger : IRule
    {
        private string source, dest;
        public SourceFileSizeLarger(string source, string destination)
        {
            this.source = source;
            this.dest = destination;
        }
        #region IRule Members

        public override void ApplyRule()
        {
            if (IsDirectory(dest))
            {
                Directory.Delete(dest, true);
            }
            else
            {
                File.Delete(dest);
            }
        }

        #endregion


    }
}
