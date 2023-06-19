using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PrimeNumber.Core.Helper;
using System.Net;

namespace PrimeNumber.Ui.Helper
{
    public class AuthenticationFilterAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; } = String.Empty;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpContextAccessor _httpContextAccessor = new();

            var userInfo = TokenManagerService.GetCurrentUser(_httpContextAccessor?.HttpContext?.Request);
            if (string.IsNullOrEmpty(userInfo.UserName))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult("Please Send Valid Token In Request Header :| ");
            }
            if (!string.IsNullOrEmpty(Roles) && !string.IsNullOrEmpty(userInfo.UserName))
            {
                var allUserRole = userInfo.Roles?.Split(",");
                if (allUserRole is not null && !allUserRole.Contains(Roles))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult($"This User Must have Role {Roles} :| ");
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
