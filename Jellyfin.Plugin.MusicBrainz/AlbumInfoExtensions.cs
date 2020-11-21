using System.Linq;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.MusicBrainz
{
    /// <summary>
    /// Album info extensions.
    /// </summary>
    public static class AlbumInfoExtensions
    {
        /// <summary>
        /// Gets the album artist.
        /// </summary>
        /// <param name="info">The album info.</param>
        /// <returns>The album artist.</returns>
        public static string GetAlbumArtist(this AlbumInfo info)
        {
            var id = info.SongInfos.SelectMany(i => i.AlbumArtists)
                    .FirstOrDefault(i => !string.IsNullOrEmpty(i));

            if (!string.IsNullOrEmpty(id))
            {
                return id;
            }

            return info.AlbumArtists.Count > 0 ? info.AlbumArtists[0] : string.Empty;
        }

        /// <summary>
        /// Gets the release group id.
        /// </summary>
        /// <param name="info">The album info.</param>
        /// <returns>The release group id.</returns>
        public static string? GetReleaseGroupId(this AlbumInfo info)
        {
            var id = info.GetProviderId(MetadataProvider.MusicBrainzReleaseGroup);

            if (string.IsNullOrEmpty(id))
            {
                return info.SongInfos.Select(i => i.GetProviderId(MetadataProvider.MusicBrainzReleaseGroup))
                    .FirstOrDefault(i => !string.IsNullOrEmpty(i));
            }

            return id;
        }

        /// <summary>
        /// Gets the release id.
        /// </summary>
        /// <param name="info">The album info.</param>
        /// <returns>The release id.</returns>
        public static string? GetReleaseId(this AlbumInfo info)
        {
            var id = info.GetProviderId(MetadataProvider.MusicBrainzAlbum);

            if (string.IsNullOrEmpty(id))
            {
                return info.SongInfos.Select(i => i.GetProviderId(MetadataProvider.MusicBrainzAlbum))
                    .FirstOrDefault(i => !string.IsNullOrEmpty(i));
            }

            return id;
        }

        /// <summary>
        /// Get the music brainz artist id.
        /// </summary>
        /// <param name="info">The album info.</param>
        /// <returns>The artist id.</returns>
        public static string? GetMusicBrainzArtistId(this AlbumInfo info)
        {
            info.ProviderIds.TryGetValue(MetadataProvider.MusicBrainzAlbumArtist.ToString(), out var id);

            if (string.IsNullOrEmpty(id))
            {
                info.ArtistProviderIds.TryGetValue(MetadataProvider.MusicBrainzArtist.ToString(), out id);
            }

            if (string.IsNullOrEmpty(id))
            {
                return info.SongInfos.Select(i => i.GetProviderId(MetadataProvider.MusicBrainzAlbumArtist))
                    .FirstOrDefault(i => !string.IsNullOrEmpty(i));
            }

            return id;
        }

        /// <summary>
        /// Get the music brainz artist id.
        /// </summary>
        /// <param name="info">The artist info.</param>
        /// <returns>The artist id.</returns>
        public static string? GetMusicBrainzArtistId(this ArtistInfo info)
        {
            info.ProviderIds.TryGetValue(MetadataProvider.MusicBrainzArtist.ToString(), out var id);

            if (string.IsNullOrEmpty(id))
            {
                return info.SongInfos.Select(i => i.GetProviderId(MetadataProvider.MusicBrainzAlbumArtist))
                    .FirstOrDefault(i => !string.IsNullOrEmpty(i));
            }

            return id;
        }
    }
}