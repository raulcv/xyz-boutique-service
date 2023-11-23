using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xyzboutique.common.Core;

namespace xyzboutique.common.Entities.Security;

[Description("To store User information")]
[Table("Users", Schema = "Security")]
public class User : BaseEntityLog
{
  [Key]
  [Column("IdUser")]
  public Guid Id { get; set; }

  [Column("IdRole")]
  public Guid IdRole { get; set; }

  [Column("Code")]
  public string Code { get; set; }

  [Column("Name")]
  public string Name { get; set; }
  
  [Column("Email")]
  public string Email { get; set; }
  
  [Column("Phone")]
  public string Phone { get; set; }
  
  [Column("State")]
  public int State { get; set; }

  public User()
  {
    Code = string.Empty;
    Name = string.Empty;
    Email = string.Empty;
    State = 0;
    Phone = string.Empty;
  }
}
