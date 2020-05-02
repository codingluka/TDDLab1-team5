#region

using System;

#endregion

namespace Budget_Lab
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; }
        public DateTime End   { get; }

        public int OverlappingDays(Budget currentBudget)
        {
            var overlappingEnd = currentBudget.LastDay() < End
                ? currentBudget.LastDay()
                : End;

            var overlappingStart = currentBudget.FirstDay() > Start
                ? currentBudget.FirstDay()
                : Start;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}