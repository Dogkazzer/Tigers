using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tigers.Data.Entities;

namespace Tigers.Data
{
    public class TigerContext:IdentityDbContext<StoreUser>
    {
        private readonly ModelBuilder builder;

        public TigerContext(DbContextOptions<TigerContext> options): base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        //dont need a property for OrderItem, as it will prob be more a child of Orders
        //only create DbSets for things you want to query directly against
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");

            builder.Entity<OrderItem>()
              .Property(p => p.UnitPrice)
              .HasColumnType("decimal(18,2)");
        }



    }
}
