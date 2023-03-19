using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnjOffer.Infrastructure
{
    public class EnjOfferDbContext : DbContext
    {
        public DbSet<Advice> Advice { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<DefaultWords> DefaultWords { get; set; }
        public DbSet<UserWords> UserWords { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserStatistics> UserStatistics { get; set; }

        public EnjOfferDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
