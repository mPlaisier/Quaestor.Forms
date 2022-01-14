using FreshMvvm;
using Quaestor.Forms.Core.Dashboard.Constants;
using Quaestor.Forms.Core.Dashboard.ViewModels;
using Quaestor.Forms.Core.Database.Services;
using Xamarin.Forms;

namespace Quaestor.Forms.Core
{
    public partial class App : Application
    {
        #region Constructor

        public App()
        {
            InitializeComponent();

            InitializeServices();
            InitializeNavigation();
        }

        #endregion

        #region Private

        void InitializeServices()
        {
            //Db
            FreshIOC.Container.Register<IDatabaseService, LocalDatabase>();
        }

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
