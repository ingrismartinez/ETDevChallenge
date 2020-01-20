using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Requests
{
    public class ExpenseRequest:UserRequest
    {
        public string ExpenseId { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal ExpendedValue { get; set; }
    }
}
