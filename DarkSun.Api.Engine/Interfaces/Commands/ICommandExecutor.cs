using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Api.Engine.Interfaces.Commands
{
    public interface ICommandExecutor
    {
        double TargetTimeMills { get; set; }
    }
}
