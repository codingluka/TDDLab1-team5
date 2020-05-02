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

            if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
            {
                //// 當月超過1日
                var intervalDays = (end - start).Days + 1;
                return intervalDays * startOneDay;
            }
            else
            {
                var tmpMid = (decimal) 0;

                var currentMonth = new DateTime(start.Year, start.Month, 1);
                while (currentMonth <= end)
                {
                    var currentBudget = budgets.FirstOrDefault(b => b.YearMonth == currentMonth.ToString("yyyyMM"));
                    if (currentBudget != null)
                    {
                        DateTime overlappingEnd;
                        DateTime overlappingStart;
                        if (currentBudget.YearMonth == start.ToString("yyyyMM"))
                        {
                            overlappingEnd = currentBudget.LastDay();
                            overlappingStart = start;
                        }
                        else if (currentBudget.YearMonth == end.ToString("yyyyMM"))
                        {
                            overlappingEnd = end;
                            overlappingStart = currentBudget.FirstDay();
                        }
                        else
                        {
                            overlappingEnd = currentBudget.LastDay();
                            overlappingStart = currentBudget.FirstDay();
                        }

                        var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
                        tmpMid += overlappingDays * currentBudget.DailyAmount();
                    }

                    currentMonth = currentMonth.AddMonths(1);
                }

                return tmpMid;
            }
        }
    }
}