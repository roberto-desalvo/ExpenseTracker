using AutoMapper;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.DataAccess.Entities;
using RDS.ExpenseTracker.Desktop.WPF.Models;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels;
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
            CreateMap<Transaction, TransactionGridRowModel>()
                .ForMember(x => x.AccountName, opt => opt.MapFrom(src => src.FinancialAccountName))
                .ForMember(x => x.Category, opt => opt.MapFrom(src => src.CategoryDescription))
                .ForMember(x => x.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date ?? DateTime.Now)));
        }
    }
}
