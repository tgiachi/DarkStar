using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Commands;
using DarkStar.Api.Engine.Interfaces.Services.Base;

namespace DarkStar.Api.Engine.Interfaces.Services
{
    public interface ICommandService : IDarkSunEngineService
    {
        void EnqueueNpcAction<ActionEntity>(ActionEntity entity) where ActionEntity : ICommandAction;

        void EnqueuePlayerAction<ActionEntity>(ActionEntity entity) where ActionEntity : ICommandAction;
    }
}
