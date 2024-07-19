using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Principal;

namespace eHandbook.Infrastructure.Utilities
{

    /// <summary>
    /// Helper Class to shared auxiliar methods.
    /// </summary>
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

        /// <summary>
        /// Ensures parameter is not null.
        /// </summary>
        /// <example>
        ///  Validation helper method to ensure a value is not null or neither empty.
        /// Rest of the method logic
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool AgainstNullorEmpty<T>([NotNull] T value, [CallerArgumentExpression("value")] string? str = default)
        {
            return value == null ? throw new ArgumentNullException(str) : false;
        }
    }
}
