using Microsoft.Extensions.Configuration;

namespace xyzboutique.common.Configuration;

public class Message
{
  private static IConfigurationRoot ConfigurationRoot { get; } = ConfigurationHelper.GetConfiguration(Directory.GetCurrentDirectory(), string.Empty);

  #region Configuration
  public static string TimeZone { get; } = ConfigurationRoot.GetSection("Configuracion")["TimeZone"];

  #endregion

  #region Common Messages

  public static string InvalidOrdering { get; } = ConfigurationRoot.GetSection("Messages")["OrdenamientoNoValido"];
  public static string YouShouldSend { get; } = ConfigurationRoot.GetSection("Messages")["YouShouldSend"];
  public static string YouShouldSendA { get; } = ConfigurationRoot.GetSection("Messages")["YouShouldSendA"];
  public static string YouShouldSendAN { get; } = ConfigurationRoot.GetSection("Messages")["YouShouldSendAN"];
  public static string MaxMinLengthCharacterField { get; } = ConfigurationRoot.GetSection("Messages")["MaxMinLengthCharacterField"];
  public static string MinLenghtCharacterField { get; } = ConfigurationRoot.GetSection("Messages")["MinLenghtCharacterField"];
  public static string FieldCharacterMaxLenth { get; } = ConfigurationRoot.GetSection("Messages")["FieldCharacterMaxLenth"];
  public static string NoEnterValueField { get; } = ConfigurationRoot.GetSection("Messages")["NoEnterValueField"];
  public static string YouShouldEnterValues { get; } = ConfigurationRoot.GetSection("Messages")["YouShouldEnterValues"];
  public static string NotExistDefault { get; } = ConfigurationRoot.GetSection("Messages")["NotExistDefault"];
  public static string NotExists { get; } = ConfigurationRoot.GetSection("Messages")["NotExists"];

  public static string YouShouldSelectAValid { get; set; } = ConfigurationRoot.GetSection("Messages")["YouShouldSelectAValid"];
  public static string YouShouldSelectANValid { get; set; } = ConfigurationRoot.GetSection("Messages")["YouShouldSelectANValid"];
  public static string Type => ConfigurationRoot.GetSection("Messages")["Type"];

  public static string Code { get; } = ConfigurationRoot.GetSection("Messages")["Code"];
  public static string Value { get; } = ConfigurationRoot.GetSection("Messages")["Value"];

  public static string ValidEntity { get; } = ConfigurationRoot.GetSection("Messages")["ValidEntity"];
  public static string WithThis => ConfigurationRoot.GetSection("Messages")["WithThis"];
  public static string Phone => ConfigurationRoot.GetSection("Messages")["Phone"];
  public static string Password => ConfigurationRoot.GetSection("Messages")["Password"];

  public static string Save { get; } = ConfigurationRoot.GetSection("Messages")["Save"];
  public static string Move { get; } = ConfigurationRoot.GetSection("Messages")["Mover"];
  public static string Copy { get; } = ConfigurationRoot.GetSection("Messages")["Copy"];
  public static string Delete { get; } = ConfigurationRoot.GetSection("Messages")["Delete"];

  public static string NoValidInput { get; } = ConfigurationRoot.GetSection("messages")["NoValidInput"];

  public static string PasswordCurrent { get; } = ConfigurationRoot.GetSection("Messages")["PasswordCurrent"];
  public static string RepeatPassword { get; } = ConfigurationRoot.GetSection("Messages")["RepeatPassword"];
  public static string ConfirmPassword { get; } = ConfigurationRoot.GetSection("Messages")["ConfirmPassword"];
  public static string ErrorPassword { get; } = ConfigurationRoot.GetSection("Messages")["ErrorPassword"];
  public static string ErrorPasswordCurrent { get; } = ConfigurationRoot.GetSection("Messages")["ErrorPasswordCurrent"];
  public static string Login { get; } = ConfigurationRoot.GetSection("Messages")["Login"];
  public static string Incorrect { get; } = ConfigurationRoot.GetSection("Messages")["Incorrect"];

  public static string InvalidData { get; } = ConfigurationRoot.GetSection("Messages")["InvalidData"];
  public static string YourUserIsNotAllowed { get; } = ConfigurationRoot.GetSection("Messages")["YourUserIsNotAllowed"];

  #endregion

  #region JWT
  public static string JWTKey { get; } = ConfigurationRoot.GetSection("Jwt")["Key"];
  public static string JWTIssuer { get; } = ConfigurationRoot.GetSection("Jwt")["Issuer"];

  #endregion
  #region Security
  public static string User { get; } = ConfigurationRoot.GetSection("SingularTitle")["User"];
  public static string UserPlural => ConfigurationRoot.GetSection("PluralTitle")["User"];
  public static string Role { get; } = ConfigurationRoot.GetSection("TituloSingular")["Role"];
  public static string RolePlural => ConfigurationRoot.GetSection("PluralTitle")["Role"];
  #endregion

}