
using Microsoft.Win32;
using SampleMVVM.Commands;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;

using SampleMVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;


namespace SampleMVVM.ViewModels
{
    public class AddListMusicViewModel : ViewModelBase
    {
        private readonly Users users;
        private List<PlayList> playList;
        public AddListMusicViewModel()
        {
            users = CurrentUser;
            playList = MusicManager.Instance.unitOfWork.PlayListRepositories.Get(users).ToList();
        }
        public List<PlayList> PlayList
        {
            get
            {
                return MusicManager.Instance.unitOfWork.PlayListRepositories.Get(users).ToList();
            }
            set
            {
                    playList = value;
                    OnPropertyChanged("PlayList");
            }
        }
        #region
        public Frame CurrentFrame
        {
            set
            {
                MainView.MainFrame.Content = value;
                OnPropertyChanged(nameof(CurrentFrame));
            }
        }
        #endregion
        #region Выбранный элемент
        private PlayList _selectedMusic;
        public PlayList SelectedPlayList
        {
            get { return _selectedMusic; }
            set
            {
                if (_selectedMusic != value)
                {
                    _selectedMusic = value;
                    OnPropertyChanged("SelectedPlayList");
                }
            }
        }
        #endregion
        #region Oткрытие нового окна
        private DelegateCommand openNewWindows;

        public ICommand IOpenNewWindows
        {
            get
            {
                if (openNewWindows == null)
                {
                    openNewWindows = new DelegateCommand(OpenNewWindow);
                }
                return openNewWindows;
            }
        }
        private static void OpenNewWindow()
        {
            var newWindow = new NewPlayListView();

            var newWindowViewModel = new NewPlayListViewModel(newWindow);
            newWindow.DataContext = newWindowViewModel;
            newWindow.ShowDialog();
        }
        #endregion
        #region Открытие фрейма со списком песен
        private DelegateCommand openNewFrameListMusicAdd;
        public ICommand OpenNewFrameListMusicAdd
        {
            get
            {
                if (openNewFrameListMusicAdd == null)
                {
                    openNewFrameListMusicAdd = new DelegateCommand(OpenNewFrameListMusicAddMe);
                }
                return openNewFrameListMusicAdd;
            }
        }

        private void OpenNewFrameListMusicAddMe()
        {
            if (SelectedPlayList == null)
            {
                return;
            }
            var newContext = new AddMusicInPlayListViewModel(SelectedPlayList);
            CurrentFrame = new Frame() { Content = new AddMusicInPlayListView() { DataContext = newContext } };
        }
        #endregion
        #region удаление плей листа

        #region Удаление
        private DelegateCommand deleteCommand;
        public ICommand Delete
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new DelegateCommand(deleteMusic);
                }
                return deleteCommand;
            }
        }

        private void deleteMusic()
        {
            if (SelectedPlayList == null) { return; }
            MusicManager.Instance.unitOfWork.PlayListRepositories.Delete(SelectedPlayList.Id);
            MusicManager.Instance.unitOfWork.Save();
            PlayList = MusicManager.Instance.unitOfWork.PlayListRepositories.Get(users).ToList();
            SelectedPlayList = null;
        }
        public bool CanDeleteItem
        {
            get
            {
                return SelectedPlayList != null;
            }
        }
        #endregion

        #endregion
    }
}
