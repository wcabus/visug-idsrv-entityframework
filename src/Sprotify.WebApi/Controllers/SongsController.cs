using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Sprotify.Domain.Services;
using Sprotify.WebApi.Models.Songs;
using Sprotify.WebApi.Models;

namespace Sprotify.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [Authorize]
    public class SongsController : Controller
    {
        private readonly ISongService _songService;
        private readonly IMapper _mapper;

        public SongsController(ISongService songService, IMapper mapper)
        {
            _songService = songService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var songs = await _songService.GetSongs();
            return Ok(_mapper.Map<IEnumerable<Song>>(songs));
        }

        [HttpGet("/songs/search")]
        public async Task<IActionResult> Get(string filter)
        {
            var songs = await _songService.GetSongs(filter);
            return Ok(_mapper.Map<IEnumerable<SearchItem>>(songs));
        }
    }
}