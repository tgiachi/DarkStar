using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Engine.Types.Commands;

public enum CommandActionType : short
{
    PlayerMove,
    ItemAction,
    GameObjectAction
}
