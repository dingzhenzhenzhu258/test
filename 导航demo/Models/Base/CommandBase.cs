using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace 导航demo.Models.Base
{
    /// <summary>
    /// 命令抽象基类
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter) => true;

        public abstract void Execute(object parameter);

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
