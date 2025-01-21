﻿using Microsoft.EntityFrameworkCore;

namespace RDS.ExpenseTracker.DataAccess.Entities
{
    public class EFinancialAccount
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Precision(18, 2)]
        public decimal Availability { get; set; }
        public ICollection<ETransaction>? Transactions { get; set; }
    }
}
