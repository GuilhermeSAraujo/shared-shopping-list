using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SL.Application.Ports.User;
using SL.Application.UseCases.Interfaces;
using SL.Domain.Adapters;
using System.Security.Cryptography;

namespace SL.Application.UseCases.User;

public class RegisterUser(IUsersAdapter usersAdapter) : IRegisterUser
{
    private readonly IUsersAdapter _usersAdapter = usersAdapter
            ?? throw new ArgumentNullException(nameof(usersAdapter));

    private readonly IPasswordHasher _passwordHasher = new PasswordHasher();

    public async Task ExecuteAsync(RegisterUserInput input)
    {
        ValidateInput(input);

        var user = await _usersAdapter.FindUserByEmail(input.Email);

        if (user is not null)
            throw new Exception("User already exists.");

        string encrypedPassword = EncryptPassword(input.Password);

        await _usersAdapter.CreateUser(new()
        {
            Email = input.Email,
            Name = input.Name,
            Password = encrypedPassword,
        });
    }

    private void ValidateInput(RegisterUserInput input)
    {
        if (string.IsNullOrEmpty(input.Email))
            throw new Exception("Email cannot be empty.");

        if (string.IsNullOrEmpty(input.Name))
            throw new Exception("Name cannot be empty.");

        if (string.IsNullOrEmpty(input.Password) ||
            input.Password.Length < 8)
            throw new Exception("Invalid password.");
    }

    private string EncryptPassword(string password)
    {
        return _passwordHasher.HashPassword(password);
    }
}
