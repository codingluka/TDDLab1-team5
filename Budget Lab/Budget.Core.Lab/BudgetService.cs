using System;
using System.Collections.Generic;
using System.Linq;

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

            var startMonthDays = DateTime.DaysInMonth(start.Year, start.Month);
            var startBudget = GetBudget(start);
            var startAmount = startBudget?.Amount ?? 0;
            decimal startOneDay = startAmount / startMonthDays;
            var s = (startMonthDays - start.Day + 1) * startOneDay;

            var endMonthDays = DateTime.DaysInMonth(end.Year, end.Month);
            var endBudget = GetBudget(end);
            var endAmount = endBudget?.Amount ?? 0;
            decimal endOneDay = endAmount / endMonthDays;
            var e = end.Day * endOneDay;
            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) + 1;

            if (diffMonth < 2)
            {
                //// 當月超過1日
                var diffDays = end.Subtract(start).TotalDays + 1;
                return (decimal) diffDays * startOneDay;
            }

            var tmpMid = 0m;
            for (var i = 1; i < diffMonth - 1; i++)
            {
                var currentBudget = GetBudget(start.AddMonths(i));
                var midAmount = currentBudget
                    ?.Amount ?? 0;
                tmpMid += midAmount;
            }

            return s + tmpMid + e;
        }

        private static int GetBudgetAmount(Budget budget)
        {
            return budget?.Amount ?? 0;
        }

        private Budget GetBudget(DateTime start)
        {
            return this._budgetRepo.GetAll()
                .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"));
        }
    }
}