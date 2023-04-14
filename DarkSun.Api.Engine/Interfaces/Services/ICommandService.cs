using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Interfaces.Commands;
using DarkSun.Api.Engine.Interfaces.Services.Base;

namespace DarkSun.Api.Engine.Interfaces.Services
{
    public interface ICommandService : IDarkSunEngineService
    {
        void EnqueueNpcAction<ActionEntity>(ActionEntity entity) where ActionEntity : ICommandAction;

        void EnqueuePlayerAction<ActionEntity>(ActionEntity entity) where ActionEntity : ICommandAction;
    }
}
