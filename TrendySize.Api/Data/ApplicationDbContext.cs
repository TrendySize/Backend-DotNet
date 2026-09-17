using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TrendySize.Api.DTOs;
using TrendySize.Api.Models;


namespace TrendySize.Api.Data
{
    // The ApplicationDbContext class inherits from IdentityDbContext, which is a specialized DbContext provided by ASP.NET Core Identity for managing user authentication and authorization. It uses the ApplicationUser class as the user entity and IdentityRole<Guid> for role management, with Guid as the primary key type.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Customer> Customers => Set <Customer>();
        public DbSet<Measurement> Measurements => Set<Measurement>();

        // The constructor of the ApplicationDbContext class takes DbContextOptions as a parameter, which allows configuration of the database connection and other options. It passes these options to the base class constructor.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           
        }

        // The OnModelCreating method is overridden to customize the model creation process. It calls the base implementation to ensure that the default configurations for IdentityDbContext are applied. This method can be used to configure entity relationships, constraints, and other model settings.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Customer>()
            .Property(c => c.Gender)
            .HasConversion<string>();


            builder.Entity<Measurement>()
            .Property(m => m.Status)
            .HasConversion<string>();
        }
    }
}
