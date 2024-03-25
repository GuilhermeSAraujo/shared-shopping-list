using System.Security.Claims;
using System.Text;
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
                    cfg.RequireHttpsMetadata = false;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidIssuer = AuthUrl,
                        ClockSkew = TimeSpan.Zero, // Optionally, set clock skew to zero to consider the token invalid if it's expired exactly at the expiration time
                        // Add your security key(s) for signature validation
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("guizinnnnnnnnho333333tokenn12345"))
                        // If you are using asymmetric keys, you can use the following:
                        // IssuerSigningKey = new X509SecurityKey(new X509Certificate2("path_to_your_certificate"))
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