using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BattlenetResources.Controls
{
    public class VerificationTextBox : TextBox
    {
        static VerificationTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerificationTextBox),
    new FrameworkPropertyMetadata(typeof(VerificationTextBox)));
        }
    }
}
