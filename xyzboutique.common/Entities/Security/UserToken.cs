using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using xyzboutique.common.Core;

namespace xyzboutique.Common.Entities.Security;

[Description("To store User tOEKN information")]
[Table("UserTokens", Schema = "Security")]
public class UserToken : BaseEntity
{
    [Key]
    [Column("IdUserToken")]
    public Guid Id { get; set; }

    [Column("Token")]
    public string Token { get; set; }

    [Column("DateCreation")]
    public DateTime DateCreation { get; set; }

    [Column("DateExpiration")]
    public DateTime DateExpiration { get; set; }

    [Column("IdUser")]
    public Guid IdUser { get; set; }

    public UserToken()
    {
        Token = string.Empty;
    }
}