using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprotify.Web.Services;
using Sprotify.Web.Areas.Administration.Models;
using Sprotify.Web.Services.Core;

namespace Sprotify.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class BandController : Controller
    {
        private readonly BandService _service;

        public BandController(BandService service)
        {
            _service = service;
        }

        [Route("[area]/bands")]
        public async Task<IActionResult> Index()
        {
            var bands = await _service.GetBands();
            return View(bands);
        }

        [Route("[area]/band/create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Band/Create
        [HttpPost]
        [Route("[area]/band/create")]
        public async Task<IActionResult> Create(EditBand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.CreateBand(model.Name);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (BadRequestException e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }

        // GET: Band/Edit/5
        [Route("[area]/band/edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var band = await _service.GetBandById(id);
            if (band == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new EditBand { Name = band.Name });
        }

        // POST: Band/Edit/5
        [HttpPost]
        [Route("[area]/band/edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, EditBand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.UpdateBand(id, model.Name);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (BadRequestException e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }
    }
}