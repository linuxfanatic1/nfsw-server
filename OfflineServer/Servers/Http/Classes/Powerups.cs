﻿using OfflineServer.Servers.Xmpp.Responses;
using System;

namespace OfflineServer.Servers.Http.Classes
{
    public static class Powerups
    {
        public static String activated()
        {
            Int64 powerupHash = Int64.Parse(Access.sHttp.request.Path.Split('/')[4]);
            PowerupActivated powerupActivated = new PowerupActivated();
            powerupActivated.id = powerupHash;
            powerupActivated.targetPersonaId = Int32.Parse(Access.sHttp.request.Params.Get("targetId"));
            powerupActivated.personaId = Access.CurrentSession.ActivePersona.Id;

            PowerupActivatedResponse powerupActivatedResponse = new PowerupActivatedResponse();
            powerupActivatedResponse.powerupActivated = powerupActivated;

            Message message = new Message();
            message.setBody(powerupActivatedResponse);

#pragma warning disable CS4014
            Access.sXmpp.write(message.SerializeObject(true));
#pragma warning restore CS4014

            return "";
        }
    }
}