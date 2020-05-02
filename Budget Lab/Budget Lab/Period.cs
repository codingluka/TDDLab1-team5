using System;

namespace Budget_Lab
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End   { get; private set; }

        public int OverlappingDays(Budget currentBudget)
        {
            DateTime overlappingEnd;
            DateTime overlappingStart;
            if (currentBudget.YearMonth == Start.ToString("yyyyMM"))
            {
                overlappingEnd = currentBudget.LastDay();
                overlappingStart = Start;
            }
            else if (currentBudget.YearMonth == End.ToString("yyyyMM"))
            {
                overlappingEnd = End;
                overlappingStart = currentBudget.FirstDay();
            }
            else
            {
                overlappingEnd = currentBudget.LastDay();
                overlappingStart = currentBudget.FirstDay();
            }

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}