using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SampleMVVM.Commands;
using SampleMVVM.Managers;
using SampleMVVM.Views;
using SampleMVVM.DataBase;
using System.Diagnostics;
using System.Windows.Media;
using NAudio.Wave;
using System.Threading.Tasks;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Threading;
using WMPLib;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;

namespace SampleMVVM.ViewModels
{
    class MainViewModel  : ViewModelBase
    {
        private static bool chek = true;
        private static bool chekLNG = true;
        private static MainViewModel thisMein;
        ApplicationContext db = new ApplicationContext();

        private ListMusicPageModel _listMusicPage;
        private AddListMusicViewModel _addListMusicViewModel;
        private UserListMusicViewModel _userListMusicViewModel;
        private bool isAdmin;
        private bool isUser;
        private bool checkPause = false;
        private bool checkPlay = true;
        private bool checkClose = false;
        public MainViewModel()
        {
            _listMusicPage = new ListMusicPageModel();
            _addListMusicViewModel = new AddListMusicViewModel();
            _userListMusicViewModel = new UserListMusicViewModel();
            isAdmin = IsAdmin;
            isUser = IsUser;
            thisMein = this;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        public bool CheckPause
        {
            get
            {
                return checkPause;
            }
            set
            {
                checkPause = value;
                OnPropertyChanged(nameof(CheckPause));
            }
        }
        public bool CheckPlay
        {
            get
            {
                return checkPlay;
            }
            set
            {
                checkPlay = value;
                OnPropertyChanged(nameof(checkPlay));
            }
        }
        public static MainViewModel ThisMein
        {
            get { return thisMein; }
        }

        public Frame CurrentFrame
        {
            set
            {
                MainView.MainFrame.Content = value;
                OnPropertyChanged(nameof(CurrentFrame));
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


        private DelegateCommand giveItemCommand;


        public ICommand SetTheme
        {
            get
            {
                if (giveItemCommand == null)
                {
                    giveItemCommand = new DelegateCommand(setTheme);
                }
                return giveItemCommand;
            }
        }
        #region Смена стилей
        private static void setTheme()
        {
            if (chek)
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri( String.Format($"/ResiurceDictionary/light.xaml"), UriKind.Relative)
                }
                );
                chek = false;
                return;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(
                    String.Format($"/ResiurceDictionary/dark.xaml"),
                    UriKind.Relative)
                }
            );
                chek = true;
                return;
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
            var newWindow = new FormToAddView();

            var newWindowViewModel = new FormToAddViewModel(newWindow);
            newWindow.DataContext = newWindowViewModel;
            newWindow.ShowDialog();
        }
        //---//
        #endregion
        #region Открытие фрейма со списком песен
        private DelegateCommand openNewFrameListMusic;
        public ICommand OpenNewFrameListMusic1
        {
            get
            {
                if (openNewFrameListMusic == null)
                {
                    openNewFrameListMusic = new DelegateCommand(OpenNewFrameListMusicMe);
                }
                return openNewFrameListMusic;
            }
        }

        private void OpenNewFrameListMusicMe()

        {
            CurrentFrame = new Frame() { Content = new ListMusicPage() { DataContext = _listMusicPage } };
        }
        #region Открытие фрейма со списком песен пользователя
        private DelegateCommand openNewFrameUserListMusic;
        public ICommand OpenNewFrameUserListMusic
        {
            get
            {
                if (openNewFrameUserListMusic == null)
                {
                    openNewFrameUserListMusic = new DelegateCommand(OpenNewFrameUserListMusicMethod);
                }
                return openNewFrameUserListMusic;
            }
        }

        private void OpenNewFrameUserListMusicMethod()

        {
            CurrentFrame = new Frame() { Content = new UserListMusicView() { DataContext = _userListMusicViewModel } };
        }
        #endregion
        //-----------------------------------//
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
            CurrentFrame = new Frame() { Content = new AddListMusicView() { DataContext = _addListMusicViewModel } };
        }
        #endregion


        #region Управление песней

        #region Линия песни
        //----//
        private DispatcherTimer timer;

        public double CurrentPosition
        {
            get { return  MusicManager.Instance.GetCurrentPosition() / SongDuration; }
            set
            {
                MusicManager.Instance.SetPosition(SongDuration * value);
                OnPropertyChanged(nameof(CurrentPosition));
            }
        }

        public double SongDuration { get => MusicManager.Instance.GetSongDuration(); }

        public bool IsPlaying => MusicManager.Instance.IsPlaying();


            private void Timer_Tick(object sender, EventArgs e)
            {
                OnPropertyChanged(nameof(CurrentPosition));
            }


        private DelegateCommand<double> moveSlider;
        public ICommand MoveSliderCommand
        {
            get
            {
                if (moveSlider == null)
                {
                    moveSlider = new DelegateCommand<double>(position =>
                    {
                        CurrentPosition = position;
                    });
                }
                return moveSlider;
            }
        }
        private ICommand setPositionCommand;
        public ICommand SetPositionCommand => setPositionCommand ?? (setPositionCommand = new RelayCommand<double>(SetPosition));

        private void SetPosition(double newPosition)
        {
            MusicManager.Instance.SetPosition(newPosition);
            OnPropertyChanged(nameof(CurrentPosition));
        }
        #endregion


        private DelegateCommand play;
        public ICommand Play
        {
            get
            {
                if (play == null)
                {
                    play = new DelegateCommand(MusicManager.Instance.PlaySong);
                    OnPropertyChanged(nameof(IsPlaying));
                }
                return play;
            }
        }


        private DelegateCommand startPlayback;
        public ICommand StartPlayback
        {
            get
            {
                if (startPlayback == null)
                {
                    CheckPause = false;
                    CheckPlay = true;
                    startPlayback = new DelegateCommand(MusicManager.Instance.PlayingAfterPause);
                }
                return startPlayback;
            }
        }

        private DelegateCommand playNext;
        public ICommand PlayNext
        {
            get
            {
                
                if (playNext == null)
                {

                    playNext = new DelegateCommand(MusicManager.Instance.PlayNext);
                }
                return playNext;
            }
        }
        private DelegateCommand playPrevious;
        public ICommand PlayPrevious
        {
            get
            {
                if (playPrevious == null)
                {

                    playPrevious = new DelegateCommand(MusicManager.Instance.PlayPrevious);
                }
                return playPrevious;
            }
        }
        private DelegateCommand pause;
        public ICommand Pause
        {
            get
            {
                if (pause == null)
                {

                    pause = new DelegateCommand(MusicManager.Instance.Pause);
                }
                CheckPause = true;
                CheckPlay = false;
                return pause;
            }
        }
        #endregion
        #region Смена языка

        private DelegateCommand testSmena;
        public ICommand LangSmena
        {
            get
            {
                if (testSmena == null)
                {
                    testSmena = new DelegateCommand(LoadResourcesForEnglishSmena);
                }
                return testSmena;
            }
        }
        private void LoadResourcesForEnglishSmena()
        {

            foreach (var dict in Application.Current.Resources.MergedDictionaries)
            {
                if (dict.Source != null && dict.Source.OriginalString.Contains("lang"))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dict);
                    break;
                }
            }
            var appResources = Application.Current.Resources;
            if (chekLNG)
            {

                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(
                        String.Format($"/ResiurceDictionary/lang-RU.xaml"),
                        UriKind.Relative)
                });
                Image = "../resurse/ru.png";
                chekLNG = false;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(
                        String.Format($"/ResiurceDictionary/lang-ENG.xaml"),
                        UriKind.Relative)
                });
                Image = "../resurse/us.png";
                chekLNG = true;
            }
        }
        private string image = "../resurse/ru.png";
        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged(nameof(image));
            }
        }

        #endregion
        #region Show login
        private DelegateCommand? showLoginCommand;
        public ICommand ShowLoginCommand
        {
            get
            {  
                if (showLoginCommand == null)
                {
                    checkClose = false;
                    showLoginCommand = new DelegateCommand(() =>
                    { ShowWindow(new LoginView()); CloseMainView(); });
                }
                return showLoginCommand;
            }
        }

        #endregion
        #region Управление окном

        #endregion
    }
}