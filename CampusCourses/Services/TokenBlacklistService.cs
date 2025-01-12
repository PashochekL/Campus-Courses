using StackExchange.Redis;

namespace CampusCourses.Services
{
    public class TokenBlacklistService
    {
        private readonly IDatabase _redisDatabase;

        public TokenBlacklistService(IConnectionMultiplexer redisConnection)
        {
            _redisDatabase = redisConnection.GetDatabase();
        }

        public async Task RevokeToken(string token, TimeSpan expiration)
        {
            await _redisDatabase.StringSetAsync(token, "revoked", expiration);
        }

        public async Task<bool> IsTokenRevoked(string token)
        {
            var result = await _redisDatabase.StringGetAsync(token);

            return result.HasValue;
        }
    }
}
