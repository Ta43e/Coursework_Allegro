using Microsoft.EntityFrameworkCore;
using SampleMVVM.Model.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace SampleMVVM.DataBase.Repositories
{
    public class UserListRepositories : IRepository<UserList>
    {
        private ApplicationContext db;
        public UserListRepositories(ApplicationContext context)
        {
            this.db = context;
        }
        public IEnumerable<UserList> GetAll()
        {
            return db.UserList;
        }
        public UserList Get(Users userId)
        {
            return db.UserList.Find(userId);
        }
        public void Create(UserList userList)
        {
            db.UserList.Add(userList);
            db.SaveChanges();

        }
        public void Update(UserList userList)
        {
            db.Entry(userList).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            UserList userList = db.UserList.Find(id);
            if (userList != null)
                db.UserList.Remove(userList);

        }
        public bool AddSongs(Songs songs, Users users)
        {
            var list = GetSongs(users);
            foreach (var item in list)
            {
                if (item.Name == songs.Name && item.SongsPath == songs.SongsPath)
                {
                    MessageBox.Show(Application.Current.FindResource("ErrorAddSongs").ToString());
                    return false;
                }
            }
            UserListMusic userListMusic = new UserListMusic()
            {
                UserListId = users.Id,
                SoungId = songs.Id
            };

            db.UserListMusic.Add(userListMusic);

            db.SaveChanges();
            return true;
        }

        public List<UserListMusic> GetItems(Users playList)
        {
            return db.UserListMusic.Where(i => playList.Id == i.UserListId).ToList();
        }

        public List<Songs> GetSongs(Users users)
        {
            List<UserListMusic> userListMusics = db.UserListMusic.Where(i => i.UserListId == users.Id).ToList();

            List<Songs> list = new List<Songs>();

            foreach (var item in userListMusics)
            {
                list.Add(db.Songs.FirstOrDefault(i => i.Id == item.SoungId) ?? new Songs());
            }

            return list;
        }

        public UserList Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
