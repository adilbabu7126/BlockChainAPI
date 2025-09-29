using BlockChain.Application.Implementation;
using BlockChain.Domain.Entities;
using BlockChain.Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using System.Net;

namespace Test
{
    public class UnitTest
    {
        [Fact]
        public async Task FetchAndStoreAsync_StoresEntity_WhenApiReturnsSuccess()
        {
            // Arrange - Expected fake http response
            var json = "{\"hash\":\"8e51d\",\"height\":201}";
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(json)
               });

            var httpClient = new HttpClient(handlerMock.Object);

            var repoMock = new Mock<IBlockchainRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<BlockchainData>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            var logger = NullLogger<BlockchainService>.Instance;
            var service = new BlockchainService(httpClient, repoMock.Object, logger);

            // Act
            var result = await service.FetchAndStoreAsync("DASH");

            // Assert
            Assert.Equal("DASH", result.BlockchainType);
            Assert.Equal("8e51d", result.Hash);
            Assert.Equal(201, result.Height);
            repoMock.Verify();
        }
    }
}
