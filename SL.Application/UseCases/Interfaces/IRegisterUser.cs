using SL.Application.Ports.User;

namespace SL.Application.UseCases.Interfaces;

public interface IRegisterUser
{
    Task ExecuteAsync(RegisterUserInput input);
}
