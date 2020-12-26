using KhatmaBackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.DBContext
{
    public class KhatmaContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> UserGroups { get; set; }
        public DbSet<Setting> KhatmaSettings { get; set; }
        public DbSet<UserDevice> userDevices { get; set; }
        public DbSet<Khatma> khatmas { get; set; }
        public DbSet<UserPages> userPages { get; set; }

        public KhatmaContext(DbContextOptions<KhatmaContext> options) : base(options)
        {
          Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Khatma>().HasData(
       new Khatma
       {
           Id = 1,
           Name = "Public",
           Type = Utilites.Enums.KhatmaTypeEnum.onepage
       }
   );
  modelBuilder.Entity<Group>().HasData(
new Group
{
  Id = 1,
  Name = "Public",
KhatmaId=1
}
);
            modelBuilder.Entity<Setting>().HasData(
new Setting
{
    Id=1,
  KhatmaCount = 0,
  LastDistributedPage = 0,
  KhatmaId=1
}
);
        }
    }
}
