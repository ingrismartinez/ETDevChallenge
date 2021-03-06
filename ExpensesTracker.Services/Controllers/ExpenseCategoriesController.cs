﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensesTracker.Services.AppServices;
using ExpensesTracker.Services.Requests;
using ExpensesTracker.Services.Responses;
using ExpensesTracker.Services.Responses.Budget;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoriesController : ControllerBase
    {

        private readonly ExpensesTrackerContext _context;
        private readonly ExpensesCategoriesAppService _expensesCategoriesAppService;

        public ExpenseCategoriesController(ExpensesTrackerContext context)
        {
            _context = context;
            _expensesCategoriesAppService = new ExpensesCategoriesAppService(_context);

        }
        
        // POST: api/ExpenseCategories/add-default
        [HttpPost("add-default")]
        public async Task<ActionResult<NewCategoryResponse>> AddDefault([FromBody] ExpenseCategoryRequest request)
        {
           return await _expensesCategoriesAppService.CreateDefaultCategory(request);
        }
        // POST: api/ExpenseCategories/add-custom
        [HttpPost("add-custom")]
        public async Task<ActionResult<NewCategoryResponse>> AddCustom([FromBody] ExpenseCategoryRequest request)
        {
            return await _expensesCategoriesAppService.CreateCustomCategory(request);
        }

        // DELETE: api/delete-custom/5
        [HttpDelete("{id}/{userId}")]
        public async Task<ActionResult<ResponseBase>> Delete(int id,string userId)
        {
            return await _expensesCategoriesAppService.DeleteCustomCategory(id,userId);
        }
    }
}
