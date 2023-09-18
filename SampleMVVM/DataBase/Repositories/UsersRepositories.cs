using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.DataBase.Repositories
{
    public class UsersRepositories : IRepository<Users>
    {
        private ApplicationContext db;
        public UsersRepositories(ApplicationContext context)
        {
            this.db = context;
        }
        public IEnumerable<Users> GetAll()
        {
            return db.Users;
        }
        public Users Get(int id)
        {
            return db.Users.Find(id);
        }
        public void Create(Users user)
        {
            UserList userList = new UserList();
            userList.User = user;
            userList.List_Name = "";
            db.Users.Add(user);
            MusicManager.Instance.unitOfWork.Save();
            MusicManager.Instance.unitOfWork.UserListRepositories.Create(userList);
   
        }
        public void Update(Users userList)
        {
            db.Entry(userList).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Users userList = db.Users.Find(id);
            if (userList != null)
                db.Users.Remove(userList);
        }
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            return true;
        }
    }
}
