using Connect4.ViewModels;
using Connect4.Services;
using System.Windows.Controls;

namespace Connect4.Views
{
    /// <summary>
    /// Interaction logic for GameSettingsView.xaml
    /// </summary>
    public partial class GameSettingsView : Page
    {
        public GameSettingsView(NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new GameSettingsViewModel(navigationService);
        }
    }
}
