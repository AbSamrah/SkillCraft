using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.DataAccessLayer.Models;

namespace DataAccessLayer.Data
{
    public class UsersDbContext: DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PendingUser> PendingUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = new Guid("5b612e20-44f5-4c27-8e3e-e2e7e7c23cc3"), Title="User" },
                new Role() { Id = new Guid("6d0193c3-da6c-46ad-aa23-88169e3f9202"), Title="Admin" },
                new Role() { Id = new Guid("949acd82-f23d-4d2a-970e-ee89a6e1109c"), Title="Editor" }
                );

            modelBuilder.Entity<PendingUser>()
                .HasIndex(u => u.Email)
                .IsUnique();
            
            /*modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();*/
        }
    }
}
