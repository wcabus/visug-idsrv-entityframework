using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sprotify.Web.Models;
using Sprotify.Web.Services;
using System;

namespace Sprotify.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly SubscriptionService _service;
        private readonly UserService _userService;
        private readonly SongService _songService;

        public HomeController(SubscriptionService service, UserService userService, SongService songService)
        {
            _service = service;
            _userService = userService;
            _songService = songService;
        }

        public async Task<IActionResult> Index()
        {
            var subscriptions = await _service.GetSubscriptions();
            return View(subscriptions.OrderBy(x => x.PricePerMonth));
        }

        [Route("/subscribe/{subscription:guid}")]
        public async Task<IActionResult> Signup([FromRoute]Guid subscription)
        {
            var isSubscribed = await _userService.IsSubscribed(User.GetSubject());
            if (!isSubscribed)
            {
                await _userService.Subscribe(User.GetSubject(), subscription);
            }

            return RedirectToAction("About");
        }

        public async Task<IActionResult> About()
        {
            var songs = await _songService.GetAllSongs();
            return View(songs.OrderBy(x => x.Band).ThenBy(x => x.Album).ThenBy(x => x.Position));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
