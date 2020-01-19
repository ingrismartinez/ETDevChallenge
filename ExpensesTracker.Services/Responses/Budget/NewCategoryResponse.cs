using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Services.Responses.Budget
{
    public class NewCategoryResponse:ResponseBase
    {
        public int CategoryId { get; set; }
        public string UserId { get; set; }
    }
}
