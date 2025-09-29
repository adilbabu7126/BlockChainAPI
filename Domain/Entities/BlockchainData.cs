using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Domain.Entities
{
    public class BlockchainData
    {
        public int Id { get; set; }
        //[Required]
        public string BlockchainType { get; set; } = string.Empty; // BTC, ETH, DASH, LTC
        public string? Hash { get; set; }
        public long? Height { get; set; }
        public string RawJson { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
