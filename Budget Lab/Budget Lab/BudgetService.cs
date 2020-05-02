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
                // var amountOfLastMonth = end.Day * endOneDay;
                var tmpMid = (decimal) 0;

                var startOfMiddleMonth = new DateTime(start.Year, start.Month, 1);
                var currentMonth = startOfMiddleMonth;
                while (currentMonth <= end)
                {
                    var budget = budgets.FirstOrDefault(b => b.YearMonth == currentMonth.ToString("yyyyMM"));
                    var midAmount = 0m;
                    if (budget != null)
                    {
                        if (budget.YearMonth == start.ToString("yyyyMM"))
                        {
                            var amountOfFirstMonth = (startMonthDays - start.Day + 1) * startOneDay;
                            midAmount = amountOfFirstMonth;
                        }
                        else if (budget.YearMonth == end.ToString("yyyyMM"))
                        {
                            var amountOfLastMonth = end.Day * endOneDay;
                            midAmount = amountOfLastMonth;
                        }
                        else
                        {
                            midAmount = budget.Amount;
                        }
                    }

                    tmpMid += midAmount;

                    currentMonth = currentMonth.AddMonths(1);
                }

                return tmpMid;
                // return tmpMid + amountOfLastMonth;
            }
        }
    }
}