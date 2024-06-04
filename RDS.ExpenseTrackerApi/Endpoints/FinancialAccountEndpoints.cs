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
            app.MapGet("/account/{id}", IResult (string id, IFinancialAccountService service) =>
            {
                return Results.Ok(service.GetFinancialAccount(id));
            })
                .WithName("GET Single account")
                .WithOpenApi();

            app.MapGet("/account/{id}/availability", IResult (int id, IFinancialAccountService service) =>
            {
                return Results.Ok(service.GetAvailability(id));
            })
                .WithName("GET account availabililty")
                .WithOpenApi();

            app.MapGet("/accounts", IResult (IFinancialAccountService service) =>
            {
                return Results.Ok(service.GetFinancialAccounts());
            })
                .WithName("GET All accounts")
                .WithOpenApi();

            app.MapPost("/account", IResult (FinancialAccountDto dto, IFinancialAccountService service, IMapper mapper) =>
            {
                var account = mapper.Map<FinancialAccount>(dto);
                return SafeInvoker.SafeInvoke(() => service.AddFinancialAccount(account))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Insert account")
                .WithOpenApi();

            app.MapPut("/account", IResult (FinancialAccountDto dto, IFinancialAccountService service, IMapper mapper) =>
            {
                var account = mapper.Map<FinancialAccount>(dto);
                return SafeInvoker.SafeInvoke(() => service.UpdateFinancialAccount(account))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Update account")
                .WithOpenApi();

            app.MapDelete("/account/{id}", IResult (string id, IFinancialAccountService service, IMapper mapper) =>
            {
                return SafeInvoker.SafeInvoke(() => service.DeleteFinancialAccount(id))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Delete account")
                .WithOpenApi();

            app.MapGet("/account/{id}/Transactions", IResult (int id, ITransactionService service) =>
            {
                return Results.Ok(service.GetTransactions(x => x.FinancialAccountId == id));
            })
                .WithName("GET Transactions by account")
                .WithOpenApi();
        }
    }
}
