using Microsoft.Extensions.Configuration;

namespace Interneuron.Caching
{
    public class CacheSettings
    {
        static class Constants
        {
            public const string CacheDurationInMinutes = "cacheDurationInMinutes";
            public const string Enabled = "enabled";
            public const string Provider = "provider";
        }

        private static readonly CacheSettings Settings = CacheServiceExtension.Configuration == null ? null : CacheServiceExtension.Configuration.GetSection("cache").Get<CacheSettings>();

        public static CacheSettings Instance
        {
            get
            {
                return Settings ?? new CacheSettings();
            }
        }


        public int CacheDurationInMinutes { get; set; } = 5;

        public string Provider { get; set; } = null;

        public bool Enabled { get; set; } = false;
    }
}
