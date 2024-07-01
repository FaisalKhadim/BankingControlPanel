using BankingControlPanel.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace BankingControlPanel.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           

            // Configure primary keys
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Account>().HasKey(u => u.Id);
            modelBuilder.Entity<Address>().HasKey(a => a.Id);
            modelBuilder.Entity<Role>().HasKey(a => a.Id);
            modelBuilder.Entity<Client>().HasKey(u => u.Id);

            modelBuilder.Entity<Client>()
               .Property(c => c.Id)
               .ValueGeneratedOnAdd();
        }

    }


}
