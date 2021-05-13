using Newtonsoft.Json;
using Syroot.BinaryData;
using System.IO;

namespace BFUDPClient.Models
{
    public class RoundInfo
    {
        [JsonProperty("team", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Team { get; set; }
        [JsonProperty("bases", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Bases { get; set; }
        [JsonProperty("basesMax", NullValueHandling = NullValueHandling.Ignore)]
        public byte? BasesMax { get; set; }
        [JsonProperty("attacker", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Attacker { get; set; }
        [JsonProperty("tickets", NullValueHandling = NullValueHandling.Ignore)]
        public ushort? Tickets { get; set; }
        [JsonProperty("ticketsMax", NullValueHandling = NullValueHandling.Ignore)]
        public ushort? TicketsMax { get; set; }
        [JsonProperty("flagsMax", NullValueHandling = NullValueHandling.Ignore)]
        public byte? FlagsMax { get; set; }
        [JsonProperty("roundTimeMax", NullValueHandling = NullValueHandling.Ignore)]
        public ushort? RoundTimeMax { get; set; }
        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public byte? Flags { get; set; }
        [JsonProperty("kills", NullValueHandling = NullValueHandling.Ignore)]
        public int? Kills { get; set; }
        [JsonProperty("killsMax", NullValueHandling = NullValueHandling.Ignore)]
        public int? KillsMax { get; set; }
        [JsonProperty("destroyedCrates", NullValueHandling = NullValueHandling.Ignore)]
        public int? DestroyedCrates { get; set; }
        [JsonProperty("carrierHealth", NullValueHandling = NullValueHandling.Ignore)]
        public int? CarrierHealth { get; set; }

        public RoundInfo() {

        }

        public RoundInfo(MemoryStream stream, bool isAttacker)
        {
            Attacker = isAttacker ? (byte)1 : (byte)0;
            if (!isAttacker) stream.Position += 1;
            Team = stream.Read1Byte();

            if (isAttacker)
            {
                stream.Position += 1;
                Tickets = stream.ReadUInt16(ByteConverter.Big);
                TicketsMax = stream.ReadUInt16(ByteConverter.Big);
            }
            else
            {
                stream.Position += 4;
                Bases = stream.Read1Byte();
                BasesMax = stream.Read1Byte();
            }
        }

        public RoundInfo(RoundInfo rInfo) {
            this.Team = rInfo.Team;
            this.Bases = rInfo.Bases;
            this.BasesMax = rInfo.BasesMax;
            this.Attacker = rInfo.Attacker;
            this.Tickets = rInfo.Tickets;
            this.TicketsMax = rInfo.TicketsMax;
            this.FlagsMax = rInfo.FlagsMax;
            this.RoundTimeMax = rInfo.RoundTimeMax;
            this.Flags = rInfo.Flags;
            this.Kills = rInfo.Kills;
            this.KillsMax = rInfo.KillsMax;
            this.DestroyedCrates = rInfo.DestroyedCrates;
            this.CarrierHealth = rInfo.CarrierHealth;
        }
    }
}