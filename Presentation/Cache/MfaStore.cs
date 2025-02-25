using System.Collections.Concurrent;

namespace Presentation.Cache
{
    public static class MfaStore
    {
        // Thread‑safe dictionary to store codes per email (In future replace with redis)
        public static ConcurrentDictionary<string, string> Codes = new ConcurrentDictionary<string, string>();
    }
}
