using System;
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

            var startAmount = budgets
                              .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"))
                              ?.Amount ?? 0;
            var startMonthDays = DateTime.DaysInMonth(start.Year, start.Month);
            var startOneDay = (decimal) startAmount / startMonthDays;

            var endAmount = budgets
                            .FirstOrDefault(i => i.YearMonth == end.ToString("yyyyMM"))
                            ?.Amount ?? 0;
            var endMonthDays = DateTime.DaysInMonth(end.Year, end.Month);
            var endOneDay = (decimal) endAmount / endMonthDays;

            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) + 1;

            if (diffMonth < 2)
            {
                //// 當月超過1日
                var intervalDays = (end - start).Days + 1;
                return intervalDays * startOneDay;
            }
            else
            {
                var s = (startMonthDays - start.Day + 1) * startOneDay;
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
            }
        }
    }
}