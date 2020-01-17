using ExpensesTracker.Services.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MapUserBudget : IEntityTypeConfiguration<UserBudget>
{
    public void Configure(EntityTypeBuilder<UserBudget> builder)
    {
        builder.ToTable("Budget", "dbo");
        builder.HasKey(t => t.UId);
        builder.Property(c => c.UId).HasColumnName("UId").IsRequired().ValueGeneratedOnAdd();
        builder.Property(c => c.StartDate).HasColumnName("StartDate").IsRequired();
        builder.Property(c => c.EndDate).HasColumnName("EndDate").IsRequired();
        builder.Property(c => c.Amount).HasColumnName("Amount").IsRequired();
        builder.Property(c => c.CurrencySign).HasColumnName("Currency").IsRequired();
        builder.Property(c => c.UserId).HasColumnName("IdOwner").IsRequired();
        builder.HasMany(c => c.BudgetDetails).WithOne(c=>c.UserBudget);
    }
}