using Microsoft.EntityFrameworkCore;
using SampleMVVM.Model.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Admin> Admin { get; set; } = null!;
        public DbSet<AdminList> AdminList { get; set; } = null!;
        public DbSet<PlayList> PlayList { get; set; } = null!;
        public DbSet<Songs> Songs { get; set; } = null!;
        public DbSet<UserList> UserList { get; set; } = null!;
        public DbSet<UserListMusic> UserListMusic { get; set; } = null!;
        public DbSet<PlayListItem> PlayListItem { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-NCBR8BG;Database=AllegroBD;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public List<Users> GetUserList()
        {
            using var context = new ApplicationContext();
            return context.Users.ToList();
        }
        public List<Songs> GetSongList()
        {
            using var context = new ApplicationContext();
            return context.Songs.ToList();
        }
        public List<Admin> GetAdminList()
        {
            using var context = new ApplicationContext();
            return context.Admin.ToList();
        }
    }
}
