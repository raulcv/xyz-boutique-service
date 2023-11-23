using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.businesslayer.Core;
using xyzboutique.common.Entities.Security;

namespace xyzboutique.businesslayer.Manager.HelperManagement;
public interface IHelperManager : IActionManager
{
  #region Helper Date Time

  DateTime ReturnCurrentDate();
  DateTime ConvertDate(string stringdate);
  string ReturnCorrectDate(string date);
  bool ValidateConvertDate(string stringdate);
  string ValidateOrdering(string ordering);
  #endregion

  #region Validate DB
  // int ValidateEntityRegister(string token, string entity, int type, string id);
  bool ValidateIdRol(string id);
  bool ValidateIdUser(string id);
  // bool ValidateUserAdministrator(User user);
  string ReturnUserEmail(string id);
  bool ReturnPasswordIsExpired(string id, int type);
  bool ReturnPasswordIsRepeated(string id, string password, int type);
  #endregion

  #region Contraseña Encriptada
  string GenerateSalt();
  string EncryptText(string text, string salt);
  string DecryptText(string cipherText, string salt);
  string EncryptTextMD5(string text);
  #endregion

  #region Contraseña Segura
bool ValidateEmail(string email);
  bool SecurePasswordList(string password);
  bool SecurePasswordNumber(string passwordNotVerified);
  bool SecurePasswordCharacter(String passwordNotVerified);
  bool SecurePasswordWord(String passwordNotVerified);
  bool SecurePasswordUpperCaseWord(String passwordNotVerified);
  //raulcv
  #endregion


  #region User Account
  string ReturnPersonNameByToken(string token);
  string ReturnIdUserByToken(string token);
  string ReturnUserNameByToken(string token);

  #endregion
  bool ValidateUserRoleState(string id);

  #region validate Text
  bool ValidateIsXSS(string verifyText);
  bool ValidateIsSQL(string verifyText);
  bool ValidateTextLength(string text, int quantity);

  #endregion
}