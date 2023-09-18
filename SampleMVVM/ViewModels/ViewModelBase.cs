using SampleMVVM.DataBase;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;
using SampleMVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SampleMVVM.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Fields

        protected static ApplicationContext db = new();
        protected static Users? CurrentUser = new();
        
        protected static Admin? CurrentAdmin = new();
        public static MainView? MainView = new MainView();
        private static Frame? MainFrame;
        private static bool isAdmin = false;
        private static bool isUser = false;
        private static List<Songs> songs = new(db.GetSongList());
        public bool IsAdmin { get { return isAdmin; } set { isAdmin = value; OnPropertyChanged(nameof(IsAdmin)); } }
        public bool IsUser { get { return isUser; } set { isUser = value; OnPropertyChanged(nameof(IsUser)); } }

        #endregion
        protected static void ShowMainWindow()
        {
            MainView view = new MainView();
            MainView = view;
            MainFrame = view.MainFrame;
            view.Show();
        }
        protected static void CloseMainView()
        {
            MusicManager.Instance.Pause();
            MainView.Close();
        }
        protected static void ShowPage(Page page)
        {
            MainFrame.Content = page;
        }
        protected void ShowWindow(Window window)
        {
            window.Show();
        }
    }
}
