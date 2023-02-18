﻿using System;
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

        public DbSet<Item> CostItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany<Budget>(b => b.Budgets)
                .WithOne(u => u.User)
                .HasForeignKey(b => b.userId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Budget>()
            //.HasMany<Budget>(b => b.createdBy)
            //  .WithOne(b => b.budget)
            //    .HasForeignKey(c => c.budgetId)
            //  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Budget>()
              .HasMany<CostCategory>(c => c.costCategories)
              .WithOne(b => b.budget)
                .HasForeignKey(c => c.budgetId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CostCategory>()
               .HasMany<Item>(c => c.costItems)
               .WithOne(b => b.costCategory)
                 .HasForeignKey(c => c.CostCategoryId)
               .OnDelete(DeleteBehavior.Cascade);

          
        }

    }
}
