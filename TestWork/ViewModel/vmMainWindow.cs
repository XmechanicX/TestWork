using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using TestWork.DataBase;
using TestWork.Instruments;
using TestWork.View;

namespace TestWork.ViewModel
{
    public class vmMainWindow : ViewModelBase
    {
        public vmMainWindow()
        {
            NextPage("");
        }

        public ApplicationContext db = new ApplicationContext();

        private Page _currentPage;
        public Page CurrentPage
        {
            set
            {
                _currentPage = value;
                CallPropertyChanged("CurrentPage");
            }
            get { return _currentPage; }
        }

        public void NextPage<T>(T _object)
        {
            if (_currentPage == null)
            {
                var vm = new vmCardNumberInput() { Parent = this };
                CurrentPage = new CardNumberInputView() { DataContext = vm };
            }
            else if (_currentPage is CardNumberInputView)
            {
                var vm = new vmPinCodeInput() { Parent = this, Account = _object as Account };
                CurrentPage = new PinCodeInputView() { DataContext = vm };
            }
            else if (_currentPage is PinCodeInputView)
            {
                var vm = new vmCardOperationView(_object as string) { Parent = this };
                vm.GetAccountInfo();
                CurrentPage = new CardOperationView() { DataContext = vm };
            }
        }

        public void GoToStartPage()
        {
            var vm = new vmCardNumberInput() { Parent = this };
            CurrentPage = new CardNumberInputView() { DataContext = vm };
        }
    }
}
