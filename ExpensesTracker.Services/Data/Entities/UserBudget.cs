using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Entities
{
    public class UserBudget
    {
        public int UId { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencySign { get; set; }
        public virtual ICollection<BudgetDetail> BudgetDetails { get; set; }

    }
}
