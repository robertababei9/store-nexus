
using Domain.Common;
using Domain.Entities;
using Domain.ModelBuilders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        #region Entities
        public DbSet<Company> Company{ get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceItem> InvoiceItem { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // model builders
            #region Model Builders
            modelBuilder.Entity<Company>(CompanyModelBuilder.Get());
            modelBuilder.Entity<User>(UserModelBuilder.Get());
            modelBuilder.Entity<Role>(RoleModelBuilder.Get());
            modelBuilder.Entity<Invoice>(InvoiceModelBuilder.Get());
            modelBuilder.Entity<InvoiceItem>(InvoiceItemModelBuilder.Get());
            #endregion
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IAuditableEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


    }
}
