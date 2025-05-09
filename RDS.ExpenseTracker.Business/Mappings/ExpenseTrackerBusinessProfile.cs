﻿using AutoMapper;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.DataAccess.Entities;

namespace RDS.ExpenseTracker.Business.Mappings
{
    public class ExpenseTrackerBusinessProfile : Profile
    {
        public ExpenseTrackerBusinessProfile()
        {
            CreateMap<ETransaction, Transaction>()
                .ForMember(x => x.CategoryDescription, opt => opt.MapFrom(x => x.Category.Description))
                .ForMember(x => x.FinancialAccountName, opt => opt.MapFrom(x => x.FinancialAccount.Name));

            CreateMap<Transaction, ETransaction>();

            CreateMap<EFinancialAccount, FinancialAccount>();
            CreateMap<FinancialAccount, EFinancialAccount>();

            CreateMap<ECategory, Category>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(src => src.Tags.Split(';', StringSplitOptions.None)));

            CreateMap<Category, ECategory>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(src => string.Join(';',src.Tags)));
        }
    }
}
