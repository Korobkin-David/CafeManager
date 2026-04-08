using System.Configuration;
using System.Data;
using System.Windows;
using CafeManager.Database;
using CafeManager.Pages;

namespace CafeManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            new Database.Database().Initialize();
        }
    }
};
