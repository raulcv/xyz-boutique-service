using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Globalization;
using xyzboutique.dataaccess.Core;
using xyzboutique.businesslayer.Manager.HelperManagement;
using xyzboutique.businesslayer.Core;
using xyzboutique.common.BusinessObjects.Security;
using xyzboutique.common.Core;
using xyzboutique.common.Configuration;
using xyzboutique.common.Entities.Security;

namespace xyzboutique.businesslayer.Manager.UserManagement;
public class UserManager : IUserManager
{
    IRepository _repository;
    IHelperManager _helperManager;
    IUnitOfWork _unitOfWork;
    ILogger<UserManager> _logger;
    GeneralValidator _generalValidator;
    public UserManager(IRepository repository, ILogger<UserManager> logger, IUnitOfWork unitOfWork, BusinessManagerFactory businessManagerFactory
         ) : base()
    {
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _helperManager = businessManagerFactory.GetHelperManager();
        _generalValidator = new GeneralValidator(_repository, _unitOfWork);
    }

    public IUnitOfWork UnitOfWork
    {
        get
        {
            return _unitOfWork;
        }
    }

    #region Search
    public SingleQuery SingleById(string id, string token)
    {
        UserIndOutput usuario = new UserIndOutput();

        usuario = _repository.ExecuteProcedureSingle<UserIndOutput>(DataBaseQuery.SegUserInd, id);
        if (usuario == null)
        {
            usuario = new UserIndOutput
            {
                ApiMessage = string.Format(Message.NotExists, Message.User),
                ApiState = Status.Error
            };

        }
        return usuario;
    }

    private CheckStatus ValidateSearch(UserQueryInput input)
    {
        _generalValidator.Validate<UserQueryInput>(input, string.Empty);
        return _generalValidator.GetStatus();
    }

    public DataQuery Search(DataQueryInput input)
    {
        UserQueryInput queryInput = (UserQueryInput)input;
        CheckStatus checkstatus = ValidateSearch(queryInput);
        DataQuery data = null;

        if (checkstatus.ApiState.Equals(Status.Ok))
        {
            SqlParameter[] sqlParams =
                {
                    new SqlParameter("@text", queryInput.Text),
                    new SqlParameter("@email", queryInput.Email),
                    new SqlParameter("@idRole", queryInput.IdRole),
                    new SqlParameter("@orderby", queryInput.OrderBy),
                    new SqlParameter("@page", queryInput.Page),
                    new SqlParameter("@quantity", queryInput.Quantity)
                };

            data = _repository.ExecuteProcedureQuery(DataBaseQuery.SegUserSearch, sqlParams, Message.UserPlural);
        }
        else
        {
            data = new DataQuery(checkstatus.ApiState, checkstatus.ApiMessage);
        }
        return data;
    }

    #endregion

    #region Create edit 

    private CheckStatus Validate(UserInput input, string accion = "")
    {
        _generalValidator.Validate<UserInput>(input, accion);
        return _generalValidator.GetStatus();
    }

    public CheckStatus Create(BaseInputEntity entity)
    {
        UserInput input = (UserInput)entity;
        CheckStatus checkstatus = Validate(input);

        if (checkstatus.ApiState.Equals(Status.Ok))
        {
            DateTime date = _helperManager.ReturnCurrentDate();
            string id = "";
            User user = new User()
            {
                IdRole = new Guid(input.idRole),
                Code = input.Code,
                Name = input.name.Trim(),
                Email = input.email.Trim(),
                State = input.state,
                Phone = input.phone,

                DateCreation = date,
                DateUpdate = date,
                UserCreation = input.UserName,
                UserUpdate = input.UserName,
                Deleted = false,
            };

            /* save user */
            _logger.LogInformation("Creating record for {0}", this.GetType());
            _repository.Create<User>(user);
            SaveChanges();
            _logger.LogInformation("Record saved for {0}", this.GetType());
            checkstatus = new CheckStatus(user.Id.ToString(), user.Code, Status.Ok,
                string.Format(Message.Save, Message.User));

            id = user.Id.ToString();

            if (checkstatus.ApiState.Equals(Status.Ok))
            {
                /*create user Hash*/
                if (string.IsNullOrEmpty(input.password))
                {
                    input.password = generatePassword();
                }

                //generar Contraseña
                string salt = _helperManager.GenerateSalt();
                string hash = _helperManager.EncryptText(input.password, salt);

                UserHash userHash = new UserHash();

                userHash = new UserHash()
                {
                    IdUser = new Guid(id),
                    Salt = salt,
                    Hash = hash,
                    DateCreation = date,
                    Deleted = false,
                };

                _repository.Create<UserHash>(userHash);
                SaveChanges();
            }
        }
        else
        {
            checkstatus.ApiMessage = checkstatus.ApiMessage.Replace("OtherCharacter", "|");
        }
        return checkstatus;
    }

    public CheckStatus Update(BaseInputEntity entity)
    {
        UserInput input = (UserInput)entity;
        CheckStatus checkstatus = Validate(input, Status.Update);
        if (checkstatus.ApiState.Equals(Status.Ok))
        {
            // input.UserName = _helperManager.DevolverNombreUsuarioPorToken(input.idUsuario);
            DateTime fecha = _helperManager.ReturnCurrentDate();
            User user = _repository.Single<User>(p => p.Id.ToString().Equals(input.id));

            if (user != null)
            {
                string oldAntiguo = user.Name;
                
                user.Email = input.email;
                user.State = input.state;
                user.DateUpdate = fecha;
                user.UserUpdate = input.UserName;
                user.Deleted = false;
                user.Phone = input.phone;

                _logger.LogInformation("Updating record for {0}", this.GetType());
                _repository.Update<User>(user);
                SaveChanges();
                _logger.LogInformation("Record saved for {0}", this.GetType());

                checkstatus = new CheckStatus(user.Id.ToString(), user.Code, Status.Ok,
                    string.Format(Message.Save, Message.User));

                if (checkstatus.ApiState.Equals(Status.Ok))
                {

                    if (input.changePassowrd)
                    {
                        //Delete the last password
                        _repository.ExecuteProcedure(DataBaseQuery.SegUsuarioHashEliminar, user.Id);

                        if (string.IsNullOrEmpty(input.password))
                        {
                            input.password = generatePassword();
                        }

                        // generar contraseña encriptada
                        string salt = _helperManager.GenerateSalt();
                        string hash = _helperManager.EncryptText(input.password, salt);
                        UserHash userHash = new UserHash()
                        {
                            IdUser = user.Id,
                            Salt = salt,
                            Hash = hash,
                            DateCreation = fecha,
                            Deleted = false,
                        };
                        _repository.Create<UserHash>(userHash);
                        SaveChanges();
                    }
                }
            }
            else
            {
                checkstatus = new CheckStatus(Status.Error, string.Format(Message.NotExists, Message.User));
            }
        }
        else
        {
            checkstatus.ApiMessage = checkstatus.ApiMessage.Replace("otherCharacter", "|");
        }
        return checkstatus;
    }

    #endregion
    #region delete
    public CheckStatus Delete(BaseInputDelete entity)
    {
        throw new NotImplementedException();
    }

    #endregion


    private string generatePassword()
    {
        string randomPassword = string.Empty;
        Random rdn = new Random();
        string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
        int length = characters.Length;
        char word;
        int longitudContrasenia = 8;

        for (int i = 0; i < longitudContrasenia; i++)
        {
            word = characters[rdn.Next(length)];
            randomPassword += length.ToString();
        }

        return randomPassword;
    }
    public void SaveChanges()
    {
        _unitOfWork.SaveChanges();
    }

}