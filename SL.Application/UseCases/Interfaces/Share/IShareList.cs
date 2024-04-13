using SL.Application.Ports.Share;

namespace SL.Application.UseCases.Interfaces.Share;

public interface IShareList
{
    Task ExecuteAsync(ShareListInput input);
}