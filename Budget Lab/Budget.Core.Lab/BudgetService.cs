using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace Budget.Core.Lab
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            this._budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (end < start)
            {
                return 0;
            }

            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month);

            var result = 0m;
            for (var i = 0; i < diffMonth + 1; i++)
            {
                var currentMonth = start.AddMonths(i);

                int currentMonthDays = GetDaysInMonth(currentMonth);
                var availableDays = GetAvailableDays(new Period(start, end), diffMonth, i, currentMonth);

                var currentBudget = GetBudget(currentMonth);
                var currentBudgetAmount = currentBudget?.Amount ?? 0;
                var dailyAmount = currentBudgetAmount / currentMonthDays;
                result += availableDays * dailyAmount;
            }

            return result;
        }

        private int GetAvailableDays(Period period, int diffMonth, int i, DateTime currentMonth)
        {
            var monthFirstDay = new DateTime(currentMonth.Year,currentMonth.Month,01);
            var monthLastDay = new DateTime(currentMonth.Year,currentMonth.Month,DateTime.DaysInMonth(currentMonth.Year,currentMonth.Month));
            var periodStart = monthFirstDay <= period.Start ? period.Start : monthFirstDay;
            var periodEnd = period.End <= monthLastDay ? period.End : monthLastDay;
            return (periodEnd - periodStart).Days + 1;
        }

        private int GetDaysInMonth(DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        private Budget GetBudget(DateTime start)
        {
            return this._budgetRepo.GetAll()
                .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"));
        }
    }
}