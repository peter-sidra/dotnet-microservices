using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc {
    public class PlatformDataClient : IPlatformDataClient {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly GrpcChannel _channel;
        private readonly GrpcPlatform.GrpcPlatformClient _client;

        public PlatformDataClient(IConfiguration config, IMapper mapper) {
            _config = config;
            _mapper = mapper;

            var GrpcPlatform = _config["GrpcPlatform"];
            _channel = GrpcChannel.ForAddress(GrpcPlatform);
            _client = new GrpcPlatform.GrpcPlatformClient(_channel);
        }

        public IEnumerable<Platform> ReturnAllPlatforms() {
            Console.WriteLine($"--> Calling gRPC service {_config["GrpcPlatform"]}");

            var request = new GetAllRequest();

            try {
                var reply = _client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (System.Exception ex) {
                Console.WriteLine($"--> Could not call gRPC server {ex.Message}");
                return null;
            }
        }
    }
}