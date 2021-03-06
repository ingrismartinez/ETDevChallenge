﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Entities
{
    public class BudgetDetail
    {
        public int UId { get; set; }
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public virtual ExpenseCategory ExpenseCategory { get; set; }
        public virtual UserBudget UserBudget { get; set; }
        public virtual ICollection<ExpenseTransaction> Expenses { get; set; }

        internal void AddExpense(ExpenseTransaction newExpense)
        {
            if(Expenses==null)
            {
                Expenses = new List<ExpenseTransaction>();
            }
            Expenses.Add(newExpense);
        }
    }
}
