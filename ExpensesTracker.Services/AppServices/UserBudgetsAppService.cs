using ExpensesTracker.Services.Data.Entities;
using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Entities;
using ExpensesTracker.Services.Requests;
using ExpensesTracker.Services.Responses;
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

        public async Task<MonthBudgetResponse> GetMyCurrentMonthBudget(UserRequest request)
        {
            var userId = request.UserId;
            DateTime monthBeginningDate;
            DateTime monthEndingDate;

            _budgetDomainService.FindCurrentMonthDates(out monthBeginningDate, out monthEndingDate);

            var userBudgets = await _context.UserBudget.Include(c=>c.BudgetDetails).Where(c => c.UserId == userId)
                
                .ToListAsync();
            var currentMonth = userBudgets.FirstOrDefault(c =>
                           c.StartDate >= monthBeginningDate && c.EndDate <= monthEndingDate);
            var categories = await _context.ExpenseCategory.Where(c => c.IsDefault || c.OwnerId == userId).ToListAsync();

            var defaultTemplate = currentMonth == null;
            if (defaultTemplate)
            {
                currentMonth = _budgetDomainService.CreateDefaultBudget(request.UserId,categories);
            }

            return MapUserBudgetToResponse(currentMonth, defaultTemplate);

        }

        internal async Task<ExpensesResponse> GetAllUserExpenses(string userId)
        {
            var expenses = await
                _context.Expenses.Include(c => c.BudgetCategory).ThenInclude(c => c.UserBudget)
                .Where(c => c.BudgetCategory.UserBudget.UserId == userId ).ToListAsync();

            return MapExpense(expenses);
        }

        internal async Task<ExpensesResponse> GetCategoryExpenses(string userId, int categoryId)
        {
            var expenses = await
                _context.Expenses.Include(c => c.BudgetCategory).ThenInclude(c => c.UserBudget)
                .Where(c => c.BudgetCategory.UserBudget.UserId == userId && c.BudgetCategory.UId == categoryId).ToListAsync();

            return MapExpense(expenses);
        }

        internal async Task<ExpensesResponse> GetBudgetExpenses(string userId, int budgetIdValue)
        {
            var budgetId = Convert.ToInt32(budgetIdValue);
            var expenses = await
                _context.Expenses.Include(c => c.BudgetCategory).ThenInclude(c => c.UserBudget)
                .Where(c => c.BudgetCategory.UserBudget.UserId == userId && c.BudgetCategory.UserBudget.UId == budgetIdValue).ToListAsync();

            return MapExpense(expenses);

        }

        internal async Task<ResponseBase> DeleteExpense(string expenseIdValue, string userId)
        {
            var expenseId = Convert.ToInt32(expenseIdValue);
            var expense =
                _context.Expenses.Include(c => c.BudgetCategory).ThenInclude(c => c.UserBudget)
                .FirstOrDefault(c => c.UId == expenseId && c.BudgetCategory.UserBudget.UserId == userId);
            
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
                return new ResponseBase();
            }
            else
            {
                return new ResponseBase { ValidationMessage = Messages.InvalidDeleteExpense };
            }
        }

        internal async Task<ExpensesResponse> EditExpense(ExpenseRequest userBudget)
        {
            var userId = userBudget.UserId;
            var expenseId = Convert.ToInt32(userBudget.ExpenseId);
            var expense =
                _context.Expenses.Include(c => c.BudgetCategory).ThenInclude(c=>c.UserBudget)
                .FirstOrDefault(c => c.UId == expenseId && c.BudgetCategory.UserBudget.UserId == userId);
            if (expense != null)
            {
                expense.Description = userBudget.Description;
                expense.TransactionDate = userBudget.TransactionDate;
                expense.Value = userBudget.ExpendedValue;
                _context.Expenses.Update(expense);
                await _context.SaveChangesAsync();
                return MapExpense(expense);
            }
            return new ExpensesResponse { ValidationMessage = Messages.CantEditExpense };
        }

        internal async Task<ExpensesResponse> AddNewExpense(ExpenseRequest userBudget)
        {
            var userId = userBudget.UserId;
            var budgetCategoryId = Convert.ToInt32(userBudget.CategoryId);
            var budgetCategory =
                _context.BudgetDetail.Include(c => c.UserBudget).FirstOrDefault(c => c.UId == budgetCategoryId && c.UserBudget.UserId == userId);
            if (budgetCategory != null)
            {
                var newExpense = 
                    ExpensesTrackerFactory.CreateNewExpense(budgetCategory, userBudget.Description, userBudget.ExpendedValue, userBudget.TransactionDate);

                _context.Expenses.Add(newExpense);
                await _context.SaveChangesAsync();
                return MapExpense(newExpense);
            }else
            {
                return new ExpensesResponse { ValidationMessage = Messages.CannotAddExpenses };
            }
        }

        private ExpensesResponse MapExpense(ExpenseTransaction newExpense)
        {
            return new ExpensesResponse
            {
                Expenses = new List<ExpenseDto>
                { ExpenseToDto(newExpense)
                }
            };
        }

        private static ExpenseDto ExpenseToDto(ExpenseTransaction newExpense)
        {
            return new ExpenseDto
            {
                ExpenseId = newExpense.UId.ToString(),
                CategoryId = newExpense.BudgetCategoryId.ToString(),
                Description = newExpense.Description,
                ExpendedValue = newExpense.Value,
                TransactionDate = newExpense.TransactionDate,
                BudgetId = newExpense.BudgetCategory?.BudgetId.ToString()
            };
        }

        private ExpensesResponse MapExpense(List<ExpenseTransaction> expenses)
        {
            return new ExpensesResponse
            {
                Expenses = expenses.Select(newExpense => ExpenseToDto(newExpense)).ToList()

            };
        }

        public async Task<MonthBudgetResponse> AddNewUserBudget(MonthBudgetRequest request)
        {

            var categories = await _context.ExpenseCategory.Where(c => c.IsDefault || c.OwnerId == request.UserId).ToListAsync();
            var newBudget = ExpensesTrackerFactory.CreateNewBudget(request, categories);

            var validation = _budgetDomainService.IsValidNewUserBudget(newBudget);
            
            if(validation.IsValid())
            {
                _context.UserBudget.Add(newBudget);
                await _context.SaveChangesAsync();
            }
            return MapUserBudgetToResponse(newBudget, !validation.IsValid(), validation);
        }

        private static MonthBudgetResponse MapUserBudgetToResponse(UserBudget currentMonth, bool defaultTemplate,DomainValidation validation =null)
        {
            return new MonthBudgetResponse
            {
                IsProposedBudget = defaultTemplate,
                
                Budget = new Responses.BudgetDto
                {
                    UserId = currentMonth.UserId,
                    Amount = currentMonth.Amount,
                    Currency = currentMonth.CurrencySign,
                    Year = currentMonth.EndDate.Year,
                    Name = currentMonth.StartDate.MonthName(),
                    StartDate= currentMonth.StartDate,
                    EndDate =currentMonth.EndDate,
                    BudgetCategory = MapBudgetCategories(currentMonth)
                },
                ValidationMessage = validation?.ValidationErrorMessage
            };
        }

        private static IEnumerable<BudgetCategoryDto> MapBudgetCategories(UserBudget currentMonth)
        {
            return currentMonth.BudgetDetails?.Select(c => new BudgetCategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.ExpenseCategory?.Name,
                Percentage= c.Percentage,
                Amount = c.Amount,
            });
        }
    }
}
