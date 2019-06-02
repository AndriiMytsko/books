using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestWebApi.Filters
{
    public class ExceptionFilter: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
