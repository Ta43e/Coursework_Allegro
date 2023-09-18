using Microsoft.EntityFrameworkCore;
using SampleMVVM.Model.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.DataBase.Repositories
{
    public class AdminListRepositories : IRepository<AdminList>
    {
        private ApplicationContext db;
        public AdminListRepositories(ApplicationContext context)
        {
            this.db = context; 
        }
        public IEnumerable<AdminList> GetAll()
        {
            return db.AdminList; 
        }
        public AdminList Get(int id)
        {
            return db.AdminList.Find(id); 
        }
        public void Create(AdminList Songs)
        {
            db.AdminList.Add(Songs); 
        }
        public void Update(AdminList Songs)
        {
            db.Entry(Songs).State = EntityState.Modified; 
        }

        public void Delete(Songs song)
        {
                db.Songs.Remove(song);
        }
        public IEnumerable<Songs> GetAllSongs()
        {

            return db.Songs.ToList();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
