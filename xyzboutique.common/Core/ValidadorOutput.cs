using System;
using System.Collections.Generic;
using System.Text;

namespace xyzboutique.common.Core;
public class OutputValidator
{
  public bool Exists { get; set; }
  public string Id { get; set; }

  public OutputValidator()
  {
    Exists = false;
    this.Id = string.Empty;
  }

  public OutputValidator(bool Exists, string id)
  {
    this.Exists = Exists;
    this.Id = string.Empty;
  }
  public OutputValidator(string id)
  {
    this.Exists = true;
    this.Id = string.Empty;
  }
}