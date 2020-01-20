using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Entities
{
    public class ExpenseTransaction
    {
        public BudgetDetail BudgetCategory { get; set; }
        public int BudgetCategoryId { get; set; }
        public int UId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
