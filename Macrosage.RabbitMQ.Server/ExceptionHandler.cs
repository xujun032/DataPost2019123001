using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Macrosage.RabbitMQ.Server
{
    public class ExceptionHandler
    {

        public static void ThrowArgumentNull(string parameterName,object parameterValue)
        {
            if (parameterValue.Equals(null)) {
                throw new ArgumentNullException($"{parameterName} is null");
            }
        }
    }
}
