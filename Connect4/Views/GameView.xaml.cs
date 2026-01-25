using Connect4.Services;
using Connect4.Models;
using Connect4.ViewModels;
using System.Windows.Controls;

namespace Connect4.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : Page
    {
        public GameView(NavigationService navigationService, GameSettingsModel gameSettings)
        {
            InitializeComponent();
            DataContext = new GameViewModel(navigationService, gameSettings);
        }
    }
}
