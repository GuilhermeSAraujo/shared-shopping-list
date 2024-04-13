using SL.Application.Ports.List;

namespace SL.Application.UseCases.Interfaces.List;

public interface ICreateList
{
    Task ExecuteAsync(CreateListInput input);
}
