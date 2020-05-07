using System;
using System.Collections.Generic;

namespace Budget.Core.Lab
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public int GetAvailableDays(Period period)
        {
            var month = DateTime.ParseExact(this.YearMonth, "yyyyMM", null);
            var monthFirstDay = new DateTime(month.Year, month.Month, 01);
            var monthLastDay = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));
            var periodStart = monthFirstDay <= period.Start ? period.Start : monthFirstDay;
            var periodEnd = period.End <= monthLastDay ? period.End : monthLastDay;
            return (periodEnd - periodStart).Days + 1;
        }

        public decimal BudgetDailyAmount()
        {
            var month = DateTime.ParseExact(this.YearMonth, "yyyyMM", null);
            var dailyAmount = Amount / DateTime.DaysInMonth(month.Year, month.Month);
            return dailyAmount;
        }
    }
}