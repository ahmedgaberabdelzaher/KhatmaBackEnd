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
        public KhatmaContext(DbContextOptions<KhatmaContext> options) : base(options)
        {
          Database.EnsureCreated();
        }
    }
}
