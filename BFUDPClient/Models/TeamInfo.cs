using Newtonsoft.Json;
using System.Collections.Generic;
using Syroot.BinaryData;
using System.IO;
using System.Text;

namespace BFUDPClient.Models
{
    public class TeamInfo
    {
        [JsonProperty("faction")]
        public byte Faction { get; set; }
        [JsonProperty("players")]
        public Dictionary<string, Player> Players { get; set; }

        public TeamInfo()
        {
            Players = new Dictionary<string, Player>();
        }

        public TeamInfo(MemoryStream stream, byte teamsCount, long teamInfoOffset, byte teamId)
        {
            Players = new Dictionary<string, Player>();

            byte playerCount = 0;
            using (stream.TemporarySeek(teamInfoOffset + (teamId * 1), SeekOrigin.Begin))
            {
                playerCount = stream.Read1Byte();
                if (teamId > 0)
                {
                    stream.Seek(teamInfoOffset + (teamsCount * 1) + (teamId * 1), SeekOrigin.Begin);
                    Faction = stream.Read1Byte();
                }
            }

            if (teamId == 0)
            {
                stream.Seek(teamInfoOffset + (teamsCount * 1) + 1 + (teamsCount * 1), SeekOrigin.Begin);
            }

            for (byte i = 0; i < playerCount; i++)
            {
                var player = new Player(stream);
                Players.TryAdd(player.PersonaId.ToString(), player);
            }
        }
    }
}
