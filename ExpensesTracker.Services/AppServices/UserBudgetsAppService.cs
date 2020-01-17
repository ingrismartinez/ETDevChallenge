﻿using ExpensesTracker.Services.Data.Entities;
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

        public async Task<MonthBudgetResponse> GetMyCurrentMonthBudget(UserRequest request)
        {
            var userId = request.UserId;
            DateTime monthBeginningDate;
            DateTime monthEndingDate;

            _budgetDomainService.FindCurrentMonthDates(out monthBeginningDate, out monthEndingDate);

            var userBudgets = await _context.UserBudget.Where(c => c.UserId == userId).Include(c=>nameof(UserBudget.BudgetDetails)).ToListAsync();
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
            return MapUserBudgetToResponse(newBudget, false, validation);
        }

        private static MonthBudgetResponse MapUserBudgetToResponse(UserBudget currentMonth, bool defaultTemplate,DomainValidation validation =null)
        {
            return new MonthBudgetResponse
            {
                IsProposedBudget = defaultTemplate,
                Budget = new Responses.BudgetDto
                {
                    Amount = currentMonth.Amount,
                    Currency = currentMonth.CurrencySign,
                    Year = currentMonth.EndDate.Year,
                    Name = currentMonth.StartDate.MonthName(),
                    BudgetCategory = MapBudgetCategories(currentMonth)
                }
            };
        }

        private static IEnumerable<BudgetCategoryDto> MapBudgetCategories(UserBudget currentMonth)
        {
            return currentMonth.BudgetDetails?.Select(c => new BudgetCategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.ExpenseCategory?.Name,
                Percentage= c.Percentage,
            });
        }
    }
}
