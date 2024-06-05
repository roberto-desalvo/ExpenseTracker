using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTrackerApi.Dtos;
using RDS.ExpenseTrackerApi.Helpers;

namespace RDS.ExpenseTrackerApi.Endpoints
{
    public static class TransactionEndpoints
    {
        public static void AddTransactionEndpoints(this WebApplication app)
        {
            app.MapGet("/Transaction/{id}", IResult (string id, ITransactionService service) =>
            {
                return Results.Ok(service.GetTransaction(id));
            })
                .WithName("GET single Transaction")
                .WithOpenApi();

            
            app.MapGet("/Transactions", IResult (ITransactionService service) =>
            {
                return Results.Ok(service.GetTransactions());
            })
                .WithName("GET all Transactions")
                .WithOpenApi();            

            app.MapPost("/Transaction", IResult (TransactionDto dto, ITransactionService service, IMapper mapper) =>
            {
                var Transaction = mapper.Map<Transaction>(dto);
                return SafeInvoker.SafeInvoke(() => service.AddTransaction(Transaction, true))  
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Insert Transaction")
                .WithOpenApi();

            app.MapPut("/Transaction", IResult (TransactionDto dto, ITransactionService service, IMapper mapper) =>
            {
                var Transaction = mapper.Map<Transaction>(dto);
                return SafeInvoker.SafeInvoke(() => service.UpdateTransaction(Transaction))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Update Transaction")
                .WithOpenApi();

            app.MapDelete("/Transaction/{id}", IResult (string id, ITransactionService service, IMapper mapper) =>
            {
                return SafeInvoker.SafeInvoke(() => service.DeleteTransaction(id))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Delete Transaction")
                .WithOpenApi();
        }
    }
}
