using Microsoft.EntityFrameworkCore;
using SampleMVVM.Model.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.DataBase.Repositories
{
    public class AdminRepository : IRepository<Admin>
    {
        private ApplicationContext db;
        public AdminRepository(ApplicationContext context)
        { this.db = context; }
        public IEnumerable<Admin> GetAll()
        { return db.Admin; }
        public Admin Get(int id)
        { return db.Admin.Find(id); }
        public void Create(Admin admins)
        { db.Admin.Add(admins); }
        public void Update(Admin admins)
        { db.Entry(admins).State = EntityState.Modified; }
        public void Delete(int id)
        {
            Admin student = db.Admin.Find(id);
            if (student != null)
                db.Admin.Remove(student);
        }
    }
}
