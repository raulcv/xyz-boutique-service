using System;

namespace xyzboutique.common.Configuration;
public class TextValidator : Attribute
{
  //Text Obligatory text validation
  public bool IsObligatory { get; set; }
  public string ObligatoryError { get; set; }
  public string Action { get; set; } //For id

  //Validate Exact quantity
  public bool IsExactLength { get; set; }
  public int ExactLength { get; set; }
  public string ExactLengthError { get; set; }

  //Validate Min Max Quantity
  public bool IsMinMaxLength { get; set; }
  public int MinLength { get; set; }
  public int MaxLength { get; set; }
  public string MinMaxLengthError { get; set; }

  #region EthicalHacking

  // XSS text Validaro
  public bool XSS { get; set; }
  public string XSSError { get; set; }

  //SQL inyection Vlidation
  public bool SQLInjection { get; set; }
  public string SQLInjectionError { get; set; }

  #endregion

  public TextValidator()
  {
    IsObligatory = false;
    ObligatoryError = string.Empty;
    Action = string.Empty;

    IsExactLength = false;
    ExactLength = 0;
    ExactLengthError = string.Empty;

    IsMinMaxLength = false;
    MinLength = 0;
    MaxLength = 0;
    MinMaxLengthError = string.Empty;

    XSS = false;
    XSSError = string.Empty;

    SQLInjection = false;
    SQLInjectionError = string.Empty;
  }

}