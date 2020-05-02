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
            var anotherPeriod = new Period(currentBudget.FirstDay(), currentBudget.LastDay());
            
            var lastDay = currentBudget.LastDay();
            var firstDay = currentBudget.FirstDay();
            
            var overlappingEnd = lastDay < End
                ? lastDay
                : End;

            var overlappingStart = firstDay > Start
                ? firstDay
                : Start;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}