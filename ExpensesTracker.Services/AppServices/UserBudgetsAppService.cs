using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Entities;
using ExpensesTracker.Services.Requests;
using ExpensesTracker.Services.Responses.Budget;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.AppServices
{
    public class UserBudgetsAppService
    {
        private readonly ExpensesTrackerContext _context;
        private readonly UserBudgetsDomainService _budgetDomainService;

        public UserBudgetsAppService(ExpensesTrackerContext context,
            UserBudgetsDomainService budgetDomainService)
        {
            _context = context ?? throw new ArgumentNullException("context");
            _budgetDomainService = budgetDomainService;

        }
        public async Task<MonthBudgetResponse> GetMyCurrentMonthBudget(MonthBudgetRequest request)
        {
            var userId = request.UserId;
            DateTime monthBeginningDate;
            DateTime monthEndingDate;

            _budgetDomainService.FindCurrentMonthDates(out monthBeginningDate, out monthEndingDate);

            var userBudgets = await _context.UserBudget.Where(c => c.UserId == userId).ToListAsync();
            var currentMonth = userBudgets.FirstOrDefault(c =>
                           c.StartDate >= monthBeginningDate && c.EndDate <= monthEndingDate);

            var defaultTemplate = currentMonth == null;
            if (defaultTemplate)
            {
                currentMonth = _budgetDomainService.CreateDefaultBudget(request.UserId);
            }


            return new MonthBudgetResponse
            {
                IsProposedBudget = defaultTemplate,
                Budget = new Responses.BudgetDto
                {
                    Amount = currentMonth.Amount,
                    Currency = currentMonth.CurrencySign,
                    Year = currentMonth.EndDate.Year,
                    Name = currentMonth.StartDate.Month.ToString(),
                    BudgetCategory = MapBudgetCategories(currentMonth)
                }
            };


        }

        private static IEnumerable<BudgetCategoryDto> MapBudgetCategories(UserBudget currentMonth)
        {
            return currentMonth.BudgetDetails.Select(c => new BudgetCategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.ExpenseCategory.Name,
                Percentaje= c.Percentage,
            });
        }
    }
}
