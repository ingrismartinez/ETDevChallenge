using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.AppServices
{
    public class UserBudgetsAppService
    {
        private readonly ExpensesTrackerContext _context;
        public UserBudgetsAppService(ExpensesTrackerContext context)
        {
            _context = context ?? throw new ArgumentNullException("context");
        }
    }
}
