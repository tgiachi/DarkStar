

using System.Text;
using TinyCsv;
using TinyCsv.Extensions;

namespace DarkStar.Api.Engine.Serialization
{
    public class SeedCsvParser
    {
        private static SeedCsvParser s_instance = null!;
        public static SeedCsvParser Instance => s_instance ??= new();

        public async Task<IEnumerable<TEntity>> ParseAsync<TEntity>(string fileName) where TEntity : class, new()
        {
            var csvReader = new TinyCsv<TEntity>();
            return await csvReader.LoadFromFileAsync(fileName);
        }

        public async Task<bool> WriteHeaderToFileAsync<TEntity>(string fileName, IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            var csvReader = new TinyCsv<TEntity>();
            await csvReader.SaveAsync(fileName, entities);

            return true;
        }

        public async Task<bool> WriteHeaderToFileAsync(string fileName, IEnumerable<object> entities)
        {
            var csvReader = new TinyCsv<object>();
            await csvReader.SaveAsync(fileName, entities);

            return true;
        }

        public async Task<string> WriteHeaderToStringAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            var csvReader = new TinyCsv<TEntity>();
            using var csvDumpStream = new MemoryStream();
            await csvReader.SaveAsync(csvDumpStream, entities);

            return Encoding.UTF8.GetString(csvDumpStream.ToArray());
        }
    }
}
