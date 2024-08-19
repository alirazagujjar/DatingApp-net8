using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class RegisterUser
{
  [Required] public  string UserName{get;set;} = string.Empty;
  [Required] public  string? KnownAs{get;set;}
  [Required] public  string? Gender{get;set;}
  [Required] public  string? DateOfBirth{get;set;}
  [Required] public  string? City{get;set;}
  [Required] public  string? Country{get;set;}
  [Required]
  [StringLength(8,MinimumLength =6)]
  public string PassWord{get;set;}=string.Empty;
}