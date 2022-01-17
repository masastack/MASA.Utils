using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.Framework.Exceptions.Handling
{
    public class UserFriendlyExceptionResult : IActionResult
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public UserFriendlyExceptionResult(string message)
        {
            StatusCode = (int)MasaHttpStatusCode.UserFriendlyException;
            Message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await context.HttpContext.Response.WriteTextAsync(StatusCode, Message);
        }
    }
}
