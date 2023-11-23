using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace xyzboutique.common.Core;
public class SingleQuery
{
  [NotMapped]
  public string ApiState { get; set; }
  [NotMapped]
  public string ApiMessage { get; set; }
  public SingleQuery()
  {
    this.ApiState = Status.Ok;
    this.ApiMessage = string.Empty;

  }
}