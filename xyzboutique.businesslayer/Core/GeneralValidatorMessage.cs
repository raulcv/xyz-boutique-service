namespace xyzboutique.businesslayer.Core;
class GeneralValidatorMessage
{
  public string key { get; set; }
  public IList<string> messages { get; set; }

  public GeneralValidatorMessage()
  {
    key = string.Empty;
    this.messages = new List<string>();
  }
}