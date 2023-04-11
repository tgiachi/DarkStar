using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.MessageListeners;
using DarkSun.Api.Utils;
using DarkSun.Database.Entities.Account;
using DarkSun.Network.Protocol.Messages.Accounts;
using DarkSun.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.MessageListeners
{
    [NetworkMessageListener(DarkSunMessageType.AccountLoginRequest)]
    public class AccountLoginMessageListener : BaseNetworkMessageListener<AccountLoginRequestMessage>
    {
        public AccountLoginMessageListener(ILogger<BaseNetworkMessageListener<AccountLoginRequestMessage>> logger, IDarkSunEngine engine) : base(logger, engine)
        {
        }

        public override async Task OnMessageReceivedAsync(Guid sessionId, DarkSunMessageType messageType, AccountLoginRequestMessage message)
        {
            Logger.LogInformation("Received login request from {Id}", sessionId);
            var account = await Engine.DatabaseService.QueryAsSingleAsync<AccountEntity>(entity =>
                entity.Email == message.Email && message.Password == message.Password.CreateMd5Hash());
            if (account == null)
            {
                Logger.LogWarning("Invalid login attempt from {Id}", sessionId);
                await Engine.NetworkServer.SendMessageAsync(sessionId, new AccountLoginResponseMessage(false));
            }
            else
            {
                Logger.LogInformation("Login successful for {Id}", sessionId);
                Engine.PlayerSessionService.GetPlayerSession(sessionId).AccountId = account.Id;
                await Engine.NetworkServer.SendMessageAsync(sessionId, new AccountLoginResponseMessage(true));
            }
        }
    }
}
