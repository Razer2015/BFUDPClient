using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BFUDPClient.Models.GameModes
{
    public class GunMaster
    {
        public int max_level { get; set; }
        [JsonProperty(PropertyName = "194284171_level")]
        public int highest_level { get; set; }
    }
}