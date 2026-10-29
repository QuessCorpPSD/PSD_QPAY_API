using StackExchange.Redis;

namespace QPay.API.Extensions
{
    public class RedisCacheService
    {
        private readonly ConnectionMultiplexer _connection;

        public RedisCacheService(ConnectionMultiplexer connection)
        {
            _connection = connection;
        }
    }
}
