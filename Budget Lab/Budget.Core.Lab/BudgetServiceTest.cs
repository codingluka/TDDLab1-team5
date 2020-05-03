#region

using System;
using System.Linq;
using Budget_Lab;
using NSubstitute;
using Xunit;

#endregion

namespace Budget.Core.Lab
{
    public class BudgetServiceTest
    {
        private readonly IBudgetRepo _budgetRepo;
        private readonly BudgetService _budgetService;

        public BudgetServiceTest()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
            _budgetService = new BudgetService(_budgetRepo);
        }

        [Fact]
        public void end小於start()
        {
            GivenBudgets(new Budget_Lab.Budget {YearMonth = "202004", Amount = 30000});
            QueryAmountShouldBe(0,
                                new DateTime(2020, 4, 01),
                                new DateTime(2020, 3, 01));
        }

        [Fact]
        public void 當天()
        {
            GivenBudgets(new Budget_Lab.Budget {YearMonth = "202004", Amount = 30000});
            QueryAmountShouldBe(1000,
                                new DateTime(2020, 4, 01),
                                new DateTime(2020, 4, 01));
        }

        [Fact]
        public void 當月N天()
        {
            GivenBudgets(new Budget_Lab.Budget {YearMonth = "202004", Amount = 30000});
            QueryAmountShouldBe(10000,
                                new DateTime(2020, 4, 01),
                                new DateTime(2020, 4, 10));
        }

        [Fact]
        public void 跨三月()
        {
            GivenBudgets(
                new Budget_Lab.Budget {YearMonth = "202004", Amount = 30000},
                new Budget_Lab.Budget {YearMonth = "202005", Amount = 3100},
                new Budget_Lab.Budget {YearMonth = "202006", Amount = 300}
            );

            QueryAmountShouldBe(6120,
                                new DateTime(2020, 4, 28),
                                new DateTime(2020, 6, 2));
        }

        [Fact]
        public void 跨兩月()
        {
            GivenBudgets(
                new Budget_Lab.Budget {YearMonth = "202004", Amount = 30000},
                new Budget_Lab.Budget {YearMonth = "202005", Amount = 3100}
            );
            QueryAmountShouldBe(3200,
                                new DateTime(2020, 4, 28),
                                new DateTime(2020, 5, 2));
        }

        [Fact]
        public void invalid_period()
        {
            GivenBudgets(
                new Budget_Lab.Budget {YearMonth = "202004", Amount = 30000}
            );
            QueryAmountShouldBe(0,
                                new DateTime(2020, 5, 28),
                                new DateTime(2020, 5, 2));
        }

        private void QueryAmountShouldBe(decimal expected, DateTime start, DateTime end)
        {
            Assert.Equal(expected, _budgetService.Query(start, end));
        }

        private void GivenBudgets(params Budget_Lab.Budget[] budgets)
        {
            _budgetRepo.GetAll().Returns(budgets.ToList());
        }
    }
}