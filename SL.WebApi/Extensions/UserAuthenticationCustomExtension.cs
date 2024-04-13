using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SL.WebApi.Extensions;
public static class UserAuthenticationCustomExtension
{
    public static AuthenticationBuilder AddUserAuthentication(this AuthenticationBuilder auth)
    {
        auth
            .AddJwtBearer(
                cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.IncludeErrorDetails = true;

                    // Parse the JWKS and create an RsaSecurityKey
                    var rsaParameters = new RSAParameters
                    {
                        Modulus = Base64UrlEncoder.DecodeBytes(Environment.GetEnvironmentVariable("JWT_MODULUS")),
                        Exponent = Base64UrlEncoder.DecodeBytes(Environment.GetEnvironmentVariable("JWT_EXPONENT"))
                    };
                    var signingKey = new RsaSecurityKey(rsaParameters);

                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                        ValidateLifetime = true,
                        IssuerSigningKey = signingKey
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

                                var id = token.Claims.FirstOrDefault(x => x.Type.Equals("user_id"));
                                var email = token.Claims.FirstOrDefault(x => x.Type.Equals("user_email"));
                                var name = token.Claims.FirstOrDefault(x => x.Type.Equals("user_full_name"));

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