using SampleMVVM.Commands;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;
using SampleMVVM.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PlayList = SampleMVVM.Model.BD.PlayList;

namespace SampleMVVM.ViewModels
{
    public class AddMusicInPlayListViewModel : ViewModelBase
    {
        private PlayList playList;
        private List<Songs> Musics;

        public AddMusicInPlayListViewModel (PlayList playList)
        {
            this.playList = playList;

            Musics = MusicManager.Instance.unitOfWork.PlayListRepositories.GetSongs(playList);
        }

        private void SyncData()
        {
            Musics = MusicManager.Instance.unitOfWork.PlayListRepositories.GetSongs(playList);

            OnPropertyChanged(nameof(ListMusicViewMod));
        }

        #region Отображение


        public string PlayList_Name
        {
            get
            {
                return playList.PlayList_Name;
            }
            set
            {
                playList.PlayList_Name = value;
                OnPropertyChanged(nameof(PlayList_Name));
            }
        }
        public string ImagePath
        {
            get
            {
                return playList.ImagePath;
            }
            set
            {
                playList.ImagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        public string Author
        {
            get
            {
                return playList.Author;
            }
            set
            {
                playList.Author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        public List<Songs> ListMusicViewMod
        {
            get
            {
                return Musics;
            }
            set
            {
                Musics = value;
                OnPropertyChanged(nameof(ListMusicViewMod));
            }
        }
        #endregion

        #region Выбранный элемент
        private Songs _selectedMusic;
        public Songs SelectedMusic
        {
            get { return _selectedMusic; }
            set
            {
                    _selectedMusic = value;
                    OnPropertyChanged(nameof(SelectedMusic));
                    OnPropertyChanged(nameof(CanDeleteItem));
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

        private void deleteMusic(int Id)
        {
            var item = MusicManager.Instance.unitOfWork.SongsRepositories.Get(Id);
            MusicManager.Instance.unitOfWork.PlayListItemRepositories.Delete(
                MusicManager.Instance.unitOfWork.PlayListRepositories.GetItems(playList).FirstOrDefault(i => i.SoungId == item.Id).Id);
            MusicManager.Instance.unitOfWork.Save();
            SyncData();
            ListMusicViewMod = Musics;
            SelectedMusic = null;
        }
        public bool CanDeleteItem
        {
            get
            {
                return SelectedMusic != null;
            }
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
            MusicManager.Instance.selectSongs = _selectedMusic;
            MusicManager.Instance.SongsList = Musics;
            MusicManager.Instance.PlaySong();
        }
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
        private void descriptionMe(int Id)
        {
            var item = MusicManager.Instance.unitOfWork.SongsRepositories.Get(Id);
            MessageBox.Show(item.Title);
        }
        #endregion
        #region Oткрытие окна редактирования
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
        private void OpenNewWindow()
        {
            var newWindow = new PlayListEditionView();
            var newWindowViewModel = new PlayListEditionViewModel(newWindow, playList);
            newWindow.DataContext = newWindowViewModel;
            newWindow.ShowDialog();
        }
        #endregion
    }
}
