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

        private DateTime Start { get; }
        private DateTime End   { get; }

        public int OverlappingDays(Period another)
        {
            var overlappingEnd = another.End < End
                ? another.End
                : End;

            var overlappingStart = another.Start > Start
                ? another.Start
                : Start;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}