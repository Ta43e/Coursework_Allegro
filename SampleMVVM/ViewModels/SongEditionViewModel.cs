using Microsoft.Win32;
using SampleMVVM.Commands;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;
using SampleMVVM.Views;
using System.Windows;
using System.Windows.Input;

namespace SampleMVVM.ViewModels
{
    public class SongEditionViewModel : ViewModelBase
    {
        #region Данные
        private Songs music;
        private string description = "";
        private string image;

        private string name;
        private string author;
        private string title;
        private SongEditionView windows;

        public SongEditionViewModel(SongEditionView windows, Songs music)
        {
            this.windows = windows;
            this.music = music;
            name = music.Name;
            author = music.Artist;
            title = music.Title;
            image = music.ImagePath;
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
        public string ErrorTitle
        {
            get => errorTitle;
            set
            {
                errorTitle = value;
                OnPropertyChanged(nameof(ErrorTitle));
            }
        }
        #endregion

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Title");
            }
        }
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
        #endregion
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
            ErrorTitle = "";
            if (Image == null)
            {
                Image = music.ImagePath;       
            }
            else if (Title == "" || Title == null)
            {
                Title = music.Title;
            }
            else if (Name == "" || Name == null)
            {
                Name = music.Name;
            }
            else if (Author == "" || Author == null)
            {
                Author = music.Artist;
            }
            music.Name = Name;
            music.Title = Title;
            music.Artist= Author;
            music.ImagePath= Image;
            MusicManager.Instance.unitOfWork.SongsRepositories.Update(music);
            MusicManager.Instance.unitOfWork.Save();
            MessageBox.Show(Application.Current.FindResource("gut").ToString());
        }
        #endregion
    }
}
