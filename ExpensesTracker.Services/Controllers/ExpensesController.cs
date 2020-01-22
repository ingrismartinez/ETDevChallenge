using System.Threading.Tasks;
using ExpensesTracker.Services.AppServices;
using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Requests;
using ExpensesTracker.Services.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Services.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpensesTrackerContext _context;
        private readonly UserBudgetsAppService _userBudgetAppService;
        public ExpensesController(ExpensesTrackerContext context)
        {
            _context = context;
            _userBudgetAppService = new UserBudgetsAppService(_context, new UserBudgetsDomainService());
        }
        [HttpPost("add")]
        public async Task<ActionResult<ExpensesResponse>> Post(ExpenseRequest userBudget)
        {
            var result = await _userBudgetAppService.AddNewExpense(userBudget);
            return Ok(result);
        }
        [HttpPut("edit")]
        public async Task<ActionResult<ExpensesResponse>> Put(ExpenseRequest userBudget)
        {
            var result = await _userBudgetAppService.EditExpense(userBudget);
            return Ok(result);
        }
        [HttpDelete("delete")]
        public async Task<ActionResult<ResponseBase>> Delete([FromRoute]ExpenseRequest userBudget)
        {
            var result = await _userBudgetAppService.DeleteExpense(userBudget.ExpenseId,userBudget.UserId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<ExpensesResponse>> Get(string userId,int budgetId,int categoryId)
        {

            var result = await (budgetId > 0 ?
                _userBudgetAppService.GetBudgetExpenses(userId, budgetId) :
                categoryId > 0 ? _userBudgetAppService.GetCategoryExpenses(userId, categoryId) :
                _userBudgetAppService.GetAllUserExpenses(userId));
            return Ok(result);
        }
    }
}