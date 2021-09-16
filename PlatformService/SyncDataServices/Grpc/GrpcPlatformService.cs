using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc {
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo repo, IMapper mapper) {
            _repo = repo;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context) {
            var response = new PlatformResponse();

            var platforms = _repo.GetAllPlatforms();

            response.Platform.AddRange(_mapper.Map<IEnumerable<GrpcPlatformModel>>(platforms));

            return Task.FromResult(response);
        }
    }
}