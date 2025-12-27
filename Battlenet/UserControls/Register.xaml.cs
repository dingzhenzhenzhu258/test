using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Battlenet.UserControls
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : UserControl
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            // 获取协议网址
            string url = "https://legal.battlenet.com.cn/legal/2e90163d-612f-41a5-addd-8a837ac43d02";

            // 调用系统浏览器打开
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // 必须设置为 true 才能在 .NET Core/5+ 中生效
            });
        }

        // 暴露一个公共属性，内部指向按钮的 Content
        public Button LoginButton
        {
            get => this.RegisterButton;
        }
    }
}
