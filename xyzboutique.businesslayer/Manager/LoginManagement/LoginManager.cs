using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using xyzboutique.businesslayer.Core;
using xyzboutique.businesslayer.Manager.HelperManagement;
using xyzboutique.common.BusinessObjects.Security;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;
using xyzboutique.Common.Entities.Security;
using xyzboutique.dataaccess.Core;

namespace xyzboutique.BusinessLayer.Manager.LoginManagement;
public class LoginManager : ILoginManager
{
    IRepository _repository;
    IHelperManager _helperManager;
    IUnitOfWork _unitOfWork;
    ILogger<LoginManager> _logger;
    GeneralValidator _generalValidator;

    public LoginManager(IRepository repository,
            ILogger<LoginManager> logger,
            IUnitOfWork unitOfWork,
            BusinessManagerFactory businessManagerFactory
         ) : base()
    {
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _helperManager = businessManagerFactory.GetHelperManager();
        _generalValidator = new GeneralValidator();
    }

    public IUnitOfWork UnitOfWork
    {
        get
        {
            return _unitOfWork;
        }
    }

    #region CRUD
    public DataQuery Search(DataQueryInput input)
    {
        throw new NotImplementedException();
    }

    public SingleQuery SingleById(string id, string token)
    {
        throw new NotImplementedException();
    }


    public CheckStatus Create(BaseInputEntity entity)
    {
        throw new NotImplementedException();
    }
    public CheckStatus Update(BaseInputEntity entity)
    {
        throw new NotImplementedException();
    }
    public CheckStatus Delete(BaseInputDelete entity)
    {
        throw new NotImplementedException();
    }

    #endregion

    public UserLoginIndOutput Login(UserLoginInput input)
    {
        _generalValidator.Validate<UserLoginInput>(input);
        CheckStatus checkstatus = _generalValidator.GetStatus();

        UserLoginIndOutput userIndOutput = new UserLoginIndOutput();

        if (checkstatus.ApiState.Equals(Status.Ok))
        {
            userIndOutput = _repository.ExecuteProcedureSingle<UserLoginIndOutput>
                     (DataBaseQuery.SegUserSearchByUserName,
                         new SqlParameter("@usuario", input.user)
                         ) ?? new UserLoginIndOutput
                         {
                             ApiMessage = Message.InvalidData,
                             ApiState = Status.Error
                         };

            if (userIndOutput.ApiState.Equals(Status.Ok))
            {
                //validate PWD                      
                UserHashIndOutput usuarioHash = new UserHashIndOutput();

                usuarioHash = _repository.ExecuteProcedureSingle<UserHashIndOutput>(DataBaseQuery.SegUserHashByIdUser, new SqlParameter("@id", userIndOutput.id))
                ?? new UserHashIndOutput { ApiMessage = Message.InvalidData, ApiState = Status.Error };

                if (usuarioHash.ApiState.Equals(Status.Error))
                {
                    userIndOutput = new UserLoginIndOutput
                    {
                        ApiMessage = Message.InvalidData,
                        ApiState = Status.Error
                    };
                }
                else
                {
                    if (_helperManager.DecryptText(usuarioHash.hash, usuarioHash.salt).Equals(input.password))
                    {

                        //generate token
                        var claims = new List<Claim> {
                                        new Claim(JwtRegisteredClaimNames.Sub,  userIndOutput.user),
                                        new Claim(JwtRegisteredClaimNames.Email, userIndOutput.email),
                                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                        new Claim(JwtRegisteredClaimNames.UniqueName,userIndOutput.id.ToString()),
                                    };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Message.JWTKey));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        DateTime fechaCreacion = _helperManager.ReturnCurrentDate();
                        DateTime fechaExpiracion = fechaCreacion.AddDays(1);

                        var token = new JwtSecurityToken(Message.JWTIssuer, Message.JWTIssuer, claims, expires: fechaExpiracion, signingCredentials: creds);

                        userIndOutput.token = new JwtSecurityTokenHandler().WriteToken(token);

                        //save token
                        UserToken usertoken = new UserToken()
                        {
                            IdUser = Guid.Parse(userIndOutput.id),
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            DateCreation = fechaCreacion,
                            DateExpiration = fechaExpiracion
                        };

                        _logger.LogInformation("Creating record for {0}", this.GetType());
                        _repository.Create<UserToken>(usertoken);
                        SaveChanges();
                        _logger.LogInformation("Record saved for {0}", this.GetType());
                    }
                    else
                    {
                        userIndOutput.ApiState = Status.Error;
                        userIndOutput.ApiMessage = Message.YourUserIsNotAllowed;
                    }
                }
            }

        }
        else
        {
            userIndOutput.ApiState = Status.Error;
            userIndOutput.ApiMessage = Message.InvalidData;
        }

        return userIndOutput;
    }

    public void SaveChanges()
    {
        _unitOfWork.SaveChanges();
    }


}