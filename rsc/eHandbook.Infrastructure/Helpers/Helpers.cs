using System.Security;
using System.Security.Principal;

namespace eHandbook.Infrastructure.Helpers
{
    public static class Helpers
    {
        public static Func<DateTime> TimestampProvider { get; set; } = ()
            => DateTime.UtcNow;

        public static string GetUserProvider()
        {
            try
            {
                var user = WindowsIdentity.GetCurrent().Name;
                if (!string.IsNullOrEmpty(user))
                    return user.Split('\\')[1];
                return string.Empty;
            }
            catch (SecurityException e)
            {
                throw new Exception(e.Message);
            }
            

            

        }
    }
}
