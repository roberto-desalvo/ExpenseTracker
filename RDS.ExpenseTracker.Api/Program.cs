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
using RDS.ExpenseTracker.Api.Options;
using Serilog;
using RDS.ExpenseTracker.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        standardErrorFromLevel: Serilog.Events.LogEventLevel.Information
    )
    .CreateLogger();

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

builder.Services.AddDbContext<ExpenseTrackerContext>(optBuilder =>
{
    var kvOptions = new KeyVault();
    builder.Configuration.GetSection(nameof(KeyVault)).Bind(kvOptions);
    var connectionString = AzureKeyVaultHandler.GetKeyVaultSecret(kvOptions.Uri, kvOptions.ConnectionStringSecretName);
    optBuilder.UseSqlServer(connectionString, sqlServerBuilder => sqlServerBuilder.EnableRetryOnFailure());
    
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();


builder.Services.AddAutoMapper(x => x.AddProfile<ExpenseTrackerApiProfile>());
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IFinancialAccountService, FinancialAccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllers();
builder.Host.UseSerilog();

var app = builder.Build();


app.UseCors(debugCorsPolicy);

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers()
    .RequireAuthorization();

app.Run();

