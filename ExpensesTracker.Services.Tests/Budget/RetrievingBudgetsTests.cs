using ExpensesTracker.Services.AppServices;
using ExpensesTracker.Services.Data.Entities;
using ExpensesTracker.Services.DomainServices;
using ExpensesTracker.Services.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        }
        [TestMethod]
        [TestCategory("UnitTest")]
        public void ExpensesCategoryMustBeLessOrEqualTo100Percent()
        {
        }
        [TestMethod]
        [TestCategory("UnitTest")]
        public void ExpensesCategoryMustBeLessOrEqualToBudgetAmount()
        {
        }
    }
}
