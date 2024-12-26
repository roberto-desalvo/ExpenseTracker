using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTrackerApi.Dtos;
using RDS.ExpenseTrackerApi.Helpers;

namespace RDS.ExpenseTrackerApi.Endpoints
{
    public static class FinancialAccountEndpoints
    {
        public static void AddFinancialAccountEndpoints(this WebApplication app)
        {
            app.MapGet("/account/{id}", async (int id, IFinancialAccountService service, IMapper mapper) =>
            {
                var account = await service.GetFinancialAccount(id);
                var dto = mapper.Map<FinancialAccountDto>(account);
                return Results.Ok(dto);
            })
                .WithName("GET Single account")
                .WithOpenApi();

            app.MapGet("/account/{id}/availability", async (int id, IFinancialAccountService service) =>
            {
                var availability = await service.GetAvailability(id);
                return Results.Ok(availability);
            })
                .WithName("GET account availabililty")
                .WithOpenApi();

            app.MapGet("/accounts", async (IFinancialAccountService service, IMapper mapper) =>
            {
                var accounts = await service.GetFinancialAccounts();
                var dtos = mapper.Map<IEnumerable<FinancialAccountDto>>(accounts);
                return Results.Ok(dtos);
            })
                .WithName("GET All accounts")
                .WithOpenApi();

            app.MapPost("/account", IResult (FinancialAccountDto dto, IFinancialAccountService service, IMapper mapper) =>
            {
                var account = mapper.Map<FinancialAccount>(dto);
                return SafeInvoker.SafeInvoke(async () => await service.AddFinancialAccount(account))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Insert account")
                .WithOpenApi();

            app.MapPut("/account", IResult (FinancialAccountDto dto, IFinancialAccountService service, IMapper mapper) =>
            {
                var account = mapper.Map<FinancialAccount>(dto);
                return SafeInvoker.SafeInvoke(async () => await service.UpdateFinancialAccount(account))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Update account")
                .WithOpenApi();

            app.MapDelete("/account/{id}", IResult (int id, IFinancialAccountService service, IMapper mapper) =>
            {
                return SafeInvoker.SafeInvoke(async () => await service.DeleteFinancialAccount(id))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Delete account")
                .WithOpenApi();

            app.MapGet("/account/{id}/Transactions", async (int id, ITransactionService service, IMapper mapper) =>
            {
                var transactions = await service.GetTransactions(query => query.Where(transaction => transaction.FinancialAccountId == id));
                var dtos = mapper.Map<IEnumerable<TransactionDto>>(transactions);
                return Results.Ok(dtos);
            })
                .WithName("GET Transactions by account")
                .WithOpenApi();
        }
    }
}
