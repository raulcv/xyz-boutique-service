using System.ComponentModel.DataAnnotations.Schema;

namespace xyzboutique.common.Core;

public class BaseEntity
{
  [Column("Deleted")]
  public bool Deleted { get; set; }

  public BaseEntity()
  {

    Deleted = false;
  }
}

