using Microsoft.EntityFrameworkCore;
using SampleMVVM.Model.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SampleMVVM.DataBase.Repositories
{
    public class SongsRepositories : IRepository<Songs>
    {
        private ApplicationContext db;
        public SongsRepositories(ApplicationContext context)
        { 
            this.db = context;
        }
        public IEnumerable<Songs> GetAll()
        {
            return db.Songs;
        }
        public Songs Get(int id)
        { 
            return db.Songs.Find(id);
        }
        public void Create(Songs songs)
        { 
            db.Songs.Add(songs); 
        }
        public void Update(Songs songs)
        { 
            db.Entry(songs).State = EntityState.Modified; 
        }

        public void Delete(int id)
        {
            Songs songs = db.Songs.Find(id);

            if (songs == null)
                return;

            foreach (var item in db.PlayListItem.Where(i => i.SoungId == id))
            {
                db.Remove(item);
            }

            db.Songs.Remove(songs);

            db.SaveChanges();
        }
        public  List<Songs> Sort(string SearchString)
        {
            var currentSongs = db.Songs.ToList();
            return currentSongs.Where(x => x.Name.ToLower().Contains(SearchString.ToLower())).ToList();
        }
    }
}
