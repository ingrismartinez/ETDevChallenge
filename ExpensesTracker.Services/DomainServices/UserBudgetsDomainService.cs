using ExpensesTracker.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.DomainServices
{
    public class UserBudgetsDomainService
    {
        internal void FindCurrentMonthDates(out DateTime monthBeginningDate, out DateTime monthEndingDate)
        {
            var today = DateTime.Today;
            var currentMonth = today.Month;
            monthBeginningDate = new DateTime(today.Year, currentMonth, 1);
            monthEndingDate = monthBeginningDate.AddMonths(1).AddDays(-1);
        }

        internal UserBudget CreateDefaultBudget(string userId)
        {
            var today = DateTime.Today;
            var currentMonth = today.Month;
            var monthBeginningDate = new DateTime(today.Year, currentMonth, 1);
            var monthEndingDate = monthBeginningDate.AddMonths(1).AddDays(-1);

            return new UserBudget
            {
                UserId = userId,
                CurrencySign = "$",
                StartDate = monthBeginningDate,
                EndDate = monthEndingDate,
                Amount = 100,
                BudgetDetails = new List<BudgetDetail>()
                {
                    new BudgetDetail
                    {

                    }
                }
            };
        }
    }
}
