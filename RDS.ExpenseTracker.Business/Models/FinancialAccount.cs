﻿
namespace RDS.ExpenseTracker.Business.Models
{
    public class FinancialAccount
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Availability { get; set; }
        public string? Description { get; set; }
    }
}
