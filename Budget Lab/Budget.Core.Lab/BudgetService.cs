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

            var startMonthDays = GetDaysInMonth(start);
            var startBudget = GetBudget(start);
            var startAmount = startBudget?.Amount ?? 0;
            decimal startDailyAmount = startAmount / startMonthDays;
            // var s = (startMonthDays - start.Day + 1) * startDailyAmount;

            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month);
            // if (diffMonth < 1)
            // {
            //     //// 當月超過1日
            //     var diffDays = end.Subtract(start).TotalDays + 1;
            //     return (decimal) diffDays * startDailyAmount;
            // }

            var result = 0m;
            for (var i = 0; i < diffMonth + 1; i++)
            {
                var currentMonth = start.AddMonths(i);

                int currentMonthDays = GetDaysInMonth(currentMonth);
                int currentOneDay = 0;
                if (diffMonth < 1)
                {
                    currentOneDay = (int) (end.Subtract(start).TotalDays + 1);
                }
                else if (i == 0)
                {
                    currentOneDay = startMonthDays - start.Day + 1;
                }
                else if (i == diffMonth)
                {
                    currentOneDay = end.Day;
                }
                else
                {
                    currentOneDay = GetDaysInMonth(currentMonth);
                }

                var currentBudget = GetBudget(currentMonth);
                var midAmount = currentBudget?.Amount ?? 0;
                var midDailyAmount = midAmount / currentMonthDays;
                result += currentOneDay * midDailyAmount;
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