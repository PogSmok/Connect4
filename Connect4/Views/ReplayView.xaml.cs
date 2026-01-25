using Connect4.Services;
using Connect4.Models;
using Connect4.ViewModels;
using System.Windows.Controls;

namespace Connect4.Views
{
    /// <summary>
    /// Interaction logic for ReplayView.xaml
    /// </summary>
    public partial class ReplayView : Page
    {
        public ReplayView(NavigationService navigationService, GameDbRecord game)
        {
            InitializeComponent();
            DataContext = new ReplayViewModel(navigationService, game);
        }
    }
}
