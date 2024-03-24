using SL.Application.Ports.User;

namespace SL.Application.UseCases.Interfaces.User;

public interface IRegisterUser
{
    Task ExecuteAsync(RegisterUserInput input);
}
