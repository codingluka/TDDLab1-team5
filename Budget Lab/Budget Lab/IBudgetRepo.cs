using System;
using System.Collections.Generic;

namespace Budget_Lab
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        public DateTime LastDay()
        {
            return new DateTime(FirstDay().Year, FirstDay().Month, Days());
        }

        public int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        public int DailyAmount()
        {
            return Amount / Days();
        }

        public Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }
    }
}
