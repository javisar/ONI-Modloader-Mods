using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MaterialColor.Data
{
    /// <summary>
    /// Provides filtering method and rules. Used to find matching types to apply color.
    /// </summary>
    public class TypeFilter
    {
        /// <param name="methodID">
        /// 0 = Exclusive, 1 = Inclusive, 2 = RegexAny, 3 = RegexAll
        /// </param>
        public TypeFilter(IEnumerable<string> rules = null, int methodID = 0)
        {
            this.MethodID = methodID;
            this.Rules = new List<string>();

            if (rules != null)
            {
                this.Rules.AddRange(rules);
            }
        }

        // TODO: change to method string?
        /// <summary>
        /// 0 = Exclusive, 1 = Inclusive, 2 = RegexAny, 3 = RegexAll
        /// </summary>
        public readonly int MethodID;
        public readonly List<string> Rules;

        // TODO: caching strategy could be helpful (performance-wise)
        public bool IsAllowed(string name)
        {
            switch (this.MethodID)
            {
                case 0: // Exclusive
                    return !this.AnyMatch(name);
                case 1: // Inclusive
                    return this.AnyMatch(name);
                case 2: // RegexAny
                    return this.AnyMatchRegex(name);
                case 3: // RegexAll
                    return this.AllMatchRegex(name);
                default:// InvalidMethodID
                    throw new Exception("Invalid method ID: " + this.MethodID);
            }
        }

        // TODO: ? use contains instead of == ?
        private bool AnyMatch(string name)
        {
            return this.Rules.Any(rule => name.ToUpper().Contains(rule.ToUpper()));
        }

        private bool AnyMatchRegex(string name)
        {
            return this.Rules.Any(rule => Regex.IsMatch(name, rule));
        }

        private bool AllMatchRegex(string name)
        {
            return this.Rules.All(rule => Regex.IsMatch(name, rule));
        }
    }
}
