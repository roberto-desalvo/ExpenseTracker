using RDS.ExpenseTracker.Business.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public class AccountsControlViewModel : INotifyPropertyChanged
    {
        private string sellaAvailability { get; set; }
        private string hypeAvailability { get; set; }
        private string satispayAvailability { get; set; }
        private string contantiAvailability { get; set; }

        private readonly IFinancialAccountService _accountService;

        public string SellaAvailability
        {
            get { return sellaAvailability; }
            set
            {
                if (sellaAvailability != value)
                {
                    sellaAvailability = value;
                    OnPropertyChanged(nameof(SellaAvailability));
                }
            }
        }

        public string HypeAvailability
        {
            get { return hypeAvailability; }
            set
            {
                if (hypeAvailability != value)
                {
                    hypeAvailability = value;
                    OnPropertyChanged(nameof(HypeAvailability));
                }
            }
        }

        public string SatispayAvailability
        {
            get { return satispayAvailability; }
            set
            {
                if (satispayAvailability != value)
                {
                    satispayAvailability = value;
                    OnPropertyChanged(nameof(SatispayAvailability));
                }
            }
        }

        public string ContantiAvailability
        {
            get { return contantiAvailability; }
            set
            {
                if (contantiAvailability != value)
                {
                    contantiAvailability = value;
                    OnPropertyChanged(nameof(ContantiAvailability));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccountsControlViewModel()
        {
            
        }

        public AccountsControlViewModel(IFinancialAccountService accountService)
        {
            _accountService = accountService;
            Refresh();
        }

        public void Refresh()
        {
            var accounts = _accountService.GetFinancialAccounts();

            SellaAvailability = accounts.Where(x => x.Name.ToLower().Contains("sella")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
            HypeAvailability = accounts.Where(x => x.Name.ToLower().Contains("hype")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
            SatispayAvailability = accounts.Where(x => x.Name.ToLower().Contains("satispay")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
            ContantiAvailability = accounts.Where(x => x.Name.ToLower().Contains("contanti")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
        }
    }
}
