using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BasicApi
{
    public class TokenAuth : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var result = true;
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization")) result = false;
            

            string token = string.Empty;
            if (result)
            {
                token = context.HttpContext.Request.Headers.First(_ => _.Key == "Authorization").Value;
                if (!token.StartsWith("Bearer "))
                {
                    result = false;
                }
                else
                {
                    try
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var data = tokenHandler.ReadJwtToken(token.Replace("Bearer ", string.Empty));
                    }
                    catch
                    {
                        result = false;
                    }
                }
            }

            if (!result)
            {
                context.ModelState.AddModelError("Unauthorize", "You are not authorized.");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }
    }
}
