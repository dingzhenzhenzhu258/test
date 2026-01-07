using System;
using System.Collections.Generic;
using System.Text;
using 导航demo.Models.Base;

namespace 导航demo.ViewModels
{
    /// <summary>
    /// 页面容器
    /// 把 导航栏 (NavigationBarViewModel) 和 内容区页面 (ContentViewModel) 组合在一起。
    /// </summary>
    public class LayoutViewModel : ViewModelBase
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public ViewModelBase ContentViewModel { get; }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
        }

        public override void Dispose()
        {
            NavigationBarViewModel.Dispose(); // 释放导航栏（退订账号改变事件）
            ContentViewModel.Dispose();       // 释放当前页面资源

            base.Dispose();
        }
    }
}
