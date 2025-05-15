using AutoMapper;
using RDS.ExpenseTracker.Api.Dtos;
using RDS.ExpenseTracker.Domain.Models;
using Entities = RDS.ExpenseTracker.DataAccess.Entities;

namespace RDS.ExpenseTracker.Api.Helpers
{
    public class ExpenseTrackerApiProfile : Profile
    {
        public ExpenseTrackerApiProfile()
        {
            CreateMap<Entities.Transaction, Transaction>()
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Category.Description));

            CreateMap<Transaction, Entities.Transaction>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.FinancialAccount, opt => opt.Ignore());

            CreateMap<Entities.FinancialAccount, FinancialAccount>().ReverseMap();

            CreateMap<Entities.Category, Category>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(src => src.Tags.Split(';', StringSplitOptions.None)))
                .ReverseMap();

            CreateMap<FinancialAccount, FinancialAccountDto>()
                .ReverseMap();

            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryDescription))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.FinancialAccountId))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.FinancialAccountName))
                .ReverseMap();

            CreateMap<Category, CategoryDto>()
                .ReverseMap();
        }
    }
}
