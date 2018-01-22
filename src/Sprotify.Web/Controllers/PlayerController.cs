using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprotify.Web.Models.Player;
using Sprotify.Web.Models;
using Microsoft.AspNetCore.Http;
using Sprotify.Web.Services;

namespace Sprotify.Web.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        private readonly PlaylistService _service;

        public PlayerController(PlaylistService service)
        {
            _service = service;
        }

        [Route("/webplayer")]
        public async Task<IActionResult> Index()
        {
            var playlists = await _service.GetMyPlaylists(User.GetSubject());
            return View(playlists);
        }

        [Route("/search")]
        public IActionResult Search()
        {
            return View(new SearchResult());
        }

        [Route("/search")]
        [HttpPost]
        public async Task<IActionResult> Search([FromForm]string filter)
        {
            return View(await _service.Search(filter));
        }

        [Route("/playlist/{id}")]
        public async Task<IActionResult> Playlist(string id)
        {
            if (Guid.TryParse(id, out Guid playlistId))
            {
                var playlist = await _service.GetPlaylistById(playlistId);
                SavePlaylistId(playlist.Id.ToString());

                return View(playlist);
            }

            // Return dummy view
            return View(new PlaylistWithSongs
            {
                Id = Guid.NewGuid(),
                Title = "Chill Hits",
                CreatorId = Guid.Empty,
                CreatorName = "Sprotify",
                Image = "https://i.scdn.co/image/141c40597e8b10ad61822aec44584e295e2c7330",
                Songs = new List<Song>
                {
                    new Song
                    {
                        Id = Guid.NewGuid(),
                        AlbumId = Guid.NewGuid(),
                        Album = "What Lovers Do (feat. SZA)",
                        Band = "Maroon 5",
                        BandId = Guid.NewGuid(),
                        Title = "What Lovers Do (feat. SZA)",
                        Duration = new TimeSpan(0, 3, 19)
                    },
                    new Song
                    {
                        Id = Guid.NewGuid(),
                        AlbumId = Guid.NewGuid(),
                        Album = "Think Before I Talk",
                        Band = "Astrid S",
                        BandId = Guid.NewGuid(),
                        Title = "Think Before I Talk",
                        Duration = new TimeSpan(0, 3, 04)
                    },
                    new Song
                    {
                        Id = Guid.NewGuid(),
                        AlbumId = Guid.NewGuid(),
                        Album = "Complicated (feat. Kiiara)",
                        Band = "Dimitri Vegas & Like Mike",
                        BandId = Guid.NewGuid(),
                        Title = "Complicated (feat. Kiiara)",
                        Duration = new TimeSpan(0, 3, 04)
                    },
                    new Song
                    {
                        Id = Guid.NewGuid(),
                        AlbumId = Guid.NewGuid(),
                        Album = "Stargazing - EP",
                        Band = "Kygo",
                        BandId = Guid.NewGuid(),
                        Title = "Stargazing",
                        Duration = new TimeSpan(0, 3, 56)
                    },
                    new Song
                    {
                        Id = Guid.NewGuid(),
                        AlbumId = Guid.NewGuid(),
                        Album = "Summer Air",
                        Band = "ItaloBrothers",
                        BandId = Guid.NewGuid(),
                        Title = "Summer Air",
                        Duration = new TimeSpan(0, 3, 03)
                    },
                    new Song
                    {
                        Id = Guid.NewGuid(),
                        AlbumId = Guid.NewGuid(),
                        Album = "Stay Young",
                        Band = "Mike Perry",
                        BandId = Guid.NewGuid(),
                        Title = "Stay Young",
                        Duration = new TimeSpan(0, 2, 37)
                    },
                    new Song
                    {
                        Id = Guid.NewGuid(),
                        AlbumId = Guid.NewGuid(),
                        Album = "Your Shirt",
                        Band = "Chelsea Cutler",
                        BandId = Guid.NewGuid(),
                        Title = "Your Shirt",
                        Duration = new TimeSpan(0, 3, 52)
                    },
                    new Song
                    {
                        Id = Guid.NewGuid(),
                        AlbumId = Guid.NewGuid(),
                        Album = "There for You",
                        Band = "Martin Garrix",
                        BandId = Guid.NewGuid(),
                        Title = "There for You",
                        Duration = new TimeSpan(0, 3, 41)
                    },
                    new Song
                    {
                        Id = Guid.NewGuid(),
                        AlbumId = Guid.NewGuid(),
                        Album = "Sober",
                        Band = "Cheat Codes, Nicky Romero",
                        BandId = Guid.NewGuid(),
                        Title = "Sober",
                        Duration = new TimeSpan(0, 2, 42)
                    },
                }
            });
        }

        [Route("/your-music")]
        public async Task<IActionResult> Playlists()
        {
            return View(await _service.GetMyPlaylists(User.GetSubject()));
        }

        [Route("/new-playlist")]
        public IActionResult CreatePlaylist()
        {
            return View();
        }

        [Route("/new-playlist")]
        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(CreatePlaylistModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var playlist = await _service.CreatePlaylist(model);
                    SavePlaylistId(playlist.Id);

                    return RedirectToAction(nameof(Playlist), new { playlist.Id });
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> AddToPlaylist([FromBody]AddToPlaylistModel model)
        {
            var playlistId = GetPlaylistId();
            if (playlistId == Guid.Empty)
            {
                return Ok();
            }

            await _service.AddSongToPlaylist(playlistId, model.Id);
            return Ok();
        }

        private void SavePlaylistId(string playlistId)
        {
            HttpContext.Session.SetString("PlaylistId", playlistId);
        }

        private Guid GetPlaylistId()
        {
            var id = HttpContext.Session.GetString("PlaylistId");
            if (Guid.TryParse(id, out Guid playlistId))
            {
                return playlistId;
            }

            return Guid.Empty;
        }
    }
}