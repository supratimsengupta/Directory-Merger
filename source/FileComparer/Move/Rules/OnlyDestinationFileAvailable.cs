using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileComparer.Move.Rules
{
    public class OnlyDestinationFileAvailable:IRule
    {
        string source, destination;
        public OnlyDestinationFileAvailable(string source, string dest)
        {
            this.source = source;
            this.destination = dest;
        }

        public override void ApplyRule()
        {
            if (IsDirectory(source))
            {
                if (IsDirectory(destination))
                {
                    Directory.Move(destination, source);
                }
            }
        }
    }
}
