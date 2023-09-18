using Microsoft.Win32;
using SampleMVVM.Commands;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;
using SampleMVVM.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SampleMVVM.ViewModels
{
    internal class SelectPlaylistToAddViewModel
 : ViewModelBase
    {

        private ImageSource Image;
        private readonly Users users;
        private List<PlayList> playList;
        private SelectPlaylistToAddView windows;
        private Songs songs;
        public SelectPlaylistToAddViewModel(SelectPlaylistToAddView windows, Songs songs)
        {
            this.songs = songs;
            this.windows = windows;
            users = CurrentUser;
            playList = MusicManager.Instance.unitOfWork.PlayListRepositories.Get(users).ToList();
        }
        public List<PlayList> PlayList
        {
            get
            {
                return playList;
            }
            set
            {
                playList = value;
                OnPropertyChanged("PlayList");
            }
        }
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
                    OnPropertyChanged("CanDeleteItem");
                }
            }
        }

        #endregion
        #region Добавление песни в плейлист
        private DelegateCommand<int>? addSongsInPlayList;

        public ICommand AddSongsInPlayList
        {
            get
            {
                if (addSongsInPlayList == null)
                {
                    addSongsInPlayList = new DelegateCommand<int>(AddSongsInPlayListMethod);
                }
                return addSongsInPlayList;
            }
        }
        private  void AddSongsInPlayListMethod(int Id)
        {
            var item = MusicManager.Instance.unitOfWork.PlayListRepositories.Get(Id);
            MusicManager.Instance.unitOfWork.PlayListRepositories.AddMusic(item, songs);
            MusicManager.Instance.unitOfWork.Save();
            MessageBox.Show(Application.Current.FindResource("gut").ToString());
        }
        #endregion
        #region test

        public bool CanDeleteItem
        {
            get
            {
                return SelectedPlayList != null;
            }
        }
        #endregion
    }
}

