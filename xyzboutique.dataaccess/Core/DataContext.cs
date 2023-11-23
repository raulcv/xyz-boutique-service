using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using xyzboutique.common.System;
using xyzboutique.common.Entities.Security;
using xyzboutique.Common.Entities.Security;
using xyzboutique.common.Entities.Administration;

namespace xyzboutique.dataaccess.Core;

public class DataContext : DbContext
{
  public DataContext()
  {
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    base.OnConfiguring(optionsBuilder);
    optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("XYZBOUTIQUEDB"));
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
  }

  public virtual void Save()
  {
    base.SaveChanges();
  }

  #region Entities representing Database Objects

  public DbSet<Order> orders { get; set; }
  public DbSet<OrderProduct> orderProducts { get; set; }
  public DbSet<OrderStatus> orderStatuses { get; set; }
  public DbSet<Product> Products { get; set; }

  #region Security
  public DbSet<Role> Roles { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<UserRole> UserRoles { get; set; }
  public DbSet<UserToken> userTokens { get; set; }

  #endregion
  public DbSet<UserHash> UserHashs { get; set; }

  #endregion

}

