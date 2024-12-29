using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Business.Services;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using RDS.ExpenseTrackerApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var localhostCorsPolicy = "Debug";
builder.Services.AddCors(options =>
{
    options.AddPolicy(localhostCorsPolicy,
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5500", "http://127.0.0.1:5173", "http://localhost:5500", "http://localhost:5173")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddDbContext<ExpenseTrackerContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
    );

builder.Services.AddAutoMapper(x => x.AddProfile(typeof(ExpenseTrackerApiProfile)));
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IFinancialAccountService, FinancialAccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(localhostCorsPolicy);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.Run();

