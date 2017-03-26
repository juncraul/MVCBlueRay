using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MVCBlueRay.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BluRay> BluRays { get; set; }
        public DbSet<UserBlueRay> UserBlueRays { get; set; }
    }
}