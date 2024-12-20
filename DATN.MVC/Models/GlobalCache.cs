using System.Collections.Concurrent;

namespace DATN.MVC.Models
{
    public static class GlobalCache
    {
        public static readonly ConcurrentDictionary<int, int> PostLikes = new ConcurrentDictionary<int, int>();
        public static readonly ConcurrentDictionary<int, int> CommentLikes = new ConcurrentDictionary<int, int>();

    }

}
