#region

using System;

#endregion

namespace Budget_Lab
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int    Amount    { get; set; }

        public int OverlappingAmount(Period period)
        {
            return period.OverlappingDays(CreatePeriod()) * DailyAmount();
        }

        private Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        private int DailyAmount()
        {
            return Amount / Days();
        }

        private int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        private DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        private DateTime LastDay()
        {
            return new DateTime(FirstDay().Year, FirstDay().Month, Days());
        }
    }
}