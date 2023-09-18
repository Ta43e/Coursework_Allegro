using System;
using System.Windows;
using System.Windows.Controls;

namespace CustomControls.Controls
{
    [TemplatePart(Name = "textBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "errorTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "passwordBox", Type = typeof(PasswordBox))]
    public class BindablePasswordBox : UserControl
    {
        #region Constructor

        static BindablePasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(typeof(BindablePasswordBox)));
        }

        #endregion

        #region Property

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public string NameBox
        {
            get { return (string)GetValue(NameBoxProperty); }
            set { SetValue(NameBoxProperty, value); }
        }

        public string MessageError
        {
            get { return (string)GetValue(MessageErrorProperty); }
            set { SetValue(MessageErrorProperty, value); }
        }

        #endregion

        #region Dependecy property

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty NameBoxProperty =
           DependencyProperty.Register("NameBox", typeof(string), typeof(BindablePasswordBox),
               new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty MessageErrorProperty =
           DependencyProperty.Register("MessageError", typeof(string), typeof(BindablePasswordBox),
               new PropertyMetadata(string.Empty));

        #endregion

        #region Fields

        private TextBlock? textBlock;
        private TextBox? errorTextBox;
        private PasswordBox? passwordBox;

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template != null)
            {
                passwordBox = this.Template.FindName("passwordBox", this) as PasswordBox;
                errorTextBox = this.Template.FindName("errorTextBox", this) as TextBox;
                textBlock = this.Template.FindName("textBlock", this) as TextBlock;
                
                passwordBox.PasswordChanged += PasswordBoxPasswordChanged;
                errorTextBox.TextChanged += ErrorTextBoxChanged;
                MessageError = errorTextBox.Text;
                textBlock.Text = NameBox;

            }
        }

        private void ErrorTextBoxChanged(object sender, TextChangedEventArgs e)
        {
            if ((string)GetValue(MessageErrorProperty) != string.Empty)
            {
                errorTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                errorTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = passwordBox.Password;
        }

        #endregion
    }
}
