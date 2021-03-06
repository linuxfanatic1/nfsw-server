﻿using System;
using System.Xml.Serialization;

namespace OfflineServer.Servers.Http.Responses
{
    [Serializable()]
    [XmlRoot("CommerceSessionTrans", IsNullable = true)]
    public class CommerceSessionTrans
    {
        [XmlElement("Basket")]
        public String basketTrans;
        [XmlElement("EntitlementsToSell")]
        public String entitlementsToSell;
        [XmlElement("UpdatedCar")]
        public UpdatedCar updatedCar;
    }
}
