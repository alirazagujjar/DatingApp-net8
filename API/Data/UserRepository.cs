using API.Dtos;
using API.Entities;
using API.interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context,IMapper mapper) : IUserRepository
{
    public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        return await context.Users.Include(x=>x.Photos).ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int Id)
    {
        return await context.Users.FindAsync(Id);
    }

    public async Task<AppUser?> GetUserByNameAsync(string name)
    {
       return await context.Users.Include(x=>x.Photos).SingleOrDefaultAsync(a=>a.UserName ==name);
    }

    public  void UpdateUser(AppUser user)
    {
        context.Entry(user).State=EntityState.Modified;
    }
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
        return await context.Users.ProjectTo<MemberDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<MemberDto?> GetMemberAsync(string name)
    {
        return await context.Users.Where(a=>a.UserName==name).ProjectTo<MemberDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }
}