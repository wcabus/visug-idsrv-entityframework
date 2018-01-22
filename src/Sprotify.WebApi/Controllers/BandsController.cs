using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sprotify.Domain.Services;
using AutoMapper;
using Sprotify.WebApi.Models.Bands;
using Microsoft.AspNetCore.Authorization;

namespace Sprotify.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class BandsController : Controller
    {
        private readonly IBandService _service;
        private readonly IMapper _mapper;

        public BandsController(IBandService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery]string filter = null)
        {
            return Ok(_mapper.Map<IEnumerable<Band>>(await _service.GetBands(filter)));
        }

        [HttpGet("{id:guid}", Name = Routes.GetBandById)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var band = await _service.GetBandById(id);
            if (band == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Band>(band));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBand([FromBody]BandToCreate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var band = await _service.CreateBand(model.Name);
            return CreatedAtRoute(Routes.GetBandById, new { id = band.Id }, _mapper.Map<Band>(band));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBand(Guid id, [FromBody]BandToUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var band = await _service.GetBandById(id);
            if (band == null)
            {
                return NotFound();
            }

            _mapper.Map(model, band);
            await _service.UpdateBand(band);

            return Ok(_mapper.Map<Band>(band));
        }

        private static class Routes
        {
            public const string GetBandById = nameof(GetBandById);
        }
    }
}