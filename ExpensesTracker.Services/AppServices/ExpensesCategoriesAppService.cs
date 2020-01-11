using ExpensesTracker.Services.Data.Entities;
using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Requests;
using ExpensesTracker.Services.Responses;
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
        public async Task<ResponseBase> CreateDefaultCategory(ExpenseCategoryRequest request)
        {
            var categories = await _context.ExpenseCategory.Where(c => c.IsDefault == true).ToListAsync();
            var newCategory = ExpenseCategoryFactory.DefaultCategory(request.CategoryName);

            var validation = _domainService.IsValidNewCategory(newCategory, categories);
            if(validation.IsValid())
            {
                _context.ExpenseCategory.Add(newCategory);
                int result = await _context.SaveChangesAsync();
            }
            return new ResponseBase { ValidationMessage = validation.ValidationErrorMessage };
        }

        internal async Task<ActionResult<ResponseBase>> CreateCustomCategory(ExpenseCategoryRequest request)
        {
            var userId = request.UserId;
            var categories = await _context.ExpenseCategory.Where(c => c.IsDefault == true || c.OwnerId == userId).ToListAsync();
            var newCategory = ExpenseCategoryFactory.CustomCategory(request.CategoryName, userId);

            var validation = _domainService.IsValidNewCustomCategory(newCategory, categories);
            if (validation.IsValid())
            {
                _context.ExpenseCategory.Add(newCategory);
                int result = await _context.SaveChangesAsync();
            }
            return new ResponseBase { ValidationMessage = validation.ValidationErrorMessage };
        }
    }
}
