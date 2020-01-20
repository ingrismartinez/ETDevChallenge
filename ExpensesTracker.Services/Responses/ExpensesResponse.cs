using System;
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
    public class ExpenseReportDto:ExpenseDto
    {
        public string CategoryName { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal CategoryBudgetAmount { get; set; }
        public decimal CategoryExpendedAmount { get; set; }
        public decimal CategoryPercentage { get; set; }
        public decimal CategoryExpendedPercentage { get; set; }
        public string BudgetName { get; set; }
        public DateTime BudgetStartDate { get; set; }
        public DateTime BudgetEndDate { get; set; }
    }
}
