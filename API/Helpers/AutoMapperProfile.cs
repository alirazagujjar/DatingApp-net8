using API.Dtos;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfile:Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser,MemberDto>().
        ForMember(v=>v.Age,m=>m.MapFrom(s=>s.DateOfBirth.CalculateAge())).
        ForMember(v=>v.PhotoUrl,m=>m.MapFrom(c=>c.Photos.FirstOrDefault(x=>x.IsMain)!.Url));
        CreateMap<Photo,PhotoDto>();
    }
}