using API.Dtos;
using API.Entities;
using API.Helpers;
using API.interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        return await context.Users.Include(x => x.Photos).ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int Id)
    {
        return await context.Users.FindAsync(Id);
    }

    public async Task<AppUser?> GetUserByNameAsync(string name)
    {
        return await context.Users.Include(x => x.Photos).SingleOrDefaultAsync(a => a.UserName == name);
    }

    public void UpdateUser(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = context.Users.AsQueryable();
        query = query.Where(a => a.UserName != userParams.CurrentUserName);
        if (userParams.Gender != null)
        {
            query = query.Where(a => a.Gender == userParams.Gender);
        }
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
        query = query.Where(a => a.DateOfBirth >= maxDob && a.DateOfBirth <= minDob);
        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x=>x.LastActive)

        };
        return await PageList<MemberDto>.CreateAsynce(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider),
        userParams.PageNumber, userParams.PageSize);
    }

    public async Task<MemberDto?> GetMemberAsync(string name)
    {
        return await context.Users.Where(a => a.UserName == name).ProjectTo<MemberDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }
}