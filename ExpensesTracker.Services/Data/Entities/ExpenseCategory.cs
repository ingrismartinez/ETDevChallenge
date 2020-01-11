using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Entities
{
    public class ExpenseCategory
    {
        public int UId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string OwnerId { get; set; }
    }
}
