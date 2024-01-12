using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.Infrastructure.GuardValidation
{
    public static class GuardValidation
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
        public static void AgainstNull<T>(T value, string paramName)
        {
            if (paramName == null) throw new ArgumentNullException(paramName, "Parameter cannot be null.");
        }
    }
}
