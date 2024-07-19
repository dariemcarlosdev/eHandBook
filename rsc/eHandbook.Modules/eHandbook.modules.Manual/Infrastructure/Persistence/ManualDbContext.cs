using eHandbook.modules.ManualManagement.CoreDomain.EntitiesModels;
using eHandbook.modules.ManualManagement.Infrastructure.Configuration.FluentAPIs;
using Microsoft.EntityFrameworkCore;

namespace eHandbook.modules.ManualManagement.Infrastructure.Persistence
{
    public class ManualDbContext : DbContext
    {
        /*
         * An ORM can manage changes between in-memory objects and the database. This feature is also known as object tracking. Note that this feature is turned on by default on all entities. As a consequence, you may modify those entities and then persist the changes to the database.
           However, we should be aware of the performance cost associated. Unless necessary, you should disable object tracking. The following code shows how to use the AsNoTracking method to minimize memory use and increase speed.
          
           var order = dbContext.Orders.Where(o => o.ShipCountry == "India").AsNoTracking().ToList();            
            
           We may also deactivate query-tracking behavior at the database context level. This would deactivate change tracking for all entities associated with the database context.
            
           this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            Here’s the updated source code of our custom ManualDbContext class with change tracking and query tracking disabled.
         */

        public ManualDbContext(DbContextOptions<ManualDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior =
             QueryTrackingBehavior.NoTracking;
            //Disable Lazy Loading and Use Eager Loading for Improved Performance.
            ChangeTracker.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Defined Manual Class as one of the Entity we intend to work with.
        /// </summary>
        public DbSet<ManualEntity> Manuals { get; set; }

        //Using Fluent API approach.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Applying ManualConfiguration.
            modelBuilder.ApplyConfiguration(new ManualConfiguration());
            //Entity Framework Core maps all the properties into the table columns.
            base.OnModelCreating(modelBuilder);

        }

    }
}
