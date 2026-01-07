using System;
using System.Collections.Generic;
using System.Text;
using 导航demo.Models;
using 导航demo.Models.Base;
using 导航demo.ViewModels;

namespace 导航demo.Services
{
    /*
        在一般的导航中，我们直接跳转到 HomeViewModel 或 LoginViewModel。但在有导航栏的应用中，我们不希望每个页面都手动去写一遍导航栏的代码。
           这个服务的作用就是：
           取零件 A：生产一个 NavigationBarViewModel（顶部菜单）。
           取零件 B：生产一个 TViewModel（比如具体的首页或账户页）。
           封装：把它们装进 LayoutViewModel 这个壳子里。
           交付：告诉 NavigationStore：“这是拼好的完整界面，拿去显示吧。”
        */
    /// <summary>
    /// 当用户点击导航按钮时，它负责把“通用的导航栏”和“具体的页面内容”像拼乐高一样组装好，然后交给系统去显示
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        // 使用 Func 意味着只有在执行 Maps() 的那一刻，才会调用方法去创建一个新的页面实例。这保证了你每次点开页面，看到的数据和状态都是最新的
        private readonly Func<TViewModel> _createViewModel;

        private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;

        public LayoutNavigationService(NavigationStore navigationStore,
            Func<TViewModel> createViewModel,
            Func<NavigationBarViewModel> createNavigationBarViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _createNavigationBarViewModel = createNavigationBarViewModel;
        }

        public void Navigate()
        {
            // 1. 调用委托创建导航栏
            var navBar = _createNavigationBarViewModel();

            // 2. 调用委托创建具体的页面内容（如 HomeViewModel）
            var content = _createViewModel();

            // 3. 组装成一个 Layout，并更新全局存储
            _navigationStore.CurrentViewModel = new LayoutViewModel(navBar, content);
        }
    }
}
