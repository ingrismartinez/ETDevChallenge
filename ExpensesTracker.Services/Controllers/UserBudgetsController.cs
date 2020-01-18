using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpensesTracker.Services.Entities;
using ExpensesTracker.Services.AppServices;
using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Requests;
using ExpensesTracker.Services.Responses.Budget;

namespace ExpensesTracker.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBudgetsController : ControllerBase
    {
        private readonly ExpensesTrackerContext _context;
        private readonly UserBudgetsAppService _userBudgetAppService;

        public UserBudgetsController(ExpensesTrackerContext context)
        {
            _context = context;
            _userBudgetAppService = new UserBudgetsAppService(_context, new UserBudgetsDomainService());
        }

        // GET: api/UserBudgets
        [HttpGet]
        public async Task<ActionResult<MonthBudgetResponse>> GetUserBudget(string userId)
        {
            return Ok(await _userBudgetAppService.GetMyCurrentMonthBudget(new UserRequest { UserId = userId }));
        }

        // GET: api/UserBudgets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserBudget>> GetUserBudget(int id)
        {
            var userBudget = await _context.UserBudget.FindAsync(id);

            if (userBudget == null)
            {
                return NotFound();
            }

            return userBudget;
        }

        // POST: api/UserBudgets/add-budget
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("add-budget")]
        public async Task<ActionResult<MonthBudgetResponse>> PostUserBudget(MonthBudgetRequest userBudget)
        {
            var result = await _userBudgetAppService.AddNewUserBudget(userBudget);
            return Ok(result);
        }

        // DELETE: api/UserBudgets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserBudget>> DeleteUserBudget(int id)
        {
            var userBudget = await _context.UserBudget.FindAsync(id);
            if (userBudget == null)
            {
                return NotFound();
            }

            _context.UserBudget.Remove(userBudget);
            await _context.SaveChangesAsync();

            return userBudget;
        }

    }
}
