using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Entities
{
    public class BudgetContext : DbContext
    {
        public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
        {
            
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Meetup> Meetups { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Lecture> Lectures { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public DbSet<CostCategory> CostCategories { get; set; }

        public DbSet<CostItem> CostItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meetup>()
                .HasOne(c => c.CreatedBy);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role);

            modelBuilder.Entity<Meetup>()
                .HasOne(m => m.Location)
                .WithOne(l => l.Meetup)
                .HasForeignKey<Location>(l => l.MeetupId);

            modelBuilder.Entity<Meetup>()
                .HasMany(m => m.Lectures)
                .WithOne(l => l.Meetup);

            /*
            modelBuilder.Entity<User>()
             .HasOne(u => u.Budget)
             .WithOne(b => b.User)
            .HasForeignKey<Budget>(b => b.UserId);
            */
            modelBuilder.Entity<Budget>()
              .HasMany<CostCategory>(c => c.costCategories)
              .WithOne(b => b.budget)
                .HasForeignKey(c => c.budgetId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CostCategory>()
               .HasMany<CostItem>(c => c.costItems)
               .WithOne(b => b.costCategory)
                 .HasForeignKey(c => c.costCategoryId)
               .OnDelete(DeleteBehavior.Cascade);

          
        }

    }
}
