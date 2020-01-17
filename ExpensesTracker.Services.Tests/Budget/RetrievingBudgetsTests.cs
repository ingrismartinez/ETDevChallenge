using ExpensesTracker.Services.AppServices;
using ExpensesTracker.Services.Data.Entities;
using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Entities;
using ExpensesTracker.Services.Requests;
using ExpensesTracker.Services.Responses.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesTracker.Services.Tests
{
    [TestClass]
    public class RetrievingBudgetsTests
    {
        private readonly UserBudgetsDomainService _userBudgetDomainService;
        public RetrievingBudgetsTests()
        {
            _userBudgetDomainService = new UserBudgetsDomainService();
        }
        [TestMethod]
        [TestCategory("UnitTest")]
        public void BudgetMustHaveOwnerId()
        {
            var request = new MonthBudgetRequest { };
            var budget = ExpensesTrackerFactory.CreateNewBudget(request, null);
            var validation = _userBudgetDomainService.IsValidNewUserBudget(budget);
            Assert.IsFalse(validation.IsValid());
            Assert.AreEqual(Messages.BudgetMustHaveAnOwner, validation.ValidationErrorMessage);
        }
        [TestMethod]
        [TestCategory("UnitTest")]
        public void BudgetMustHaveExpensesCategory()
        {
            var request = new MonthBudgetRequest {UserId="ingris" };
            var budget = ExpensesTrackerFactory.CreateNewBudget(request, null);
            var validation = _userBudgetDomainService.IsValidNewUserBudget(budget);
            Assert.IsFalse(validation.IsValid());
            Assert.AreEqual(Messages.BudgetMustContainsExpensesCategories, validation.ValidationErrorMessage);
        }
        [TestMethod]
        [TestCategory("UnitTest")]
        public void AllExpensesCategoriesExists()
        {
            var request = new MonthBudgetRequest { UserId = "ingris", BudgetCategories = new List<BudgetCategoryDto> {
            new BudgetCategoryDto{}
            } };
            var categories = new List<ExpenseCategory> { };
            var budget = ExpensesTrackerFactory.CreateNewBudget(request, categories);
            var validation = _userBudgetDomainService.IsValidNewUserBudget(budget);
            Assert.IsFalse(validation.IsValid());
            Assert.AreEqual(Messages.ExpensesCategoriesMustExists, validation.ValidationErrorMessage);
        }
        [TestMethod]
        [TestCategory("UnitTest")]
      
        public void ExpensesCategoryMustBeGreaterThanCero()
        {
            var request = new MonthBudgetRequest
            {
                UserId = "ingris",
                BudgetCategories = new List<BudgetCategoryDto> {
            new BudgetCategoryDto{}
            }
            };
            var categories = new List<ExpenseCategory> { new ExpenseCategory { } };
            var budget = ExpensesTrackerFactory.CreateNewBudget(request, categories);
            var validation = _userBudgetDomainService.IsValidNewUserBudget(budget);
            Assert.IsFalse(validation.IsValid());
            Assert.AreEqual(Messages.ExpensesPercentMustBeGreaterThanCero, validation.ValidationErrorMessage);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ExpensesCategoryMustBeLessOrEqualTo100Percent()
        {
            var request = new MonthBudgetRequest
            {
                UserId = "ingris",
                Amount=3000,
                BudgetCategories = new List<BudgetCategoryDto> {
            new BudgetCategoryDto{Percentage=120,}
            }
            };
            var categories = new List<ExpenseCategory> { new ExpenseCategory { } };
            var budget = ExpensesTrackerFactory.CreateNewBudget(request, categories);
            var validation = _userBudgetDomainService.IsValidNewUserBudget(budget);
            Assert.IsFalse(validation.IsValid());
            Assert.AreEqual(Messages.ExpesesCategoriesExceed100Percent, validation.ValidationErrorMessage);
        }
        
        [TestMethod]
        [TestCategory("UnitTest")]
        public void ExpensesCategoryMustBeLessOrEqualToBudgetAmount()
        {
            var request = new MonthBudgetRequest
            {
                UserId = "ingris",
                Amount = 3000,
                BudgetCategories = new List<BudgetCategoryDto> {
            new BudgetCategoryDto{Percentage=80},
            new BudgetCategoryDto{Percentage=20,CategoryId=1}
            }
            };
            var categories = new List<ExpenseCategory> { new ExpenseCategory { }, new ExpenseCategory { UId = 1 } };

            var budget = ExpensesTrackerFactory.CreateNewBudget(request, categories);
            var validation = _userBudgetDomainService.IsValidNewUserBudget(budget);
            Assert.IsTrue(validation.IsValid());
            Assert.AreEqual((decimal)3000, budget.BudgetDetails.Sum(c => c.Amount));
        }
    }
}
