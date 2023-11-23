using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xyzboutique.common.Core;

namespace xyzboutique.common.Entities.Security;

[Description("To store User Role information")]
[Table("Role", Schema = "Securirty")]
public class Role : BaseEntityLog
{
  [Key]
  [Column("IdRole")]
  public Guid Id { get; set; }

  [Column("Name")]
  public string Name { get; set; }

  [Column("State")]
  public int State { get; set; }

  public Role()
  {
    this.Name = string.Empty;
    this.State = 0;
  }
}

