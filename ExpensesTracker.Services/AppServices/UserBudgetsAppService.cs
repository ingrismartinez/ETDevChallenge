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

        internal async Task<List<ExpenseReportDto>> GetExpensesReport(string userId, DateTime startDate, DateTime endDate)
        {
            var byDateRange = startDate != DateTime.MinValue && endDate!= DateTime.MinValue && startDate != endDate;

            var expensesquery = _context.Expenses.Include(c => c.BudgetCategory).ThenInclude(c => c.ExpenseCategory)
                .Include(c => c.BudgetCategory).ThenInclude(c => c.UserBudget)
                .Where(c => c.BudgetCategory.UserBudget.UserId == userId);

            if(byDateRange)
            {
                expensesquery.Where(c => c.BudgetCategory.UserBudget.StartDate >= startDate && c.BudgetCategory.UserBudget.EndDate <= endDate);
            }
              var expenses = await expensesquery.ToListAsync();

            return MapExpenseReport(expenses);
        }
        
        public async Task<MonthBudgetResponse> GetMyCurrentMonthBudget(UserRequest request)
        {
            var userId = request.UserId;
            DateTime monthBeginningDate;
            DateTime monthEndingDate;

            _budgetDomainService.FindCurrentMonthDates(out monthBeginningDate, out monthEndingDate);

            var userBudgets = await _context.BudgetDetail.Include(c=>c.UserBudget).Include(c=>c.Expenses).Include(c=>c.ExpenseCategory).Where(c => c.UserBudget.UserId == userId)
                
                .ToListAsync();
            var currentMonth = userBudgets.FirstOrDefault(c =>
                           c.UserBudget.StartDate >= monthBeginningDate && c.UserBudget.EndDate <= monthEndingDate).UserBudget;
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
            var budgetCategory = await
                _context.BudgetDetail.Include(c => c.UserBudget).FirstOrDefaultAsync(c => c.UId == budgetCategoryId && c.UserBudget.UserId == userId);
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
                Expenses = new List<Responses.ExpenseDto>
                { ExpenseToDto(newExpense)
                }
            };
        }

        private static Responses.ExpenseDto ExpenseToDto(ExpenseTransaction newExpense)
        {
            return new Responses.ExpenseDto
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

        private List<ExpenseReportDto> MapExpenseReport(List<ExpenseTransaction> expenses)
        {
            return expenses.Select(c => new ExpenseReportDto
            {
                BudgetAmount = c.BudgetCategory.UserBudget.Amount,
                BudgetEndDate = c.BudgetCategory.UserBudget.EndDate,
                BudgetStartDate = c.BudgetCategory.UserBudget.StartDate,
                BudgetId = c.BudgetCategory.UserBudget.UId.ToString(),
                BudgetName = c.BudgetCategory.UserBudget.StartDate.MonthName(),
                CategoryBudgetAmount = c.BudgetCategory.Amount,
                CategoryPercentage = c.BudgetCategory.Percentage * 100,
                CategoryId = c.BudgetCategoryId.ToString(),
                CategoryName = c.BudgetCategory.ExpenseCategory.Name,
                ExpenseId = c.UId.ToString(),
                Description = c.Description,
                ExpendedValue = c.Value,
                TransactionDate = c.TransactionDate,
                CategoryExpendedAmount = c.BudgetCategory.Expenses.Sum(c => c.Value),
                CategoryExpendedPercentage = c.BudgetCategory.Amount > 0 ? c.BudgetCategory.Expenses.Sum(c => c.Value) / c.BudgetCategory.Amount : 0

            }).ToList();
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
                Id = c.UId,
                Name = c.ExpenseCategory?.Name,
                Percentage= c.Percentage,
                Amount = c.Amount,
                Expenses = c.Expenses?.Select(c=>new Expense{ Description = c.Description,TransactionDate = c.TransactionDate,Value = c.Value}).ToList(),
            });
        }
    }
}
