using ExpensesTracker.Services.Responses.Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Requests
{
    public class MonthBudgetRequest:UserRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<BudgetCategoryDto> BudgetCategories { get; set; }
    }
}
