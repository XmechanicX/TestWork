using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TestWork.ViewModel;

namespace TestWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new vmMainWindow();
        }
    }
}
