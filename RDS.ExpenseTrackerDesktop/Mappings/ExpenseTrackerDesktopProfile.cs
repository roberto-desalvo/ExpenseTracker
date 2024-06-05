using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Data.Entities;
using RDS.ExpenseTracker.Desktop.MVP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.Mappings
{
    public class ExpenseTrackerDesktopProfile : Profile
    {
        public ExpenseTrackerDesktopProfile()
        {
            CreateMap<Transaction, TransactionViewModel>()
                .ForMember(x => x.Transaction, opt => opt.MapFrom(src => src));
            CreateMap<FinancialAccount, FinancialAccountViewModel>()
                .ForMember(x => x.FinancialAccount, opt => opt.MapFrom(src => src));
        }

    }
}
