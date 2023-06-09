using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Interfaces.Entities;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Base;

public class BaseEntity : IBaseEntity
{
    [Column(IsPrimary = true)] public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
