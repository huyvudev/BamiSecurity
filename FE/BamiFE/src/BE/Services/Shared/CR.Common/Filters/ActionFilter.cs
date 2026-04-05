using Microsoft.AspNetCore.Mvc.Filters;

namespace CR.Common.Filters
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something before the action executes.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do something after the action executes.
        }
    }
}
