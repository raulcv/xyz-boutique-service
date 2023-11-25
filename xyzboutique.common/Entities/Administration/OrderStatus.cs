using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xyzboutique.common.Core;

namespace xyzboutique.common.Entities.Administration;

[Description("To store OrderStatus information")]
[Table("OrderStates", Schema = "Administration")]
public class OrderStatus
{

  [Key]
  [Column("IdOrderState")]
  public Guid Id { get; set; }

  [Column("Name")]
  public string Name { get; set; }

  public OrderStatus()
  {
    this.Name = string.Empty;
  }
}