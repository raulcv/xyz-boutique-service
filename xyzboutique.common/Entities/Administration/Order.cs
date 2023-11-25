using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xyzboutique.common.Core;

namespace xyzboutique.common.Entities.Administration;

[Description("To store Order information")]
[Table("Orders", Schema = "Administration")]
public class Order : BaseEntityLog
{

  [Key]
  [Column("IdOrder")]
  public Guid Id { get; set; }

  [Column("IdCustomer")]
  public Guid IdCustomer { get; set; }

  [Column("IdEmployee")]
  public Guid IdEmployee { get; set; }

  [Column("IdOrderState")]
  public Guid IdOrderState { get; set; }

  [Column("OrderNumber")]
  public string OrderNumber { get; set; }

  [Column("OrderDate")]
  public DateTime OrderDate { get; set; }

  [Column("ReceptionDate")]
  public DateTime ReceptionDate { get; set; }

  [Column("ShippingDate")]
  public DateTime ShippingDate { get; set; }

  [Column("DeliveryDate")]
  public DateTime DeliveryDate { get; set; }

  public Order()
  {
    this.OrderNumber = string.Empty;
  }
}