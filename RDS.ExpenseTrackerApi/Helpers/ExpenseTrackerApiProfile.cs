using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;
using RDS.ExpenseTrackerApi.Dtos;

namespace RDS.ExpenseTrackerApi.Helpers
{
    public class ExpenseTrackerApiProfile : Profile
    {
        public ExpenseTrackerApiProfile()
        {
            CreateMap<ETransaction, Transaction>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<EFinancialAccount, FinancialAccount>().ReverseMap();
            CreateMap<FinancialAccount, FinancialAccountDto>().ReverseMap();
        }
    }
}
