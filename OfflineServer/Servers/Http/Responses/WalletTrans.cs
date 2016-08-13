﻿using System;
using System.Xml.Serialization;

namespace OfflineServer.Servers.Http.Responses
{
    [Serializable()]
    public class WalletTrans
    {
        [XmlElement("Balance")]
        public Int32 balance = 1337;
        [XmlElement("Currency")]
        public String currency = "CASH";
    }
}
