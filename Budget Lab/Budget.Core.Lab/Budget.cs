using System;

namespace Budget.Core.Lab
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        private int GetAvailableDays(Period period)
        {
            var month = DateTime.ParseExact(YearMonth, "yyyyMM", null);
            var monthFirstDay = new DateTime(month.Year, month.Month, 01);
            var monthLastDay = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));
            var periodStart = monthFirstDay <= period.Start ? period.Start : monthFirstDay;
            var periodEnd = period.End <= monthLastDay ? period.End : monthLastDay;
            if (periodEnd >= periodStart) return (periodEnd - periodStart).Days + 1;

            return 0;
        }

        private decimal BudgetDailyAmount()
        {
            var month = DateTime.ParseExact(YearMonth, "yyyyMM", null);
            var dailyAmount = Amount / DateTime.DaysInMonth(month.Year, month.Month);
            return dailyAmount;
        }

        public decimal GetOverlapBudget(Period period)
        {
            return GetAvailableDays(period) * BudgetDailyAmount();
        }
    }
}