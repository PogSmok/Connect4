using System.Configuration;
using System.Data;
using System.Windows;
using Connect4.Data;

namespace Connect4
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ensure the database exists
            using (var db = new GameDbContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}
