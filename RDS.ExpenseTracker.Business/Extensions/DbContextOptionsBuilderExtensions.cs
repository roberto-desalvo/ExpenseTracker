using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDS.ExpenseTracker.DataAccess.Entities;
using RDS.ExpenseTracker.DataAccess.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static void AddSeedData(this DbContextOptionsBuilder optBuilder)
        {
            optBuilder.UseSeeding((context, _) =>
            {
                if (!context.Set<ECategory>().AsNoTracking().Any())
                {
                    var seedCategories = SeedData.GetSeedCategories();
                    context.Set<ECategory>().AddRange(seedCategories);
                    context.SaveChanges();
                }
            });
        }
    }
}
