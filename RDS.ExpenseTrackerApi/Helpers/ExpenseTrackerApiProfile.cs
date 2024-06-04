using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;
using RDS.ExpenseTrackerApi.Dtos;
using RDS.ExpenseTrackerApi.Dtos.Requests;

namespace RDS.ExpenseTrackerApi.Helpers
{
    public class ExpenseTrackerApiProfile : Profile
    {
        public ExpenseTrackerApiProfile()
        {
            CreateMap<ETransaction, Transaction>();
            CreateMap<Transaction, ETransaction>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionDto, Transaction>();

            CreateMap<EFinancialAccount, FinancialAccount>();
            CreateMap<FinancialAccount, EFinancialAccount>();
            CreateMap<FinancialAccount, FinancialAccountDto>();
            CreateMap<FinancialAccountDto, FinancialAccount>();

            CreateMap<EMoneyTransfer, MoneyTransfer>();
            CreateMap<MoneyTransfer, EMoneyTransfer>();
            CreateMap<MoneyTransfer, MoneyTransferRequestDto>();
            CreateMap<MoneyTransferRequestDto, MoneyTransfer>();
        }
    }
}
