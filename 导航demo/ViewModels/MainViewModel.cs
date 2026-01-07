using System;
using System.Collections.Generic;
using System.Text;
using 导航demo.Models;
using 导航demo.Models.Base;

namespace 导航demo.ViewModels
{
    /// <summary>
    /// 主页面视图模型
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// 保存当前主内容区的 ViewModel
        /// </summary>
        private readonly NavigationStore _navigationStore;

        /// <summary>
        /// 当前主内容区显示的页面（绑定到 MainWindow.xaml  的 ContentControl
        /// </summary>
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        /// <summary>
        /// 当主导航切换时，通知 UI 更新 CurrentViewModel
        /// </summary>
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
