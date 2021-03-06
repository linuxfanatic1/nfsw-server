﻿using System;
using System.Globalization;

namespace OfflineServer.Servers.Http.Classes
{
    public static class Root
    {
        public static String systeminfo()
        {
            return 
                "<SystemInfo xmlns=\"http://schemas.datacontract.org/2004/07/EA.NFSWO.ENGINE.Service\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                    "<Branch>D4</Branch>" +
                    "<ChangeList>513698</ChangeList>" +
                    "<ClientVersion>2578</ClientVersion>" +
                    "<ClientVersionCheck>false</ClientVersionCheck>" +
                    "<Deployed>12/30/2070 20:00:70</Deployed>" +
                    "<EntitlementsToDownload>false</EntitlementsToDownload>" +
                    "<EntitlementsToPlay>true</EntitlementsToPlay>" +
                    "<ForcePermanentSession>true</ForcePermanentSession>" +
                    "<JidPrepender>" + Access.sXmpp.jidPrepender + "</JidPrepender>" +
                    "<LauncherServiceUrl/>" +
                    "<NucleusNamespace>nfsw-ob</NucleusNamespace>" +
                    "<NucleusNamespaceWeb>nfs_web</NucleusNamespaceWeb>" +
                    "<PortalDomain/>" +
                    "<PortalSecureDomain/>" +
                    "<PortalTimeOut>10000</PortalTimeOut>" +
                    "<Time>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture) + ":00.0000000+00:00</Time>" +
                    "<Version>D4_41</Version>" +
                "</SystemInfo>";
        }
    }
}