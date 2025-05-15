using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDS.ExpenseTracker.DataAccess;
using RDS.ExpenseTracker.DataAccess.Entities;
using RDS.ExpenseTracker.DataAccess.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.Extensions
{
    public static class ExpenseTrackerContextExtensions
    {
        public static void AddSeedData(this ExpenseTrackerContext context)
        {
            if (!context.Set<ECategory>().AsNoTracking().Any())
            {
                var seedCategories = SeedData.GetSeedCategories();
                context.Set<ECategory>().AddRange(seedCategories);
                context.SaveChanges();
            }
        }
    }
}
