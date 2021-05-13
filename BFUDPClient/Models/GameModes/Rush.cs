using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace BFUDPClient.Models.GameModes
{
    public class Rush
    {
        [JsonProperty("defenders", NullValueHandling = NullValueHandling.Ignore)]
        public RoundInfo Defenders { get; set; }
        [JsonProperty("attackers", NullValueHandling = NullValueHandling.Ignore)]
        public RoundInfo Attackers { get; set; }

        public Rush() {

        }

        public Rush(MemoryStream stream)
        {
            var pos = stream.Position;
            Attackers = new RoundInfo(stream, true);
            stream.Position = pos;
            Defenders = new RoundInfo(stream, false);
        }

        public Rush(Rush rush) {
            this.Defenders = (rush.Defenders == null) ? null : new RoundInfo(rush.Defenders);
            this.Attackers = (rush.Attackers == null) ? null : new RoundInfo(rush.Attackers);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("##############################################################################################################");
            sb.AppendLine("#                                                  Attackers                                                 #");
            sb.AppendLine("##############################################################################################################");
            sb.AppendLine($"Attacker: {Attackers.Attacker}");
            sb.AppendLine($"Team: {Attackers.Team}");
            sb.AppendLine($"Tickets: {Attackers.Tickets}");
            sb.AppendLine($"TicketsMax: {Attackers.TicketsMax}");
            sb.AppendLine();
            sb.AppendLine("##############################################################################################################");
            sb.AppendLine("#                                                  Defenders                                                 #");
            sb.AppendLine("##############################################################################################################");
            sb.AppendLine($"Attacker: {Defenders.Attacker}");
            sb.AppendLine($"Team: {Defenders.Team}");
            sb.AppendLine($"Bases: {Defenders.Bases}");
            sb.AppendLine($"BasesMax: {Defenders.BasesMax}");

            return sb.ToString();
        }
    }
}