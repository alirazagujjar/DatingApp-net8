using API.Dtos;
using API.Entities;
using API.Helpers;

namespace API.interfaces;

public interface IUserRepository
{
    void UpdateUser(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetAllUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int Id);
    Task<AppUser?> GetUserByNameAsync(string name);

    Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<MemberDto?> GetMemberAsync(string name);
}