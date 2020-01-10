using ExpensesTracker.Services.AppServices;
using ExpensesTracker.Services.DomainServices;
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
        public void RetrievingUnExistingBudget()
        {
        }
    }
}
