using MaterialColor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaterialColor
{
    /// <summary>
    /// Filters text based on method and rules supplied.
    /// Designed for filtering building for coloring in HarmonyPatches.UpdateBuildingColor.
    /// </summary>
    public class TextFilter
    {
        /// <summary>
        /// Chooses strategy for filtering based on info supplied.
        /// </summary>
        public TextFilter(FilterInfo info)
        {
            this.Rules = info.Rules;
            this.MatchId = info.ExactMatch;

            if (info.Inclusive)
            {
                this.Check = this.InclusiveCheck;
            }
            else
            {
                this.Check = this.ExclusiveCheck;
            }
        }

        /// <summary>
        /// Checks if value passes through the filter's ruleset.
        /// </summary>
        public Func<string, bool> Check;

        private readonly bool MatchId;

        private readonly List<string> Rules;

        private bool InclusiveCheck(string value)
        {
            return this.Rules.Any(rule => this.MatchId
                ? value.Equals(rule)
                : value.Contains(rule));
        }

        private bool ExclusiveCheck(string value)
        {
            return !this.Rules.Any(rule => this.MatchId
                ? value.Equals(rule)
                : value.Contains(rule));
        }
    }
}
