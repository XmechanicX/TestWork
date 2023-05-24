using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestWork.DataBase;
using TestWork.Instruments;
using TestWork.View;

namespace TestWork.ViewModel
{
    public class vmCardNumberInput : ViewModelBase
    {
        public vmCardNumberInput()
        {
            Continue = new NonParamRelayCommand(ContinueCommand, CanContinueCommand);
            ClearAll = new NonParamRelayCommand(ClearAllCommand, CanClearAllCommand);
        }

        public NonParamRelayCommand Continue { get; set; }
        public NonParamRelayCommand ClearAll { get; set; }

        public vmMainWindow Parent;

        private string _cardNumber;
        public string CardNumber
        {
            set
            {
                _cardNumber = value;
                CallPropertyChanged("CardNumber");
            }
            get { return _cardNumber; }
        }

        private void ContinueCommand()
        {
            var cardNumber = String.Concat(_cardNumber.Split(new char[] { '_' }));
            var @return = Parent.db.GetCardNumber(cardNumber);

            if (@return == null)
            {
                MessageBox.Show("Карта не найдена.");
                return;
            }

            if (@return.StatusAccounts == StatusAccount.Blocked)
            {
                MessageBox.Show("Статус счета: Заблокирован.");
                return;
            }

            if (Parent != null && @return != null)
                Parent.NextPage(@return);
        }

        private bool CanContinueCommand()
        {
            bool @return = false;

            if (!string.IsNullOrEmpty(_cardNumber))
                @return = true;

            return @return;
        }

        private void ClearAllCommand()
        {
            CardNumber = "";
        }

        private bool CanClearAllCommand()
        {
            bool @return = false;

            if (!string.IsNullOrEmpty(_cardNumber))
                @return = true;

            return @return;
        }
    }
}
