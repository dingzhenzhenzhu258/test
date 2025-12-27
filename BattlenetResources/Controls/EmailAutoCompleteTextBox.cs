using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BattlenetResources.Controls
{
    // 声明所需的控件和对应名字
    [TemplatePart(Name = "PART_EmailList", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_MyPopup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Placeholder", Type = typeof(TextBlock))]
    public class EmailAutoCompleteTextBox : TextBox
    {
        private ListBox _emailList;
        private Popup _popup;
        private Button _clearButton;
        private TextBlock _placeholder;

        // 依赖属性，用于控制弹出框是否显示
        public static readonly DependencyProperty ShowSuffixListProperty =
            DependencyProperty.Register("ShowSuffixList", typeof(bool),
                typeof(EmailAutoCompleteTextBox), new PropertyMetadata(false));

        public bool ShowSuffixList
        {
            get { return (bool)GetValue(ShowSuffixListProperty); }
            set { SetValue(ShowSuffixListProperty, value); }
        }

        /// <summary>
        /// 静态构造函数 覆盖默认样式的索引键
        /// </summary>
        static EmailAutoCompleteTextBox()
        {
            /*
              DefaultStyleKeyProperty.OverrideMetadata(...)“覆盖默认样式的索引键”。
                默认情况：如果你不写这段代码，EmailAutoCompleteTextBox 继承自 TextBox。WPF 会认为：“既然你没说你有自己的样子，那我就去全局找 TextBox 的样式给你套上。” 结果就是：你的 Popup、ListBox 全都不会显示出来，因为它用的是普通的文本框样式。
                覆盖之后：这段代码告诉系统：“请注意！这个控件的样式索引键不再是 TextBox，而是 EmailAutoCompleteTextBox 本身。”

                typeof(EmailAutoCompleteTextBox)
                这一行指明了查找路径。
                WPF 会去 Themes/Generic.xaml 文件中，寻找 TargetType 正好是 EmailAutoCompleteTextBox 的那段 <Style>
             */

            /*
             一个形象的比喻
                TextBox 是“普通西装”。
                EmailAutoCompleteTextBox 是你设计的“宇航服”，虽然宇航服也是一种衣服（继承自衣服类）。
                如果你不写这段代码，系统会默认给你发一件“普通西装”。
                写了这段代码，就等于在系统后台注册了：“凡是看到 EmailAutoCompleteTextBox 标签，必须去仓库里取那件专门设计的宇航服（Generic.xaml 里的 Style），别发错货。”
             */

            /*
             Themes/Generic.xaml 是 WPF 自定义控件默认的“寻址规范”。

                如果你没有这个文件，你的自定义控件就无法找到它的“外壳”（Template），导致界面显示不出来或者报错。

                1. 为什么是 Themes/Generic.xaml？
                这是 WPF 框架强制约定的路径。当你调用 DefaultStyleKeyProperty.OverrideMetadata 覆盖元数据后，WPF 内部逻辑会按照以下固定流程寻找样式：
                检查程序集属性：它会先看你的控件库项目（Properties/AssemblyInfo.cs）里有没有声明 ThemeInfo 属性。
                定位文件：如果使用了默认配置，WPF 会强制去你项目根目录下的 Themes 文件夹里找名为 Generic.xaml 的文件。
                匹配 Key：在这个文件中寻找 TargetType 匹配的 Style。

                2. 如果你没有这个文件，该怎么办？
                作为控件库开发者，你必须手动创建它。这是标准的开发流程：

                第一步：创建目录和文件
                在你的项目根目录下，新建一个文件夹，命名为 Themes（注意首字母大写，必须是这个名字）。
                在 Themes 文件夹内，新建一个 “资源字典 (Resource Dictionary)”，命名为 Generic.xaml（必须是这个名字）。

                第二步：在 AssemblyInfo.cs 中注册（非常重要）
                打开你项目中的 Properties/AssemblyInfo.cs 文件，确保包含以下代码（通常新建控件库项目时会自动生成，但建议检查）：

                C#
                using System.Windows;

                [assembly: ThemeInfo(
                    ResourceDictionaryLocation.None, // 主题特定资源字典的位置
                    ResourceDictionaryLocation.SourceAssembly // 通用资源字典的位置 (即 Generic.xaml)
                )]
                这段代码的作用是告诉编译器：“我的控件样式就在我自己的程序集（SourceAssembly）里，请去那里找。”

                3. 我能把样式写在别的文件里吗？
                可以，但最终必须汇总到 Generic.xaml。
                随着控件越来越多，Generic.xaml 会变得非常臃肿。专业的做法是：
                为每个控件单独建一个 XAML 文件（例如 EmailTextBoxStyle.xaml）。
                在 Generic.xaml 中使用 MergedDictionaries 把它们合并进来：

                XML
                <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
                    <ResourceDictionary.MergedDictionaries>
                        <ResourceDictionary Source="/YourAssemblyName;component/Styles/EmailTextBoxStyle.xaml" />
                    </ResourceDictionary.MergedDictionaries>
                </ResourceDictionary>

                4. 为什么不能像普通项目那样写在 App.xaml 里？
                App.xaml 是“应用程序”级别的资源。
                Generic.xaml 是“控件库”级别的资源。
                当你把控件库（DLL）发给别人用时，别人的项目里有自己的 App.xaml。WPF 引擎在加载 DLL 里的自定义控件时，只会去该 DLL 的 Themes/Generic.xaml 里找默认样式。如果你写在 App.xaml 里，别人引用你的 DLL 时，样式是带不走的。
             */

            /*
             你是在当前项目的某个页面（窗口）里直接使用，或者你在 App.xaml 中手动引入了那个同级的 XAML 文件，那么样式确实会生效。
                但如果你正在开发的是一个**“控件库项目”（Class Library）**，且目标是生成一个 DLL 供其他项目调用，那么同级的 XAML 就会失效。原因如下：
                没有被编译进资源查找链：WPF 的自定义控件在被跨程序集调用时，其 DefaultStyleKey 只会顺着程序集的 Themes/Generic.xaml 这一条线去找。
                无法自动加载：放在同级的 XAML 除非被显式合并（MergedDictionary），否则它只是一个孤立的资源字典文件，WPF 引擎不会在创建 EmailAutoCompleteTextBox 实例时主动去翻这个文件。
             */
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmailAutoCompleteTextBox),
                new FrameworkPropertyMetadata(typeof(EmailAutoCompleteTextBox)));
        }

        /// <summary>
        /// 为每个特定的文本框绑定功能
        /// </summary>
        public EmailAutoCompleteTextBox()
        {
            // 每创建一个文本框，就给它挂上“文字改变”的监听器
            // 监听文本变化
            this.TextChanged += EmailAutoCompleteTextBox_TextChanged;
        }

        /// <summary>
        /// 这个方法在 XAML 模板（ControlTemplate）成功加载并渲染到界面上的那一刻触发。它晚于构造函数。它早于控件的 Loaded 事件。
        /// “当 UI 皮肤（XAML）贴在 C# 逻辑骨架上时，去把皮肤里的开关、按钮等零件找出来，并接上电（绑定事件）。”
        /// </summary>
        /*
         * 1. 为什么需要它？
           WPF 的自定义控件采用“无外观”设计。
            C# 类：只负责逻辑（比如：点击了要清空、输入了要弹出）。
            XAML(ControlTemplate)：只负责长相（比如：按钮是圆的还是方的）。
            问题是： C# 类在编写时，并不知道未来的 XAML 模板里到底长什么样。OnApplyTemplate 就是两者“正式见面”的时刻。
         */

        /*
         执行时机：为什么不在构造函数里写？
            这是初学者最容易犯的错误。
            构造函数执行时：控件刚被 new 出来，此时还没有关联 XAML 模板，GetTemplateChild 永远返回 null。
            OnApplyTemplate执行时：WPF 引擎已经把 XAML 模板加载并生成了视觉树（Visual Tree）。
            Loaded 事件：比 OnApplyTemplate 更晚。虽然在 Loaded 里也能找控件，但 OnApplyTemplate 是官方推荐的初始化逻辑位置，它能确保在控件第一次显示前一切就绪。
         */
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            /*
                 GetTemplateChild: 这是专门用来在 XAML 模板中寻找 x:Name 匹配控件的方法。
                 为什么要找？: 因为 C# 类不知道 XAML 里有没有这些控件。只有找到了，后续的逻辑（比如弹出、清空）才能操作具体的对象。
             */
            _emailList = GetTemplateChild("PART_EmailList") as ListBox;
            _popup = GetTemplateChild("PART_MyPopup") as Popup;
            _clearButton = GetTemplateChild("PART_ClearButton") as Button;
            _placeholder = GetTemplateChild("PART_Placeholder") as TextBlock;

            // 绑定逻辑
            if (_emailList != null)
            {
                _emailList.SelectionChanged += OnEmailListSelectionChanged;
            }

            // 绑定逻辑
            if (_clearButton != null)
            {
                _clearButton.Click += (s, e) =>
                {
                    this.Clear();
                    this.Focus();
                };
            }

            // 处理失去焦点事件
            this.LostFocus += (s, e) =>
            {
                if (_popup != null && !_popup.IsKeyboardFocusWithin)
                {
                    _popup.IsOpen = false;
                }
            };
        }

        private void EmailAutoCompleteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 有文本时显示清除按钮
            if (_clearButton != null)
            {
                _clearButton.Visibility = string.IsNullOrEmpty(this.Text) ? Visibility.Collapsed : Visibility.Visible;
            }

            // 隐藏占位符
            if (_placeholder != null)
            {
                _placeholder.Visibility = string.IsNullOrEmpty(this.Text) ? Visibility.Visible : Visibility.Collapsed;
            }

            // 如果有文本，检查是否包含@符号
            if (!string.IsNullOrEmpty(this.Text))
            {
                // 检查是否有@符号但没有后缀
                int atIndex = this.Text.IndexOf('@');
                if (atIndex >= 0 && !this.Text.Contains(".") && _popup != null && this.IsFocused)
                {
                    _popup.IsOpen = true;
                }
            }
        }

        /// <summary>
        /// 后缀正确地填入文本框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEmailListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_emailList?.SelectedItem is ListBoxItem item)
            {
                // 拿到选中的那一项内容
                string suffix = item.Content.ToString();
                string currentText = this.Text;

                // 智能拼接逻辑
                int atIndex = currentText.IndexOf('@');
                if (atIndex >= 0)
                {
                    // 如果已经有@，替换@之后的部分
                    this.Text = currentText.Substring(0, atIndex) + suffix;
                }
                else
                {
                    // 如果没有@，直接追加
                    this.Text = currentText + suffix;
                }

                this.SelectionStart = this.Text.Length;

                if (_popup != null)
                {
                    _popup.IsOpen = false;
                }

                _emailList.SelectedIndex = -1;
                this.Focus();
            }
        }

        /// <summary>
        /// 通过键盘上的 “向下方向键” 快速进入选择模式，或通过 “Esc 键” 快速退出选择模式
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (_popup != null && !_popup.IsOpen)
                {
                    _popup.IsOpen = true;
                    if (_emailList != null && _emailList.Items.Count > 0)
                    {
                        _emailList.SelectedIndex = 0;
                        _emailList.Focus();
                    }
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Escape)
            {
                if (_popup != null && _popup.IsOpen)
                {
                    _popup.IsOpen = false;
                    e.Handled = true;
                }
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// 当用户点击或通过 Tab 键切换回这个文本框时，自动恢复（重新弹出）补全列表
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (!string.IsNullOrEmpty(this.Text) && _popup != null)
            {
                int atIndex = this.Text.IndexOf('@');
                if (atIndex >= 0 && !this.Text.Contains("."))
                {
                    _popup.IsOpen = true;
                }
            }
        }
    }
}
