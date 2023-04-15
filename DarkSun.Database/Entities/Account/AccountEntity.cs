using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Player;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Account;

[Table(Name = "accounts")]
public class AccountEntity : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;
    public bool IsEnabled { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginDate { get; set; }
    public ICollection<PlayerEntity> Players { get; set; } = null!;
}
