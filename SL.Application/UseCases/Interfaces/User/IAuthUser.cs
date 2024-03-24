using SL.Application.Ports.User;

namespace SL.Application.UseCases.Interfaces.User;

public interface IAuthUser
{
    Task<AuthUserOutput?> ExecuteAsync(AuthUserInput input);
}