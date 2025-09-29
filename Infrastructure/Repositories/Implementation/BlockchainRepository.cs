using BlockChain.Domain.Entities;
using BlockChain.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlockChain.Infrastructure.Repositories
{
    public class BlockchainRepository : IBlockchainRepository
    {
        private readonly AppDbContext _context;

        public BlockchainRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BlockchainData data)
        {
            await _context.BlockchainData.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BlockchainData>> GetHistoryAsync(string blockchainType)
        {
            return await _context.BlockchainData
                .Where(b => b.BlockchainType.ToUpper() == blockchainType.ToUpper())
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<BlockchainData?> GetByIdAsync(int id)
        {
            return await _context.BlockchainData.FindAsync(id);
        }

        Task<List<BlockchainData>> IBlockchainRepository.GetHistoryAsync(string blockchainType)
        {
            throw new NotImplementedException();
        }

        Task<BlockchainData?> IBlockchainRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
