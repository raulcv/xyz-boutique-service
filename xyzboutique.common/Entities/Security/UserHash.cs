using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xyzboutique.common.Core;

namespace xyzboutique.common.Entities.Security;

[Description("To store user HASH")]
[Table("UserHashes", Schema = "Security")]
public class UserHash : BaseEntity
{
    [Key]
    [Column("IdUserHash")]
    public Guid Id { get; set; }

    [Column("IdUser")]
    public Guid IdUser { get; set; }

    [Column("Hash")]
    public string Hash { get; set; }

    [Column("Salt")]
    public string Salt { get; set; }

    [Column("DateCreation")]
    public DateTime? DateCreation { get; set; }

    public UserHash()
    {
        DateCreation = DateTime.Now;
        Hash = string.Empty;
        Salt = string.Empty;
    }
}