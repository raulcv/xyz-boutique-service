using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xyzboutique.common.Core;

namespace xyzboutique.common.Entities.Security;
[Description("To store UserRole information")]
[Table("UserRole", Schema = "Security")]
public class UserRole : BaseEntityLog
{
  [Key]
  [Column("IdUserRole")]
  public Guid Id { get; set; }

  [Column("IdUser")]
  public Guid IdUser { get; set; }

  [Column("IdRole")]
  public Guid IdRole { get; set; }
}