using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MASA.Framework.Exceptions.Handling
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object obj)
            : base(obj)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
