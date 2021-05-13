using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BFUDPClient.Models.GameModes
{
    public class CaptureTheFlag
    {
        [JsonProperty(PropertyName = "2")]
        public RoundInfo _2 { get; set; }
        [JsonProperty(PropertyName = "1")]
        public RoundInfo _1 { get; set; }

        public CaptureTheFlag() {

        }

        public CaptureTheFlag(CaptureTheFlag cTF) {
            this._2 = (cTF._2 == null) ? null : new RoundInfo(cTF._2);
            this._1 = (cTF._1 == null) ? null : new RoundInfo(cTF._1);
        }
    }
}