using eHandbook.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace eHandbook.Core.Extensions
{
    public static class DbContextExtensions
    {

        /// <summary>
        /// Extension Method that extend dbContext to save changes in dbContetext.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static async Task<bool> ExtensionSaveChangesAsync(this DbContext dbContext, CancellationToken cancellationToken = new CancellationToken())
        {

            foreach (var entry in dbContext.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditableEntity)
                {
                    var auditable = entry.Entity as IAuditableEntity;
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedBy = "@User";//  
                        auditable.CreatedOn = TimestampProvider();
                        auditable.UpdatedOn = TimestampProvider();
                    }
                    else
                    {
                        auditable.UpdatedBy = "@User";
                        auditable.UpdatedOn = TimestampProvider();
                    }
                }
            }
            return await dbContext.SaveChangesAsync(cancellationToken) >= 0;
        }

        public static Func<DateTime> TimestampProvider { get; set; } = ()
        => DateTime.UtcNow;
    }

}
