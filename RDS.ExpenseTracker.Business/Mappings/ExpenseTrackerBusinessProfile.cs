using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;

namespace RDS.ExpenseTracker.Business.Mappings
{
    public class ExpenseTrackerBusinessProfile : Profile
    {
        public ExpenseTrackerBusinessProfile()
        {
            CreateMap<ETransaction, Transaction>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name));

            CreateMap<Transaction, ETransaction>()
                .ForPath(x => x.Category.Name, opt => opt.MapFrom(x => x.CategoryName));

            CreateMap<EFinancialAccount, FinancialAccount>();
            CreateMap<FinancialAccount, EFinancialAccount>();

            CreateMap<ECategory, Category>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(src => src.Tags.Split(';', StringSplitOptions.None)));

            CreateMap<Category, ECategory>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(src => string.Join(';',src.Tags)));
        }
    }
}
