using SampleMVVM.Commands;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;
using SampleMVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SampleMVVM.ViewModels
{
    public class UserListMusicViewModel : ViewModelBase
    {
        #region Отображение
        private List<Songs> songsList = MusicManager.Instance.unitOfWork.AdminListRepositories.GetAllSongs().ToList();
        private ApplicationContext db = new ApplicationContext();
        public List<Songs> ListMusicShow
        {
            get => MusicManager.Instance.unitOfWork.UserListRepositories.GetSongs(CurrentUser);
            set
            {
                if (songsList != value)
                {
                    OnPropertyChanged(nameof(ListMusicShow));
                }
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
            var item = songsList.Find(x => x.Id == id);
            MusicManager.Instance.unitOfWork.UserListMusicRepositories.Delete(
            MusicManager.Instance.unitOfWork.UserListRepositories.GetItems(CurrentUser).FirstOrDefault(i => i.SoungId == item.Id).Id);
            MusicManager.Instance.unitOfWork.Save();
            ListMusicShow = MusicManager.Instance.unitOfWork.UserListRepositories.GetSongs(CurrentUser);
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
            var item = songsList.Find(x => x.Id == id);
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
        private void OpenSelectPlaylistT(int id)
        {
            var item = songsList.Find(x => x.Id == id);
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
            var item = songsList.Find(x => x.Id == id);
            MessageBox.Show(item.Title);
        }

        #endregion
    }
}
