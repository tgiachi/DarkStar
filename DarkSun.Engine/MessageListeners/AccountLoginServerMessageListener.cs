﻿using System;
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
using DarkSun.Network.Protocol.Messages.Server;
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

        public override async Task<List<IDarkSunNetworkMessage>> OnMessageReceivedAsync(Guid sessionId, DarkSunMessageType messageType, AccountLoginRequestMessage message)
        {
            Logger.LogInformation("Received login request from {Id}", sessionId);
            var account = await Engine.DatabaseService.QueryAsSingleAsync<AccountEntity>(entity =>
                entity.Email == message.Email && entity.PasswordHash == message.Password.CreateMd5Hash());
            if (account == null!)
            {
                Logger.LogWarning("Invalid login attempt from {Id}", sessionId);
                return SingleMessage(new AccountLoginResponseMessage(false));
            }
            else
            {
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