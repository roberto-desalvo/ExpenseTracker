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
            app.MapGet("/Transaction/{id}", async (int id, ITransactionService service, IMapper mapper) =>
            {
                var transaction = await service.GetTransaction(id);
                var dto = mapper.Map<TransactionDto>(transaction);
                return Results.Ok(dto);
            })
                .WithName("GET single Transaction")
                .WithOpenApi();

            
            app.MapGet("/Transactions", async (ITransactionService service, IMapper mapper) =>
            {
                try
                {
                    var results = await service.GetTransactions();
                    var dtos = mapper.Map<IEnumerable<TransactionDto>>(results);
                    return Results.Ok(dtos);
                }
                catch(Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
                
            })
                .WithName("GET all Transactions")
                .WithOpenApi();            

            app.MapPost("/Transaction", IResult (TransactionDto dto, ITransactionService service, IMapper mapper) =>
            {
                var transaction = mapper.Map<Transaction>(dto);
                return SafeInvoker.SafeInvoke(async () => await service.AddTransaction(transaction))  
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Insert Transaction")
                .WithOpenApi();

            app.MapPut("/Transaction", IResult (TransactionDto dto, ITransactionService service, IMapper mapper) =>
            {
                var transaction = mapper.Map<Transaction>(dto);
                return SafeInvoker.SafeInvoke(async () => await service.UpdateTransaction(transaction))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Update Transaction")
                .WithOpenApi();

            app.MapDelete("/Transaction/{id}", IResult (int id, ITransactionService service, IMapper mapper) =>
            {
                return SafeInvoker.SafeInvoke(async () => await service.DeleteTransaction(id))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Delete Transaction")
                .WithOpenApi();
        }
    }
}
