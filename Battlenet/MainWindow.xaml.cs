using BattlenetResources.Controls;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Battlenet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 3秒钟 后显示登录窗口

            InitialzeApp();
        }

        private async void InitialzeApp()
        {
            await Task.Delay(1);
            new Thread(() => {
                Thread.Sleep(3000); 
                this.Dispatcher.Invoke(() =>
                {
                    win.Height = 599;
                    ContentShow.Content = new Register();
                });
            }).Start();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // 关闭当前窗口
            this.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            // 将窗口状态设置为最小化
            this.WindowState = WindowState.Minimized;
        }

    }
}