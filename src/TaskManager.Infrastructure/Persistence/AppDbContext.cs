using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<TaskItem>().HasKey(x => x.Id);

            modelBuilder.Entity<TaskItem>().HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
        }
    }
}