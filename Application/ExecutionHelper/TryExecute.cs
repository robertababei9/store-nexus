using Application.ExecutionHelper.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ExecutionHelper
{
    public static class ExecuteFunc
    {
        public static T TryExecute<T>(Func<T> func, ILogger logger)
        {
            string callerMethodName = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name;

            try
            {
                return func();
            }
            catch ( StoreNexusException ex )
            {
                throw new StoreNexusException(ex.Message, ex, ExceptionCode.CodeError);
            }
            catch ( Exception ex )
            {
                throw new StoreNexusException(callerMethodName + ".Exception -> " + ex.Message, ex, ExceptionCode.Invalid);
            }
        }
    }

}
