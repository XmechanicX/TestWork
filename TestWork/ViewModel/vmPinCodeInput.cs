using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestWork.DataBase;
using TestWork.Instruments;

namespace TestWork.ViewModel
{
    public class vmPinCodeInput : ViewModelBase
    {
        public vmPinCodeInput()
        {
            Continue = new NonParamRelayCommand(ContinueCommand, CanContinueCommand);
            ClearAll = new NonParamRelayCommand(ClearAllCommand, CanClearAllCommand); Cancel = new NonParamRelayCommand(CancelCommand);
        }

        public NonParamRelayCommand Continue { get; set; }
        public NonParamRelayCommand ClearAll { get; set; }
        public NonParamRelayCommand Cancel { get; set; }


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

        private string _pinCode;
        public string PinCode
        {
            set
            {
                _pinCode = value;
                CallPropertyChanged("PinCode");
            }
            get { return _pinCode; }
        }

        private int FailedPinCodeEntry = 0;

        private void ContinueCommand()
        {
            if (_pinCode == "") return;

            var result = Parent.db.ComparePinCodes(Account.CardNumber, _pinCode);

            if (result)
                Parent.NextPage(Account.CardNumber);
            else
            {
                FailedPinCodeEntry++;
                MessageBox.Show("Неправильный пинкод");
            }

            if (FailedPinCodeEntry == 3)
            {
                Parent.db.BlockingCard(Account.CardNumber);
                MessageBox.Show("Ваша карта заблокирована");
            }
        }

        private bool CanContinueCommand()
        {
            bool @return = false;

            if (!string.IsNullOrEmpty(_pinCode) 
                && FailedPinCodeEntry < 3 
                && Account.StatusAccounts != StatusAccount.Blocked)
                @return = true;

            return @return;
        }

        private void ClearAllCommand()
        {
            PinCode = "";
        }

        private bool CanClearAllCommand()
        {
            bool @return = false;

            if (!string.IsNullOrEmpty(_pinCode))
                @return = true;

            return @return;
        }

        private void CancelCommand()
        {
            Parent.GoToStartPage();
        }
    }
}
