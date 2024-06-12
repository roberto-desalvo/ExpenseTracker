using RDS.ExpenseTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Data.Helpers
{
    public static class SeedingHelper
    {
        public static IEnumerable<ECategory> GetSeedCategories()
        {
            yield return new ECategory { Id = 1, Priority = 0, Name = "SpostamentiDenaro", Tags = "hype;sella;satispay;contanti" };
            yield return new ECategory { Id = 2, Priority = 1, Name = "Lavoro", Tags = "lavoro;salsedine;mare nostrum;msc" };
            yield return new ECategory { Id = 3, Priority = 2, Name = "Psicologa", Tags = "psicolog" };
            yield return new ECategory { Id = 4, Priority = 3, Name = "Spese_di_Casa", Tags = "casa;affitto;bollette" };
            yield return new ECategory { Id = 5, Priority = 4, Name = "Spesa", Tags = "spesa" };
            yield return new ECategory { Id = 6, Priority = 5, Name = "Salute", Tags = "farmaci;salute;medicine;integrator" };
            yield return new ECategory { Id = 7, Priority = 6, Name = "Pranzo", Tags = "pranzo" };
            yield return new ECategory { Id = 8, Priority = 7, Name = "Cena", Tags = "cena" };
            yield return new ECategory { Id = 9, Priority = 8, Name = "Bere", Tags = "bere;birra;cocktail" };
            yield return new ECategory { Id = 10, Priority = 9, Name = "Fuori", Tags = "fuori" };
            yield return new ECategory { Id = 11, Priority = 10, Name = "Spostamenti", Tags = "dott;gtt;metro;spostament" };
            yield return new ECategory { Id = 12, Priority = 11, Name = "Abbonamento", Tags = "abbonament" };
            yield return new ECategory { Id = 13, Priority = 12, Name = "Cibo_a_Domicilio", Tags = "glovo;just eat;uber;" };
            yield return new ECategory { Id = 14, Priority = 13, Name = "Svago", Tags = "svago;calcetto;fantacalcio;gioco" };
            yield return new ECategory { Id = 15, Priority = 14, Name = "Sigarette", Tags = "tabacc;siga;" };
            yield return new ECategory { Id = 16, Priority = 15, Name = "Straordinarie", Tags = "xx" };
            yield return new ECategory { Id = 17, Priority = 16, Name = "Altro", Tags = "aggiustament" };
        }
    }
}
