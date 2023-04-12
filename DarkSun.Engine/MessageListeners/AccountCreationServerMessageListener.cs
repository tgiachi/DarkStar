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
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Messages.Accounts;
using DarkSun.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.MessageListeners
{
    [NetworkMessageListener(DarkSunMessageType.AccountCreateRequest)]
    public class AccountCreationMessageListener : BaseNetworkMessageListener<AccountCreateRequestMessage>
    {
        public AccountCreationMessageListener(ILogger<BaseNetworkMessageListener<AccountCreateRequestMessage>> logger, IDarkSunEngine engine) : base(logger, engine)
        {
        }


        public override async Task<List<IDarkSunNetworkMessage>> OnMessageReceivedAsync(Guid sessionId, DarkSunMessageType messageType, AccountCreateRequestMessage message)
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
                PasswordHash = message.Password.CreateMd5Hash(),
                RegistrationDate = DateTime.UtcNow,
                IsEnabled = true,
            });

            Logger.LogInformation("Account registered: {Email}", message.Email);

            return SingleMessage(new AccountCreateResponseMessage(true, "Account created"));
        }
    }
}
