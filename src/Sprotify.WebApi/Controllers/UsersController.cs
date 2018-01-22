using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprotify.Domain.Services;
using Sprotify.WebApi.Models.Users;

namespace Sprotify.WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id:guid}", Name = Routes.GetUserById)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<User>(user));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]UserToRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.RegisterUser(model.Id, model.Name);
            return CreatedAtRoute(Routes.GetUserById, new { id = user.Id}, _mapper.Map<User>(user));
        }

        [HttpPost("{id:guid}/subscriptions")]
        public async Task<IActionResult> Subscribe(Guid id, [FromBody]SubscribeUser model)
        {
            var userId = Guid.Parse(User.FindFirst("sub").Value);
            if (userId != id)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var subscription = await _userService.GetSubscriptionById(model.SubscriptionId);
            if (subscription == null)
            {
                return BadRequest("Invalid subscription");
            }

            var userSubscription = await _userService.Subscribe(user, subscription);
            return Ok(_mapper.Map<UserSubscription>(userSubscription));
        }

        public static class Routes
        {
            public const string GetUserById = nameof(GetUserById);
        }
    }
}