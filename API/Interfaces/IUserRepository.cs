using API.Dtos;
using API.Entities;

namespace API.interfaces;

public interface IUserRepository
{
    void UpdateUser(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetAllUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int Id);
    Task<AppUser?> GetUserByNameAsync(string name);

    Task<IEnumerable<MemberDto>> GetMembersAsync();
    Task<MemberDto?> GetMemberAsync(string name);
}