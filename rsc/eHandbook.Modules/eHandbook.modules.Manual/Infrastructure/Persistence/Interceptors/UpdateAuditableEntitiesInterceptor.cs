using eHandbook.Core.Domain.Common;
using eHandbook.Infrastructure.CrossCutting.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eHandbook.modules.ManualManagement.Infrastructure.Persistence.Interceptors
{
    public sealed class UpdateMyAuditableEntitiesInterceptor :
        SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData
            , InterceptionResult<int> result
            , CancellationToken cancellationToken = new CancellationToken())
        {

            //Using userService to GetLoggedUser()(Implement for UserManagement Module) 
            //Leak implementation.
            //var applicationUser = _userMockService.GetLoggedUser();


            DbContext? dbContext = eventData.Context;

            if (dbContext is null)
            {
                return base.SavingChangesAsync(
                    eventData
                    , result
                    , cancellationToken
                    );
            }

            // track auditable changes Added / Modified / Deleted  states 
            IEnumerable<EntityEntry<IAuditableEntity>> entries =
                dbContext
                .ChangeTracker
                .Entries<IAuditableEntity>();

            foreach (EntityEntry<IAuditableEntity> entry in entries)
            {

                if (entry.State == EntityState.Added)
                {
                    entry.Property(a => a.CreatedBy).CurrentValue = Helpers.GetUserProvider();
                    entry.Property(a => a.CreatedOn).CurrentValue = Helpers.TimestampProvider();
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(a => a.UpdatedBy).CurrentValue = Helpers.GetUserProvider();
                    entry.Property(a => a.UpdatedOn).CurrentValue = Helpers.TimestampProvider();
                    entry.Property(a => a.IsUpdated).CurrentValue = true;
                }

                if (entry.State == EntityState.Detached)
                {
                    entry.Property(a => a.IsDeleted).CurrentValue = true;
                    entry.Property(a => a.DeletedBy).CurrentValue = Helpers.GetUserProvider();
                    entry.Property(a => a.DeletedOn).CurrentValue = Helpers.TimestampProvider();
                }

            }

            return base.SavingChangesAsync(
                eventData
                , result
                , cancellationToken);
        }
    }

}
