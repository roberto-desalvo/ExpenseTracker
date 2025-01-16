using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.Commands
{
    public class RefreshMessage
    {
        public Category? SelectedCategory { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; internal set; }
        public FinancialAccount? SelectedAccount { get; internal set; }

        public Func<IQueryable<ETransaction>, IQueryable<ETransaction>> GetTransactionFilter()
        {
            return transactions => transactions.Where(x =>

            (SelectedCategory == null) || (string.Equals(x.Category.Name, SelectedCategory.Name, StringComparison.InvariantCultureIgnoreCase))
            &&
            ((x.Date == null) || (StartDate == null) || (DateOnly.FromDateTime(x.Date.Value) > StartDate) || ((EndDate == null) || (DateOnly.FromDateTime(x.Date.Value) < EndDate))
            &&
            ((SelectedAccount == null) || string.Equals(x.FinancialAccount.Name, SelectedAccount.Name, StringComparison.InvariantCultureIgnoreCase))));

        }
    }
}
