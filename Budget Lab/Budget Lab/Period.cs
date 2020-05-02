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

            var overlappingEnd = anotherPeriod.End < End
                ? anotherPeriod.End
                : End;

            var overlappingStart = anotherPeriod.Start > Start
                ? anotherPeriod.Start
                : Start;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}