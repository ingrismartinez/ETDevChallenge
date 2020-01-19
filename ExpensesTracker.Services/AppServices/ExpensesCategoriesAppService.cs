using ExpensesTracker.Services.Data.Entities;
using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Requests;
using ExpensesTracker.Services.Responses;
using ExpensesTracker.Services.Responses.Budget;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.AppServices
{
    public class ExpensesCategoriesAppService
    {
        private readonly ExpensesTrackerContext _context;
        private readonly ExpensesCategoryDomainService _domainService;

        public ExpensesCategoriesAppService(ExpensesTrackerContext context)
        {
            _context = context ?? throw new ArgumentNullException("context");
            _domainService = new ExpensesCategoryDomainService();
        }
        public async Task<NewCategoryResponse> CreateDefaultCategory(ExpenseCategoryRequest request)
        {
            var categories = await _context.ExpenseCategory.Where(c => c.IsDefault == true).ToListAsync();
            var newCategory = ExpensesTrackerFactory.DefaultCategory(request.CategoryName);

            var validation = _domainService.IsValidNewCategory(newCategory, categories);
            if(validation.IsValid())
            {
                _context.ExpenseCategory.Add(newCategory);
                int result = await _context.SaveChangesAsync();
                return new NewCategoryResponse { CategoryId = newCategory.UId };
            }
            return new NewCategoryResponse { ValidationMessage = validation.ValidationErrorMessage };
        }

        internal async Task<ActionResult<NewCategoryResponse>> CreateCustomCategory(ExpenseCategoryRequest request)
        {
            var userId = request.UserId;
            var categories = await _context.ExpenseCategory.Where(c => c.IsDefault || c.OwnerId == userId).ToListAsync();
            var newCategory = ExpensesTrackerFactory.CustomCategory(request.CategoryName, userId);

            var validation = _domainService.IsValidNewCustomCategory(newCategory, categories);
            if (validation.IsValid())
            {
                _context.ExpenseCategory.Add(newCategory);
                int result = await _context.SaveChangesAsync();
                return new NewCategoryResponse { CategoryId = newCategory.UId, UserId = newCategory.OwnerId };
            }
            return new NewCategoryResponse { ValidationMessage = validation.ValidationErrorMessage };
        }

        internal async Task<ActionResult<ResponseBase>> DeleteCustomCategory(int id, string userId)
        {
            var category = await _context.ExpenseCategory.FirstOrDefaultAsync(c => !c.IsDefault && c.OwnerId == userId && c.UId == id);
            var userBudget = await _context.UserBudget.Where(c => c.UserId == userId).ToListAsync();
            var validation = _domainService.IsValidToRemove(category, userBudget);
            if(validation.IsValid())
            {
                _context.ExpenseCategory.Remove(category);
                await _context.SaveChangesAsync();
            }
            return new ResponseBase { ValidationMessage = validation.ValidationErrorMessage };
        }
    }
}
