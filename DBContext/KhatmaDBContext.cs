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
        public DbSet<Group> Groups { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public KhatmaContext(DbContextOptions<KhatmaContext> options) : base(options)
        {
          Database.EnsureCreated();
        }
    }
}
