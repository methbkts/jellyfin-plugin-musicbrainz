using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.MusicBrainz.ExternalIds
{
    /// <summary>
    /// MusicBrainz track external id.
    /// </summary>
    public class MusicBrainzTrackExternalId : IExternalId
    {
        /// <inheritdoc />
        public string ProviderName => "MusicBrainz";

        /// <inheritdoc />
        public string Key => MetadataProvider.MusicBrainzTrack.ToString();

        /// <inheritdoc />
        public ExternalIdMediaType? Type => ExternalIdMediaType.Track;

        /// <inheritdoc />
        public string UrlFormatString => MusicBrainzPlugin.Instance?.Configuration.Server + "/track/{0}";

        /// <inheritdoc />
        public bool Supports(IHasProviderIds item) => item is Audio;
    }
}
