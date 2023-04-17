using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Api.Utils;
using DarkStar.Database.Entities.Account;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Accounts;
using DarkStar.Network.Protocol.Messages.Server;
using DarkStar.Network.Protocol.Types;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners
{
    [NetworkMessageListener(DarkStarMessageType.AccountLoginRequest)]
    public class AccountLoginMessageListener : BaseNetworkMessageListener<AccountLoginRequestMessage>
    {
        public AccountLoginMessageListener(ILogger<BaseNetworkMessageListener<AccountLoginRequestMessage>> logger,
            IDarkSunEngine engine) : base(logger, engine)
        {
        }

        public override async Task<List<IDarkSunNetworkMessage>> OnMessageReceivedAsync(Guid sessionId,
            DarkStarMessageType messageType, AccountLoginRequestMessage message)
        {
            Logger.LogInformation("Received login request from {Id}", sessionId);
            var account = await Engine.DatabaseService.QueryAsSingleAsync<AccountEntity>(entity =>
                entity.Email == message.Email);

            
            if (account == null!)
            {
                Logger.LogWarning("Invalid login attempt from {Id}", sessionId);
                return SingleMessage(new AccountLoginResponseMessage(false));
            }
            else
            {
                if (BCrypt.Net.BCrypt.Verify(message.Password, account.PasswordHash) == false)
                {
                    Logger.LogWarning("Invalid login attempt from {Id}", sessionId);
                    return SingleMessage(new AccountLoginResponseMessage(false));
                }

                Logger.LogInformation("Login successful for {Email}", account.Email);
                Engine.PlayerService.GetSession(sessionId).AccountId = account.Id;
                Engine.PlayerService.GetSession(sessionId).IsLogged = true;
                return MultipleMessages(
                    new AccountLoginResponseMessage(true),
                    new ServerMotdResponseMessage(Engine.ServerMotd)
                );
            }
        }
    }
}
