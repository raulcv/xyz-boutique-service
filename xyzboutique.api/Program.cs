using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.OpenApi.Models;
using xyzboutique.businesslayer.Manager.RoleManagement;
using xyzboutique.dataaccess.Core;
using xyzboutique.dataaccess.Filters;
using xyzboutique.businesslayer.Manager.HelperManagement;
using xyzboutique.businesslayer.Core;
using xyzboutique.common.Configuration;
using xyzboutique.businesslayer.Manager.UserManagement;
using xyzboutique.BusinessLayer.Manager.LoginManagement;
using xyzboutique.businesslayer.Manager.OrderManagement;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbFactory, DbFactory>();
builder.Services.AddScoped<DataContext>();
builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddScoped<IHelperManager, HelperManager>();

builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IRoleManager, RoleManager>();
builder.Services.AddScoped<ILoginManager, LoginManager>();
builder.Services.AddScoped<IOrderManager, OrderManager>();


builder.Services.AddScoped<BusinessManagerFactory>();

builder.Services.AddControllers();

// builder.Services.AddHealthChecks()
//     .AddCheck("DB Health Check", () => DbHealthCheckProvider.Check(
//         Environment.GetEnvironmentVariable("XYZBOUTIQUEDB")
//         ));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
  var groupName = "v1";

  options.SwaggerDoc(groupName, new OpenApiInfo
  {
    Title = $"XYZ Boutique API {groupName}",
    Version = groupName,
    Description = "API XYZ Boutique",
    Contact = new OpenApiContact
    {
      Name = "raulcv",
      Email = "iraulcv@gmail.com",
      Url = new Uri("https://raulcv.com"),
    }
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(u => u.SwaggerEndpoint("/swagger/v1/swagger.json", $"API XYZBoutique V1"));
}

// app.UseEndpoints(endpoints =>
// {

//   endpoints.MapControllers();
//   endpoints.MapHealthChecks("/health", new HealthCheckOptions
//   {
//     Predicate = _ => true,
//     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//   });

// });
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
