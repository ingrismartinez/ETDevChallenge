using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Responses.Budget
{
    public class BudgetCategoryDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public List<Expense> Expenses { get; set; }
    }
    public class Expense
    {
        public decimal Value { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }

}
