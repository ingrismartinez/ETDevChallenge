using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpensesTracker.Services.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExpensesTrackerContext : DbContext
{
    public ExpensesTrackerContext(DbContextOptions<ExpensesTrackerContext> options)
        : base(options)
    {
    }

    public DbSet<ExpensesTracker.Services.Entities.UserBudget> UserBudget { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Write Fluent API configurations here

        //Property Configurations
        modelBuilder.ApplyConfiguration(new MapUserBudget());
    }
}
