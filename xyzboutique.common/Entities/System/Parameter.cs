using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using xyzboutique.common.Core;

namespace xyzboutique.common.System;

[Description("To store General information")]
[Table("Parameter", Schema = "System")]
public class Parameter : BaseEntity
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Column("IdParameter")]
  public Guid Id { get; set; }

  [Column("Name")]
  public string Nombre { get; set; }

  [Column("Val")]
  public string Val { get; set; }
}
