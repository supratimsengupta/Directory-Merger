using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FileComparer.Move.Rules;

namespace DirectoryComparer
{
    public delegate void SomeAction();
    public class MultiFileMover
    {
        private Semaphore semaphore = new Semaphore(5, 5);

        public void MoveFile(object rule)
        {
            IRule ruleToExecute = rule as IRule;
            semaphore.WaitOne();
            ruleToExecute.ApplyRule();
            semaphore.Release();
        }
    }
}
