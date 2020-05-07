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
                var currentBudget = GetBudget(currentMonth);

                var availableDays = currentBudget.GetAvailableDays(new Period(start, end));

                decimal dailyAmount = currentBudget.BudgetDailyAmount(GetDaysInMonth(currentMonth));
                result += availableDays * dailyAmount;
            }

            return result;
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