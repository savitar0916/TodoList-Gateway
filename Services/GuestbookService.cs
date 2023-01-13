using Gateway.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GuestbookClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class GuestbookService
    {

        //  查詢所有要的資料
        public static async Task<List<GuestbookModel>> GetGuestbook(GetGuestbookProtobufRequest getGuestbookProtobufRequest, IConfiguration config)
        {
            var GuestbookGrpcServerConnectionString = config["GuestbookGrpcServer:Address"] + config["GuestbookGrpcServer:Port"];
            var channel = GrpcChannel.ForAddress(GuestbookGrpcServerConnectionString);
            var client = new GuestbookProtoBuf.GuestbookProtoBufClient(channel);
            var response = await client.GetGuestbookProtobufAsync(getGuestbookProtobufRequest);
            //var response = await client.CreateGuestbookProtobufAsync(createGuestbookProtobufRequest);
            List<GuestbookModel> guestbookModels = response.Guestbooks.Select(g => new GuestbookModel 
                {
                    Id = g.Id,
                    Name = g.Name,
                    Title = g.Title,
                    Content = g.Content,
                    Status = g.Status,
                    Endtime = g.Endtime.ToDateTimeOffset(),
                }).ToList();
            return guestbookModels;
        }

        //  新增一個要做的事項
        public static async Task<dynamic> CreateGuestbook(GuestbookModel guestbookModel, IConfiguration config)
        {
            //從appsetting.json撈出設定
            var GuestbookGrpcServerConnectionString = config["GuestbookGrpcServer:Address"] + config["GuestbookGrpcServer:Port"];
            var channel = GrpcChannel.ForAddress(GuestbookGrpcServerConnectionString);
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
            var httpResponse = new { 
                Message = response.Message,
                Status = response.Status,
                CreateAt = response.CreateAt.ToDateTimeOffset()
            };
            return httpResponse;
        }

        //  依據Id來做更新
        public static async Task<UpdateGuestbookProtobufResponse> UpdateGuestbook(GuestbookModel guestbookModel, IConfiguration config)
        {
            var GuestbookGrpcServerConnectionString = config["GuestbookGrpcServer:Address"] + config["GuestbookGrpcServer:Port"];
            var channel = GrpcChannel.ForAddress(GuestbookGrpcServerConnectionString);
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
        
        //  依據Id來做刪除
        public static async Task<DeleteGuestbookProtobufResponse> DeleteGuestbook(string Id, IConfiguration config) 
        {
            var GuestbookGrpcServerConnectionString = config["GuestbookGrpcServer:Address"] + config["GuestbookGrpcServer:Port"];
            var channel = GrpcChannel.ForAddress(GuestbookGrpcServerConnectionString);
            var client = new GuestbookProtoBuf.GuestbookProtoBufClient(channel);
            var response = await client.DeleteGuestbookProtobufAsync(new DeleteGuestbookProtobufRequest() { Id = Id });
            return response;
        }

        public static async Task<dynamic> SCreateGuestbooks(GuestbookModel guestbookModel , IConfiguration config)
        {
            var response = new CreateGuestbookProtobufResponse();
            try
            {
                var GuestbookGrpcServerConnectionString = config["GuestbookGrpcServer:Address"] + config["GuestbookGrpcServer:Port"];
                var channel = GrpcChannel.ForAddress(GuestbookGrpcServerConnectionString);
                var client = new GuestbookProtoBuf.GuestbookProtoBufClient(channel);
                var requestStream = client.SCreateGuestbooks();
                await requestStream.RequestStream.WriteAsync(new CreateGuestbookProtobufRequest
                {
                    Name = guestbookModel.Name,
                    Title = guestbookModel.Title,
                    Content = guestbookModel.Content,
                    Status = guestbookModel.Status,
                    Endtime = Timestamp.FromDateTimeOffset(guestbookModel.Endtime)
                });
                
                while (await requestStream.ResponseStream.MoveNext())
                {
                    response = requestStream.ResponseStream.Current;
                    return response;
                }
            }
            catch (SocketException socketException)
            {
                response.Message = "Create Guestbook Error.Please Try Again";
                response.Status = 500;
                Console.WriteLine("There is a Proble : " + socketException.Message);
            }
            catch (HttpRequestException httpRequestException)
            {
                response.Message = "Create Guestbook Error.Please Try Again";
                response.Status = 500;
                Console.WriteLine("There is a Proble : " + httpRequestException.Message);
            }
            return response;
        }
    }
}
