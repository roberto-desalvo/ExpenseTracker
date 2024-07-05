using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;
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

        public Predicate<ETransaction> ToTransactionPredicate()
        {
            return x =>
            {
                var result = true;

                if (SelectedCategory != null && x.Category.Name.ToUpperInvariant() != SelectedCategory.Name.ToUpperInvariant())
                {
                    result = false;
                }

                if (x.Date != null)
                {
                    if (StartDate != null && DateOnly.FromDateTime(x.Date.Value) < StartDate)
                    {
                        result = false;
                    }

                    if (EndDate != null && DateOnly.FromDateTime(x.Date.Value) > EndDate)
                    {
                        result = false;
                    }
                }

                if(SelectedAccount != null && x.FinancialAccount.Name.ToUpperInvariant() != SelectedAccount.Name.ToUpperInvariant())
                {
                    return false;
                }

                return result;
            };
        }
    }
}
