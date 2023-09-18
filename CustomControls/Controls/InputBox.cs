using System.Windows;
using System.Windows.Controls;

namespace CustomControls.Controls
{
    [TemplatePart(Name = "textBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "textBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "errorTextBox", Type = typeof(TextBox))]
    public class InputBox : Control
    {
        #region Constructor

        static InputBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputBox),
                new FrameworkPropertyMetadata(typeof(InputBox)));
        }

        #endregion

        #region Fields

        private TextBox? textBox;
        private TextBlock? textBlock;
        private TextBox? errorTextBox;

        #endregion

        #region Dependency property

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(InputBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(InputBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty NameBoxProperty =
           DependencyProperty.Register("NameBox", typeof(string), typeof(InputBox),
               new PropertyMetadata(string.Empty));

        #endregion

        #region Property

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        public string NameBox
        {
            get { return (string)GetValue(NameBoxProperty); }
            set { SetValue(NameBoxProperty, value); }
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.Template != null)
            {
                textBox = this.Template.FindName("textBox", this) as TextBox;
                textBlock = this.Template.FindName("textBlock", this) as TextBlock;
                errorTextBox = this.Template.FindName("errorTextBox", this) as TextBox;

                textBlock.Text = NameBox;
                ErrorMessage = errorTextBox.Text;
                textBox.TextChanged += InputTextChanged;
                errorTextBox.TextChanged += ErrorTextBoxChanged;
            }
        }

        private void ErrorTextBoxChanged(object sender, TextChangedEventArgs e)
        {
            if ((string)GetValue(ErrorMessageProperty) != string.Empty)
            {
                errorTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                errorTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void InputTextChanged(object sender, TextChangedEventArgs e)
        {
            InputText = textBox.Text;
        }

        #endregion
    }
}
