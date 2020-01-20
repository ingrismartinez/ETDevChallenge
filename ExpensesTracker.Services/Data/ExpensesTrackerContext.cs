using Microsoft.EntityFrameworkCore;
using ExpensesTracker.Services.Entities;
using ExpensesTracker.Services.Data.Maps;

public class ExpensesTrackerContext : DbContext
{
    public ExpensesTrackerContext(DbContextOptions<ExpensesTrackerContext> options)
        : base(options)
    {
    }

    public DbSet<UserBudget> UserBudget { get; set; }
    public DbSet<BudgetDetail> BudgetDetail { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategory { get; set; }
    public DbSet<ExpenseTransaction> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Property Configurations
        modelBuilder.ApplyConfiguration(new MapUserBudget());
        modelBuilder.ApplyConfiguration(new MapBudgetDetail());
        modelBuilder.ApplyConfiguration(new MapExpenseCategory());
        modelBuilder.ApplyConfiguration(new MapExpenseTransaction());
    }
}
