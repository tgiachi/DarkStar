using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Objects;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Items.WorldObjects.Base;
using DarkStar.Api.World.Types.GameObjects;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Items.WorldObjects;

//[GameObjectAction(GameObjectType.Internal_World_Spawner)]
public class TestSchedulerObject : BaseScheduledWorldObject
{
    public TestSchedulerObject(ILogger<TestSchedulerObject> logger, IDarkSunEngine engine) : base(logger, engine)
    {
        SetInterval(4000);
    }
}
