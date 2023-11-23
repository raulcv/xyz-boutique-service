using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.common.Configuration;

namespace xyzboutique.common.Core;
public class BaseInputDelete
{
  [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.Id", XSS = true, SQLInjection = true, IsMinMaxLength = true, MaxLength = 50)]
  public string Id { get; set; }
  public string IdUser { get; set; }

  public BaseInputDelete()
  {
    this.Id = string.Empty;
    this.IdUser = string.Empty;
  }

  public BaseInputDelete(string id)
  {
    this.Id = id;
    this.IdUser = string.Empty;
  }

  public BaseInputDelete(string id, string idUser)
  {
    this.Id = id;
    this.IdUser = idUser;
  }
}