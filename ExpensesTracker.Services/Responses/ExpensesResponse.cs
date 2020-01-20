﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Responses
{
    public class ExpensesResponse:ResponseBase
    {
        public List<ExpenseDto> Expenses { get; set; }
    }
    public class ExpenseDto
    {
        public string BudgetId { get; set; }
        public string CategoryId { get; set; }
        public string ExpenseId { get; set; }
        public string Description { get; set; }
        public decimal ExpendedValue { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
