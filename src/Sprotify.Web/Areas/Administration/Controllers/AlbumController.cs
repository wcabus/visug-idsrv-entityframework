using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprotify.Web.Services;
using Sprotify.Web.Areas.Administration.Models.Albums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sprotify.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class AlbumController : Controller
    {
        private readonly AlbumService _service;

        public AlbumController(AlbumService service)
        {
            _service = service;
        }

        [Route("[area]/band/{bandId:guid}/albums")]
        public async Task<IActionResult> Index(Guid bandId)
        {
            var albums = await _service.GetAlbumsForBand(bandId);
            if (albums == null)
            {
                return RedirectToAction("Index", "Band");
            }

            ViewBag.Band = albums.FirstOrDefault()?.Band;
            if (ViewBag.Band == null)
            {
                ViewBag.Band = await _service.GetBandName(bandId);
            }

            ViewBag.BandId = bandId;
            return View(albums);
        }

        [Route("[area]/band/{bandId:guid}/album/{albumId:guid}")]
        public async Task<IActionResult> Details(Guid bandId, Guid albumId)
        {
            var album = await _service.GetAlbumWithSongs(bandId, albumId);
            if (album == null)
            {
                return RedirectToAction(nameof(Index), new { bandId });
            }

            ViewBag.BandId = bandId;
            return View(album);
        }

        [Route("[area]/band/{bandId:guid}/album/create")]
        public IActionResult Create(Guid bandId)
        {
            ViewBag.BandId = bandId;
            return View();
        }

        [HttpPost]
        [Route("[area]/band/{bandId:guid}/album/create")]
        public async Task<IActionResult> Create(Guid bandId, AlbumToCreate album)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.CreateAlbum(bandId, album.Title, album.ReleaseDate, album.Art);
                    return RedirectToAction(nameof(Index), new { bandId });
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            ViewBag.BandId = bandId;
            return View(album);
        }

        [Route("[area]/band/{bandId:guid}/album/edit/{albumId:guid}")]
        public async Task<IActionResult> Edit(Guid bandId, Guid albumId)
        {
            var album = await _service.GetAlbum(bandId, albumId);
            if (album == null)
            {
                return RedirectToAction(nameof(Index), new { bandId });
            }

            ViewBag.BandId = bandId;
            return View(new AlbumToEdit(album));
        }

        [HttpPost]
        [Route("[area]/band/{bandId:guid}/album/edit/{albumId:guid}")]
        public async Task<IActionResult> Edit(Guid bandId, Guid albumId, AlbumToEdit album)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.UpdateAlbum(bandId, albumId, album.Title, album.ReleaseDate, album.Art);
                    return RedirectToAction(nameof(Index), new { bandId });
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            ViewBag.BandId = bandId;
            return View(album);
        }
    }
}
