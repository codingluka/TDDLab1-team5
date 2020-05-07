using System;

namespace Budget.Core.Lab
{
    public class Budget
    {
        private DateTime _month;
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        private int GetAvailableDays(Period period)
        {
            _month = DateTime.ParseExact(YearMonth, "yyyyMM", null);
            var monthFirstDay = new DateTime(_month.Year, _month.Month, 01);
            var monthLastDay = new DateTime(_month.Year, _month.Month, DateTime.DaysInMonth(_month.Year, _month.Month));
            var periodStart = monthFirstDay <= period.Start ? period.Start : monthFirstDay;
            var periodEnd = period.End <= monthLastDay ? period.End : monthLastDay;
            if (periodEnd >= periodStart) return (periodEnd - periodStart).Days + 1;

            return 0;
        }

        private decimal BudgetDailyAmount()
        {
            var dailyAmount = Amount / DateTime.DaysInMonth(_month.Year, _month.Month);
            return dailyAmount;
        }

        public decimal GetOverlapBudget(Period period)
        {
            return GetAvailableDays(period) * BudgetDailyAmount();
        }
    }
}