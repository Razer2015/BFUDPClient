using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BFUDPClient.Models.GameModes
{
    public class CarrierAssault
    {
        [JsonProperty(PropertyName = "2")]
        public RoundInfo _2 { get; set; }
        [JsonProperty(PropertyName = "1")]
        public RoundInfo _1 { get; set; }

        public CarrierAssault() {

        }

        public CarrierAssault(CarrierAssault cAssault) {
            this._2 = (cAssault._2 == null) ? null : new RoundInfo(cAssault._2);
            this._1 = (cAssault._1 == null) ? null : new RoundInfo(cAssault._1);
        }
    }
}