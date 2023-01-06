using Gateway.Models;
using Gateway.Services;
using Google.Protobuf.WellKnownTypes;
using GuestbookClient;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestbookController : ControllerBase
    {
        public GuestbookController()
        {
            Task.Delay(5000);
        }
        [HttpGet]
        public async Task<IActionResult> GetGuestbook(string query)
        {
            GetGuestbookProtobufRequest getGuestbookProtobufRequest = new GetGuestbookProtobufRequest
            {
                Query = query
            };
            var response = await GuestbookService.GetGuestbook(getGuestbookProtobufRequest);
            return Content(JsonConvert.SerializeObject(response), "application/json");
        }
        [HttpPost]
        public async Task<IActionResult> CreateGuestbook(GuestbookModel guestbookModel)
        {
            CreateGuestbookProtobufRequest createGuestbookProtobufRequest = new CreateGuestbookProtobufRequest
            {
                Name = guestbookModel.Name,
                Title = guestbookModel.Title,
                Content = guestbookModel.Content,
                Status = guestbookModel.Status,
                Endtime = Timestamp.FromDateTimeOffset(guestbookModel.Endtime)
            };
            //var response = client.CreateGuestbook(createGuestbookRequest);
            var response = await GuestbookService.CreateGuestbook(createGuestbookProtobufRequest);
            return Content(JsonConvert.SerializeObject(response), "application/json"); ;
        }
    }
}
