using OrderManagementApi.Models;
using System.Collections.Concurrent;

namespace OrderManagementApi.Services
{
    // Very simple token store for demo (in-memory mapping). NOT secure.
    public class TokenService
    {
        private readonly ConcurrentDictionary<string, Guid> _map = new();

        public string GenerateToken(User user)
        {
            // Intentionally trivial token generation
            var token = Guid.NewGuid().ToString();
            _map[token] = user.Id;
            return token;
        }

        public bool TryGetUserId(string token, out Guid userId)
        {
            return _map.TryGetValue(token, out userId);
        }
    }
}
