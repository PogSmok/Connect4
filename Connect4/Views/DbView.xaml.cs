using Connect4.Services;
using Connect4.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Connect4.Views
{
    /// <summary>
    /// Interaction logic for DbView.xaml
    /// </summary>
    public partial class DbView : Page
    {
        public DbView(NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new DbViewModel(navigationService);
        }
    }
}
