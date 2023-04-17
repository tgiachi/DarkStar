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
using DarkStar.Network.Protocol.Types;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners
{
    [NetworkMessageListener(DarkStarMessageType.AccountCreateRequest)]
    public class AccountCreationMessageListener : BaseNetworkMessageListener<AccountCreateRequestMessage>
    {
        public AccountCreationMessageListener(ILogger<BaseNetworkMessageListener<AccountCreateRequestMessage>> logger,
            IDarkSunEngine engine) : base(logger, engine)
        {
        }


        public override async Task<List<IDarkSunNetworkMessage>> OnMessageReceivedAsync(Guid sessionId,
            DarkStarMessageType messageType, AccountCreateRequestMessage message)
        {
            var userExists = await Engine.DatabaseService.QueryAsSingleAsync<AccountEntity>(entity =>
                entity.Email.ToLower() == message.Email.ToLower());

            if (userExists != null!)
            {
                return SingleMessage(new AccountCreateResponseMessage(false, "Account already exists"));
            }

            await Engine.DatabaseService.InsertAsync(new AccountEntity
            {
                Email = message.Email,
                PasswordHash = message.Password.CreateBCryptHash(),
                RegistrationDate = DateTime.UtcNow,
                IsEnabled = true
            });

            Logger.LogInformation("Account registered: {Email}", message.Email);

            return SingleMessage(new AccountCreateResponseMessage(true, "Account created"));
        }
    }
}
