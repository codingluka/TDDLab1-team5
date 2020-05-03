﻿using System;
using System.CodeDom;
using System.Linq;

namespace Budget_Lab
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

            var budgets = this._budgetRepo.GetAll();
            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) + 1;
            var startMonthDays = DateTime.DaysInMonth(start.Year, start.Month);
            var startBudget = budgets
                .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"));
            var startAmount = startBudget
                                  ?.Amount ?? 0;
            var endMonthDays = DateTime.DaysInMonth(end.Year, end.Month);
            var endBudget = budgets
                .FirstOrDefault(i => i.YearMonth == end.ToString("yyyyMM"));
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
                var midAmount = budgets
                    .FirstOrDefault(j => j.YearMonth == start.AddMonths(i).ToString("yyyyMM"))
                    ?.Amount ?? 0;
                tmpMid += midAmount;
            }

            return s + tmpMid + e;

            return 0;
        }
    }
}