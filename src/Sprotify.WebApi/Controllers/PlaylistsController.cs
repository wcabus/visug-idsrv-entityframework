using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprotify.Domain.Services;
using Sprotify.WebApi.Models.Playlists;
using AutoMapper;
using Sprotify.WebApi.Models.Songs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sprotify.WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistService _service;
        private readonly IMapper _mapper;

        public PlaylistsController(IPlaylistService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaylists()
        {
            // retrieve all public playlists AND your private playlists
            var playlists = await _service.GetPlaylists(GetCurrentUserId());
            return Ok(_mapper.Map<IEnumerable<Playlist>>(playlists));
        }

        [HttpGet("/users/{userId:guid}/playlists")]
        public async Task<IActionResult> GetUserPlaylists(Guid userId)
        {
            if (! await _service.UserExists(userId))
            {
                return NotFound();
            }

            // if userId == currentuser => private and public
            // else => only public
            var playlists = await _service.GetPlaylistsForUser(userId, userId == GetCurrentUserId());

            return Ok(_mapper.Map<IEnumerable<Playlist>>(playlists));
        }

        [HttpGet("{id:guid}", Name = Routes.GetPlaylistById)]
        public async Task<IActionResult> GetPlaylistById(Guid id)
        {
            var playlist = await _service.GetPlaylistById(id, GetCurrentUserId());
            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PlaylistWithSongs>(playlist));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody]PlayListToCreate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlist = await _service.CreatePlaylist(GetCurrentUserId(), model.Title, model.IsPrivate, model.IsCollaborative);
            return CreatedAtRoute(Routes.GetPlaylistById, new { playlist.Id }, playlist);
        }

        [HttpPost("{id:guid}/songs")]
        public async Task<IActionResult> AddSongToPlaylist(Guid id, [FromBody]AddSongToPlaylist model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlist = await _service.GetPlaylistById(id, GetCurrentUserId());
            if (playlist == null)
            {
                return NotFound();
            }

            var song = await _service.GetSongById(model.Id);
            if (song == null)
            {
                return BadRequest("Unknown song");
            }

            var playlistSong = await _service.AddSongToPlaylist(playlist, song.Song, GetCurrentUserId());
            return Ok(_mapper.Map<Song>(playlistSong));
        }

        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirst("sub")?.Value ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                return Guid.Empty;
            }

            return Guid.Parse(userId);
        }

        public class Routes
        {
            public const string GetPlaylistById = nameof(GetPlaylistById);
        }
    }
}
