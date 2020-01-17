using ExpensesTracker.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.DomainServices
{
    public class ExpensesCategoryDomainService
    {
        public DomainValidation IsValidNewCategory(ExpenseCategory newCategory,IEnumerable<ExpenseCategory> systemCategories)
        {
            if(string.IsNullOrWhiteSpace(newCategory.Name))
            {
                return new DomainValidation(Messages.InvalidCategoryName);
            }
            newCategory.Name = newCategory.Name.Trim();
            var existing = systemCategories?.FirstOrDefault(c => c.Name.ToUpper() == newCategory.Name.ToUpper()) != null;
            if(existing)
            {
                return new DomainValidation(Messages.CantAddDuplicateCategory);
            }
            return new DomainValidation();

        }

        public DomainValidation IsValidNewCustomCategory(ExpenseCategory newCategory, List<ExpenseCategory> categories)
        {
            var validation = IsValidNewCategory(newCategory, categories);
            if(validation.IsValid())
            {
                if(string.IsNullOrEmpty(newCategory.OwnerId))
                {
                    return new DomainValidation(Messages.CustomCategoriesMustHaveOwner);
                }
                if(newCategory.IsDefault)
                {
                    return new DomainValidation(Messages.CustomCategoriesCantBeAddedAsSystemCategory);

                }

            }
            return new DomainValidation();
        }
    }
}
