
using SampleMVVM.Commands;
using SampleMVVM.DataBase;
using SampleMVVM.DataBase.UnitOfWorks;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;
using SampleMVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SampleMVVM.ViewModels
{
    public class ListMusicPageModel : ViewModelBase
    {
        #region Отображение
        private List<Songs> songsList = MusicManager.Instance.unitOfWork.AdminListRepositories.GetAllSongs().ToList();
        private ApplicationContext db = new ApplicationContext();
        private bool isAdmin = false;
        private bool isUser = true;
        public ListMusicPageModel()
        {
            isAdmin = IsAdmin;
            isUser = !isAdmin;
        }
        public  List<Songs> ListMusicShow
        {
            get 
             {
                if (searchString == null || searchString == "") 
                {
                    return songsList = MusicManager.Instance.unitOfWork.AdminListRepositories.GetAllSongs().ToList();
                }
                return songsList; 
            }
            set
            {
                if (songsList != value)
                {
                    songsList = value;
                    OnPropertyChanged(nameof(ListMusicShow));
                }
            }
        }

        public bool CheckAdmin
        {
            get
            {
                return isAdmin;
            }
        }
        public bool CheckUser
        {
            get
            {
                return isUser;
            }
        }
        #region Поиск
        #endregion

        #endregion

        #region Выбранный элемент

        private Songs? _selectedSongs;
        public Songs SelectedSongs
        {
            get { return _selectedSongs; }
            set
            {
                if (_selectedSongs != value)
                {
                    _selectedSongs = value;
                    OnPropertyChanged("SelectedMusic");
                    OnPropertyChanged("CanDeleteItem");
                }
            }
        }

        #endregion
        #region Удаление
        private DelegateCommand<int>? deleteCommand;
        public ICommand Delete
        {
            get
            {
                if (deleteCommand == null)
                {

                    deleteCommand = new DelegateCommand<int>(deleteMusic);
                }
                return deleteCommand;
            }
        }

        private void deleteMusic(int id)
        {
            var item = MusicManager.Instance.unitOfWork.SongsRepositories.Get(id);
            MusicManager.Instance.unitOfWork.AdminListRepositories.Delete(item);
            MusicManager.Instance.unitOfWork.Save();
            ListMusicShow = MusicManager.Instance.unitOfWork.AdminListRepositories.GetAllSongs().ToList();
            SelectedSongs = null;
        }


        public bool CanDeleteItem
        {
            get
            {
                return SelectedSongs != null;
            }
        }
        #endregion
        #region Oткрытие нового окна
        private DelegateCommand<int>? openNewWindows;

        public ICommand IOpenNewWindows
        {
            get
            {
                if (openNewWindows == null)
                {
                    openNewWindows = new DelegateCommand<int>(OpenNewWindow);
                }
                return openNewWindows;
            }
        }
        private void OpenNewWindow(int id)
        {

                var item = MusicManager.Instance.unitOfWork.SongsRepositories.Get(id);
                var newWindow = new SongEditionView();
                var newWindowViewModel = new SongEditionViewModel(newWindow, item);
                newWindow.DataContext = newWindowViewModel;
                newWindow.ShowDialog();

        }
        #endregion
        #region Выбор плейлиста
        private DelegateCommand<int>? openSelectPlaylistToAddViewModel;

        public ICommand OpenSelectPlaylistToAddViewModel
        {
            get
            {
                if (openSelectPlaylistToAddViewModel == null)
                {
                    openSelectPlaylistToAddViewModel = new DelegateCommand<int>(OpenSelectPlaylistT);
                }
                return openSelectPlaylistToAddViewModel;
            }
        }
        private  void OpenSelectPlaylistT(int id)
        {
            var item = MusicManager.Instance.unitOfWork.SongsRepositories.Get(id);
            var newWindow = new SelectPlaylistToAddView();

            var newWindowViewModel = new SelectPlaylistToAddViewModel(newWindow, item);
            newWindow.DataContext = newWindowViewModel;
            newWindow.ShowDialog();
        }



        private DelegateCommand doubleClickCommand;
        public ICommand DoubleClickCommand
        {
            get
            {
                if (doubleClickCommand == null)
                {

                    doubleClickCommand = new DelegateCommand(OnDoubleClick);
                }
                return doubleClickCommand;
            }
        }


        private void OnDoubleClick()
        {
            MusicManager.Instance.selectSongs = SelectedSongs;
            MusicManager.Instance.SongsList = ListMusicShow;
            MusicManager.Instance.PlaySong();
        }
        #endregion
        #region Описание
        private DelegateCommand<int>? description;

        public ICommand Description
        {
            get
            {
                if (description == null)
                {
                    description = new DelegateCommand<int>(descriptionMe);
                }
                return description;
            }
        }
        private void descriptionMe(int id)
        {
            var item = MusicManager.Instance.unitOfWork.SongsRepositories.Get(id);
            MessageBox.Show(item.Title);
        }

        #endregion
        #region Добавление псни в список музыки пользователя
        private DelegateCommand<int>? addMusicInUserList;

        public ICommand AddMusicInUserList
        {
            get
            {
                if (addMusicInUserList == null)
                {
                    addMusicInUserList = new DelegateCommand<int>(AddSongToUserList);
                }
                return addMusicInUserList;
            }
        }
        private void AddSongToUserList(int id)
        {
            var item = MusicManager.Instance.unitOfWork.SongsRepositories.Get(id);
            if (MusicManager.Instance.unitOfWork.UserListRepositories.AddSongs(item, CurrentUser))
            {

                MessageBox.Show(Application.Current.FindResource("gut").ToString());
                MusicManager.Instance.unitOfWork.Save();
                songsList = MusicManager.Instance.unitOfWork.AdminListRepositories.GetAllSongs().ToList();
                OnPropertyChanged(nameof(ListMusicShow));
            }
        }
        #endregion
        #region Поиск песни

        private DelegateCommand suche;
        public ICommand Suche
        {
            get
            {
                if (suche == null)
                {

                    suche = new DelegateCommand(sucheMethod);
                }
                return suche;
            }
        }


        private void sucheMethod()
        {
            if (string.IsNullOrEmpty(searchString))
            {
                ListMusicShow = MusicManager.Instance.unitOfWork.AdminListRepositories.GetAllSongs().ToList();
                return;
            }

            var currentSongs = db.Songs.ToList();
            var songs = new List<Songs>();
            foreach (var item in currentSongs)
            {
                if (item.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    songs.Add(item);
                }
            }
            songsList = songs;
            OnPropertyChanged(nameof(ListMusicShow));
        }


        private string searchString = "";
        public string SearchString
        {
            get
            {
                return searchString;
            }
            set
            {
                searchString = value;
                OnPropertyChanged(nameof(SearchString));
                sucheMethod();
            }
        }
        #endregion
    }
}
