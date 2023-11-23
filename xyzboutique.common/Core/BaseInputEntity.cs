using System.ComponentModel.DataAnnotations.Schema;

namespace xyzboutique.common.Core;
public class BaseInputEntity
{
  [NotMapped]
  public string IdUser { get; set; }
  [NotMapped]
  public string UserName { get; set; }
  [NotMapped]
  public string IpUser { get; set; }

  public BaseInputEntity()
  {
    this.IdUser = string.Empty;
    this.UserName = "raulcv";
    this.IpUser = string.Empty;
  }
}