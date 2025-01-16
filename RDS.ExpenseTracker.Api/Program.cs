using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Business.Services;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.DataAccess;
using RDS.ExpenseTracker.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var debugCorsPolicy = "Debug";
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(debugCorsPolicy,
            builder =>
            {
                builder.WithOrigins("http://127.0.0.1:5500", "http://127.0.0.1:5173")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });
}

builder.Services.AddDbContext<ExpenseTrackerContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
    );

builder.Services.AddAutoMapper(x => x.AddProfile<ExpenseTrackerApiProfile>());
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IFinancialAccountService, FinancialAccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseCors(debugCorsPolicy);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();
app.Run();

