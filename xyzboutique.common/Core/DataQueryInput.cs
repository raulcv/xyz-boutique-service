using System;
using xyzboutique.common.Configuration;

namespace xyzboutique.common.Core;
public class DataQueryInput
{
  [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 200)]
  public string Text { get; set; }

  [TextValidator(SQLInjection = true, XSS = true)]
  public string OrderBy { get; set; }
  public int Page { get; set; }

  public int Quantity { get; set; }
  public string IdUser { get; set; }

  public DataQueryInput()
  {
    this.Text = string.Empty;
    this.OrderBy = string.Empty;
    this.Page = 1;
    this.Quantity = 0;
    this.IdUser = string.Empty;
  }

  public DataQueryInput(string texto, string ordenamiento, int pagina, string idUser)
  {
    this.Text = texto;
    this.OrderBy = ordenamiento;
    this.Page = pagina;
    this.IdUser = idUser;
  }
}