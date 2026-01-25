using System.Windows;
using Connect4.Services;

namespace Connect4.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NavigationService MainNavigationService { get; }

        public MainWindow()
        {
            InitializeComponent();

            // Create navigation service for the main frame
            MainNavigationService = new NavigationService(MainFrame);

            // Load main menu on startup
            MainFrame.Navigate(new MainMenuView(MainNavigationService));

            // Play music when window is shown
            this.Loaded += (s, e) =>
            {
                SoundService.PlayBackgroundMusic();
            };

            // Stop music when window closes
            this.Closing += (s, e) =>
            {
                SoundService.StopBackgroundMusic();
            };
        }
    }
}