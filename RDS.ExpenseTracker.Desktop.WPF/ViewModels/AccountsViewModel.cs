using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public class AccountsViewModel : BaseViewModel
    {

        private readonly IFinancialAccountService _accountService;
        private IEnumerable<FinancialAccount> accounts;

        private string sellaAvailability { get; set; } = string.Empty;
        public string SellaAvailability
        {
            get { return sellaAvailability; }
            set
            {
                if (sellaAvailability != value)
                {
                    sellaAvailability = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string hypeAvailability { get; set; } = string.Empty;
        public string HypeAvailability
        {
            get { return hypeAvailability; }
            set
            {
                if (hypeAvailability != value)
                {
                    hypeAvailability = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string satispayAvailability { get; set; } = string.Empty;
        public string SatispayAvailability
        {
            get { return satispayAvailability; }
            set
            {
                if (satispayAvailability != value)
                {
                    satispayAvailability = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string contantiAvailability { get; set; } = string.Empty;
        public string ContantiAvailability
        {
            get { return contantiAvailability; }
            set
            {
                if (contantiAvailability != value)
                {
                    contantiAvailability = value;
                    NotifyPropertyChanged();
                }
            }
        }
        

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
