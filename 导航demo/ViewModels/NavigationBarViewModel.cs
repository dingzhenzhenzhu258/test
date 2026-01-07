using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using 导航demo.Models.Base;
using 导航demo.Services;

namespace 导航demo.ViewModels
{
    /// <summary>
    /// 顶部导航栏VM
    /// </summary>
    public class NavigationBarViewModel : ViewModelBase
    {
        #region 路由控制 导航到不同的窗口 NavigationBarViewModel 并不直接操作 NavigationStore，而是通过 INavigationService 接口来触发跳转
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        #endregion


        public NavigationBarViewModel(
            INavigationService homeNavigationService,
            INavigationService accountNavigationService)
        {
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
        }
    }
}
