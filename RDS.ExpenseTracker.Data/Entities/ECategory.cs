using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Data.Entities
{
    public class ECategory
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public ICollection<ETransaction>? Transactions { get; set; }
    }
}
