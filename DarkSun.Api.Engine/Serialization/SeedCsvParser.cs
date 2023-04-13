

using TinyCsv;

namespace DarkSun.Api.Engine.Serialization
{
    public class SeedCsvParser
    {
        private static SeedCsvParser s_instance = null!;
        public static SeedCsvParser Instance => s_instance ??= new();

        public async Task<IEnumerable<TEntity>> ParseAsync<TEntity>(string fileName) where TEntity : class, new()
        {
            var csvReader = new TinyCsv.TinyCsv<TEntity>();
            return await csvReader.LoadFromFileAsync(fileName);
        }

        public async Task<bool> WriteHeaderToFileAsync<TEntity>(string fileName, IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            var csvReader = new TinyCsv.TinyCsv<TEntity>(new CsvOptions<TEntity>() { HasHeaderRecord = true });
            await csvReader.SaveAsync(fileName, entities);

            return true;
        }

        public async Task<bool> WriteHeaderToFileAsync(string fileName, IEnumerable<object> entities)
        {
            var csvReader = new TinyCsv.TinyCsv<object>(new CsvOptions<object>() { HasHeaderRecord = true });
            await csvReader.SaveAsync(fileName, entities);

            return true;
        }

        public async Task<string> WriteHeaderToStringAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            var csvReader = new TinyCsv.TinyCsv<TEntity>();
            using var csvDumpStream = new MemoryStream();
            await csvReader.SaveAsync(csvDumpStream, entities);

            return Encoding.UTF8.GetString(csvDumpStream.ToArray());
        }
    }
}
