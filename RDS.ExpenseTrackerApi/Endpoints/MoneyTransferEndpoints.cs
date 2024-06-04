using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTrackerApi.Dtos.Requests;
using RDS.ExpenseTrackerApi.Helpers;

namespace RDS.ExpenseTrackerApi.Endpoints
{
    public static class MoneyTransferEndpoints
    {
        public static void AddMoneyTransferEndpoints(this WebApplication app)
        {
            app.MapGet("/moneytransfer/{id}", IResult (string id, IMoneyTransferService service) =>
            {
                return Results.Ok(service.GetMoneyTransfer(id));
            })
                .WithName("GET single MoneyTransfer")
                .WithOpenApi();

            app.MapGet("/moneytransfers", IResult (IMoneyTransferService service) =>
            {
                return Results.Ok(service.GetMoneyTransfers());
            })
                .WithName("GET all MoneyTransfers")
                .WithOpenApi();

            app.MapPost("/moneytransfer", IResult (MoneyTransferRequestDto dto, IMoneyTransferService service, IMapper mapper) =>
            {
                var MoneyTransfer = mapper.Map<MoneyTransfer>(dto);
                return SafeInvoker.SafeInvoke(() => service.AddMoneyTransfer(MoneyTransfer))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Insert moneytransfer")
                .WithOpenApi();

            app.MapPut("/moneytransfer", IResult (MoneyTransferRequestDto dto, IMoneyTransferService service, IMapper mapper) =>
            {
                var MoneyTransfer = mapper.Map<MoneyTransfer>(dto);
                return SafeInvoker.SafeInvoke(() => service.UpdateMoneyTransfer(MoneyTransfer))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Update moneytransfer")
                .WithOpenApi();

            app.MapDelete("/moneytransfer/{id}", IResult (string id, IMoneyTransferService service, IMapper mapper) =>
            {
                return SafeInvoker.SafeInvoke(() => service.DeleteMoneyTransfer(id))
                ?
                TypedResults.Ok() : TypedResults.Problem();
            })
                .WithName("Delete moneytransfer")
                .WithOpenApi();
        }
    }
}

