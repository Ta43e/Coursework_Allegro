using Microsoft.Win32;
using SampleMVVM.Commands;
using SampleMVVM.Managers;
using SampleMVVM.Views;
using System.Windows;
using System.Windows.Input;
using PlayList = SampleMVVM.Model.BD.PlayList;

namespace SampleMVVM.ViewModels
{
    public class NewPlayListViewModel : ViewModelBase
    {
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public int Text3 { get; set; }
        public string Image { get; set; }


        #region Errors
        private string errorName = string.Empty;
        private string errorArtist = string.Empty;
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

        private NewPlayListView windows;
        private DelegateCommand createItem;
        public NewPlayListViewModel(NewPlayListView windows)
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
            if (Image == null)
            {
                MessageBox.Show("Добавте картинку!");
                return;
            }
            else if (Text1 == "" || Text1 == null)
            {
                ErrorName = "Введите названиен плейлиста";
                return;
            }
            else if (Text2 == "" || Text2 == null)
            {
                ErrorArtist = "Введите описание";
                return;
            }
            PlayList newItemViewModel = new PlayList();
            newItemViewModel.User = MusicManager.Instance.unitOfWork.UsersRepositories.Get(CurrentUser.Id);
            newItemViewModel.PlayList_Name = Text1;
            newItemViewModel.Author = Text2;
            newItemViewModel.ImagePath = Image;
            MusicManager.Instance.unitOfWork.PlayListRepositories.Create(newItemViewModel);
            MusicManager.Instance.unitOfWork.Save();
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
    }
}
