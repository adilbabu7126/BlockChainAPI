using BlockChain.Domain.Entities;

namespace BlockChain.Application.Interfaces
{
    public interface IBlockchainService
    {
        Task<BlockchainData> FetchAndStoreAsync(string blockchainType);
        Task<List<BlockchainData>> GetHistoryAsync(string blockchainType);
        Task<BlockchainData?> GetByIdAsync(int id);
        bool IsSupportedBlockchainType(string blockchainType);
    }

}
