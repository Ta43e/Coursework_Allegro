using SampleMVVM.Commands;
using SampleMVVM.DataBase.Repositories;
using SampleMVVM.DataBase.UnitOfWorks;
using SampleMVVM.Model.BD;
using SampleMVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SampleMVVM.Managers;
using Allegro.Model;

namespace SampleMVVM.ViewModels
{
    public class LoginTestViewModel : ViewModelBase
    {

        private Users user = new();             
        private Admin admin = new();             
        private List<Users> users = db.GetUserList();
        private List<Admin> admins = db.GetAdminList();
        private DelegateCommand<LoginView>? loginCommand;
        private DelegateCommand<LoginView>? registerCommand;

        private string errorEmail = string.Empty;
        private string errorPassword = string.Empty;
        private string errorUserName = string.Empty;

        private bool chekReg = false;
        #region Property


        #region Errors

        public string ErrorEmail
        {
            get => errorEmail;
            set
            {
                errorEmail = value;
                OnPropertyChanged(nameof(ErrorEmail));
            }
        }

        public string ErrorPassword
        {
            get => errorPassword;
            set
            {
                errorPassword = value;
                OnPropertyChanged(nameof(ErrorPassword));
            }
        }
        public string ErrorUserName
        {
            get => errorUserName;
            set
            {
                errorUserName = value;
                OnPropertyChanged(nameof(ErrorUserName));
            }
        }
        #endregion

        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                admin.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                admin.Name = value;
                OnPropertyChanged(nameof(UserName));
            }
        }



        #endregion

        #region Command

        #region Login

        public ICommand LoginCommand
        {
            get 
            {
                if (loginCommand == null)
                {
                    ErrorUserName = "";
                    ErrorPassword = "";
                    ErrorEmail = "";
                    chekReg = false;
                    loginCommand = new DelegateCommand<LoginView>((LoginView view) =>
                    {
                        if (CheckingForPasswordLength(Password))
                        {
                            if (users.Any(x => x.UserName == user.UserName && PasswordHasher.VerifyPassword(user.Password, x.Password)) && user.UserName != "admin")
                            {
                                InitCurrentUser();
                                IsUser = true;
                                IsAdmin = false;
                                ShowMainWindow();
                                view.Close();
                            }
                            else if (admins.Any(x => x.Name == admin.Name && PasswordHasher.VerifyPassword(admin.Password, x.Password)))
                            {
                                InitCurrentUser();
                                IsAdmin = true;
                                IsUser = false;
                                ShowMainWindow();
                                view.Close();
                            }
                            else if (user.UserName == "")
                            {
                                ErrorUserName = "Enter a name";
                            }
                            else
                            {
                                ErrorUserName = "Invalid username or password";
                            }
                        }
                    });
                }
                return loginCommand;
            }
        }

        #endregion

        #region Register

        public ICommand RegisterCommand
        {
            get
            {

                if (registerCommand == null)
                {
                    ErrorUserName = "";
                    ErrorPassword = "";
                    ErrorEmail = "";
                    chekReg = true;
                    registerCommand = new DelegateCommand<LoginView>((LoginView view) =>
                    {
                        if (ValidateRegisterData())
                        {
                            if (!users.Any(x => x.Email == user.Email && PasswordHasher.VerifyPassword(user.Password, x.Password)) && user.UserName != "admin")
                            {
                                MusicManager.Instance.unitOfWork.UsersRepositories.Create(user);
                                MusicManager.Instance.unitOfWork.Save();
                                InitCurrentUser();
                                IsUser = true;
                                IsAdmin = false;
                                ShowMainWindow();
                                view.Close();
                            }
                            else
                            {
                                MessageBox.Show("A user with this name already exists.");
                            }
                        }
                    });
                }
                return registerCommand;
            }
        }

        #endregion

        #endregion

        #region Methods

        private void InitCurrentUser()
        {
            CurrentUser = db.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
        }
        private bool ValidateRegisterData()
        {

            return ValidationOfEmail(Email) && CheckingForPasswordLength(Password) && ValidationOfUserName(UserName);
        }

        private bool ValidationOfEmail(string value)
        {
            Regex regex = new Regex("^((\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*)\\s*[;]{0,1}\\s*)+$");
            if (value == null)
            {
                ErrorEmail = "Enter Email";
                return false;
            }
            else if (!regex.IsMatch(value) || value.Length < 10)
            {
                ErrorEmail = "Incorrect email";
                return false;
            }
            else if (chekReg)
            {
                foreach (var item in MusicManager.Instance.unitOfWork.UsersRepositories.GetAll())
                {
                    if (item.Email == value)
                    {
                        ErrorEmail = "This email is already in use";
                        return false;
                    }
                }
            }
            ErrorEmail = "";
            return true;
        }

        public bool CheckingForPasswordLength(string value)
        {
            if (value == null)
            {
                ErrorPassword = "Enter password";
                return false;
            }
            else if (value.Length <= 6)
            {
                ErrorPassword = "The password is too short";
                return false;
            }
            ErrorPassword = "";
            user.Password =  PasswordHasher.HashPassword(value);
            admin.Password =  PasswordHasher.HashPassword(value);
            return true;
        }

        public bool ValidationOfUserName(string value)
        {

            Regex regex = new Regex("^[a-zA-Z]+$");
            if (value == null)
            {
                ErrorUserName = "Enter UserName";
                return false;
            }
            else if ((!regex.IsMatch(value) || value.Length < 3) && value == "admin")
            {
                ErrorUserName = "Incorrect data";
                return false;
            }
            else if(chekReg)
            {
                foreach (var item in MusicManager.Instance.unitOfWork.UsersRepositories.GetAll())
                {
                    if(item.UserName == value)
                    {
                        ErrorUserName = "This name is taken";
                        return false;
                    }
                }
            }
            ErrorUserName = "";
            return true;
        }
        #endregion
    }
}
