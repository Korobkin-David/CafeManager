using System.Windows;
using System.Windows.Controls;
using CafeManager.Pages;

namespace CafeManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Login()); // přepnutí na Login stránku
        }
    }
}