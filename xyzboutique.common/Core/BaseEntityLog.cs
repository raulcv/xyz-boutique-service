using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xyzboutique.common.Core;

public class BaseEntityLog : BaseEntity
{
  [Column("UserCreation")]
  public string UserCreation { get; set; }
  [Column("DateCreation")]
  public DateTime DateCreation { get; set; }
  [Column("UserUpdate")]
  public string UserUpdate { get; set; }
  [Column("DateUpdate")]
  public DateTime DateUpdate { get; set; }

  public BaseEntityLog()
  {
    DateTime fechaActual = new DateTime();
    fechaActual = DateTime.Now;
    this.UserCreation = string.Empty;
    this.DateCreation = fechaActual;
    this.UserUpdate = string.Empty;
    this.DateUpdate = fechaActual;
  }
}
