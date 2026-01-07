using System;
using System.Collections.Generic;
using System.Text;
using 导航demo.Models.Base;

namespace 导航demo.Models
{
    /*
     在 MVVM 架构中，我们通常不直接操作 UI（比如 Window.Content = new Page()），而是通过切换 ViewModel 来让 UI 自动更新
        这个 NavigationStore 就是那个“大管家”，它负责：
        持有当前页面：CurrentViewModel 属性保存了现在屏幕上正在显示的视图模型
        发布通知：当页面发生变化（set 被调用）时，它会触发 CurrentViewModelChanged 事件
        内存管理：在切换走之前，它会调用旧页面的 Dispose()，防止内存泄漏

    它在架构中处于什么位置？
        你可以把它想象成一个中转站
        MainWindowViewModel（主窗体）：它会订阅（Listen）这个 NavigationStore 的事件,一旦 CurrentViewModel 变了，主窗体就刷新自己的界面
        各种子页面（如 LoginViewModel）：它们不直接控制主窗体，而是通过修改 NavigationStore 里的值来告诉程序：“嘿，我要换成主页了”
     */

    /// <summary>
    /// 导航存储器
    /// 它保存着当前正在显示的页面（ViewModel），并负责在切换页面时通知主窗口进行更新
    /// </summary>
    public class NavigationStore
    {
        // 这意味着 NavigationStore 不关心具体是哪个页面，只要是 ViewModel 就能放进去
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        /*
           观察者模式
            主窗口通常会在构造函数里写： 
                _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged; 这样主窗口就能知道什么时候该切 UI 了
         */
        public event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
