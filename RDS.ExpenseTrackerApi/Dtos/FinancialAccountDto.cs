namespace RDS.ExpenseTrackerApi.Dtos
{
    public class FinancialAccountDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Availability { get; set; }
        public string? Description { get; set; }
    }
}
