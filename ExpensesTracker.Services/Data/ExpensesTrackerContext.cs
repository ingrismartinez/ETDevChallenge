using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpensesTracker.Services.Entities;

    public class ExpensesTrackerContext : DbContext
    {
        public ExpensesTrackerContext (DbContextOptions<ExpensesTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<ExpensesTracker.Services.Entities.UserBudget> UserBudget { get; set; }
    }
