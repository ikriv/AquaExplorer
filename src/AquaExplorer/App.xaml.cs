using System.Windows;
using AquaExplorer.Services;
using AquaExplorer.ViewModels;
using AquaExplorer.ViewModels.Services;
using AquaExplorer.Views;
using IKriv.Windows.Mvvm;
using Microsoft.Practices.Unity;

namespace AquaExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureDataTemplates();
            var mainWindow = new MainWindow();
            var container = ConfigureContainer(mainWindow);
            mainWindow.DataContext = container.Resolve<MainViewModel>();
            mainWindow.Show();
        }

        private void ConfigureDataTemplates()
        {
            var dt = new DataTemplateManager();
            dt
                .RegisterDataTemplate<AccountListViewModel, AccountListView>()
                .RegisterDataTemplate<ContainerListViewModel, ContainerListView>()
                .RegisterDataTemplate<BlobListViewModel, BlobListView>()
                .RegisterDataTemplate<DownloadViewModel, DownloadView>()
                .RegisterDataTemplate<DownloadBarViewModel, DownloadBarView>()
                .RegisterDataTemplate<AddAccountViewModel, AddAccountView>()
                .RegisterDataTemplate<ExceptionViewModel, ExceptionView>();
        }

        private static ContainerControlledLifetimeManager Singleton()
        {
            return new ContainerControlledLifetimeManager();
        }

        private IUnityContainer ConfigureContainer(Window mainWindow)
        {
            var container = new UnityContainer();
            container
                .RegisterInstance(mainWindow)
                .RegisterType<IDataProtectionService, DataProtectionService>(Singleton())
                .RegisterType<IConfigurationService, ConfigurationService>(Singleton())
                .RegisterType<INavigationController, NavigationController>(Singleton())
                .RegisterType<INavigationTargetBuilder, ViewModelBuilder>(Singleton())
                .RegisterType<IAzureService, AzureService>(Singleton())
                .RegisterInstance<IScheduler>(new Scheduler())
                .RegisterType<ITimeService, TimeService>(Singleton())
                .RegisterType<IFileSystem, FileSystem>(Singleton())
                .RegisterType<IWebClient, WebClientImpl>() // NOT singleton!
                .RegisterType<IDownloadController, DownloadController>(Singleton())
                .RegisterType<IDialogService, DialogService>()
                .RegisterType<IWindow, WindowWrapper>()
                .RegisterType<IFileDialogService, FileDialogService>();

            return container;
        }
    }
}
