using RDS.ExpenseTracker.Business.Models;

namespace RDS.ExpenseTracker.Business.Helpers
{
    public static class CategoryHelper
    {
        private static Dictionary<string, CategoryEnum> CategoryMap = new()
        {
            { "xx", CategoryEnum.Straordinarie },
            { "psicolog", CategoryEnum.Psicologa },
            { "tabacc", CategoryEnum.Sigarette },
            { "glovo", CategoryEnum.Cibo_a_Domicilio },
            { "uber", CategoryEnum.Cibo_a_Domicilio },
            { "just eat", CategoryEnum.Cibo_a_Domicilio },
            { "spesa", CategoryEnum.Spesa },
            { "svago", CategoryEnum.Svago },
            { "gtt", CategoryEnum.Spostamenti },
            { "spostament", CategoryEnum.Spostamenti },
            { "metro", CategoryEnum.Spostamenti },
            { "dott", CategoryEnum.Spostamenti },
            { "abbonament", CategoryEnum.Abbonamento },
            { "salute", CategoryEnum.Salute },
            { "farmacia", CategoryEnum.Salute},
            { "fuori", CategoryEnum.Fuori},
            { "bere", CategoryEnum.Bere},
            { "pranzo", CategoryEnum.Pranzo},
            { "cena", CategoryEnum.Cena},
            { "hype", CategoryEnum.SpostamentiDenaro},
            { "sella", CategoryEnum.SpostamentiDenaro},
            { "satispay", CategoryEnum.SpostamentiDenaro},
            { "contanti", CategoryEnum.SpostamentiDenaro},
            { "lavoro", CategoryEnum.Lavoro},
            { "salsedine", CategoryEnum.Lavoro},
            { "mare nostrum", CategoryEnum.Lavoro},
            { "msc", CategoryEnum.Lavoro}
        };

        public static void UpdateCategories(IEnumerable<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var description = transaction.Description.ToLower();
                foreach (var category in CategoryMap)
                {
                    if (description.Contains(category.Key))
                    {
                        transaction.Category = category.Value;
                        continue;
                    }
                }
            }
        }

        public static CategoryEnum GetCategory(string description)
        {
            foreach (var category in CategoryMap)
            {
                if (description.Contains(category.Key))
                {
                    return category.Value;
                }
            }
            return CategoryEnum.Altro;
        }
    }
}
