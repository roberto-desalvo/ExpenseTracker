namespace RDS.ExpenseTracker.Api.Options
{
    public class KeyVault
    {
        public string Uri { get; set; } = string.Empty;
        public string ConnectionStringSecretName { get; set; } = string.Empty;
    }
}
