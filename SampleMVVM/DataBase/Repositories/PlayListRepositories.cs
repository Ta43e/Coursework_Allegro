using Microsoft.EntityFrameworkCore;
using SampleMVVM.DataBase.UnitOfWorks;
using SampleMVVM.Managers;
using SampleMVVM.Model.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayList = SampleMVVM.Model.BD.PlayList;

namespace SampleMVVM.DataBase.Repositories
{
    public class PlayListRepositories : IRepository<PlayList>
    {
        private ApplicationContext db;
        public PlayListRepositories(ApplicationContext context)
        { 
            this.db = context;
        }
        public IEnumerable<PlayList> GetAll()
        { 
            return db.PlayList;
        }
        public IEnumerable<PlayList> Get(Users userId)
        {
            return db.PlayList.Where(p => p.User == userId).ToList();
        }
        public void Create(PlayList playList)
        {
            var user = MusicManager.Instance.unitOfWork.UsersRepositories.Get(playList.User.Id);
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }
            db.PlayList.Add(playList); 
        }
        public List<PlayListItem> GetItems(PlayList playList)
        {
            return db.PlayListItem.Where(i => playList.Id == i.PlayListId).ToList();
        }
        public List<Songs> GetSongs(PlayList playList)
        {
            List<PlayListItem> playListItems = GetItems(playList);

            List<Songs> songs = new List<Songs>();

            foreach (var item in playListItems)
            {
                songs.Add(db.Songs.FirstOrDefault(i => i.Id == item.SoungId) ?? new Songs());
            }
            return songs;
        }

        public void AddMusic(PlayList playList, Songs songs)
        {
            PlayListItem item = new PlayListItem()
            {
                PlayListId = playList.Id,
                SoungId = songs.Id,
            };

            db.PlayListItem.Add(item);
        }
        public void Update(PlayList playList)
        {
            db.Entry(playList).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            PlayList playList = db.PlayList.Find(id);
            if (playList != null)
                db.PlayList.Remove(playList);
        }

        public PlayList Get(int id)
        {
            return db.PlayList.Find(id);
        }

    }
}
