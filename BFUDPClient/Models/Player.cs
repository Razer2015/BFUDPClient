using Newtonsoft.Json;
using Syroot.BinaryData;
using System.IO;

namespace BFUDPClient.Models
{
    public class Player
    {
        [JsonProperty("personaId")]
        public ulong PersonaId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("tag")]
        public string Tag { get; set; }
        [JsonProperty("rank")]
        public byte Rank { get; set; }
        [JsonProperty("score")]
        public uint Score { get; set; }
        [JsonProperty("kills")]
        public ushort Kills { get; set; }
        [JsonProperty("deaths")]
        public ushort Deaths { get; set; }
        [JsonProperty("squadId")]
        public byte SquadId { get; set; }
        [JsonProperty("role")]
        public byte Role { get; set; }

        public Player(MemoryStream stream)
        {
            PersonaId = stream.ReadUInt64(ByteConverter.Big);
            Name = stream.ReadString(StringCoding.ByteCharCount);
            Tag = stream.ReadString(StringCoding.ByteCharCount);
            var unk = stream.Read1Byte();
            Rank = stream.Read1Byte();
            Score = stream.ReadUInt32(ByteConverter.Big);
            Kills = stream.ReadUInt16(ByteConverter.Big);
            Deaths = stream.ReadUInt16(ByteConverter.Big);
            SquadId = stream.Read1Byte();
            Role = stream.Read1Byte();
        }

        public override string ToString()
        {
            return $"| {PersonaId,13} | {Tag,4} | {Name,30} | {Rank,4} | {Score,10} | {Kills,5} | {Deaths,6} | {SquadId,7} | {Role,4} |";
        }
    }
}
