using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BattlenetResources.Controls
{
    [TemplatePart(Name ="PART_ClearButton",Type = typeof(Button))]
    public class PhoneNumberTextBox : TextBox
    {
        static PhoneNumberTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PhoneNumberTextBox),
                    new FrameworkPropertyMetadata(typeof(PhoneNumberTextBox)));
        }

        // 声明一个变量来持有按钮引用
        private Button _clearButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // 1. 移除旧按钮的事件订阅（防止内存泄漏）
            if (_clearButton != null)
            {
                _clearButton.Click -= ClearButton_Click;
            }

            // 2. 从模板中寻找名为 PART_ClearButton 的按钮
            _clearButton = GetTemplateChild("PART_ClearButton") as Button;

            // 3. 挂载点击事件
            if (_clearButton != null)
            {
                _clearButton.Click += ClearButton_Click;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // 4. 执行清空逻辑
            this.Text = string.Empty;
            // 清空后让输入框重新获得焦点
            this.Focus();
        }
    }
}
