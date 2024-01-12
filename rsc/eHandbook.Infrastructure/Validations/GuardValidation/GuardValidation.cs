using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.Infrastructure.Validations.GuardValidation
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
        public static void AgainstNull<T>(T value, string msg)
        {
            if (value == null) throw new ArgumentNullException(msg);
        }
    }
}
