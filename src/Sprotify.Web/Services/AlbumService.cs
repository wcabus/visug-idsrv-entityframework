using Sprotify.Web.Models;
using Sprotify.Web.Services.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Web.Services
{
    public class AlbumService : ApiServiceBase
    {
        public AlbumService(SprotifyHttpClient sprotifyclient) : base(sprotifyclient)
        {
        }

        public async Task<IEnumerable<Album>> GetAlbumsForBand(Guid bandId)
        {
            try
            {
                return await Get<IEnumerable<Album>>($"bands/{bandId}/albums")
                    .ConfigureAwait(false);
            }
            catch (ResourceNotFoundException) { }

            return null;
        }

        public async Task<string> GetBandName(Guid bandId)
        {
            try
            {
                return (await Get<Band>($"bands/{bandId}")
                    .ConfigureAwait(false))?.Name;
            }
            catch (ResourceNotFoundException) { }

            return "";
        }

        public async Task<Album> GetAlbum(Guid bandId, Guid albumId)
        {
            try
            {
                return await Get<Album>($"bands/{bandId}/albums/{albumId}")
                    .ConfigureAwait(false);
            }
            catch (ResourceNotFoundException) { }

            return null;
        }

        public async Task<AlbumWithSongs> GetAlbumWithSongs(Guid bandId, Guid albumId)
        {
            try
            {
                return await Get<AlbumWithSongs>($"bands/{bandId}/albums/{albumId}?includeSongs=true")
                    .ConfigureAwait(false);
            }
            catch (ResourceNotFoundException) { }

            return null;
        }

        public Task<Album> CreateAlbum(Guid bandId, string title, DateTimeOffset? releaseDate, string art)
        {
            return Post<Album>($"bands/{bandId}/albums", new
            {
                title,
                releaseDate,
                art
            });
        }

        public Task<Album> UpdateAlbum(Guid bandId, Guid albumId, string title, DateTimeOffset? releaseDate, string art)
        {
            return Put<Album>($"bands/{bandId}/albums/{albumId}", new
            {
                title,
                releaseDate,
                art
            });
        }
    }
}
