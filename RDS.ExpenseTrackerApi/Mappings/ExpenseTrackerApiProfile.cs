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
            CreateMap<EFinancialAccount, FinancialAccount>().ReverseMap();
            CreateMap<FinancialAccount, FinancialAccountDto>().ReverseMap();
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryDescription))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.FinancialAccountId))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.FinancialAccountName));
        }
    }
}
