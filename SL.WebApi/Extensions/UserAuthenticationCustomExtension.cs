using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SL.WebApi.Extensions;

public static class UserAuthenticationCustomExtension
{
    public static AuthenticationBuilder AddUserAuthentication(this AuthenticationBuilder auth, string AuthUrl)
    {
        auth
            .AddJwtBearer(
                cfg =>
                {
                    // cfg.Authority = AuthUrl;
                    cfg.RequireHttpsMetadata = false;
                    cfg.IncludeErrorDetails = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = false,
                        ValidIssuer = AuthUrl,
                        ValidateLifetime = true
                    };
                    cfg.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = context =>
                        {
                            try
                            {
                                var token = context.SecurityToken as Microsoft.IdentityModel.JsonWebTokens.JsonWebToken;
                                if (token == null)
                                {
                                    throw new Exception("Invalid security token");
                                }

                                var id = token.Claims.FirstOrDefault(x => x.Type.Equals("id"));
                                var email = token.Claims.FirstOrDefault(x => x.Type.Equals("email"));
                                var name = token.Claims.FirstOrDefault(x => x.Type.Equals("name"));

                                if (id is null || email is null || name is null)
                                {
                                    context.Fail("Invalid claims");
                                }

                                var claims = new List<Claim> { id, email, name };

                                var identity = context.Principal?.Identity as ClaimsIdentity;

                                identity?.AddClaims(claims);
                            }
                            catch (Exception e)
                            {
                                context.Fail(e);
                            }

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = async c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            c.Response.ContentType = "application/json";
                            await c.Response.WriteAsync(c.Exception.ToString());
                        }
                    };
                }
            );

        return auth;
    }
}