using ExpensesTracker.Services.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Data.Maps
{
    public class MapExpenseCategory : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.ToTable("ExpenseCategory", "dbo");
            builder.HasKey(t => t.UId);
            builder.Property(c => c.UId).HasColumnName("UId").IsRequired();
            builder.Property(c => c.Name).HasColumnName("Name").IsRequired();
            builder.Property(c => c.IsDefault).HasColumnName("IsDefault").IsRequired();
        }
    }
}
