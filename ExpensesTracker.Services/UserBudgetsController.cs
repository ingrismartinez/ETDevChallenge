using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpensesTracker.Services.Entities;

namespace ExpensesTracker.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBudgetsController : ControllerBase
    {
        private readonly ExpensesTrackerContext _context;

        public UserBudgetsController(ExpensesTrackerContext context)
        {
            _context = context;
        }

        // GET: api/UserBudgets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBudget>>> GetUserBudget()
        {
            return await _context.UserBudget.ToListAsync();
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

        // PUT: api/UserBudgets/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserBudget(int id, UserBudget userBudget)
        {
            if (id != userBudget.UId)
            {
                return BadRequest();
            }

            _context.Entry(userBudget).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBudgetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserBudgets
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserBudget>> PostUserBudget(UserBudget userBudget)
        {
            _context.UserBudget.Add(userBudget);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserBudget", new { id = userBudget.UId }, userBudget);
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

        private bool UserBudgetExists(int id)
        {
            return _context.UserBudget.Any(e => e.UId == id);
        }
    }
}
