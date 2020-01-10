using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Responses.Budget
{
    public class MonthBudgetResponse
    {
        public bool IsProposedBudget { get; set; }
        public BudgetDto Budget { get; set; }
    }
}
