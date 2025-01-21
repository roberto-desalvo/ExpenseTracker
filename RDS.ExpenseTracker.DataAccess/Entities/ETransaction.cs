﻿using Microsoft.EntityFrameworkCore;

namespace RDS.ExpenseTracker.DataAccess.Entities
{
    public class ETransaction
    {
        public int Id { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public ECategory Category { get; set; }
        public DateTime? Date {  get; set; }
        public int FinancialAccountId { get; set; }
        public EFinancialAccount FinancialAccount { get; set; }
        public bool IsTransfer {  get; set; }
    }
}
