using System;
using Jellyfin.Plugin.MusicBrainz.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.MusicBrainz
{
    /// <summary>
    /// MusicBrainz plugin.
    /// </summary>
    public class MusicBrainzPlugin : MediaBrowser.Common.Plugins.BasePlugin<PluginConfiguration>, IHasWebPages
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MusicBrainzPlugin"/> class.
        /// </summary>
        /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
        /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
        public MusicBrainzPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <summary>
        /// Gets the current plugin instance.
        /// </summary>
        public static MusicBrainzPlugin? Instance { get; private set; }

        /// <inheritdoc />
        public override Guid Id => new Guid("8c95c4d2-e50c-4fb0-a4f3-6c06ff0f9a1a");

        /// <inheritdoc />
        public override string Name => "MusicBrainz";

        /// <inheritdoc />
        public override string Description => "Get artist and album metadata from any MusicBrainz server.";

        /// <inheritdoc />
        public System.Collections.Generic.IEnumerable<PluginPageInfo> GetPages()
        {
            yield return new PluginPageInfo
            {
                Name = Name,
                EmbeddedResourcePath = GetType().Namespace + ".Configuration.config.html"
            };
        }
    }
}
