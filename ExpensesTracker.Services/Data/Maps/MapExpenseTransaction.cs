using ExpensesTracker.Services.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Data.Maps
{
    public class MapExpenseTransaction : IEntityTypeConfiguration<ExpenseTransaction>
    {
        public void Configure(EntityTypeBuilder<ExpenseTransaction> builder)
        {
            builder.ToTable("ExpenseTracking", "dbo");
            builder.HasKey(t => t.UId);
            builder.Property(c => c.UId).HasColumnName("UId").IsRequired().ValueGeneratedOnAdd();
            builder.Property(c => c.BudgetCategoryId).HasColumnName("BudgeCategoryId").IsRequired();
            builder.Property(c => c.Description).HasColumnName("Description").IsRequired();
            builder.Property(c => c.Value).HasColumnName("Value").IsRequired();
            builder.Property(c => c.TransactionDate).HasColumnName("TransactionDate").IsRequired();
            builder.HasOne(c => c.BudgetCategory).WithMany(c => c.Expenses).HasForeignKey(c => c.BudgetCategoryId);

        }
    }
}
