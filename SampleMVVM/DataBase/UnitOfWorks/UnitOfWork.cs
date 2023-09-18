
using SampleMVVM.DataBase.Repositories;
using SampleMVVM.DataBase.Repositories.SampleMVVM.DataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.DataBase.UnitOfWorks
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationContext db = new ApplicationContext();
        private AdminListRepositories adminListRepositories;
        private AdminRepository adminRepository;
        private PlayListItemRepositories playlistRepository;
        private PlayListRepositories playListRepositories;
        private SongsRepositories songsRepositories;
        private UserListMusicRepositories userListMusicRepositories;
        private UserListRepositories userListRepositories;
        private UsersRepositories usersRepositories;


        public AdminListRepositories AdminListRepositories
        {
            get
            {
                if (adminListRepositories == null)
                    adminListRepositories = new AdminListRepositories(db);
                return adminListRepositories;
            }
        }
        public PlayListItemRepositories PlayListItemRepositories
        {
            get
            {
                if (playlistRepository == null)
                    playlistRepository = new PlayListItemRepositories(db);
                return playlistRepository;
            }
        }
        public AdminRepository AdminRepository
        {
            get
            {
                if (adminRepository == null)
                    adminRepository = new AdminRepository(db);
                return adminRepository;
            }
        }
        public PlayListRepositories PlayListRepositories
        {
            get
            {
                if (playListRepositories == null)
                    playListRepositories = new PlayListRepositories(db);
                return playListRepositories;
            }
        }
        public SongsRepositories SongsRepositories
        {
            get
            {
                if (songsRepositories == null)
                    songsRepositories = new SongsRepositories(db);
                return songsRepositories;
            }
        }
        public UserListMusicRepositories UserListMusicRepositories
        {
            get
            {
                if (userListMusicRepositories == null)
                    userListMusicRepositories = new UserListMusicRepositories(db);
                return userListMusicRepositories;
            }
        }
        public UserListRepositories UserListRepositories
        {
            get
            {
                if (userListRepositories == null)
                    userListRepositories = new UserListRepositories(db);
                return userListRepositories;
            }
        }
        public UsersRepositories UsersRepositories
        {
            get
            {
                if (usersRepositories == null)
                    usersRepositories = new UsersRepositories(db);
                return usersRepositories;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }

}
