using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.Framework.Exceptions.Handling
{
    public class MasaExceptionHandlingOptions
    {
        public bool CatchAllException { get; set; } = true;

        public Func<Exception, (Exception? OverrideException, bool ExceptionHandled)> CustomExceptionHandler { get; set; }
    }
}
