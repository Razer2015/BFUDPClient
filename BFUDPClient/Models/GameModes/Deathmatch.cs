using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BFUDPClient.Models.GameModes
{
    public class Deathmatch
    {
        [JsonProperty(PropertyName = "2")]
        public RoundInfo _2 { get; set; }
        [JsonProperty(PropertyName = "4")]
        public RoundInfo _4 { get; set; }
        [JsonProperty(PropertyName = "1")]
        public RoundInfo _1 { get; set; }
        [JsonProperty(PropertyName = "3")]
        public RoundInfo _3 { get; set; }

        public Deathmatch() {

        }

        public Deathmatch(Deathmatch dm) {
            this._2 = (dm._2 == null) ? null : new RoundInfo(dm._2);
            this._4 = (dm._4 == null) ? null : new RoundInfo(dm._4);
            this._1 = (dm._1 == null) ? null : new RoundInfo(dm._1);
            this._3 = (dm._3 == null) ? null : new RoundInfo(dm._3);
        }
    }
}