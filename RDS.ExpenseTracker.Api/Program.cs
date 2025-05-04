using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Business.Services;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.DataAccess;
using RDS.ExpenseTracker.Api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var debugCorsPolicy = "Debug";

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ExpenseTrackerContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
    );

builder.Services.AddAutoMapper(x => x.AddProfile<ExpenseTrackerApiProfile>());
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IFinancialAccountService, FinancialAccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllers();

var app = builder.Build();


app.UseCors(debugCorsPolicy);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

