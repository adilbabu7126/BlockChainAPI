using BlockChain.Domain.Entities;
namespace BlockChain.Infrastructure.Repositories
{
    public interface IBlockchainRepository
    {
        Task AddAsync(BlockchainData data);
        Task<List<BlockchainData>> GetHistoryAsync(string blockchainType);
        Task<BlockchainData?> GetByIdAsync(int id);
    }
}
