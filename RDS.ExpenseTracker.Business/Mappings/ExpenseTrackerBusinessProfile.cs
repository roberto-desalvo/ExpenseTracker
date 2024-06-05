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
                .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Category.ToString()));

            CreateMap<Transaction, ETransaction>()
                .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Category.ToString()));

            CreateMap<EFinancialAccount, FinancialAccount>();
            CreateMap<FinancialAccount, EFinancialAccount>()
                .ForMember(x => x.Availability, opt => opt.MapFrom(x => x.Availability));
        }

        public class CategoryEnumConverter : IValueResolver<ETransaction, Transaction, CategoryEnum>
        {
            public CategoryEnum Resolve(ETransaction source, Transaction destination, CategoryEnum destMember, ResolutionContext context)
            {
                if (Enum.TryParse(typeof(CategoryEnum), source.Category, out var parsedResult))
                {
                    return (CategoryEnum)parsedResult;
                }
                return CategoryEnum.Altro;
            }
        }
    }
}
