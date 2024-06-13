using RDS.ExpenseTracker.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.Services.Abstractions
{
    public interface ICategoryService
    {
        public IEnumerable<Category> GetCategories();
    }
}
