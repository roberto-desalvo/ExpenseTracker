using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Business.Services;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.DataAccess;
using RDS.ExpenseTracker.Api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using RDS.ExpenseTracker.DataAccess.Utilities;
using System.Collections;
using System.Diagnostics;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var debugCorsPolicy = "Debug";

builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(debugCorsPolicy,
            builder =>
            {
                builder.WithOrigins("http://127.0.0.1:5500", "http://127.0.0.1:5173", "http://localhost:5500", "http://localhost:5173")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });
}

builder.Configuration.AddEnvironmentVariables();

Trace.WriteLine("Environment Variables:");
foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
{
    Trace.WriteLine($"{entry.Key}: {entry.Value}");
}

var config = builder.Configuration.GetSection("AzureAd");

Trace.WriteLine("AzureAd Configuration:");
foreach (var key in config.GetChildren())
{
    Trace.WriteLine($"{key.Key}: {key.Value}");
}


builder.Services.AddDbContext<ExpenseTrackerContext>(x =>
{
    var keyVaultConfigSection = builder.Configuration.GetSection("KeyVault");
    var keyVaultUri = keyVaultConfigSection["Uri"];
    var connectionStringSecretName = keyVaultConfigSection["ConnectionStringSecretName"];
    var connectionString = AzureKeyVaultHandler.GetKeyVaultSecret(keyVaultUri, connectionStringSecretName);
    x.UseSqlServer(connectionString);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();


builder.Services.AddAutoMapper(x => x.AddProfile<ExpenseTrackerApiProfile>());
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IFinancialAccountService, FinancialAccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllers();

var app = builder.Build();


app.UseCors(debugCorsPolicy);

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();

