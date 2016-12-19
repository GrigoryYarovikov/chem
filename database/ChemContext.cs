using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Chem.Models;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chem.DataContext
{
    public class ChemContext : IdentityDbContext<ApplicationUser>, IDisposable
    {
        DbSet<Element> Elements { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Substance> Substances { get; set; }

        public ChemContext(): base("DefaultConnection", false)
        {
        }

        public static ChemContext Create()
        {
            return new ChemContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            var user = modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers");
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));

            user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("AspNetUserRoles");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("AspNetUserLogins");

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("AspNetUserClaims");

            var role = modelBuilder.Entity<IdentityRole>()
                .ToTable("AspNetRoles");
            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Substance>().HasMany(x => x.Categories).WithMany();
            modelBuilder.Entity<Substance>().HasMany(x => x.Names);
            modelBuilder.Entity<Substance>().HasMany(x => x.Scheme);
            modelBuilder.Entity<Category>().HasMany(x => x.Parents).WithMany();
        }

        public void Dispose()
        {
            base.Dispose();
        }
    }
}
