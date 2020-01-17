﻿using ExpensesTracker.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Data.Entities
{
    public static class ExpensesTrackerFactory
    {
        public static ExpenseCategory DefaultCategory(string categoryName)
        {
            return new ExpenseCategory { IsDefault = true, Name = categoryName };
        }

        public static ExpenseCategory CustomCategory(string categoryName, string userId)
        {
            return new ExpenseCategory { IsDefault = false, Name = categoryName, OwnerId = userId };
        }
        public static string MonthName(this DateTime date)
        {
            return $"{date.ToString("MMMM")} {date.Year}";
        }
    }
}