using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace 导航demo.Models.Base
{
    /// <summary>
    /// VM基类 实现通知和资源销毁
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose() { }
    }
}
