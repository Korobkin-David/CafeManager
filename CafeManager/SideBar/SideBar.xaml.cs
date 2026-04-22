using System.Windows.Controls;
using CafeManager.Utils;
using CafeManager.ViewModels;

namespace CafeManager.SideBar;

public partial class SideBar : UserControl
{
    public SideBar()
    {
        InitializeComponent();
        var vm = new UsersViewModel();
        vm.NactiUzivatele(SessionManager.UserId);
        DataContext = vm;
    }
}