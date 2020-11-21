using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.MusicBrainz.Configuration
{
    /// <summary>
    /// Plugin configuration.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// The default server.
        /// </summary>
        public const string DefaultServer = "https://musicbrainz.org";

        /// <summary>
        /// The default rate limit.
        /// </summary>
        public const long DefaultRateLimit = 2000u;

        private string _server = DefaultServer;

        private long _rateLimit = DefaultRateLimit;

        /// <summary>
        /// Gets or sets the configured server address.
        /// </summary>
        public string Server
        {
            get => _server;
            set => _server = value.TrimEnd('/');
        }

        /// <summary>
        /// Gets or sets the rate limit.
        /// </summary>
        public long RateLimit
        {
            get => _rateLimit;

            set
            {
                if (value < DefaultRateLimit && _server == DefaultServer)
                {
                    _rateLimit = DefaultRateLimit;
                }
                else
                {
                    _rateLimit = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to replace the artist name.
        /// </summary>
        public bool ReplaceArtistName { get; set; }
    }
}
