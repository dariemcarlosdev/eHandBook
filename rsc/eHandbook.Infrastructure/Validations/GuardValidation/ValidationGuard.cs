using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace eHandbook.Infrastructure.Validations.GuardValidation
{
    public static class EnsureValidation
    {
        /// <summary>
        /// Ensures parameter is not null.
        /// </summary>
        /// <example>
        /// GuardValidation.AgainstNull(int id); // Ensures id is not null.
        /// Rest of the method logic
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AgainstNullorEmpty<T>([NotNull] T value, [CallerArgumentExpression("value")] string? paramName = default)
        {
            if (value == null) throw new ArgumentNullException(paramName);
        }
    }
}
