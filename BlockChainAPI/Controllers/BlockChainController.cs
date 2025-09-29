using BlockChain.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockChainController : ControllerBase
    {
        private readonly IBlockchainService _service;
        private readonly ILogger<BlockChainController> _logger;
        public BlockChainController(IBlockchainService service, ILogger<BlockChainController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("Get/{blockchainType}")]
        public async Task<IActionResult> Get(string blockchainType)
        {
            try
            {
                blockchainType = blockchainType?.ToUpper() ?? string.Empty;
                if (!_service.IsSupportedBlockchainType(blockchainType))
                    return BadRequest(new { message = "Unsupported blockchainType. Supported: BTC, ETH, DASH, LTC." });

                var result = await _service.FetchAndStoreAsync(blockchainType);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error while fetching from BlockCypher");
                return StatusCode(StatusCodes.Status502BadGateway, new { message = "Upstream service error." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error" });
            }
        }

        [HttpGet("history/{blockchainType}")]
        public async Task<IActionResult> History(string blockchainType)
        {
            try
            {
                blockchainType = blockchainType?.ToUpper() ?? string.Empty;
                if (!_service.IsSupportedBlockchainType(blockchainType))
                    return BadRequest(new { message = "Unsupported blockchainType. Supported: BTC, ETH, DASH, LTC." });

                var list = await _service.GetHistoryAsync(blockchainType);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting history");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }
    }
}
