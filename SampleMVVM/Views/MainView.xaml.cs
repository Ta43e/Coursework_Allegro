using SampleMVVM.Managers;
using SampleMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SampleMVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                btnFullScrin.Content = "▢";
            }
            else
            {
                WindowState = WindowState.Maximized;
                btnFullScrin.Content = "❐";
            }
        }

        private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            // Выполняйте необходимые действия при начале перетаскивания ползунка
            // Например, приостановите воспроизведение песни
            MusicManager.Instance.Pause();
        }

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            // Выполняйте необходимые действия при завершении перетаскивания ползунка
            // Например, установите позицию воспроизведения песни
            MusicManager.Instance.PlayingAfterPause();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slider.Value = e.NewValue;

            if (e.NewValue >= 0.999)
            {
                MusicManager.Instance.PlayNext();
                //MusicManager.Instance.Play();
            }
        }
    }
}
