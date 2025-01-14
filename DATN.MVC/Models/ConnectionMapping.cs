using System.Collections.Concurrent;

namespace DATN.MVC.Hubs
{
    public static class ConnectionMapping
    {
        // key: userId (string), value: list connectionIds
        private static readonly ConcurrentDictionary<string, List<string>> _userConnections
            = new ConcurrentDictionary<string, List<string>>();

        public static void AddConnection(string userId, string connectionId)
        {
            _userConnections.AddOrUpdate(userId,
                (key) => new List<string> { connectionId },
                (key, existingList) =>
                {
                    existingList.Add(connectionId);
                    return existingList;
                }
            );
        }

        public static void RemoveConnection(string userId, string connectionId)
        {
            if (_userConnections.TryGetValue(userId, out var existingList))
            {
                existingList.Remove(connectionId);

                if (existingList.Count == 0)
                {
                    _userConnections.TryRemove(userId, out _);
                }
            }
        }

        public static bool IsUserOnline(string userId)
        {
            return _userConnections.ContainsKey(userId);
        }

        // Hàm này để lấy tất cả các connection của 1 user
        public static List<string> GetConnections(string userId)
        {
            if (_userConnections.TryGetValue(userId, out var connections))
            {
                return connections;
            }
            return new List<string>();
        }
    }
}
