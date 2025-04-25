using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WhatsApp.Models;
using WhatsApp.Services;
using Microsoft.AspNetCore.Identity;
using WhatsApp.Extensions;
using Microsoft.AspNetCore.SignalR;
using WhatsApp.Hubs;

namespace WhatsApp.Controllers
{
    public class HomeController(ILogger<HomeController> _logger, ChatRegistry _chatRegistry, RoomManager _manager) : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
      
        [HttpGet("/auth")]
        public IActionResult Authenticate(string username)
        {
            var claims = new Claim[]
            {
                new("user_id", Guid.NewGuid().ToString()),
                new("username", username),
            };

            var identity = new ClaimsIdentity(claims, "Cookie");
            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync("Cookie", principal);
            return Ok();
        }

        [HttpGet("/create")]
        public IActionResult CreateRoom(string room)
        {
            _chatRegistry.CreateRoom(room);
            return Ok();
        }

        [HttpGet("/list")]
        public IActionResult ListRooms()
        {
            return Ok(_chatRegistry.GetRooms());
        }
        private readonly RoomManager _manager;


        [HttpGet("my")]
        public IActionResult MyRoom()
        {
            var userId = HttpContext.UserId();

            var myRoom = _manager.GetRoomByUserId(userId);

            if (myRoom == null)
                return NoContent();

            return Ok(new RoomView(myRoom, userId));
        }

        public IActionResult List()
        {
            return Ok(_manager.Rooms.Select(x => new RoomView(x, HttpContext.UserId())));
        }
      

    }
}
