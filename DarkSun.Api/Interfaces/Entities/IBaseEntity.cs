namespace DarkSun.Api.Interfaces.Entities;

public interface IBaseEntity
{
    Guid Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
