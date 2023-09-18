
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WMPLib;

namespace SampleMVVM.ViewModels
{
    public class FormToAddViewModel : ViewModelBase
    {
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }


        #region Errors
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
        public string Image { get; set; }
        public string MP3FilePath { get; set; }

        private FormToAddView windows;
        private DelegateCommand createItem;
        public FormToAddViewModel(FormToAddView windows)
        {
            this.windows = windows;
        }
        public ICommand CreateItem
        {
            get
            {
                if (createItem == null)
                {
                    createItem = new DelegateCommand(AddMus);
                }
                return createItem;
            }
        }
        private void AddMus()
        {
            ErrorName = "";
            ErrorArtist = "";
            ErrorTitle = "";
            if (Image == null)
            {
                MessageBox.Show(Application.Current.FindResource("ErrorImage").ToString());
                return;
            }
            else if (MP3FilePath == null)
            {
                MessageBox.Show(Application.Current.FindResource("ErrorSong").ToString());
                return;
            }
            else if (Text1 == "" || Text1 == null)
            {
                ErrorName = Application.Current.FindResource("ErrorName").ToString();
                return;
            }
            else if (Text2 == "" || Text2 == null)
            {
                ErrorArtist = Application.Current.FindResource("ErrorArtist").ToString();
                return;
            }
            else if (Text3 == "" || Text3 == null)
            {
                ErrorTitle = Application.Current.FindResource("ErrorTitle").ToString();
                return;
            }
            Songs songs = new Songs();
            songs.Name = Text1;
            songs.Artist = Text2;
            songs.Title = Text3;
            songs.ImagePath = Image;
            songs.SongsPath = MP3FilePath;
            TimeSpan timeSpan = TimeSpan.FromSeconds(GetAudioDuration(songs.SongsPath));
            songs.Duration = timeSpan.ToString("m\\:ss");
            MusicManager.Instance.unitOfWork.SongsRepositories.Create(songs);
            MusicManager.Instance.unitOfWork.Save();

            MessageBox.Show(Application.Current.FindResource("gut").ToString());
        }

        public static double GetAudioDuration(string filePath)
        {
            WindowsMediaPlayer player = new WindowsMediaPlayer();
            IWMPMedia media = player.newMedia(filePath);

            return media.duration;
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
        private DelegateCommand loadMP3Command;

        public ICommand LoadMP3Command
        {
            get
            {
                if (loadMP3Command == null)
                {
                    loadMP3Command = new DelegateCommand(LoadMP3);
                }
                return loadMP3Command;
            }
        }

        private void LoadMP3()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                MP3FilePath = openFileDialog.FileName;
            }
        }
        #endregion
    }
}
