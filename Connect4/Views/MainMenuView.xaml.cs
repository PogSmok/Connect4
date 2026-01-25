using Connect4.ViewModels;
using Connect4.Services;
using System.Windows.Controls;

namespace Connect4.Views
{
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : Page
    {
        public MainMenuView(NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new MainMenuViewModel(navigationService);
        }
    }
}
