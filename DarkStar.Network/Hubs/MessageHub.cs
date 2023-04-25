using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DarkStar.Network.Hubs;
public class MessageHub : Hub
{
    public override Task OnConnectedAsync()
    {
        
       return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception) => base.OnDisconnectedAsync(exception);

    
}
