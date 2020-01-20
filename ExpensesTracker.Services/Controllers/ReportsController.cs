using ExpensesTracker.Services.AppServices;
using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController: ControllerBase
    {
        private readonly ExpensesTrackerContext _context;
        private readonly UserBudgetsAppService _userBudgetAppService;
        public ReportsController(ExpensesTrackerContext context)
        {
            _context = context;
            _userBudgetAppService = new UserBudgetsAppService(_context, new UserBudgetsDomainService());
        }
        [HttpGet()]
        public async Task<ActionResult<List<ExpenseReportDto>>> Get(string userId,DateTime startDate,DateTime endDate)
        {
            var result = await _userBudgetAppService.GetExpensesReport(userId, startDate, endDate);
            return Ok(result);
        }
    }
}
