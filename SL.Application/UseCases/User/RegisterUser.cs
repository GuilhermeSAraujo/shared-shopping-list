using Microsoft.AspNet.Identity;
using SL.Application.Ports.User;
using SL.Application.UseCases.Interfaces.User;
using SL.Domain.Adapters;

namespace SL.Application.UseCases.User;

public class RegisterUser(IUsersAdapter usersAdapter) : IRegisterUser
{
    private readonly IUsersAdapter _usersAdapter = usersAdapter
            ?? throw new ArgumentNullException(nameof(usersAdapter));

    private readonly PasswordHasher _passwordHasher = new();

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
