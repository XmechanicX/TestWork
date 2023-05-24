using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Converters;
using TestWork.DataBase;
using TestWork.Instruments;

namespace TestWork.ViewModel
{
    public class vmCardOperationView : ViewModelBase
    {
        public vmCardOperationView(string cardNumber)
        {
            _account = new Account() { CardNumber = cardNumber };

            CardBalance = new NonParamRelayCommand(CardBalanceCommand, CanCardBalanceCommand);
            WithdrawMoney = new NonParamRelayCommand(WithdrawMoneyCommand, CanWithdrawMoneyCommand);
            Cancel = new NonParamRelayCommand(CancelCommand);
            PerformOperation = new NonParamRelayCommand(PerformOperationCommand, CanPerformOperationCommand);
        }

        public NonParamRelayCommand CardBalance { get; set; }
        public NonParamRelayCommand WithdrawMoney { get; set; }
        public NonParamRelayCommand Cancel { get; set; }
        public NonParamRelayCommand PerformOperation { get; set; }


        public vmMainWindow Parent;

        private Account _account;
        public Account Account
        {
            set
            {
                _account = value;
                CallPropertyChanged("Account");
            }
            get { return _account; }
        }

        private bool _visibilityBalance = false;
        public bool VisibilityBalance
        {
            get { return _visibilityBalance; }
        }

        private bool _visibilityWithdrawMoney = false;
        public bool VisibilityWithdrawMoney
        {
            get { return _visibilityWithdrawMoney; }
        }

        private string _money;
        public string Money
        {
            set
            {
                _money = value;
                CallPropertyChanged("Money");
            }
            get { return _money; }
        }

        private void CardBalanceCommand()
        {
            if (_visibilityBalance)
                _visibilityBalance = false;
            else
                _visibilityBalance = true;

            CallPropertyChanged("VisibilityBalance");
        }

        private bool CanCardBalanceCommand()
        {
            bool @return = true;



            return @return;
        }

        private void WithdrawMoneyCommand()
        {
            if (_visibilityWithdrawMoney)
                _visibilityWithdrawMoney = false;
            else
                _visibilityWithdrawMoney = true;

            CallPropertyChanged("VisibilityWithdrawMoney");
        }

        private bool CanWithdrawMoneyCommand()
        {
            bool @return = true;

            return @return;
        }

        private void CancelCommand()
        {
            Parent.GoToStartPage();
        }

        public void GetAccountInfo()
        {
            Parent.db.GetAccountInfo(Account);
        }

        private void PerformOperationCommand()
        {
            double balance = Account.Balance;
            double moneyWithDraw = double.Parse(Money);

            if (balance < moneyWithDraw)
            {
                MessageBox.Show("Недостаточно средств");
                return;
            }

            balance -= moneyWithDraw;
            bool result = Parent.db.SetNewBalance(balance, Account.CardNumber);

            if (!result) return;
            Account.Balance = balance;
            Money = "";
            WithdrawMoneyCommand();
            CallPropertyChanged("Account");

            MessageBox.Show
                (
                $"\tДата операции: {DateTime.Now.ToString("d")} \r\t" +
                $"Время операции: {DateTime.Now.ToString("T")} \r\t" +
                $"Номер банкомата: {23}\r\t" +
                $"Номер карты: ***{Account.CardNumber.Substring(Account.CardNumber.Length-4)}\r\t" +
                $"Сумма операции: {moneyWithDraw}\r\t" +
                $"Остаток на счете: {Account.Balance}\r\t"
                );
            //doto: информация о операции;
        }

        private bool CanPerformOperationCommand()
        {
            bool @return = false;

            if (!string.IsNullOrEmpty(Money))
                @return = true;

            return @return;
        }

    }
}
