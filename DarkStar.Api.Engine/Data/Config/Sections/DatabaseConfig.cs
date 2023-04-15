using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Engine.Data.Config.Sections;

public class DatabaseConfig
{
    public string ConnectionString { get; set; } = "Data Source={DATABASE_DIRECTORY}DarkStar.db;Version=3;";
    public DatabaseType DatabaseType { get; set; } = DatabaseType.SqlLite;

    public bool RecreateDatabase { get; set; } = true;
}
