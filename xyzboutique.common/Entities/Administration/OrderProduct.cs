using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xyzboutique.common.Core;

namespace xyzboutique.common.Entities.Administration;

[Description("To store Order information")]
[Table("OrderProducts", Schema = "Administration")]
public class OrderProduct : BaseEntityLog
{

  [Key]
  [Column("IdOrderProducts")]
  public string Id { get; set; }

  [Column("IdProduct")]
  public string IdProduct { get; set; }

  [Column("IdOrder")]
  public Guid IdOrder { get; set; }

  public OrderProduct()
  {
  }
}