using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.MusicBrainz.ExternalIds
{
    /// <summary>
    /// MusicBrainz release group external id.
    /// </summary>
    public class MusicBrainzReleaseGroupExternalId : IExternalId
    {
        /// <inheritdoc />
        public string ProviderName => "MusicBrainz";

        /// <inheritdoc />
        public string Key => MetadataProvider.MusicBrainzReleaseGroup.ToString();

        /// <inheritdoc />
        public ExternalIdMediaType? Type => ExternalIdMediaType.ReleaseGroup;

        /// <inheritdoc />
        public string UrlFormatString => MusicBrainzPlugin.Instance?.Configuration.Server + "/release-group/{0}";

        /// <inheritdoc />
        public bool Supports(IHasProviderIds item) => item is Audio || item is MusicAlbum;
    }
}