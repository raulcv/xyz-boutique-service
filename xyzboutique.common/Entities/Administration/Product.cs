using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xyzboutique.common.Core;

namespace xyzboutique.common.Entities.Administration;

[Description("To store Product information")]
[Table("Products", Schema = "Administration")]
public class Product : BaseEntityLog
{

  [Key]
  [Column("IdProduct")]
  public string Id { get; set; }

  [Column("IdProductType")]
  public Guid IdProductType { get; set; }

  [Column("IdMeasurement")]
  public Guid IdMeasurement { get; set; }

  [Column("Sku")]
  public string Sku { get; set; }

  [Column("Name")]
  public string Name { get; set; }

  [Column("Labels")]
  public string Labels { get; set; }

  [Column("Price")]
  public decimal Price { get; set; }
  public Product()
  {
    this.Sku = string.Empty;
    this.Name = string.Empty;
    this.Labels = string.Empty;
    this.Price = 0;
  }
}