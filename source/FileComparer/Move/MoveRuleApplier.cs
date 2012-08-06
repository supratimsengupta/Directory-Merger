using System;
using System.Collections.Generic;
using System.Text;
using FileComparer.Move.Rules;

namespace FileComparer.Move
{
    public class MoveRuleApplier
    {
        private List<Rules.IRule> rules = new List<Rules.IRule>();

        public void AddRule(IRule rule)
        {
            rules.Add(rule);
        }

        public void RemoveRule(IRule rule)
        {
            rules.Remove(rule);
        }

        public void ClearRules()
        {
            foreach (IRule rule in rules)
            {
                rules.Remove(rule);
            }
        }

        public void ApplyRules()
        {
            foreach(IRule rule in rules)
            {
                rule.ApplyRule();
            }
        }
    }
}
