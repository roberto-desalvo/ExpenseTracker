using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels.Abstractions;
using System.Linq;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public class AccountsControlViewModel : BaseViewModel
    {
        private string sellaAvailability { get; set; } = string.Empty;
        private string hypeAvailability { get; set; } = string.Empty;
        private string satispayAvailability { get; set; } = string.Empty;
        private string contantiAvailability { get; set; } = string.Empty;

        private readonly IFinancialAccountService _accountService;

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
        

        public AccountsControlViewModel(IFinancialAccountService accountService)
        {
            _accountService = accountService;
            EventAggregator.Instance.Subscribe<RefreshMessage>(Refresh);
            Refresh();
        }

        public void Refresh(RefreshMessage message = null)
        {
            var accounts = _accountService.GetFinancialAccounts();

            SellaAvailability = accounts.Where(x => x.Name.ToLower().Contains("sella")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
            HypeAvailability = accounts.Where(x => x.Name.ToLower().Contains("hype")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
            SatispayAvailability = accounts.Where(x => x.Name.ToLower().Contains("satispay")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
            ContantiAvailability = accounts.Where(x => x.Name.ToLower().Contains("contanti")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
        }
    }
}
