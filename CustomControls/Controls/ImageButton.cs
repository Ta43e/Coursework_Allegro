using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;

namespace CustomControls.Controls
{
    [TemplatePart(Name = "img", Type = typeof(Image))]
    public class ImageButton : UserControl
    {
        #region Constructor

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton),
              new FrameworkPropertyMetadata(typeof(ImageButton)));
        } 

        #endregion
        
        #region Fields

        private Image img; 

        #endregion

        #region Property

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public ImageSource ImgSource
        {
            get { return (ImageSource)GetValue(ImgSourceProperty); }
            set { SetValue(ImgSourceProperty, value); }
        } 

        #endregion

        #region Dependency property

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ImageButton),
                new PropertyMetadata(null));


        public static readonly DependencyProperty ImgSourceProperty =
            DependencyProperty.Register("ImgSource", typeof(ImageSource), typeof(ImageButton));

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template != null)
            {
                img = this.Template.FindName("img", this) as Image;
                img.Source = this.ImgSource;
            }
        }

        #endregion
    }
}
