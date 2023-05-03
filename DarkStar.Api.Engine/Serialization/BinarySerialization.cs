using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace DarkStar.Api.Engine.Serialization;

public static class BinarySerialization
{
    public static ValueTask<bool> SerializeToFileAsync<TEntity>(TEntity entity, string fileName)
    {
        using var fileStream = File.Create(fileName);
        Serializer.Serialize(fileStream, entity);
        return new ValueTask<bool>(true);
    }

    public static ValueTask<TEntity> DeserializeFromFileAsync<TEntity>(string fileName)
    {
        using var fileStream = File.OpenRead(fileName);
        return new ValueTask<TEntity>(Serializer.Deserialize<TEntity>(fileStream));
    }
}
