using Grpc.Net.Client;
using GuestbookClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class GuestbookService
    {
        //新增一個要做的事項
        public static async Task<CreateGuestbookProtobufResponse> CreateGuestbook(CreateGuestbookProtobufRequest createGuestbookProtobufRequest)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GuestbookProtoBuf.GuestbookProtoBufClient(channel);
            var response = await client.CreateGuestbookProtobufAsync(createGuestbookProtobufRequest);
            //var response = await client.CreateGuestbookProtobufAsync(createGuestbookProtobufRequest);
            return response;
        }

        public static async Task<GetGuestbookProtobufResponse> GetGuestbook(GetGuestbookProtobufRequest getGuestbookProtobufRequest)
        { 
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GuestbookProtoBuf.GuestbookProtoBufClient(channel);
            var response = await client.GetGuestbookProtobufAsync(getGuestbookProtobufRequest);
            //var response = await client.CreateGuestbookProtobufAsync(createGuestbookProtobufRequest);
            return response;
        }
    }
}
