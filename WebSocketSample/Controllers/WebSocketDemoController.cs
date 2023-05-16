using Microsoft.AspNetCore.Mvc;
using WebSocketSample.Interface;

namespace WebSocketSample.Controllers
{
    public class WebSocketDemoController : Controller
    {
        private readonly IWebSocketService _iWebSocketService;

        public WebSocketDemoController(IWebSocketService iWebSocketService)
        {
            _iWebSocketService = iWebSocketService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("/ws")]
        //public async Task Get()
        //{
        //    if (HttpContext.WebSockets.IsWebSocketRequest)
        //    {
        //        using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        //        await _iWebSocketService.WebSocketProcess(webSocket);
        //    }
        //    else
        //    {
        //        HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        //    }
        //}
    }
}
