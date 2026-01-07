using System;
using System.Collections.Generic;
using System.Text;
using 导航demo.Models.Base;

namespace 导航demo.Services
{
    /// <summary>
    /// 导航命令
    /// </summary>
    public class NavigateCommand : CommandBase
    {
        /// <summary>
        /// 它不关心具体要跳到哪个页面（是首页还是设置页），它只持有一个“导航服务”的引用
        /// </summary>
        private readonly INavigationService _navigationService;

        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        /// 执行导航服务的 Navigate() 方法
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
