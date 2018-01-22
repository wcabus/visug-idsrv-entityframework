using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Sprotify.WebApi.Models.Albums;
using Sprotify.Domain.Services;
using Sprotify.WebApi.Models;

namespace Sprotify.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class AlbumsController : Controller
    {
        private readonly IAlbumService _service;
        private readonly IMapper _mapper;

        public AlbumsController(IAlbumService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums([FromQuery]string filter = null)
        {
            return Ok(_mapper.Map<IEnumerable<SearchItem>>(await _service.GetAlbums(filter)));
        }

        [HttpGet("/bands/{bandId:guid}/albums")]
        public async Task<IActionResult> GetAlbumsForBand(Guid bandId)
        {
            if (!await _service.BandExists(bandId))
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<Album>>(await _service.GetAlbumsForBand(bandId)));
        }

        [HttpGet("/bands/{bandId:guid}/albums/{id:guid}", Name = "GetAlbumForBand")]
        public async Task<IActionResult> GetAlbumForBandById(Guid bandId, Guid id, bool includeSongs = false)
        {
            if (!await _service.BandExists(bandId))
            {
                return NotFound();
            }

            if (!await _service.AlbumExists(id))
            {
                return NotFound();
            }

            if (includeSongs)
            {
                return Ok(_mapper.Map<AlbumWithSongs>(await _service.GetAlbumById(id, includeSongs)));
            }

            return Ok(_mapper.Map<Album>(await _service.GetAlbumById(id, false)));
        }

        [HttpPost("/bands/{bandId:guid}/albums")]
        public async Task<IActionResult> CreateAlbum(Guid bandId, [FromBody]AlbumToCreate model)
        {
            if (!await _service.BandExists(bandId))
            {
                return NotFound();
            }

            var album = await _service.CreateAlbum(bandId, model.Title, model.ReleaseDate, model.Art);
            return CreatedAtAction(nameof(GetAlbumForBandById), new { bandId, album.Id }, _mapper.Map<Album>(album));
        }

        [HttpPut("/bands/{bandId:guid}/albums/{id:guid}")]
        public async Task<IActionResult> UpdateAlbum(Guid bandId, Guid id, [FromBody]AlbumToUpdate model)
        {
            if (!await _service.BandExists(bandId))
            {
                return NotFound();
            }

            var album = await _service.GetAlbumById(id, false);
            if (album == null)
            {
                return NotFound();
            }

            _mapper.Map(model, album);
            await _service.UpdateAlbum(album);

            return Ok(_mapper.Map<Album>(await _service.GetAlbumById(id, false)));
        }
    }
}