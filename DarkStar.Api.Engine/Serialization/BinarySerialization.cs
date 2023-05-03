using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace DarkStar.Api.Engine.Serialization;

public class BinarySerialization
{
    public static ValueTask<bool> SerializeToFileAsync<TEntity>(TEntity entity, string fileName)
    {
        using var fileStream = File.Create(fileName);
        Serializer.Serialize(fileStream, entity);
        return new ValueTask<bool>(true);
    }
}
