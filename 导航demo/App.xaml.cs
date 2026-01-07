using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using 导航demo.Models;
using 导航demo.Services;
using 导航demo.ViewModels;
using 导航demo.Views;

namespace 导航demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 依赖注入容器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            // 导航存储器
            services.AddSingleton<NavigationStore>();

            // 导航服务
            services.AddSingleton<INavigationService>(s => CreateHomeNavigationService(s));

            // 视图模型
            services.AddTransient<HomeViewModel>(s => new HomeViewModel());
            services.AddTransient<HomeViewModel2>(s => new HomeViewModel2());

            services.AddTransient<NavigationBarViewModel>(CreateNavigationBarViewModel);
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// 用于带导航栏的页面
        /// 工厂方法返回一个导航服务，负责切换到 HomeViewModel
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<HomeViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<HomeViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        /// <summary>
        /// 用于带导航栏的页面
        /// 工厂方法返回一个导航服务，负责切换到 HomeViewModel2
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private INavigationService CreateHome2NavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<HomeViewModel2>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<HomeViewModel2>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        /// <summary>
        /// 导航栏 ViewModel
        /// 导航栏包含多个按钮，每个按钮对应一个导航服务 这样用户点击导航栏按钮时，就能切换到不同页面
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                CreateHomeNavigationService(serviceProvider),
                CreateHome2NavigationService(serviceProvider));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
