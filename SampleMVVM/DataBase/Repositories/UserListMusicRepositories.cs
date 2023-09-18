using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.DataBase.Repositories
{
    using global::SampleMVVM.Managers;
    using global::SampleMVVM.Model.BD;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace SampleMVVM.DataBase.Repositories
    {
        public class UserListMusicRepositories : IRepository<UserListMusic>
        {

            private ApplicationContext db;
            public UserListMusicRepositories(ApplicationContext context)
            { this.db = context; }
            public IEnumerable<PlayListItem> GetAll()
            {
                return db.PlayListItem;
            }
            public UserListMusic Get(int id)
            {
                return db.UserListMusic.Find(id);
            }
            public void Create(UserList userList)
            {
                UserListMusic userListMusic = new UserListMusic();
                userListMusic.UserList = userList;
                userListMusic.UserListId = userList.Id;
                db.UserListMusic.Add(userListMusic);
                db.SaveChanges();
            }
            public void Update(UserListMusic playListItem)
            {
                db.Entry(playListItem).State = EntityState.Modified;
            }
            public void Delete(int id)
            {
                UserListMusic playListItem = db.UserListMusic.Find(id);
                if (playListItem != null)
                    db.UserListMusic.Remove(playListItem);
            }

            IEnumerable<UserListMusic> IRepository<UserListMusic>.GetAll()
            {
                throw new NotImplementedException();
            }

            public void Create(UserListMusic item)
            {
                throw new NotImplementedException();
            }
        }
    }

}
