using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class RegisterUser
{
  [Required]
  public  string? UserName{get;set;}
  [Required]
  [StringLength(8,MinimumLength =4)]
  public  string? PassWord{get;set;}
}