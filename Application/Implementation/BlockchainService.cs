using BlockChain.Application.Interfaces;
using BlockChain.Domain.Entities;
using BlockChain.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace BlockChain.Application.Implementation
{
    public class BlockchainService : IBlockchainService
    {
        private readonly HttpClient _httpClient;
        private readonly IBlockchainRepository _repository;
        private readonly ILogger<BlockchainService> _logger;

        private static readonly HashSet<string> SupportedBlockchainType = new(StringComparer.OrdinalIgnoreCase)
        {
            "BTC", "ETH", "DASH", "LTC"
        };

        public BlockchainService(HttpClient httpClient, IBlockchainRepository repository, ILogger<BlockchainService> logger)
        {
            _httpClient = httpClient;
            _repository = repository;
            _logger = logger;
        }

        public bool IsSupportedBlockchainType(string blockchainType) => SupportedBlockchainType.Contains(blockchainType);

        private string GetApiUrl(string blockchainType) => blockchainType.ToUpper() switch
        {
            "BTC" => "https://api.blockcypher.com/v1/btc/main",
            "ETH" => "https://api.blockcypher.com/v1/eth/main",
            "DASH" => "https://api.blockcypher.com/v1/dash/main",
            "LTC" => "https://api.blockcypher.com/v1/ltc/main",
            _ => throw new ArgumentException("Unsupported blockchainType")
        };

        public async Task<BlockchainData> FetchAndStoreAsync(string blockchainType)
        {
            if (!IsSupportedBlockchainType(blockchainType))
                throw new ArgumentException($"Unsupported blockchainType: {blockchainType}");

            var url = GetApiUrl(blockchainType);
            _logger.LogInformation("Fetching data from {Url} for {BlockchainType}", url, blockchainType);

            using var resp = await _httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var body = await resp.Content.ReadAsStringAsync();

            // parse some keys if exist (hash, height)
            string? hash = null;
            long? height = null;
            try
            {
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;
                if (root.TryGetProperty("hash", out var hEl) && hEl.ValueKind == JsonValueKind.String)
                    hash = hEl.GetString();

                if (root.TryGetProperty("height", out var heightEl) && heightEl.TryGetInt64(out var h))
                    height = h;
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to parse json from {Url}", url);
            }

            var entity = new BlockchainData
            {
                BlockchainType = blockchainType.ToUpper(),
                Hash = hash,
                Height = height,
                RawJson = body,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(entity);
            _logger.LogInformation("Stored data for {BlockchainType} (Id: {Id})", entity.BlockchainType, entity.Id);

            return entity;
        }

        public async Task<List<BlockchainData>> GetHistoryAsync(string blockchainType)
        {
            if (!IsSupportedBlockchainType(blockchainType))
                throw new ArgumentException($"Unsupported blockchainType: {blockchainType}");

            return await _repository.GetHistoryAsync(blockchainType.ToUpper());
        }

        public async Task<BlockchainData?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
    }
}
