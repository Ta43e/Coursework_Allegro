using Microsoft.EntityFrameworkCore;
using SampleMVVM.Model.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.DataBase.Repositories
{
    public class PlayListItemRepositories : IRepository<PlayListItem>
    {

        private ApplicationContext db;
        public PlayListItemRepositories(ApplicationContext context)
        { this.db = context; }
        public IEnumerable<PlayListItem> GetAll()
        { 
            return db.PlayListItem; 
        }
        public PlayListItem Get(int id)
        { 
            return db.PlayListItem.Find(id); 
        }
        public void Create(PlayListItem playListItem)
        { 
            db.PlayListItem.Add(playListItem);
        }
        public void Update(PlayListItem playListItem)
        { 
            db.Entry(playListItem).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            PlayListItem playListItem = db.PlayListItem.Find(id);
            if (playListItem != null)
                db.PlayListItem.Remove(playListItem);
        }
    }
}
