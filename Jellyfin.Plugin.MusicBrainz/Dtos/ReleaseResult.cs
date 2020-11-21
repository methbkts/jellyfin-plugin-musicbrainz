using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Jellyfin.Plugin.MusicBrainz.Dtos
{
    /// <summary>
    /// Release result.
    /// </summary>
    public class ReleaseResult
    {
        /// <summary>
        /// Gets or sets the release id.
        /// </summary>
        public string? ReleaseId { get; set; }

        /// <summary>
        /// Gets or sets the release group id.
        /// </summary>
        public string? ReleaseGroupId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the overview.
        /// </summary>
        public string? Overview { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets the artists.
        /// </summary>
        public List<ValueTuple<string?, string?>> Artists { get; } = new List<ValueTuple<string?, string?>>();

        /// <summary>
        /// Parse xml to a list of release results.
        /// </summary>
        /// <param name="reader">The xml reader.</param>
        /// <returns>The list of results.</returns>
        public static IEnumerable<ReleaseResult> Parse(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();

            // Loop through each element
            while (!reader.EOF && reader.ReadState == ReadState.Interactive)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "release-list":
                        {
                            if (reader.IsEmptyElement)
                            {
                                reader.Read();
                                continue;
                            }

                            using (var subReader = reader.ReadSubtree())
                            {
                                return ParseReleaseList(subReader).ToList();
                            }
                        }

                        default:
                        {
                            reader.Skip();
                            break;
                        }
                    }
                }
                else
                {
                    reader.Read();
                }
            }

            return Enumerable.Empty<ReleaseResult>();
        }

        private static IEnumerable<ReleaseResult> ParseReleaseList(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();

            // Loop through each element
            while (!reader.EOF && reader.ReadState == ReadState.Interactive)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "release":
                        {
                            if (reader.IsEmptyElement)
                            {
                                reader.Read();
                                continue;
                            }

                            var releaseId = reader.GetAttribute("id");
                            if (releaseId == null)
                            {
                                continue;
                            }

                            using var subReader = reader.ReadSubtree();
                            yield return ParseRelease(subReader, releaseId);

                            break;
                        }

                        default:
                        {
                            reader.Skip();
                            break;
                        }
                    }
                }
                else
                {
                    reader.Read();
                }
            }
        }

        private static ReleaseResult ParseRelease(XmlReader reader, string releaseId)
        {
            var result = new ReleaseResult
            {
                ReleaseId = releaseId
            };

            reader.MoveToContent();
            reader.Read();

            // http://stackoverflow.com/questions/2299632/why-does-xmlreader-skip-every-other-element-if-there-is-no-whitespace-separator

            // Loop through each element
            while (!reader.EOF && reader.ReadState == ReadState.Interactive)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "title":
                        {
                            result.Title = reader.ReadElementContentAsString();
                            break;
                        }

                        case "date":
                        {
                            var val = reader.ReadElementContentAsString();
                            if (DateTime.TryParse(val, out var date))
                            {
                                result.Year = date.Year;
                            }

                            break;
                        }

                        case "annotation":
                        {
                            result.Overview = reader.ReadElementContentAsString();
                            break;
                        }

                        case "release-group":
                        {
                            result.ReleaseGroupId = reader.GetAttribute("id");
                            reader.Skip();
                            break;
                        }

                        case "artist-credit":
                        {
                            using (var subReader = reader.ReadSubtree())
                            {
                                var artist = ParseArtistCredit(subReader);

                                if (!string.IsNullOrEmpty(artist.Item1))
                                {
                                    result.Artists.Add(artist);
                                }
                            }

                            break;
                        }

                        default:
                        {
                            reader.Skip();
                            break;
                        }
                    }
                }
                else
                {
                    reader.Read();
                }
            }

            return result;
        }

        private static (string?, string?) ParseArtistCredit(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();

            // http://stackoverflow.com/questions/2299632/why-does-xmlreader-skip-every-other-element-if-there-is-no-whitespace-separator

            // Loop through each element
            while (!reader.EOF && reader.ReadState == ReadState.Interactive)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "name-credit":
                        {
                            using (var subReader = reader.ReadSubtree())
                            {
                                return ParseArtistNameCredit(subReader);
                            }
                        }

                        default:
                        {
                            reader.Skip();
                            break;
                        }
                    }
                }
                else
                {
                    reader.Read();
                }
            }

            return default;
        }

        private static (string?, string?) ParseArtistNameCredit(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();

            // http://stackoverflow.com/questions/2299632/why-does-xmlreader-skip-every-other-element-if-there-is-no-whitespace-separator

            // Loop through each element
            while (!reader.EOF && reader.ReadState == ReadState.Interactive)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "artist":
                        {
                            var id = reader.GetAttribute("id");
                            if (id == null)
                            {
                                continue;
                            }

                            using var subReader = reader.ReadSubtree();
                            return ParseArtistArtistCredit(subReader, id);
                        }

                        default:
                        {
                            reader.Skip();
                            break;
                        }
                    }
                }
                else
                {
                    reader.Read();
                }
            }

            return (null, null);
        }

        private static (string? name, string? id) ParseArtistArtistCredit(XmlReader reader, string artistId)
        {
            reader.MoveToContent();
            reader.Read();

            string? name = null;

            // http://stackoverflow.com/questions/2299632/why-does-xmlreader-skip-every-other-element-if-there-is-no-whitespace-separator

            // Loop through each element
            while (!reader.EOF && reader.ReadState == ReadState.Interactive)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "name":
                        {
                            name = reader.ReadElementContentAsString();
                            break;
                        }

                        default:
                        {
                            reader.Skip();
                            break;
                        }
                    }
                }
                else
                {
                    reader.Read();
                }
            }

            return (name, artistId);
        }
    }
}