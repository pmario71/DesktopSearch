using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Utils.Async
{
    public static class ExceptionHelpers
    {
        /// <summary>
        /// Attempts to prepare the exception for re-throwing by preserving the stack trace. The returned exception should be immediately thrown.
        /// </summary>
        /// <param name="exception">The exception. May not be <c>null</c>.</param>
        /// <returns>The <see cref="Exception"/> that was passed into this method.</returns>
        public static Exception PrepareForRethrow(Exception exception)
        {
            return ExceptionEnlightenment.PrepareForRethrow(exception);
        }
    }

    public static class ExceptionEnlightenment
    {
        public static Exception PrepareForRethrow(Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
            return exception;
        }
    }
}
