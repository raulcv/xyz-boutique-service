using System;

namespace xyzboutique.common.Configuration;
public class IntegerValidator : Attribute
{
  public bool IsObligatory { get; set; }
  public string ObligatoryError { get; set; }
  public string Action { get; set; } //for id
  public IntegerValidator()
  {
    this.IsObligatory = false;
    this.ObligatoryError = string.Empty;
    this.Action = string.Empty;
  }
}