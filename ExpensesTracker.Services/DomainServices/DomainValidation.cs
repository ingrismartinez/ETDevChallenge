using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.DomainServices
{
    public class DomainValidation
    {
        public DomainValidation()
        {

        }
        public DomainValidation(string message)
        {
            ValidationErrorMessage = message;
        }
        public string ValidationErrorMessage { get; set; }

        public bool IsValid()
        {
            return string.IsNullOrWhiteSpace(ValidationErrorMessage);
        }
    }
}
