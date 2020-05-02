using System;
using System.CodeDom;
using System.Collections.Generic;
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

            var period = new Period(start, end);
            return _budgetRepo.GetAll()
                       .Sum(budget => budget.OverlappingAmount(period));
        }
    }
}