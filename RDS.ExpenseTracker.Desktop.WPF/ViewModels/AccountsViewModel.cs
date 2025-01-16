using CommunityToolkit.Mvvm.ComponentModel;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public partial class AccountsViewModel : ObservableObject
    {

        private readonly IFinancialAccountService _accountService;
        private IEnumerable<FinancialAccount> accounts;

        [ObservableProperty]
        private string sellaAvailability = string.Empty;

        [ObservableProperty]
        private string hypeAvailability = string.Empty;

        [ObservableProperty]
        private string satispayAvailability = string.Empty;

        [ObservableProperty]
        private string contantiAvailability = string.Empty;
        

        public AccountsViewModel(IFinancialAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            accounts = Task.Run(async () => await _accountService.GetFinancialAccounts()).Result;
            EventAggregator.Instance.Subscribe<RefreshMessage>(Refresh);
        }

        public void Refresh(RefreshMessage message = null)
        {
            accounts = Task.Run(async () => await _accountService.GetFinancialAccounts()).Result;

            SellaAvailability = GetAccountAvailability("sella");
            HypeAvailability = GetAccountAvailability("hype");
            SatispayAvailability = GetAccountAvailability("satispay");
            ContantiAvailability = GetAccountAvailability("contanti");
        }

        private string GetAccountAvailability(string accountName)
        {
            var availability = accounts.Where(x => x.Name.ToLower().Contains(accountName)).FirstOrDefault()?.Availability.ToString();

            return string.IsNullOrEmpty(availability) ? string.Empty 
                : (availability.Length == 1) ? $"0.0{availability}"
                : (availability.Length == 2) ? $"0.{availability}"
                : $"{availability[..^2]}.{availability[^2..]}";
        }
    }
}
