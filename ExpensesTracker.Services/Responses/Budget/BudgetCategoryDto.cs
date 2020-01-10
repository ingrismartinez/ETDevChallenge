using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Responses.Budget
{
    public class BudgetCategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Percentaje { get; set; }
    }
}
