using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;
using RDS.ExpenseTracker.Desktop.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.Mappings
{
    public class ExpenseTrackerDesktopProfile : Profile
    {
        public ExpenseTrackerDesktopProfile()
        {
            CreateMap<Transaction, TransactionDataGridViewModel>()
                .ForMember(x => x.AccountName, opt => opt.MapFrom(src => src.FinancialAccountName))
                .ForMember(x => x.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(x => x.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date.Value)));
        }

    }
}
