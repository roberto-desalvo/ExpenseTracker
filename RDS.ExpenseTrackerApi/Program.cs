using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Business.Services;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using RDS.ExpenseTrackerApi.Endpoints;
using RDS.ExpenseTrackerApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var corsPolicyLiveServer = "AllowLiveServer";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyLiveServer,
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5500")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddDbContext<ExpenseTrackerContext>(x => 
    x.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
    );

builder.Services.AddAutoMapper(typeof(ExpenseTrackerApiProfile));
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IFinancialAccountService, FinancialAccountService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
}

app.AddTransactionEndpoints();
app.AddFinancialAccountEndpoints();
app.UseCors(corsPolicyLiveServer);
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.Run();

