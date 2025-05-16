﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.DataAccess.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
