using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BFUDPClient.Models.GameModes
{
    public class Conquest
    {
        [JsonProperty(PropertyName = "2")]
        public RoundInfo _2 { get; set; }
        [JsonProperty(PropertyName = "1")]
        public RoundInfo _1 { get; set; }

        public Conquest() {

        }

        public Conquest(Conquest conq) {
            this._2 = (conq._2 == null) ? null : new RoundInfo(conq._2);
            this._1 = (conq._1 == null) ? null : new RoundInfo(conq._1);
        }
    }
}