using ExpensesTracker.Services.Responses.Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Responses
{
    public class BudgetDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<BudgetCategoryDto> BudgetCategory { get; set; }
    }
}
