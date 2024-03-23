using SL.Domain.Models.Users;

namespace SL.Domain.Adapters;

public interface IUsersAdapter
{
    Task<UserModel?> FindUserByEmail(string email);
    Task CreateUser(CreateUserRequest request);
}
