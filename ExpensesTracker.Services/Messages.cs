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
        internal static readonly string CategoryCantBeDelete= "Category can't be deleted";
        internal static readonly string SystemDefaultCategoryCantBeDelete="System Categories can't be deleted";
        internal static readonly string BudgedCategoryCantBeDeleted= "Budged Category can't be unliked from budget";
        internal static readonly string CantEditExpense="Expense can't be updated";
        internal static readonly string InvalidDeleteExpense="Invalid delete expense";
        public static string InvalidCategoryName = "Invalid Category Name";
        public static string CantAddDuplicateCategory = "Can't add duplicate category";
        public static string CustomCategoriesMustHaveOwner = "Custom Category with invalid owner";
        public static string CustomCategoriesCantBeAddedAsSystemCategory = "Can't be Default Category";

        public static string BudgetMustContainsExpensesCategories = "Budget missing expenses categories";

        public static string CategoriesNameMustBeUnique = "Category is already created";

        public static string CannotAddExpenses = "Can not add new buy";
    }
}
