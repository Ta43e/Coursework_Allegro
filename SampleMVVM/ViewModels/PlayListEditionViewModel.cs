using Microsoft.Win32;
using SampleMVVM.Commands;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;
using SampleMVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SampleMVVM.ViewModels
{
    class PlayListEditionViewModel : ViewModelBase
    {
        private PlayListEditionView windows;
        private PlayList playList;
        private string image;
        private string title;
        private string author;
        public PlayListEditionViewModel(PlayListEditionView windows, PlayList playList)
        {
            this.windows = windows;
            this.playList = playList;
            title = playList.PlayList_Name;
            author = playList.Author;
            image = playList.ImagePath;
        }
        #region Error
        private string errorName = string.Empty;
        private string errorArtist = string.Empty;
        private string errorTitle = string.Empty;
        public string ErrorName
        {
            get => errorName;
            set
            {
                errorName = value;
                OnPropertyChanged(nameof(ErrorName));
            }
        }

        public string ErrorArtist
        {
            get => errorArtist;
            set
            {
                errorArtist = value;
                OnPropertyChanged(nameof(ErrorArtist));
            }
        }
        #endregion
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public string Author
        {
            get
            {
                return author;
            }
            set
            {
                author = value;
                OnPropertyChanged("Author");
            }
        }
        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }
        private DelegateCommand closeWindows;
        public ICommand CloseWindows
        {
            get
            {
                if (closeWindows == null)
                {
                    closeWindows = new DelegateCommand(CloseWindow);
                }
                return closeWindows;
            }
        }
        private void CloseWindow()
        {
            if (windows != null)
            {
                windows.Close();
            }
        }
        #region Добавить изображение
        private DelegateCommand loadImageCommand;
        public ICommand LoadImageCommand
        {
            get
            {
                if (loadImageCommand == null)
                {
                    loadImageCommand = new DelegateCommand(LoadImage);
                }
                return loadImageCommand;
            }
        }

        private void LoadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                Image = openFileDialog.FileName;
            }
        }
        #endregion
        #region сохранить изменения
        private DelegateCommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new DelegateCommand(Save);
                }
                return saveCommand;
            }
        }
        private void Save()
        {
            ErrorName = "";
            ErrorArtist = "";
            if (Image == null)
            {
                MessageBox.Show(Application.Current.FindResource("ErrorImage").ToString());
                return;
            }
            else if (Title == "" || Title == null)
            {
                ErrorName = Application.Current.FindResource("ErrorName").ToString();
                return;
            }
            else if (Author == "" || Author == null)
            {
                ErrorArtist = Application.Current.FindResource("ErrorArtist").ToString();
                return;
            }
            playList.PlayList_Name = Title;
            playList.Author = Author;
            playList.ImagePath = Image;
            MusicManager.Instance.unitOfWork.PlayListRepositories.Update(playList);
            MusicManager.Instance.unitOfWork.Save();
            MessageBox.Show(Application.Current.FindResource("gut").ToString());
        }
        #endregion
    }
}
