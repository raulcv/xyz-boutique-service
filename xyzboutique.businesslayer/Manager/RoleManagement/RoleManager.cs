using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using xyzboutique.businesslayer.Core;
using xyzboutique.businesslayer.Manager.HelperManagement;
using xyzboutique.common.BusinessObjects.Security;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;
using xyzboutique.dataaccess.Core;

namespace xyzboutique.businesslayer.Manager.RoleManagement;
public class RoleManager : IRoleManager
{
  IRepository _repository;
  IHelperManager _helperManager;
  IUnitOfWork _unitOfWork;
  ILogger<RoleManager> _logger;
  GeneralValidator _generalValidator;
  public RoleManager(IRepository repository, ILogger<RoleManager> logger, IUnitOfWork unitOfWork, BusinessManagerFactory businessManagerFactory) : base()
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

  public CheckStatus Create(BaseInputEntity entity)
  {
    throw new NotImplementedException();
  }

  public CheckStatus Delete(BaseInputDelete entity)
  {
    throw new NotImplementedException();
  }

  public CheckStatus RoleStatus(BaseInputEntity entity)
  {
    throw new NotImplementedException();
  }

  public CheckStatus RolImport(BaseInputEntity entity)
  {
    throw new NotImplementedException();
  }

  public void SaveChanges()
  {
    throw new NotImplementedException();
  }

  private CheckStatus ValidateSearch(RoleQueryInput input)
  {
    _generalValidator.Validate<RoleQueryInput>(input, string.Empty);
    return _generalValidator.GetStatus();
  }
  public DataQuery Search(DataQueryInput input)
  {
    RoleQueryInput queryInput = (RoleQueryInput)input;
    CheckStatus checkstatus = ValidateSearch(queryInput);
    DataQuery data = null;
    if (checkstatus.ApiState.Equals(Status.Ok))
    {
      SqlParameter[] sqlParams =
      {
        new SqlParameter("@state", queryInput.State),
        new SqlParameter("@text", queryInput.Text),
        new SqlParameter("@orderby", queryInput.OrderBy),
        new SqlParameter("@quantity", queryInput.Quantity),
        new SqlParameter("@page", queryInput.Page),
      };
      data = _repository.ExecuteProcedureQuery(DataBaseQuery.SecRoleSearch, sqlParams, Message.RolePlural);
    }
    else
    {
      data = new DataQuery(checkstatus.ApiState, checkstatus.ApiMessage);
    }
    return data;
  }
  public MemoryStream SearchExport(DataQueryInput input)
  {
    throw new NotImplementedException();
  }

  public SingleQuery SingleById(string id, string token)
  {
    throw new NotImplementedException();
  }

  public CheckStatus Update(BaseInputEntity entity)
  {
    throw new NotImplementedException();
  }
}