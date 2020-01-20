using ExpensesTracker.Services.Entities;
using ExpensesTracker.Services.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Data.Entities
{
    public static class ExpensesTrackerFactory
    {
        public static ExpenseCategory DefaultCategory(string categoryName)
        {
            return new ExpenseCategory { IsDefault = true, Name = categoryName };
        }

        public static ExpenseCategory CustomCategory(string categoryName, string userId)
        {
            return new ExpenseCategory { IsDefault = false, Name = categoryName, OwnerId = userId };
        }
        public static string MonthName(this DateTime date)
        {
            return $"{date.ToString("MMMM")} {date.Year}";
        }

        public static UserBudget CreateNewBudget(MonthBudgetRequest request,IEnumerable<ExpenseCategory> categories)
        {
            var budget = new UserBudget
            {
                UserId = request.UserId,
                CurrencySign = request.Currency??"$",
                StartDate= request.StartDate,
                EndDate = request.EndDate,
                Amount = request.Amount,
                
            };
            budget.BudgetDetails = request.BudgetCategories?.Select(c => new BudgetDetail
            {
                CategoryId = c.CategoryId,
                UserBudget = budget,
                ExpenseCategory = categories?.FirstOrDefault(d => d.UId == c.CategoryId),
                Percentage = c.Percentage / 100,
                Amount = request.Amount * (c.Percentage / 100)
            }).ToList();

            return budget;
        }

        internal static ExpenseTransaction CreateNewExpense(BudgetDetail budgetCategory, string description, decimal expendedValue, DateTime transactionDate)
        {
            var newExpense = new ExpenseTransaction
            {
                BudgetCategoryId = budgetCategory.UId,
                BudgetCategory = budgetCategory,
                Description = description,
                Value = expendedValue,
                TransactionDate = transactionDate,
            };
            budgetCategory.AddExpense(newExpense);
            return newExpense;
        }
    }
}
