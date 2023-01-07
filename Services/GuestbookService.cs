using Gateway.Models;
using Google.Protobuf.WellKnownTypes;
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
        //  查詢所有要的資料
        public static async Task<GetGuestbookProtobufResponse> GetGuestbook(GetGuestbookProtobufRequest getGuestbookProtobufRequest)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GuestbookProtoBuf.GuestbookProtoBufClient(channel);
            var response = await client.GetGuestbookProtobufAsync(getGuestbookProtobufRequest);
            //var response = await client.CreateGuestbookProtobufAsync(createGuestbookProtobufRequest);
            return response;
        }

        //  新增一個要做的事項
        public static async Task<CreateGuestbookProtobufResponse> CreateGuestbook(GuestbookModel guestbookModel)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GuestbookProtoBuf.GuestbookProtoBufClient(channel);

            CreateGuestbookProtobufRequest createGuestbookProtobufRequest = new CreateGuestbookProtobufRequest
            {
                Name = guestbookModel.Name,
                Title = guestbookModel.Title,
                Content = guestbookModel.Content,
                Status = guestbookModel.Status,
                Endtime = Timestamp.FromDateTimeOffset(guestbookModel.Endtime)
            };
            var response = await client.CreateGuestbookProtobufAsync(createGuestbookProtobufRequest);
            //var response = await client.CreateGuestbookProtobufAsync(createGuestbookProtobufRequest);
            return response;
        }

        //依據Id來做更新
        public static async Task<UpdateGuestbookProtobufResponse> UpdateGuestbook(GuestbookModel guestbookModel)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GuestbookProtoBuf.GuestbookProtoBufClient(channel);
            //  將類別的值Bind進PB Request
            UpdateGuestbookProtobufRequest updateGuestbookProtobufRequest = new UpdateGuestbookProtobufRequest()
            {
                Id = guestbookModel.Id,
                Name = guestbookModel.Name,
                Title = guestbookModel.Title,
                Content = guestbookModel.Content,
                Status = guestbookModel.Status,
                Endtime = Timestamp.FromDateTimeOffset(guestbookModel.Endtime)
            };
            var response = await client.UpdateGuestbookProtobufAsync(updateGuestbookProtobufRequest);
            return response;
        }

        public static async Task<DeleteGuestbookProtobufResponse> DeleteGuestbook(string Id) 
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GuestbookProtoBuf.GuestbookProtoBufClient(channel);
            var response = await client.DeleteGuestbookProtobufAsync(new DeleteGuestbookProtobufRequest() { Id = Id });
            return response;
        }

    }
}
