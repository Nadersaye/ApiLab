using Microsoft.AspNetCore.Mvc.Filters;

namespace LabApi.Filters
{
    public class AddAppNameHeaderFilter : IResultFilter
    {
        private readonly string _appName;

        public AddAppNameHeaderFilter(IConfiguration configuration)
        {
            _appName = configuration["AppName"] ?? "LabApi";
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("X-App-Name", _appName);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }
    }
}
