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

        internal UserBudget CreateDefaultBudget(string userId,List<ExpenseCategory> categories)
        {
            var today = DateTime.Today;
            var currentMonth = today.Month;
            var monthBeginningDate = new DateTime(today.Year, currentMonth, 1);
            var monthEndingDate = monthBeginningDate.AddMonths(1).AddDays(-1);

            var budget = new UserBudget
            {
                UserId = userId,
                CurrencySign = "$",
                StartDate = monthBeginningDate,
                EndDate = monthEndingDate,
                Amount = 1000,
            };
            var percentage = 0;
            budget.BudgetDetails = categories.Select(c => new BudgetDetail
            {
                UserBudget = budget,
                CategoryId = c.UId,
                Percentage = percentage,
                Amount = budget.Amount * percentage,
                ExpenseCategory = c,
            }).ToList();

            return budget;
        }

        public DomainValidation IsValidNewUserBudget(UserBudget newBudget)
        {
            if (string.IsNullOrWhiteSpace(newBudget.UserId))
            {
                return new DomainValidation(Messages.BudgetMustHaveAnOwner);
            }
            if (newBudget.BudgetDetails==null || !newBudget.BudgetDetails.Any())
            {
                return new DomainValidation(Messages.BudgetMustContainsExpensesCategories);
            }
            if (newBudget.BudgetDetails.Any(c => c.ExpenseCategory == null))
            {
                return new DomainValidation(Messages.ExpensesCategoriesMustExists);
            }
            if (newBudget.BudgetDetails.Sum(c => c.Amount)<=0)
            {
                return new DomainValidation(Messages.ExpensesPercentMustBeGreaterThanCero);
            }
            if (newBudget.BudgetDetails.Sum(c => c.Percentage) > 1)
            {
                return new DomainValidation(Messages.ExpesesCategoriesExceed100Percent);
            }
            return new DomainValidation();
        }
    }
}
