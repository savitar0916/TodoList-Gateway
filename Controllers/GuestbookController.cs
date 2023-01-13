using Gateway.Models;
using Gateway.Services;
using Google.Protobuf.WellKnownTypes;
using GuestbookClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;
        public GuestbookController(IConfiguration config)
        {
            Task.Delay(5000);
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> GetGuestbook(string query)
        {
            GetGuestbookProtobufRequest getGuestbookProtobufRequest = new GetGuestbookProtobufRequest
            {
                Query = query
            };
            var response = await GuestbookService.GetGuestbook(getGuestbookProtobufRequest,_config);
            return Content(JsonConvert.SerializeObject(response), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> CreateGuestbook(GuestbookModel guestbookModel)
        {

            //var response = client.CreateGuestbook(createGuestbookRequest);
            var response = await GuestbookService.CreateGuestbook(guestbookModel, _config);
            return Content(JsonConvert.SerializeObject(response), "application/json");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGuestbook(GuestbookModel guestbookModel) 
        {
            var response = await GuestbookService.UpdateGuestbook(guestbookModel, _config);
            return Content(JsonConvert.SerializeObject(response), "application/json");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGuestbook(string Id)
        {
            var response = await GuestbookService.DeleteGuestbook(Id, _config);
            return Content(JsonConvert.SerializeObject(response), "application/json");
        }

        [HttpPost]
        [Route("SCreateGuestbooks")]
        public async Task<IActionResult> SCreateGuestbooks(GuestbookModel guestbookModel)
        {
            var response = await GuestbookService.SCreateGuestbooks(guestbookModel, _config);
            return Content(JsonConvert.SerializeObject(response), "application/json");
        }
    }
}
