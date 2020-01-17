using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensesTracker.Services.AppServices;
using ExpensesTracker.Services.Requests;
using ExpensesTracker.Services.Responses;
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
        
        // POST: api/ExpenseCategories
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // POST: api/ExpenseCategories/add-default
        [HttpPost("add-default")]
        public async Task<ActionResult<ResponseBase>> AddDefault([FromBody] ExpenseCategoryRequest request)
        {
           return await _expensesCategoriesAppService.CreateDefaultCategory(request);
        }
        // POST: api/ExpenseCategories/add-custom
        [HttpPost("add-custom")]
        public async Task<ActionResult<ResponseBase>> AddCustom([FromBody] ExpenseCategoryRequest request)
        {
            return await _expensesCategoriesAppService.CreateCustomCategory(request);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
