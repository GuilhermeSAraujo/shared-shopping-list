using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Tokens;
using SL.Application.Ports.User;
using SL.Application.UseCases.Interfaces.User;
using SL.Domain.Adapters;
using SL.Domain.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SL.Application.UseCases.User;

public class AuthUser(IUsersAdapter usersAdapter) : IAuthUser
{
    private readonly IUsersAdapter _usersAdapter = usersAdapter
        ?? throw new ArgumentNullException(nameof(usersAdapter));

    private readonly PasswordHasher _passwordHasher = new();
    
    public async Task<AuthUserOutput?> ExecuteAsync(AuthUserInput input)
    {
        ValidateEntries(input);

        var user = await _usersAdapter.FindUserByEmail(input.Email);

        if (user == null)
            throw new Exception("User not registered.");

        var validPassword = ValidateCredentials(user, input.Password);

        if (!validPassword)
            return null;

        var jwtToken = await GenerateToken(user);

        return new()
        {
            Token = jwtToken
        };
    }

    private void ValidateEntries(AuthUserInput input)
    {
        if (string.IsNullOrEmpty(input.Email))
            throw new Exception("Email cannot be empty");

        if (string.IsNullOrEmpty(input.Password))
            throw new Exception("Password cannot be empty");
    }

    private bool ValidateCredentials(UserModel user, string password)
    {
        var result =  _passwordHasher.VerifyHashedPassword(user.Password, password);

        return result == PasswordVerificationResult.Success;
    }

    private async Task<string> GenerateToken(UserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = await Task.Run(() =>
        {

            var key = Encoding.ASCII.GetBytes("guizinnnnnnnnho333333tokenn12345");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", user.Id.ToString()),
                    new Claim("email", user.Email),
                    new Claim("name", user.Name),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.CreateToken(tokenDescriptor);
        });

        return tokenHandler.WriteToken(token);
    }
}