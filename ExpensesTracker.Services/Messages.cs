using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services
{
    public static class Messages
    {
        public static readonly string BudgetMustHaveAnOwner="Budget is missing an owner";
        public static readonly string ExpensesCategoriesMustExists="Categories must exists";
        public static readonly string ExpesesCategoriesExceed100Percent="Expenses Can't exceed 100 %";
        public static readonly string ExpensesPercentMustBeGreaterThanCero="Expenses can't be 0 %";
        public static string InvalidCategoryName = "Invalid Category Name";
        public static string CantAddDuplicateCategory = "Can't add duplicate category";
        public static string CustomCategoriesMustHaveOwner = "Custom Category with invalid owner";
        public static string CustomCategoriesCantBeAddedAsSystemCategory = "Can't be Default Category";

        public static string BudgetMustContainsExpensesCategories = "Budget missing expenses categories";
    }
}
