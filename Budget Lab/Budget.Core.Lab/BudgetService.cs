using System;
using System.Collections.Generic;
using System.Linq;
using Budget_Lab;

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

            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) + 1;
            var startMonthDays = DateTime.DaysInMonth(start.Year, start.Month);
            var startBudget = GetBudget(start);
            var startAmount = startBudget
                                  ?.Amount ?? 0;
            var endMonthDays = DateTime.DaysInMonth(end.Year, end.Month);
            var endBudget = GetBudget(end);
            var endAmount = endBudget
                                ?.Amount ?? 0;


            var diffDays = end.Subtract(start).TotalDays + 1;

            decimal startOneDay = startAmount / startMonthDays;

            if (diffMonth < 2)
            {
                //// 當月超過1日
                return (decimal) (diffDays) * startOneDay;
            }

            var s = (startMonthDays - start.Day + 1) * startOneDay;
            decimal endOneDay = endAmount / endMonthDays;
            var e = end.Day * endOneDay;
            var tmpMid = (decimal) 0;
            for (var i = 1; i < diffMonth - 1; i++)
            {
                var currentBudget = GetBudget(start.AddMonths(i));
                var midAmount = currentBudget
                                    ?.Amount ?? 0;
                tmpMid += midAmount;
            }

            return s + tmpMid + e;

            return 0;
        }

        private Budget_Lab.Budget GetBudget(DateTime start)
        {
            return this._budgetRepo.GetAll()
                .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"));
        }
    }
}