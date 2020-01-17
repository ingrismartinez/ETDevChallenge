using ExpensesTracker.Services.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Data.Maps
{
    public class MapBudgetDetail : IEntityTypeConfiguration<BudgetDetail>
    {
        public void Configure(EntityTypeBuilder<BudgetDetail> builder)
        {
            builder.ToTable("BudgetDetail", "dbo");
            builder.HasKey(t => t.UId);
            builder.Property(c => c.UId).HasColumnName("UId").IsRequired().ValueGeneratedOnAdd();
            builder.Property(c => c.BudgetId).HasColumnName("BudgetId").IsRequired();
            builder.Property(c => c.CategoryId).HasColumnName("CategoryId").IsRequired();
            builder.Property(c => c.Amount).HasColumnName("Amount").IsRequired();
            builder.Property(c => c.Percentage).HasColumnName("Percentage").IsRequired();
            builder.HasOne(c => c.ExpenseCategory).WithMany(c=>c.BudgetDetails).HasForeignKey(c=>c.CategoryId);
            builder.HasOne(c => c.UserBudget).WithMany(c=>c.BudgetDetails).HasForeignKey(c => c.BudgetId);

        }
    }
}
