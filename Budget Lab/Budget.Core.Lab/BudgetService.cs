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
            decimal startDailyAmount = startAmount / startMonthDays;
            var s = (startMonthDays - start.Day + 1) * startDailyAmount;

            var endMonthDays = GetDaysInMonth(end);
            var endBudget = GetBudget(end);
            var endDailyAmount = endBudget?.Amount ?? 0;
            decimal endOneDay = endDailyAmount / endMonthDays;
            var e = end.Day * endOneDay;
            
            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) ;
            if (diffMonth < 1)
            {
                //// 當月超過1日
                var diffDays = end.Subtract(start).TotalDays + 1;
                return (decimal) diffDays * startDailyAmount;
            }

            var tmpMid = 0m;
            for (var i = 1; i < diffMonth; i++)
            {
                var currentMonth = start.AddMonths(i);
                
                var currentMonthDays = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                var currentBudget = GetBudget(currentMonth);
                var midAmount = currentBudget ?.Amount ?? 0;
                var midDailyAmount = midAmount / currentMonthDays;
                tmpMid += currentMonthDays * midDailyAmount ;
            }

            return s + tmpMid + e;
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