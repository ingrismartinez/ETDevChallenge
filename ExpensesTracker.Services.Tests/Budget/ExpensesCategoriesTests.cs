using ExpensesTracker.Services.Data.Entities;
using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesTracker.Services.Tests.Budget
{
    [TestClass]
    public class ExpensesCategoriesTests
    {
        private readonly ExpensesCategoryDomainService _DomainService;
        public ExpensesCategoriesTests()
        {
            _DomainService = new ExpensesCategoryDomainService();
        }
        [TestMethod]
        [TestCategory("UnitTest")]
        public void EmptyCategoryNameNotValid()
        {
            var newCategory = ExpenseCategoryFactory.DefaultCategory(string.Empty);
            var validation = _DomainService.IsValidNewCategory(newCategory, null);

            Assert.IsFalse(validation.IsValid());
            Assert.AreEqual(Messages.InvalidCategoryName,validation.ValidationErrorMessage);
        }
        [TestMethod]
        [TestCategory("UnitTest")]
        public void CantAddDuplicateMeaning()
        {
            var newCategory = ExpenseCategoryFactory.DefaultCategory("Coffee");
            var systemCategoies = new List<ExpenseCategory>() { new ExpenseCategory { Name = "COFFEE" } };
            var validation = _DomainService.IsValidNewCategory(newCategory, systemCategoies);

            Assert.IsFalse(validation.IsValid());
            Assert.AreEqual(Messages.CantAddDuplicateCategory, validation.ValidationErrorMessage);
        }
        [TestMethod]
        [TestCategory("UnitTest")]
        public void CantAddDuplicateMissingTrimName()
        {
            var newCategory = ExpenseCategoryFactory.DefaultCategory(" Coffee ");
            var systemCategoies = new List<ExpenseCategory>() { new ExpenseCategory { Name = "COFFEE" } };
            var validation = _DomainService.IsValidNewCategory(newCategory, systemCategoies);

            Assert.IsFalse(validation.IsValid());
            Assert.AreEqual(Messages.CantAddDuplicateCategory, validation.ValidationErrorMessage);
        }
        [TestMethod]
        [TestCategory("UnitTest")]
        public void CustomCategoryMustHaveOwner()
        {
            var newCategory = ExpenseCategoryFactory.CustomCategory("Coffee",string.Empty);
            var validation = _DomainService.IsValidNewCustomCategory(newCategory, null);

            Assert.IsFalse(validation.IsValid());
            Assert.AreEqual(Messages.CustomCategoriesMustHaveOwner, validation.ValidationErrorMessage);
        }
    }
}
