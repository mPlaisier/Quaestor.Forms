using FreshMvvm;
using Quaestor.Forms.Core.Dashboard.Constants;
using Quaestor.Forms.Core.Dashboard.ViewModels;
using Xamarin.Forms;

namespace Quaestor.Forms.Core
{
    public partial class App : Application
    {
        #region Constructor

        public App()
        {
            InitializeComponent();


            InitializeNavigation();
            //MainPage = new MainPage();
        }

        #endregion

        #region Private

        void InitializeNavigation()
        {
            var masterDetailNav = new FreshMasterDetailNavigationContainer();
            masterDetailNav.Init("Menu");

            masterDetailNav.AddPage<DashboardViewModel>(Labels.Dashboard_Title, null);

            MainPage = masterDetailNav;
        }

        #endregion
    }
}
