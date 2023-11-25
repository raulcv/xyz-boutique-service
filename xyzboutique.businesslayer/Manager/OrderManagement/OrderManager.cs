using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using xyzboutique.businesslayer.Core;
using xyzboutique.businesslayer.Manager.HelperManagement;
using xyzboutique.common.BusinessObjects.Security;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;
using xyzboutique.common.Entities.Administration;
using xyzboutique.dataaccess.Core;

namespace xyzboutique.businesslayer.Manager.OrderManagement;
public class OrderManager : IOrderManager
{
  IRepository _repository;
  IHelperManager _helperManager;
  IUnitOfWork _unitOfWork;
  ILogger<OrderManager> _logger;
  GeneralValidator _generalValidator;
  public OrderManager(IRepository repository, ILogger<OrderManager> logger, IUnitOfWork unitOfWork, BusinessManagerFactory businessManagerFactory) : base()
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

  private CheckStatus Validate(OrderInput input, string accion = "")
  {
    _generalValidator.Validate<OrderInput>(input, accion);
    return _generalValidator.GetStatus();
  }

  public CheckStatus Create(BaseInputEntity entity)
  {
    OrderInput input = (OrderInput)entity;
    CheckStatus checkstatus = Validate(input);

    if (checkstatus.ApiState.Equals(Status.Ok))
    {
      DateTime date = _helperManager.ReturnCurrentDate();
      string id = "";
      Order order = new Order()
      {
        IdCustomer = new Guid(input.IdCustomer),
        IdEmployee = new Guid(input.IdEmployee),
        IdOrderState = new Guid(input.IdOrderState),
        OrderNumber = input.OrderNumber.Trim(),
        OrderDate = _helperManager.ConvertDate(input.OrderDate),
        ShippingDate = _helperManager.ConvertDate(input.ShippingDate),
        DeliveryDate = _helperManager.ConvertDate(input.DeliveryDate),
        ReceptionDate = _helperManager.ConvertDate(input.ReceptionDate),

        DateCreation = date,
        DateUpdate = date,
        UserCreation = input.UserName,
        UserUpdate = input.UserName,
        Deleted = false,
      };

      _logger.LogInformation("Creating record for {0}", this.GetType());
      _repository.Create<Order>(order);
      SaveChanges();
      _logger.LogInformation("Record saved for {0}", this.GetType());

      checkstatus = new CheckStatus(order.Id.ToString(), order.OrderNumber, Status.Ok, string.Format(Message.Save, Message.User));

      id = order.Id.ToString();

      if (checkstatus.ApiState.Equals(Status.Ok))
      {
        if (input.orderProductInputs.Count() > 0)
        {
          OrderProduct orderProduct = new OrderProduct();

          foreach (var item in input.orderProductInputs)
          {

            orderProduct = new OrderProduct()
            {
              IdProduct = new Guid(item.IdProduct),
              IdOrder = new Guid(id),
              DateCreation = date,
              DateUpdate = date,
              UserCreation = input.UserName,
              UserUpdate = input.UserName,
              Deleted = false,
            };
            _logger.LogInformation("Order Product Creating record for {0}", this.GetType());
            _repository.Create<OrderProduct>(orderProduct);
            _logger.LogInformation("Order Product Record saved for {0}", this.GetType());
            SaveChanges();
          }
        }
      }
    }
    else
    {
      checkstatus.ApiMessage = checkstatus.ApiMessage.Replace("OtherCharacter", "|");
    }
    return checkstatus;
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

  public CheckStatus ChangeOrderStatus(string idOrder)
  {

    CheckStatus checkstatus = new CheckStatus();

    Order order = _repository.Find<Order>(f => f.Id.Equals(new Guid(idOrder)));

    // Order orderFind = order.Where(o => o.Id.Equals(id)).FirstOrDefault();

    Guid IdnextStatus = Guid.Empty;

    if (order.Id != Guid.Empty)
    {
      OrderStatus orderStatus = _repository.Find<OrderStatus>(f => f.Id.Equals(order.IdOrderState));

      var NextOrderStatus = new OrderStatus();

      if (orderStatus.Id != Guid.Empty)
      {
        if (orderStatus.Name.Equals("Por Atender"))
        {
          NextOrderStatus = _repository.Find<OrderStatus>(f => f.Name.Equals("En Proceso"));
          order.OrderDate = _helperManager.ReturnCurrentDate();
        }
        else if (orderStatus.Name.Equals("En Proceso"))
        {
          NextOrderStatus = _repository.Find<OrderStatus>(f => f.Name.Equals("En Delivery"));
          order.ReceptionDate = _helperManager.ReturnCurrentDate();
        }
        else if (orderStatus.Name.Equals("En Delivery"))
        {
          NextOrderStatus = _repository.Find<OrderStatus>(f => f.Name.Equals("Recibido"));
          order.DeliveryDate = _helperManager.ReturnCurrentDate();
        }
        else if (orderStatus.Name.Equals("Recibido"))
        {
          NextOrderStatus = _repository.Find<OrderStatus>(f => f.Name.Equals("Recibido"));
          order.DeliveryDate = _helperManager.ReturnCurrentDate();
        }
        order.IdOrderState = NextOrderStatus.Id;
      }

      DateTime date = _helperManager.ReturnCurrentDate();

      _logger.LogInformation("Creating record for {0}", this.GetType());
      _repository.Update<Order>(order);
      SaveChanges();
      _logger.LogInformation("Record saved for {0}", this.GetType());

      checkstatus = new CheckStatus(order.Id.ToString(), order.OrderNumber, Status.Ok, string.Format(Message.Save, Message.User));
    }
    else
    {
      checkstatus.ApiMessage = checkstatus.ApiMessage.Replace("OtherCharacter", "|");
    }
    return checkstatus;
  }

  public void SaveChanges()
  {
    _unitOfWork.SaveChanges();
  }

}