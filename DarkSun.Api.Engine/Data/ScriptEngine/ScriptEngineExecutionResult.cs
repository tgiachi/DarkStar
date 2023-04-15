using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Engine.Data.ScriptEngine
{
    public class ScriptEngineExecutionResult
    {
        public object[] Result { get; set; } = null!;
        public Exception? Exception { get; set; }
    }
}
